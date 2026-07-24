using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class IntExtensionsTests : PrimitiveSerializationTestSuite<int> {
    protected override int Value => 42;
    protected override int[] Values => [42, -42, 42, 42, -42];

    [Fact]
    public void WriteWithMaxCount_WritesValuesWithinLimit() {
        int[] expected = Values;
        ulong[] buffer = new ulong[16];
        WriteContext write = new(buffer);
        write.WriteIntsWithMaxCount(expected, expected.Length);

        ReadContext read = new(buffer);
        Assert.Equal(expected, read.ReadInts());
    }

    [Fact]
    public void WriteWithMaxCount_RejectsValuesAboveLimitWithoutAdvancing() {
        ulong[] buffer = new ulong[16];
        WriteContext write = new(buffer);
        long originalPosition = write.Position;

        ArgumentException? exception = null;
        try {
            write.WriteIntsWithMaxCount(Values, Values.Length - 1);
        }
        catch (ArgumentException caught) {
            exception = caught;
        }

        Assert.NotNull(exception);
        Assert.Equal(originalPosition, write.Position);
    }

    protected override SerializationOperations<int> Operations { get; } = new() {
        Write = (ref WriteContext context, int value) => context.WriteInt(value),
        Peek = (ReadContext context) => context.PeekInt(),
        Read = (ReadContext context) => context.ReadInt(),
        TryPeek = (ReadContext context, out int value) => context.TryPeekInt(out value),
        TryRead = (ReadContext context, out int value) => context.TryReadInt(out value),
        WriteSpan = (ref WriteContext context, Span<int> values) => context.WriteInts(values),
        PeekSpan = (ReadContext context, Span<int> destination) => context.PeekInts(destination),
        ReadSpan = (ReadContext context, Span<int> destination) => context.ReadInts(destination),
        TryPeekSpan = (ReadContext context, Span<int> destination) => context.TryPeekInts(destination),
        TryReadSpan = (ReadContext context, Span<int> destination) => context.TryReadInts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<int> values) => context.WriteIntsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.PeekInts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.ReadInts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.TryPeekInts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.TryReadInts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.PeekIntsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.ReadIntsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.TryPeekIntsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.TryReadIntsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, int[] values) => context.WriteInts(values),
        PeekArray = (ReadContext context) => context.PeekInts(),
        ReadArray = (ReadContext context) => context.ReadInts(),
        TryPeekArray = (ReadContext context, out int[] values) => context.TryPeekInts(out values),
        TryReadArray = (ReadContext context, out int[] values) => context.TryReadInts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, int[] values) => context.WriteIntsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekInts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadInts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out int[] values) => context.TryPeekInts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out int[] values) => context.TryReadInts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekIntsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadIntsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out int[] values) => context.TryPeekIntsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out int[] values) => context.TryReadIntsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<int> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, int value) => context.WriteIntPrimitive(value),
        Peek = (ReadContext context) => context.PeekIntPrimitive(),
        Read = (ReadContext context) => context.ReadIntPrimitive(),
        WriteSpan = (ref WriteContext context, Span<int> values) => context.WriteIntsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<int> destination) => context.PeekIntSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<int> destination) => context.ReadIntSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, int[] values) => context.WriteIntsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekIntArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadIntArrayPrimitive(count),
    };
}
