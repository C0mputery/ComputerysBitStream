using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal static class DiagnosticDescriptors {
    internal static readonly DiagnosticDescriptor DuplicateTypeRule = new DiagnosticDescriptor(
        id: "CBSG001",
        title: "Duplicate BitStreamTypeAttribute",
        messageFormat: "The type '{0}' is already handled by another BitStreamTypeAttribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateDuplicateType(BitStreamTypeInfo typeInfo) {
        return Diagnostic.Create(DuplicateTypeRule, typeInfo.Location?.ToLocation(), typeInfo.TargetTypeFullName);
    }
    
    internal static readonly DiagnosticDescriptor DuplicateRawRoleRule = new DiagnosticDescriptor(
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

    internal static readonly DiagnosticDescriptor NonPublicRawMethodRule = new DiagnosticDescriptor(
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
}