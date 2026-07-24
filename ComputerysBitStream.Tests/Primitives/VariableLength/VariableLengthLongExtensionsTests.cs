using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthLongExtensionsTests : VariableLengthExtensionTestSuite<long> {
    protected override long Value => 42L;
    protected override long[] Values => [42L, 0L, -1000000000L];
    protected override int GetSize(long value) => PrimitiveVariableLengthLongExtensions.GetVariableLengthLongSize(value);

    protected override SerializationOperations<long> Operations { get; } = new() {
        Write = (ref WriteContext context, long value) => context.WriteVariableLengthLong(value),
        Peek = (ReadContext context) => context.PeekVariableLengthLong(),
        Read = (ReadContext context) => context.ReadVariableLengthLong(),
        TryPeek = (ReadContext context, out long value) => context.TryPeekVariableLengthLong(out value),
        TryRead = (ReadContext context, out long value) => context.TryReadVariableLengthLong(out value),
        WriteSpan = (ref WriteContext context, Span<long> values) => context.WriteVariableLengthLongs(values),
        PeekSpan = (ReadContext context, Span<long> destination) => context.PeekVariableLengthLongs(destination),
        ReadSpan = (ReadContext context, Span<long> destination) => context.ReadVariableLengthLongs(destination),
        TryPeekSpan = (ReadContext context, Span<long> destination) => context.TryPeekVariableLengthLongs(destination),
        TryReadSpan = (ReadContext context, Span<long> destination) => context.TryReadVariableLengthLongs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<long> values) => context.WriteVariableLengthLongsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.PeekVariableLengthLongs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.ReadVariableLengthLongs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.TryPeekVariableLengthLongs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<long> destination) => context.TryReadVariableLengthLongs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.PeekVariableLengthLongsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.ReadVariableLengthLongsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.TryPeekVariableLengthLongsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<long> destination) => context.TryReadVariableLengthLongsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, long[] values) => context.WriteVariableLengthLongs(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthLongs(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthLongs(),
        TryPeekArray = (ReadContext context, out long[] values) => context.TryPeekVariableLengthLongs(out values),
        TryReadArray = (ReadContext context, out long[] values) => context.TryReadVariableLengthLongs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, long[] values) => context.WriteVariableLengthLongsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthLongs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthLongs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out long[] values) => context.TryPeekVariableLengthLongs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out long[] values) => context.TryReadVariableLengthLongs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthLongsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthLongsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out long[] values) => context.TryPeekVariableLengthLongsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out long[] values) => context.TryReadVariableLengthLongsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<long> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, long value) => context.WriteVariableLengthLongPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthLongPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthLongPrimitive(),
        WriteSpan = (ref WriteContext context, Span<long> values) => context.WriteVariableLengthLongsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<long> destination) => context.PeekVariableLengthLongSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<long> destination) => context.ReadVariableLengthLongSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, long[] values) => context.WriteVariableLengthLongsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthLongArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthLongArrayPrimitive(count),
    };
}
