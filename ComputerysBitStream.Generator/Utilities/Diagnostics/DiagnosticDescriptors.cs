using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Diagnostics;

internal readonly record struct DiagnosticValueType(
    DiagnosticDescriptor Descriptor,
    ValueTypeLocation? Location,
    params object?[] MessageArgs
) {
    public Diagnostic ToDiagnostic() => Diagnostic.Create(Descriptor, Location?.ToLocation(), MessageArgs);

    public static bool HasErrors(IEnumerable<DiagnosticValueType> diagnostics) => diagnostics.Any(static diagnostic => diagnostic.Descriptor.DefaultSeverity == DiagnosticSeverity.Error);
}

internal static class DiagnosticDescriptors {
    public static readonly DiagnosticDescriptor DuplicateRole = new(
        id: "CBS001",
        title: "Multiple methods with the same role",
        messageFormat: "Role '{0}' is defined by more than one [BitStreamPrimitiveMethod] on this primitive",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateIncludedPrimitive = new(
        id: "CBS002",
        title: "Multiple included primitives for the same extension class",
        messageFormat: "Settings interface lists primitive extension '{0}' more than once",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MultipleGlobalSettings = new(
        id: "CBS003",
        title: "Multiple global settings",
        messageFormat: "Assembly defines global settings more than once ({0}). Remove duplicate [BitStreamDefaultSettings] attributes or merge the interfaces into one definition.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidSettingsInterface = new(
        id: "CBS004",
        title: "Invalid settings interface",
        messageFormat: "Type '{0}' is not a settings interface. Add [BitStreamSettings].",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidSettingType = new(
        id: "CBS005",
        title: "Invalid setting type",
        messageFormat: "Type '{0}' included in settings has none of [BitStreamPrimitive], [BitStreamStruct], or [BitStreamProxyStruct]. Add one of these attributes to register the type as a serializer.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor NoPrimitiveMethods = new(
        id: "CBS006",
        title: "No primitive methods on BitStreamPrimitive class",
        messageFormat: "Class '{0}' has [BitStreamPrimitive] but no methods marked with [BitStreamPrimitiveMethod]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MethodNotPublicStatic = new(
        id: "CBS007",
        title: "[BitStreamPrimitiveMethod] must be public static",
        messageFormat: "Method '{0}' marked with [BitStreamPrimitiveMethod] must be public and static",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidFixedSize = new(
        id: "CBS008",
        title: "Invalid fixed size in BitStreamFixedSizePrimitive",
        messageFormat: "Size '{0}' in [BitStreamFixedSizePrimitive] must be greater than 0",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor TypeMustBeStatic = new(
        id: "CBS009",
        title: "Type must be static",
        messageFormat: "Type '{0}' marked with [{1}] must be static",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidPrimitiveMethodSignature = new(
        id: "CBS010",
        title: "Invalid primitive method signature",
        messageFormat: "Method '{0}' marked with [BitStreamPrimitiveMethod({1})] does not match the expected signature: {2}",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Each BitStreamPrimitiveMethod role has a required signature. Copy the expected signature from the diagnostic and replace MethodName with your method name."
    );

    public static readonly DiagnosticDescriptor MemberSkipped = new(
        id: "CBS011",
        title: "Struct member skipped",
        messageFormat: "Member '{0}' was skipped because its {1}",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ProxyStructNotStruct = new(
        id: "CBS012",
        title: "[BitStreamProxyStruct] target is not a struct",
        messageFormat: "Type '{0}' specified in [BitStreamProxyStruct] is not a struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MissingSizeRole = new(
        id: "CBS013",
        title: "Missing BitStreamPrimitiveMethod(Size)",
        messageFormat: "Class '{0}' has [BitStreamPrimitive(VariableLength)] but no method marked with [BitStreamPrimitiveMethod(Size)]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor TypeMustBePartial = new(
        id: "CBS014",
        title: "Type must be partial",
        messageFormat: "Type '{0}' marked with [{1}] must be partial",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MissingCompanionAttribute = new(
        id: "CBS015",
        title: "Missing companion attribute on BitStreamPrimitive",
        messageFormat: "Class '{0}' marked with [BitStreamPrimitive({1})] must also have [{2}]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidQuantizedBitRange = new(
        id: "CBS016",
        title: "Invalid BitStreamQuantizedPrimitive bit range",
        messageFormat: "Minimum bits '{0}' and maximum bits '{1}' in [BitStreamQuantizedPrimitive] must satisfy 0 < minimum <= maximum",
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
        messageFormat: "Method '{0}' marked with [BitStreamPrimitiveMethod(Size)] is only valid on VariableLength primitives",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor TypeMustBePublic = new(
        id: "CBS020",
        title: "Type must be public",
        messageFormat: "Type '{0}' marked with [{1}] must be public",
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
        messageFormat: "Struct '{0}' with alias '{1}' has more than one [BitStreamStruct] or [BitStreamProxyStruct] definition",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicatePrimitiveDefinition = new(
        id: "CBS023",
        title: "Duplicate primitive definition",
        messageFormat: "Type '{0}' with alias '{1}' in namespace '{2}' has more than one [BitStreamPrimitive] definition",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateIncludedStruct = new(
        id: "CBS024",
        title: "Multiple included structs for the same type",
        messageFormat: "Settings interface lists struct type '{0}' more than once",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateIncludedExternalStruct = new(
        id: "CBS025",
        title: "Multiple included external structs for the same type",
        messageFormat: "Settings interface lists external struct type '{0}' more than once",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateMemberSerializer = new(
        id: "CBS026",
        title: "Multiple serializers on struct member",
        messageFormat: "Member '{0}' has more than one [BitStreamSerializer] attribute",
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
        messageFormat: "Argument '{0}' on [{1}] has an invalid value: '{2}'",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CircularSettingsReference = new(
        id: "CBS030",
        title: "Circular settings reference",
        messageFormat: "Settings interface '{0}' includes itself through inherited or nested settings interfaces and cannot be expanded",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "A settings interface cannot reference itself directly or through another settings interface in its inheritance chain."
    );

    public static readonly DiagnosticDescriptor PrimitiveMethodCalledOutsidePrimitive = new(
        id: "CBS031",
        title: "BitStream primitive method called outside primitive context",
        messageFormat: "BitStream primitive method '{0}' is only valid inside a type marked with [BitStreamPrimitive] or [BitStreamPrimitiveContext]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
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

    public static readonly DiagnosticDescriptor MissingTryReadRole = new(
        id: "CBS033",
        title: "Missing BitStreamPrimitiveMethod(TryRead)",
        messageFormat: "Class '{0}' has [BitStreamPrimitive(VariableLength)] but no method marked with [BitStreamPrimitiveMethod(TryRead)]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidTryReadRole = new(
        id: "CBS034",
        title: "BitStreamPrimitiveMethod(TryRead) on non-VariableLength primitive",
        messageFormat: "Method '{0}' marked with [BitStreamPrimitiveMethod(TryRead)] is only valid on VariableLength primitives",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CyclicStructReference = new(
        id: "CBS035",
        title: "Cyclic struct reference detected",
        messageFormat: "Struct '{0}' is nested inside itself through struct members and cannot be serialized",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "A struct cannot contain a member whose type resolves back to the same struct, directly or through other struct members."
    );

    public static readonly DiagnosticDescriptor StructMemberNotSerializable = new(
        id: "CBS036",
        title: "Struct member type not serializable",
        messageFormat: "Member '{0}' with type '{1}' could not be serialized using settings '{2}'. Fix errors on the nested struct type first.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Nested struct resolution failed. Check diagnostics on the member's struct type for cyclic references, missing primitives, or members with no serializer."
    );

    public static readonly DiagnosticDescriptor StructNoSerializableMembers = new(
        id: "CBS037",
        title: "Struct has no serializable members",
        messageFormat: "Struct '{0}' has no serializable members after skipping inaccessible or unsupported members",
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
        isEnabledByDefault: true,
        description: "Add a BitStreamPrimitive with Quantized serialization mode for the member type, then include it on your settings interface with [BitStreamSerializer]."
    );

    public static readonly DiagnosticDescriptor DuplicateAlias = new(
        id: "CBS039",
        title: "Duplicate struct alias",
        messageFormat: "Struct alias '{0}' is already used by a primitive or another struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidStructMetadataSize = new(
        id: "CBS040",
        title: "Invalid size in BitStreamStructMetadata",
        messageFormat: "Size '{0}' in [BitStreamStructMetadata] must be greater than 0 or -1 for variable-length structs",
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
        isEnabledByDefault: true,
        description: "Add a BitStreamPrimitive with VariableLength serialization mode for the member type, then include it on your settings interface with [BitStreamSerializer]."
    );

    public static readonly DiagnosticDescriptor DefaultPrimitiveNotInSettings = new(
        id: "CBS043",
        title: "Default primitive not in settings",
        messageFormat: "Member type '{0}' has no fixed-size serializer registered in settings '{1}'. Register a fixed-size primitive on your settings interface, or mark the member with [BitStreamStructVariableLength] and register a variable-length primitive.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Register a fixed-size BitStreamPrimitive for the member type on your settings interface. For variable-length encoding, add [BitStreamStructVariableLength] on the member and register a VariableLength primitive instead."
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

    public static readonly DiagnosticDescriptor MissingAttributeArgument = new(
        id: "CBS046",
        title: "Missing attribute argument",
        messageFormat: "Argument '{0}' is required on [{1}]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionMaxEntriesRequired = new(
        id: "CBS047",
        title: "Array member requires a read limit",
        messageFormat: "Array member '{0}' must have [BitStreamStructCollectionMaxEntries] with one limit for each array dimension or jagged level",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidCollectionMaxEntries = new(
        id: "CBS048",
        title: "Invalid array read limit count",
        messageFormat: "Array member '{0}' requires {1} max-read value(s) for its array shape, but the attribute supplied {2}",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionElementNotSerializable = new(
        id: "CBS049",
        title: "Array element type not serializable",
        messageFormat: "Array member '{0}' has element type '{1}', which is not serializable using settings '{2}'",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionMissingLengthPrefixSupport = new(
        id: "CBS050",
        title: "Array element serializer lacks length-prefixed support",
        messageFormat: "Array member '{0}' uses serializer '{1}', which must support span writes and array or span reads with a fixed-size int length prefix",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionAttributeOnNonArray = new(
        id: "CBS051",
        title: "Array read-limit attribute requires an array",
        messageFormat: "Member '{0}' has [BitStreamStructCollectionMaxEntries] but its type is not an array",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionMaxEntriesNegative = new(
        id: "CBS052",
        title: "Array read limits must be non-negative",
        messageFormat: "Array member '{0}' has a negative max-read limit in [BitStreamStructCollectionMaxEntries]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionMaxEntriesProductOverflow = new(
        id: "CBS053",
        title: "Array read limits exceed supported maximum",
        messageFormat: "Array member '{0}' max-read limits multiply to more than {1} elements, which is not supported",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor CollectionMissingSizeSupport = new(
        id: "CBS054",
        title: "Array element serializer lacks Size support",
        messageFormat: "Array member '{0}' uses variable-length serializer '{1}', which must define a Size method",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}
