namespace ComputerysBitStream.Tests.Extensions;

public class RawLongExtensionsTests {
    [Fact]
    public void WriteAndReadLongRaw_ShouldReturnIdenticalValue() {
        long valueToWrite = -1234567890123456789L;

        RawRoundTripTestHarness<long>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteLongRaw(value),
            readCtx => readCtx.PeekLongRaw(),
            readCtx => readCtx.ReadLongRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
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