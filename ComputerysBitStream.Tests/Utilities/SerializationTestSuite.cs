namespace ComputerysBitStream.Tests.Utilities;

public abstract class SerializationTestSuite<T> {
    protected abstract T Value { get; }

    protected abstract T[] Values { get; }

    protected abstract SerializationOperations<T> Operations { get; }

    protected virtual bool SupportsOutOfBoundsTests => true;
    protected virtual Type? MetadataStructType => null;
    protected virtual int? ExpectedFixedSizeBits => null;
    protected virtual int? GetEncodedSize(T value) => null;
    protected virtual void AssertValuesEqual(T expected, T actual) => Assert.Equal(expected, actual);

    protected void AssertValuesEqual(T[] expected, T[] actual) {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++) {
            AssertValuesEqual(expected[i], actual[i]);
        }
    }

    protected void AssertValuesEqualPair(T expected, T actual) => AssertValuesEqual(expected, actual);

    protected void AssertValuesEqualPair(T[] expected, T[] actual) => AssertValuesEqual(expected, actual);

    public static IEnumerable<object[]> InitialOffsetData() =>
        Enumerable.Range(0, 128).Select(static offset => new object[] { offset });

    private T AssertTryPeek(ReadContext context) {
        Assert.True(Operations.TryPeek(context, out T value));
        return value;
    }

    private T AssertTryRead(ReadContext context) {
        Assert.True(Operations.TryRead(context, out T value));
        return value;
    }

    private void AssertTryPeekSpan(ReadContext context, Span<T> destination) =>
        Assert.True(Operations.TryPeekSpan(context, destination));

    private void AssertTryReadSpan(ReadContext context, Span<T> destination) =>
        Assert.True(Operations.TryReadSpan(context, destination));

    private void AssertTryPeekSpanWithoutLength(ReadContext context, int count, Span<T> destination) =>
        Assert.True(Operations.TryPeekSpanWithoutLength(context, count, destination));

    private void AssertTryReadSpanWithoutLength(ReadContext context, int count, Span<T> destination) =>
        Assert.True(Operations.TryReadSpanWithoutLength(context, count, destination));

    private void AssertTryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<T> destination) =>
        Assert.True(Operations.TryPeekSpanWithMaxCount(context, maxCount, destination));

    private void AssertTryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<T> destination) =>
        Assert.True(Operations.TryReadSpanWithMaxCount(context, maxCount, destination));

    private T[] AssertTryPeekArray(ReadContext context) {
        Assert.True(Operations.TryPeekArray(context, out T[] values));
        return values;
    }

    private T[] AssertTryReadArray(ReadContext context) {
        Assert.True(Operations.TryReadArray(context, out T[] values));
        return values;
    }

    private T[] AssertTryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(Operations.TryPeekArrayWithoutLength(context, count, out T[] values));
        return values;
    }

    private T[] AssertTryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(Operations.TryReadArrayWithoutLength(context, count, out T[] values));
        return values;
    }

    private T[] AssertTryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(Operations.TryPeekArrayWithMaxCount(context, maxCount, out T[] values));
        return values;
    }

    private T[] AssertTryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(Operations.TryReadArrayWithMaxCount(context, maxCount, out T[] values));
        return values;
    }

    [Fact]
    public void ShouldReportCorrectFixedSize() {
        if (ExpectedFixedSizeBits is null || MetadataStructType is null) { return; }
        Assert.Equal(ExpectedFixedSizeBits.Value, StructMetadataAssertions.GetMetadataSize(MetadataStructType));
        Assert.True(StructMetadataAssertions.IsFixedSize(MetadataStructType));
    }

    [Fact]
    public void Size_ShouldMatchActualBitsWritten() {
        if (GetEncodedSize(Value) is not int expectedSize) { return; }
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext context = new(buffer);
        long start = context.Position;
        Operations.Write(ref context, Value);
        Assert.Equal(expectedSize, context.Position - start);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSingle_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Operations.Write, Operations.Peek, Operations.Read, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSingle_Try_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Operations.Write, AssertTryPeek, AssertTryRead, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpanWithoutLength_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, Operations.WriteSpanWithoutLength, Operations.PeekSpanWithoutLength, Operations.ReadSpanWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpanWithoutLength_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, Operations.WriteSpanWithoutLength, AssertTryPeekSpanWithoutLength, AssertTryReadSpanWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArrayWithoutLength_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, Operations.WriteArrayWithoutLength, Operations.PeekArrayWithoutLength, Operations.ReadArrayWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArrayWithoutLength_Try_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, Operations.WriteArrayWithoutLength, AssertTryPeekArrayWithoutLength, AssertTryReadArrayWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpan_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, Operations.WriteSpan, Operations.PeekSpan, Operations.ReadSpan, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpan_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, Operations.WriteSpan, AssertTryPeekSpan, AssertTryReadSpan, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpan_WithMaxCount_ShouldReturnIdenticalSpan(int initialOffset) {
        int maxCount = Values.Length + 10;
        RoundTripTestHarness<T>.AssertSpanWithMaxCountRoundTrip(initialOffset, Values, maxCount, Operations.WriteSpan, Operations.PeekSpanWithMaxCount, Operations.ReadSpanWithMaxCount, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpan_WithMaxCount_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        int maxCount = Values.Length + 10;
        RoundTripTestHarness<T>.AssertSpanWithMaxCountRoundTrip(initialOffset, Values, maxCount, Operations.WriteSpan, AssertTryPeekSpanWithMaxCount, AssertTryReadSpanWithMaxCount, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArray_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, Operations.WriteArray, Operations.PeekArray, Operations.ReadArray, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArray_Try_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, Operations.WriteArray, AssertTryPeekArray, AssertTryReadArray, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArray_WithMaxCount_ShouldReturnIdenticalArray(int initialOffset) {
        int maxCount = Values.Length + 10;
        RoundTripTestHarness<T>.AssertArrayWithMaxCountRoundTrip(initialOffset, Values, maxCount, Operations.WriteArray, Operations.PeekArrayWithMaxCount, Operations.ReadArrayWithMaxCount, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArray_WithMaxCount_Try_ShouldReturnIdenticalArray(int initialOffset) {
        int maxCount = Values.Length + 10;
        RoundTripTestHarness<T>.AssertArrayWithMaxCountRoundTrip(initialOffset, Values, maxCount, Operations.WriteArray, AssertTryPeekArrayWithMaxCount, AssertTryReadArrayWithMaxCount, AssertValuesEqualPair);
    }

    private long MeasureSingleWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.Write(ref writeContext, Value);
        return writeContext.Position;
    }

    private long MeasureArrayWithLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteArray(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureArrayWithoutLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteArrayWithoutLength(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureSpanWithLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteSpan(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureSpanWithoutLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteSpanWithoutLength(ref writeContext, Values);
        return writeContext.Position;
    }

    private delegate void RefWriteContextAction(ref WriteContext context);

    private static void AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(long bitsNeeded, RefWriteContextAction writeOperation) {
        Assert.True(bitsNeeded > 0, "Write operation must require at least one bit.");
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext context = new(buffer, 0, bitsNeeded - 1);
        long originalPosition = context.Position;
        try {
            writeOperation(ref context);
            Assert.Fail("Expected an InsufficientWriteCapacityException.");
        }
        catch (InsufficientWriteCapacityException) { }
        Assert.Equal(originalPosition, context.Position);
    }

    private void AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance() {
        long bitsNeeded = MeasureSingleWriteBits();
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(bitsNeeded, (ref WriteContext context) => Operations.Write(ref context, Value));
    }

    private ReadContext CreateTruncatedReadContextForSingle() {
        long bitsWritten = MeasureSingleWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.Write(ref writeContext, Value);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateReadContextForArrayWithLength() {
        long bitsWritten = MeasureArrayWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteArray(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten);
    }

    private ReadContext CreateTruncatedReadContextForArrayWithLength() {
        long bitsWritten = MeasureArrayWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteArray(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForArrayWithoutLength() {
        long bitsWritten = MeasureArrayWithoutLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteArrayWithoutLength(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForSpanWithLength() {
        long bitsWritten = MeasureSpanWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteSpan(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForSpanWithoutLength() {
        long bitsWritten = MeasureSpanWithoutLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Operations.WriteSpanWithoutLength(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private static void AssertReadArrayWithMaxCountThrows(ReadContext context, int maxCount, params Func<ReadContext, int, T[]>[] operations) {
        long originalPosition = context.Position;
        foreach (Func<ReadContext, int, T[]> operation in operations) {
            try {
                operation(context, maxCount);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanWithMaxCountThrows(ReadContext context, T[] initialValues, int maxCount, params FixedSpanDestinationDelegate<T>[] operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();
        foreach (FixedSpanDestinationDelegate<T> operation in operations) {
            try {
                operation(context, maxCount, destination);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadArrayOutOfBoundsThrows(ReadContext context, params Func<ReadContext, T[]>[] operations) {
        long originalPosition = context.Position;
        foreach (Func<ReadContext, T[]> operation in operations) {
            try {
                operation(context);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadArrayOutOfBoundsThrows(ReadContext context, int count, params Func<ReadContext, int, T[]>[] operations) {
        long originalPosition = context.Position;
        foreach (Func<ReadContext, int, T[]> operation in operations) {
            try {
                operation(context, count);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanOutOfBoundsThrows(ReadContext context, T[] initialValues, params SpanDestinationDelegate<T>[] operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();
        foreach (SpanDestinationDelegate<T> operation in operations) {
            try {
                operation(context, destination);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanOutOfBoundsThrows(ReadContext context, T[] initialValues, int count, params FixedSpanDestinationDelegate<T>[] operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();
        foreach (FixedSpanDestinationDelegate<T> operation in operations) {
            try {
                operation(context, count, destination);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }
        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void WriteSingle_WhenOutOfBounds_ShouldThrow() {
        if (!SupportsOutOfBoundsTests) { return; }
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance();
    }

    [Fact]
    public void ReadSingle_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSingle();
        long originalPosition = context.Position;
        try {
            Operations.Peek(context);
            Assert.Fail("Expected InsufficientReadSpaceException or BitStreamReadException.");
        }
        catch (InsufficientReadSpaceException) { }
        catch (BitStreamReadException) { }
        try {
            Operations.Read(context);
            Assert.Fail("Expected InsufficientReadSpaceException or BitStreamReadException.");
        }
        catch (InsufficientReadSpaceException) { }
        catch (BitStreamReadException) { }
        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void TryReadSingle_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSingle();
        TryReadOutOfBoundsAssertions<T>.AssertSingleFailsWithoutAdvancing(context, Operations);
    }

    [Fact]
    public void WriteSpanAndArray_WhenOutOfBounds_ShouldThrow() {
        if (!SupportsOutOfBoundsTests) { return; }
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureSpanWithoutLengthWriteBits(), (ref WriteContext context) => Operations.WriteSpanWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureSpanWithLengthWriteBits(), (ref WriteContext context) => Operations.WriteSpan(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureArrayWithoutLengthWriteBits(), (ref WriteContext context) => Operations.WriteArrayWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureArrayWithLengthWriteBits(), (ref WriteContext context) => Operations.WriteArray(ref context, Values));
    }

    [Fact]
    public void ReadArray_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithLength();
        AssertReadArrayOutOfBoundsThrows(context, Operations.PeekArray, Operations.ReadArray);
    }

    [Fact]
    public void TryReadArray_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertArrayWithLengthFailsWithoutAdvancing(context, Operations);
    }

    [Fact]
    public void ReadFixedLengthArray_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithoutLength();
        int count = Values.Length;
        AssertReadArrayOutOfBoundsThrows(context, count, Operations.PeekArrayWithoutLength, Operations.ReadArrayWithoutLength);
    }

    [Fact]
    public void TryReadFixedLengthArray_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithoutLength();
        TryReadOutOfBoundsAssertions<T>.AssertFixedLengthArrayFailsWithoutAdvancing(context, Values.Length, Operations);
    }

    [Fact]
    public void ReadSpan_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithLength();
        AssertReadSpanOutOfBoundsThrows(context, Values, Operations.PeekSpan, Operations.ReadSpan);
    }

    [Fact]
    public void TryReadSpan_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertSpanWithLengthFailsWithoutAdvancing(context, Values, Operations);
    }

    [Fact]
    public void ReadFixedLengthSpan_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithoutLength();
        AssertReadSpanOutOfBoundsThrows(context, Values, Values.Length, Operations.PeekSpanWithoutLength, Operations.ReadSpanWithoutLength);
    }

    [Fact]
    public void TryReadArray_WithMaxCountExceeded_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        int maxCount = Values.Length - 1;
        Assert.True(maxCount >= 0);
        TryReadOutOfBoundsAssertions<T>.AssertArrayWithMaxCountFailsWithoutAdvancing(context, maxCount, Operations);
    }

    [Fact]
    public void ReadArray_WithMaxCountExceeded_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        int maxCount = Values.Length - 1;
        Assert.True(maxCount >= 0);
        AssertReadArrayWithMaxCountThrows(context, maxCount, Operations.PeekArrayWithMaxCount, Operations.ReadArrayWithMaxCount);
    }

    [Fact]
    public void TryReadArray_WithNegativeMaxCount_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertArrayWithMaxCountFailsWithoutAdvancing(context, -1, Operations);
    }

    [Fact]
    public void ReadArray_WithNegativeMaxCount_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        AssertReadArrayWithMaxCountThrows(context, -1, Operations.PeekArrayWithMaxCount, Operations.ReadArrayWithMaxCount);
    }

    [Fact]
    public void TryReadSpan_WithMaxCountExceeded_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        int maxCount = Values.Length - 1;
        Assert.True(maxCount >= 0);
        TryReadOutOfBoundsAssertions<T>.AssertSpanWithMaxCountFailsWithoutAdvancing(context, Values, maxCount, Operations);
    }

    [Fact]
    public void ReadSpan_WithMaxCountExceeded_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        int maxCount = Values.Length - 1;
        Assert.True(maxCount >= 0);
        AssertReadSpanWithMaxCountThrows(context, Values, maxCount, Operations.PeekSpanWithMaxCount, Operations.ReadSpanWithMaxCount);
    }

    [Fact]
    public void TryReadSpan_WithNegativeMaxCount_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertSpanWithMaxCountFailsWithoutAdvancing(context, Values, -1, Operations);
    }

    [Fact]
    public void ReadSpan_WithNegativeMaxCount_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateReadContextForArrayWithLength();
        AssertReadSpanWithMaxCountThrows(context, Values, -1, Operations.PeekSpanWithMaxCount, Operations.ReadSpanWithMaxCount);
    }

    [Fact]
    public void TryReadFixedLengthSpan_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithoutLength();
        TryReadOutOfBoundsAssertions<T>.AssertFixedLengthSpanFailsWithoutAdvancing(context, Values, Values.Length, Operations);
    }
}
