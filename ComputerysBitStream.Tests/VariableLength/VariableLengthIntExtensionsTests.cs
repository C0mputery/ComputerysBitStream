namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthIntExtensionsTests : VariableLengthExtensionTestSuite<int> {
    protected override int Value => 42;
    protected override int[] Values => [42, 0, -100000];
    protected override int GetSize(int value) => PrimitiveVariableLengthIntExtensions.GetVariableLengthIntSize(value);

    protected override void WritePrimitive(ref WriteContext context, int value) => context.WriteVariableLengthIntPrimitive(value);
    protected override int PeekPrimitive(ReadContext context) => context.PeekVariableLengthIntPrimitive();
    protected override int ReadPrimitive(ReadContext context) => context.ReadVariableLengthIntPrimitive();
    protected override void Write(ref WriteContext context, int value) => context.WriteVariableLengthInt(value);
    protected override int Peek(ReadContext context) => context.PeekVariableLengthInt();
    protected override int Read(ReadContext context) => context.ReadVariableLengthInt();

    protected override int TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthInt(out int v));
        return v;
    }

    protected override int TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthInt(out int v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<int> values) => context.WriteVariableLengthIntsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<int> destination) => context.PeekVariableLengthIntSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<int> destination) => context.ReadVariableLengthIntSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<int> values) => context.WriteVariableLengthIntsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<int> destination) => context.PeekVariableLengthInts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<int> destination) => context.ReadVariableLengthInts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryPeekVariableLengthInts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryReadVariableLengthInts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<int> values) => context.WriteVariableLengthInts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<int> destination) => context.PeekVariableLengthInts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<int> destination) => context.ReadVariableLengthInts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<int> destination) { Assert.True(context.TryPeekVariableLengthInts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<int> destination) { Assert.True(context.TryReadVariableLengthInts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, int[] values) => context.WriteVariableLengthIntsPrimitive(values);
    protected override int[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthIntArrayPrimitive(count);
    protected override int[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthIntArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, int[] values) => context.WriteVariableLengthIntsWithoutLength(values);
    protected override int[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthInts(count);
    protected override int[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthInts(count);

    protected override int[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthInts(count, out int[] values));
        return values;
    }

    protected override int[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthInts(count, out int[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, int[] values) => context.WriteVariableLengthInts(values);
    protected override int[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthInts();
    protected override int[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthInts();

    protected override int[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthInts(out int[] values));
        return values;
    }

    protected override int[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthInts(out int[] values));
        return values;
    }

    protected override TryReadOperationSet<int> TryOperations => new() {
        TryPeekValue = (ReadContext c, out int v) => c.TryPeekVariableLengthInt(out v),
        TryReadValue = (ReadContext c, out int v) => c.TryReadVariableLengthInt(out v),
        TryPeekArrayWithLength = (ReadContext c, out int[] v) => c.TryPeekVariableLengthInts(out v),
        TryReadArrayWithLength = (ReadContext c, out int[] v) => c.TryReadVariableLengthInts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out int[] v) => c.TryPeekVariableLengthInts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out int[] v) => c.TryReadVariableLengthInts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<int> d) => c.TryPeekVariableLengthInts(d),
        TryReadSpanWithLength = (ReadContext c, Span<int> d) => c.TryReadVariableLengthInts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<int> d) => c.TryPeekVariableLengthInts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<int> d) => c.TryReadVariableLengthInts(count, d),
    };
}
