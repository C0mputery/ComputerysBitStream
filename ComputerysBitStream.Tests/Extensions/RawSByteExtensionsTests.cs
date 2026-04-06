namespace ComputerysBitStream.Tests.Extensions;

public class RawSByteExtensionsTests {
    [Fact]
    public void WriteAndReadSByteRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        sbyte valueToWrite = -100;

        writeCtx.WriteSByteRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        sbyte peekedValue = readCtx.PeekSByteRaw();
        sbyte readValue = readCtx.ReadSByteRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadSByteSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        sbyte[] values = [1, -2, sbyte.MaxValue, sbyte.MinValue, 0];

        writeCtx.WriteSBytesRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        sbyte[] peekedValues = readCtx.PeekSByteArrayRaw(values.Length);
        sbyte[] readValues = new sbyte[values.Length];
        Span<sbyte> readSpan = readValues.AsSpan();
        readCtx.ReadSByteSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}