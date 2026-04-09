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
        messageFormat: "Multiple global settings are defined for the assembly. Only the first one will be used.",
        category: "BitStream",
        defaultSeverity: DiagnosticSeverity.Warning,
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
    };

    public static Diagnostic? Create(DiagnosticData diagnostic) {
        return ById.TryGetValue(diagnostic.DescriptorId, out DiagnosticDescriptor? descriptor) ? Diagnostic.Create(descriptor, diagnostic.Location?.ToLocation(), diagnostic.MessageArgs) : null;
    }
}
