namespace ComputerysBitStream.Tests.Extensions;

public class RawUShortExtensionsTests {
    [Fact]
    public void WriteAndReadUShortRaw_ShouldReturnIdenticalValue() {
        ushort valueToWrite = 40000;

        RawRoundTripTestHarness<ushort>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteUShortRaw(value),
            readCtx => readCtx.PeekUShortRaw(),
            readCtx => readCtx.ReadUShortRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
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