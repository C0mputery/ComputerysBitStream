using ComputerysBitStream;

namespace ComputerysBitStream.Tests;

[BitStreamStruct]
public partial struct SimpleStruct {
    public int X { get; set; }
    public float Y { get; set; }
    public bool Z { get; set; }
}

public struct ExternalPlainStruct {
    public int X { get; set; }
    public float Y { get; set; }
}

[BitStreamProxyStruct(typeof(ExternalPlainStruct))]
public static partial class ExternalPlainStructProxy { }

[BitStreamStruct]
public partial struct NestedStruct {
    public int Value { get; set; }
}

[BitStreamSettings]
[BitStreamSetting(typeof(RawIntExtensions))]
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
public static partial class AnotherExternalStructProxy { }

[BitStreamSettings]
[BitStreamSetting(typeof(RawIntExtensions))]
[BitStreamSetting(typeof(NestedStruct))]
public interface IContainerSettings { }

[BitStreamStruct(typeof(IContainerSettings))]
public partial struct ContainerStruct {
    public int RawValue { get; set; }
    public NestedStruct Nested { get; set; }
}

// Case-sensitivity test struct with a field so includes are effective
public struct CaseTestStruct {
    public int Value;
}

[BitStreamProxyStruct(typeof(CaseTestStruct), ["Value"], null)]
public static partial class CaseTestStructProxyCorrect { }

// Covers BitStreamStructAttribute(string alias, Type? settings = null)
[BitStreamStruct("Aliased")]
public partial struct AliasedStruct {
    public int A { get; set; }
    public float B { get; set; }
}

// Covers BitStreamProxyStructAttribute(Type targetType, string alias, Type? settings = null)
public struct AliasedExternalStruct {
    public int X { get; set; }
    public bool Y { get; set; }
}

[BitStreamProxyStruct(typeof(AliasedExternalStruct), "AliasedExt")]
public static partial class AliasedExternalStructProxy { }

// Covers BitStreamProxyStructAttribute(Type, string[]? includes, string[]? ignores, string alias, Type? settings)
public struct AliasedIncludeExternalStruct {
    public int Included { get; set; }
    public int Ignored { get; set; }
}

[BitStreamProxyStruct(typeof(AliasedIncludeExternalStruct), [nameof(AliasedIncludeExternalStruct.Included)], [nameof(AliasedIncludeExternalStruct.Ignored)], "AliasedInc")]
public static partial class AliasedIncludeExternalStructProxy { }


