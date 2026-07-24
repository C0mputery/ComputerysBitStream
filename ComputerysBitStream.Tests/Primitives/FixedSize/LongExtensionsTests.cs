using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class LongExtensionsTests : PrimitiveSerializationTestSuite<long> {
    protected override long Value => 42L;
    protected override long[] Values => [42L, -42L, 42L, 42L, -42L];

    protected override SerializationOperations<long> Operations { get; } = new() {
        Write = (ref WriteContext context, long value) => context.WriteLong(value),
        Peek = (ReadContext context) => context.PeekLong(),
        Read = (ReadContext context) => context.ReadLong(),
        TryPeek = (ReadContext context, out long value) => context.TryPeekLong(out value),
        TryRead = (ReadContext context, out long value) => context.TryReadLong(out value),
        WriteSpan = (ref WriteContext context, Span<long> values) => context.WriteLongs(values),
        PeekSpan = (ReadContext context, Span<long> destination) => context.PeekLongs(destination),
        ReadSpan = (ReadContext context, Span<long> destination) => context.ReadLongs(destination),
        TryPeekSpan = (ReadContext context, Span<long> destination) => context.TryPeekLongs(destination),
        TryReadSpan = (ReadContext context, Span<long> destination) => context.TryReadLongs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<long> values) => context.WriteLongsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.PeekLongs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.ReadLongs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.TryPeekLongs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.TryReadLongs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.PeekLongsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.ReadLongsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.TryPeekLongsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.TryReadLongsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, long[] values) => context.WriteLongs(values),
        PeekArray = (ReadContext context) => context.PeekLongs(),
        ReadArray = (ReadContext context) => context.ReadLongs(),
        TryPeekArray = (ReadContext context, out long[] values) => context.TryPeekLongs(out values),
        TryReadArray = (ReadContext context, out long[] values) => context.TryReadLongs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, long[] values) => context.WriteLongsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekLongs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadLongs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out long[] values) => context.TryPeekLongs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out long[] values) => context.TryReadLongs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekLongsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadLongsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out long[] values) => context.TryPeekLongsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out long[] values) => context.TryReadLongsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<long> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, long value) => context.WriteLongPrimitive(value),
        Peek = (ReadContext context) => context.PeekLongPrimitive(),
        Read = (ReadContext context) => context.ReadLongPrimitive(),
        WriteSpan = (ref WriteContext context, Span<long> values) => context.WriteLongsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<long> destination) => context.PeekLongSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<long> destination) => context.ReadLongSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, long[] values) => context.WriteLongsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekLongArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadLongArrayPrimitive(count),
    };
}
