namespace ComputerysBitStream.Tests;

public static class RoundTripTestHarness<T> {
    public static void AssertSingleValueRoundTrip(int initialOffset, T valueToWrite, Action<WriteContext, T> writeValue, Func<ReadContext, T> peekValue, Func<ReadContext, T> readValue) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValue(writeCtx, valueToWrite);

        ReadContext readCtx = new(buffer, initialOffset);
        T peekedValue = peekValue(readCtx);
        T readBackValue = readValue(readCtx);

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readBackValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    public static void AssertFixedLengthSpanRoundTrip(int initialOffset, T[] values, Action<WriteContext, Span<T>> writeValues, PeekSpanDelegateWithoutLength peekSpan, ReadSpanDelegateWithoutLength readSpan) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        Span<T> peekValues = new T[values.Length];
        peekSpan(readCtx, values.Length, peekValues);
        Span<T> readValues = new T[values.Length];
        readSpan(readCtx, values.Length, readValues);

        Assert.Equal(values, peekValues.ToArray());
        Assert.Equal(values, readValues.ToArray());
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    public static void AssertSpanRoundTrip(int initialOffset, T[] values, Action<WriteContext, Span<T>> writeValues, PeekSpanDelegate peekSpan, ReadSpanDelegate readSpan) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        Span<T> peekValues = new T[values.Length];
        peekSpan(readCtx, peekValues);
        Span<T> readValues = new T[values.Length];
        readSpan(readCtx, readValues);

        Assert.Equal(values, peekValues.ToArray());
        Assert.Equal(values, readValues.ToArray());
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    public static void AssertFixedLengthArrayRoundTrip(int initialOffset, T[] values, Action<WriteContext, T[]> writeValues, Func<ReadContext, int, T[]> peekValues, Func<ReadContext, int, T[]> readValues) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        T[] peekedValues = peekValues(readCtx, values.Length);
        Assert.Equal(initialOffset, readCtx.Position);

        T[] readBackValues = readValues(readCtx, values.Length);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readBackValues);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    public static void AssertArrayRoundTrip(int initialOffset, T[] values, Action<WriteContext, T[]> writeValues, Func<ReadContext, T[]> peekValues, Func<ReadContext, T[]> readValues) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        T[] peekedValues = peekValues(readCtx);
        Assert.Equal(initialOffset, readCtx.Position);

        T[] readBackValues = readValues(readCtx);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readBackValues);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }
    
    public delegate void PeekSpanDelegate(ReadContext context, Span<T> destination);
    public delegate void PeekSpanDelegateWithoutLength(ReadContext context, int count, Span<T> destination);

    public delegate void ReadSpanDelegate(ReadContext context, Span<T> destination);
    public delegate void ReadSpanDelegateWithoutLength(ReadContext context, int count, Span<T> destination);
}