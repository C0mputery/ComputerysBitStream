using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator;

internal static class SettingsCollector {
    private static readonly string DefaultSettingsAttribute = typeof(DefaultBitStreamSettingsAttribute).FullName!;
    private static readonly string SettingsAttribute = typeof(BitStreamSettingsAttribute).FullName!;
    private static readonly string SettingAttribute = typeof(BitStreamSettingAttribute).FullName!;

    public static IncrementalValuesProvider<SettingsData> GetGlobalSettingsData(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<ImmutableArray<SettingsData>> globalSettings = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: DefaultSettingsAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is CompilationUnitSyntax,
            transform: GetGlobalSettingsDataTransform
        );
        IncrementalValuesProvider<SettingsData> flattened = globalSettings.SelectMany((array, cancellationToken) => array);
        return flattened;
    }

    private static ImmutableArray<SettingsData> GetGlobalSettingsDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        ImmutableArray<SettingsData>.Builder builder = ImmutableArray.CreateBuilder<SettingsData>();
        foreach (AttributeData attributeData in context.Attributes) {
            SettingsData settingsData = HandleSingleGlobalSettingData(attributeData, context);
            builder.Add(settingsData);
        }
        return builder.ToImmutable();
    }

    private static SettingsData HandleSingleGlobalSettingData(AttributeData attributeData, GeneratorAttributeSyntaxContext context) {
        ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
        ITypeSymbol configInterfaceSymbol = (ITypeSymbol)constructorArguments[0].Value!;

        SettingsData settings = ProcessInterface(configInterfaceSymbol, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), context.SemanticModel.Compilation);
        
        List<DiagnosticData> diagnostics = new List<DiagnosticData>(settings.Diagnostics);
        if (!configInterfaceSymbol.HasAttribute(SettingsAttribute)) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.MissingSettingsAttribute, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [configInterfaceSymbol.Name]));
        }
        
        return settings with { Diagnostics = diagnostics.ToImmutableArray() };
    }

    public static IncrementalValuesProvider<SettingsData> GetSettingsData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: SettingsAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is InterfaceDeclarationSyntax,
            transform: GetSettingsDataTransform
        );
    }

    private static SettingsData GetSettingsDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        return ProcessInterface((ITypeSymbol)context.TargetSymbol, context.TargetSymbol.Locations.FirstOrDefault(), context.SemanticModel.Compilation);
    }

    public static SettingsData ProcessInterface(ITypeSymbol configInterfaceSymbol, Location? location, Compilation compilation) {
        List<ITypeSymbol> symbolsToInspect = [..configInterfaceSymbol.AllInterfaces, configInterfaceSymbol];

        List<RawData> includedRawTypes = [];
        List<StructData> localStructs = [];
        List<ExternalStructData> externalStructs = [];
        List<DiagnosticData> diagnostics = [];
        foreach (ITypeSymbol symbol in symbolsToInspect) {
            HashSet<string> seenTypesInThisSymbol = new();
            foreach (AttributeData attributeData in symbol.GetAttributes()) {
                if (!attributeData.IsAttribute(SettingAttribute)) { continue; }

                ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
                INamedTypeSymbol rawTypeSymbol = (INamedTypeSymbol)constructorArguments[0].Value!;
                string typeName = rawTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                if (!seenTypesInThisSymbol.Add(typeName)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.DuplicateIncludedRawType, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [rawTypeSymbol.Name]));
                    continue;
                }

                ParseAttributeData(compilation, rawTypeSymbol, includedRawTypes, localStructs, externalStructs, diagnostics, attributeData);
            }

        }

        return new SettingsData(
            InterfaceFullyQualifiedName: configInterfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            RawTypes: includedRawTypes.ToImmutableArray(),
            Structs: localStructs.ToImmutableArray(),
            ExternalStructs: externalStructs.ToImmutableArray(),
            Location: location,
            Diagnostics: diagnostics.ToImmutableArray()
        );
    }

    private static void ParseAttributeData(Compilation compilation, INamedTypeSymbol targetedTypeSymbol, List<RawData> includedRawTypes, List<StructData> localStructs, List<ExternalStructData> externalStructs, List<DiagnosticData> diagnostics, AttributeData attributeData) {
        ImmutableArray<AttributeData> attributeDataArray = targetedTypeSymbol.GetAttributes();
        
        foreach (AttributeData? targetedTypeAttributes in attributeDataArray) {
            string? fullyQualifiedWithoutGlobal = targetedTypeAttributes.AttributeClass?.GetFullyQualifiedWithoutGlobalFormat();
            if (fullyQualifiedWithoutGlobal == null) { continue; }

            if (fullyQualifiedWithoutGlobal == RawTypeCollector.RawTypeAttribute) {
                includedRawTypes.Add(RawTypeCollector.RawTypeAttributeData(targetedTypeAttributes, targetedTypeSymbol, compilation));
                return;
            }

            if (SymbolEqualityComparer.Default.Equals(targetedTypeSymbol.ContainingAssembly, compilation.Assembly)) {
                if (fullyQualifiedWithoutGlobal == StructCollector.StructAttribute) {
                    localStructs.Add(StructCollector.ParseStructData(targetedTypeAttributes, targetedTypeSymbol));
                    return;
                }

                if (fullyQualifiedWithoutGlobal == StructCollector.ProxyStructAttribute) {
                    localStructs.Add(StructCollector.ParseProxyStructData(targetedTypeAttributes, targetedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
                    return;
                }
            }
            else {
                if (fullyQualifiedWithoutGlobal != StructCollector.ExternalStructAttribute) { continue; }

                ImmutableArray<TypedConstant> constructorArguments = targetedTypeAttributes.ConstructorArguments;
                bool fixedSize = constructorArguments.Length > 0 && constructorArguments[0].Value is true;
                int size = constructorArguments.Length > 1 && constructorArguments[1].Value is int sizeValue ? sizeValue : 0;

                INamedTypeSymbol resolvedTypeSymbol = targetedTypeSymbol;
                string resolvedAlias = DisplayNameUtility.GetDisplayName(targetedTypeSymbol);
                foreach (AttributeData attribute in attributeDataArray) {
                    string? attrName = attribute.AttributeClass?.GetFullyQualifiedWithoutGlobalFormat();
                    if (attrName == StructCollector.ProxyStructAttribute) {
                        if (attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is INamedTypeSymbol targetStruct) {
                            resolvedTypeSymbol = targetStruct;
                            resolvedAlias = DisplayNameUtility.GetDisplayName(targetStruct);
                            break;
                        }
                    }
                }

                externalStructs.Add(new ExternalStructData(
                    TypeFullyQualifiedName: resolvedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    Alias: resolvedAlias,
                    FixedSize: fixedSize,
                    Size: size,
                    Location: attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                ));
                return;
            }
        }
                
        diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidSettingType, attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [targetedTypeSymbol.Name]));
    }
}
