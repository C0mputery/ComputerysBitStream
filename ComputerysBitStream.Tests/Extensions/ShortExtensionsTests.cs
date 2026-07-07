namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class ShortExtensionsTests : PrimitiveSerializationTestSuite<short> {
    protected override short Value => 42;
    protected override short[] Values => [42, -42, 42, 42, -42];

    protected override void WritePrimitive(ref WriteContext context, short value) => context.WriteShortPrimitive(value);
    protected override short PeekPrimitive(ReadContext context) => context.PeekShortPrimitive();
    protected override short ReadPrimitive(ReadContext context) => context.ReadShortPrimitive();
    protected override void Write(ref WriteContext context, short value) => context.WriteShort(value);
    protected override short Peek(ReadContext context) => context.PeekShort();
    protected override short Read(ReadContext context) => context.ReadShort();

    protected override short TryPeek(ReadContext context) {
        Assert.True(context.TryPeekShort(out short v));
        return v;
    }

    protected override short TryRead(ReadContext context) {
        Assert.True(context.TryReadShort(out short v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<short> values) => context.WriteShortsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<short> destination) => context.PeekShortSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<short> destination) => context.ReadShortSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<short> values) => context.WriteShortsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<short> destination) => context.PeekShorts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<short> destination) => context.ReadShorts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<short> destination) { Assert.True(context.TryPeekShorts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<short> destination) { Assert.True(context.TryReadShorts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<short> values) => context.WriteShorts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<short> destination) => context.PeekShorts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<short> destination) => context.ReadShorts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<short> destination) { Assert.True(context.TryPeekShorts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<short> destination) { Assert.True(context.TryReadShorts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, short[] values) => context.WriteShortsPrimitive(values);
    protected override short[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekShortArrayPrimitive(count);
    protected override short[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadShortArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, short[] values) => context.WriteShortsWithoutLength(values);
    protected override short[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekShorts(count);
    protected override short[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadShorts(count);

    protected override short[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekShorts(count, out short[] values));
        return values;
    }

    protected override short[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadShorts(count, out short[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, short[] values) => context.WriteShorts(values);
    protected override short[] PeekArrayWithLength(ReadContext context) => context.PeekShorts();
    protected override short[] ReadArrayWithLength(ReadContext context) => context.ReadShorts();

    protected override short[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekShorts(out short[] values));
        return values;
    }

    protected override short[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadShorts(out short[] values));
        return values;
    }

    protected override short[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekShortsWithMaxCount(maxCount);
    protected override short[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadShortsWithMaxCount(maxCount);

    protected override short[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekShortsWithMaxCount(maxCount, out short[] values));
        return values;
    }

    protected override short[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadShortsWithMaxCount(maxCount, out short[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<short> destination) => context.PeekShortsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<short> destination) => context.ReadShortsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<short> destination) { Assert.True(context.TryPeekShortsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<short> destination) { Assert.True(context.TryReadShortsWithMaxCount(maxCount, destination)); }

    protected override TryReadOperationSet<short> TryOperations => new() {
        TryPeekValue = (ReadContext c, out short v) => c.TryPeekShort(out v),
        TryReadValue = (ReadContext c, out short v) => c.TryReadShort(out v),
        TryPeekArrayWithLength = (ReadContext c, out short[] v) => c.TryPeekShorts(out v),
        TryReadArrayWithLength = (ReadContext c, out short[] v) => c.TryReadShorts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out short[] v) => c.TryPeekShorts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out short[] v) => c.TryReadShorts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<short> d) => c.TryPeekShorts(d),
        TryReadSpanWithLength = (ReadContext c, Span<short> d) => c.TryReadShorts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<short> d) => c.TryPeekShorts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<short> d) => c.TryReadShorts(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out short[] v) => c.TryPeekShortsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out short[] v) => c.TryReadShortsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<short> d) => c.TryPeekShortsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<short> d) => c.TryReadShortsWithMaxCount(maxCount, d),
    };
}
