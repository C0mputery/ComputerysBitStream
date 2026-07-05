namespace ComputerysBitStream.Tests;

public static class RoundTripTestHarness<T> {
    public delegate void WriteValueDelegate(ref WriteContext context, T value);

    public delegate void WriteSpanDelegate(ref WriteContext context, Span<T> values);

    public delegate void WriteArrayDelegate(ref WriteContext context, T[] values);

    public static void AssertSingleValueRoundTrip(int initialOffset, T valueToWrite, WriteValueDelegate writeValue, Func<ReadContext, T> peekValue, Func<ReadContext, T> readValue, Action<T, T>? assertEqual = null) {
        Action<T, T> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValue(ref writeCtx, valueToWrite);

        ReadContext readCtx = new(buffer, initialOffset);
        T peekedValue = peekValue(readCtx);
        T readBackValue = readValue(readCtx);

        assert(valueToWrite, peekedValue);
        assert(valueToWrite, readBackValue);
    }

    public static void AssertFixedLengthSpanRoundTrip(int initialOffset, T[] values, WriteSpanDelegate writeValues, PeekSpanDelegateWithoutLength peekSpan, ReadSpanDelegateWithoutLength readSpan, Action<T[], T[]>? assertEqual = null) {
        Action<T[], T[]> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(ref writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        Span<T> peekValues = new T[values.Length];
        peekSpan(readCtx, values.Length, peekValues);
        Span<T> readValues = new T[values.Length];
        readSpan(readCtx, values.Length, readValues);

        assert(values, peekValues.ToArray());
        assert(values, readValues.ToArray());
    }

    public static void AssertSpanRoundTrip(int initialOffset, T[] values, WriteSpanDelegate writeValues, PeekSpanDelegate peekSpan, ReadSpanDelegate readSpan, Action<T[], T[]>? assertEqual = null) {
        Action<T[], T[]> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(ref writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        Span<T> peekValues = new T[values.Length];
        peekSpan(readCtx, peekValues);
        Span<T> readValues = new T[values.Length];
        readSpan(readCtx, readValues);

        assert(values, peekValues.ToArray());
        assert(values, readValues.ToArray());
    }

    public static void AssertFixedLengthArrayRoundTrip(int initialOffset, T[] values, WriteArrayDelegate writeValues, Func<ReadContext, int, T[]> peekValues, Func<ReadContext, int, T[]> readValues, Action<T[], T[]>? assertEqual = null) {
        Action<T[], T[]> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(ref writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        T[] peekedValues = peekValues(readCtx, values.Length);

        T[] readBackValues = readValues(readCtx, values.Length);

        assert(values, peekedValues);
        assert(values, readBackValues);
    }

    public static void AssertArrayRoundTrip(int initialOffset, T[] values, WriteArrayDelegate writeValues, Func<ReadContext, T[]> peekValues, Func<ReadContext, T[]> readValues, Action<T[], T[]>? assertEqual = null) {
        Action<T[], T[]> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);

        writeValues(ref writeCtx, values);

        ReadContext readCtx = new(buffer, initialOffset);
        T[] peekedValues = peekValues(readCtx);

        T[] readBackValues = readValues(readCtx);

        assert(values, peekedValues);
        assert(values, readBackValues);
    }

    public delegate void PeekSpanDelegate(ReadContext context, Span<T> destination);

    public delegate void PeekSpanDelegateWithoutLength(ReadContext context, int count, Span<T> destination);

    public delegate void ReadSpanDelegate(ReadContext context, Span<T> destination);

    public delegate void ReadSpanDelegateWithoutLength(ReadContext context, int count, Span<T> destination);
}
