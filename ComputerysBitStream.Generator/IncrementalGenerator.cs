using ComputerysBitStream.Generator.Collectors;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

[Generator]
public class IncrementalGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<Collected<SettingsDefinition>> globalSettings = SettingsCollector.GetGlobalSettingsData(context);
        IncrementalValuesProvider<Collected<SettingsDefinition>> settings = SettingsCollector.GetSettingsData(context);
        IncrementalValuesProvider<Collected<PrimitiveDefinition>> primitives = PrimitiveCollector.GetPrimitiveData(context);
        IncrementalValuesProvider<Collected<StructDefinition>> structs = StructCollector.GetAllStructData(context);

        IncrementalValueProvider<Collected<SettingsDefinition>> fallbackGlobalSettings = context.CompilationProvider.Select(SettingsCollector.GetFallbackGlobalSettings);

        IncrementalValueProvider<AllCollectedData> allCollectedData = globalSettings.Collect()
            .Combine(settings.Collect())
            .Combine(primitives.Collect())
            .Combine(structs.Collect())
            .Combine(fallbackGlobalSettings)
            .Select((data, _) => new AllCollectedData(
                GlobalSettings: data.Left.Left.Left.Left,
                Settings: data.Left.Left.Left.Right,
                Primitives: data.Left.Left.Right,
                Structs: data.Left.Right,
                FallbackGlobalSettings: data.Right
            ));

        context.RegisterSourceOutput(allCollectedData, ExecutionContext.Run);
    }
}
