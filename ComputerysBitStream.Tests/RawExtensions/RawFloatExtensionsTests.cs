namespace ComputerysBitStream.Tests.RawExtensions;

public class RawFloatExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
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
    [ClassData(typeof(BitOffsetRange))]
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