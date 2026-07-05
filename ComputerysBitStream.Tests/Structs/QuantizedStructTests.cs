namespace ComputerysBitStream.Tests.Structs;

public class QuantizedStructTests {
    private const int Precision = 0;
    private static readonly QuantizedStruct Value = new() { Value = 50f };

    private static readonly QuantizedStruct[] Values = [
        new() { Value = 0f },
        new() { Value = 50f },
        new() { Value = 100f }
    ];

    private static void Write(ref WriteContext context, QuantizedStruct value) => context.WriteQuantizedStruct(value);
    private static QuantizedStruct Peek(ReadContext context) => context.PeekQuantizedStruct();
    private static QuantizedStruct Read(ReadContext context) => context.ReadQuantizedStruct();

    private static void AssertEqual(QuantizedStruct expected, QuantizedStruct actual) =>
        Assert.Equal(expected.Value, actual.Value, Precision);

    private static void AssertEqual(QuantizedStruct[] expected, QuantizedStruct[] actual) {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++) {
            AssertEqual(expected[i], actual[i]);
        }
    }

    [Fact]
    public void ShouldReportCorrectFixedSize() {
        Assert.Equal(8, StructMetadataAssertions.GetMetadataSize(typeof(QuantizedStruct)));
        Assert.True(StructMetadataAssertions.IsFixedSize(typeof(QuantizedStruct)));
    }

    [Theory]
    [ClassData(typeof(ZeroBitOffsetRange))]
    public void WriteAndReadSingle_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<QuantizedStruct>.AssertSingleValueRoundTrip(initialOffset, Value, Write, Peek, Read, AssertEqual);
    }

    [Theory]
    [ClassData(typeof(ZeroBitOffsetRange))]
    public void WriteAndReadSingle_StructPrimitive_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<QuantizedStruct>.AssertSingleValueRoundTrip(
            initialOffset,
            Value,
            static (ref WriteContext context, QuantizedStruct value) => context.WriteQuantizedStructStructPrimitive(value),
            static context => context.PeekQuantizedStructStructPrimitive(),
            static context => context.ReadQuantizedStructStructPrimitive(),
            AssertEqual
        );
    }

    [Theory]
    [ClassData(typeof(ZeroBitOffsetRange))]
    public void WriteAndReadArray_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<QuantizedStruct>.AssertArrayRoundTrip(
            initialOffset,
            Values,
            static (ref WriteContext context, QuantizedStruct[] values) => context.WriteQuantizedStructs(values),
            static context => context.PeekQuantizedStructs(),
            static context => context.ReadQuantizedStructs(),
            AssertEqual
        );
    }

    [Theory]
    [ClassData(typeof(ZeroBitOffsetRange))]
    public void TryReadSingle_ShouldReturnIdenticalValue(int initialOffset) {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer, initialOffset);
        Write(ref writeContext, Value);

        ReadContext readContext = new(buffer, initialOffset);
        Assert.True(readContext.TryPeekQuantizedStruct(out QuantizedStruct peeked));
        AssertEqual(Value, peeked);

        Assert.True(readContext.TryReadQuantizedStruct(out QuantizedStruct read));
        AssertEqual(Value, read);
    }
}
