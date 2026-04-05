using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal static class DiagnosticDescriptors {
    private static readonly DiagnosticDescriptor DuplicateTypeRule = new DiagnosticDescriptor(
        id: "CBSG001",
        title: "Duplicate BitStreamTypeAttribute",
        messageFormat: "The type '{0}' is already handled by another BitStreamTypeAttribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateDuplicateType(BitStreamTypeInfo typeInfo) {
        return Diagnostic.Create(DuplicateTypeRule, typeInfo.Location?.ToLocation(), typeInfo.TargetTypeFullName);
    }

    private static readonly DiagnosticDescriptor DuplicateRawRoleRule = new DiagnosticDescriptor(
        id: "CBSG002",
        title: "Duplicate BitStreamRawAttribute role",
        messageFormat: "The role '{0}' is specified more than once in '{1}' (first: '{2}', again: '{3}')",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateDuplicateRawRole(DuplicateRawRoleInfo duplicate) {
        return Diagnostic.Create(
            DuplicateRawRoleRule,
            duplicate.Location?.ToLocation(),
            duplicate.Role,
            duplicate.ClassName,
            duplicate.FirstMethod,
            duplicate.SecondMethod);
    }

    private static readonly DiagnosticDescriptor NonPublicRawMethodRule = new DiagnosticDescriptor(
        id: "CBSG003",
        title: "BitStreamRawAttribute method must be public",
        messageFormat: "The method '{0}' in '{1}' is marked as role '{2}' but has '{3}' accessibility; BitStreamRaw methods must be public",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateNonPublicRawMethod(NonPublicRawMethodInfo nonPublicRawMethod) {
        return Diagnostic.Create(
            NonPublicRawMethodRule,
            nonPublicRawMethod.Location?.ToLocation(),
            nonPublicRawMethod.MethodName,
            nonPublicRawMethod.ClassName,
            nonPublicRawMethod.Role,
            nonPublicRawMethod.Accessibility);
    }

    private static readonly DiagnosticDescriptor InvalidTargetTypeNameRule = new DiagnosticDescriptor(
        id: "CBSG004",
        title: "Invalid TargetTypeName",
        messageFormat: "The BitStreamTypeAttribute on '{0}' specifies an empty TargetTypeName",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateInvalidTargetTypeName(BitStreamTypeInfo typeInfo) {
        return Diagnostic.Create(InvalidTargetTypeNameRule, typeInfo.Location?.ToLocation(), typeInfo.ClassName);
    }

    private static readonly DiagnosticDescriptor DuplicateTargetTypeNameRule = new DiagnosticDescriptor(
        id: "CBSG005",
        title: "Duplicate TargetTypeName",
        messageFormat: "The TargetTypeName '{0}' from '{1}' conflicts with '{2}'; generated method names must be unique",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateDuplicateTargetTypeName(BitStreamTypeInfo firstTypeInfo, BitStreamTypeInfo duplicateTypeInfo) {
        return Diagnostic.Create(
            DuplicateTargetTypeNameRule,
            duplicateTypeInfo.Location?.ToLocation(),
            duplicateTypeInfo.TargetTypeName,
            duplicateTypeInfo.ClassName,
            firstTypeInfo.ClassName);
    }
}