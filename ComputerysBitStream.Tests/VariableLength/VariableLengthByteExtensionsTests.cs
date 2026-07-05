namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthByteExtensionsTests : VariableLengthExtensionTestSuite<byte> {
    protected override byte Value => 42;
    protected override byte[] Values => [42, 0, 200];
    protected override int GetSize(byte value) => PrimitiveVariableLengthByteExtensions.GetVariableLengthByteSize(value);

    protected override void WritePrimitive(ref WriteContext context, byte value) => context.WriteVariableLengthBytePrimitive(value);
    protected override byte PeekPrimitive(ReadContext context) => context.PeekVariableLengthBytePrimitive();
    protected override byte ReadPrimitive(ReadContext context) => context.ReadVariableLengthBytePrimitive();
    protected override void Write(ref WriteContext context, byte value) => context.WriteVariableLengthByte(value);
    protected override byte Peek(ReadContext context) => context.PeekVariableLengthByte();
    protected override byte Read(ReadContext context) => context.ReadVariableLengthByte();

    protected override byte TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthByte(out byte v));
        return v;
    }

    protected override byte TryRead(ReadContext context) {
        Assert.True(context.TryReadVariableLengthByte(out byte v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<byte> values) => context.WriteVariableLengthBytesPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<byte> destination) => context.PeekVariableLengthByteSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<byte> destination) => context.ReadVariableLengthByteSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<byte> values) => context.WriteVariableLengthBytesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<byte> destination) => context.PeekVariableLengthBytes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<byte> destination) => context.ReadVariableLengthBytes(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<byte> destination) { Assert.True(context.TryPeekVariableLengthBytes(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<byte> destination) { Assert.True(context.TryReadVariableLengthBytes(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<byte> values) => context.WriteVariableLengthBytes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<byte> destination) => context.PeekVariableLengthBytes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<byte> destination) => context.ReadVariableLengthBytes(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<byte> destination) { Assert.True(context.TryPeekVariableLengthBytes(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<byte> destination) { Assert.True(context.TryReadVariableLengthBytes(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, byte[] values) => context.WriteVariableLengthBytesPrimitive(values);
    protected override byte[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekVariableLengthByteArrayPrimitive(count);
    protected override byte[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadVariableLengthByteArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, byte[] values) => context.WriteVariableLengthBytesWithoutLength(values);
    protected override byte[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVariableLengthBytes(count);
    protected override byte[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVariableLengthBytes(count);

    protected override byte[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVariableLengthBytes(count, out byte[] values));
        return values;
    }

    protected override byte[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVariableLengthBytes(count, out byte[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, byte[] values) => context.WriteVariableLengthBytes(values);
    protected override byte[] PeekArrayWithLength(ReadContext context) => context.PeekVariableLengthBytes();
    protected override byte[] ReadArrayWithLength(ReadContext context) => context.ReadVariableLengthBytes();

    protected override byte[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVariableLengthBytes(out byte[] values));
        return values;
    }

    protected override byte[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVariableLengthBytes(out byte[] values));
        return values;
    }

    protected override TryReadOperationSet<byte> TryOperations => new() {
        TryPeekValue = (ReadContext c, out byte v) => c.TryPeekVariableLengthByte(out v),
        TryReadValue = (ReadContext c, out byte v) => c.TryReadVariableLengthByte(out v),
        TryPeekArrayWithLength = (ReadContext c, out byte[] v) => c.TryPeekVariableLengthBytes(out v),
        TryReadArrayWithLength = (ReadContext c, out byte[] v) => c.TryReadVariableLengthBytes(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out byte[] v) => c.TryPeekVariableLengthBytes(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out byte[] v) => c.TryReadVariableLengthBytes(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<byte> d) => c.TryPeekVariableLengthBytes(d),
        TryReadSpanWithLength = (ReadContext c, Span<byte> d) => c.TryReadVariableLengthBytes(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<byte> d) => c.TryPeekVariableLengthBytes(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<byte> d) => c.TryReadVariableLengthBytes(count, d),
    };
}
