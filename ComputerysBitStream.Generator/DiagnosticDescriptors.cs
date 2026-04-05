using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal static class DiagnosticDescriptors {
    private static Location? ToLocation(BitStreamLocation? location) {
        if (!location.HasValue) { return null; }
        BitStreamLocation value = location.Value;
        return Location.Create(value.FilePath, value.TextSpan, value.LineSpan);
    }
    
    internal static readonly DiagnosticDescriptor DuplicateTypeRule = new DiagnosticDescriptor(
        id: "CBSG001",
        title: "Duplicate BitStreamTypeAttribute",
        messageFormat: "The type '{0}' is already handled by another BitStreamTypeAttribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    internal static Diagnostic CreateDuplicateType(BitStreamTypeInfo typeInfo) {
        return Diagnostic.Create(DuplicateTypeRule, ToLocation(typeInfo.Location), typeInfo.TargetTypeFullName);
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
            ToLocation(duplicate.Location),
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
            ToLocation(nonPublicRawMethod.Location),
            nonPublicRawMethod.MethodName,
            nonPublicRawMethod.ClassName,
            nonPublicRawMethod.Role,
            nonPublicRawMethod.Accessibility);
    }
}