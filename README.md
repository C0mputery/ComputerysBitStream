# Okay, this project currently has no real documentation, but here are some important notes and quick rundown on how to use it until I actually write xml docs and setup docfx:

ComputerysBitStream is a C# library for reading and writing values at bit granularity. `WriteContext` and `ReadContext` take a `Span<ulong>` / `ReadOnlySpan<ulong>` over the backing memory, track the current bit position, and expose extension methods for primitives and user-defined structs.

## Solution layout

| Project | Role |
|---|---|
| `ComputerysBitStream` | Runtime library: contexts, built-in primitive serializers, attributes |
| `ComputerysBitStream.Generator` | Roslyn source generator; emits read/write extensions at compile time |
| `ComputerysBitStream.Extras` | Optional serializers for `System.Numerics` types and quantized variants |
| `ComputerysBitStream.Tests` | xUnit test suite |

Target frameworks: `netstandard2.1` and `net10.0` for the library projects; tests run on `net10.0`.

## Project reference

Reference the main project and wire the generator as an analyzer (same pattern as the test project):

```xml
<ProjectReference Include="path\to\ComputerysBitStream\ComputerysBitStream.csproj" />
<ProjectReference Include="path\to\ComputerysBitStream.Generator\ComputerysBitStream.Generator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
```

Add `ComputerysBitStream.Extras` only if you need `System.Numerics` types (`Vector2`, `Vector3`, `Vector4`, `Quaternion`, `Plane`, `Matrix4x4`) or their quantized variants. The extras proxy sources are a reasonable template for adding Unity types later.

## Buffer and contexts

Contexts only need a `Span<ulong>` (write) or `ReadOnlySpan<ulong>` (read). The underlying memory can be a `ulong[]`, a `byte[]`, a pinned struct, native memory, or anything else you can view as ulongs (typically via `MemoryMarshal.Cast<T, ulong>`). Position and capacity are measured in **bits**, not bytes. You can start at a non-zero bit offset when packing multiple values into one stream.

```csharp
Span<ulong> storage = stackalloc ulong[16];
var write = new WriteContext(storage);
write.WriteInt(42);
write.WriteBool(true);

var read = new ReadContext(storage);
int i = read.ReadInt();
bool b = read.ReadBool();

// Or back the stream with any castable memory (needs System.Runtime.InteropServices):
Span<byte> bytes = stackalloc byte[128];
var write2 = new WriteContext(MemoryMarshal.Cast<byte, ulong>(bytes));
```

`Peek*` methods read without advancing position. `Try*` and `TryPeek*` variants return `false` when the buffer does not hold enough data.

Array helpers come in two forms: with a length prefix (`WriteInts`, `ReadInts`) and without (`WriteIntsWithoutLength`, `ReadInts(count)`). The same pattern applies to structs.

## Built-in primitives

`IDefaultSettings` registers fixed-size serializers for `bool`, `byte`, `char`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`, and `DateTime`, and emits `WriteInt`, `ReadBool`, `WriteDateTime`, and the rest of that API.

Variable-length serializers live in `ComputerysBitStream.Primitives.VariableLength` (`WriteVariableLengthUInt`, `WriteString`, etc) for `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, and `string`. Quantized serializers live in `ComputerysBitStream.Primitives.Quantized` (`WriteQuantizedFloat`, etc.) for `float`, `double`, and `decimal`; they take `min`, `max`, and `bitCount` to store a value in fewer bits than the fixed-size form. All of these are registered on `IDefaultSettings` alongside the fixed-size primitives.

A settings interface can register fixed-size, variable-length, and quantized serializers for the same CLR type. Members pick the mode via attribute: none for fixed-size, `[BitStreamStructVariableLength]`, or `[BitStreamStructQuantized(...)]`.

## Defining structs

Mark a `partial struct` with `[BitStreamStruct]`. The generator emits `Write{StructName}` and `Read{StructName}` as extension methods on the contexts. Extension methods can only access public members.

Member inclusion matches `System.Text.Json` on public surface area: public properties serialize by default, fields stay out unless opted in, and `[BitStreamStructIgnore]` / `[BitStreamStructInclude]` mirror `[JsonIgnore]` / `[JsonInclude]`. Unlike STJ, `[BitStreamStructInclude]` cannot pull in `private`, `internal`, or `protected` members; the build reports `CBS041`.

Member rules for `[BitStreamStruct]`:
- Public properties with a public getter and setter are serialized by default.
- `[BitStreamStructIgnore]` excludes a property.
- Public fields are skipped unless you mark them with `[BitStreamStructInclude]`.
- `private`, `internal`, and `protected` members are not serialized.

When Unity moves to CoreCLR with .NET 8+ support, I plan to look at `[UnsafeAccessorAttribute]` as a way to read and write non-public members from generated code without leaving the extension-method model.

```csharp
using ComputerysBitStream;
using ComputerysBitStream.Attributes;

[BitStreamStruct]
public partial struct PlayerState {
    public int Health { get; set; }
    public float Speed { get; set; }
    public string Name { get; set; }
    [BitStreamStructIgnore] public int DebugOnly { get; set; }
    [BitStreamStructInclude] public int Flags;
}

write.WritePlayerState(state);
var copy = read.ReadPlayerState();
```

Put `[BitStreamSerializer(typeof(...))]` on a field or property to pick which primitive extension class serializes that member. The generator checks this attribute before quantized, variable-length, or default resolution. The extension class must already be registered in effective settings (`IDefaultSettings`, `[DefaultBitStreamSettings]`, or a `[BitStreamSettings]` interface passed to `[BitStreamStruct(typeof(...))]`).

This works for primitive-typed members only (`int`, `float`, `bool`, `string`, and so on). It does not register or override serialization for nested `[BitStreamStruct]` types or proxy-serialized external structs; those require settings registration (see Nested structs).

```csharp
[BitStreamStruct]
public partial struct SimpleStruct {
    public int X { get; set; }

    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    public float Y { get; set; }
}
```

For members without a per-member `[BitStreamSerializer]` attribute:

- `[BitStreamStructQuantized(...)]` uses the quantized serializer (`CBS038`; `CBS045` with `[BitStreamStructVariableLength]`)
- `[BitStreamStructVariableLength]` uses the variable-length serializer (`CBS042`)
- otherwise the generator looks for a fixed-size serializer; if none is registered but a variable-length serializer is (`string`, or a custom primitive with only that mode), it uses variable-length without an attribute
- if neither fixed-size nor variable-length resolves, `CBS043` (or `CBS044` when only a quantized serializer is registered)


## Nested structs

`[BitStreamStruct]` generates `Write*` / `Read*` for that type. It does not register the type as a serializable member of other structs. Primitives resolve from effective settings: assembly global settings (`[DefaultBitStreamSettings]`, or `IDefaultSettings` when the assembly declares none), plus any settings on `[BitStreamStruct(typeof(...))]`. Nested `[BitStreamStruct]` types are never added automatically.

Add `[BitStreamSerializer(typeof(NestedStruct))]` to a `[BitStreamSettings]` interface, using the struct type rather than the generated extension class. That line can live on the interface passed to the parent's `[BitStreamStruct(typeof(...))]`, on a base interface in that settings chain, or in assembly-wide `[DefaultBitStreamSettings(...)]`. The generator merges global settings, inherited interfaces, and per-struct settings into one effective set.

An unregistered nested struct produces `CBS043`. `CBS036` is reported when the nested type is registered on a settings interface but its own resolution fails, for example because a member inside the nested struct cannot be serialized. If any member fails to resolve, the parent struct is not generated. Cyclic nesting (for example, A holds B and B holds A) produces error `CBS035`.

Each nesting level needs its own registration. If `Container` holds `Inner` and `Inner` holds `Core`, settings for `Container` must list `Inner`, and settings for `Inner` must list `Core`.

```csharp
[BitStreamStruct]
public partial struct NestedStruct {
    public int Value { get; set; }
}

[BitStreamSettings]
[BitStreamSerializer(typeof(NestedStruct))]
public interface IContainerSettings : IDefaultSettings { }

[BitStreamStruct(typeof(IContainerSettings))]
public partial struct ContainerStruct {
    public int RawValue { get; set; }
    public NestedStruct Nested { get; set; }
}
```

Proxy-serialized external structs follow the same rule: register the proxy class, e.g. `[BitStreamSerializer(typeof(ExternalPlainStructProxy))]`. `[BitStreamSerializer(typeof(NestedStruct))]` on a member field does not replace settings registration.

## Settings interfaces

Serializers are grouped behind `[BitStreamSettings]` interfaces. Primitives use the extension class (`typeof(PrimitiveIntExtensions)`). Structs and proxy classes use the struct or proxy type (`typeof(MyStruct)`, `typeof(MyStructProxy)`). Pass the interface to `[BitStreamStruct(typeof(IMySettings))]` or register it assembly-wide:

```csharp
[assembly: DefaultBitStreamSettings(typeof(IMySettings))]

[BitStreamSettings]
[BitStreamSerializer(typeof(ExternalPlainStructProxy))]
public interface IMySettings : IDefaultSettings { }
```

`IDefaultSettings` already wires fixed-size, variable-length, and quantized primitives. Inherit from it when those defaults are what you want, and mark members with `[BitStreamStructVariableLength]` or `[BitStreamStructQuantized(...)]` when you need a non-default mode:

```csharp
[BitStreamSettings]
public interface IMixedIntStructSettings : IDefaultSettings { }

[BitStreamStruct(typeof(IMixedIntStructSettings))]
public partial struct MixedIntStruct {
    public int FixedValue { get; set; }

    [BitStreamStructVariableLength]
    public int VariableValue { get; set; }
}
```

A struct with any variable-length member is itself variable-length (metadata size is negative).

## Serializing types you do not own

For structs defined outside your project, add a static proxy class. Each public static field or property you declare on the proxy is serialized. You do not need `[BitStreamStructInclude]` on those proxy members.

When you cannot edit the target type, declare every serialized member on the proxy. Omitted members are not serialized.

When you can edit the target struct in your assembly, you can mark a target field with `[BitStreamStructInclude]` even if the proxy defines no mirror for it (`CaseTestStruct` in `ComputerysBitStream.Tests/Structs/TestStructs.cs`). Proxies cannot read or write non-public members of the target type.

```csharp
[BitStreamProxyStruct(typeof(ExternalPlainStruct))]
public static partial class ExternalPlainStructProxy {
    public static int X;
    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    public static float Y;
}
```

The generator emits `WriteExternalPlainStruct` / `ReadExternalPlainStruct`, or a custom alias if you pass one to the attribute.

## Extras package

`ComputerysBitStream.Extras` ships proxy serializers for `Vector2`, `Vector3`, `Vector4`, `Quaternion`, `Plane`, and `Matrix4x4`, plus quantized serializers for those types. Its `IGameExtrasSettings` interface extends `IDefaultSettings`; reference the Extras assembly and include that interface in your settings chain when you need those types.

## Build and test
Pass compile-time flags through the MSBuild property `BitStreamDefineConstants`:
- `BITSTREAM_HOST_BIG_ENDIAN`: big-endian bit layout
- `BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE`: unsafe buffer paths; on `netstandard2.1` this adds a reference to `System.Runtime.CompilerServices.Unsafe`
- `BITSTREAM_SUPPORT_THREAD_SAFE`: thread-safe `WriteContext` overloads

Example: `-p:BitStreamDefineConstants=BITSTREAM_HOST_BIG_ENDIAN`

# License
APACHE 2.0<br>
like actually please give me attribution :pleading_face:<br>
I'm also super interested if you have any projects that use this, reach out to me (`Computery` on discord)<br>
also open for work <br>
AlSO if you've read this far give me a star
