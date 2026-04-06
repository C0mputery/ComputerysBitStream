namespace ComputerysBitStream.Tests.Extensions;

public class RawUIntExtensionsTests {
    [Fact]
    public void WriteAndReadUIntRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        uint valueToWrite = 4000000000U;

        writeCtx.WriteUIntRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        uint peekedValue = readCtx.PeekUIntRaw();
        uint readValue = readCtx.ReadUIntRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadUIntSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        uint[] values = [1u, 2u, uint.MaxValue, uint.MinValue];

        writeCtx.WriteUIntsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        uint[] peekedValues = readCtx.PeekUIntArrayRaw(values.Length);
        uint[] readValues = new uint[values.Length];
        Span<uint> readSpan = readValues.AsSpan();
        readCtx.ReadUIntSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}