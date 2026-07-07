namespace ComputerysBitStream.Tests;

public abstract class SerializationTestSuite<T> {
    protected abstract T Value { get; }
    protected abstract T[] Values { get; }
    protected abstract TryReadOperationSet<T> TryOperations { get; }

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

    private void AssertValuesEqualPair(T expected, T actual) => AssertValuesEqual(expected, actual);
    private void AssertValuesEqualPair(T[] expected, T[] actual) => AssertValuesEqual(expected, actual);

    public static IEnumerable<object[]> InitialOffsetData() =>
        Enumerable.Range(0, 128).Select(static offset => new object[] { offset });

    protected abstract void Write(ref WriteContext context, T value);
    protected abstract T Peek(ReadContext context);
    protected abstract T Read(ReadContext context);
    protected abstract T TryPeek(ReadContext context);
    protected abstract T TryRead(ReadContext context);

    protected abstract void WriteSpan(ref WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithLength(ReadContext context, Span<T> destination);
    protected abstract void ReadSpanWithLength(ReadContext context, Span<T> destination);
    protected abstract void TryPeekSpanWithLength(ReadContext context, Span<T> destination);
    protected abstract void TryReadSpanWithLength(ReadContext context, Span<T> destination);

    protected abstract void WriteSpanWithoutLength(ref WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithoutLength(ReadContext context, int count, Span<T> destination);
    protected abstract void ReadSpanWithoutLength(ReadContext context, int count, Span<T> destination);
    protected abstract void TryPeekSpanWithoutLength(ReadContext context, int count, Span<T> destination);
    protected abstract void TryReadSpanWithoutLength(ReadContext context, int count, Span<T> destination);

    protected abstract void WriteArray(ref WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithLength(ReadContext context);
    protected abstract T[] ReadArrayWithLength(ReadContext context);
    protected abstract T[] TryPeekArrayWithLength(ReadContext context);
    protected abstract T[] TryReadArrayWithLength(ReadContext context);

    protected abstract void WriteArrayWithoutLength(ref WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithoutLength(ReadContext context, int count);
    protected abstract T[] ReadArrayWithoutLength(ReadContext context, int count);
    protected abstract T[] TryPeekArrayWithoutLength(ReadContext context, int count);
    protected abstract T[] TryReadArrayWithoutLength(ReadContext context, int count);

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
        Write(ref context, Value);
        Assert.Equal(expectedSize, context.Position - start);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSingle_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, Peek, Read, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSingle_Try_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, TryPeek, TryRead, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpanWithoutLength_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, PeekSpanWithoutLength, ReadSpanWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpanWithoutLength_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, TryPeekSpanWithoutLength, TryReadSpanWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArrayWithoutLength_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, PeekArrayWithoutLength, ReadArrayWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArrayWithoutLength_Try_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, TryPeekArrayWithoutLength, TryReadArrayWithoutLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpan_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, PeekSpanWithLength, ReadSpanWithLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadSpan_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, TryPeekSpanWithLength, TryReadSpanWithLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArray_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, PeekArrayWithLength, ReadArrayWithLength, AssertValuesEqualPair);
    }

    [Theory]
    [MemberData(nameof(InitialOffsetData))]
    public void WriteAndReadArray_Try_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, TryPeekArrayWithLength, TryReadArrayWithLength, AssertValuesEqualPair);
    }

    private long MeasureSingleWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Write(ref writeContext, Value);
        return writeContext.Position;
    }

    private long MeasureArrayWithLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteArray(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureArrayWithoutLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteArrayWithoutLength(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureSpanWithLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteSpan(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureSpanWithoutLengthWriteBits() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteSpanWithoutLength(ref writeContext, Values);
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
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(bitsNeeded, (ref WriteContext context) => Write(ref context, Value));
    }

    private ReadContext CreateTruncatedReadContextForSingle() {
        long bitsWritten = MeasureSingleWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        Write(ref writeContext, Value);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForArrayWithLength() {
        long bitsWritten = MeasureArrayWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteArray(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForArrayWithoutLength() {
        long bitsWritten = MeasureArrayWithoutLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteArrayWithoutLength(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForSpanWithLength() {
        long bitsWritten = MeasureSpanWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteSpan(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForSpanWithoutLength() {
        long bitsWritten = MeasureSpanWithoutLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        WriteSpanWithoutLength(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
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

    private static void AssertReadSpanOutOfBoundsThrows(ReadContext context, T[] initialValues, params SpanReadOperation[] operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        foreach (SpanReadOperation operation in operations) {
            try {
                operation(context, destination);
                Assert.Fail("Expected BitStreamReadException.");
            }
            catch (BitStreamReadException) { }
        }

        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanOutOfBoundsThrows(ReadContext context, T[] initialValues, int count, params FixedLengthSpanReadOperation[] operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        foreach (FixedLengthSpanReadOperation operation in operations) {
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
            Peek(context);
            Assert.Fail("Expected InsufficientReadSpaceException or BitStreamReadException.");
        }
        catch (InsufficientReadSpaceException) { }
        catch (BitStreamReadException) { }

        try {
            Read(context);
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
        TryReadOutOfBoundsAssertions<T>.AssertSingleFailsWithoutAdvancing(context, TryOperations);
    }

    [Fact]
    public void WriteSpanAndArray_WhenOutOfBounds_ShouldThrow() {
        if (!SupportsOutOfBoundsTests) { return; }
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureSpanWithoutLengthWriteBits(), (ref WriteContext context) => WriteSpanWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureSpanWithLengthWriteBits(), (ref WriteContext context) => WriteSpan(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureArrayWithoutLengthWriteBits(), (ref WriteContext context) => WriteArrayWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(MeasureArrayWithLengthWriteBits(), (ref WriteContext context) => WriteArray(ref context, Values));
    }

    [Fact]
    public void ReadArray_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithLength();
        AssertReadArrayOutOfBoundsThrows(context, PeekArrayWithLength, ReadArrayWithLength);
    }

    [Fact]
    public void TryReadArray_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertArrayWithLengthFailsWithoutAdvancing(context, TryOperations);
    }

    [Fact]
    public void ReadFixedLengthArray_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithoutLength();
        int count = Values.Length;
        AssertReadArrayOutOfBoundsThrows(context, count, PeekArrayWithoutLength, ReadArrayWithoutLength);
    }

    [Fact]
    public void TryReadFixedLengthArray_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForArrayWithoutLength();
        TryReadOutOfBoundsAssertions<T>.AssertFixedLengthArrayFailsWithoutAdvancing(context, Values.Length, TryOperations);
    }

    [Fact]
    public void ReadSpan_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithLength();
        AssertReadSpanOutOfBoundsThrows(context, Values, PeekSpanWithLength, ReadSpanWithLength);
    }

    [Fact]
    public void TryReadSpan_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertSpanWithLengthFailsWithoutAdvancing(context, Values, TryOperations);
    }

    [Fact]
    public void ReadFixedLengthSpan_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithoutLength();
        AssertReadSpanOutOfBoundsThrows(context, Values, Values.Length, PeekSpanWithoutLength, ReadSpanWithoutLength);
    }

    [Fact]
    public void TryReadFixedLengthSpan_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        if (!SupportsOutOfBoundsTests) { return; }
        ReadContext context = CreateTruncatedReadContextForSpanWithoutLength();
        TryReadOutOfBoundsAssertions<T>.AssertFixedLengthSpanFailsWithoutAdvancing(context, Values, Values.Length, TryOperations);
    }

    private delegate void SpanReadOperation(ReadContext context, Span<T> destination);

    private delegate void FixedLengthSpanReadOperation(ReadContext context, int count, Span<T> destination);
}
