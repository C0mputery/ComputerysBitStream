namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class LongExtensionsTests : PrimitiveSerializationTestSuite<long> {
    protected override long Value => 42L;
    protected override long[] Values => [42L, -42L, 42L, 42L, -42L];

    protected override void WritePrimitive(ref WriteContext context, long value) => context.WriteLongPrimitive(value);
    protected override long PeekPrimitive(ReadContext context) => context.PeekLongPrimitive();
    protected override long ReadPrimitive(ReadContext context) => context.ReadLongPrimitive();
    protected override void Write(ref WriteContext context, long value) => context.WriteLong(value);
    protected override long Peek(ReadContext context) => context.PeekLong();
    protected override long Read(ReadContext context) => context.ReadLong();

    protected override long TryPeek(ReadContext context) {
        Assert.True(context.TryPeekLong(out long v));
        return v;
    }

    protected override long TryRead(ReadContext context) {
        Assert.True(context.TryReadLong(out long v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<long> values) => context.WriteLongsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<long> destination) => context.PeekLongSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<long> destination) => context.ReadLongSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<long> values) => context.WriteLongsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<long> destination) => context.PeekLongs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<long> destination) => context.ReadLongs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<long> destination) { Assert.True(context.TryPeekLongs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<long> destination) { Assert.True(context.TryReadLongs(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<long> values) => context.WriteLongs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<long> destination) => context.PeekLongs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<long> destination) => context.ReadLongs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<long> destination) { Assert.True(context.TryPeekLongs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<long> destination) { Assert.True(context.TryReadLongs(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, long[] values) => context.WriteLongsPrimitive(values);
    protected override long[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekLongArrayPrimitive(count);
    protected override long[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadLongArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, long[] values) => context.WriteLongsWithoutLength(values);
    protected override long[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekLongs(count);
    protected override long[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadLongs(count);

    protected override long[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekLongs(count, out long[] values));
        return values;
    }

    protected override long[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadLongs(count, out long[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, long[] values) => context.WriteLongs(values);
    protected override long[] PeekArrayWithLength(ReadContext context) => context.PeekLongs();
    protected override long[] ReadArrayWithLength(ReadContext context) => context.ReadLongs();

    protected override long[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekLongs(out long[] values));
        return values;
    }

    protected override long[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadLongs(out long[] values));
        return values;
    }

    protected override TryReadOperationSet<long> TryOperations => new() {
        TryPeekValue = (ReadContext c, out long v) => c.TryPeekLong(out v),
        TryReadValue = (ReadContext c, out long v) => c.TryReadLong(out v),
        TryPeekArrayWithLength = (ReadContext c, out long[] v) => c.TryPeekLongs(out v),
        TryReadArrayWithLength = (ReadContext c, out long[] v) => c.TryReadLongs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out long[] v) => c.TryPeekLongs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out long[] v) => c.TryReadLongs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<long> d) => c.TryPeekLongs(d),
        TryReadSpanWithLength = (ReadContext c, Span<long> d) => c.TryReadLongs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<long> d) => c.TryPeekLongs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<long> d) => c.TryReadLongs(count, d),
    };
}
