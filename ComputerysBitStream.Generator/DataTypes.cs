using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator;

internal readonly record struct AllCollectedData(
    EquatableImmutableArray<Collected<SettingsDefinition>> GlobalSettings,
    EquatableImmutableArray<Collected<SettingsDefinition>> Settings,
    EquatableImmutableArray<Collected<PrimitiveDefinition>> Primitives,
    EquatableImmutableArray<Collected<StructDefinition>> Structs,
    Collected<SettingsDefinition> FallbackGlobalSettings
);

internal record struct SettingsReference(
    EquatableImmutableArray<string> LocalSettingsInterfaceFullyQualifiedNames,
    SettingsDefinition? ExternalSettings,
    ValueTypeLocation? Location
);

internal record struct PrimitiveMethodDefinition(
    string MethodName,
    bool IsValid
);

internal record struct PrimitiveDefinition(
    string ExtensionClassFullyQualifiedName,
    string TargetTypeFullyQualifiedName,
    string? TargetTypeNamespace,
    string TargetTypeEmitName,
    string Alias,
    string? Namespace,
    PrimitiveSerializationMode Mode,
    int? FixedSize,
    int? MinBits,
    int? MaxBits,
    EquatableImmutableDictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> Methods,
    SettingsReference? Settings,
    ValueTypeLocation? Location
);

internal record SettingsDefinition(
    EquatableImmutableArray<string> InterfaceFullyQualifiedNames,
    EquatableImmutableDictionary<string, PrimitiveDefinition> Primitives,
    EquatableImmutableDictionary<string, StructDefinition> Structs,
    EquatableImmutableDictionary<string, ExternalStructDefinition> ExternalStructs,
    ValueTypeLocation? Location
);

internal record struct QuantizedRangeDefinition(
    string MinExpression,
    string MaxExpression,
    int BitCount,
    ValueTypeLocation? Location
);

internal record struct StructMemberDefinition(
    string MemberName,
    string TypeFullyQualifiedFormat,
    bool IsProperty,
    bool IsInitOnly,
    string? SerializerExtensionClassFullyQualifiedName,
    QuantizedRangeDefinition? QuantizedRange,
    ValueTypeLocation? Location
);

internal record struct StructDefinition(
    string TypeFullyQualifiedName,
    string Alias,
    string? Namespace,
    EquatableImmutableArray<StructMemberDefinition> Members,
    bool IsProxyClass,
    string DeclarationTypeFullyQualifiedName,
    string DeclarationTypeEmitName,
    SettingsReference? Settings,
    ValueTypeLocation? Location
);

internal record struct ExternalStructDefinition(
    string TypeFullyQualifiedName,
    string Alias,
    int Size,
    string? ExtensionNamespace
) {
    public bool IsVariableLength => StructMetadataConstants.IsVariableLength(Size);
}

internal enum ResolvedStructMemberKind {
    Primitive,
    NestedStruct,
    ExternalStruct,
    Quantized
}

internal enum MemberTryReadKind {
    TryReadOut,
    PreflightThenRead
}

internal readonly record struct MemberTryReadSpec(
    MemberTryReadKind Kind,
    string? TryReadCall,
    int FixedBits
);

internal record struct ResolvedStructMember(
    string MemberName,
    string TypeFullyQualifiedName,
    bool IsInitOnly,
    ResolvedStructMemberKind Kind,
    string WriteCall,
    string ReadExpression,
    MemberTryReadSpec TryRead,
    string SizeExpression,
    QuantizedRangeDefinition? QuantizedRange
);

internal record struct ResolvedStructDefinition(
    StructDefinition Source,
    PrimitiveSerializationMode Mode,
    int? FixedSize,
    EquatableImmutableArray<ResolvedStructMember> Members,
    string PrimitiveExtensionClassFqn,
    EquatableImmutableDictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> Methods,
    EquatableImmutableArray<string> RequiredUsings
);
