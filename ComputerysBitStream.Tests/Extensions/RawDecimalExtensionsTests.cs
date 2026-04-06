namespace ComputerysBitStream.Tests.Extensions;

public class RawDecimalExtensionsTests {
    [Fact]
    public void WriteAndReadDecimalRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        decimal valueToWrite = 12345.6789m;

        writeCtx.WriteDecimalRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        decimal peekedValue = readCtx.PeekDecimalRaw();
        decimal readValue = readCtx.ReadDecimalRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadDecimalSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        decimal[] values = [1.1m, -2.2m, decimal.MaxValue, decimal.MinValue, 0m];

        writeCtx.WriteDecimalsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        decimal[] peekedValues = readCtx.PeekDecimalArrayRaw(values.Length);
        decimal[] readValues = new decimal[values.Length];
        Span<decimal> readSpan = readValues.AsSpan();
        readCtx.ReadDecimalSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}