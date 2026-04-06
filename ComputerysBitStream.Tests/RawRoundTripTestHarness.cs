namespace ComputerysBitStream.Tests;

internal static class RawRoundTripTestHarness<T> {
    public static void AssertSingleValueRoundTrip(T valueToWrite, Action<WriteContext, T> writeValue, Func<ReadContext, T> peekValue, Func<ReadContext, T> readValue) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);

        writeValue(writeCtx, valueToWrite);

        ReadContext readCtx = new(buffer);
        T peekedValue = peekValue(readCtx);
        T readBackValue = readValue(readCtx);

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readBackValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    public static void AssertSpanRoundTrip(int initialOffset, T[] values, Action<WriteContext, T[]> writeValues, Func<ReadContext, int, T[]> peekArray, ReadSpanRawDelegate readSpan) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        T[] peekedValues = peekArray(readCtx, values.Length);
        T[] readValues = new T[values.Length];
        Span<T> readBuffer = readValues.AsSpan();
        readSpan(readCtx, values.Length, ref readBuffer);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }

    internal delegate void ReadSpanRawDelegate(ReadContext context, int count, ref Span<T> destination);
}