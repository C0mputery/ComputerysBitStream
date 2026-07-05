using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal readonly record struct Collected<T>(
    T Value,
    EquatableImmutableArray<DiagnosticValueType> Diagnostics = default
) {
    public bool IsValid => !HasErrors(Diagnostics);

    internal static bool IsValidDiagnostics(ImmutableArray<DiagnosticValueType> diagnostics) => !HasErrors(diagnostics);

    internal static T UnwrapCollected(Collected<T> collected, out ImmutableArray<DiagnosticValueType> diagnostics) {
        diagnostics = collected.Diagnostics;
        return collected.Value;
    }

    internal static ImmutableArray<T> UnwrapCollectedArray(ImmutableArray<Collected<T>> collectedArray, out ImmutableArray<DiagnosticValueType> diagnostics) {
        ImmutableArray<T>.Builder values = ImmutableArray.CreateBuilder<T>();
        ImmutableArray<DiagnosticValueType>.Builder diagnosticsBuilder = ImmutableArray.CreateBuilder<DiagnosticValueType>();

        foreach (Collected<T> collected in collectedArray) {
            T value = UnwrapCollected(collected, out ImmutableArray<DiagnosticValueType> itemDiagnostics);
            values.Add(value);
            diagnosticsBuilder.AddRange(itemDiagnostics);
        }

        diagnostics = diagnosticsBuilder.ToImmutable();
        return values.ToImmutable();
    }

    internal static ImmutableArray<T> UnwrapValidCollectedArray(ImmutableArray<Collected<T>> collectedArray, out ImmutableArray<DiagnosticValueType> diagnostics) {
        ImmutableArray<T>.Builder values = ImmutableArray.CreateBuilder<T>();
        ImmutableArray<DiagnosticValueType>.Builder diagnosticsBuilder = ImmutableArray.CreateBuilder<DiagnosticValueType>();

        foreach (Collected<T> collected in collectedArray) {
            diagnosticsBuilder.AddRange(collected.Diagnostics);
            if (!collected.IsValid) { continue; }

            values.Add(collected.Value);
        }

        diagnostics = diagnosticsBuilder.ToImmutable();
        return values.ToImmutable();
    }

    private static bool HasErrors(ImmutableArray<DiagnosticValueType> diagnostics) {
        return diagnostics.Any(static diagnostic => diagnostic.Descriptor.DefaultSeverity == DiagnosticSeverity.Error);
    }
}
