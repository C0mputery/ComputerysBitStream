using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal readonly record struct AllCollectedData(
    EquatableImmutableArray<SettingsData> GlobalSettings,
    EquatableImmutableArray<SettingsData> Settings,
    EquatableImmutableArray<RawData> RawTypes,
    SettingsData FallbackGlobalSetting
);

internal record struct ParsedRawData(
    string TargetTypeFullyQualifiedName,
    string Alias,
    int Size,
    Dictionary<BitStreamRawRole, RawMethodData> Methods,
    ValueTypeLocation? Location
);

internal record struct ParsedSettingsData(
    string InterfaceFullyQualifiedName,
    Dictionary<string, ParsedRawData> IncludedRawTypes,
    ValueTypeLocation? Location
);

[Generator]
public partial class IncrementalGenerator : IIncrementalGenerator {
    private static ParsedRawData ParseRawData(RawData raw) {
        Dictionary<BitStreamRawRole, RawMethodData> methodsByRole = new Dictionary<BitStreamRawRole, RawMethodData>();
        foreach (RawMethodData method in raw.Methods) {
            methodsByRole[method.Role] = method;
        }

        return new ParsedRawData(
            TargetTypeFullyQualifiedName: raw.TargetTypeFullyQualifiedName,
            Alias: raw.Alias,
            Size: raw.Size,
            Methods: methodsByRole,
            Location: raw.Location
        );
    }

    private static ParsedRawData ParseRawDataWithDiagnostics(RawData raw, SourceProductionContext spc) {
        if (!raw.Diagnostics.IsDefaultOrEmpty) {
            foreach (DiagnosticData diagnostic in raw.Diagnostics) {
                Diagnostic? diagnosticInstance = Diagnostics.Create(diagnostic);
                if (diagnosticInstance is not null) { spc.ReportDiagnostic(diagnosticInstance); }
            }
        }

        HashSet<BitStreamRawRole> seenRoles = [];
        foreach (RawMethodData method in raw.Methods) {
            if (!seenRoles.Add(method.Role)) {
                spc.ReportDiagnostic(Diagnostics.CreateDuplicateRole(method.Location?.ToLocation(), method.Role));
            }
        }

        return ParseRawData(raw);
    }
    
    private static ParsedSettingsData CreateParsedSettingsData(SettingsData settings, SourceProductionContext spc) {
        if (settings.Diagnostics.IsDefaultOrEmpty == false) {
            foreach (DiagnosticData diagnostic in settings.Diagnostics) {
                Diagnostic? diagnosticInstance = Diagnostics.Create(diagnostic);
                if (diagnosticInstance is not null) { spc.ReportDiagnostic(diagnosticInstance); }
            }
        }

        Dictionary<string, ParsedRawData> includedRawTypes = new Dictionary<string, ParsedRawData>();
        foreach (RawData rawType in settings.IncludedRawTypes) {
            ParsedRawData parsedRawType = ParseRawData(rawType);
            if (includedRawTypes.ContainsKey(parsedRawType.TargetTypeFullyQualifiedName)) {
                spc.ReportDiagnostic(Diagnostics.CreateDuplicateIncludedRawType(rawType.Location?.ToLocation(), parsedRawType.TargetTypeFullyQualifiedName));
            }
            includedRawTypes[parsedRawType.TargetTypeFullyQualifiedName] = parsedRawType;
        }

        return new ParsedSettingsData(
            InterfaceFullyQualifiedName: settings.InterfaceFullyQualifiedName,
            IncludedRawTypes: includedRawTypes,
            Location: settings.Location
        );
    }

    private static SettingsData GetFallbackGlobalSetting(Compilation compilation, CancellationToken _) {
        INamedTypeSymbol? defaultSettingsSymbol = compilation.GetTypeByMetadataName(typeof(IDefaultSettings).FullName!);
        // No idea how this ccould fail
        return defaultSettingsSymbol is null ? new SettingsData(InterfaceFullyQualifiedName: "", IncludedRawTypes: [], Location: null, Diagnostics: []) : ProcessInterface(defaultSettingsSymbol, null, compilation);
    }

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<SettingsData> globalSerializationSettingData = GetGlobalSettingsData(context);
        IncrementalValueProvider<ImmutableArray<SettingsData>> collectedSettingData = globalSerializationSettingData.Collect();
        
        IncrementalValuesProvider<SettingsData> serializationSettingData = GetSettingsData(context);
        IncrementalValueProvider<ImmutableArray<SettingsData>> collectedSerializationSettingData = serializationSettingData.Collect();
        
        IncrementalValuesProvider<RawData> rawAttributeData = GetBitStreamRawAttributeData(context);
        IncrementalValueProvider<ImmutableArray<RawData>> collectedRawAttributeData = rawAttributeData.Collect();
        
        IncrementalValueProvider<SettingsData> fallbackGlobalSetting = context.CompilationProvider.Select(GetFallbackGlobalSetting);
        
        IncrementalValueProvider<AllCollectedData> allCollectedData = collectedSettingData
            .Combine(collectedSerializationSettingData)
            .Combine(collectedRawAttributeData)
            .Combine(fallbackGlobalSetting)
            .Select(static (data, _) => new AllCollectedData(
                data.Left.Left.Left,
                data.Left.Left.Right,
                data.Left.Right,
                data.Right
            ));
        
        context.RegisterSourceOutput(allCollectedData, Execute);
    }
    
    private static void Execute(SourceProductionContext spc, AllCollectedData data) {
        ImmutableArray<SettingsData> globalSettings = data.GlobalSettings;
        ImmutableArray<SettingsData> settings = data.Settings;
        ImmutableArray<RawData> rawTypes = data.RawTypes;
        
        List<ParsedRawData> parsedRawTypes = [];
        foreach (RawData rawType in rawTypes) {
            parsedRawTypes.Add(ParseRawDataWithDiagnostics(rawType, spc));
        }
        
        SettingsData globalSetting = default;
        if (globalSettings.Length != 0) {
            globalSetting = globalSettings[0];
            if (globalSettings.Length > 1) {
                foreach (SettingsData gs in globalSettings.Skip(1)) { spc.ReportDiagnostic(Diagnostics.CreateMultipleGlobalSettings(gs.Location?.ToLocation())); }
            }
        }
        else { globalSetting = data.FallbackGlobalSetting; }
        
        ParsedSettingsData parsedGlobalSetting = CreateParsedSettingsData(globalSetting, spc);
        ParsedRawData? intHandlerData = null;
        if (parsedGlobalSetting.IncludedRawTypes.TryGetValue("int", out ParsedRawData intHandler)) { intHandlerData = intHandler; } 
        
        Dictionary<string, ParsedRawData> aliases = new Dictionary<string, ParsedRawData>();
        foreach (ParsedRawData rawType in parsedRawTypes) {
            if (aliases.ContainsKey(rawType.Alias)) {
                spc.ReportDiagnostic(Diagnostics.CreateDuplicateAlias(rawType.Location?.ToLocation(), rawType.Alias));
                continue;
            }
            aliases[rawType.Alias] = rawType;
            spc.AddSource($"{rawType.Alias}ContextExtensions.g.cs", RawTypeWrapperSourceEmitter.EmitSource(rawType, intHandlerData));
        }
    }
}