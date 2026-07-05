using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal readonly record struct DiagnosticValueType(
    DiagnosticDescriptor Descriptor,
    ValueTypeLocation? Location,
    params object?[] MessageArgs
) {
    public Diagnostic ToDiagnostic() => Diagnostic.Create(Descriptor, Location?.ToLocation(), MessageArgs);
}

internal static class Diagnostics {
    public static readonly DiagnosticDescriptor DuplicateRole = new(
        id: "CBS001",
        title: "Multiple methods with the same role",
        messageFormat: "Multiple methods with the same role '{0}' in the same primitive",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateIncludedPrimitive = new(
        id: "CBS002",
        title: "Multiple included primitives for the same extension class",
        messageFormat: "Multiple included primitives with the same extension class '{0}' in the same settings interface",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MultipleGlobalSettings = new(
        id: "CBS003",
        title: "Multiple global settings",
        messageFormat: "Multiple global settings are defined for the assembly",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidSettingsInterface = new(
        id: "CBS004",
        title: "Invalid settings interface",
        messageFormat: "The type '{0}' is not a valid settings interface. It must be annotated with [BitStreamSettings].",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidSettingType = new(
        id: "CBS005",
        title: "Invalid setting type",
        messageFormat: "The type '{0}' included in settings is missing the BitStreamPrimitive, BitStreamStruct, or BitStreamProxyStruct attribute",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor NoPrimitiveMethods = new(
        id: "CBS006",
        title: "No primitive methods on BitStreamPrimitive class",
        messageFormat: "The class '{0}' marked with [BitStreamPrimitive] has no methods marked with [BitStreamPrimitiveMethod]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MethodNotPublicStatic = new(
        id: "CBS007",
        title: "BitStreamPrimitiveMethod method is not public static",
        messageFormat: "The method '{0}' marked with [BitStreamPrimitiveMethod] must be public and static",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidFixedSize = new(
        id: "CBS008",
        title: "Invalid fixed size in BitStreamFixedSizePrimitive",
        messageFormat: "The size '{0}' in [BitStreamFixedSizePrimitive] must be greater than 0",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor TypeMustBeStatic = new(
        id: "CBS009",
        title: "Type must be static",
        messageFormat: "The type '{0}' marked with [{1}] must be static",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidPrimitiveMethodSignature = new(
        id: "CBS010",
        title: "Invalid primitive method signature",
        messageFormat: "The method '{0}' marked with [BitStreamPrimitiveMethod] for role '{1}' does not match the expected signature: {2}",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MemberSkipped = new(
        id: "CBS011",
        title: "Struct member skipped",
        messageFormat: "Member '{0}' skipped: {1}",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ProxyStructNotStruct = new(
        id: "CBS012",
        title: "BitStreamProxyStructAttribute target is not a struct",
        messageFormat: "The type '{0}' specified in [BitStreamProxyStruct] is not a struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MissingSizeRole = new(
        id: "CBS013",
        title: "Missing BitStreamPrimitiveMethod(Size)",
        messageFormat: "The class '{0}' marked with [BitStreamPrimitive(…, VariableLength)] must have a method marked with [BitStreamPrimitiveMethod(Size)]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor TypeMustBePartial = new(
        id: "CBS014",
        title: "Type must be partial",
        messageFormat: "The type '{0}' marked with [{1}] must be partial",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MissingCompanionAttribute = new(
        id: "CBS015",
        title: "Missing companion attribute on BitStreamPrimitive",
        messageFormat: "The class '{0}' marked with [BitStreamPrimitive({1})] must also have [{2}]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidQuantizedBitRange = new(
        id: "CBS016",
        title: "Invalid BitStreamQuantizedPrimitive bit range",
        messageFormat: "The minimum bits '{0}' and maximum bits '{1}' in [BitStreamQuantizedPrimitive] must satisfy 0 < minimum <= maximum",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidQuantizedBitCount = new(
        id: "CBS017",
        title: "Invalid quantized bit count",
        messageFormat: "Bit count '{0}' on member '{1}' must be greater than 0",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidQuantizedMember = new(
        id: "CBS018",
        title: "Invalid quantized member",
        messageFormat: "Quantized member '{0}' on member '{1}' must refer to an accessible const or static readonly member",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidSizeRole = new(
        id: "CBS019",
        title: "BitStreamPrimitiveMethod(Size) on non-VariableLength primitive",
        messageFormat: "The method '{0}' marked with [BitStreamPrimitiveMethod(Size)] is only valid on VariableLength primitives",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor TypeMustBePublic = new(
        id: "CBS020",
        title: "Type must be public",
        messageFormat: "The type '{0}' marked with [{1}] must be public",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ConflictingStructMemberAttributes = new(
        id: "CBS021",
        title: "Conflicting struct member attributes",
        messageFormat: "Member '{0}' cannot have both [BitStreamStructInclude] and [BitStreamStructIgnore]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateStructDefinition = new(
        id: "CBS022",
        title: "Duplicate struct definition",
        messageFormat: "Multiple [BitStreamStruct] or [BitStreamProxyStruct] definitions exist for struct '{0}' with alias '{1}'",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CircularSettingsReference = new(
        id: "CBS030",
        title: "Circular settings reference",
        messageFormat: "The settings interface '{0}' is referenced recursively and cannot be expanded",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicatePrimitiveDefinition = new(
        id: "CBS023",
        title: "Duplicate primitive definition",
        messageFormat: "Multiple [BitStreamPrimitive] definitions exist for type '{0}' with alias '{1}' in namespace '{2}'",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateIncludedStruct = new(
        id: "CBS024",
        title: "Multiple included structs for the same type",
        messageFormat: "Multiple included structs with the same type '{0}' in the same settings interface",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateIncludedExternalStruct = new(
        id: "CBS025",
        title: "Multiple included external structs for the same type",
        messageFormat: "Multiple included external structs with the same type '{0}' in the same settings interface",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateMemberSerializer = new(
        id: "CBS026",
        title: "Multiple serializers on struct member",
        messageFormat: "Member '{0}' has multiple [BitStreamSerializer] attributes",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ProxyMemberTypeMismatch = new(
        id: "CBS027",
        title: "Proxy member type mismatch",
        messageFormat: "Proxy member '{0}' does not have the same type as the corresponding member on struct '{1}'",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ProxyMemberNotOnTarget = new(
        id: "CBS028",
        title: "Proxy member not on target struct",
        messageFormat: "Proxy member '{0}' has no corresponding member on struct '{1}'",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidAttributeArgument = new(
        id: "CBS029",
        title: "Invalid attribute argument",
        messageFormat: "The argument '{0}' on [{1}] is missing or has an invalid value",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor PrimitiveMethodCalledOutsidePrimitive = new(
        id: "CBS031",
        title: "BitStream primitive method called outside primitive context",
        messageFormat: "BitStream primitive method '{0}' should only be called from within a type marked with [BitStreamPrimitive] or [BitStreamPrimitiveContext]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MissingTryReadRole = new(
        id: "CBS033",
        title: "Missing BitStreamPrimitiveMethod(TryRead)",
        messageFormat: "The class '{0}' marked with [BitStreamPrimitive(…, VariableLength)] must have a method marked with [BitStreamPrimitiveMethod(TryRead)]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidTryReadRole = new(
        id: "CBS034",
        title: "BitStreamPrimitiveMethod(TryRead) on non-VariableLength primitive",
        messageFormat: "The method '{0}' marked with [BitStreamPrimitiveMethod(TryRead)] is only valid on VariableLength primitives",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MissingLengthPrefixHandler = new(
        id: "CBS032",
        title: "Missing int length-prefix handler for primitive",
        messageFormat: "Primitive '{0}' for '{1}' declares length-prefixed span/array methods but no fixed-size int length-prefix handler was found in settings. Length-prefixed methods were not generated.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CyclicStructReference = new(
        id: "CBS035",
        title: "Cyclic struct reference detected",
        messageFormat: "Struct '{0}' contains a cyclic reference and cannot be serialized",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor StructMemberNotSerializable = new(
        id: "CBS036",
        title: "Struct member type not serializable",
        messageFormat: "Member type '{0}' is not serializable with current settings (settings: '{1}')",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor StructNoSerializableMembers = new(
        id: "CBS037",
        title: "Struct has no serializable members",
        messageFormat: "Struct '{0}' has no serializable members",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor QuantizedPrimitiveNotInSettings = new(
        id: "CBS038",
        title: "Quantized primitive not in settings",
        messageFormat: "Member '{0}' uses [BitStreamStructQuantized] but no Quantized serializer for type '{1}' is registered in settings. Register a Quantized primitive on your settings interface.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateAlias = new(
        id: "CBS039",
        title: "Duplicate alias across primitives or structs",
        messageFormat: "Alias '{0}' is already used by another primitive or struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidStructMetadataSize = new(
        id: "CBS040",
        title: "Invalid size in BitStreamStructMetadata",
        messageFormat: "The size '{0}' in [BitStreamStructMetadata] must be greater than 0 or -1 for variable-length structs",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InaccessibleStructMember = new(
        id: "CBS041",
        title: "Struct member is not accessible",
        messageFormat: "Member '{0}' cannot be serialized because it is not public. BitStream struct serializers only support public members.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor VariableLengthPrimitiveNotInSettings = new(
        id: "CBS042",
        title: "Variable-length primitive not in settings",
        messageFormat: "Member '{0}' uses [BitStreamStructVariableLength] but no variable-length serializer for type '{1}' is registered in settings. Register a variable-length primitive on your settings interface.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DefaultPrimitiveNotInSettings = new(
        id: "CBS043",
        title: "Default primitive not in settings",
        messageFormat: "Member type '{0}' has no fixed-size serializer registered in settings '{1}'. Register a fixed-size primitive on your settings interface, or mark the member with [BitStreamStructVariableLength] and register a variable-length primitive.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor QuantizedPrimitiveRequiresAttribute = new(
        id: "CBS044",
        title: "Quantized serializer requires [BitStreamStructQuantized]",
        messageFormat: "Member '{0}' of type '{1}' has a Quantized serializer in settings '{2}', but the member is missing [BitStreamStructQuantized]. Add the attribute or register a fixed-size serializer for this type.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ConflictingStructMemberSerializationAttributes = new(
        id: "CBS045",
        title: "Conflicting struct member serialization attributes",
        messageFormat: "Member '{0}' cannot have both [BitStreamStructVariableLength] and [BitStreamStructQuantized]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}
