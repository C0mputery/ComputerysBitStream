namespace ComputerysBitStream.Tests.Extensions;

public class RawSByteExtensionsTests {
    [Fact]
    public void WriteAndReadSByteRaw_ShouldReturnIdenticalValue() {
        sbyte valueToWrite = -100;

        RawRoundTripTestHarness<sbyte>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteSByteRaw(value),
            readCtx => readCtx.PeekSByteRaw(),
            readCtx => readCtx.ReadSByteRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadSByteSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        sbyte[] values = [1, -2, sbyte.MaxValue, sbyte.MinValue, 0];

        RawRoundTripTestHarness<sbyte>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteSBytesRaw(spanValues),
            (readCtx, count) => readCtx.PeekSByteArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadSByteSpanRaw(count, ref destination));
    }
}