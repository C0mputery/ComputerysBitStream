using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator;

internal record struct SettingsData(
    string InterfaceFullyQualifiedName,
    EquatableImmutableArray<RawData> IncludedRawTypes,
    ValueTypeLocation? Location,
    EquatableImmutableArray<DiagnosticData> Diagnostics = default
);

public partial class IncrementalGenerator : IIncrementalGenerator {
    private static readonly string DefaultSettingsAttribute = typeof(DefaultBitStreamSettingsAttribute).FullName!;
    private static readonly string SettingsAttribute = typeof(BitStreamSettingsAttribute).FullName!;
    private static readonly string SettingAttribute = typeof(BitStreamSettingAttribute).FullName!;
    
    private static IncrementalValuesProvider<SettingsData> GetGlobalSettingsData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: DefaultSettingsAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is CompilationUnitSyntax,
            transform: GetGlobalSettingsDataTransform);
    }

    private static SettingsData GetGlobalSettingsDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        AttributeData attributeData = context.Attributes[0];
        ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
        ITypeSymbol configInterfaceSymbol = (ITypeSymbol)constructorArguments[0].Value!;

        List<DiagnosticData> diagnostics = [];
        if (!configInterfaceSymbol.HasAttribute(SettingsAttribute)) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.MissingSettingsAttribute, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [configInterfaceSymbol.Name]));
        }

        SettingsData settings = ProcessInterface(configInterfaceSymbol, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), context.SemanticModel.Compilation);
        return settings with { Diagnostics = diagnostics.ToImmutableArray() };
    }
    
    private static IncrementalValuesProvider<SettingsData> GetSettingsData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: SettingsAttribute, 
            predicate: (SyntaxNode node, CancellationToken _) => node is InterfaceDeclarationSyntax,
            transform: GetSettingsDataTransform);
    }
    private static SettingsData GetSettingsDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) { 
        return ProcessInterface((ITypeSymbol)context.TargetSymbol, context.TargetSymbol.Locations.FirstOrDefault(), context.SemanticModel.Compilation); 
    }
    
    private static SettingsData ProcessInterface(ITypeSymbol configInterfaceSymbol, Location? location, Compilation compilation) {
        List<ITypeSymbol> symbolsToInspect = [configInterfaceSymbol, ..configInterfaceSymbol.AllInterfaces];

        List<RawData> includedRawTypes = [];
        List<DiagnosticData> diagnostics = [];
        foreach (ITypeSymbol symbol in symbolsToInspect) {
            foreach (AttributeData attributeData in symbol.GetAttributes()) {
                if (attributeData.IsAttribute(SettingAttribute)) {
                    ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
                    INamedTypeSymbol rawTypeSymbol = (INamedTypeSymbol)constructorArguments[0].Value!;
            
                    if (rawTypeSymbol.TryGetAttribute(RawTypeAttribute, out AttributeData? rawTypeAttributeData)) { includedRawTypes.Add(RawTypeAttributeData(rawTypeAttributeData, rawTypeSymbol, compilation)); } 
                    else { diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidSettingType, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [rawTypeSymbol.Name])); }
                }
            }

        }

        return new SettingsData(
            InterfaceFullyQualifiedName: configInterfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            IncludedRawTypes: includedRawTypes.ToImmutableArray(),
            Location: location,
            Diagnostics: diagnostics.ToImmutableArray()
        );
    }
}