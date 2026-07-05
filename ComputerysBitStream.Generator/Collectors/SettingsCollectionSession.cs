using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Collectors;

internal readonly ref struct SettingsCollectionSession(Compilation compilation) {
    internal static Collected<SettingsDefinition> CollectSettingsData(Compilation compilation, ITypeSymbol configInterfaceSymbol, Location? location) {
        return new SettingsCollectionSession(compilation).CollectSettingsData(configInterfaceSymbol, location);
    }

    internal static Collected<SettingsReference?> CollectSettingsReference(Compilation compilation, ImmutableArray<ITypeSymbol> settingsInterfaces, Location? location) {
        return new SettingsCollectionSession(compilation).CollectSettingsReference(settingsInterfaces, location);
    }

    internal static Collected<SettingsDefinition> MergeSettingsInterfaces(Compilation compilation, ImmutableArray<ITypeSymbol> settingsInterfaces, Location? location) {
        return new SettingsCollectionSession(compilation).MergeSettingsInterfaces(settingsInterfaces, location);
    }

    private readonly HashSet<string> _activeInterfaces = [];

    private Collected<SettingsDefinition> CollectSettingsData(ITypeSymbol configInterfaceSymbol, Location? location) {
        if (!TryEnter(configInterfaceSymbol)) {
            return new Collected<SettingsDefinition>(
                CreateEmptySettings(location), ImmutableArray.Create(new DiagnosticValueType(Diagnostics.CircularSettingsReference, location, configInterfaceSymbol.Name))
            );
        }

        try { return CollectSettingsDataCore(configInterfaceSymbol, location); }
        finally { Exit(configInterfaceSymbol); }
    }

    private Collected<SettingsReference?> CollectSettingsReference(ImmutableArray<ITypeSymbol> settingsInterfaces, Location? location) {
        if (settingsInterfaces.IsEmpty) { return new Collected<SettingsReference?>(null, ImmutableArray<DiagnosticValueType>.Empty); }

        ImmutableArray<string>.Builder localInterfaceNames = ImmutableArray.CreateBuilder<string>();
        ImmutableArray<ITypeSymbol>.Builder externalInterfaces = ImmutableArray.CreateBuilder<ITypeSymbol>();
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        HashSet<string> seenLocalInterfaces = [];

        foreach (ITypeSymbol settingsInterface in settingsInterfaces) {
            if (!settingsInterface.HasAttribute(BitStreamMetadataNames.Settings)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidSettingsInterface, location, settingsInterface.Name));
                continue;
            }

            if (settingsInterface.IsDefinedIn(compilation) || IsActive(settingsInterface)) {
                string interfaceName = settingsInterface.GetFullyQualifiedName();
                if (seenLocalInterfaces.Add(interfaceName)) { localInterfaceNames.Add(interfaceName); }
            }
            else { externalInterfaces.Add(settingsInterface); }
        }

        SettingsDefinition? externalSettings = null;
        if (externalInterfaces.Count > 0) {
            Collected<SettingsDefinition> mergedExternal = MergeSettingsInterfaces(externalInterfaces.ToImmutable(), location);
            diagnostics.AddRange(mergedExternal.Diagnostics);
            if (mergedExternal.IsValid) { externalSettings = mergedExternal.Value; }
        }

        ImmutableArray<DiagnosticValueType> diagnosticArray = diagnostics.ToImmutable();
        if (!Collected<SettingsReference?>.IsValidDiagnostics(diagnosticArray)) {
            return new Collected<SettingsReference?>(null, diagnosticArray);
        }

        SettingsReference settingsReference = new(
            LocalSettingsInterfaceFullyQualifiedNames: localInterfaceNames.ToImmutable(),
            ExternalSettings: externalSettings,
            Location: location
        );
        return new Collected<SettingsReference?>(settingsReference, diagnosticArray);
    }

    private Collected<SettingsDefinition> MergeSettingsInterfaces(ImmutableArray<ITypeSymbol> settingsInterfaces, Location? location) {
        Dictionary<string, PrimitiveDefinition> includedPrimitives = new();
        Dictionary<string, StructDefinition> localStructs = new();
        Dictionary<string, ExternalStructDefinition> externalStructs = new();
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        HashSet<string> seenInterfaces = [];
        ImmutableArray<string>.Builder mergedNamesBuilder = ImmutableArray.CreateBuilder<string>();

        foreach (ITypeSymbol settingsInterface in settingsInterfaces) {
            if (!settingsInterface.HasAttribute(BitStreamMetadataNames.Settings)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidSettingsInterface, location, settingsInterface.Name));
                continue;
            }

            string interfaceName = settingsInterface.GetFullyQualifiedName();
            if (!seenInterfaces.Add(interfaceName)) { continue; }

            Collected<SettingsDefinition> collected = CollectSettingsData(settingsInterface, location);
            diagnostics.AddRange(collected.Diagnostics);

            if (!collected.IsValid) { continue; }

            mergedNamesBuilder.Add(interfaceName);
            MergeUniquePrimitives(includedPrimitives, collected.Value.Primitives, diagnostics);
            MergeUniqueStructs(localStructs, collected.Value.Structs, diagnostics);
            MergeUniqueExternalStructs(externalStructs, collected.Value.ExternalStructs, diagnostics);
        }

        SettingsDefinition settings = new(
            InterfaceFullyQualifiedNames: mergedNamesBuilder.ToImmutable(),
            Primitives: includedPrimitives.ToImmutableDictionary(),
            Structs: localStructs.ToImmutableDictionary(),
            ExternalStructs: externalStructs.ToImmutableDictionary(),
            Location: location
        );
        return new Collected<SettingsDefinition>(settings, diagnostics.ToImmutable());
    }

    private Collected<SettingsDefinition> CollectSettingsDataCore(ITypeSymbol configInterfaceSymbol, Location? location) {
        ImmutableArray<ITypeSymbol>.Builder symbolsToInspect = ImmutableArray.CreateBuilder<ITypeSymbol>();
        symbolsToInspect.AddRange(configInterfaceSymbol.AllInterfaces);
        symbolsToInspect.Add(configInterfaceSymbol);

        Dictionary<string, PrimitiveDefinition> includedPrimitives = new();
        Dictionary<string, StructDefinition> localStructs = new();
        Dictionary<string, ExternalStructDefinition> externalStructs = new();
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        HashSet<string> seenSerializerTypes = [];

        foreach (ITypeSymbol symbol in symbolsToInspect) {
            foreach (AttributeData attributeData in symbol.GetAttributes()) {
                if (!attributeData.IsAttribute(BitStreamMetadataNames.Serializer)) { continue; }

                if (!attributeData.TryGetValue("type", out INamedTypeSymbol? serializerSymbol)) {
                    diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidAttributeArgument, attributeData.GetLocation(), "type", "BitStreamSerializer"));
                    continue;
                }

                string typeName = serializerSymbol.GetFullyQualifiedName();

                if (!seenSerializerTypes.Add(typeName)) {
                    diagnostics.Add(new DiagnosticValueType(GetDuplicateIncludedDiagnostic(serializerSymbol), attributeData.GetLocation(), typeName));
                    continue;
                }

                if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.Primitive, out AttributeData? primitiveAttribute)) {
                    Collected<PrimitiveDefinition> collectedPrimitive = PrimitiveCollector.CollectPrimitiveData(primitiveAttribute, serializerSymbol, compilation, includeSettings: false);
                    diagnostics.AddRange(collectedPrimitive.Diagnostics);
                    if (collectedPrimitive.IsValid) {
                        string primitiveKey = collectedPrimitive.Value.ExtensionClassFullyQualifiedName;
                        if (includedPrimitives.ContainsKey(primitiveKey)) {
                            diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedPrimitive, attributeData.GetLocation(), primitiveKey));
                        }
                        else {
                            includedPrimitives[primitiveKey] = collectedPrimitive.Value;
                        }
                    }
                    continue;
                }

                if (serializerSymbol.IsDefinedIn(compilation)) {
                    if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.Struct, out AttributeData? structAttribute)) {
                        Collected<StructDefinition> collectedStruct = StructCollector.CollectStructData(structAttribute, serializerSymbol, compilation, includeSettings: false);
                        diagnostics.AddRange(collectedStruct.Diagnostics);
                        if (collectedStruct.IsValid) {
                            string structKey = collectedStruct.Value.TypeFullyQualifiedName;
                            if (localStructs.ContainsKey(structKey)) {
                                diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedStruct, attributeData.GetLocation(), structKey));
                            }
                            else {
                                localStructs[structKey] = collectedStruct.Value;
                            }
                        }
                        continue;
                    }

                    if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.ProxyStruct, out AttributeData? proxyAttribute)) {
                        Collected<StructDefinition> collectedProxyStruct = StructCollector.CollectProxyStructData(proxyAttribute, serializerSymbol, compilation, includeSettings: false);
                        diagnostics.AddRange(collectedProxyStruct.Diagnostics);
                        if (collectedProxyStruct.IsValid) {
                            string structKey = collectedProxyStruct.Value.TypeFullyQualifiedName;
                            if (localStructs.ContainsKey(structKey)) {
                                diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedStruct, attributeData.GetLocation(), structKey));
                            }
                            else {
                                localStructs[structKey] = collectedProxyStruct.Value;
                            }
                        }
                        continue;
                    }
                }
                else if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.StructMetadata, out AttributeData? metadataAttribute)) {
                    if (!metadataAttribute.TryGetValue("size", out int size)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidAttributeArgument, metadataAttribute.GetLocation(), "size", "BitStreamStructMetadata"));
                        continue;
                    }

                    if (!StructMetadataConstants.IsValidSize(size)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidStructMetadataSize, metadataAttribute.GetLocation(), size.ToString()));
                        continue;
                    }

                    INamedTypeSymbol resolvedTypeSymbol = serializerSymbol;
                    string resolvedAlias = DisplayNameUtility.GetDisplayName(serializerSymbol);
                    if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.ProxyStruct, out AttributeData? proxyAttribute)
                        && proxyAttribute.TryGetConstructorArgumentByName("targetStruct", out TypedConstant targetStructArgument)
                        && targetStructArgument.Value is INamedTypeSymbol targetStruct) {
                        resolvedTypeSymbol = targetStruct;
                        resolvedAlias = DisplayNameUtility.GetDisplayName(targetStruct);
                    }

                    ExternalStructDefinition externalStruct = new(
                        TypeFullyQualifiedName: resolvedTypeSymbol.GetFullyQualifiedName(),
                        Alias: resolvedAlias,
                        Size: size,
                        ExtensionNamespace: serializerSymbol.GetFullyQualifiedNamespace()
                    );
                    if (externalStructs.ContainsKey(externalStruct.TypeFullyQualifiedName)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedExternalStruct, attributeData.GetLocation(), externalStruct.TypeFullyQualifiedName));
                    }
                    else {
                        externalStructs[externalStruct.TypeFullyQualifiedName] = externalStruct;
                    }
                    continue;
                }

                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidSettingType, attributeData.GetLocation(), serializerSymbol.Name));
            }
        }

        SettingsDefinition settings = new(
            InterfaceFullyQualifiedNames: ImmutableArray.Create(configInterfaceSymbol.GetFullyQualifiedName()),
            Primitives: includedPrimitives.ToImmutableDictionary(),
            Structs: localStructs.ToImmutableDictionary(),
            ExternalStructs: externalStructs.ToImmutableDictionary(),
            Location: location
        );
        return new Collected<SettingsDefinition>(settings, diagnostics.ToImmutable());
    }

    private bool TryEnter(ITypeSymbol settingsInterface) => _activeInterfaces.Add(settingsInterface.GetFullyQualifiedName());
    private void Exit(ITypeSymbol settingsInterface) => _activeInterfaces.Remove(settingsInterface.GetFullyQualifiedName());
    private bool IsActive(ITypeSymbol settingsInterface) => _activeInterfaces.Contains(settingsInterface.GetFullyQualifiedName());

    private static SettingsDefinition CreateEmptySettings(ValueTypeLocation? location = null) {
        return new SettingsDefinition(
            InterfaceFullyQualifiedNames: [],
            Primitives: ImmutableDictionary<string, PrimitiveDefinition>.Empty,
            Structs: ImmutableDictionary<string, StructDefinition>.Empty,
            ExternalStructs: ImmutableDictionary<string, ExternalStructDefinition>.Empty,
            Location: location
        );
    }

    private static void MergeUniquePrimitives(Dictionary<string, PrimitiveDefinition> destination, EquatableImmutableDictionary<string, PrimitiveDefinition> source, ImmutableArray<DiagnosticValueType>.Builder diagnostics) {
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in source) {
            if (destination.ContainsKey(pair.Key)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedPrimitive, pair.Value.Location, pair.Value.ExtensionClassFullyQualifiedName));
                continue;
            }

            destination[pair.Key] = pair.Value;
        }
    }

    private static void MergeUniqueStructs(Dictionary<string, StructDefinition> destination, EquatableImmutableDictionary<string, StructDefinition> source, ImmutableArray<DiagnosticValueType>.Builder diagnostics) {
        foreach (KeyValuePair<string, StructDefinition> pair in source) {
            if (destination.ContainsKey(pair.Key)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedStruct, pair.Value.Location, pair.Key));
                continue;
            }

            destination[pair.Key] = pair.Value;
        }
    }

    private static void MergeUniqueExternalStructs(Dictionary<string, ExternalStructDefinition> destination, EquatableImmutableDictionary<string, ExternalStructDefinition> source, ImmutableArray<DiagnosticValueType>.Builder diagnostics) {
        foreach (KeyValuePair<string, ExternalStructDefinition> pair in source) {
            if (destination.ContainsKey(pair.Key)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateIncludedExternalStruct, null, pair.Key));
                continue;
            }

            destination[pair.Key] = pair.Value;
        }
    }

    private DiagnosticDescriptor GetDuplicateIncludedDiagnostic(INamedTypeSymbol serializerSymbol) {
        if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.Primitive, out _)) { return Diagnostics.DuplicateIncludedPrimitive; }

        if (serializerSymbol.IsDefinedIn(compilation)
            && (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.Struct, out _) || serializerSymbol.TryGetAttribute(BitStreamMetadataNames.ProxyStruct, out _))) {
            return Diagnostics.DuplicateIncludedStruct;
        }

        if (serializerSymbol.TryGetAttribute(BitStreamMetadataNames.StructMetadata, out _)) { return Diagnostics.DuplicateIncludedExternalStruct; }

        return Diagnostics.InvalidSettingType;
    }
}
