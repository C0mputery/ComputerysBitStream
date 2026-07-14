using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.Emission;
using ComputerysBitStream.Generator.Roslyn;
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
                CreateEmptySettings(location), ImmutableArray.Create(new DiagnosticValueType(DiagnosticDescriptors.CircularSettingsReference, location, configInterfaceSymbol.Name))
            );
        }

        try { return CollectSettingsDataCore(configInterfaceSymbol, location); }
        finally { Exit(configInterfaceSymbol); }
    }

    private Collected<SettingsDefinition> CollectSettingsDataCore(ITypeSymbol configInterfaceSymbol, Location? location) {
        return CollectSettingsFromInterfaces(
            BuildSymbolsToInspect(ImmutableArray.Create(configInterfaceSymbol)),
            ImmutableArray.Create(configInterfaceSymbol.GetFullyQualifiedName()),
            location
        );
    }

    private Collected<SettingsDefinition> MergeSettingsInterfaces(ImmutableArray<ITypeSymbol> settingsInterfaces, Location? location) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        HashSet<string> seenInterfaces = [];
        ImmutableArray<ITypeSymbol>.Builder interfacesToCollect = ImmutableArray.CreateBuilder<ITypeSymbol>();
        ImmutableArray<ITypeSymbol>.Builder enteredInterfaces = ImmutableArray.CreateBuilder<ITypeSymbol>();
        ImmutableArray<string>.Builder mergedNamesBuilder = ImmutableArray.CreateBuilder<string>();

        foreach (ITypeSymbol settingsInterface in settingsInterfaces) {
            if (!settingsInterface.HasAttribute(BitStreamTypeNames.Settings)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidSettingsInterface, location, settingsInterface.Name));
                continue;
            }

            string interfaceName = settingsInterface.GetFullyQualifiedName();
            if (!seenInterfaces.Add(interfaceName)) { continue; }

            if (!TryEnter(settingsInterface)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.CircularSettingsReference, location, settingsInterface.Name));
                continue;
            }

            enteredInterfaces.Add(settingsInterface);
            interfacesToCollect.Add(settingsInterface);
            mergedNamesBuilder.Add(interfaceName);
        }

        try {
            Collected<SettingsDefinition> collected = CollectSettingsFromInterfaces(
                BuildSymbolsToInspect(interfacesToCollect.ToImmutable()),
                mergedNamesBuilder.ToImmutable(),
                location
            );
            diagnostics.AddRange(collected.Diagnostics);
            return new Collected<SettingsDefinition>(collected.Value, diagnostics.ToImmutable());
        }
        finally {
            foreach (ITypeSymbol settingsInterface in enteredInterfaces) { Exit(settingsInterface); }
        }
    }

    private static ImmutableArray<ITypeSymbol> BuildSymbolsToInspect(ImmutableArray<ITypeSymbol> settingsInterfaces) {
        ImmutableArray<ITypeSymbol>.Builder symbolsToInspect = ImmutableArray.CreateBuilder<ITypeSymbol>();
        HashSet<string> seenSymbols = [];

        for (int i = settingsInterfaces.Length - 1; i >= 0; i--) {
            AppendInterfaceHierarchy(settingsInterfaces[i], symbolsToInspect, seenSymbols);
        }

        return symbolsToInspect.ToImmutable();
    }

    private static void AppendInterfaceHierarchy(
        ITypeSymbol settingsInterface,
        ImmutableArray<ITypeSymbol>.Builder symbolsToInspect,
        HashSet<string> seenSymbols
    ) {
        ImmutableArray<INamedTypeSymbol> baseInterfaces = settingsInterface.Interfaces;
        for (int i = baseInterfaces.Length - 1; i >= 0; i--) {
            AppendInterfaceHierarchy(baseInterfaces[i], symbolsToInspect, seenSymbols);
        }

        if (seenSymbols.Add(settingsInterface.GetFullyQualifiedName())) { symbolsToInspect.Add(settingsInterface); }
    }

    private Collected<SettingsDefinition> CollectSettingsFromInterfaces(
        ImmutableArray<ITypeSymbol> symbolsToInspect,
        ImmutableArray<string> interfaceFullyQualifiedNames,
        Location? location
    ) {
        Dictionary<string, PrimitiveDefinition> includedPrimitives = new();
        Dictionary<(string TargetType, PrimitiveSerializationMode Mode), string> primitiveKeysByTargetAndMode = new();
        Dictionary<string, StructDefinition> localStructs = new();
        Dictionary<string, ExternalStructDefinition> externalStructs = new();
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();

        foreach (ITypeSymbol symbol in symbolsToInspect) {
            HashSet<string> seenSerializerTypesOnInterface = [];
            HashSet<(string TargetType, PrimitiveSerializationMode Mode)> seenPrimitiveTargetsOnInterface = [];
            HashSet<string> seenStructKeysOnInterface = [];
            HashSet<string> seenExternalStructKeysOnInterface = [];

            foreach (AttributeData attributeData in symbol.GetAttributes()) {
                if (!attributeData.IsAttribute(BitStreamTypeNames.Serializer)) { continue; }

                if (!attributeData.TryGetValue("type", out INamedTypeSymbol? serializerSymbol)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attributeData.GetLocation(), "type", "BitStreamSerializer"));
                    continue;
                }

                string typeName = serializerSymbol.GetFullyQualifiedName();

                if (!seenSerializerTypesOnInterface.Add(typeName)) {
                    diagnostics.Add(new DiagnosticValueType(GetDuplicateIncludedDiagnostic(serializerSymbol), attributeData.GetLocation(), typeName));
                    continue;
                }

                if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.Primitive, out AttributeData? primitiveAttribute)) {
                    Collected<PrimitiveDefinition> collectedPrimitive = PrimitiveCollector.CollectPrimitiveCore(primitiveAttribute, serializerSymbol, compilation);
                    diagnostics.AddRange(collectedPrimitive.Diagnostics);
                    if (collectedPrimitive.IsValid) {
                        PrimitiveDefinition primitive = collectedPrimitive.Value;
                        string primitiveKey = primitive.ExtensionClassFullyQualifiedName;
                        (string TargetType, PrimitiveSerializationMode Mode) primitiveTarget = (primitive.TargetTypeFullyQualifiedName, primitive.Mode);

                        if (!seenPrimitiveTargetsOnInterface.Add(primitiveTarget)) {
                            diagnostics.Add(new DiagnosticValueType(
                                DiagnosticDescriptors.DuplicatePrimitiveTargetAndMode,
                                attributeData.GetLocation(),
                                primitive.TargetTypeFullyQualifiedName,
                                primitive.Mode
                            ));
                        }
                        else {
                            if (primitiveKeysByTargetAndMode.TryGetValue(primitiveTarget, out string? previousKey) && previousKey != primitiveKey) {
                                includedPrimitives.Remove(previousKey);
                            }

                            primitiveKeysByTargetAndMode[primitiveTarget] = primitiveKey;
                            includedPrimitives[primitiveKey] = primitive;
                        }
                    }
                    continue;
                }

                if (serializerSymbol.IsDefinedIn(compilation)) {
                    if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.Struct, out AttributeData? structAttribute)) {
                        Collected<StructDefinition> collectedStruct = StructCollector.CollectIncludedStruct(structAttribute, serializerSymbol, compilation);
                        diagnostics.AddRange(collectedStruct.Diagnostics);
                        if (collectedStruct.IsValid) {
                            string structKey = collectedStruct.Value.TypeFullyQualifiedName;
                            if (!seenStructKeysOnInterface.Add(structKey)) {
                                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateIncludedStruct, attributeData.GetLocation(), structKey));
                            }
                            else {
                                localStructs[structKey] = collectedStruct.Value;
                            }
                        }
                        continue;
                    }

                    if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.ProxyStruct, out AttributeData? proxyAttribute)) {
                        Collected<StructDefinition> collectedProxyStruct = StructCollector.CollectIncludedProxyStruct(proxyAttribute, serializerSymbol, compilation);
                        diagnostics.AddRange(collectedProxyStruct.Diagnostics);
                        if (collectedProxyStruct.IsValid) {
                            string structKey = collectedProxyStruct.Value.TypeFullyQualifiedName;
                            if (!seenStructKeysOnInterface.Add(structKey)) {
                                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateIncludedStruct, attributeData.GetLocation(), structKey));
                            }
                            else {
                                localStructs[structKey] = collectedProxyStruct.Value;
                            }
                        }
                        continue;
                    }
                }
                else if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.StructMetadata, out AttributeData? metadataAttribute)) {
                    if (!metadataAttribute.TryGetValue("size", out int size)) {
                        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, metadataAttribute.GetLocation(), "size", "BitStreamStructMetadata"));
                        continue;
                    }

                    if (!StructMetadataHelper.IsValidSize(size)) {
                        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidStructMetadataSize, metadataAttribute.GetLocation(), size.ToString()));
                        continue;
                    }

                    INamedTypeSymbol resolvedTypeSymbol = serializerSymbol;
                    string resolvedAlias = DisplayNameUtility.GetDisplayName(serializerSymbol);
                    if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.ProxyStruct, out AttributeData? proxyAttribute)
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
                    if (!seenExternalStructKeysOnInterface.Add(externalStruct.TypeFullyQualifiedName)) {
                        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateIncludedExternalStruct, attributeData.GetLocation(), externalStruct.TypeFullyQualifiedName));
                    }
                    else {
                        externalStructs[externalStruct.TypeFullyQualifiedName] = externalStruct;
                    }
                    continue;
                }

                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidSettingType, attributeData.GetLocation(), serializerSymbol.Name));
            }
        }

        SettingsDefinition settings = new(
            InterfaceFullyQualifiedNames: interfaceFullyQualifiedNames,
            Primitives: includedPrimitives.ToImmutableDictionary(),
            Structs: localStructs.ToImmutableDictionary(),
            ExternalStructs: externalStructs.ToImmutableDictionary(),
            Location: location
        );
        return new Collected<SettingsDefinition>(settings, diagnostics.ToImmutable());
    }

    private Collected<SettingsReference?> CollectSettingsReference(ImmutableArray<ITypeSymbol> settingsInterfaces, Location? location) {
        if (settingsInterfaces.IsEmpty) { return new Collected<SettingsReference?>(null, ImmutableArray<DiagnosticValueType>.Empty); }

        ImmutableArray<string>.Builder localInterfaceNames = ImmutableArray.CreateBuilder<string>();
        ImmutableArray<ITypeSymbol>.Builder externalInterfaces = ImmutableArray.CreateBuilder<ITypeSymbol>();
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        HashSet<string> seenLocalInterfaces = [];

        foreach (ITypeSymbol settingsInterface in settingsInterfaces) {
            if (!settingsInterface.HasAttribute(BitStreamTypeNames.Settings)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidSettingsInterface, location, settingsInterface.Name));
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
        if (DiagnosticValueType.HasErrors(diagnosticArray)) {
            return new Collected<SettingsReference?>(null, diagnosticArray);
        }

        SettingsReference settingsReference = new(
            LocalSettingsInterfaceFullyQualifiedNames: localInterfaceNames.ToImmutable(),
            ExternalSettings: externalSettings,
            Location: location
        );
        return new Collected<SettingsReference?>(settingsReference, diagnosticArray);
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

    private DiagnosticDescriptor GetDuplicateIncludedDiagnostic(INamedTypeSymbol serializerSymbol) {
        if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.Primitive, out _)) { return DiagnosticDescriptors.DuplicateIncludedPrimitive; }

        if (serializerSymbol.IsDefinedIn(compilation)
            && (serializerSymbol.TryGetAttribute(BitStreamTypeNames.Struct, out _) || serializerSymbol.TryGetAttribute(BitStreamTypeNames.ProxyStruct, out _))) {
            return DiagnosticDescriptors.DuplicateIncludedStruct;
        }

        if (serializerSymbol.TryGetAttribute(BitStreamTypeNames.StructMetadata, out _)) { return DiagnosticDescriptors.DuplicateIncludedExternalStruct; }

        return DiagnosticDescriptors.InvalidSettingType;
    }
}
