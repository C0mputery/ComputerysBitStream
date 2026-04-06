namespace ComputerysBitStream.Tests.Extensions;

public class RawIntExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadIntRaw_ShouldReturnIdenticalValue(int initialOffset) {
        int valueToWrite = -123456789;

        RawRoundTripTestHarness<int>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteIntRaw(value),
            readCtx => readCtx.PeekIntRaw(),
            readCtx => readCtx.ReadIntRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadIntSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        int[] values = [1, 2, -3, int.MaxValue, int.MinValue];

        RawRoundTripTestHarness<int>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteIntsRaw(spanValues),
            (readCtx, count) => readCtx.PeekIntArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadIntSpanRaw(count, ref destination));
    }
}