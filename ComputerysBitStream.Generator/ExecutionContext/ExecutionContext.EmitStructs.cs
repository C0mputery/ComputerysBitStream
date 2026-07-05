using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Generator.Emitters;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal readonly ref partial struct ExecutionContext {
    private ImmutableArray<PrimitiveDefinition> ResolveAndEmitStructs() {
        if (_structDataArray.IsDefaultOrEmpty) { return ImmutableArray<PrimitiveDefinition>.Empty; }

        HashSet<string> usedAliases = [];
        foreach (PrimitiveDefinition primitive in _primitivesArray) { usedAliases.Add(primitive.Alias); }

        SourceProductionContext context = _context;
        StructResolver resolver = new(
            diagnostic => context.ReportDiagnostic(diagnostic.ToDiagnostic()),
            _globalSettings,
            _localSettingsByInterface,
            []
        );

        ImmutableArray<PrimitiveDefinition>.Builder structPrimitives = ImmutableArray.CreateBuilder<PrimitiveDefinition>();

        foreach (StructDefinition structDefinition in _structDataArray) {
            ResolvedStructDefinition? resolved = resolver.Resolve(structDefinition);
            if (resolved is not ResolvedStructDefinition resolvedStruct) { continue; }

            if (!usedAliases.Add(structDefinition.Alias)) {
                context.ReportDiagnostic(new DiagnosticValueType(Diagnostics.DuplicateAlias, structDefinition.Location, structDefinition.Alias).ToDiagnostic());
                continue;
            }

            context.AddSource(
                GeneratedSourceSyntax.GetSourceHintFileName(structDefinition.Namespace, $"{structDefinition.Alias}StructPrimitiveExtensions"),
                StructPrimitiveSourceEmitter.Emit(resolvedStruct)
            );

            string metadataSource = StructMetadataSourceEmitter.Emit(resolvedStruct);
            if (metadataSource.Length > 0) {
                context.AddSource(
                    GeneratedSourceSyntax.GetSourceHintFileName(structDefinition.Namespace, $"{structDefinition.Alias}StructMetadata"),
                    metadataSource
                );
            }

            structPrimitives.Add(StructPrimitiveDefinitionFactory.Create(resolvedStruct));
        }

        return structPrimitives.ToImmutable();
    }
}
