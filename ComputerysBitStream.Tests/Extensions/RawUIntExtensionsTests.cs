namespace ComputerysBitStream.Tests.Extensions;

public class RawUIntExtensionsTests {
    [Fact]
    public void WriteAndReadUIntRaw_ShouldReturnIdenticalValue() {
        uint valueToWrite = 4000000000U;

        RawRoundTripTestHarness<uint>.AssertSingleValueRoundTrip(
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteUIntRaw(value),
            readCtx => readCtx.PeekUIntRaw(),
            readCtx => readCtx.ReadUIntRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadUIntSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        uint[] values = [1u, 2u, uint.MaxValue, uint.MinValue];

        RawRoundTripTestHarness<uint>.AssertSpanRoundTrip(
            initialOffset,
            values,
            (writeCtx, spanValues) => writeCtx.WriteUIntsRaw(spanValues),
            (readCtx, count) => readCtx.PeekUIntArrayRaw(count),
            (readCtx, count, ref destination) => readCtx.ReadUIntSpanRaw(count, ref destination));
    }
}