namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class UShortExtensionsTests : PrimitiveSerializationTestSuite<ushort> {
    protected override ushort Value => 42;
    protected override ushort[] Values => [42, 100, 42, 42, 100];

    protected override void WritePrimitive(ref WriteContext context, ushort value) => context.WriteUShortPrimitive(value);
    protected override ushort PeekPrimitive(ReadContext context) => context.PeekUShortPrimitive();
    protected override ushort ReadPrimitive(ReadContext context) => context.ReadUShortPrimitive();
    protected override void Write(ref WriteContext context, ushort value) => context.WriteUShort(value);
    protected override ushort Peek(ReadContext context) => context.PeekUShort();
    protected override ushort Read(ReadContext context) => context.ReadUShort();

    protected override ushort TryPeek(ReadContext context) {
        Assert.True(context.TryPeekUShort(out ushort v));
        return v;
    }

    protected override ushort TryRead(ReadContext context) {
        Assert.True(context.TryReadUShort(out ushort v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<ushort> values) => context.WriteUShortsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<ushort> destination) => context.PeekUShortSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<ushort> destination) => context.ReadUShortSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<ushort> values) => context.WriteUShortsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) => context.PeekUShorts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) => context.ReadUShorts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) { Assert.True(context.TryPeekUShorts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<ushort> destination) { Assert.True(context.TryReadUShorts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<ushort> values) => context.WriteUShorts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<ushort> destination) => context.PeekUShorts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<ushort> destination) => context.ReadUShorts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<ushort> destination) { Assert.True(context.TryPeekUShorts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<ushort> destination) { Assert.True(context.TryReadUShorts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, ushort[] values) => context.WriteUShortsPrimitive(values);
    protected override ushort[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekUShortArrayPrimitive(count);
    protected override ushort[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadUShortArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, ushort[] values) => context.WriteUShortsWithoutLength(values);
    protected override ushort[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekUShorts(count);
    protected override ushort[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadUShorts(count);

    protected override ushort[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekUShorts(count, out ushort[] values));
        return values;
    }

    protected override ushort[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadUShorts(count, out ushort[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, ushort[] values) => context.WriteUShorts(values);
    protected override ushort[] PeekArrayWithLength(ReadContext context) => context.PeekUShorts();
    protected override ushort[] ReadArrayWithLength(ReadContext context) => context.ReadUShorts();

    protected override ushort[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekUShorts(out ushort[] values));
        return values;
    }

    protected override ushort[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadUShorts(out ushort[] values));
        return values;
    }

    protected override ushort[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekUShortsWithMaxCount(maxCount);
    protected override ushort[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadUShortsWithMaxCount(maxCount);

    protected override ushort[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekUShortsWithMaxCount(maxCount, out ushort[] values));
        return values;
    }

    protected override ushort[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadUShortsWithMaxCount(maxCount, out ushort[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<ushort> destination) => context.PeekUShortsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<ushort> destination) => context.ReadUShortsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<ushort> destination) { Assert.True(context.TryPeekUShortsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<ushort> destination) { Assert.True(context.TryReadUShortsWithMaxCount(maxCount, destination)); }

    protected override TryReadOperationSet<ushort> TryOperations => new() {
        TryPeekValue = (ReadContext c, out ushort v) => c.TryPeekUShort(out v),
        TryReadValue = (ReadContext c, out ushort v) => c.TryReadUShort(out v),
        TryPeekArrayWithLength = (ReadContext c, out ushort[] v) => c.TryPeekUShorts(out v),
        TryReadArrayWithLength = (ReadContext c, out ushort[] v) => c.TryReadUShorts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out ushort[] v) => c.TryPeekUShorts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out ushort[] v) => c.TryReadUShorts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<ushort> d) => c.TryPeekUShorts(d),
        TryReadSpanWithLength = (ReadContext c, Span<ushort> d) => c.TryReadUShorts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<ushort> d) => c.TryPeekUShorts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<ushort> d) => c.TryReadUShorts(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out ushort[] v) => c.TryPeekUShortsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out ushort[] v) => c.TryReadUShortsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<ushort> d) => c.TryPeekUShortsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<ushort> d) => c.TryReadUShortsWithMaxCount(maxCount, d),
    };
}
