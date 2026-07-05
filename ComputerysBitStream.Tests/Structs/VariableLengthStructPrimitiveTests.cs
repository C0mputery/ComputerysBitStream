namespace ComputerysBitStream.Tests.Structs;

[BitStreamPrimitiveContext]
public class VariableLengthStructPrimitiveTests : VariableLengthExtensionTestSuite<VariableLengthStruct> {
    protected override VariableLengthStruct Value => new() { A = 42, B = true };

    protected override VariableLengthStruct[] Values => [
        new() { A = 42, B = true },
        new() { A = 0, B = false },
        new() { A = -100000, B = true }
    ];

    protected override int GetSize(VariableLengthStruct value) =>
        VariableLengthStructStructPrimitiveExtensions.GetVariableLengthStructStructSize(value);

    protected override void WritePrimitive(ref WriteContext context, VariableLengthStruct value) =>
        context.WriteVariableLengthStructStructPrimitive(value);

    protected override VariableLengthStruct PeekPrimitive(ReadContext context) =>
        context.PeekVariableLengthStructStructPrimitive();

    protected override VariableLengthStruct ReadPrimitive(ReadContext context) =>
        context.ReadVariableLengthStructStructPrimitive();

    protected override void Write(ref WriteContext context, VariableLengthStruct value) =>
        context.WriteVariableLengthStruct(value);

    protected override VariableLengthStruct Peek(ReadContext context) =>
        context.PeekVariableLengthStruct();

    protected override VariableLengthStruct Read(ReadContext context) =>
        context.ReadVariableLengthStruct();

    protected override VariableLengthStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthStruct(out VariableLengthStruct v));
        return v;
    }

    protected override VariableLengthStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthStruct(out VariableLengthStruct v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<VariableLengthStruct> values) =>
        context.WriteVariableLengthStructsStructPrimitive(values);

    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<VariableLengthStruct> destination) =>
        context.PeekVariableLengthStructStructSpanPrimitive(count, destination);

    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<VariableLengthStruct> destination) =>
        context.ReadVariableLengthStructStructSpanPrimitive(count, destination);

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<VariableLengthStruct> values) =>
        context.WriteVariableLengthStructsWithoutLength(values);

    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<VariableLengthStruct> destination) =>
        context.PeekVariableLengthStructs(count, destination);

    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<VariableLengthStruct> destination) =>
        context.ReadVariableLengthStructs(count, destination);

    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<VariableLengthStruct> destination) {
        Assert.True(context.TryPeekVariableLengthStructs(count, destination));
    }

    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<VariableLengthStruct> destination) {
        Assert.True(context.TryReadVariableLengthStructs(count, destination));
    }

    protected override void WriteSpan(ref WriteContext context, Span<VariableLengthStruct> values) =>
        context.WriteVariableLengthStructs(values);

    protected override void PeekSpanWithLength(ReadContext context, Span<VariableLengthStruct> destination) =>
        context.PeekVariableLengthStructs(destination);

    protected override void ReadSpanWithLength(ReadContext context, Span<VariableLengthStruct> destination) =>
        context.ReadVariableLengthStructs(destination);

    protected override void TryPeekSpanWithLength(ReadContext context, Span<VariableLengthStruct> destination) {
        Assert.True(context.TryPeekVariableLengthStructs(destination));
    }

    protected override void TryReadSpanWithLength(ReadContext context, Span<VariableLengthStruct> destination) {
        Assert.True(context.TryReadVariableLengthStructs(destination));
    }

    protected override void WriteArrayPrimitive(ref WriteContext context, VariableLengthStruct[] values) =>
        context.WriteVariableLengthStructsStructPrimitive(values);

    protected override VariableLengthStruct[] PeekArrayPrimitive(ReadContext context, int count) =>
        context.PeekVariableLengthStructStructArrayPrimitive(count);

    protected override VariableLengthStruct[] ReadArrayPrimitive(ReadContext context, int count) =>
        context.ReadVariableLengthStructStructArrayPrimitive(count);

    protected override void WriteArrayWithoutLength(ref WriteContext context, VariableLengthStruct[] values) =>
        context.WriteVariableLengthStructsWithoutLength(values);

    protected override VariableLengthStruct[] PeekArrayWithoutLength(ReadContext context, int count) =>
        context.PeekVariableLengthStructs(count);

    protected override VariableLengthStruct[] ReadArrayWithoutLength(ReadContext context, int count) =>
        context.ReadVariableLengthStructs(count);

    protected override VariableLengthStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthStructs(count, out VariableLengthStruct[] values));
        return values;
    }

    protected override VariableLengthStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthStructs(count, out VariableLengthStruct[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, VariableLengthStruct[] values) =>
        context.WriteVariableLengthStructs(values);

    protected override VariableLengthStruct[] PeekArrayWithLength(ReadContext context) =>
        context.PeekVariableLengthStructs();

    protected override VariableLengthStruct[] ReadArrayWithLength(ReadContext context) =>
        context.ReadVariableLengthStructs();

    protected override VariableLengthStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthStructs(out VariableLengthStruct[] values));
        return values;
    }

    protected override VariableLengthStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthStructs(out VariableLengthStruct[] values));
        return values;
    }

    protected override TryReadOperationSet<VariableLengthStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out VariableLengthStruct v) => c.TryPeekVariableLengthStruct(out v),
        TryReadValue = (ReadContext c, out VariableLengthStruct v) => c.TryReadVariableLengthStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out VariableLengthStruct[] v) => c.TryPeekVariableLengthStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out VariableLengthStruct[] v) => c.TryReadVariableLengthStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out VariableLengthStruct[] v) => c.TryPeekVariableLengthStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out VariableLengthStruct[] v) => c.TryReadVariableLengthStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<VariableLengthStruct> d) => c.TryPeekVariableLengthStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<VariableLengthStruct> d) => c.TryReadVariableLengthStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<VariableLengthStruct> d) => c.TryPeekVariableLengthStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<VariableLengthStruct> d) => c.TryReadVariableLengthStructs(count, d),
    };
}
