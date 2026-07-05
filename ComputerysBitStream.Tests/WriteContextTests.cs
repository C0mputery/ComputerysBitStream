namespace ComputerysBitStream.Tests;

[BitStreamPrimitiveContext]
public class WriteContextTests {
    private static readonly int[] WriteCounts = [1, 3, 7, 8, 17, 32, 63, 64];

    [Fact]
    public void Constructor_WithBuffer_ShouldSetInitialState() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];

        WriteContext context = new(buffer);

        Assert.Equal(0, context.Position);
        Assert.Equal(buffer.Length * BitHelper.ULongSize, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void Constructor_WithPosition_ShouldSetPositionAndCapacity(int initialOffset) {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];

        WriteContext context = new(buffer, initialOffset);

        Assert.Equal(initialOffset, context.Position);
        Assert.Equal(buffer.Length * BitHelper.ULongSize, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void Constructor_WithPositionAndCapacity_ShouldKeepProvidedValues(int initialOffset) {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        int capacity = buffer.Length * BitHelper.ULongSize - 9;

        WriteContext context = new(buffer, initialOffset, capacity);

        Assert.Equal(initialOffset, context.Position);
        Assert.Equal(capacity, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteBitPrimitive_ShouldWriteExpectedBitAndAdvance(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer();
        ulong[] expected = buffer.ToArray();
        bool value = (initialOffset & 1) == 0;

        WriteContext context = new(buffer, initialOffset);
        WriteBits(expected, initialOffset, value ? 1UL : 0UL, 1);

        context.WriteBitPrimitive(value);

        Assert.Equal(expected, buffer);
        Assert.Equal(initialOffset + 1, context.Position);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteBitsPrimitive_Value_ShouldWriteExpectedBitsAndAdvance(int initialOffset) {
        const ulong value = 0xFEDCBA9876543210UL;

        foreach (int count in WriteCounts) {
            ulong[] buffer = CreatePatternBuffer();
            ulong[] expected = buffer.ToArray();
            WriteContext context = new(buffer, initialOffset);

            WriteBits(expected, initialOffset, value, count);
            context.WriteBitsPrimitive(value, count);

            Assert.Equal(expected, buffer);
            Assert.Equal(initialOffset + count, context.Position);
        }
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteBitsPrimitive_Span_ShouldWriteExpectedBitsAndAdvance(int initialOffset) {
        ulong[] source = [0x0123456789ABCDEFUL, 0xFEDCBA9876543210UL, 0x0F0F0F0F0F0F0F0FUL];
        const int count = 130;

        ulong[] buffer = CreatePatternBuffer();
        ulong[] expected = buffer.ToArray();

        WriteContext context = new(buffer, initialOffset);
        WriteBits(expected, initialOffset, source, count);

        context.WriteBitsPrimitive(source, count);

        Assert.Equal(expected, buffer);
        Assert.Equal(initialOffset + count, context.Position);
    }

    [Fact]
    public void ReserveBitsPrimitive_ShouldAdvancePosition() {
        ulong[] buffer = new ulong[2];
        WriteContext context = new(buffer, 5);

        context.ReserveBitsPrimitive(12);

        Assert.Equal(17, context.Position);
    }

    [Fact]
    public void SetPositionPrimitive_ShouldSetPosition() {
        ulong[] buffer = new ulong[2];
        WriteContext context = new(buffer);

        context.SetPositionPrimitive(23);

        Assert.Equal(23, context.Position);
    }

    [Fact]
    public void ToBytesRaw_ShouldIncludeGarbageBitsInLastByte() {
        ulong[] buffer = new ulong[1];
        buffer[0] = ulong.MaxValue;

        WriteContext context = new(buffer, 10);
        Span<byte> bytes = context.WrittenBytesSpanPrimitive();

        Assert.Equal(2, bytes.Length);
        Assert.Equal((byte)0xFF, bytes[1]);
    }

    [Theory]
    [InlineData(9, 0x01)]
    [InlineData(10, 0x03)]
    [InlineData(11, 0x07)]
    [InlineData(12, 0x0F)]
    [InlineData(13, 0x1F)]
    [InlineData(14, 0x3F)]
    [InlineData(15, 0x7F)]
    public void ToBytes_ShouldMaskUnusedBitsInLastByte(int position, byte expectedLastByte) {
        ulong[] buffer = new ulong[1];
        buffer[0] = ulong.MaxValue;

        WriteContext context = new(buffer, position);
        Span<byte> bytes = context.GetWrittenBytes();

        Assert.Equal(BitHelper.BitsToBytes(position), bytes.Length);
        Assert.Equal((byte)0xFF, bytes[0]);
        Assert.Equal(expectedLastByte, bytes[^1]);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    public void ToBytes_WhenPositionIsByteAligned_ShouldNotMaskLastByte(int position) {
        ulong[] buffer = new ulong[1];
        buffer[0] = ulong.MaxValue;

        WriteContext context = new(buffer, position);
        Span<byte> bytes = context.GetWrittenBytes();

        Assert.Equal(BitHelper.BitsToBytes(position), bytes.Length);
        Assert.Equal((byte)0xFF, bytes[^1]);
    }

    [Fact]
    public void ToBytes_WhenPositionIsZero_ShouldReturnEmptySpan() {
        ulong[] buffer = new ulong[1];
        buffer[0] = ulong.MaxValue;

        WriteContext context = new(buffer, 0);
        Span<byte> bytes = context.GetWrittenBytes();

        Assert.Equal(0, bytes.Length);
    }

    [Fact]
    public void ToBytes_ShouldMaskLastByteAcrossUlongBoundary() {
        ulong[] buffer = [ulong.MaxValue, ulong.MaxValue];

        WriteContext context = new(buffer, 71);
        Span<byte> bytes = context.GetWrittenBytes();

        Assert.Equal(BitHelper.BitsToBytes(71), bytes.Length);
        Assert.Equal((byte)0x7F, bytes[^1]);
    }

    [Fact]
    public void ThrowIfInsufficientSpace_WhenEnoughCapacity_ShouldNotThrow() {
        ulong[] buffer = new ulong[1];
        WriteContext context = new(buffer, 60, 64);

        context.ThrowIfInsufficientSpace("UInt", 4);
    }

    [Fact]
    public void ThrowIfInsufficientSpace_WhenInsufficientCapacity_ShouldThrowWithDetails() {
        ulong[] buffer = new ulong[1];
        WriteContext context = new(buffer, 62, 64);

        InsufficientWriteCapacityException? exception = null;
        try { context.ThrowIfInsufficientSpace("UInt", 3); }
        catch (InsufficientWriteCapacityException caughtException) { exception = caughtException; }

        Assert.NotNull(exception);
        InsufficientWriteCapacityException captured = exception;

        Assert.Contains("UInt", captured.Message);
        Assert.Contains("Required bits: 3", captured.Message);
        Assert.Contains("Available bits: 2", captured.Message);
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

    private static void WriteBits(ulong[] buffer, int position, ulong value, int count) {
        for (int i = 0; i < count; i++) {
            bool bit = ((value >> i) & 1UL) != 0;
            WriteBit(buffer, position + i, bit);
        }
    }

    private static void WriteBits(ulong[] buffer, int position, ReadOnlySpan<ulong> source, int count) {
        for (int i = 0; i < count; i++) {
            int sourceWord = i / 64;
            int sourceBit = i % 64;
            bool bit = ((source[sourceWord] >> sourceBit) & 1UL) != 0;
            WriteBit(buffer, position + i, bit);
        }
    }

    private static void WriteBit(ulong[] buffer, int position, bool bit) {
        int ulongIndex = position / 64;
        int bitOffset = position % 64;
        ulong mask = 1UL << bitOffset;

        if (bit) { buffer[ulongIndex] |= mask; }
        else { buffer[ulongIndex] &= ~mask; }
    }
}
