namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthSByteExtensionsTests : VariableLengthExtensionTestSuite<sbyte> {
    protected override sbyte Value => 42;
    protected override sbyte[] Values => [42, (sbyte)0, (sbyte)-100];
    protected override int GetSize(sbyte value) => PrimitiveVariableLengthSByteExtensions.GetVariableLengthSByteSize(value);

    protected override void WritePrimitive(ref WriteContext context, sbyte value) => context.WriteVariableLengthSBytePrimitive(value);
    protected override sbyte PeekPrimitive(ReadContext context) => context.PeekVariableLengthSBytePrimitive();
    protected override sbyte ReadPrimitive(ReadContext context) => context.ReadVariableLengthSBytePrimitive();
    protected override void Write(ref WriteContext context, sbyte value) => context.WriteVariableLengthSByte(value);
    protected override sbyte Peek(ReadContext context) => context.PeekVariableLengthSByte();
    protected override sbyte Read(ReadContext context) => context.ReadVariableLengthSByte();
    protected override sbyte TryPeek(ReadContext context) { Assert.True(context.TryPeekVariableLengthSByte(out sbyte v)); return v; }
    protected override sbyte TryRead(ReadContext context) { Assert.True(context.TryReadVariableLengthSByte(out sbyte v)); return v; }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<sbyte> values) => context.WriteVariableLengthSBytesPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<sbyte> destination) => context.PeekVariableLengthSByteSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<sbyte> destination) => context.ReadVariableLengthSByteSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<sbyte> values) => context.WriteVariableLengthSBytesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) => context.PeekVariableLengthSBytes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) => context.ReadVariableLengthSBytes(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) { Assert.True(context.TryPeekVariableLengthSBytes(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) { Assert.True(context.TryReadVariableLengthSBytes(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<sbyte> values) => context.WriteVariableLengthSBytes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<sbyte> destination) => context.PeekVariableLengthSBytes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<sbyte> destination) => context.ReadVariableLengthSBytes(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<sbyte> destination) { Assert.True(context.TryPeekVariableLengthSBytes(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<sbyte> destination) { Assert.True(context.TryReadVariableLengthSBytes(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, sbyte[] values) => context.WriteVariableLengthSBytesPrimitive(values);
    protected override sbyte[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthSByteArrayPrimitive(count);
    protected override sbyte[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthSByteArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, sbyte[] values) => context.WriteVariableLengthSBytesWithoutLength(values);
    protected override sbyte[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthSBytes(count);
    protected override sbyte[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthSBytes(count);
    protected override sbyte[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekVariableLengthSBytes(count, out sbyte[] values)); return values; }
    protected override sbyte[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadVariableLengthSBytes(count, out sbyte[] values)); return values; }

    protected override void WriteArray(ref WriteContext context, sbyte[] values) => context.WriteVariableLengthSBytes(values);
    protected override sbyte[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthSBytes();
    protected override sbyte[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthSBytes();
    protected override sbyte[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekVariableLengthSBytes(out sbyte[] values)); return values; }
    protected override sbyte[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadVariableLengthSBytes(out sbyte[] values)); return values; }
    protected override TryReadOperationSet<sbyte> TryOperations => new() {
        TryPeekValue = (ReadContext c, out sbyte v) => c.TryPeekVariableLengthSByte(out v),
        TryReadValue = (ReadContext c, out sbyte v) => c.TryReadVariableLengthSByte(out v),
        TryPeekArrayWithLength = (ReadContext c, out sbyte[] v) => c.TryPeekVariableLengthSBytes(out v),
        TryReadArrayWithLength = (ReadContext c, out sbyte[] v) => c.TryReadVariableLengthSBytes(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out sbyte[] v) => c.TryPeekVariableLengthSBytes(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out sbyte[] v) => c.TryReadVariableLengthSBytes(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<sbyte> d) => c.TryPeekVariableLengthSBytes(d),
        TryReadSpanWithLength = (ReadContext c, Span<sbyte> d) => c.TryReadVariableLengthSBytes(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<sbyte> d) => c.TryPeekVariableLengthSBytes(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<sbyte> d) => c.TryReadVariableLengthSBytes(count, d),
    };
}