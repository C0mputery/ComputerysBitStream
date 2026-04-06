namespace ComputerysBitStream.Tests.Extensions;

public class RawUShortExtensionsTests {
    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadUShortRaw_ShouldReturnIdenticalValue(int initialOffset) {
        ushort valueToWrite = 40000;

        RawRoundTripTestHarness<ushort>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteUShortRaw(value),
            readCtx => readCtx.PeekUShortRaw(),
            readCtx => readCtx.ReadUShortRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadUShortSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ushort[] values = [1, 2, ushort.MaxValue, ushort.MinValue];

        RawRoundTripTestHarness<ushort>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteUShortsRaw(spanValues),
            (readCtx, count) => readCtx.PeekUShortArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadUShortSpanRaw(count, ref destination));
    }
}