namespace ComputerysBitStream.Tests.RawExtensions;

public class RawCharExtensionsTests {
    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadCharRaw_ShouldReturnIdenticalValue(int initialOffset) {
        char valueToWrite = 'Z';

        RawRoundTripTestHarness<char>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteCharRaw(value),
            readCtx => readCtx.PeekCharRaw(),
            readCtx => readCtx.ReadCharRaw());
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadCharSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        char[] values = ['a', 'b', '1', '\\', '\n'];

        RawRoundTripTestHarness<char>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteCharsRaw(spanValues),
            (readCtx, count) => readCtx.PeekCharArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadCharSpanRaw(count, ref destination));
    }
}