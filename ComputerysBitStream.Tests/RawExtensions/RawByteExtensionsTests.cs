namespace ComputerysBitStream.Tests.RawExtensions;

public class RawByteExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadByteRaw_ShouldReturnIdenticalValue(int initialOffset) {
        byte valueToWrite = 200;

        RawRoundTripTestHarness<byte>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteByteRaw(value),
            readCtx => readCtx.PeekByteRaw(),
            readCtx => readCtx.ReadByteRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
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