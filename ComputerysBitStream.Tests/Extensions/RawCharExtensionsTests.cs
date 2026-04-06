namespace ComputerysBitStream.Tests.Extensions;

public class RawCharExtensionsTests {
    [Fact]
    public void WriteAndReadCharRaw_ShouldReturnIdenticalValue() {
        char valueToWrite = 'Z';

        RawRoundTripTestHarness<char>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteCharRaw(value),
            readCtx => readCtx.PeekCharRaw(),
            readCtx => readCtx.ReadCharRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
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