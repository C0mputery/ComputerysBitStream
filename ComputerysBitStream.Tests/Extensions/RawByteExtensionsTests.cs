namespace ComputerysBitStream.Tests.Extensions;

public class RawByteExtensionsTests {
    [Fact]
    public void WriteAndReadByteRaw_ShouldReturnIdenticalValue() {
        byte valueToWrite = 200;

        RawRoundTripTestHarness<byte>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteByteRaw(value),
            readCtx => readCtx.PeekByteRaw(),
            readCtx => readCtx.ReadByteRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadByteSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        byte[] values = [1, 2, 255, 128, 0];

        RawRoundTripTestHarness<byte>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteBytesRaw(spanValues),
            (readCtx, count) => readCtx.PeekByteArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadByteSpanRaw(count, ref destination));
    }
}