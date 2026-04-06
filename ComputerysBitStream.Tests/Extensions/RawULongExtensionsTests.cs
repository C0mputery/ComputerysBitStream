namespace ComputerysBitStream.Tests.Extensions;

public class RawULongExtensionsTests {
    [Fact]
    public void WriteAndReadULongRaw_ShouldReturnIdenticalValue() {
        ulong valueToWrite = 9123456789012345678UL;

        RawRoundTripTestHarness<ulong>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteULongRaw(value),
            readCtx => readCtx.PeekULongRaw(),
            readCtx => readCtx.ReadULongRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadULongSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] values = [1UL, 2UL, ulong.MaxValue, ulong.MinValue];

        RawRoundTripTestHarness<ulong>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteULongsRaw(spanValues),
            (readCtx, count) => readCtx.PeekULongArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadULongSpanRaw(count, ref destination));
    }
}