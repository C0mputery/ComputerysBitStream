namespace ComputerysBitStream.Tests.Utilities;

public delegate void WriteValueDelegate<T>(ref WriteContext context, T value);

public delegate void WriteSpanDelegate<T>(ref WriteContext context, Span<T> values);

public delegate void WriteArrayDelegate<T>(ref WriteContext context, T[] values);

public delegate void SpanDestinationDelegate<T>(ReadContext context, Span<T> destination);

public delegate void FixedSpanDestinationDelegate<T>(ReadContext context, int count, Span<T> destination);

public delegate bool TryPeekValueDelegate<T>(ReadContext context, out T value);

public delegate bool TryReadValueDelegate<T>(ReadContext context, out T value);

public delegate bool TryPeekArrayDelegate<T>(ReadContext context, out T[] values);

public delegate bool TryReadArrayDelegate<T>(ReadContext context, out T[] values);

public delegate bool TryPeekFixedArrayDelegate<T>(ReadContext context, int count, out T[] values);

public delegate bool TryReadFixedArrayDelegate<T>(ReadContext context, int count, out T[] values);

public delegate bool TryPeekSpanDelegate<T>(ReadContext context, Span<T> destination);

public delegate bool TryReadSpanDelegate<T>(ReadContext context, Span<T> destination);

public delegate bool TryPeekFixedSpanDelegate<T>(ReadContext context, int count, Span<T> destination);

public delegate bool TryReadFixedSpanDelegate<T>(ReadContext context, int count, Span<T> destination);

public sealed class SerializationOperations<T> {
    public required WriteValueDelegate<T> Write { get; init; }
    public required Func<ReadContext, T> Peek { get; init; }
    public required Func<ReadContext, T> Read { get; init; }
    public required TryPeekValueDelegate<T> TryPeek { get; init; }
    public required TryReadValueDelegate<T> TryRead { get; init; }

    public required WriteSpanDelegate<T> WriteSpan { get; init; }
    public required SpanDestinationDelegate<T> PeekSpan { get; init; }
    public required SpanDestinationDelegate<T> ReadSpan { get; init; }
    public required TryPeekSpanDelegate<T> TryPeekSpan { get; init; }
    public required TryReadSpanDelegate<T> TryReadSpan { get; init; }

    public required WriteSpanDelegate<T> WriteSpanWithoutLength { get; init; }
    public required FixedSpanDestinationDelegate<T> PeekSpanWithoutLength { get; init; }
    public required FixedSpanDestinationDelegate<T> ReadSpanWithoutLength { get; init; }
    public required TryPeekFixedSpanDelegate<T> TryPeekSpanWithoutLength { get; init; }
    public required TryReadFixedSpanDelegate<T> TryReadSpanWithoutLength { get; init; }

    public required FixedSpanDestinationDelegate<T> PeekSpanWithMaxCount { get; init; }
    public required FixedSpanDestinationDelegate<T> ReadSpanWithMaxCount { get; init; }
    public required TryPeekFixedSpanDelegate<T> TryPeekSpanWithMaxCount { get; init; }
    public required TryReadFixedSpanDelegate<T> TryReadSpanWithMaxCount { get; init; }

    public required WriteArrayDelegate<T> WriteArray { get; init; }
    public required Func<ReadContext, T[]> PeekArray { get; init; }
    public required Func<ReadContext, T[]> ReadArray { get; init; }
    public required TryPeekArrayDelegate<T> TryPeekArray { get; init; }
    public required TryReadArrayDelegate<T> TryReadArray { get; init; }

    public required WriteArrayDelegate<T> WriteArrayWithoutLength { get; init; }
    public required Func<ReadContext, int, T[]> PeekArrayWithoutLength { get; init; }
    public required Func<ReadContext, int, T[]> ReadArrayWithoutLength { get; init; }
    public required TryPeekFixedArrayDelegate<T> TryPeekArrayWithoutLength { get; init; }
    public required TryReadFixedArrayDelegate<T> TryReadArrayWithoutLength { get; init; }

    public required Func<ReadContext, int, T[]> PeekArrayWithMaxCount { get; init; }
    public required Func<ReadContext, int, T[]> ReadArrayWithMaxCount { get; init; }
    public required TryPeekFixedArrayDelegate<T> TryPeekArrayWithMaxCount { get; init; }
    public required TryReadFixedArrayDelegate<T> TryReadArrayWithMaxCount { get; init; }
}

public sealed class PrimitiveSerializationOperations<T> {
    public required WriteValueDelegate<T> Write { get; init; }
    public required Func<ReadContext, T> Peek { get; init; }
    public required Func<ReadContext, T> Read { get; init; }

    public required WriteSpanDelegate<T> WriteSpan { get; init; }
    public required FixedSpanDestinationDelegate<T> PeekSpan { get; init; }
    public required FixedSpanDestinationDelegate<T> ReadSpan { get; init; }

    public required WriteArrayDelegate<T> WriteArray { get; init; }
    public required Func<ReadContext, int, T[]> PeekArray { get; init; }
    public required Func<ReadContext, int, T[]> ReadArray { get; init; }
}

public static class TryReadOutOfBoundsAssertions<T> {
    public static void AssertSingleFailsWithoutAdvancing(ReadContext context, SerializationOperations<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeek(context, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryRead(context, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertArrayWithLengthFailsWithoutAdvancing(ReadContext context, SerializationOperations<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeekArray(context, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadArray(context, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertFixedLengthArrayFailsWithoutAdvancing(ReadContext context, int count, SerializationOperations<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeekArrayWithoutLength(context, count, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadArrayWithoutLength(context, count, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertSpanWithLengthFailsWithoutAdvancing(ReadContext context, T[] initialValues, SerializationOperations<T> operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        Assert.False(operations.TryPeekSpan(context, destination));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadSpan(context, destination));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertFixedLengthSpanFailsWithoutAdvancing(ReadContext context, T[] initialValues, int count, SerializationOperations<T> operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        Assert.False(operations.TryPeekSpanWithoutLength(context, count, destination));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadSpanWithoutLength(context, count, destination));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertArrayWithMaxCountFailsWithoutAdvancing(ReadContext context, int maxCount, SerializationOperations<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeekArrayWithMaxCount(context, maxCount, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadArrayWithMaxCount(context, maxCount, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertSpanWithMaxCountFailsWithoutAdvancing(ReadContext context, T[] initialValues, int maxCount, SerializationOperations<T> operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        Assert.False(operations.TryPeekSpanWithMaxCount(context, maxCount, destination));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadSpanWithMaxCount(context, maxCount, destination));
        Assert.Equal(originalPosition, context.Position);
    }
}
