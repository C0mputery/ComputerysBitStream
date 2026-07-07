namespace ComputerysBitStream.Tests.Structs;

public class StringStructTests : StructTestSuite<StringStruct> {
    protected override StringStruct Value => new() { Id = 42, Name = "player" };

    protected override StringStruct[] Values => [
        new() { Id = 1, Name = "alpha" },
        new() { Id = 2, Name = "" },
        new() { Id = 3, Name = "café" },
    ];

    protected override Type StructType => typeof(StringStruct);

    protected override void Write(ref WriteContext context, StringStruct value) => context.WriteStringStruct(value);
    protected override StringStruct Peek(ReadContext context) => context.PeekStringStruct();
    protected override StringStruct Read(ReadContext context) => context.ReadStringStruct();

    protected override StringStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekStringStruct(out StringStruct v));
        return v;
    }

    protected override StringStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadStringStruct(out StringStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, StringStruct[] values) => context.WriteStringStructs(values);
    protected override StringStruct[] PeekArrayWithLength(ReadContext context) => context.PeekStringStructs();
    protected override StringStruct[] ReadArrayWithLength(ReadContext context) => context.ReadStringStructs();

    protected override StringStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekStringStructs(out StringStruct[] v));
        return v;
    }

    protected override StringStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadStringStructs(out StringStruct[] v));
        return v;
    }

    protected override StringStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekStringStructsWithMaxCount(maxCount);
    protected override StringStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadStringStructsWithMaxCount(maxCount);

    protected override StringStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekStringStructsWithMaxCount(maxCount, out StringStruct[] values));
        return values;
    }

    protected override StringStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadStringStructsWithMaxCount(maxCount, out StringStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<StringStruct> destination) => context.PeekStringStructsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<StringStruct> destination) => context.ReadStringStructsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<StringStruct> destination) { Assert.True(context.TryPeekStringStructsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<StringStruct> destination) { Assert.True(context.TryReadStringStructsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, StringStruct[] values) => context.WriteStringStructsWithoutLength(values);
    protected override StringStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekStringStructs(count);
    protected override StringStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadStringStructs(count);

    protected override StringStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekStringStructs(count, out StringStruct[] v));
        return v;
    }

    protected override StringStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadStringStructs(count, out StringStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<StringStruct> values) => context.WriteStringStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<StringStruct> destination) => context.PeekStringStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<StringStruct> destination) => context.ReadStringStructs(destination);

    protected override void TryPeekSpanWithLength(ReadContext context, Span<StringStruct> destination) {
        Assert.True(context.TryPeekStringStructs(destination));
    }

    protected override void TryReadSpanWithLength(ReadContext context, Span<StringStruct> destination) {
        Assert.True(context.TryReadStringStructs(destination));
    }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<StringStruct> values) => context.WriteStringStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<StringStruct> destination) => context.PeekStringStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<StringStruct> destination) => context.ReadStringStructs(count, destination);

    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<StringStruct> destination) {
        Assert.True(context.TryPeekStringStructs(count, destination));
    }

    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<StringStruct> destination) {
        Assert.True(context.TryReadStringStructs(count, destination));
    }

    protected override TryReadOperationSet<StringStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out StringStruct v) => c.TryPeekStringStruct(out v),
        TryReadValue = (ReadContext c, out StringStruct v) => c.TryReadStringStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out StringStruct[] v) => c.TryPeekStringStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out StringStruct[] v) => c.TryReadStringStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out StringStruct[] v) => c.TryPeekStringStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out StringStruct[] v) => c.TryReadStringStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<StringStruct> d) => c.TryPeekStringStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<StringStruct> d) => c.TryReadStringStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<StringStruct> d) => c.TryPeekStringStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<StringStruct> d) => c.TryReadStringStructs(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out StringStruct[] v) => c.TryPeekStringStructsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out StringStruct[] v) => c.TryReadStringStructsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<StringStruct> d) => c.TryPeekStringStructsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<StringStruct> d) => c.TryReadStringStructsWithMaxCount(maxCount, d),
    };
}
