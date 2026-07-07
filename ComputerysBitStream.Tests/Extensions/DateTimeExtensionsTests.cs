namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class DateTimeExtensionsTests : PrimitiveSerializationTestSuite<DateTime> {
    protected override DateTime Value => new(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    protected override DateTime[] Values => [
        new(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
        new(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
        new(2024, 6, 15, 23, 59, 59, DateTimeKind.Local),
        new(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc),
        new(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
    ];

    protected override void WritePrimitive(ref WriteContext context, DateTime value) => context.WriteDateTimePrimitive(value);
    protected override DateTime PeekPrimitive(ReadContext context) => context.PeekDateTimePrimitive();
    protected override DateTime ReadPrimitive(ReadContext context) => context.ReadDateTimePrimitive();
    protected override void Write(ref WriteContext context, DateTime value) => context.WriteDateTime(value);
    protected override DateTime Peek(ReadContext context) => context.PeekDateTime();
    protected override DateTime Read(ReadContext context) => context.ReadDateTime();

    protected override DateTime TryPeek(ReadContext context) {
        Assert.True(context.TryPeekDateTime(out DateTime v));
        return v;
    }

    protected override DateTime TryRead(ReadContext context) {
        Assert.True(context.TryReadDateTime(out DateTime v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<DateTime> values) => context.WriteDateTimesPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<DateTime> destination) => context.PeekDateTimeSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<DateTime> destination) => context.ReadDateTimeSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<DateTime> values) => context.WriteDateTimesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) => context.PeekDateTimes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) => context.ReadDateTimes(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) { Assert.True(context.TryPeekDateTimes(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) { Assert.True(context.TryReadDateTimes(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<DateTime> values) => context.WriteDateTimes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<DateTime> destination) => context.PeekDateTimes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<DateTime> destination) => context.ReadDateTimes(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<DateTime> destination) { Assert.True(context.TryPeekDateTimes(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<DateTime> destination) { Assert.True(context.TryReadDateTimes(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, DateTime[] values) => context.WriteDateTimesPrimitive(values);
    protected override DateTime[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekDateTimeArrayPrimitive(count);
    protected override DateTime[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadDateTimeArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, DateTime[] values) => context.WriteDateTimesWithoutLength(values);
    protected override DateTime[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekDateTimes(count);
    protected override DateTime[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadDateTimes(count);

    protected override DateTime[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekDateTimes(count, out DateTime[] values));
        return values;
    }

    protected override DateTime[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadDateTimes(count, out DateTime[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, DateTime[] values) => context.WriteDateTimes(values);
    protected override DateTime[] PeekArrayWithLength(ReadContext context) => context.PeekDateTimes();
    protected override DateTime[] ReadArrayWithLength(ReadContext context) => context.ReadDateTimes();

    protected override DateTime[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekDateTimes(out DateTime[] values));
        return values;
    }

    protected override DateTime[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadDateTimes(out DateTime[] values));
        return values;
    }

    protected override DateTime[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekDateTimesWithMaxCount(maxCount);
    protected override DateTime[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadDateTimesWithMaxCount(maxCount);

    protected override DateTime[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekDateTimesWithMaxCount(maxCount, out DateTime[] values));
        return values;
    }

    protected override DateTime[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadDateTimesWithMaxCount(maxCount, out DateTime[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<DateTime> destination) => context.PeekDateTimesWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<DateTime> destination) => context.ReadDateTimesWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<DateTime> destination) { Assert.True(context.TryPeekDateTimesWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<DateTime> destination) { Assert.True(context.TryReadDateTimesWithMaxCount(maxCount, destination)); }

    protected override TryReadOperationSet<DateTime> TryOperations => new() {
        TryPeekValue = (ReadContext c, out DateTime v) => c.TryPeekDateTime(out v),
        TryReadValue = (ReadContext c, out DateTime v) => c.TryReadDateTime(out v),
        TryPeekArrayWithLength = (ReadContext c, out DateTime[] v) => c.TryPeekDateTimes(out v),
        TryReadArrayWithLength = (ReadContext c, out DateTime[] v) => c.TryReadDateTimes(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out DateTime[] v) => c.TryPeekDateTimes(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out DateTime[] v) => c.TryReadDateTimes(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<DateTime> d) => c.TryPeekDateTimes(d),
        TryReadSpanWithLength = (ReadContext c, Span<DateTime> d) => c.TryReadDateTimes(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<DateTime> d) => c.TryPeekDateTimes(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<DateTime> d) => c.TryReadDateTimes(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out DateTime[] v) => c.TryPeekDateTimesWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out DateTime[] v) => c.TryReadDateTimesWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<DateTime> d) => c.TryPeekDateTimesWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<DateTime> d) => c.TryReadDateTimesWithMaxCount(maxCount, d),
    };
}
