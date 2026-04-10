namespace ComputerysBitStream.Tests;

public class WriteContextTests {
    private static readonly int[] WriteCounts = [1, 3, 7, 8, 17, 32, 63, 64];

    [Fact]
    public void Constructor_WithBuffer_ShouldSetInitialState() {
        ulong[] buffer = new ulong[3];

        WriteContext context = new(buffer);

        Assert.Equal(0, context.Position);
        Assert.Equal(buffer.Length * BitHelper.ULongSize, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void Constructor_WithPosition_ShouldSetPositionAndCapacity(int initialOffset) {
        ulong[] buffer = new ulong[3];

        WriteContext context = new(buffer, initialOffset);

        Assert.Equal(initialOffset, context.Position);
        Assert.Equal(buffer.Length * BitHelper.ULongSize, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void Constructor_WithPositionAndCapacity_ShouldKeepProvidedValues(int initialOffset) {
        ulong[] buffer = new ulong[3];
        int capacity = buffer.Length * BitHelper.ULongSize - 9;

        WriteContext context = new(buffer, initialOffset, capacity);

        Assert.Equal(initialOffset, context.Position);
        Assert.Equal(capacity, context.Capacity);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteBitRaw_ShouldWriteExpectedBitAndAdvance(int initialOffset) {
        ulong[] buffer = CreatePatternBuffer(4);
        ulong[] expected = buffer.ToArray();
        bool value = (initialOffset & 1) == 0;

        WriteContext context = new(buffer, initialOffset);
        WriteBits(expected, initialOffset, value ? 1UL : 0UL, 1);

        context.WriteBitRaw(value);

        Assert.Equal(expected, buffer);
        Assert.Equal(initialOffset + 1, context.Position);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteBitsRaw_Value_ShouldWriteExpectedBitsAndAdvance(int initialOffset) {
        const ulong value = 0xFEDCBA9876543210UL;

        foreach (int count in WriteCounts) {
            ulong[] buffer = CreatePatternBuffer(4);
            ulong[] expected = buffer.ToArray();
            WriteContext context = new(buffer, initialOffset);

            WriteBits(expected, initialOffset, value, count);
            context.WriteBitsRaw(value, count);

            Assert.Equal(expected, buffer);
            Assert.Equal(initialOffset + count, context.Position);
        }
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteBitsRaw_Span_ShouldWriteExpectedBitsAndAdvance(int initialOffset) {
        ulong[] source = [0x0123456789ABCDEFUL, 0xFEDCBA9876543210UL, 0x0F0F0F0F0F0F0F0FUL];
        const int count = 130;

        ulong[] buffer = CreatePatternBuffer(6);
        ulong[] expected = buffer.ToArray();

        WriteContext context = new(buffer, initialOffset);
        WriteBits(expected, initialOffset, source, count);

        context.WriteBitsRaw(source, count);

        Assert.Equal(expected, buffer);
        Assert.Equal(initialOffset + count, context.Position);
    }

    [Fact]
    public void ReserveBitsRaw_ShouldAdvancePosition() {
        ulong[] buffer = new ulong[2];
        WriteContext context = new(buffer, 5);

        context.ReserveBitsRaw(12);

        Assert.Equal(17, context.Position);
    }

    [Fact]
    public void SetPositionRaw_ShouldSetPosition() {
        ulong[] buffer = new ulong[2];
        WriteContext context = new(buffer);

        context.SetPositionRaw(23);

        Assert.Equal(23, context.Position);
    }

    [Fact]
    public void ToBytesRaw_ShouldIncludeGarbageBitsInLastByte() {
        ulong[] buffer = new ulong[1];
        buffer[0] = ulong.MaxValue;

        WriteContext context = new(buffer, 10);
        Span<byte> bytes = context.ToBytesRaw();

        Assert.Equal(2, bytes.Length);
        Assert.Equal((byte)0xFF, bytes[1]);
    }

    [Fact]
    public void ToBytes_ShouldMaskUnusedBitsInLastByte() {
        ulong[] buffer = new ulong[1];
        buffer[0] = ulong.MaxValue;

        WriteContext context = new(buffer, 10);
        Span<byte> bytes = context.ToBytes();

        Assert.Equal(2, bytes.Length);
        Assert.Equal((byte)0x03, bytes[1]);
    }

    [Fact]
    public void ThrowIfNoSpace_WhenEnoughCapacity_ShouldNotThrow() {
        ulong[] buffer = new ulong[1];
        WriteContext context = new(buffer, 60, 64);

        context.ThrowIfNoSpace("UInt", 4);
    }

    [Fact]
    public void ThrowIfNoSpace_WhenInsufficientCapacity_ShouldThrowWithDetails() {
        ulong[] buffer = new ulong[1];
        WriteContext context = new(buffer, 62, 64);

        InsufficientWriteSpaceException? exception = null;
        try { context.ThrowIfNoSpace("UInt", 3); }
        catch (InsufficientWriteSpaceException ex) { exception = ex; }

        Assert.NotNull(exception);
        InsufficientWriteSpaceException captured = exception;

        Assert.Contains("UInt", captured.Message);
        Assert.Contains("Required bits: 3", captured.Message);
        Assert.Contains("Available bits: 2", captured.Message);
    }

    private static ulong[] CreatePatternBuffer(int length = 4) {
        ulong[] buffer = new ulong[length];
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




