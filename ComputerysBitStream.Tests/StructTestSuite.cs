namespace ComputerysBitStream.Tests;

public abstract class StructTestSuite<T> {
    protected abstract T Value { get; }
    protected abstract T[] Values { get; }
    protected virtual int? ExpectedFixedSizeBits => null;

    // Named single-value methods
    protected abstract void WriteNamed(WriteContext context, T value);
    protected abstract T PeekNamed(ReadContext context);
    protected abstract T ReadNamed(ReadContext context);

    // Generic alias single-value methods
    protected abstract void WriteAlias(WriteContext context, T value);
    protected abstract T PeekAlias(ReadContext context);
    protected abstract T ReadAlias(ReadContext context);

    // Try single-value methods
    protected abstract T TryPeekNamed(ReadContext context);
    protected abstract T TryReadNamed(ReadContext context);
    protected abstract T TryPeekAlias(ReadContext context);
    protected abstract T TryReadAlias(ReadContext context);

    // Named array-with-length methods
    protected abstract void WriteArrayNamed(WriteContext context, T[] values);
    protected abstract T[] PeekArrayNamed(ReadContext context);
    protected abstract T[] ReadArrayNamed(ReadContext context);

    // Alias array-with-length methods
    protected abstract void WriteArrayAlias(WriteContext context, T[] values);
    protected abstract T[] PeekArrayAlias(ReadContext context);
    protected abstract T[] ReadArrayAlias(ReadContext context);

    // Try array-with-length methods
    protected abstract T[] TryPeekArrayNamed(ReadContext context);
    protected abstract T[] TryReadArrayNamed(ReadContext context);
    protected abstract T[] TryPeekArrayAlias(ReadContext context);
    protected abstract T[] TryReadArrayAlias(ReadContext context);

    // Named array-without-length methods
    protected abstract void WriteArrayWithoutLengthNamed(WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithoutLengthNamed(ReadContext context, int count);
    protected abstract T[] ReadArrayWithoutLengthNamed(ReadContext context, int count);

    // Alias array-without-length methods
    protected abstract void WriteArrayWithoutLengthAlias(WriteContext context, T[] values);
    protected abstract T[] PeekArrayWithoutLengthAlias(ReadContext context, int count);
    protected abstract T[] ReadArrayWithoutLengthAlias(ReadContext context, int count);

    // Try array-without-length methods
    protected abstract T[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count);
    protected abstract T[] TryReadArrayWithoutLengthNamed(ReadContext context, int count);
    protected abstract T[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count);
    protected abstract T[] TryReadArrayWithoutLengthAlias(ReadContext context, int count);

    // Named span-with-length methods
    protected abstract void WriteSpanNamed(WriteContext context, Span<T> values);
    protected abstract void PeekSpanNamed(ReadContext context, Span<T> destination);
    protected abstract void ReadSpanNamed(ReadContext context, Span<T> destination);

    // Alias span-with-length methods
    protected abstract void WriteSpanAlias(WriteContext context, Span<T> values);
    protected abstract void PeekSpanAlias(ReadContext context, Span<T> destination);
    protected abstract void ReadSpanAlias(ReadContext context, Span<T> destination);

    // Try span-with-length methods
    protected abstract void TryPeekSpanNamed(ReadContext context, Span<T> destination);
    protected abstract void TryReadSpanNamed(ReadContext context, Span<T> destination);
    protected abstract void TryPeekSpanAlias(ReadContext context, Span<T> destination);
    protected abstract void TryReadSpanAlias(ReadContext context, Span<T> destination);

    // Named span-without-length methods
    protected abstract void WriteSpanWithoutLengthNamed(WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<T> destination);
    protected abstract void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<T> destination);

    // Alias span-without-length methods
    protected abstract void WriteSpanWithoutLengthAlias(WriteContext context, Span<T> values);
    protected abstract void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<T> destination);
    protected abstract void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<T> destination);

    // Try span-without-length methods
    protected abstract void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<T> destination);
    protected abstract void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<T> destination);
    protected abstract void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<T> destination);
    protected abstract void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<T> destination);

    // Fixed-size metadata methods
    protected abstract int GetSizeInBits(T value);
    protected abstract bool IsFixedSizeStruct(T value);

    [Fact]
    public void ShouldReportCorrectFixedSize() {
        if (ExpectedFixedSizeBits is null) { return; }
        T value = Value;
        Assert.Equal(ExpectedFixedSizeBits.Value, GetSizeInBits(value));
        Assert.True(IsFixedSizeStruct(value));
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Named_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WriteNamed, PeekNamed, ReadNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_Alias_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WriteAlias, PeekAlias, ReadAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_TryNamed_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WriteNamed, TryPeekNamed, TryReadNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSingle_TryAlias_ShouldReturnIdenticalValue(int initialOffset) {
        RoundTripTestHarness<T>.AssertSingleValueRoundTrip(initialOffset, Value, WriteNamed, TryPeekAlias, TryReadAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_Named_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArrayNamed, PeekArrayNamed, ReadArrayNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_Alias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArrayAlias, PeekArrayAlias, ReadArrayAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_TryNamed_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArrayNamed, TryPeekArrayNamed, TryReadArrayNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArray_TryAlias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertArrayRoundTrip(initialOffset, Values, WriteArrayNamed, TryPeekArrayAlias, TryReadArrayAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Named_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLengthNamed, PeekArrayWithoutLengthNamed, ReadArrayWithoutLengthNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_Alias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLengthAlias, PeekArrayWithoutLengthAlias, ReadArrayWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_TryNamed_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLengthNamed, TryPeekArrayWithoutLengthNamed, TryReadArrayWithoutLengthNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadArrayWithoutLength_TryAlias_ShouldReturnIdenticalArray(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthArrayRoundTrip(initialOffset, Values, WriteArrayWithoutLengthNamed, TryPeekArrayWithoutLengthAlias, TryReadArrayWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_Named_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpanNamed, PeekSpanNamed, ReadSpanNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_Alias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpanAlias, PeekSpanAlias, ReadSpanAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_TryNamed_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpanNamed, TryPeekSpanNamed, TryReadSpanNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpan_TryAlias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertSpanRoundTrip(initialOffset, Values, WriteSpanNamed, TryPeekSpanAlias, TryReadSpanAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Named_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLengthNamed, PeekSpanWithoutLengthNamed, ReadSpanWithoutLengthNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_Alias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLengthAlias, PeekSpanWithoutLengthAlias, ReadSpanWithoutLengthAlias);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_TryNamed_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLengthNamed, TryPeekSpanWithoutLengthNamed, TryReadSpanWithoutLengthNamed);
    }

    [Theory]
    [ClassData(typeof(BitOffsetRange))]
    public void WriteAndReadSpanWithoutLength_TryAlias_ShouldReturnIdenticalSpan(int initialOffset) {
        RoundTripTestHarness<T>.AssertFixedLengthSpanRoundTrip(initialOffset, Values, WriteSpanWithoutLengthNamed, TryPeekSpanWithoutLengthAlias, TryReadSpanWithoutLengthAlias);
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
        foreach (SpanReadOperation operation in operations) { operation(context, destination); }
        Assert.Equal(expected, destination.ToArray());
        Assert.Equal(originalPosition, context.Position);
    }

    private static void AssertReadSpanOutOfBoundsAndPositionUnchanged(ReadContext context, T[] initialValues, int count, params FixedLengthSpanReadOperation[] operations) {
        int originalPosition = context.Position;
        T[] expected = initialValues.ToArray();
        Span<T> destination = initialValues.ToArray();
        foreach (FixedLengthSpanReadOperation operation in operations) { operation(context, count, destination); }
        Assert.Equal(expected, destination.ToArray());
        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void WriteSingle_WhenOutOfBounds_ShouldThrow() {
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance(WriteNamed);
    }

    [Fact]
    public void WriteSingleAlias_WhenOutOfBounds_ShouldThrow() {
        AssertSingleWriteOutOfBoundsThrowsAndDoesNotAdvance(WriteAlias);
    }

    [Fact]
    public void ReadSingle_WhenOutOfBounds_ShouldReturnDefaultAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteNamed(writeContext, Value));
        int originalPosition = context.Position;

        Assert.Equal(default, PeekNamed(context));
        Assert.Equal(default, ReadNamed(context));
        Assert.Equal(default, PeekAlias(context));
        Assert.Equal(default, ReadAlias(context));
        Assert.Equal(originalPosition, context.Position);
    }

    [Fact]
    public void WriteSpanAndArray_WhenOutOfBounds_ShouldThrow() {
        int spanWithoutLengthBits = MeasureBitsNeeded(context => WriteSpanWithoutLengthNamed(context, Values));
        int spanWithLengthBits = MeasureBitsNeeded(context => WriteSpanNamed(context, Values));
        int arrayWithoutLengthBits = MeasureBitsNeeded(context => WriteArrayWithoutLengthNamed(context, Values));
        int arrayWithLengthBits = MeasureBitsNeeded(context => WriteArrayNamed(context, Values));

        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithoutLengthBits, context => WriteSpanWithoutLengthNamed(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithoutLengthBits, context => WriteSpanWithoutLengthAlias(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithLengthBits, context => WriteSpanNamed(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(spanWithLengthBits, context => WriteSpanAlias(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithoutLengthBits, context => WriteArrayWithoutLengthNamed(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithoutLengthBits, context => WriteArrayWithoutLengthAlias(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithLengthBits, context => WriteArrayNamed(context, Values));
        AssertOutOfBoundsWriteThrowsAndDoesNotAdvance(arrayWithLengthBits, context => WriteArrayAlias(context, Values));
    }

    [Fact]
    public void ReadArray_WhenOutOfBounds_ShouldReturnEmptyAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteArrayNamed(writeContext, Values));
        AssertReadArrayOutOfBoundsAndPositionUnchanged(context, PeekArrayNamed, ReadArrayNamed, PeekArrayAlias, ReadArrayAlias);
    }

    [Fact]
    public void ReadFixedLengthArray_WhenOutOfBounds_ShouldReturnEmptyAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteArrayWithoutLengthNamed(writeContext, Values));
        int count = Values.Length;
        AssertReadArrayOutOfBoundsAndPositionUnchanged(context, count, PeekArrayWithoutLengthNamed, ReadArrayWithoutLengthNamed, PeekArrayWithoutLengthAlias, ReadArrayWithoutLengthAlias);
    }

    [Fact]
    public void ReadSpan_WhenOutOfBounds_ShouldLeaveDestinationUnchangedAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteSpanNamed(writeContext, Values));
        AssertReadSpanOutOfBoundsAndPositionUnchanged(context, Values, PeekSpanNamed, ReadSpanNamed, PeekSpanAlias, ReadSpanAlias);
    }

    [Fact]
    public void ReadFixedLengthSpan_WhenOutOfBounds_ShouldLeaveDestinationUnchangedAndNotAdvance() {
        ReadContext context = CreateTruncatedReadContext(writeContext => WriteSpanWithoutLengthNamed(writeContext, Values));
        int count = Values.Length;
        AssertReadSpanOutOfBoundsAndPositionUnchanged(context, Values, count, PeekSpanWithoutLengthNamed, ReadSpanWithoutLengthNamed, PeekSpanWithoutLengthAlias, ReadSpanWithoutLengthAlias);
    }

    private delegate void SpanReadOperation(ReadContext context, Span<T> destination);
    private delegate void FixedLengthSpanReadOperation(ReadContext context, int count, Span<T> destination);
}
