namespace ComputerysBitStream.Tests.Extensions;

public class RawFloatExtensionsTests {
    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadFloatRaw_ShouldReturnIdenticalValue(int initialOffset) {
        float valueToWrite = 12.34f;

        RawRoundTripTestHarness<float>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteFloatRaw(value),
            readCtx => readCtx.PeekFloatRaw(),
            readCtx => readCtx.ReadFloatRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadFloatSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        float[] values = [1.1f, -2.2f, float.MaxValue, float.MinValue, float.NaN];

        RawRoundTripTestHarness<float>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteFloatsRaw(spanValues),
            (readCtx, count) => readCtx.PeekFloatArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadFloatSpanRaw(count, ref destination));
    }
}