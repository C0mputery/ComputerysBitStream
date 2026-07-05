namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthUIntExtensionsTests : VariableLengthExtensionTestSuite<uint> {
    protected override uint Value => 42u;
    protected override uint[] Values => [42u, 0u, 100000u];
    protected override int GetSize(uint value) => PrimitiveVariableLengthUIntExtensions.GetVariableLengthUIntSize(value);

    protected override void WritePrimitive(ref WriteContext context, uint value) => context.WriteVariableLengthUIntPrimitive(value);
    protected override uint PeekPrimitive(ReadContext context) => context.PeekVariableLengthUIntPrimitive();
    protected override uint ReadPrimitive(ReadContext context) => context.ReadVariableLengthUIntPrimitive();
    protected override void Write(ref WriteContext context, uint value) => context.WriteVariableLengthUInt(value);
    protected override uint Peek(ReadContext context) => context.PeekVariableLengthUInt();
    protected override uint Read(ReadContext context) => context.ReadVariableLengthUInt();

    protected override uint TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthUInt(out uint v));
        return v;
    }

    protected override uint TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthUInt(out uint v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<uint> values) => context.WriteVariableLengthUIntsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<uint> destination) => context.PeekVariableLengthUIntSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<uint> destination) => context.ReadVariableLengthUIntSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<uint> values) => context.WriteVariableLengthUIntsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<uint> destination) => context.PeekVariableLengthUInts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<uint> destination) => context.ReadVariableLengthUInts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<uint> destination) { Assert.True(context.TryPeekVariableLengthUInts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<uint> destination) { Assert.True(context.TryReadVariableLengthUInts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<uint> values) => context.WriteVariableLengthUInts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<uint> destination) => context.PeekVariableLengthUInts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<uint> destination) => context.ReadVariableLengthUInts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<uint> destination) { Assert.True(context.TryPeekVariableLengthUInts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<uint> destination) { Assert.True(context.TryReadVariableLengthUInts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, uint[] values) => context.WriteVariableLengthUIntsPrimitive(values);
    protected override uint[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthUIntArrayPrimitive(count);
    protected override uint[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthUIntArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, uint[] values) => context.WriteVariableLengthUIntsWithoutLength(values);
    protected override uint[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthUInts(count);
    protected override uint[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthUInts(count);

    protected override uint[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthUInts(count, out uint[] values));
        return values;
    }

    protected override uint[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthUInts(count, out uint[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, uint[] values) => context.WriteVariableLengthUInts(values);
    protected override uint[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthUInts();
    protected override uint[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthUInts();

    protected override uint[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthUInts(out uint[] values));
        return values;
    }

    protected override uint[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthUInts(out uint[] values));
        return values;
    }

    protected override TryReadOperationSet<uint> TryOperations => new() {
        TryPeekValue = (ReadContext c, out uint v) => c.TryPeekVariableLengthUInt(out v),
        TryReadValue = (ReadContext c, out uint v) => c.TryReadVariableLengthUInt(out v),
        TryPeekArrayWithLength = (ReadContext c, out uint[] v) => c.TryPeekVariableLengthUInts(out v),
        TryReadArrayWithLength = (ReadContext c, out uint[] v) => c.TryReadVariableLengthUInts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out uint[] v) => c.TryPeekVariableLengthUInts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out uint[] v) => c.TryReadVariableLengthUInts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<uint> d) => c.TryPeekVariableLengthUInts(d),
        TryReadSpanWithLength = (ReadContext c, Span<uint> d) => c.TryReadVariableLengthUInts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<uint> d) => c.TryPeekVariableLengthUInts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<uint> d) => c.TryReadVariableLengthUInts(count, d),
    };
}
