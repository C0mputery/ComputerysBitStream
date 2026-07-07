namespace ComputerysBitStream.Tests;

public abstract class PrimitiveSerializationTestSuite<T> : SerializationTestSuite<T> {
    protected abstract void WritePrimitive(ref WriteContext context, T value);
    protected abstract T PeekPrimitive(ReadContext context);
    protected abstract T ReadPrimitive(ReadContext context);
    protected abstract void WriteSpanPrimitive(ref WriteContext context, Span<T> values);
    protected abstract void PeekSpanPrimitive(ReadContext context, int count, Span<T> destination);
    protected abstract void ReadSpanPrimitive(ReadContext context, int count, Span<T> destination);
    protected abstract void WriteArrayPrimitive(ref WriteContext context, T[] values);
    protected abstract T[] PeekArrayPrimitive(ReadContext context, int count);
    protected abstract T[] ReadArrayPrimitive(ReadContext context, int count);

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSingle_Primitive_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WritePrimitive, PeekPrimitive, ReadPrimitive, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpanWithoutLength_Primitive_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanPrimitive, PeekSpanPrimitive, ReadSpanPrimitive, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArrayWithoutLength_Primitive_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayPrimitive, PeekArrayPrimitive, ReadArrayPrimitive, AssertValuesEqualPair);
    }

    private void AssertValuesEqualPair(T expected, T actual) => AssertValuesEqual(expected, actual);
    private void AssertValuesEqualPair(T[] expected, T[] actual) => AssertValuesEqual(expected, actual);
}
