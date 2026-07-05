namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthShortExtensionsTests : VariableLengthExtensionTestSuite<short> {
    protected override short Value => 42;
    protected override short[] Values => [42, 0, -1000];
    protected override int GetSize(short value) => PrimitiveVariableLengthShortExtensions.GetVariableLengthShortSize(value);

    protected override void WritePrimitive(ref WriteContext context, short value) => context.WriteVariableLengthShortPrimitive(value);
    protected override short PeekPrimitive(ReadContext context) => context.PeekVariableLengthShortPrimitive();
    protected override short ReadPrimitive(ReadContext context) => context.ReadVariableLengthShortPrimitive();
    protected override void Write(ref WriteContext context, short value) => context.WriteVariableLengthShort(value);
    protected override short Peek(ReadContext context) => context.PeekVariableLengthShort();
    protected override short Read(ReadContext context) => context.ReadVariableLengthShort();

    protected override short TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthShort(out short v));
        return v;
    }

    protected override short TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthShort(out short v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<short> values) => context.WriteVariableLengthShortsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<short> destination) => context.PeekVariableLengthShortSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<short> destination) => context.ReadVariableLengthShortSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<short> values) => context.WriteVariableLengthShortsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<short> destination) => context.PeekVariableLengthShorts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<short> destination) => context.ReadVariableLengthShorts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<short> destination) { Assert.True(context.TryPeekVariableLengthShorts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<short> destination) { Assert.True(context.TryReadVariableLengthShorts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<short> values) => context.WriteVariableLengthShorts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<short> destination) => context.PeekVariableLengthShorts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<short> destination) => context.ReadVariableLengthShorts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<short> destination) { Assert.True(context.TryPeekVariableLengthShorts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<short> destination) { Assert.True(context.TryReadVariableLengthShorts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, short[] values) => context.WriteVariableLengthShortsPrimitive(values);
    protected override short[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthShortArrayPrimitive(count);
    protected override short[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthShortArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, short[] values) => context.WriteVariableLengthShortsWithoutLength(values);
    protected override short[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthShorts(count);
    protected override short[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthShorts(count);

    protected override short[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthShorts(count, out short[] values));
        return values;
    }

    protected override short[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthShorts(count, out short[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, short[] values) => context.WriteVariableLengthShorts(values);
    protected override short[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthShorts();
    protected override short[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthShorts();

    protected override short[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthShorts(out short[] values));
        return values;
    }

    protected override short[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthShorts(out short[] values));
        return values;
    }

    protected override TryReadOperationSet<short> TryOperations => new() {
        TryPeekValue = (ReadContext c, out short v) => c.TryPeekVariableLengthShort(out v),
        TryReadValue = (ReadContext c, out short v) => c.TryReadVariableLengthShort(out v),
        TryPeekArrayWithLength = (ReadContext c, out short[] v) => c.TryPeekVariableLengthShorts(out v),
        TryReadArrayWithLength = (ReadContext c, out short[] v) => c.TryReadVariableLengthShorts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out short[] v) => c.TryPeekVariableLengthShorts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out short[] v) => c.TryReadVariableLengthShorts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<short> d) => c.TryPeekVariableLengthShorts(d),
        TryReadSpanWithLength = (ReadContext c, Span<short> d) => c.TryReadVariableLengthShorts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<short> d) => c.TryPeekVariableLengthShorts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<short> d) => c.TryReadVariableLengthShorts(count, d),
    };
}
