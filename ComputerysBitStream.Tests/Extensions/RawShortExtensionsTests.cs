namespace ComputerysBitStream.Tests.Extensions;

public class RawShortExtensionsTests {
    [Fact]
    public void WriteAndReadShortRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        short valueToWrite = -12345;

        writeCtx.WriteShortRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        short peekedValue = readCtx.PeekShortRaw();
        short readValue = readCtx.ReadShortRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadShortSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        short[] values = [1, -2, short.MaxValue, short.MinValue, 0];

        writeCtx.WriteShortsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        short[] peekedValues = readCtx.PeekShortArrayRaw(values.Length);
        short[] readValues = new short[values.Length];
        Span<short> readSpan = readValues.AsSpan();
        readCtx.ReadShortSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}