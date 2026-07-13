namespace ComputerysBitStream.Tests.Structs;

[BitStreamStruct]
public partial struct SimpleStruct {
    public int X { get; set; }

    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    public float Y { get; set; }

    public bool Z { get; set; }
}

public struct ExternalPlainStruct {
    public int X { get; set; }
    public float Y { get; set; }
}

[BitStreamProxyStruct(typeof(ExternalPlainStruct))]
public static partial class ExternalPlainStructProxy {
    public static int X;

    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    public static float Y;
}

[BitStreamStruct]
public partial struct NestedStruct {
    public int Value { get; set; }
}

[BitStreamSettings]
[BitStreamSerializer(typeof(PrimitiveIntExtensions))]
public interface ICustomSettings { }

[BitStreamStruct(typeof(ICustomSettings))]
public partial struct CustomSettingsStruct {
    public int B { get; set; }
}

[BitStreamStruct]
public partial struct LocalWithExternal {
    public int LocalValue { get; set; }
}

public struct AnotherExternalStruct {
    public bool Flag { get; set; }
}

[BitStreamProxyStruct(typeof(AnotherExternalStruct))]
public static partial class AnotherExternalStructProxy {
    public static bool Flag;
}

[BitStreamSettings]
[BitStreamSerializer(typeof(PrimitiveIntExtensions))]
[BitStreamSerializer(typeof(NestedStruct))]
public interface IContainerSettings { }

[BitStreamStruct(typeof(IContainerSettings))]
public partial struct ContainerStruct {
    public int RawValue { get; set; }
    public NestedStruct Nested { get; set; }
}

public struct CaseTestStruct {
    [BitStreamStructInclude] public int Value;
}

[BitStreamProxyStruct(typeof(CaseTestStruct))]
public static partial class CaseTestStructProxyCorrect { }

[BitStreamStruct("Aliased")]
public partial struct AliasedStruct {
    public int A { get; set; }

    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    public float B { get; set; }
}

public struct AliasedExternalStruct {
    public int X { get; set; }
    public bool Y { get; set; }
}

[BitStreamProxyStruct(typeof(AliasedExternalStruct), "AliasedExt")]
public static partial class AliasedExternalStructProxy {
    public static int X;
    public static bool Y;
}

public struct AliasedIncludeExternalStruct {
    public int Included { get; set; }
    public int Ignored { get; set; }
}

[BitStreamProxyStruct(typeof(AliasedIncludeExternalStruct), "AliasedInc")]
public static partial class AliasedIncludeExternalStructProxy {
    public static int Included;
}

[BitStreamSettings]
public interface IVariableLengthStructSettings : IDefaultSettings { }

[BitStreamStruct(typeof(IVariableLengthStructSettings))]
public partial struct VariableLengthStruct {
    [BitStreamStructVariableLength] public int A { get; set; }

    public bool B { get; set; }
}

[BitStreamSettings]
public interface IMixedIntStructSettings : IDefaultSettings { }

[BitStreamStruct(typeof(IMixedIntStructSettings))]
public partial struct MixedIntStruct {
    public int FixedValue { get; set; }

    [BitStreamStructVariableLength] public int VariableValue { get; set; }
}

[BitStreamSettings]
public interface IMemberSerializerOverrideSettings : IDefaultSettings { }

[BitStreamStruct(typeof(IMemberSerializerOverrideSettings))]
public partial struct MemberSerializerOverrideStruct {
    [BitStreamStructVariableLength] public int VariableLengthValue { get; set; }

    [BitStreamStructVariableLength]
    [BitStreamSerializer(typeof(PrimitiveIntExtensions))]
    public int FixedOverrideValue { get; set; }
}

[BitStreamStruct]
public partial struct StringStruct {
    public int Id { get; set; }
    public string Name { get; set; }
}

[BitStreamStruct]
public partial struct QuantizedStruct {
    public const float Min = 0f;
    public const float Max = 100f;

    [BitStreamStructQuantized(nameof(Min), nameof(Max), 8)]
    public float Value { get; set; }
}

[BitStreamStruct]
public partial struct IntArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(16)]
    public int[] Values { get; set; }
}

[BitStreamStruct]
public partial struct RectangularArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(8, 8)]
    public int[,] Values { get; set; }
}

[BitStreamStruct]
public partial struct JaggedArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(8, 8)]
    public int[][] Values { get; set; }
}

[BitStreamStruct]
public partial struct MixedArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(4, 4, 4)]
    public int[][,] Values { get; set; }
}

[BitStreamStruct]
public partial struct ThreeDimensionalArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(4, 4, 4)]
    public int[,,] Values { get; set; }
}

[BitStreamStruct]
public partial struct DeepJaggedArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(4, 4, 4)]
    public int[][][] Values { get; set; }
}

[BitStreamStruct]
public partial struct StringArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(8)]
    public string[] Values { get; set; }
}

[BitStreamStruct]
public partial struct QuantizedArrayMemberStruct {
    public const float Min = 0f;
    public const float Max = 100f;

    [BitStreamStructCollectionMaxEntries(8)]
    [BitStreamStructQuantized(nameof(Min), nameof(Max), 8)]
    public float[] Values { get; set; }
}

[BitStreamSettings]
[BitStreamSerializer(typeof(NestedStruct))]
[BitStreamSerializer(typeof(ExternalPlainStructProxy))]
public interface IArrayMemberSettings : IDefaultSettings { }

[BitStreamStruct(typeof(IArrayMemberSettings))]
public partial struct NestedArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(8)]
    public NestedStruct[] Values { get; set; }
}

[BitStreamStruct(typeof(IArrayMemberSettings))]
public partial struct NestedRectangularArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(4, 4)]
    public NestedStruct[,] Values { get; set; }
}

[BitStreamStruct(typeof(IArrayMemberSettings))]
public partial struct NestedJaggedArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(4, 4)]
    public NestedStruct[][] Values { get; set; }
}

[BitStreamStruct(typeof(IArrayMemberSettings))]
public partial struct NestedMixedArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(2, 2, 2)]
    public NestedStruct[][,] Values { get; set; }
}

[BitStreamStruct(typeof(IArrayMemberSettings))]
public partial struct NestedThreeDimensionalArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(2, 2, 2)]
    public NestedStruct[,,] Values { get; set; }
}

[BitStreamStruct(typeof(IArrayMemberSettings))]
public partial struct ExternalArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(8)]
    public ExternalPlainStruct[] Values { get; set; }
}
