namespace ComputerysBitStream.Tests.Extensions;

public class RawShortExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadShortRaw_ShouldReturnIdenticalValue(int initialOffset) {
        short valueToWrite = -12345;

        RawRoundTripTestHarness<short>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteShortRaw(value),
            readCtx => readCtx.PeekShortRaw(),
            readCtx => readCtx.ReadShortRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadShortSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        short[] values = [1, -2, short.MaxValue, short.MinValue, 0];

        RawRoundTripTestHarness<short>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteShortsRaw(spanValues),
            (readCtx, count) => readCtx.PeekShortArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadShortSpanRaw(count, ref destination));
    }
}