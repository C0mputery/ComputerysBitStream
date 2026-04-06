namespace ComputerysBitStream.Tests;

public class BitOffsetRange : TheoryData<int> { 
    public BitOffsetRange() { AddRange(Enumerable.Range(0, 16)); }
}

public abstract class ExtensionTestSuite<T> {
    protected abstract T Value { get; }
    protected abstract T[] Values { get; }

    protected abstract void WriteRaw(WriteContext context, T value);
    protected abstract T PeekRaw(ReadContext context);
    protected abstract T ReadRaw(ReadContext context);
    protected abstract void Write(WriteContext context, T value);
    protected abstract T Peek(ReadContext context);
    protected abstract T Read(ReadContext context);
    protected abstract void WriteAlias(WriteContext context, T value);
    protected abstract T PeekAlias(ReadContext context);
    protected abstract T ReadAlias(ReadContext context);
    protected abstract T TryPeek(ReadContext context);
    protected abstract T TryRead(ReadContext context);
    protected abstract T TryPeekAlias(ReadContext context);
    protected abstract T TryReadAlias(ReadContext context);

    protected abstract void WriteSpanRaw(WriteContext context, Span<T> values);
    protected abstract void PeekSpanRaw(ReadContext context, int count, ref Span<T> destination);
    protected abstract void ReadSpanRaw(ReadContext context, int count, ref Span<T> destination);
    protected abstract void WriteSpanWithoutLength(WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithoutLength(ReadContext context, int count, ref Span<T> destination);
    protected abstract void ReadSpanWithoutLength(ReadContext context, int count, ref Span<T> destination);
    protected abstract void WriteSpanWithoutLengthAlias(WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<T> destination);
    protected abstract void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<T> destination);
    protected abstract void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<T> destination);
    protected abstract void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<T> destination);
    protected abstract void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<T> destination);
    protected abstract void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<T> destination);

    protected abstract void WriteArrayRaw(WriteContext context, T[] values);
    protected abstract T[] PeekArrayRaw(ReadContext context, int count);
    protected abstract T[] ReadArrayRaw(ReadContext context, int count);
    protected abstract void WriteArrayWithoutLength(WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithoutLength(ReadContext context, int count);
    protected abstract T[] ReadArrayWithoutLength(ReadContext context, int count);
    protected abstract void WriteArrayWithoutLengthAlias(WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithoutLengthAlias(ReadContext context, int count);
    protected abstract T[] ReadArrayWithoutLengthAlias(ReadContext context, int count);
    protected abstract T[] TryPeekArrayWithoutLength(ReadContext context, int count);
    protected abstract T[] TryReadArrayWithoutLength(ReadContext context, int count);
    protected abstract T[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count);
    protected abstract T[] TryReadArrayWithoutLengthAlias(ReadContext context, int count);

    protected abstract void WriteArray(WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithLength(ReadContext context);
    protected abstract T[] ReadArrayWithLength(ReadContext context);
    protected abstract void WriteArrayAlias(WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithLengthAlias(ReadContext context);
    protected abstract T[] ReadArrayWithLengthAlias(ReadContext context);
    protected abstract T[] TryPeekArrayWithLength(ReadContext context);
    protected abstract T[] TryReadArrayWithLength(ReadContext context);
    protected abstract T[] TryPeekArrayWithLengthAlias(ReadContext context);
    protected abstract T[] TryReadArrayWithLengthAlias(ReadContext context);

    protected abstract void WriteSpan(WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithLength(ReadContext context, ref Span<T> destination);
    protected abstract void ReadSpanWithLength(ReadContext context, ref Span<T> destination);
    protected abstract void WriteSpanAlias(WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithLengthAlias(ReadContext context, ref Span<T> destination);
    protected abstract void ReadSpanWithLengthAlias(ReadContext context, ref Span<T> destination);
    protected abstract void TryPeekSpanWithLength(ReadContext context, ref Span<T> destination);
    protected abstract void TryReadSpanWithLength(ReadContext context, ref Span<T> destination);
    protected abstract void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<T> destination);
    protected abstract void TryReadSpanWithLengthAlias(ReadContext context, ref Span<T> destination);

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Raw_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WriteRaw, PeekRaw, ReadRaw);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, Peek, Read);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Alias_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WriteAlias, PeekAlias, ReadAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Try_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, TryPeek, TryRead);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_TryAlias_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, Write, TryPeekAlias, TryReadAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Raw_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanRaw, PeekSpanRaw, ReadSpanRaw);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, PeekSpanWithoutLength, ReadSpanWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Alias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLengthAlias, PeekSpanWithoutLengthAlias, ReadSpanWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, TryPeekSpanWithoutLength, TryReadSpanWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_TryAlias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLength, TryPeekSpanWithoutLengthAlias, TryReadSpanWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Raw_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayRaw, PeekArrayRaw, ReadArrayRaw);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, PeekArrayWithoutLength, ReadArrayWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Alias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLengthAlias, PeekArrayWithoutLengthAlias, ReadArrayWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Try_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, TryPeekArrayWithoutLength, TryReadArrayWithoutLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_TryAlias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLength, TryPeekArrayWithoutLengthAlias, TryReadArrayWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndRead_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, PeekSpanWithLength, ReadSpanWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndRead_Alias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpanAlias, PeekSpanWithLengthAlias, ReadSpanWithLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndRead_Try_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, TryPeekSpanWithLength, TryReadSpanWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndRead_TryAlias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpan, TryPeekSpanWithLengthAlias, TryReadSpanWithLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, PeekArrayWithLength, ReadArrayWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_Alias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArrayAlias, PeekArrayWithLengthAlias, ReadArrayWithLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_Try_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, TryPeekArrayWithLength, TryReadArrayWithLength);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_TryAlias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArray, TryPeekArrayWithLengthAlias, TryReadArrayWithLengthAlias);
    }

    private static int MeasureBitsNeeded(Action<WriteContext> writeOperation) {
        ulong[] probeBuffer = new ulong[16];
        WriteContext probeContext = new(probeBuffer);
        writeOperation(probeContext);
        return probeContext.Position;
    }

    private static void AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(int bitsNeeded, Action<WriteContext> writeOperation) {
        ulong[] buffer = new ulong[16];
        WriteContext context = new(buffer, 0, bitsNeeded - 1);
        int originalPosition = context.Position;

        try {
            writeOperation(context);
            Assert.Fail("Expected an InsufficientWriteSpaceException.");
        }
        catch (InsufficientWriteSpaceException) { }

        Assert.Equal(originalPosition, context.Position);
    }

    private void AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance(Action<WriteContext, T> writeOperation) {
        int bitsNeeded = MeasureBitsNeeded(context => writeOperation(context, Value));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(bitsNeeded, context => writeOperation(context, Value));
    }

    private ReadContext CreateTruncatedReadContext(Action<WriteContext> writeOperation) {
        ulong[] buffer = new ulong[16];
        WriteContext writeContext = new(buffer);
        writeOperation(writeContext);
        return new ReadContext(buffer, 0, writeContext.Position - 1);
    }

    private static void AssertReadArrayOutOfBoundsAndPositionUnchanged(ReadContext context, params Func<ReadContext, T[]>[] operations) {
        int originalPosition = context.Position;

        foreach (Func<ReadContext, T[]> operation in operations) { Assert.Empty(operation(context)); }

        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadArrayOutOfBoundsAndPositionUnchanged(ReadContext context, int count, params Func<ReadContext, int, T[]>[] operations) {
        int originalPosition = context.Position;

        foreach (Func<ReadContext, int, T[]> operation in operations) { Assert.Empty(operation(context, count)); }

        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanOutOfBoundsAndPositionUnchanged(ReadContext context, T[] initialValues, params SpanReadOperation[] operations) {
        int originalPosition = context.Position;
        T[] expected = initialValues.ToArray();
        Span<T> destination = initialValues.ToArray();

        foreach (SpanReadOperation operation in operations) { operation(context, ref destination); }

        Assert.Equal(expected, destination.ToArray());
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanOutOfBoundsAndPositionUnchanged(ReadContext context, T[] initialValues, int count, params FixedLengthSpanReadOperation[] operations) {
        int originalPosition = context.Position;
        T[] expected = initialValues.ToArray();
        Span<T> destination = initialValues.ToArray();

        foreach (FixedLengthSpanReadOperation operation in operations) { operation(context, count, ref destination); }

        Assert.Equal(expected, destination.ToArray());
        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void WriteSingle_WhenOutOfBounds_ShouldThrow() {
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance(Write);
    }

    [Fact]
    public void WriteSingleAlias_WhenOutOfBounds_ShouldThrow() {
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance(WriteAlias);
    }

    [Fact]
    public void ReadSingle_WhenOutOfBounds_ShouldReturnDefaultAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => Write(writeContext, Value));
        int originalPosition = context.Position;

        Assert.Equal(default, Peek(context));
        Assert.Equal(default, Read(context));
        Assert.Equal(default, PeekAlias(context));
        Assert.Equal(default, ReadAlias(context));
        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void WriteSpanAndArray_WhenOutOfBounds_ShouldThrow() {
        int spanWithoutLengthBits = MeasureBitsNeeded(context => WriteSpanWithoutLength(context, Values));
        int spanWithLengthBits = MeasureBitsNeeded(context => WriteSpan(context, Values));
        int arrayWithoutLengthBits = MeasureBitsNeeded(context => WriteArrayWithoutLength(context, Values));
        int arrayWithLengthBits = MeasureBitsNeeded(context => WriteArray(context, Values));

        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithoutLengthBits, context => WriteSpanWithoutLength(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithoutLengthBits, context => WriteSpanWithoutLengthAlias(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithLengthBits, context => WriteSpan(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithLengthBits, context => WriteSpanAlias(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithoutLengthBits, context => WriteArrayWithoutLength(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithoutLengthBits, context => WriteArrayWithoutLengthAlias(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithLengthBits, context => WriteArray(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithLengthBits, context => WriteArrayAlias(context, Values));
    }

    [Fact]
    public void ReadArray_WhenOutOfBounds_ShouldReturnEmptyAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteArray(writeContext, Values));

        AssertReadArrayOutOfBoundsAndPositionUnchanged(context, PeekArrayWithLength, ReadArrayWithLength, PeekArrayWithLengthAlias, ReadArrayWithLengthAlias);
    }

    [Fact]
    public void ReadFixedLengthArray_WhenOutOfBounds_ShouldReturnEmptyAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteArrayWithoutLength(writeContext, Values));
        int count = Values.Length;

        AssertReadArrayOutOfBoundsAndPositionUnchanged(context, count, PeekArrayWithoutLength, ReadArrayWithoutLength, PeekArrayWithoutLengthAlias, ReadArrayWithoutLengthAlias);
    }

    [Fact]
    public void ReadSpan_WhenOutOfBounds_ShouldLeaveDestinationUnchangedAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteSpan(writeContext, Values));

        AssertReadSpanOutOfBoundsAndPositionUnchanged(context, Values, PeekSpanWithLength, ReadSpanWithLength, PeekSpanWithLengthAlias, ReadSpanWithLengthAlias);
    }

    [Fact]
    public void ReadFixedLengthSpan_WhenOutOfBounds_ShouldLeaveDestinationUnchangedAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteSpanWithoutLength(writeContext, Values));
        int count = Values.Length;

        AssertReadSpanOutOfBoundsAndPositionUnchanged(context, Values, count, PeekSpanWithoutLength, ReadSpanWithoutLength, PeekSpanWithoutLengthAlias, ReadSpanWithoutLengthAlias);
    }

    private delegate void SpanReadOperation(ReadContext context, ref Span<T> destination);

    private delegate void FixedLengthSpanReadOperation(ReadContext context, int count, ref Span<T> destination);
}