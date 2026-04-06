namespace ComputerysBitStream.Tests.RawExtensions;

public class RawDoubleExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadDoubleRaw_ShouldReturnIdenticalValue(int initialOffset) {
        double valueToWrite = -123.456;

        RawRoundTripTestHarness<double>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteDoubleRaw(value),
            readCtx => readCtx.PeekDoubleRaw(),
            readCtx => readCtx.ReadDoubleRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
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