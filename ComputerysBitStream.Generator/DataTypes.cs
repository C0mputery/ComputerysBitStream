using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream;
using ComputerysBitStream.Generator;
using Microsoft.CodeAnalysis;

internal readonly record struct AllCollectedData(
    EquatableImmutableArray<SettingsData> GlobalSettings,
    EquatableImmutableArray<SettingsData> Settings,
    EquatableImmutableArray<RawData> RawTypes,
    EquatableImmutableArray<StructData> Structs,
    SettingsData FallbackGlobalSetting
);

internal record struct ParsedRawData(
    string TargetTypeFullyQualifiedName,
    string Alias,
    int Size,
    Dictionary<BitStreamRawRole, RawMethodData> Methods,
    ValueTypeLocation? Location
);

internal record struct ParsedSettingsData(
    string InterfaceFullyQualifiedName,
    Dictionary<string, ParsedRawData> IncludedRawTypes,
    Dictionary<string, StructData> IncludedLocalStructs,
    Dictionary<string, ExternalStructData> IncludedExternalStructs,
    ValueTypeLocation? Location
);

internal record struct RawData(
    string TargetTypeFullyQualifiedName,
    string Alias,
    int Size,
    EquatableImmutableArray<RawMethodData> Methods,
    ValueTypeLocation? Location,
    EquatableImmutableArray<DiagnosticData> Diagnostics = default
);

internal record struct RawMethodData(
    BitStreamRawRole Role,
    string MethodName,
    ValueTypeLocation? Location
);

internal record struct SettingsData(
    string InterfaceFullyQualifiedName,
    EquatableImmutableArray<RawData> RawTypes,
    EquatableImmutableArray<StructData> Structs,
    EquatableImmutableArray<ExternalStructData> ExternalStructs,
    ValueTypeLocation? Location,
    EquatableImmutableArray<DiagnosticData> Diagnostics = default
);


internal record struct StructMemberData(
    string MemberName,
    string TypeFullyQualifiedFormat,
    bool IsProperty,
    bool IsInitOnly,
    ValueTypeLocation? Location
);

internal record struct StructData(
    string TypeFullyQualifiedName,
    string Alias,
    EquatableImmutableArray<StructMemberData> Members,
    Accessibility Accessibility,
    string? SettingsInterfaceFullyQualifiedName,
    bool IsProxyClass,
    string DeclarationTypeFullyQualifiedName,
    ValueTypeLocation? Location,
    EquatableImmutableArray<DiagnosticData> Diagnostics = default
);

internal record struct ExternalStructData(
    string TypeFullyQualifiedName,
    string Alias,
    bool FixedSize,
    int Size,
    ValueTypeLocation? Location
);

internal record struct ResolvedStructMember(
    string MemberName,
    string TypeFullyQualifiedName,
    string Alias,
    int Size,
    bool IsFixedSize
);

internal record struct ParsedStructData(
    string TypeFullyQualifiedName,
    string Alias,
    Accessibility Accessibility,
    bool IsFixedSize,
    int FixedSize,
    ImmutableArray<ResolvedStructMember> Members
);