namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class IntExtensionsTests : ExtensionTestSuite<int> {
        protected override int Value => 42;
        protected override int[] Values => [42, -42, 42, 42, -42];

    protected override void WritePrimitive(ref WriteContext context, int value) => context.WriteIntPrimitive(value);
    protected override int PeekPrimitive(ReadContext context) => context.PeekIntPrimitive();
    protected override int ReadPrimitive(ReadContext context) => context.ReadIntPrimitive();
    protected override void Write(ref WriteContext context, int value) => context.WriteInt(value);
    protected override int Peek(ReadContext context) => context.PeekInt();
    protected override int Read(ReadContext context) => context.ReadInt();
    protected override int TryPeek(ReadContext context) { Assert.True(context.TryPeekInt(out int v)); return v; }
    protected override int TryRead(ReadContext context) { Assert.True(context.TryReadInt(out int v)); return v; }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<int> values) => context.WriteIntsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<int> destination) => context.PeekIntSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<int> destination) => context.ReadIntSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<int> values) => context.WriteIntsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<int> destination) => context.PeekInts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<int> destination) => context.ReadInts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryPeekInts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryReadInts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<int> values) => context.WriteInts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<int> destination) => context.PeekInts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<int> destination) => context.ReadInts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<int> destination) { Assert.True(context.TryPeekInts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<int> destination) { Assert.True(context.TryReadInts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, int[] values) => context.WriteIntsPrimitive(values);
    protected override int[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekIntArrayPrimitive(count);
    protected override int[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadIntArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, int[] values) => context.WriteIntsWithoutLength(values);
    protected override int[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekInts(count);
    protected override int[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadInts(count);
    protected override int[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekInts(count, out int[] values)); return values; }
    protected override int[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadInts(count, out int[] values)); return values; }

    protected override void WriteArray(ref WriteContext context, int[] values) => context.WriteInts(values);
    protected override int[] PeekArrayWithLength(ReadContext context) => context.PeekInts();
    protected override int[] ReadArrayWithLength(ReadContext context) => context.ReadInts();
    protected override int[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekInts(out int[] values)); return values; }
    protected override int[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadInts(out int[] values)); return values; }
    protected override TryReadOperationSet<int> TryOperations => new() {
        TryPeekValue = (ReadContext c, out int v) => c.TryPeekInt(out v),
        TryReadValue = (ReadContext c, out int v) => c.TryReadInt(out v),
        TryPeekArrayWithLength = (ReadContext c, out int[] v) => c.TryPeekInts(out v),
        TryReadArrayWithLength = (ReadContext c, out int[] v) => c.TryReadInts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out int[] v) => c.TryPeekInts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out int[] v) => c.TryReadInts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<int> d) => c.TryPeekInts(d),
        TryReadSpanWithLength = (ReadContext c, Span<int> d) => c.TryReadInts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<int> d) => c.TryPeekInts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<int> d) => c.TryReadInts(count, d),
    };
}
