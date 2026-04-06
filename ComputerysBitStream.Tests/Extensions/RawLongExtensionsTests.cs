using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawLongExtensionsTests : ExtensionTestSuite<long> {
    protected override long Value => 42L;
    protected override long[] Values => [42L, -42L, 42L, 42L, -42L];

    protected override void WriteRaw(WriteContext context, long value) => context.WriteLongRaw(value);
    protected override long PeekRaw(ReadContext context) => context.PeekLongRaw();
    protected override long ReadRaw(ReadContext context) => context.ReadLongRaw();
    protected override void Write(WriteContext context, long value) => context.WriteLong(value);
    protected override long Peek(ReadContext context) => context.PeekLong();
    protected override long Read(ReadContext context) => context.ReadLong();
    protected override void WriteAlias(WriteContext context, long value) => context.Write(value);
    protected override long PeekAlias(ReadContext context) { context.Peek(out long v); return v; }
    protected override long ReadAlias(ReadContext context) { context.Read(out long v); return v; }
    protected override long TryPeek(ReadContext context) { Assert.True(context.TryPeek(out long v)); return v; }
    protected override long TryRead(ReadContext context) { Assert.True(context.TryRead(out long v)); return v; }
    protected override long TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out long v)); return v; }
    protected override long TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out long v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<long> values) => context.WriteLongsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<long> destination) => context.PeekLongSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<long> destination) => context.ReadLongSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<long> values) => context.WriteLongsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<long> destination) => context.PeekLongs(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<long> destination) => context.ReadLongs(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<long> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<long> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<long> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<long> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<long> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<long> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<long> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<long> values) => context.WriteLongs(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<long> destination) => context.PeekLongs(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<long> destination) => context.ReadLongs(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<long> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<long> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<long> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<long> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<long> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<long> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<long> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, long[] values) => context.WriteLongsRaw(values);
    protected override long[] PeekArrayRaw(ReadContext context, int count) => context.PeekLongArrayRaw(count);
    protected override long[] ReadArrayRaw(ReadContext context, int count) => context.ReadLongArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, long[] values) => context.WriteLongsWithoutLength(values);
    protected override long[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekLongs(count);
    protected override long[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadLongs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, long[] values) => context.WriteWithoutLength(values);
    protected override long[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out long[] values); return values; }
    protected override long[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out long[] values); return values; }
    protected override long[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekLongs(count, out long[] values)); return values; }
    protected override long[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadLongs(count, out long[] values)); return values; }
    protected override long[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out long[] values)); return values; }
    protected override long[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out long[] values)); return values; }

    protected override void WriteArray(WriteContext context, long[] values) => context.WriteLongs(values);
    protected override long[] PeekArrayWithLength(ReadContext context) => context.PeekLongs();
    protected override long[] ReadArrayWithLength(ReadContext context) => context.ReadLongs();
    protected override void WriteArrayAlias(WriteContext context, long[] values) => context.Write(values);
    protected override long[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out long[] values); return values; }
    protected override long[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out long[] values); return values; }
    protected override long[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekLongs(out long[] values)); return values; }
    protected override long[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadLongs(out long[] values)); return values; }
    protected override long[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out long[] values)); return values; }
    protected override long[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out long[] values)); return values; }
}
