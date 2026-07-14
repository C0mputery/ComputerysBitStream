using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.Emission;
using ComputerysBitStream.Generator.EquatableCollections;

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

internal record struct QuantizedDefinition(
    string MinExpression,
    string MaxExpression,
    int BitCount,
    ValueTypeLocation? Location
);

internal record struct StructCollectionDefinition(
    string ElementTypeFullyQualifiedFormat,
    string ElementTypeEmitFormat,
    EquatableImmutableArray<string> ArrayTypeFullyQualifiedFormats,
    EquatableImmutableArray<string> ArrayTypeEmitFormats,
    EquatableImmutableArray<int> Ranks,
    EquatableImmutableArray<int> MaxEntries
);

internal record struct StructMemberDefinition(
    string MemberName,
    string TypeFullyQualifiedFormat,
    string TypeEmitFormat,
    bool IsProperty,
    bool IsInitOnly,
    string? SerializerExtensionClassFullyQualifiedName,
    bool IsVariableLength,
    QuantizedDefinition? Quantized,
    StructCollectionDefinition? Collection,
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
    public bool IsVariableLength => StructMetadataHelper.IsVariableLength(Size);
}

internal enum ResolvedStructMemberKind {
    Primitive,
    NestedStruct,
    ExternalStruct,
    Quantized,
    Collection
}

internal enum MemberTryReadKind {
    TryReadOut,
    PreflightThenRead,
    Collection
}

internal readonly record struct MemberTryReadSpec(
    MemberTryReadKind Kind,
    string? TryReadCall,
    int FixedBits
);

internal record struct ResolvedStructMember(
    string MemberName,
    string TypeFullyQualifiedName,
    string TypeEmitName,
    bool IsInitOnly,
    ResolvedStructMemberKind Kind,
    string WriteCall,
    string ReadExpression,
    MemberTryReadSpec TryRead,
    string SizeExpression,
    QuantizedDefinition? Quantized,
    ResolvedStructCollection? Collection = null
);

internal record struct ResolvedStructCollection(
    StructCollectionDefinition Source,
    string ElementTypeEmitName,
    string ElementWriteContextClass,
    string ElementReadContextClass,
    string ElementWriteWithMaxCountMethod,
    string ElementWriteWithoutLengthMethod,
    string ElementTryReadMethod,
    string ElementTryReadWithCountMethod,
    string ElementExtraArguments,
    string IntExtensionClass,
    string IntWriteMethod,
    string IntTryReadMethod,
    string IntSizeMethod,
    string IntTargetTypeEmitName,
    string? ElementSizeExpression,
    int? ElementFixedSize
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
