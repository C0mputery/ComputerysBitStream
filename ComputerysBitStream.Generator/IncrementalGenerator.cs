using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

[Generator]
public class IncrementalGenerator : IIncrementalGenerator {
    private static SettingsData GetFallbackGlobalSetting(Compilation compilation, CancellationToken _) {
        INamedTypeSymbol? defaultSettingsSymbol = compilation.GetTypeByMetadataName(typeof(IDefaultSettings).FullName!);
        // No idea how this ccould fail
        if (defaultSettingsSymbol is not null) { return SettingsCollector.ProcessInterface(defaultSettingsSymbol, null, compilation); }
        else { return new SettingsData(InterfaceFullyQualifiedName: "", RawTypes: [], Structs: [], ExternalStructs: [], Location: null, Diagnostics: []); }
    }

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<SettingsData> globalSettingData = SettingsCollector.GetGlobalSettingsData(context);
        IncrementalValueProvider<ImmutableArray<SettingsData>> collectedGlobalSettingData = globalSettingData.Collect();
        
        IncrementalValuesProvider<SettingsData> settingData = SettingsCollector.GetSettingsData(context);
        IncrementalValueProvider<ImmutableArray<SettingsData>> collectedSettingData = settingData.Collect();
        
        IncrementalValuesProvider<RawData> rawAttributeData = RawTypeCollector.GetBitStreamRawAttributeData(context);
        IncrementalValueProvider<ImmutableArray<RawData>> collectedRawAttributeData = rawAttributeData.Collect();
        
        IncrementalValuesProvider<StructData> allStructData = StructCollector.GetAllStructData(context);
        IncrementalValueProvider<ImmutableArray<StructData>> collectedAllStructData = allStructData.Collect();
        
        IncrementalValueProvider<SettingsData> fallbackGlobalSetting = context.CompilationProvider.Select(GetFallbackGlobalSetting);
        
        IncrementalValueProvider<AllCollectedData> allCollectedData = collectedSettingData
            .Combine(collectedGlobalSettingData)
            .Combine(collectedRawAttributeData)
            .Combine(collectedAllStructData)
            .Combine(fallbackGlobalSetting)
            .Select(static (data, _) => new AllCollectedData(
                GlobalSettings: data.Left.Left.Left.Right,
                Settings: data.Left.Left.Left.Left,
                RawTypes: data.Left.Left.Right,
                Structs: data.Left.Right,
                FallbackGlobalSetting: data.Right
            )
        );
        
        context.RegisterSourceOutput(allCollectedData, Execute);
    }
    
    private static void Execute(SourceProductionContext context, AllCollectedData data) => new ExecutionContext(context, data).Run();
}