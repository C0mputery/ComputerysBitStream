namespace ComputerysBitStream.Tests.Extensions;

public class RawDoubleExtensionsTests {
    [Fact]
    public void WriteAndReadDoubleRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        double valueToWrite = -123.456;

        writeCtx.WriteDoubleRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        double peekedValue = readCtx.PeekDoubleRaw();
        double readValue = readCtx.ReadDoubleRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadDoubleSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        double[] values = [1.1, -2.2, double.MaxValue, double.MinValue, double.NaN];

        writeCtx.WriteDoublesRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        double[] peekedValues = readCtx.PeekDoubleArrayRaw(values.Length);
        double[] readValues = new double[values.Length];
        Span<double> readSpan = readValues.AsSpan();
        readCtx.ReadDoubleSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}