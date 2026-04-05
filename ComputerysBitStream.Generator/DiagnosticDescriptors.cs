using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal class DiagnosticDescriptors {
    internal static readonly DiagnosticDescriptor DuplicateTypeRule = new DiagnosticDescriptor(
        id: "CBSG001",
        title: "Duplicate BitStreamTypeAttribute",
        messageFormat: "The type '{0}' is already handled by another BitStreamTypeAttribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    internal static readonly DiagnosticDescriptor DuplicateRawRoleRule = new DiagnosticDescriptor(
        id: "CBSG002",
        title: "Duplicate BitStreamRawAttribute role",
        messageFormat: "The role '{0}' is specified more than once in '{1}' (first: '{2}', again: '{3}')",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}