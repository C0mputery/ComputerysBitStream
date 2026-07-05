namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthULongExtensionsTests : VariableLengthExtensionTestSuite<ulong> {
    protected override ulong Value => 42UL;
    protected override ulong[] Values => [42UL, 0UL, 1000000000UL];
    protected override int GetSize(ulong value) => PrimitiveVariableLengthULongExtensions.GetVariableLengthULongSize(value);

    protected override void WritePrimitive(ref WriteContext context, ulong value) => context.WriteVariableLengthULongPrimitive(value);
    protected override ulong PeekPrimitive(ReadContext context) => context.PeekVariableLengthULongPrimitive();
    protected override ulong ReadPrimitive(ReadContext context) => context.ReadVariableLengthULongPrimitive();
    protected override void Write(ref WriteContext context, ulong value) => context.WriteVariableLengthULong(value);
    protected override ulong Peek(ReadContext context) => context.PeekVariableLengthULong();
    protected override ulong Read(ReadContext context) => context.ReadVariableLengthULong();

    protected override ulong TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthULong(out ulong v));
        return v;
    }

    protected override ulong TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthULong(out ulong v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<ulong> values) => context.WriteVariableLengthULongsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<ulong> destination) => context.PeekVariableLengthULongSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<ulong> destination) => context.ReadVariableLengthULongSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<ulong> values) => context.WriteVariableLengthULongsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) => context.PeekVariableLengthULongs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) => context.ReadVariableLengthULongs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) { Assert.True(context.TryPeekVariableLengthULongs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) { Assert.True(context.TryReadVariableLengthULongs(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<ulong> values) => context.WriteVariableLengthULongs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<ulong> destination) => context.PeekVariableLengthULongs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<ulong> destination) => context.ReadVariableLengthULongs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<ulong> destination) { Assert.True(context.TryPeekVariableLengthULongs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<ulong> destination) { Assert.True(context.TryReadVariableLengthULongs(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, ulong[] values) => context.WriteVariableLengthULongsPrimitive(values);
    protected override ulong[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthULongArrayPrimitive(count);
    protected override ulong[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthULongArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, ulong[] values) => context.WriteVariableLengthULongsWithoutLength(values);
    protected override ulong[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthULongs(count);
    protected override ulong[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthULongs(count);

    protected override ulong[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthULongs(count, out ulong[] values));
        return values;
    }

    protected override ulong[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthULongs(count, out ulong[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, ulong[] values) => context.WriteVariableLengthULongs(values);
    protected override ulong[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthULongs();
    protected override ulong[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthULongs();

    protected override ulong[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthULongs(out ulong[] values));
        return values;
    }

    protected override ulong[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthULongs(out ulong[] values));
        return values;
    }

    protected override TryReadOperationSet<ulong> TryOperations => new() {
        TryPeekValue = (ReadContext c, out ulong v) => c.TryPeekVariableLengthULong(out v),
        TryReadValue = (ReadContext c, out ulong v) => c.TryReadVariableLengthULong(out v),
        TryPeekArrayWithLength = (ReadContext c, out ulong[] v) => c.TryPeekVariableLengthULongs(out v),
        TryReadArrayWithLength = (ReadContext c, out ulong[] v) => c.TryReadVariableLengthULongs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out ulong[] v) => c.TryPeekVariableLengthULongs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out ulong[] v) => c.TryReadVariableLengthULongs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<ulong> d) => c.TryPeekVariableLengthULongs(d),
        TryReadSpanWithLength = (ReadContext c, Span<ulong> d) => c.TryReadVariableLengthULongs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<ulong> d) => c.TryPeekVariableLengthULongs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<ulong> d) => c.TryReadVariableLengthULongs(count, d),
    };
}
