using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawDateTimeExtensionsTests : ExtensionTestSuite<DateTime> {
    protected override DateTime Value => new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    protected override DateTime[] Values => [
        new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
        new DateTime(2024, 6, 15, 23, 59, 59, DateTimeKind.Local),
        new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc),
        new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
    ];

    protected override void WriteRaw(WriteContext context, DateTime value) => context.WriteDateTimeRaw(value);
    protected override DateTime PeekRaw(ReadContext context) => context.PeekDateTimeRaw();
    protected override DateTime ReadRaw(ReadContext context) => context.ReadDateTimeRaw();
    protected override void Write(WriteContext context, DateTime value) => context.WriteDateTime(value);
    protected override DateTime Peek(ReadContext context) => context.PeekDateTime();
    protected override DateTime Read(ReadContext context) => context.ReadDateTime();
    protected override void WriteAlias(WriteContext context, DateTime value) => context.Write(value);
    protected override DateTime PeekAlias(ReadContext context) { context.Peek(out DateTime v); return v; }
    protected override DateTime ReadAlias(ReadContext context) { context.Read(out DateTime v); return v; }
    protected override DateTime TryPeek(ReadContext context) { Assert.True(context.TryPeek(out DateTime v)); return v; }
    protected override DateTime TryRead(ReadContext context) { Assert.True(context.TryRead(out DateTime v)); return v; }
    protected override DateTime TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out DateTime v)); return v; }
    protected override DateTime TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out DateTime v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<DateTime> values) => context.WriteDateTimesRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, Span<DateTime> destination) => context.PeekDateTimeSpanRaw(count, destination);
    protected override void ReadSpanRaw(ReadContext context, int count, Span<DateTime> destination) => context.ReadDateTimeSpanRaw(count, destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<DateTime> values) => context.WriteDateTimesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) => context.PeekDateTimes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) => context.ReadDateTimes(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<DateTime> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<DateTime> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<DateTime> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<DateTime> destination) { Assert.True(context.TryRead(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<DateTime> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<DateTime> destination) { Assert.True(context.TryRead(count, destination)); }
    protected override void WriteSpan(WriteContext context, Span<DateTime> values) => context.WriteDateTimes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<DateTime> destination) => context.PeekDateTimes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<DateTime> destination) => context.ReadDateTimes(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<DateTime> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, Span<DateTime> destination) => context.Peek(destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, Span<DateTime> destination) => context.Read(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<DateTime> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<DateTime> destination) { Assert.True(context.TryRead(destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, Span<DateTime> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, Span<DateTime> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteArrayRaw(WriteContext context, DateTime[] values) => context.WriteDateTimesRaw(values);
    protected override DateTime[] PeekArrayRaw(ReadContext context, int count) => context.PeekDateTimeArrayRaw(count);
    protected override DateTime[] ReadArrayRaw(ReadContext context, int count) => context.ReadDateTimeArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, DateTime[] values) => context.WriteDateTimesWithoutLength(values);
    protected override DateTime[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekDateTimes(count);
    protected override DateTime[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadDateTimes(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, DateTime[] values) => context.WriteWithoutLength(values);
    protected override DateTime[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out DateTime[] values); return values; }
    protected override DateTime[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out DateTime[] values); return values; }
    protected override DateTime[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekDateTimes(count, out DateTime[] values)); return values; }
    protected override DateTime[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadDateTimes(count, out DateTime[] values)); return values; }
    protected override DateTime[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out DateTime[] values)); return values; }
    protected override DateTime[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out DateTime[] values)); return values; }

    protected override void WriteArray(WriteContext context, DateTime[] values) => context.WriteDateTimes(values);
    protected override DateTime[] PeekArrayWithLength(ReadContext context) => context.PeekDateTimes();
    protected override DateTime[] ReadArrayWithLength(ReadContext context) => context.ReadDateTimes();
    protected override void WriteArrayAlias(WriteContext context, DateTime[] values) => context.Write(values);
    protected override DateTime[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out DateTime[] values); return values; }
    protected override DateTime[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out DateTime[] values); return values; }
    protected override DateTime[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekDateTimes(out DateTime[] values)); return values; }
    protected override DateTime[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadDateTimes(out DateTime[] values)); return values; }
    protected override DateTime[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out DateTime[] values)); return values; }
    protected override DateTime[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out DateTime[] values)); return values; }
}
