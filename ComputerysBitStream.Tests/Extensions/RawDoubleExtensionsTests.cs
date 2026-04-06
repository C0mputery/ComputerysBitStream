namespace ComputerysBitStream.Tests.Extensions;

public class RawDoubleExtensionsTests {
    [Fact]
    public void WriteAndReadDoubleRaw_ShouldReturnIdenticalValue() {
        double valueToWrite = -123.456;

        RawRoundTripTestHarness<double>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteDoubleRaw(value),
            readCtx => readCtx.PeekDoubleRaw(),
            readCtx => readCtx.ReadDoubleRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadDoubleSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        double[] values = [1.1, -2.2, double.MaxValue, double.MinValue, double.NaN];

        RawRoundTripTestHarness<double>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteDoublesRaw(spanValues),
            (readCtx, count) => readCtx.PeekDoubleArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadDoubleSpanRaw(count, ref destination));
    }
}