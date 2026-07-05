namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthLongExtensionsTests : VariableLengthExtensionTestSuite<long> {
    protected override long Value => 42L;
    protected override long[] Values => [42L, 0L, -1000000000L];
    protected override int GetSize(long value) => PrimitiveVariableLengthLongExtensions.GetVariableLengthLongSize(value);

    protected override void WritePrimitive(ref WriteContext context, long value) => context.WriteVariableLengthLongPrimitive(value);
    protected override long PeekPrimitive(ReadContext context) => context.PeekVariableLengthLongPrimitive();
    protected override long ReadPrimitive(ReadContext context) => context.ReadVariableLengthLongPrimitive();
    protected override void Write(ref WriteContext context, long value) => context.WriteVariableLengthLong(value);
    protected override long Peek(ReadContext context) => context.PeekVariableLengthLong();
    protected override long Read(ReadContext context) => context.ReadVariableLengthLong();

    protected override long TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthLong(out long v));
        return v;
    }

    protected override long TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthLong(out long v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<long> values) => context.WriteVariableLengthLongsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<long> destination) => context.PeekVariableLengthLongSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<long> destination) => context.ReadVariableLengthLongSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<long> values) => context.WriteVariableLengthLongsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<long> destination) => context.PeekVariableLengthLongs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<long> destination) => context.ReadVariableLengthLongs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<long> destination) { Assert.True(context.TryPeekVariableLengthLongs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<long> destination) { Assert.True(context.TryReadVariableLengthLongs(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<long> values) => context.WriteVariableLengthLongs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<long> destination) => context.PeekVariableLengthLongs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<long> destination) => context.ReadVariableLengthLongs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<long> destination) { Assert.True(context.TryPeekVariableLengthLongs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<long> destination) { Assert.True(context.TryReadVariableLengthLongs(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, long[] values) => context.WriteVariableLengthLongsPrimitive(values);
    protected override long[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthLongArrayPrimitive(count);
    protected override long[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthLongArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, long[] values) => context.WriteVariableLengthLongsWithoutLength(values);
    protected override long[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthLongs(count);
    protected override long[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthLongs(count);

    protected override long[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthLongs(count, out long[] values));
        return values;
    }

    protected override long[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthLongs(count, out long[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, long[] values) => context.WriteVariableLengthLongs(values);
    protected override long[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthLongs();
    protected override long[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthLongs();

    protected override long[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthLongs(out long[] values));
        return values;
    }

    protected override long[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthLongs(out long[] values));
        return values;
    }

    protected override TryReadOperationSet<long> TryOperations => new() {
        TryPeekValue = (ReadContext c, out long v) => c.TryPeekVariableLengthLong(out v),
        TryReadValue = (ReadContext c, out long v) => c.TryReadVariableLengthLong(out v),
        TryPeekArrayWithLength = (ReadContext c, out long[] v) => c.TryPeekVariableLengthLongs(out v),
        TryReadArrayWithLength = (ReadContext c, out long[] v) => c.TryReadVariableLengthLongs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out long[] v) => c.TryPeekVariableLengthLongs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out long[] v) => c.TryReadVariableLengthLongs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<long> d) => c.TryPeekVariableLengthLongs(d),
        TryReadSpanWithLength = (ReadContext c, Span<long> d) => c.TryReadVariableLengthLongs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<long> d) => c.TryPeekVariableLengthLongs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<long> d) => c.TryReadVariableLengthLongs(count, d),
    };
}
