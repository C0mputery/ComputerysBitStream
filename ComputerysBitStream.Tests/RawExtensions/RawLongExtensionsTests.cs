namespace ComputerysBitStream.Tests.RawExtensions;

public class RawLongExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadLongRaw_ShouldReturnIdenticalValue(int initialOffset) {
        long valueToWrite = -1234567890123456789L;

        RawRoundTripTestHarness<long>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteLongRaw(value),
            readCtx => readCtx.PeekLongRaw(),
            readCtx => readCtx.ReadLongRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadLongSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        long[] values = [1L, 2L, -3L, long.MaxValue, long.MinValue];

        RawRoundTripTestHarness<long>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteLongsRaw(spanValues),
            (readCtx, count) => readCtx.PeekLongArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadLongSpanRaw(count, ref destination));
    }
}