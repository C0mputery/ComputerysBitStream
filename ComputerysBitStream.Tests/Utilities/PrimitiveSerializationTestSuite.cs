namespace ComputerysBitStream.Tests.Utilities;

public abstract class PrimitiveSerializationTestSuite<T> : SerializationTestSuite<T> {
    protected abstract PrimitiveSerializationOperations<T> PrimitiveOperations { get; }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSingle_Primitive_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, PrimitiveOperations.Write, PrimitiveOperations.Peek, PrimitiveOperations.Read, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpanWithoutLength_Primitive_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, PrimitiveOperations.WriteSpan, PrimitiveOperations.PeekSpan, PrimitiveOperations.ReadSpan, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArrayWithoutLength_Primitive_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, PrimitiveOperations.WriteArray, PrimitiveOperations.PeekArray, PrimitiveOperations.ReadArray, AssertValuesEqualPair);
    }
}
