using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthULongExtensionsTests : VariableLengthExtensionTestSuite<ulong> {
    protected override ulong Value => 42UL;
    protected override ulong[] Values => [42UL, 0UL, 1000000000UL];
    protected override int GetSize(ulong value) => PrimitiveVariableLengthULongExtensions.GetVariableLengthULongSize(value);

    protected override SerializationOperations<ulong> Operations { get; } = new() {
        Write = (ref WriteContext context, ulong value) => context.WriteVariableLengthULong(value),
        Peek = (ReadContext context) => context.PeekVariableLengthULong(),
        Read = (ReadContext context) => context.ReadVariableLengthULong(),
        TryPeek = (ReadContext context, out ulong value) => context.TryPeekVariableLengthULong(out value),
        TryRead = (ReadContext context, out ulong value) => context.TryReadVariableLengthULong(out value),
        WriteSpan = (ref WriteContext context, Span<ulong> values) => context.WriteVariableLengthULongs(values),
        PeekSpan = (ReadContext context, Span<ulong> destination) => context.PeekVariableLengthULongs(destination),
        ReadSpan = (ReadContext context, Span<ulong> destination) => context.ReadVariableLengthULongs(destination),
        TryPeekSpan = (ReadContext context, Span<ulong> destination) => context.TryPeekVariableLengthULongs(destination),
        TryReadSpan = (ReadContext context, Span<ulong> destination) => context.TryReadVariableLengthULongs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<ulong> values) => context.WriteVariableLengthULongsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.PeekVariableLengthULongs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.ReadVariableLengthULongs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.TryPeekVariableLengthULongs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.TryReadVariableLengthULongs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.PeekVariableLengthULongsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.ReadVariableLengthULongsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.TryPeekVariableLengthULongsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.TryReadVariableLengthULongsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, ulong[] values) => context.WriteVariableLengthULongs(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthULongs(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthULongs(),
        TryPeekArray = (ReadContext context, out ulong[] values) => context.TryPeekVariableLengthULongs(out values),
        TryReadArray = (ReadContext context, out ulong[] values) => context.TryReadVariableLengthULongs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, ulong[] values) => context.WriteVariableLengthULongsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthULongs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthULongs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out ulong[] values) => context.TryPeekVariableLengthULongs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out ulong[] values) => context.TryReadVariableLengthULongs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthULongsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthULongsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out ulong[] values) => context.TryPeekVariableLengthULongsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out ulong[] values) => context.TryReadVariableLengthULongsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<ulong> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, ulong value) => context.WriteVariableLengthULongPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthULongPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthULongPrimitive(),
        WriteSpan = (ref WriteContext context, Span<ulong> values) => context.WriteVariableLengthULongsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<ulong> destination) => context.PeekVariableLengthULongSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<ulong> destination) => context.ReadVariableLengthULongSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, ulong[] values) => context.WriteVariableLengthULongsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthULongArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthULongArrayPrimitive(count),
    };
}
