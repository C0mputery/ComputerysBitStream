using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator.Collectors;

internal static class SettingsCollector {
    public static IncrementalValuesProvider<Collected<SettingsDefinition>> GetGlobalSettingsData(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<ImmutableArray<Collected<SettingsDefinition>>> globalSettings = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: BitStreamTypeNames.DefaultSettings,
            predicate: (SyntaxNode node, CancellationToken _) => node is CompilationUnitSyntax,
            transform: GlobalSettingsAttributeDataTransform
        );
        return globalSettings.SelectMany((array, _) => array);
    }

    private static ImmutableArray<Collected<SettingsDefinition>> GlobalSettingsAttributeDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        ImmutableArray<Collected<SettingsDefinition>>.Builder builder = ImmutableArray.CreateBuilder<Collected<SettingsDefinition>>();
        foreach (AttributeData attributeData in context.Attributes) { builder.Add(CollectGlobalSettingsData(attributeData, context)); }
        return builder.ToImmutable();
    }

    private static Collected<SettingsDefinition> CollectGlobalSettingsData(AttributeData attributeData, GeneratorAttributeSyntaxContext context) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        Location? attributeLocation = attributeData.GetLocation();
        ImmutableArray<ITypeSymbol> settingsInterfaces = attributeData.TryGetConstructorArgumentByName("settings", out TypedConstant settingsArgument) ? TypedConstantUtility.ExtractTypeSymbols(settingsArgument) : ImmutableArray<ITypeSymbol>.Empty;

        ImmutableArray<ITypeSymbol>.Builder validSettingsInterfaces = ImmutableArray.CreateBuilder<ITypeSymbol>();
        foreach (ITypeSymbol settingsInterface in settingsInterfaces) {
            if (!settingsInterface.HasAttribute(BitStreamTypeNames.Settings)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidSettingsInterface, attributeLocation, settingsInterface.Name));
                continue;
            }

            validSettingsInterfaces.Add(settingsInterface);
        }

        Collected<SettingsDefinition> merged = SettingsCollectionSession.MergeSettingsInterfaces(context.SemanticModel.Compilation, validSettingsInterfaces.ToImmutable(), attributeLocation);
        diagnostics.AddRange(merged.Diagnostics);
        return new Collected<SettingsDefinition>(merged.Value, diagnostics.ToImmutable());
    }

    public static Collected<SettingsDefinition> GetFallbackGlobalSettings(Compilation compilation, CancellationToken _) {
        INamedTypeSymbol? defaultSettingsSymbol = compilation.GetTypeByMetadataName(BitStreamTypeNames.DefaultSettingsInterface);
        if (defaultSettingsSymbol is not null) { return SettingsCollectionSession.CollectSettingsData(compilation, defaultSettingsSymbol, null); }
        return new Collected<SettingsDefinition>(CreateEmptySettings(), ImmutableArray<DiagnosticValueType>.Empty);
    }

    private static SettingsDefinition CreateEmptySettings() {
        return new SettingsDefinition(
            InterfaceFullyQualifiedNames: [],
            Primitives: ImmutableDictionary<string, PrimitiveDefinition>.Empty,
            Structs: ImmutableDictionary<string, StructDefinition>.Empty,
            ExternalStructs: ImmutableDictionary<string, ExternalStructDefinition>.Empty,
            Location: null
        );
    }

    public static IncrementalValuesProvider<Collected<SettingsDefinition>> GetSettingsData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: BitStreamTypeNames.Settings,
            predicate: (SyntaxNode node, CancellationToken _) => node is InterfaceDeclarationSyntax,
            transform: SettingsAttributeDataTransform
        );
    }

    private static Collected<SettingsDefinition> SettingsAttributeDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        return SettingsCollectionSession.CollectSettingsData(context.SemanticModel.Compilation, (ITypeSymbol)context.TargetSymbol, context.TargetSymbol.Locations.FirstOrDefault());
    }
}
