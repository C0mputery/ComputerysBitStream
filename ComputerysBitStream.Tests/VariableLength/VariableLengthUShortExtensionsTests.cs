namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthUShortExtensionsTests : VariableLengthExtensionTestSuite<ushort> {
    protected override ushort Value => 42;
    protected override ushort[] Values => [42, 0, 50000];
    protected override int GetSize(ushort value) => PrimitiveVariableLengthUShortExtensions.GetVariableLengthUShortSize(value);

    protected override void WritePrimitive(ref WriteContext context, ushort value) => context.WriteVariableLengthUShortPrimitive(value);
    protected override ushort PeekPrimitive(ReadContext context) => context.PeekVariableLengthUShortPrimitive();
    protected override ushort ReadPrimitive(ReadContext context) => context.ReadVariableLengthUShortPrimitive();
    protected override void Write(ref WriteContext context, ushort value) => context.WriteVariableLengthUShort(value);
    protected override ushort Peek(ReadContext context) => context.PeekVariableLengthUShort();
    protected override ushort Read(ReadContext context) => context.ReadVariableLengthUShort();

    protected override ushort TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthUShort(out ushort v));
        return v;
    }

    protected override ushort TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthUShort(out ushort v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<ushort> values) => context.WriteVariableLengthUShortsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<ushort> destination) => context.PeekVariableLengthUShortSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<ushort> destination) => context.ReadVariableLengthUShortSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<ushort> values) => context.WriteVariableLengthUShortsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) => context.PeekVariableLengthUShorts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) => context.ReadVariableLengthUShorts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) { Assert.True(context.TryPeekVariableLengthUShorts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) { Assert.True(context.TryReadVariableLengthUShorts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<ushort> values) => context.WriteVariableLengthUShorts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<ushort> destination) => context.PeekVariableLengthUShorts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<ushort> destination) => context.ReadVariableLengthUShorts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<ushort> destination) { Assert.True(context.TryPeekVariableLengthUShorts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<ushort> destination) { Assert.True(context.TryReadVariableLengthUShorts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, ushort[] values) => context.WriteVariableLengthUShortsPrimitive(values);
    protected override ushort[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthUShortArrayPrimitive(count);
    protected override ushort[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthUShortArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, ushort[] values) => context.WriteVariableLengthUShortsWithoutLength(values);
    protected override ushort[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthUShorts(count);
    protected override ushort[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthUShorts(count);

    protected override ushort[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthUShorts(count, out ushort[] values));
        return values;
    }

    protected override ushort[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthUShorts(count, out ushort[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, ushort[] values) => context.WriteVariableLengthUShorts(values);
    protected override ushort[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthUShorts();
    protected override ushort[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthUShorts();

    protected override ushort[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthUShorts(out ushort[] values));
        return values;
    }

    protected override ushort[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthUShorts(out ushort[] values));
        return values;
    }

    protected override TryReadOperationSet<ushort> TryOperations => new() {
        TryPeekValue = (ReadContext c, out ushort v) => c.TryPeekVariableLengthUShort(out v),
        TryReadValue = (ReadContext c, out ushort v) => c.TryReadVariableLengthUShort(out v),
        TryPeekArrayWithLength = (ReadContext c, out ushort[] v) => c.TryPeekVariableLengthUShorts(out v),
        TryReadArrayWithLength = (ReadContext c, out ushort[] v) => c.TryReadVariableLengthUShorts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out ushort[] v) => c.TryPeekVariableLengthUShorts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out ushort[] v) => c.TryReadVariableLengthUShorts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<ushort> d) => c.TryPeekVariableLengthUShorts(d),
        TryReadSpanWithLength = (ReadContext c, Span<ushort> d) => c.TryReadVariableLengthUShorts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<ushort> d) => c.TryPeekVariableLengthUShorts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<ushort> d) => c.TryReadVariableLengthUShorts(count, d),
    };
}
