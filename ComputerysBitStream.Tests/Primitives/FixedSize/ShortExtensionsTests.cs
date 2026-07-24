using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class ShortExtensionsTests : PrimitiveSerializationTestSuite<short> {
    protected override short Value => 42;
    protected override short[] Values => [42, -42, 42, 42, -42];

    protected override SerializationOperations<short> Operations { get; } = new() {
        Write = (ref WriteContext context, short value) => context.WriteShort(value),
        Peek = (ReadContext context) => context.PeekShort(),
        Read = (ReadContext context) => context.ReadShort(),
        TryPeek = (ReadContext context, out short value) => context.TryPeekShort(out value),
        TryRead = (ReadContext context, out short value) => context.TryReadShort(out value),
        WriteSpan = (ref WriteContext context, Span<short> values) => context.WriteShorts(values),
        PeekSpan = (ReadContext context, Span<short> destination) => context.PeekShorts(destination),
        ReadSpan = (ReadContext context, Span<short> destination) => context.ReadShorts(destination),
        TryPeekSpan = (ReadContext context, Span<short> destination) => context.TryPeekShorts(destination),
        TryReadSpan = (ReadContext context, Span<short> destination) => context.TryReadShorts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<short> values) => context.WriteShortsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.PeekShorts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.ReadShorts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.TryPeekShorts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.TryReadShorts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.PeekShortsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.ReadShortsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.TryPeekShortsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.TryReadShortsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, short[] values) => context.WriteShorts(values),
        PeekArray = (ReadContext context) => context.PeekShorts(),
        ReadArray = (ReadContext context) => context.ReadShorts(),
        TryPeekArray = (ReadContext context, out short[] values) => context.TryPeekShorts(out values),
        TryReadArray = (ReadContext context, out short[] values) => context.TryReadShorts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, short[] values) => context.WriteShortsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekShorts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadShorts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out short[] values) => context.TryPeekShorts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out short[] values) => context.TryReadShorts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekShortsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadShortsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out short[] values) => context.TryPeekShortsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out short[] values) => context.TryReadShortsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<short> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, short value) => context.WriteShortPrimitive(value),
        Peek = (ReadContext context) => context.PeekShortPrimitive(),
        Read = (ReadContext context) => context.ReadShortPrimitive(),
        WriteSpan = (ref WriteContext context, Span<short> values) => context.WriteShortsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<short> destination) => context.PeekShortSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<short> destination) => context.ReadShortSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, short[] values) => context.WriteShortsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekShortArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadShortArrayPrimitive(count),
    };
}
