using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerysBitStream.Generator;

internal static class GeneratedSourceSyntax {
    private static readonly string[] StandardUsings = [
        "System",
        "System.ComponentModel",
        "System.Runtime.CompilerServices",
        "ComputerysBitStream"
    ];

    internal static void EmitStandardUsings(SourceWriter writer, ReadOnlySpan<string> additionalNamespaces = default) {
        HashSet<string> emittedNamespaces = new(StringComparer.Ordinal);
        foreach (string namespaceName in StandardUsings) { EmitUsing(writer, emittedNamespaces, namespaceName); }
        foreach (string namespaceName in additionalNamespaces) { EmitUsing(writer, emittedNamespaces, namespaceName); }
        writer.WriteLine();
    }

    internal static string GetShortTypeName(string fullyQualifiedTypeName) {
        int lastDot = fullyQualifiedTypeName.LastIndexOf('.');
        return lastDot < 0 ? fullyQualifiedTypeName : fullyQualifiedTypeName.Substring(lastDot + 1);
    }

    internal static string? GetNamespaceFromFullyQualifiedName(string fullyQualifiedTypeName) {
        int lastDot = fullyQualifiedTypeName.LastIndexOf('.');
        return lastDot < 0 ? null : fullyQualifiedTypeName.Substring(0, lastDot);
    }

    internal static string QualifyTypeReference(string? generatedNamespaceName, string fullyQualifiedTypeName, List<string>? additionalUsings = null) {
        string? typeNamespace = GetNamespaceFromFullyQualifiedName(fullyQualifiedTypeName);
        if (additionalUsings is not null) {
            CollectAdditionalUsings(additionalUsings, typeNamespace, generatedNamespaceName);
            return GetShortTypeName(fullyQualifiedTypeName);
        }

        if (generatedNamespaceName is null || typeNamespace is null || !string.Equals(typeNamespace, generatedNamespaceName, StringComparison.Ordinal)) {
            return fullyQualifiedTypeName;
        }

        return GetShortTypeName(fullyQualifiedTypeName);
    }

    internal static void CollectAdditionalUsings(List<string> usings, string? namespaceName, string? generatedNamespaceName) {
        if (namespaceName is null) { return; }
        if (StandardUsings.Any(standardUsing => string.Equals(namespaceName, standardUsing, StringComparison.Ordinal))) { return; }
        if (string.Equals(namespaceName, generatedNamespaceName, StringComparison.Ordinal)) { return; }
        if (usings.Contains(namespaceName)) { return; }

        usings.Add(namespaceName);
    }

    private static void EmitUsing(SourceWriter writer, HashSet<string> emittedNamespaces, string namespaceName) {
        if (namespaceName.Length == 0) { return; }
        if (!emittedNamespaces.Add(namespaceName)) { return; }

        writer.WriteLine($"using {namespaceName};");
    }

    internal static string GetSourceHintFileName(string? namespaceName, string typeName) {
        return namespaceName is null ? $"{typeName}.g.cs" : $"{namespaceName}.{typeName}.g.cs";
    }

    internal static string EmitThrowInsufficientReadSpace(string typeName, string requiredBitsExpression) {
        return $"context.ThrowIfInsufficientSpace(\"{typeName}\", {requiredBitsExpression});";
    }

    internal static string EmitThrowReadFailed(string typeName) {
        return $"context.ThrowIfReadFailed(\"{typeName}\");";
    }

    internal static string EmitThrowIfTryReadFailed(string typeName, string tryExpression, string successBody) {
        return $$"""
                 if (!{{tryExpression}}) {
                     {{EmitThrowReadFailed(typeName)}}
                 }
                 {{successBody}}
                 """;
    }
}
