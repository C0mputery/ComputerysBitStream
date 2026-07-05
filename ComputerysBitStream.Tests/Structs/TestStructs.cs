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

[BitStreamStruct]
public partial struct VariableLengthStruct {
    [BitStreamSerializer(typeof(PrimitiveVariableLengthIntExtensions))]
    public int A { get; set; }

    public bool B { get; set; }
}
