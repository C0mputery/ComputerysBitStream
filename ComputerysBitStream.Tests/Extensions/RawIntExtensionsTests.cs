namespace ComputerysBitStream.Tests.Extensions;

public class RawIntExtensionsTests {
    [Fact]
    public void WriteAndReadIntRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        int valueToWrite = -123456789;

        writeCtx.WriteIntRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        int peekedValue = readCtx.PeekIntRaw();
        int readValue = readCtx.ReadIntRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadIntSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        int[] values = [1, 2, -3, int.MaxValue, int.MinValue];

        writeCtx.WriteIntsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        int[] peekedValues = readCtx.PeekIntArrayRaw(values.Length);
        int[] readValues = new int[values.Length];
        Span<int> readSpan = readValues.AsSpan();
        readCtx.ReadIntSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}