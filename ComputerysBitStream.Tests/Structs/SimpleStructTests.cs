using Xunit;

namespace ComputerysBitStream.Tests;

public class SimpleStructTests : StructTestSuite<SimpleStruct> {
    protected override SimpleStruct Value => new() { X = 42, Y = 3.14f, Z = true };
    protected override SimpleStruct[] Values => [
        new() { X = 1, Y = 1.0f, Z = true },
        new() { X = 2, Y = 2.0f, Z = false },
        new() { X = 3, Y = 3.0f, Z = true }
    ];

    protected override void WriteNamed(WriteContext context, SimpleStruct value) => context.WriteSimpleStruct(value);
    protected override SimpleStruct PeekNamed(ReadContext context) => context.PeekSimpleStruct();
    protected override SimpleStruct ReadNamed(ReadContext context) => context.ReadSimpleStruct();
    protected override void WriteAlias(WriteContext context, SimpleStruct value) => context.Write(value);
    protected override SimpleStruct PeekAlias(ReadContext context) { context.Peek(out SimpleStruct v); return v; }
    protected override SimpleStruct ReadAlias(ReadContext context) { context.Read(out SimpleStruct v); return v; }
    protected override SimpleStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekSimpleStruct(out SimpleStruct v)); return v; }
    protected override SimpleStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadSimpleStruct(out SimpleStruct v)); return v; }
    protected override SimpleStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out SimpleStruct v)); return v; }
    protected override SimpleStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out SimpleStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, SimpleStruct[] values) => context.WriteSimpleStructs(values);
    protected override SimpleStruct[] PeekArrayNamed(ReadContext context) => context.PeekSimpleStructs();
    protected override SimpleStruct[] ReadArrayNamed(ReadContext context) => context.ReadSimpleStructs();
    protected override void WriteArrayAlias(WriteContext context, SimpleStruct[] values) => context.Write(values);
    protected override SimpleStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out SimpleStruct[] v); return v; }
    protected override SimpleStruct[] ReadArrayAlias(ReadContext context) { context.Read(out SimpleStruct[] v); return v; }
    protected override SimpleStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekSimpleStructs(out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadSimpleStructs(out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out SimpleStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, SimpleStruct[] values) => context.WriteSimpleStructsWithoutLength(values);
    protected override SimpleStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekSimpleStructs(count);
    protected override SimpleStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadSimpleStructs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, SimpleStruct[] values) => context.WriteWithoutLength(values);
    protected override SimpleStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out SimpleStruct[] v); return v; }
    protected override SimpleStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out SimpleStruct[] v); return v; }
    protected override SimpleStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekSimpleStructs(count, out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadSimpleStructs(count, out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out SimpleStruct[] v)); return v; }
    protected override SimpleStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out SimpleStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<SimpleStruct> values) => context.WriteSimpleStructs(values);
    protected override void PeekSpanNamed(ReadContext context, ref Span<SimpleStruct> destination) => context.PeekSimpleStructs(ref destination);
    protected override void ReadSpanNamed(ReadContext context, ref Span<SimpleStruct> destination) => context.ReadSimpleStructs(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<SimpleStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, ref Span<SimpleStruct> destination) => context.Peek(ref destination);
    protected override void ReadSpanAlias(ReadContext context, ref Span<SimpleStruct> destination) => context.Read(ref destination);
    protected override void TryPeekSpanNamed(ReadContext context, ref Span<SimpleStruct> destination) { Assert.True(context.TryPeekSimpleStructs(ref destination)); }
    protected override void TryReadSpanNamed(ReadContext context, ref Span<SimpleStruct> destination) { Assert.True(context.TryReadSimpleStructs(ref destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, ref Span<SimpleStruct> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanAlias(ReadContext context, ref Span<SimpleStruct> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<SimpleStruct> values) => context.WriteSimpleStructsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, ref Span<SimpleStruct> destination) => context.PeekSimpleStructs(count, ref destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, ref Span<SimpleStruct> destination) => context.ReadSimpleStructs(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<SimpleStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<SimpleStruct> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<SimpleStruct> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, ref Span<SimpleStruct> destination) { Assert.True(context.TryPeekSimpleStructs(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, ref Span<SimpleStruct> destination) { Assert.True(context.TryReadSimpleStructs(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<SimpleStruct> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<SimpleStruct> destination) { Assert.True(context.TryRead(count, ref destination)); }

    protected override int GetSizeInBits(SimpleStruct value) => value.GetSimpleStructSizeInBits();
    protected override bool IsFixedSizeStruct(SimpleStruct value) => value.IsSimpleStructFixedSizeStruct();
}
