namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class SByteExtensionsTests : PrimitiveSerializationTestSuite<sbyte> {
    protected override sbyte Value => 42;
    protected override sbyte[] Values => [42, -42, 42, 42, -42];

    protected override void WritePrimitive(ref WriteContext context, sbyte value) => context.WriteSBytePrimitive(value);
    protected override sbyte PeekPrimitive(ReadContext context) => context.PeekSBytePrimitive();
    protected override sbyte ReadPrimitive(ReadContext context) => context.ReadSBytePrimitive();
    protected override void Write(ref WriteContext context, sbyte value) => context.WriteSByte(value);
    protected override sbyte Peek(ReadContext context) => context.PeekSByte();
    protected override sbyte Read(ReadContext context) => context.ReadSByte();

    protected override sbyte TryPeek(ReadContext context) {
        Assert.True(context.TryPeekSByte(out sbyte v));
        return v;
    }

    protected override sbyte TryRead(ReadContext context) {
        Assert.True(context.TryReadSByte(out sbyte v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<sbyte> values) => context.WriteSBytesPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<sbyte> destination) => context.PeekSByteSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<sbyte> destination) => context.ReadSByteSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<sbyte> values) => context.WriteSBytesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) => context.PeekSBytes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) => context.ReadSBytes(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) { Assert.True(context.TryPeekSBytes(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<sbyte> destination) { Assert.True(context.TryReadSBytes(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<sbyte> values) => context.WriteSBytes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<sbyte> destination) => context.PeekSBytes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<sbyte> destination) => context.ReadSBytes(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<sbyte> destination) { Assert.True(context.TryPeekSBytes(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<sbyte> destination) { Assert.True(context.TryReadSBytes(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, sbyte[] values) => context.WriteSBytesPrimitive(values);
    protected override sbyte[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekSByteArrayPrimitive(count);
    protected override sbyte[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadSByteArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, sbyte[] values) => context.WriteSBytesWithoutLength(values);
    protected override sbyte[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekSBytes(count);
    protected override sbyte[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadSBytes(count);

    protected override sbyte[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekSBytes(count, out sbyte[] values));
        return values;
    }

    protected override sbyte[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadSBytes(count, out sbyte[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, sbyte[] values) => context.WriteSBytes(values);
    protected override sbyte[] PeekArrayWithLength(ReadContext context) => context.PeekSBytes();
    protected override sbyte[] ReadArrayWithLength(ReadContext context) => context.ReadSBytes();

    protected override sbyte[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekSBytes(out sbyte[] values));
        return values;
    }

    protected override sbyte[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadSBytes(out sbyte[] values));
        return values;
    }

    protected override sbyte[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekSBytesWithMaxCount(maxCount);
    protected override sbyte[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadSBytesWithMaxCount(maxCount);

    protected override sbyte[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekSBytesWithMaxCount(maxCount, out sbyte[] values));
        return values;
    }

    protected override sbyte[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadSBytesWithMaxCount(maxCount, out sbyte[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<sbyte> destination) => context.PeekSBytesWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<sbyte> destination) => context.ReadSBytesWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<sbyte> destination) { Assert.True(context.TryPeekSBytesWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<sbyte> destination) { Assert.True(context.TryReadSBytesWithMaxCount(maxCount, destination)); }

    protected override TryReadOperationSet<sbyte> TryOperations => new() {
        TryPeekValue = (ReadContext c, out sbyte v) => c.TryPeekSByte(out v),
        TryReadValue = (ReadContext c, out sbyte v) => c.TryReadSByte(out v),
        TryPeekArrayWithLength = (ReadContext c, out sbyte[] v) => c.TryPeekSBytes(out v),
        TryReadArrayWithLength = (ReadContext c, out sbyte[] v) => c.TryReadSBytes(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out sbyte[] v) => c.TryPeekSBytes(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out sbyte[] v) => c.TryReadSBytes(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<sbyte> d) => c.TryPeekSBytes(d),
        TryReadSpanWithLength = (ReadContext c, Span<sbyte> d) => c.TryReadSBytes(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<sbyte> d) => c.TryPeekSBytes(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<sbyte> d) => c.TryReadSBytes(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out sbyte[] v) => c.TryPeekSBytesWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out sbyte[] v) => c.TryReadSBytesWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<sbyte> d) => c.TryPeekSBytesWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<sbyte> d) => c.TryReadSBytesWithMaxCount(maxCount, d),
    };
}
