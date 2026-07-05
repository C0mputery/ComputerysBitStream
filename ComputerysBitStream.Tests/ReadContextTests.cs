namespace ComputerysBitStream.Tests;

[BitStreamPrimitiveContext]
public class ReadContextTests {
    private static readonly int[] ReadCounts = [1, 2, 7, 8, 16, 31, 32, 63, 64];

    [Fact]
    public void Constructor_WithBuffer_ShouldSetInitialState() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];

        ReadContext context = new(buffer);

        Assert.Equal(0, context.Position);
        Assert.Equal(buffer.Length * BitHelper.ULongSize, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void Constructor_WithPosition_ShouldSetPositionAndCapacity(int initialOffset) {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];

        ReadContext context = new(buffer, initialOffset);

        Assert.Equal(initialOffset, context.Position);
        Assert.Equal(buffer.Length * BitHelper.ULongSize, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void Constructor_WithPositionAndCapacity_ShouldKeepProvidedValues(int initialOffset) {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        int capacity = buffer.Length * BitHelper.ULongSize - 5;

        ReadContext context = new(buffer, initialOffset, capacity);

        Assert.Equal(initialOffset, context.Position);
        Assert.Equal(capacity, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void CapacityHelpers_ShouldReflectRemainingBits(int initialOffset) {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        ReadContext context = new(buffer, initialOffset, 128);

        Assert.Equal(128 - initialOffset, context.GetRemainingCapacity());
        Assert.True(context.HasSpaceRemaining(128 - initialOffset));
        Assert.False(context.IsInsufficientSpace(128 - initialOffset));
        Assert.False(context.HasSpaceRemaining((128 - initialOffset) + 1));
        Assert.True(context.IsInsufficientSpace((128 - initialOffset) + 1));
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void PeekBitPrimitive_ShouldReturnExpectedBitWithoutAdvancing(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();
        ReadContext context = new(buffer, initialOffset);

        bool expected = ReadBit(buffer, initialOffset);

        bool actual = context.PeekBitPrimitive();

        Assert.Equal(expected, actual);
        Assert.Equal(initialOffset, context.Position);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void ReadBitPrimitive_ShouldReturnExpectedBitAndAdvance(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();
        ReadContext context = new(buffer, initialOffset);

        bool expected = ReadBit(buffer, initialOffset);

        bool actual = context.ReadBitPrimitive();

        Assert.Equal(expected, actual);
        Assert.Equal(initialOffset + 1, context.Position);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void PeekBitsPrimitive_ShouldReturnExpectedBitsWithoutAdvancing(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();
        ReadContext context = new(buffer, initialOffset);

        foreach (int count in ReadCounts) {
            ulong expected = ReadBits(buffer, initialOffset, count) & MaskLowerBits(count);

            ulong actual = context.PeekBitsPrimitive(count);

            Assert.Equal(expected, actual);
            Assert.Equal(initialOffset, context.Position);
        }
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void ReadBitsPrimitive_ShouldReturnExpectedBitsAndAdvance(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();

        foreach (int count in ReadCounts) {
            ReadContext context = new(buffer, initialOffset);
            ulong expected = ReadBits(buffer, initialOffset, count) & MaskLowerBits(count);

            ulong actual = context.ReadBitsPrimitive(count);

            Assert.Equal(expected, actual);
            Assert.Equal(initialOffset + count, context.Position);
        }
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void PeekBitsPrimitive_Span_ShouldFillDestinationWithoutAdvancing(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();
        const int count = 130;
        int expectedWords = (count + BitHelper.ULongSize - 1) / BitHelper.ULongSize;

        ReadContext context = new(buffer, initialOffset);
        ulong[] destinationArray = [ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue];
        Span<ulong> destination = destinationArray;

        ulong[] expected = ReadBitsSpan(buffer, initialOffset, count);

        context.PeekBitsPrimitive(count, destination);

        for (int i = 0; i < expectedWords; i++) { Assert.Equal(expected[i], destination[i]); }

        Assert.Equal(ulong.MaxValue, destinationArray[3]);
        Assert.Equal(initialOffset, context.Position);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void ReadBitsPrimitive_Span_ShouldFillDestinationAndAdvance(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();
        const int count = 130;
        int expectedWords = (count + BitHelper.ULongSize - 1) / BitHelper.ULongSize;

        ReadContext context = new(buffer, initialOffset);
        ulong[] destinationArray = [ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue];
        Span<ulong> destination = destinationArray;

        ulong[] expected = ReadBitsSpan(buffer, initialOffset, count);

        context.ReadBitsPrimitive(count, destination);

        for (int i = 0; i < expectedWords; i++) {
            Assert.Equal(expected[i], destination[i]);
        }

        Assert.Equal(ulong.MaxValue, destinationArray[3]);
        Assert.Equal(initialOffset + count, context.Position);
    }

    private static ulong[] CreatePatternBuffer() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        const ulong seed = 0xF0E1D2C3B4A59687UL;
        const ulong stride = 0x9E3779B97F4A7C15UL;

        for (int i = 0; i < buffer.Length; i++) {
            buffer[i] = seed ^ (stride * (ulong)(i + 1));
        }

        return buffer;
    }

    private static bool ReadBit(ulong[] buffer, int position) {
        int ulongIndex = position / BitHelper.ULongSize;
        int bitOffset = position % BitHelper.ULongSize;
        return (buffer[ulongIndex] & (1UL << bitOffset)) != 0;
    }

    private static ulong ReadBits(ulong[] buffer, int position, int count) {
        ulong result = 0;

        for (int i = 0; i < count; i++) {
            if (ReadBit(buffer, position + i)) { result |= 1UL << i; }
        }

        return result;
    }

    private static ulong[] ReadBitsSpan(ulong[] buffer, int position, int count) {
        int words = (count + BitHelper.ULongSize - 1) / BitHelper.ULongSize;
        ulong[] result = new ulong[words];

        for (int i = 0; i < count; i++) {
            if (ReadBit(buffer, position + i)) {
                int wordIndex = i / BitHelper.ULongSize;
                int bitOffset = i % BitHelper.ULongSize;
                result[wordIndex] |= 1UL << bitOffset;
            }
        }

        return result;
    }

    private static ulong MaskLowerBits(int count) {
        if (count == BitHelper.ULongSize) { return ulong.MaxValue; }
        return (1UL << count) - 1;
    }
}
