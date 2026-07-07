using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.EquatableCollections;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal readonly ref partial struct ExecutionContext {
    internal static void Run(SourceProductionContext context, AllCollectedData data) {
        ExecutionContext executionContext = new(context, data);
        executionContext.RunCore();
    }

    private readonly SourceProductionContext _context;
    private readonly ImmutableArray<PrimitiveDefinition> _primitivesArray;
    private readonly ImmutableArray<StructDefinition> _structDataArray;
    private readonly ImmutableDictionary<string, SettingsDefinition> _localSettingsByInterface;
    private readonly SettingsDefinition _globalSettings;

    private ExecutionContext(SourceProductionContext context, AllCollectedData data) {
        _context = context;

        SettingsDefinition fallbackGlobalSetting = UnwrapSettingsOrDefault(data.FallbackGlobalSettings);

        ImmutableArray<SettingsDefinition> globalSettingsDataArray = UnwrapValidSettingsArray(data.GlobalSettings);
        if (globalSettingsDataArray.Length > 1) {
            string conflictingDefinitions = string.Join("; ", globalSettingsDataArray.Select(static settings => {
                ImmutableArray<string> interfaces = settings.InterfaceFullyQualifiedNames;
                return interfaces.IsDefaultOrEmpty ? "(no interfaces)" : string.Join(", ", interfaces);
            }));
            ReportDiagnostics(ImmutableArray.Create(new DiagnosticValueType(DiagnosticDescriptors.MultipleGlobalSettings, globalSettingsDataArray[0].Location, conflictingDefinitions)));
        }
        _globalSettings = globalSettingsDataArray.Length > 0 ? globalSettingsDataArray[0] : fallbackGlobalSetting;

        ImmutableArray<SettingsDefinition> localSettingsArray = UnwrapValidSettingsArray(data.Settings);
        _localSettingsByInterface = BuildLocalSettingsDictionary(localSettingsArray);

        _primitivesArray = UnwrapDeduplicateAndReportPrimitives(data.Primitives);
        _structDataArray = UnwrapDeduplicateAndReportStructs(data.Structs);
    }

    private static ImmutableDictionary<string, SettingsDefinition> BuildLocalSettingsDictionary(ImmutableArray<SettingsDefinition> localSettingsArray) {
        Dictionary<string, SettingsDefinition> settingsByInterface = new();
        foreach (SettingsDefinition settings in localSettingsArray) {
            foreach (string interfaceName in settings.InterfaceFullyQualifiedNames) { settingsByInterface[interfaceName] = settings; }
        }

        return settingsByInterface.ToImmutableDictionary();
    }

    private SettingsDefinition UnwrapSettingsOrDefault(Collected<SettingsDefinition> collected) {
        ReportDiagnostics(collected.Diagnostics);
        return collected.IsValid
            ? collected.Value
            : new SettingsDefinition(
                InterfaceFullyQualifiedNames: [],
                Primitives: ImmutableDictionary<string, PrimitiveDefinition>.Empty,
                Structs: ImmutableDictionary<string, StructDefinition>.Empty,
                ExternalStructs: ImmutableDictionary<string, ExternalStructDefinition>.Empty,
                Location: null
            );
    }

    private ImmutableArray<SettingsDefinition> UnwrapValidSettingsArray(EquatableImmutableArray<Collected<SettingsDefinition>> collectedArray) {
        ImmutableArray<SettingsDefinition> values = Collected<SettingsDefinition>.UnwrapValidCollectedArray(collectedArray, out ImmutableArray<DiagnosticValueType> diagnostics);
        ReportDiagnostics(diagnostics);
        return values;
    }

    private ImmutableArray<PrimitiveDefinition> UnwrapDeduplicateAndReportPrimitives(EquatableImmutableArray<Collected<PrimitiveDefinition>> collectedArray) {
        HashSet<string> seenKeys = [];
        ImmutableArray<PrimitiveDefinition>.Builder values = ImmutableArray.CreateBuilder<PrimitiveDefinition>();
        ImmutableArray<DiagnosticValueType>.Builder duplicateDiagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();

        foreach (Collected<PrimitiveDefinition> collected in collectedArray) {
            ReportDiagnostics(collected.Diagnostics);
            if (!collected.IsValid) { continue; }

            PrimitiveDefinition definition = collected.Value;
            string key = string.IsNullOrEmpty(definition.TargetTypeFullyQualifiedName) ? string.Empty : $"{definition.TargetTypeFullyQualifiedName}|{definition.Namespace}|{definition.Alias}";

            if (!string.IsNullOrEmpty(key) && !seenKeys.Add(key)) {
                duplicateDiagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicatePrimitiveDefinition, definition.Location, definition.TargetTypeFullyQualifiedName, definition.Alias, definition.Namespace));
                continue;
            }

            values.Add(definition);
        }

        ReportDiagnostics(duplicateDiagnostics.ToImmutable());
        return values.ToImmutable();
    }

    private ImmutableArray<StructDefinition> UnwrapDeduplicateAndReportStructs(EquatableImmutableArray<Collected<StructDefinition>> collectedArray) {
        HashSet<string> seenKeys = [];
        ImmutableArray<StructDefinition>.Builder values = ImmutableArray.CreateBuilder<StructDefinition>();
        ImmutableArray<DiagnosticValueType>.Builder duplicateDiagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();

        foreach (Collected<StructDefinition> collected in collectedArray) {
            ReportDiagnostics(collected.Diagnostics);
            if (!collected.IsValid) { continue; }

            StructDefinition definition = collected.Value;
            string key = string.IsNullOrEmpty(definition.TypeFullyQualifiedName) ? string.Empty : $"{definition.TypeFullyQualifiedName}|{definition.Alias}";

            if (!string.IsNullOrEmpty(key) && !seenKeys.Add(key)) {
                duplicateDiagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateStructDefinition, definition.Location, definition.TypeFullyQualifiedName, definition.Alias));
                continue;
            }

            values.Add(definition);
        }

        ReportDiagnostics(duplicateDiagnostics.ToImmutable());
        return values.ToImmutable();
    }

    private void ReportDiagnostics(ImmutableArray<DiagnosticValueType> diagnostics) {
        if (diagnostics.IsDefaultOrEmpty) { return; }
        foreach (DiagnosticValueType diagnostic in diagnostics) { _context.ReportDiagnostic(diagnostic.ToDiagnostic()); }
    }

    private void RunCore() {
        ImmutableArray<PrimitiveDefinition> structPrimitives = ResolveAndEmitStructs();
        ImmutableArray<PrimitiveDefinition>.Builder allPrimitives = ImmutableArray.CreateBuilder<PrimitiveDefinition>();
        allPrimitives.AddRange(_primitivesArray);
        allPrimitives.AddRange(structPrimitives);
        EmitPrimitiveDefinitions(allPrimitives.ToImmutable());
    }
}
