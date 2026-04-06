namespace ComputerysBitStream.Tests.Extensions;

public class RawByteExtensionsTests {
    [Fact]
    public void WriteAndReadByteRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        byte valueToWrite = 200;

        writeCtx.WriteByteRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        byte peekedValue = readCtx.PeekByteRaw();
        byte readValue = readCtx.ReadByteRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadByteSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        byte[] values = [1, 2, 255, 128, 0];

        writeCtx.WriteBytesRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        byte[] peekedValues = readCtx.PeekByteArrayRaw(values.Length);
        byte[] readValues = new byte[values.Length];
        Span<byte> readSpan = readValues.AsSpan();
        readCtx.ReadByteSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}