using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class CustomSettingsStructTests : StructTestSuite<CustomSettingsStruct> {
    protected override CustomSettingsStruct Value => new() { B = 42 };

    protected override CustomSettingsStruct[] Values => [
        new() { B = 1 },
        new() { B = 2 },
        new() { B = 3 }
    ];

    protected override int? ExpectedFixedSizeBits => 32;
    protected override Type StructType => typeof(CustomSettingsStruct);

    protected override SerializationOperations<CustomSettingsStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, CustomSettingsStruct value) => context.WriteCustomSettingsStruct(value),
        Peek = (ReadContext context) => context.PeekCustomSettingsStruct(),
        Read = (ReadContext context) => context.ReadCustomSettingsStruct(),
        TryPeek = (ReadContext context, out CustomSettingsStruct value) => context.TryPeekCustomSettingsStruct(out value),
        TryRead = (ReadContext context, out CustomSettingsStruct value) => context.TryReadCustomSettingsStruct(out value),
        WriteSpan = (ref WriteContext context, Span<CustomSettingsStruct> values) => context.WriteCustomSettingsStructs(values),
        PeekSpan = (ReadContext context, Span<CustomSettingsStruct> destination) => context.PeekCustomSettingsStructs(destination),
        ReadSpan = (ReadContext context, Span<CustomSettingsStruct> destination) => context.ReadCustomSettingsStructs(destination),
        TryPeekSpan = (ReadContext context, Span<CustomSettingsStruct> destination) => context.TryPeekCustomSettingsStructs(destination),
        TryReadSpan = (ReadContext context, Span<CustomSettingsStruct> destination) => context.TryReadCustomSettingsStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<CustomSettingsStruct> values) => context.WriteCustomSettingsStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<CustomSettingsStruct> destination) => context.PeekCustomSettingsStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<CustomSettingsStruct> destination) => context.ReadCustomSettingsStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<CustomSettingsStruct> destination) => context.TryPeekCustomSettingsStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<CustomSettingsStruct> destination) => context.TryReadCustomSettingsStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<CustomSettingsStruct> destination) => context.PeekCustomSettingsStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<CustomSettingsStruct> destination) => context.ReadCustomSettingsStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<CustomSettingsStruct> destination) => context.TryPeekCustomSettingsStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<CustomSettingsStruct> destination) => context.TryReadCustomSettingsStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, CustomSettingsStruct[] values) => context.WriteCustomSettingsStructs(values),
        PeekArray = (ReadContext context) => context.PeekCustomSettingsStructs(),
        ReadArray = (ReadContext context) => context.ReadCustomSettingsStructs(),
        TryPeekArray = (ReadContext context, out CustomSettingsStruct[] values) => context.TryPeekCustomSettingsStructs(out values),
        TryReadArray = (ReadContext context, out CustomSettingsStruct[] values) => context.TryReadCustomSettingsStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, CustomSettingsStruct[] values) => context.WriteCustomSettingsStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekCustomSettingsStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadCustomSettingsStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out CustomSettingsStruct[] values) => context.TryPeekCustomSettingsStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out CustomSettingsStruct[] values) => context.TryReadCustomSettingsStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekCustomSettingsStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadCustomSettingsStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out CustomSettingsStruct[] values) => context.TryPeekCustomSettingsStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out CustomSettingsStruct[] values) => context.TryReadCustomSettingsStructsWithMaxCount(maxCount, out values),
    };
}
