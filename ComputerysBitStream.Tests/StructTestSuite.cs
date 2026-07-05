namespace ComputerysBitStream.Tests;

public abstract class StructTestSuite<T> {
    protected abstract T Value { get; }
    protected abstract T[] Values { get; }
    protected virtual int? ExpectedFixedSizeBits => null;
    protected abstract TryReadOperationSet<T> TryOperations { get; }

    protected abstract void Write(ref WriteContext context, T value);
    protected abstract T Peek(ReadContext context);
    protected abstract T Read(ReadContext context);

    protected abstract T TryPeek(ReadContext context);
    protected abstract T TryRead(ReadContext context);

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

    protected abstract Type StructType { get; }

    [Fact]
    public void ShouldReportCorrectFixedSize() {
        if (ExpectedFixedSizeBits is null) { return; }
        Assert.Equal(ExpectedFixedSizeBits.Value, StructMetadataAssertions.GetMetadataSize(StructType));
        Assert.True(StructMetadataAssertions.IsFixedSize(StructType));
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Named_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, Peek, Read);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Alias_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, Peek, Read);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_TryNamed_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, TryPeek, TryRead);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_TryAlias_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, TryPeek, TryRead);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_Named_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, PeekArrayWithLength, ReadArrayWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_Alias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, PeekArrayWithLength, ReadArrayWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_TryNamed_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, TryPeekArrayWithLength, TryReadArrayWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_TryAlias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, TryPeekArrayWithLength, TryReadArrayWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Named_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, PeekArrayWithoutLength, ReadArrayWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Alias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, PeekArrayWithoutLength, ReadArrayWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_TryNamed_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, TryPeekArrayWithoutLength, TryReadArrayWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_TryAlias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, TryPeekArrayWithoutLength, TryReadArrayWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_Named_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, PeekSpanWithLength, ReadSpanWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_Alias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, PeekSpanWithLength, ReadSpanWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_TryNamed_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, TryPeekSpanWithLength, TryReadSpanWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_TryAlias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, TryPeekSpanWithLength, TryReadSpanWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Named_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, PeekSpanWithoutLength, ReadSpanWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Alias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, PeekSpanWithoutLength, ReadSpanWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_TryNamed_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, TryPeekSpanWithoutLength, TryReadSpanWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_TryAlias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, TryPeekSpanWithoutLength, TryReadSpanWithoutLength);
    }

    private long MeasureSingleWriteBits() {
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        Write(ref writeContext, Value);
        return writeContext.Position;
    }

    private long MeasureArrayWithLengthWriteBits() {
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteArray(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureArrayWithoutLengthWriteBits() {
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteArrayWithoutLength(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureSpanWithLengthWriteBits() {
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteSpan(ref writeContext, Values);
        return writeContext.Position;
    }

    private long MeasureSpanWithoutLengthWriteBits() {
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteSpanWithoutLength(ref writeContext, Values);
        return writeContext.Position;
    }

    private delegate void RefWriteContextAction(ref WriteContext context);

    private static void AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(long bitsNeeded, RefWriteContextAction writeOperation) {
        Assert.True(bitsNeeded > 0, "Write operation must require at least one bit.");
        ulong[] buffer = new ulong[16];
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
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        Write(ref writeContext, Value);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForArrayWithLength() {
        long bitsWritten = MeasureArrayWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteArray(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForArrayWithoutLength() {
        long bitsWritten = MeasureArrayWithoutLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteArrayWithoutLength(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForSpanWithLength() {
        long bitsWritten = MeasureSpanWithLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        WriteSpan(ref writeContext, Values);
        return new ReadContext(buffer, 0, bitsWritten - 1);
    }

    private ReadContext CreateTruncatedReadContextForSpanWithoutLength() {
        long bitsWritten = MeasureSpanWithoutLengthWriteBits();
        Assert.True(bitsWritten > 0);
        ulong[] buffer = new ulong[16];
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
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance();
    }

    [Fact]
    public void WriteSingleAlias_WhenOutOfBounds_ShouldThrow() {
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance();
    }

    [Fact]
    public void ReadSingle_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForSingle();
        long originalPosition = context.Position;

        try {
            Peek(context);
            Assert.Fail("Expected InsufficientReadSpaceException.");
        }
        catch (InsufficientReadSpaceException) { }

        try {
            Read(context);
            Assert.Fail("Expected InsufficientReadSpaceException.");
        }
        catch (InsufficientReadSpaceException) { }

        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void TryReadSingle_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForSingle();
        TryReadOutOfBoundsAssertions<T>.AssertSingleFailsWithoutAdvancing(context, TryOperations);
    }

    [Fact]
    public void WriteSpanAndArray_WhenOutOfBounds_ShouldThrow() {
        long spanWithoutLengthBits = MeasureSpanWithoutLengthWriteBits();
        long spanWithLengthBits = MeasureSpanWithLengthWriteBits();
        long arrayWithoutLengthBits = MeasureArrayWithoutLengthWriteBits();
        long arrayWithLengthBits = MeasureArrayWithLengthWriteBits();

        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithoutLengthBits, (ref WriteContext context) => WriteSpanWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithoutLengthBits, (ref WriteContext context) => WriteSpanWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithLengthBits, (ref WriteContext context) => WriteSpan(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithLengthBits, (ref WriteContext context) => WriteSpan(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithoutLengthBits, (ref WriteContext context) => WriteArrayWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithoutLengthBits, (ref WriteContext context) => WriteArrayWithoutLength(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithLengthBits, (ref WriteContext context) => WriteArray(ref context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithLengthBits, (ref WriteContext context) => WriteArray(ref context, Values));
    }

    [Fact]
    public void ReadArray_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForArrayWithLength();
        AssertReadArrayOutOfBoundsThrows(context, PeekArrayWithLength, ReadArrayWithLength, PeekArrayWithLength, ReadArrayWithLength);
    }

    [Fact]
    public void TryReadArray_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForArrayWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertArrayWithLengthFailsWithoutAdvancing(context, TryOperations);
    }

    [Fact]
    public void ReadFixedLengthArray_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForArrayWithoutLength();
        int count = Values.Length;
        AssertReadArrayOutOfBoundsThrows(context, count, PeekArrayWithoutLength, ReadArrayWithoutLength, PeekArrayWithoutLength, ReadArrayWithoutLength);
    }

    [Fact]
    public void TryReadFixedLengthArray_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForArrayWithoutLength();
        TryReadOutOfBoundsAssertions<T>.AssertFixedLengthArrayFailsWithoutAdvancing(context, Values.Length, TryOperations);
    }

    [Fact]
    public void ReadSpan_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForSpanWithLength();
        AssertReadSpanOutOfBoundsThrows(context, Values, PeekSpanWithLength, ReadSpanWithLength, PeekSpanWithLength, ReadSpanWithLength);
    }

    [Fact]
    public void TryReadSpan_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForSpanWithLength();
        TryReadOutOfBoundsAssertions<T>.AssertSpanWithLengthFailsWithoutAdvancing(context, Values, TryOperations);
    }

    [Fact]
    public void ReadFixedLengthSpan_WhenOutOfBounds_ShouldThrowAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForSpanWithoutLength();
        int count = Values.Length;
        AssertReadSpanOutOfBoundsThrows(context, Values, count, PeekSpanWithoutLength, ReadSpanWithoutLength, PeekSpanWithoutLength, ReadSpanWithoutLength);
    }

    [Fact]
    public void TryReadFixedLengthSpan_WhenOutOfBounds_ShouldReturnFalseAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContextForSpanWithoutLength();
        TryReadOutOfBoundsAssertions<T>.AssertFixedLengthSpanFailsWithoutAdvancing(context, Values, Values.Length, TryOperations);
    }

    private delegate void SpanReadOperation(ReadContext context, Span<T> destination);
    private delegate void FixedLengthSpanReadOperation(ReadContext context, int count, Span<T> destination);
}
