namespace ComputerysBitStream.Tests.Extensions;

public class RawBoolExtensionsTests {
    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadBoolRaw_ShouldReturnIdenticalValue(int initialOffset) {
        bool valueToWrite = true;

        RawRoundTripTestHarness<bool>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteBoolRaw(value),
            readCtx => readCtx.PeekBoolRaw(),
            readCtx => readCtx.ReadBoolRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadBoolSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        bool[] values = [true, false, true, true, false];

        RawRoundTripTestHarness<bool>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteBoolsRaw(spanValues),
            (readCtx, count) => readCtx.PeekBoolArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadBoolSpanRaw(count, ref destination));
    }
}