namespace ComputerysBitStream.Tests.Extensions;

public class RawDecimalExtensionsTests {
    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadDecimalRaw_ShouldReturnIdenticalValue(int initialOffset) {
        decimal valueToWrite = 12345.6789m;

        RawRoundTripTestHarness<decimal>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteDecimalRaw(value),
            readCtx => readCtx.PeekDecimalRaw(),
            readCtx => readCtx.ReadDecimalRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadDecimalSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        decimal[] values = [1.1m, -2.2m, decimal.MaxValue, decimal.MinValue, 0m];

        RawRoundTripTestHarness<decimal>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteDecimalsRaw(spanValues),
            (readCtx, count) => readCtx.PeekDecimalArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadDecimalSpanRaw(count, ref destination));
    }
}