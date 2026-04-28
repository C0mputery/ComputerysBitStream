using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ComputerysBitStream.Generator;

internal record struct DiagnosticData(
    string DescriptorId,
    ValueTypeLocation? Location,
    string[] MessageArgs
) {
    public static DiagnosticData Create(DiagnosticDescriptor descriptor, Location? location, string[] messageArgs) => new(descriptor.Id, location, messageArgs);
}

internal static class Diagnostics {
    public static readonly DiagnosticDescriptor DuplicateRole = new(
        id: "CBS001",
        title: "Multiple methods with the same role",
        messageFormat: "Multiple methods with the same role '{0}' in the same raw type",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateDuplicateRole(Location? location, BitStreamRawRole role) => Diagnostic.Create(DuplicateRole, location, role);

    public static readonly DiagnosticDescriptor DuplicateIncludedRawType = new(
        id: "CBS002",
        title: "Multiple included raw types for the same target type",
        messageFormat: "Multiple included raw types with the same target type '{0}' in the same settings interface",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateDuplicateIncludedRawType(Location? location, string targetTypeFullyQualifiedName) => Diagnostic.Create(DuplicateIncludedRawType, location, targetTypeFullyQualifiedName);

    public static readonly DiagnosticDescriptor MultipleGlobalSettings = new(
        id: "CBS003",
        title: "Multiple global settings",
        messageFormat: "Multiple global settings are defined for the assembly",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateMultipleGlobalSettings(Location? location) => Diagnostic.Create(MultipleGlobalSettings, location);

    public static readonly DiagnosticDescriptor MissingSettingsAttribute = new(
        id: "CBS004",
        title: "Missing BitStreamSettings attribute",
        messageFormat: "The type '{0}' passed to DefaultBitStreamSettings must have the BitStreamSettings attribute",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateMissingSettingsAttribute(Location? location, params object?[]? messageArgs) => Diagnostic.Create(MissingSettingsAttribute, location, messageArgs);

    public static readonly DiagnosticDescriptor InvalidSettingType = new(
        id: "CBS005",
        title: "Invalid setting type",
        messageFormat: "The type '{0}' included in settings is missing the BitStreamRawType attribute",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateInvalidSettingType(Location? location, params object?[]? messageArgs) => Diagnostic.Create(InvalidSettingType, location, messageArgs);

    public static readonly DiagnosticDescriptor NoRawMethods = new(
        id: "CBS006",
        title: "No raw methods on BitStreamRawType class",
        messageFormat: "The class '{0}' marked with [BitStreamRawType] has no methods marked with [BitStreamRawMethod]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateNoRawMethods(Location? location, string className) => Diagnostic.Create(NoRawMethods, location, className);

    public static readonly DiagnosticDescriptor MethodNotPublicStatic = new(
        id: "CBS007",
        title: "BitStreamRawMethod method is not public static",
        messageFormat: "The method '{0}' marked with [BitStreamRawMethod] must be public and static",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateMethodNotPublicStatic(Location? location, string methodName) => Diagnostic.Create(MethodNotPublicStatic, location, methodName);

    public static readonly DiagnosticDescriptor DuplicateAlias = new(
        id: "CBS008",
        title: "Duplicate alias across raw types",
        messageFormat: "Multiple raw types use the alias '{0}', which would produce duplicate generated class names",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateDuplicateAlias(Location? location, string alias) => Diagnostic.Create(DuplicateAlias, location, alias);

    public static readonly DiagnosticDescriptor InvalidSize = new(
        id: "CBS009",
        title: "Invalid size in BitStreamRawType",
        messageFormat: "The size '{0}' in [BitStreamRawType] must be greater than 0",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateInvalidSize(Location? location, int size) => Diagnostic.Create(InvalidSize, location, size);

    public static readonly DiagnosticDescriptor RawTypeClassNotStatic = new(
        id: "CBS010",
        title: "BitStreamRawType class is not static",
        messageFormat: "The class '{0}' marked with [BitStreamRawType] must be static because it contains extension methods",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateRawTypeClassNotStatic(Location? location, string className) => Diagnostic.Create(RawTypeClassNotStatic, location, className);

    public static readonly DiagnosticDescriptor InvalidRawMethodSignature = new(
        id: "CBS011",
        title: "Invalid raw method signature",
        messageFormat: "The method '{0}' marked with [BitStreamRawMethod] for role '{1}' does not match the expected signature: {2}",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateInvalidRawMethodSignature(Location? location, string methodName, BitStreamRawRole role, string expectedSignature)
        => Diagnostic.Create(InvalidRawMethodSignature, location, methodName, role, expectedSignature);

    public static readonly DiagnosticDescriptor StructMemberNotSerializable = new(
        id: "CBS012",
        title: "Struct member type not serializable",
        messageFormat: "Member type '{0}' is not serializable with current settings (settings: '{1}')",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateStructMemberNotSerializable(Location? location, string memberType, string settingsName) => Diagnostic.Create(StructMemberNotSerializable, location, memberType, settingsName);

    public static readonly DiagnosticDescriptor StructNoSerializableMembers = new(
        id: "CBS013",
        title: "Struct has no serializable members",
        messageFormat: "Struct '{0}' has no serializable members",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateStructNoSerializableMembers(Location? location, string structName) => Diagnostic.Create(StructNoSerializableMembers, location, structName);

    public static readonly DiagnosticDescriptor DuplicateAliasStruct = new(
        id: "CBS014",
        title: "Duplicate alias across raw types or structs",
        messageFormat: "Alias '{0}' is already used by another raw type or struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateDuplicateAliasStruct(Location? location, string alias) => Diagnostic.Create(DuplicateAliasStruct, location, alias);

    public static readonly DiagnosticDescriptor ReadOnlyPropertySkipped = new(
        id: "CBS015",
        title: "Read-only property skipped",
        messageFormat: "Read-only property '{0}' skipped",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateReadOnlyPropertySkipped(Location? location, string propertyName) => Diagnostic.Create(ReadOnlyPropertySkipped, location, propertyName);

    public static readonly DiagnosticDescriptor NonPublicSetterSkipped = new(
        id: "CBS019",
        title: "Non-public setter skipped",
        messageFormat: "Property '{0}' skipped because its setter is not public",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateNonPublicSetterSkipped(Location? location, string propertyName) => Diagnostic.Create(NonPublicSetterSkipped, location, propertyName);

    public static readonly DiagnosticDescriptor ReadOnlyFieldSkipped = new(
        id: "CBS016",
        title: "Read-only field skipped despite inclusion attribute",
        messageFormat: "Read-only field '{0}' skipped despite [BitStreamStructInclude]",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateReadOnlyFieldSkipped(Location? location, string fieldName) => Diagnostic.Create(ReadOnlyFieldSkipped, location, fieldName);

    public static readonly DiagnosticDescriptor RefFieldSkipped = new(
        id: "CBS024",
        title: "Ref field skipped",
        messageFormat: "Ref field '{0}' skipped because ref fields cannot be serialized",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateRefFieldSkipped(Location? location, string fieldName) => Diagnostic.Create(RefFieldSkipped, location, fieldName);

    public static readonly DiagnosticDescriptor ProxyStructNotStruct = new(
        id: "CBS017",
        title: "BitStreamProxyStructAttribute target is not a struct",
        messageFormat: "The type '{0}' specified in [BitStreamProxyStruct] is not a struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateProxyStructNotStruct(Location? location, string typeName) => Diagnostic.Create(ProxyStructNotStruct, location, typeName);

    public static readonly DiagnosticDescriptor CyclicStructReference = new(
        id: "CBS023",
        title: "Cyclic struct reference detected",
        messageFormat: "Struct '{0}' contains a cyclic reference and cannot be serialized",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateCyclicStructReference(Location? location, string structName) => Diagnostic.Create(CyclicStructReference, location, structName);

    public static readonly DiagnosticDescriptor ProxyStructIncludeNotFound = new(
        id: "CBS018",
        title: "Included member not found on external struct",
        messageFormat: "Included member '{0}' was not found on the target struct",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateProxyStructIncludeNotFound(Location? location, string memberName) => Diagnostic.Create(ProxyStructIncludeNotFound, location, memberName);

    public static readonly DiagnosticDescriptor ProxyStructClassNotStatic = new(
        id: "CBS020",
        title: "BitStreamProxyStruct class is not static",
        messageFormat: "The class '{0}' marked with [BitStreamProxyStruct] must be static",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateProxyStructClassNotStatic(Location? location, string className) => Diagnostic.Create(ProxyStructClassNotStatic, location, className);

    public static readonly DiagnosticDescriptor ProxyStructClassNotPartial = new(
        id: "CBS021",
        title: "BitStreamProxyStruct class is not partial",
        messageFormat: "The class '{0}' marked with [BitStreamProxyStruct] must be partial",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateProxyStructClassNotPartial(Location? location, string className) => Diagnostic.Create(ProxyStructClassNotPartial, location, className);

    public static readonly DiagnosticDescriptor StructNotPartial = new(
        id: "CBS022",
        title: "BitStreamStruct struct is not partial",
        messageFormat: "The struct '{0}' marked with [BitStreamStruct] must be partial",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateStructNotPartial(Location? location, string structName) => Diagnostic.Create(StructNotPartial, location, structName);

    public static readonly DiagnosticDescriptor InvalidStructSettingsType = new(
        id: "CBS025",
        title: "Invalid settings type in BitStreamStruct",
        messageFormat: "The type '{0}' passed as settings is not a valid settings interface. It must be annotated with [BitStreamSettings].",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    public static Diagnostic CreateInvalidStructSettingsType(Location? location, string typeName) => Diagnostic.Create(InvalidStructSettingsType, location, typeName);

    public static readonly Dictionary<string, DiagnosticDescriptor> ById = new() {
        [DuplicateRole.Id] = DuplicateRole,
        [DuplicateIncludedRawType.Id] = DuplicateIncludedRawType,
        [MultipleGlobalSettings.Id] = MultipleGlobalSettings,
        [MissingSettingsAttribute.Id] = MissingSettingsAttribute,
        [InvalidSettingType.Id] = InvalidSettingType,
        [NoRawMethods.Id] = NoRawMethods,
        [MethodNotPublicStatic.Id] = MethodNotPublicStatic,
        [DuplicateAlias.Id] = DuplicateAlias,
        [InvalidSize.Id] = InvalidSize,
        [RawTypeClassNotStatic.Id] = RawTypeClassNotStatic,
        [InvalidRawMethodSignature.Id] = InvalidRawMethodSignature,
        [StructMemberNotSerializable.Id] = StructMemberNotSerializable,
        [StructNoSerializableMembers.Id] = StructNoSerializableMembers,
        [DuplicateAliasStruct.Id] = DuplicateAliasStruct,
        [ReadOnlyPropertySkipped.Id] = ReadOnlyPropertySkipped,
        [NonPublicSetterSkipped.Id] = NonPublicSetterSkipped,
        [ReadOnlyFieldSkipped.Id] = ReadOnlyFieldSkipped,
        [RefFieldSkipped.Id] = RefFieldSkipped,
        [ProxyStructNotStruct.Id] = ProxyStructNotStruct,
        [ProxyStructIncludeNotFound.Id] = ProxyStructIncludeNotFound,
        [ProxyStructClassNotStatic.Id] = ProxyStructClassNotStatic,
        [CyclicStructReference.Id] = CyclicStructReference,
        [ProxyStructClassNotPartial.Id] = ProxyStructClassNotPartial,
        [StructNotPartial.Id] = StructNotPartial,
        [InvalidStructSettingsType.Id] = InvalidStructSettingsType,
    };

    public static Diagnostic? Create(DiagnosticData diagnostic) {
        return ById.TryGetValue(diagnostic.DescriptorId, out DiagnosticDescriptor? descriptor) ? Diagnostic.Create(descriptor, diagnostic.Location?.ToLocation(), diagnostic.MessageArgs) : null;
    }
}
