namespace ComputerysBitStream.Tests.Utilities;

public static class RoundTripTestHarness<T> {
    public static void AssertSingleValueRoundTrip(int initialOffset, T valueToWrite, WriteValueDelegate<T> writeValue, Func<ReadContext, T> peekValue, Func<ReadContext, T> readValue, Action<T, T>? assertEqual = null) {
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

    public static void AssertFixedLengthSpanRoundTrip(int initialOffset, T[] values, WriteSpanDelegate<T> writeValues, FixedSpanDestinationDelegate<T> peekSpan, FixedSpanDestinationDelegate<T> readSpan, Action<T[], T[]>? assertEqual = null) {
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

    public static void AssertSpanRoundTrip(int initialOffset, T[] values, WriteSpanDelegate<T> writeValues, SpanDestinationDelegate<T> peekSpan, SpanDestinationDelegate<T> readSpan, Action<T[], T[]>? assertEqual = null) {
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

    public static void AssertFixedLengthArrayRoundTrip(int initialOffset, T[] values, WriteArrayDelegate<T> writeValues, Func<ReadContext, int, T[]> peekValues, Func<ReadContext, int, T[]> readValues, Action<T[], T[]>? assertEqual = null) {
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

    public static void AssertArrayRoundTrip(int initialOffset, T[] values, WriteArrayDelegate<T> writeValues, Func<ReadContext, T[]> peekValues, Func<ReadContext, T[]> readValues, Action<T[], T[]>? assertEqual = null) {
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

    public static void AssertArrayWithMaxCountRoundTrip(int initialOffset, T[] values, int maxCount, WriteArrayDelegate<T> writeValues, Func<ReadContext, int, T[]> peekValues, Func<ReadContext, int, T[]> readValues, Action<T[], T[]>? assertEqual = null) {
        Action<T[], T[]> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);
        writeValues(ref writeCtx, values);
        ReadContext readCtx = new(buffer, initialOffset);
        T[] peekedValues = peekValues(readCtx, maxCount);
        T[] readBackValues = readValues(readCtx, maxCount);
        assert(values, peekedValues);
        assert(values, readBackValues);
    }

    public static void AssertSpanWithMaxCountRoundTrip(int initialOffset, T[] values, int maxCount, WriteSpanDelegate<T> writeValues, FixedSpanDestinationDelegate<T> peekSpan, FixedSpanDestinationDelegate<T> readSpan, Action<T[], T[]>? assertEqual = null) {
        Action<T[], T[]> assert = assertEqual ?? ((expected, actual) => Assert.Equal(expected, actual));
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer, initialOffset);
        writeValues(ref writeCtx, values);
        ReadContext readCtx = new(buffer, initialOffset);
        Span<T> peekValues = new T[values.Length];
        peekSpan(readCtx, maxCount, peekValues);
        Span<T> readValues = new T[values.Length];
        readSpan(readCtx, maxCount, readValues);
        assert(values, peekValues.ToArray());
        assert(values, readValues.ToArray());
    }
}
