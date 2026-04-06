namespace ComputerysBitStream.Tests.Extensions;

public class RawUIntExtensionsTests {
    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void WriteAndReadUIntRaw_ShouldReturnIdenticalValue(int initialOffset) {
        uint valueToWrite = 4000000000U;

        RawRoundTripTestHarness<uint>.AssertSingleValueRoundTrip(
            initialOffset,
            valueToWrite,
            (writeCtx, value) => writeCtx.WriteUIntRaw(value),
            readCtx => readCtx.PeekUIntRaw(),
            readCtx => readCtx.ReadUIntRaw());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
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