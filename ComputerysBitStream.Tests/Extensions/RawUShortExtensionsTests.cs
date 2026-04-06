namespace ComputerysBitStream.Tests.Extensions;

public class RawUShortExtensionsTests {
    [Fact]
    public void WriteAndReadUShortRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        ushort valueToWrite = 40000;

        writeCtx.WriteUShortRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        ushort peekedValue = readCtx.PeekUShortRaw();
        ushort readValue = readCtx.ReadUShortRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadUShortSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        ushort[] values = [1, 2, ushort.MaxValue, ushort.MinValue];

        writeCtx.WriteUShortsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        ushort[] peekedValues = readCtx.PeekUShortArrayRaw(values.Length);
        ushort[] readValues = new ushort[values.Length];
        Span<ushort> readSpan = readValues.AsSpan();
        readCtx.ReadUShortSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}