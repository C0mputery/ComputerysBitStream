using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator;

internal sealed class ExecutionContext(SourceProductionContext context, AllCollectedData data) {
    private readonly ImmutableArray<SettingsData> _globalSettingsDataArray = data.GlobalSettings;
    private readonly ImmutableArray<SettingsData> _settingsDataArray = data.Settings;
    private readonly ImmutableArray<RawData> _rawTypesArray = data.RawTypes;
    private readonly ImmutableArray<StructData> _structDataArray = data.Structs;
    private readonly SettingsData _fallbackGlobalSetting = data.FallbackGlobalSetting;

    private readonly List<ParsedRawData> _parsedRawTypes = [];
    private ParsedSettingsData _parsedGlobalSettings = default;
    private Dictionary<string, ParsedSettingsData> ParsedSettings = new();

    private ParsedRawData ParseRawData(RawData raw) {
        HashSet<BitStreamRawRole> seenRoles = [];
        foreach (RawMethodData method in raw.Methods) {
            if (!seenRoles.Add(method.Role)) { context.ReportDiagnostic(Diagnostics.CreateDuplicateRole(method.Location?.ToLocation(), method.Role)); }
        }

        Dictionary<BitStreamRawRole, RawMethodData> methodsByRole = new Dictionary<BitStreamRawRole, RawMethodData>();
        foreach (RawMethodData method in raw.Methods) { methodsByRole[method.Role] = method; }

        return new ParsedRawData(
            TargetTypeFullyQualifiedName: raw.TargetTypeFullyQualifiedName,
            Alias: raw.Alias,
            Size: raw.Size,
            Methods: methodsByRole,
            Location: raw.Location
        );
    }

    private ParsedSettingsData CreateParsedSettingsData(SettingsData settings) {
        Dictionary<string, ParsedRawData> includedRawTypes = new Dictionary<string, ParsedRawData>();
        foreach (RawData rawType in settings.RawTypes) {
            ParsedRawData parsedRawType = ParseRawData(rawType);
            if (!includedRawTypes.ContainsKey(parsedRawType.TargetTypeFullyQualifiedName)) {
                includedRawTypes[parsedRawType.TargetTypeFullyQualifiedName] = parsedRawType;
            }
        }

        Dictionary<string, StructData> includedLocalStructs = new Dictionary<string, StructData>();
        foreach (StructData structData in settings.Structs) {
            if (!includedLocalStructs.ContainsKey(structData.TypeFullyQualifiedName)) {
                includedLocalStructs[structData.TypeFullyQualifiedName] = structData;
            }
        }

        Dictionary<string, ExternalStructData> includedExternalStructs = new Dictionary<string, ExternalStructData>();
        foreach (ExternalStructData externalStruct in settings.ExternalStructs) {
            if (!includedExternalStructs.ContainsKey(externalStruct.TypeFullyQualifiedName)) {
                includedExternalStructs[externalStruct.TypeFullyQualifiedName] = externalStruct;
            }
        }

        return new ParsedSettingsData(
            InterfaceFullyQualifiedName: settings.InterfaceFullyQualifiedName,
            IncludedRawTypes: includedRawTypes,
            IncludedLocalStructs: includedLocalStructs,
            IncludedExternalStructs: includedExternalStructs,
            Location: settings.Location
        );
    }

    private void ReportDiagnostics(EquatableImmutableArray<DiagnosticData> diagnostics) {
        if (diagnostics.IsDefaultOrEmpty) return;
        foreach (DiagnosticData diagnostic in diagnostics) {
            Diagnostic? instance = Diagnostics.Create(diagnostic);
            if (instance is not null) context.ReportDiagnostic(instance);
        }
    }

    public void Run() {
        foreach (RawData rawType in _rawTypesArray) { ReportDiagnostics(rawType.Diagnostics); }
        foreach (SettingsData settingsData in _globalSettingsDataArray) { ReportDiagnostics(settingsData.Diagnostics); }
        foreach (SettingsData settingsData in _settingsDataArray) { ReportDiagnostics(settingsData.Diagnostics); }
        foreach (StructData structData in _structDataArray) { ReportDiagnostics(structData.Diagnostics); }
        ReportDiagnostics(_fallbackGlobalSetting.Diagnostics);
        
        foreach (RawData rawType in _rawTypesArray) { _parsedRawTypes.Add(ParseRawData(rawType)); }
        
        if (_globalSettingsDataArray.Length > 1) {
            foreach (SettingsData additionalGlobalSetting in _globalSettingsDataArray) {
                context.ReportDiagnostic(Diagnostics.CreateMultipleGlobalSettings(additionalGlobalSetting.Location?.ToLocation()));
            }
        }
        _parsedGlobalSettings = CreateParsedSettingsData(_fallbackGlobalSetting);
        ParsedSettings = new Dictionary<string, ParsedSettingsData>(_settingsDataArray.Length);
        foreach (SettingsData settingsData in _settingsDataArray) {
            ParsedSettingsData parsedSettingsData = CreateParsedSettingsData(settingsData);
            ParsedSettings[parsedSettingsData.InterfaceFullyQualifiedName] = parsedSettingsData;
        }
        
        EmitRawMethods();
        ProcessAndEmitStructs();
    }

    private void EmitRawMethods() {
        ParsedRawData? globalIntHandler = null;
        if (_parsedGlobalSettings.IncludedRawTypes.TryGetValue("int", out ParsedRawData foundGlobalIntHandler)) { globalIntHandler = foundGlobalIntHandler; }
        
        Dictionary<string, ParsedRawData> rawAliases = new();
        foreach (ParsedRawData rawType in _parsedRawTypes) {
            if (rawAliases.ContainsKey(rawType.Alias)) {
                context.ReportDiagnostic(Diagnostics.CreateDuplicateAlias(rawType.Location?.ToLocation(), rawType.Alias));
                continue;
            }

            rawAliases[rawType.Alias] = rawType;
            context.AddSource($"{rawType.Alias}ContextExtensions.g.cs", RawTypeWrapperSourceEmitter.EmitSource(rawType, globalIntHandler));
        }
    }

    private readonly Dictionary<string, ParsedStructData?> _computedStructs = new();
    private readonly HashSet<string> _computingStructs = new();

    private ParsedSettingsData GetEffectiveSettings(StructData structData) {
        if (structData.SettingsInterfaceFullyQualifiedName != null) {
            if (ParsedSettings.TryGetValue(structData.SettingsInterfaceFullyQualifiedName, out ParsedSettingsData customSettings)) {
                return customSettings;
            }
        }
        return _parsedGlobalSettings;
    }

    private ParsedStructData? ComputeStruct(StructData structData) {
        if (_computedStructs.TryGetValue(structData.TypeFullyQualifiedName, out ParsedStructData? cached)) {
            return cached;
        }

        if (string.IsNullOrEmpty(structData.TypeFullyQualifiedName)) {
            return null;
        }

        if (!_computingStructs.Add(structData.TypeFullyQualifiedName)) {
            context.ReportDiagnostic(Diagnostics.CreateCyclicStructReference(structData.Location?.ToLocation(), structData.TypeFullyQualifiedName));
            _computedStructs[structData.TypeFullyQualifiedName] = null;
            return null;
        }

        ParsedSettingsData effectiveSettings = GetEffectiveSettings(structData);

        List<ResolvedStructMember> resolvedMembers = [];
        foreach (StructMemberData member in structData.Members) {
            bool resolved = false;
            string alias = "";
            int size = 0;
            bool isFixedSize = true;

            if (effectiveSettings.IncludedRawTypes.TryGetValue(member.TypeFullyQualifiedFormat, out ParsedRawData rawData)) {
                alias = rawData.Alias;
                size = rawData.Size;
                isFixedSize = true;
                resolved = true;
            } else if (effectiveSettings.IncludedExternalStructs.TryGetValue(member.TypeFullyQualifiedFormat, out ExternalStructData externalStructData)) {
                alias = externalStructData.Alias;
                size = externalStructData.Size;
                isFixedSize = externalStructData.FixedSize;
                resolved = true;
            } else if (effectiveSettings.IncludedLocalStructs.TryGetValue(member.TypeFullyQualifiedFormat, out StructData nestedStructData)) {
                ParsedStructData? nestedParsed = ComputeStruct(nestedStructData);
                if (nestedParsed.HasValue) {
                    alias = nestedParsed.Value.Alias;
                    size = nestedParsed.Value.FixedSize;
                    isFixedSize = nestedParsed.Value.IsFixedSize;
                    resolved = true;
                }
            }

            if (resolved) {
                resolvedMembers.Add(new ResolvedStructMember(
                    MemberName: member.MemberName,
                    TypeFullyQualifiedName: member.TypeFullyQualifiedFormat,
                    Alias: alias,
                    Size: size,
                    IsFixedSize: isFixedSize
                ));
            } else {
                context.ReportDiagnostic(Diagnostics.CreateStructMemberNotSerializable(structData.Location?.ToLocation(), member.TypeFullyQualifiedFormat, effectiveSettings.InterfaceFullyQualifiedName));
            }
        }

        _computingStructs.Remove(structData.TypeFullyQualifiedName);

        if (resolvedMembers.Count == 0) {
            context.ReportDiagnostic(Diagnostics.CreateStructNoSerializableMembers(structData.Location?.ToLocation(), structData.TypeFullyQualifiedName));
            _computedStructs[structData.TypeFullyQualifiedName] = null;
            return null;
        }

        bool aggregateIsFixedSize = true;
        int aggregateFixedSize = 0;
        foreach (ResolvedStructMember member in resolvedMembers) {
            aggregateIsFixedSize &= member.IsFixedSize;
            aggregateFixedSize += member.Size;
        }

        ParsedStructData result = new ParsedStructData(
            TypeFullyQualifiedName: structData.TypeFullyQualifiedName,
            Alias: structData.Alias,
            Accessibility: structData.Accessibility,
            IsFixedSize: aggregateIsFixedSize,
            FixedSize: aggregateFixedSize,
            Members: resolvedMembers.ToImmutableArray()
        );

        _computedStructs[structData.TypeFullyQualifiedName] = result;
        return result;
    }

    private void ProcessAndEmitStructs() {
        HashSet<string> usedAliases = new();
        foreach (ParsedRawData rawType in _parsedRawTypes) {
            usedAliases.Add(rawType.Alias);
        }

        foreach (StructData structData in _structDataArray) {
            ComputeStruct(structData);
        }

        foreach (StructData structData in _structDataArray) {
            if (!_computedStructs.TryGetValue(structData.TypeFullyQualifiedName, out ParsedStructData? parsedStructData) || !parsedStructData.HasValue) {
                continue;
            }

            if (!usedAliases.Add(structData.Alias)) {
                context.ReportDiagnostic(Diagnostics.CreateDuplicateAliasStruct(structData.Location?.ToLocation(), structData.Alias));
                continue;
            }

            ParsedSettingsData effectiveSettings = GetEffectiveSettings(structData);
            ParsedRawData? intHandler = _parsedGlobalSettings.IncludedRawTypes.TryGetValue("int", out ParsedRawData globalInt)
                ? globalInt
                : null;
            if (effectiveSettings.IncludedRawTypes.TryGetValue("int", out ParsedRawData effectiveInt)) {
                intHandler = effectiveInt;
            }

            SourceText source = StructWrapperSourceEmitter.EmitSource(parsedStructData.Value, intHandler);
            context.AddSource($"{structData.Alias}StructContextExtensions.g.cs", source);

            SourceText metadataSource = StructMetadataSourceEmitter.EmitSource(parsedStructData.Value, structData.IsProxyClass, structData.DeclarationTypeFullyQualifiedName);
            context.AddSource($"{structData.Alias}StructMetadata.g.cs", metadataSource);
        }
    }
}
