namespace ComputerysBitStream.Tests.Extensions;

public class RawULongExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadULongRaw_ShouldReturnIdenticalValue(int initialOffset) {
        ulong valueToWrite = 9123456789012345678UL;

        RawRoundTripTestHarness<ulong>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteULongRaw(value),
            readCtx => readCtx.PeekULongRaw(),
            readCtx => readCtx.ReadULongRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
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