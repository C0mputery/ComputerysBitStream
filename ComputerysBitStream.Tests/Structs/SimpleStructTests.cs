namespace ComputerysBitStream.Tests;

public class SimpleStructTests : StructTestSuite<SimpleStruct> {
    protected override SimpleStruct Value => new() { X = 42, Y = 3.14f, Z = true };
    protected override SimpleStruct[] Values => [
        new() { X = 1, Y = 1.0f, Z = true },
        new() { X = 2, Y = 2.0f, Z = false },
        new() { X = 3, Y = 3.0f, Z = true }
    ];

    protected override void Write(ref WriteContext context, SimpleStruct value) => context.WriteSimpleStruct(value);
    protected override SimpleStruct Peek(ReadContext context) => context.PeekSimpleStruct();
    protected override SimpleStruct Read(ReadContext context) => context.ReadSimpleStruct();
    protected override SimpleStruct TryPeek(ReadContext context) { Assert.True(context.TryPeekSimpleStruct(out SimpleStruct v)); return v; }
    protected override SimpleStruct TryRead(ReadContext context) { Assert.True(context.TryReadSimpleStruct(out SimpleStruct v)); return v; }

    protected override void WriteArray(ref WriteContext context, SimpleStruct[] values) => context.WriteSimpleStructs(values);
    protected override SimpleStruct[] PeekArrayWithLength(ReadContext context) => context.PeekSimpleStructs();
    protected override SimpleStruct[] ReadArrayWithLength(ReadContext context) => context.ReadSimpleStructs();
    protected override SimpleStruct[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekSimpleStructs(out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadSimpleStructs(out SimpleStruct[] v)); return v; }

    protected override void WriteArrayWithoutLength(ref WriteContext context, SimpleStruct[] values) => context.WriteSimpleStructsWithoutLength(values);
    protected override SimpleStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekSimpleStructs(count);
    protected override SimpleStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadSimpleStructs(count);
    protected override SimpleStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekSimpleStructs(count, out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadSimpleStructs(count, out SimpleStruct[] v)); return v; }

    protected override void WriteSpan(ref WriteContext context, Span<SimpleStruct> values) => context.WriteSimpleStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<SimpleStruct> destination) => context.PeekSimpleStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<SimpleStruct> destination) => context.ReadSimpleStructs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<SimpleStruct> destination) { Assert.True(context.TryPeekSimpleStructs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<SimpleStruct> destination) { Assert.True(context.TryReadSimpleStructs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<SimpleStruct> values) => context.WriteSimpleStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<SimpleStruct> destination) => context.PeekSimpleStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<SimpleStruct> destination) => context.ReadSimpleStructs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<SimpleStruct> destination) { Assert.True(context.TryPeekSimpleStructs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<SimpleStruct> destination) { Assert.True(context.TryReadSimpleStructs(count, destination)); }

    protected override Type StructType => typeof(SimpleStruct);
    protected override TryReadOperationSet<SimpleStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out SimpleStruct v) => c.TryPeekSimpleStruct(out v),
        TryReadValue = (ReadContext c, out SimpleStruct v) => c.TryReadSimpleStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out SimpleStruct[] v) => c.TryPeekSimpleStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out SimpleStruct[] v) => c.TryReadSimpleStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out SimpleStruct[] v) => c.TryPeekSimpleStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out SimpleStruct[] v) => c.TryReadSimpleStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<SimpleStruct> d) => c.TryPeekSimpleStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<SimpleStruct> d) => c.TryReadSimpleStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<SimpleStruct> d) => c.TryPeekSimpleStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<SimpleStruct> d) => c.TryReadSimpleStructs(count, d),
    };
}