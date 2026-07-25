using System.Collections.Immutable;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.EquatableCollections;

namespace ComputerysBitStream.Generator;

internal readonly record struct Collected<T>(
    T Value,
    EquatableImmutableArray<DiagnosticValueType> Diagnostics = default
) {
    public bool IsValid => !DiagnosticValueType.HasErrors(Diagnostics);

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
}
