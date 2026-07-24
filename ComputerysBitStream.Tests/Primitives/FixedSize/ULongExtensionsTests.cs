using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class ULongExtensionsTests : PrimitiveSerializationTestSuite<ulong> {
    protected override ulong Value => 42ul;
    protected override ulong[] Values => [42ul, 100ul, 42ul, 42ul, 100ul];

    protected override SerializationOperations<ulong> Operations { get; } = new() {
        Write = (ref WriteContext context, ulong value) => context.WriteULong(value),
        Peek = (ReadContext context) => context.PeekULong(),
        Read = (ReadContext context) => context.ReadULong(),
        TryPeek = (ReadContext context, out ulong value) => context.TryPeekULong(out value),
        TryRead = (ReadContext context, out ulong value) => context.TryReadULong(out value),
        WriteSpan = (ref WriteContext context, Span<ulong> values) => context.WriteULongs(values),
        PeekSpan = (ReadContext context, Span<ulong> destination) => context.PeekULongs(destination),
        ReadSpan = (ReadContext context, Span<ulong> destination) => context.ReadULongs(destination),
        TryPeekSpan = (ReadContext context, Span<ulong> destination) => context.TryPeekULongs(destination),
        TryReadSpan = (ReadContext context, Span<ulong> destination) => context.TryReadULongs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<ulong> values) => context.WriteULongsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.PeekULongs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.ReadULongs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.TryPeekULongs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<ulong> destination) => context.TryReadULongs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.PeekULongsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.ReadULongsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.TryPeekULongsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ulong> destination) => context.TryReadULongsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, ulong[] values) => context.WriteULongs(values),
        PeekArray = (ReadContext context) => context.PeekULongs(),
        ReadArray = (ReadContext context) => context.ReadULongs(),
        TryPeekArray = (ReadContext context, out ulong[] values) => context.TryPeekULongs(out values),
        TryReadArray = (ReadContext context, out ulong[] values) => context.TryReadULongs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, ulong[] values) => context.WriteULongsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekULongs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadULongs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out ulong[] values) => context.TryPeekULongs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out ulong[] values) => context.TryReadULongs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekULongsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadULongsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out ulong[] values) => context.TryPeekULongsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out ulong[] values) => context.TryReadULongsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<ulong> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, ulong value) => context.WriteULongPrimitive(value),
        Peek = (ReadContext context) => context.PeekULongPrimitive(),
        Read = (ReadContext context) => context.ReadULongPrimitive(),
        WriteSpan = (ref WriteContext context, Span<ulong> values) => context.WriteULongsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<ulong> destination) => context.PeekULongSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<ulong> destination) => context.ReadULongSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, ulong[] values) => context.WriteULongsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekULongArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadULongArrayPrimitive(count),
    };
}
