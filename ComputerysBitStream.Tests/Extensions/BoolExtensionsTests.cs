namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class BoolExtensionsTests : PrimitiveSerializationTestSuite<bool> {
    protected override bool Value => true;
    protected override bool[] Values => [true, false, true, true, false];

    protected override void WritePrimitive(ref WriteContext context, bool value) => context.WriteBoolPrimitive(value);
    protected override bool PeekPrimitive(ReadContext context) => context.PeekBoolPrimitive();
    protected override bool ReadPrimitive(ReadContext context) => context.ReadBoolPrimitive();
    protected override void Write(ref WriteContext context, bool value) => context.WriteBool(value);
    protected override bool Peek(ReadContext context) => context.PeekBool();
    protected override bool Read(ReadContext context) => context.ReadBool();

    protected override bool TryPeek(ReadContext context) {
        Assert.True(context.TryPeekBool(out bool v));
        return v;
    }

    protected override bool TryRead(ReadContext context) {
        Assert.True(context.TryReadBool(out bool v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<bool> values) => context.WriteBoolsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<bool> destination) => context.PeekBoolSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<bool> destination) => context.ReadBoolSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<bool> values) => context.WriteBoolsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<bool> destination) => context.PeekBools(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<bool> destination) => context.ReadBools(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<bool> destination) { Assert.True(context.TryPeekBools(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<bool> destination) { Assert.True(context.TryReadBools(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<bool> values) => context.WriteBools(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<bool> destination) => context.PeekBools(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<bool> destination) => context.ReadBools(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<bool> destination) { Assert.True(context.TryPeekBools(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<bool> destination) { Assert.True(context.TryReadBools(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, bool[] values) => context.WriteBoolsPrimitive(values);
    protected override bool[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekBoolArrayPrimitive(count);
    protected override bool[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadBoolArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, bool[] values) => context.WriteBoolsWithoutLength(values);
    protected override bool[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekBools(count);
    protected override bool[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadBools(count);

    protected override bool[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekBools(count, out bool[] values));
        return values;
    }

    protected override bool[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadBools(count, out bool[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, bool[] values) => context.WriteBools(values);
    protected override bool[] PeekArrayWithLength(ReadContext context) => context.PeekBools();
    protected override bool[] ReadArrayWithLength(ReadContext context) => context.ReadBools();

    protected override bool[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekBools(out bool[] values));
        return values;
    }

    protected override bool[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadBools(out bool[] values));
        return values;
    }

    protected override bool[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekBoolsWithMaxCount(maxCount);
    protected override bool[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadBoolsWithMaxCount(maxCount);

    protected override bool[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekBoolsWithMaxCount(maxCount, out bool[] values));
        return values;
    }

    protected override bool[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadBoolsWithMaxCount(maxCount, out bool[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<bool> destination) => context.PeekBoolsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<bool> destination) => context.ReadBoolsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<bool> destination) { Assert.True(context.TryPeekBoolsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<bool> destination) { Assert.True(context.TryReadBoolsWithMaxCount(maxCount, destination)); }

    protected override TryReadOperationSet<bool> TryOperations => new() {
        TryPeekValue = (ReadContext c, out bool v) => c.TryPeekBool(out v),
        TryReadValue = (ReadContext c, out bool v) => c.TryReadBool(out v),
        TryPeekArrayWithLength = (ReadContext c, out bool[] v) => c.TryPeekBools(out v),
        TryReadArrayWithLength = (ReadContext c, out bool[] v) => c.TryReadBools(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out bool[] v) => c.TryPeekBools(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out bool[] v) => c.TryReadBools(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<bool> d) => c.TryPeekBools(d),
        TryReadSpanWithLength = (ReadContext c, Span<bool> d) => c.TryReadBools(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<bool> d) => c.TryPeekBools(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<bool> d) => c.TryReadBools(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out bool[] v) => c.TryPeekBoolsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out bool[] v) => c.TryReadBoolsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<bool> d) => c.TryPeekBoolsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<bool> d) => c.TryReadBoolsWithMaxCount(maxCount, d),
    };
}
