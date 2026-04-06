namespace ComputerysBitStream.Tests.Extensions;

public class RawULongExtensionsTests {
    [Fact]
    public void WriteAndReadULongRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        ulong valueToWrite = 9123456789012345678UL;

        writeCtx.WriteULongRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        ulong peekedValue = readCtx.PeekULongRaw();
        ulong readValue = readCtx.ReadULongRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadULongSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        ulong[] values = [1UL, 2UL, ulong.MaxValue, ulong.MinValue];

        writeCtx.WriteULongsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        ulong[] peekedValues = readCtx.PeekULongArrayRaw(values.Length);
        ulong[] readValues = new ulong[values.Length];
        Span<ulong> readSpan = readValues.AsSpan();
        readCtx.ReadULongSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}