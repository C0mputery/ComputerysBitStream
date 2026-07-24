using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

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

    protected override SerializationOperations<DateTime> Operations { get; } = new() {
        Write = (ref WriteContext context, DateTime value) => context.WriteDateTime(value),
        Peek = (ReadContext context) => context.PeekDateTime(),
        Read = (ReadContext context) => context.ReadDateTime(),
        TryPeek = (ReadContext context, out DateTime value) => context.TryPeekDateTime(out value),
        TryRead = (ReadContext context, out DateTime value) => context.TryReadDateTime(out value),
        WriteSpan = (ref WriteContext context, Span<DateTime> values) => context.WriteDateTimes(values),
        PeekSpan = (ReadContext context, Span<DateTime> destination) => context.PeekDateTimes(destination),
        ReadSpan = (ReadContext context, Span<DateTime> destination) => context.ReadDateTimes(destination),
        TryPeekSpan = (ReadContext context, Span<DateTime> destination) => context.TryPeekDateTimes(destination),
        TryReadSpan = (ReadContext context, Span<DateTime> destination) => context.TryReadDateTimes(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<DateTime> values) => context.WriteDateTimesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<DateTime> destination) => context.PeekDateTimes(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<DateTime> destination) => context.ReadDateTimes(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<DateTime> destination) => context.TryPeekDateTimes(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<DateTime> destination) => context.TryReadDateTimes(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<DateTime> destination) => context.PeekDateTimesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<DateTime> destination) => context.ReadDateTimesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<DateTime> destination) => context.TryPeekDateTimesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<DateTime> destination) => context.TryReadDateTimesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, DateTime[] values) => context.WriteDateTimes(values),
        PeekArray = (ReadContext context) => context.PeekDateTimes(),
        ReadArray = (ReadContext context) => context.ReadDateTimes(),
        TryPeekArray = (ReadContext context, out DateTime[] values) => context.TryPeekDateTimes(out values),
        TryReadArray = (ReadContext context, out DateTime[] values) => context.TryReadDateTimes(out values),
        WriteArrayWithoutLength = (ref WriteContext context, DateTime[] values) => context.WriteDateTimesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekDateTimes(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadDateTimes(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out DateTime[] values) => context.TryPeekDateTimes(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out DateTime[] values) => context.TryReadDateTimes(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekDateTimesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadDateTimesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out DateTime[] values) => context.TryPeekDateTimesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out DateTime[] values) => context.TryReadDateTimesWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<DateTime> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, DateTime value) => context.WriteDateTimePrimitive(value),
        Peek = (ReadContext context) => context.PeekDateTimePrimitive(),
        Read = (ReadContext context) => context.ReadDateTimePrimitive(),
        WriteSpan = (ref WriteContext context, Span<DateTime> values) => context.WriteDateTimesPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<DateTime> destination) => context.PeekDateTimeSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<DateTime> destination) => context.ReadDateTimeSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, DateTime[] values) => context.WriteDateTimesPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekDateTimeArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadDateTimeArrayPrimitive(count),
    };
}
