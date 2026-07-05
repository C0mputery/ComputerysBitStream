namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class ByteExtensionsTests : ExtensionTestSuite<byte> {
    protected override byte Value => 42;
    protected override byte[] Values => [42, 100, 42, 42, 100];

    protected override void WritePrimitive(ref WriteContext context, byte value) => context.WriteBytePrimitive(value);
    protected override byte PeekPrimitive(ReadContext context) => context.PeekBytePrimitive();
    protected override byte ReadPrimitive(ReadContext context) => context.ReadBytePrimitive();
    protected override void Write(ref WriteContext context, byte value) => context.WriteByte(value);
    protected override byte Peek(ReadContext context) => context.PeekByte();
    protected override byte Read(ReadContext context) => context.ReadByte();

    protected override byte TryPeek(ReadContext context) {
        Assert.True(context.TryPeekByte(out byte v));
        return v;
    }

    protected override byte TryRead(ReadContext context) {
        Assert.True(context.TryReadByte(out byte v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<byte> values) => context.WriteBytesPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<byte> destination) => context.PeekByteSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<byte> destination) => context.ReadByteSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<byte> values) => context.WriteBytesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<byte> destination) => context.PeekBytes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<byte> destination) => context.ReadBytes(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<byte> destination) { Assert.True(context.TryPeekBytes(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<byte> destination) { Assert.True(context.TryReadBytes(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<byte> values) => context.WriteBytes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<byte> destination) => context.PeekBytes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<byte> destination) => context.ReadBytes(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<byte> destination) { Assert.True(context.TryPeekBytes(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<byte> destination) { Assert.True(context.TryReadBytes(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, byte[] values) => context.WriteBytesPrimitive(values);
    protected override byte[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekByteArrayPrimitive(count);
    protected override byte[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadByteArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, byte[] values) => context.WriteBytesWithoutLength(values);
    protected override byte[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekBytes(count);
    protected override byte[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadBytes(count);

    protected override byte[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekBytes(count, out byte[] values));
        return values;
    }

    protected override byte[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadBytes(count, out byte[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, byte[] values) => context.WriteBytes(values);
    protected override byte[] PeekArrayWithLength(ReadContext context) => context.PeekBytes();
    protected override byte[] ReadArrayWithLength(ReadContext context) => context.ReadBytes();

    protected override byte[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekBytes(out byte[] values));
        return values;
    }

    protected override byte[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadBytes(out byte[] values));
        return values;
    }

    protected override TryReadOperationSet<byte> TryOperations => new() {
        TryPeekValue = (ReadContext c, out byte v) => c.TryPeekByte(out v),
        TryReadValue = (ReadContext c, out byte v) => c.TryReadByte(out v),
        TryPeekArrayWithLength = (ReadContext c, out byte[] v) => c.TryPeekBytes(out v),
        TryReadArrayWithLength = (ReadContext c, out byte[] v) => c.TryReadBytes(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out byte[] v) => c.TryPeekBytes(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out byte[] v) => c.TryReadBytes(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<byte> d) => c.TryPeekBytes(d),
        TryReadSpanWithLength = (ReadContext c, Span<byte> d) => c.TryReadBytes(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<byte> d) => c.TryPeekBytes(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<byte> d) => c.TryReadBytes(count, d),
    };
}
