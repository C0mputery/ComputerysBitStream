namespace ComputerysBitStream.Tests.Extensions;

public class RawLongExtensionsTests {
    [Fact]
    public void WriteAndReadLongRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        long valueToWrite = -1234567890123456789L;

        writeCtx.WriteLongRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        long peekedValue = readCtx.PeekLongRaw();
        long readValue = readCtx.ReadLongRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadLongSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        long[] values = [1L, 2L, -3L, long.MaxValue, long.MinValue];

        writeCtx.WriteLongsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        long[] peekedValues = readCtx.PeekLongArrayRaw(values.Length);
        long[] readValues = new long[values.Length];
        Span<long> readSpan = readValues.AsSpan();
        readCtx.ReadLongSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}