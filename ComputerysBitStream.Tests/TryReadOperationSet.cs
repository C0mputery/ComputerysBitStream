namespace ComputerysBitStream.Tests;

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

public sealed class TryReadOperationSet<T> {
    public required TryPeekValueDelegate<T> TryPeekValue { get; init; }
    public required TryReadValueDelegate<T> TryReadValue { get; init; }
    public required TryPeekArrayDelegate<T> TryPeekArrayWithLength { get; init; }
    public required TryReadArrayDelegate<T> TryReadArrayWithLength { get; init; }
    public required TryPeekFixedArrayDelegate<T> TryPeekArrayWithoutLength { get; init; }
    public required TryReadFixedArrayDelegate<T> TryReadArrayWithoutLength { get; init; }
    public required TryPeekSpanDelegate<T> TryPeekSpanWithLength { get; init; }
    public required TryReadSpanDelegate<T> TryReadSpanWithLength { get; init; }
    public required TryPeekFixedSpanDelegate<T> TryPeekSpanWithoutLength { get; init; }
    public required TryReadFixedSpanDelegate<T> TryReadSpanWithoutLength { get; init; }
}

public static class TryReadOutOfBoundsAssertions<T> {
    public static void AssertSingleFailsWithoutAdvancing(ReadContext context, TryReadOperationSet<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeekValue(context, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadValue(context, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertArrayWithLengthFailsWithoutAdvancing(ReadContext context, TryReadOperationSet<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeekArrayWithLength(context, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadArrayWithLength(context, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertFixedLengthArrayFailsWithoutAdvancing(ReadContext context, int count, TryReadOperationSet<T> operations) {
        long originalPosition = context.Position;

        Assert.False(operations.TryPeekArrayWithoutLength(context, count, out _));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadArrayWithoutLength(context, count, out _));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertSpanWithLengthFailsWithoutAdvancing(ReadContext context, T[] initialValues, TryReadOperationSet<T> operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        Assert.False(operations.TryPeekSpanWithLength(context, destination));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadSpanWithLength(context, destination));
        Assert.Equal(originalPosition, context.Position);
    }

    public static void AssertFixedLengthSpanFailsWithoutAdvancing(ReadContext context, T[] initialValues, int count, TryReadOperationSet<T> operations) {
        long originalPosition = context.Position;
        Span<T> destination = initialValues.ToArray();

        Assert.False(operations.TryPeekSpanWithoutLength(context, count, destination));
        Assert.Equal(originalPosition, context.Position);

        Assert.False(operations.TryReadSpanWithoutLength(context, count, destination));
        Assert.Equal(originalPosition, context.Position);
    }
}
