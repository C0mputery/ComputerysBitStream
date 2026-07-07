namespace ComputerysBitStream.Tests.Structs;

public class AliasedStructTests : StructTestSuite<AliasedStruct> {
    protected override AliasedStruct Value => new() { A = 42, B = 3.14f };

    protected override AliasedStruct[] Values => [
        new() { A = 1, B = 1.0f },
        new() { A = 2, B = 2.0f },
        new() { A = 3, B = 3.0f }
    ];

    protected override void Write(ref WriteContext context, AliasedStruct value) => context.WriteAliased(value);
    protected override AliasedStruct Peek(ReadContext context) => context.PeekAliased();
    protected override AliasedStruct Read(ReadContext context) => context.ReadAliased();

    protected override AliasedStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekAliased(out AliasedStruct v));
        return v;
    }

    protected override AliasedStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadAliased(out AliasedStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, AliasedStruct[] values) => context.WriteAliaseds(values);
    protected override AliasedStruct[] PeekArrayWithLength(ReadContext context) => context.PeekAliaseds();
    protected override AliasedStruct[] ReadArrayWithLength(ReadContext context) => context.ReadAliaseds();

    protected override AliasedStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekAliaseds(out AliasedStruct[] v));
        return v;
    }

    protected override AliasedStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadAliaseds(out AliasedStruct[] v));
        return v;
    }

    protected override AliasedStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekAliasedsWithMaxCount(maxCount);
    protected override AliasedStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadAliasedsWithMaxCount(maxCount);

    protected override AliasedStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekAliasedsWithMaxCount(maxCount, out AliasedStruct[] values));
        return values;
    }

    protected override AliasedStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadAliasedsWithMaxCount(maxCount, out AliasedStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedStruct> destination) => context.PeekAliasedsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedStruct> destination) => context.ReadAliasedsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedStruct> destination) { Assert.True(context.TryPeekAliasedsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedStruct> destination) { Assert.True(context.TryReadAliasedsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, AliasedStruct[] values) => context.WriteAliasedsWithoutLength(values);
    protected override AliasedStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekAliaseds(count);
    protected override AliasedStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadAliaseds(count);

    protected override AliasedStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekAliaseds(count, out AliasedStruct[] v));
        return v;
    }

    protected override AliasedStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadAliaseds(count, out AliasedStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<AliasedStruct> values) => context.WriteAliaseds(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<AliasedStruct> destination) => context.PeekAliaseds(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<AliasedStruct> destination) => context.ReadAliaseds(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<AliasedStruct> destination) { Assert.True(context.TryPeekAliaseds(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<AliasedStruct> destination) { Assert.True(context.TryReadAliaseds(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<AliasedStruct> values) => context.WriteAliasedsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<AliasedStruct> destination) => context.PeekAliaseds(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<AliasedStruct> destination) => context.ReadAliaseds(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<AliasedStruct> destination) { Assert.True(context.TryPeekAliaseds(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<AliasedStruct> destination) { Assert.True(context.TryReadAliaseds(count, destination)); }

    protected override Type StructType => typeof(AliasedStruct);

    protected override TryReadOperationSet<AliasedStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out AliasedStruct v) => c.TryPeekAliased(out v),
        TryReadValue = (ReadContext c, out AliasedStruct v) => c.TryReadAliased(out v),
        TryPeekArrayWithLength = (ReadContext c, out AliasedStruct[] v) => c.TryPeekAliaseds(out v),
        TryReadArrayWithLength = (ReadContext c, out AliasedStruct[] v) => c.TryReadAliaseds(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out AliasedStruct[] v) => c.TryPeekAliaseds(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out AliasedStruct[] v) => c.TryReadAliaseds(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<AliasedStruct> d) => c.TryPeekAliaseds(d),
        TryReadSpanWithLength = (ReadContext c, Span<AliasedStruct> d) => c.TryReadAliaseds(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<AliasedStruct> d) => c.TryPeekAliaseds(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<AliasedStruct> d) => c.TryReadAliaseds(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out AliasedStruct[] v) => c.TryPeekAliasedsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out AliasedStruct[] v) => c.TryReadAliasedsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<AliasedStruct> d) => c.TryPeekAliasedsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<AliasedStruct> d) => c.TryReadAliasedsWithMaxCount(maxCount, d),
    };
}

public class AliasedExternalStructTests : StructTestSuite<AliasedExternalStruct> {
    protected override AliasedExternalStruct Value => new() { X = 99, Y = true };

    protected override AliasedExternalStruct[] Values => [
        new() { X = 1, Y = true },
        new() { X = 2, Y = false },
        new() { X = 3, Y = true }
    ];

    protected override void Write(ref WriteContext context, AliasedExternalStruct value) => context.WriteAliasedExt(value);
    protected override AliasedExternalStruct Peek(ReadContext context) => context.PeekAliasedExt();
    protected override AliasedExternalStruct Read(ReadContext context) => context.ReadAliasedExt();

    protected override AliasedExternalStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekAliasedExt(out AliasedExternalStruct v));
        return v;
    }

    protected override AliasedExternalStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadAliasedExt(out AliasedExternalStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, AliasedExternalStruct[] values) => context.WriteAliasedExts(values);
    protected override AliasedExternalStruct[] PeekArrayWithLength(ReadContext context) => context.PeekAliasedExts();
    protected override AliasedExternalStruct[] ReadArrayWithLength(ReadContext context) => context.ReadAliasedExts();

    protected override AliasedExternalStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekAliasedExts(out AliasedExternalStruct[] v));
        return v;
    }

    protected override AliasedExternalStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadAliasedExts(out AliasedExternalStruct[] v));
        return v;
    }

    protected override AliasedExternalStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekAliasedExtsWithMaxCount(maxCount);
    protected override AliasedExternalStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadAliasedExtsWithMaxCount(maxCount);

    protected override AliasedExternalStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekAliasedExtsWithMaxCount(maxCount, out AliasedExternalStruct[] values));
        return values;
    }

    protected override AliasedExternalStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadAliasedExtsWithMaxCount(maxCount, out AliasedExternalStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) => context.PeekAliasedExtsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) => context.ReadAliasedExtsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeekAliasedExtsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) { Assert.True(context.TryReadAliasedExtsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, AliasedExternalStruct[] values) => context.WriteAliasedExtsWithoutLength(values);
    protected override AliasedExternalStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekAliasedExts(count);
    protected override AliasedExternalStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadAliasedExts(count);

    protected override AliasedExternalStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekAliasedExts(count, out AliasedExternalStruct[] v));
        return v;
    }

    protected override AliasedExternalStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadAliasedExts(count, out AliasedExternalStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<AliasedExternalStruct> values) => context.WriteAliasedExts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<AliasedExternalStruct> destination) => context.PeekAliasedExts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<AliasedExternalStruct> destination) => context.ReadAliasedExts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeekAliasedExts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<AliasedExternalStruct> destination) { Assert.True(context.TryReadAliasedExts(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<AliasedExternalStruct> values) => context.WriteAliasedExtsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.PeekAliasedExts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.ReadAliasedExts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeekAliasedExts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<AliasedExternalStruct> destination) { Assert.True(context.TryReadAliasedExts(count, destination)); }

    protected override Type StructType => typeof(AliasedExternalStruct);

    protected override TryReadOperationSet<AliasedExternalStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out AliasedExternalStruct v) => c.TryPeekAliasedExt(out v),
        TryReadValue = (ReadContext c, out AliasedExternalStruct v) => c.TryReadAliasedExt(out v),
        TryPeekArrayWithLength = (ReadContext c, out AliasedExternalStruct[] v) => c.TryPeekAliasedExts(out v),
        TryReadArrayWithLength = (ReadContext c, out AliasedExternalStruct[] v) => c.TryReadAliasedExts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out AliasedExternalStruct[] v) => c.TryPeekAliasedExts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out AliasedExternalStruct[] v) => c.TryReadAliasedExts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<AliasedExternalStruct> d) => c.TryPeekAliasedExts(d),
        TryReadSpanWithLength = (ReadContext c, Span<AliasedExternalStruct> d) => c.TryReadAliasedExts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<AliasedExternalStruct> d) => c.TryPeekAliasedExts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<AliasedExternalStruct> d) => c.TryReadAliasedExts(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out AliasedExternalStruct[] v) => c.TryPeekAliasedExtsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out AliasedExternalStruct[] v) => c.TryReadAliasedExtsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<AliasedExternalStruct> d) => c.TryPeekAliasedExtsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<AliasedExternalStruct> d) => c.TryReadAliasedExtsWithMaxCount(maxCount, d),
    };
}

public class AliasedIncludeExternalStructTests : StructTestSuite<AliasedIncludeExternalStruct> {
    protected override AliasedIncludeExternalStruct Value => new() { Included = 7, Ignored = 0 };

    protected override AliasedIncludeExternalStruct[] Values => [
        new() { Included = 1, Ignored = 0 },
        new() { Included = 2, Ignored = 0 },
        new() { Included = 3, Ignored = 0 }
    ];

    protected override void Write(ref WriteContext context, AliasedIncludeExternalStruct value) => context.WriteAliasedInc(value);
    protected override AliasedIncludeExternalStruct Peek(ReadContext context) => context.PeekAliasedInc();
    protected override AliasedIncludeExternalStruct Read(ReadContext context) => context.ReadAliasedInc();

    protected override AliasedIncludeExternalStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekAliasedInc(out AliasedIncludeExternalStruct v));
        return v;
    }

    protected override AliasedIncludeExternalStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadAliasedInc(out AliasedIncludeExternalStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteAliasedIncs(values);
    protected override AliasedIncludeExternalStruct[] PeekArrayWithLength(ReadContext context) => context.PeekAliasedIncs();
    protected override AliasedIncludeExternalStruct[] ReadArrayWithLength(ReadContext context) => context.ReadAliasedIncs();

    protected override AliasedIncludeExternalStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekAliasedIncs(out AliasedIncludeExternalStruct[] v));
        return v;
    }

    protected override AliasedIncludeExternalStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadAliasedIncs(out AliasedIncludeExternalStruct[] v));
        return v;
    }

    protected override AliasedIncludeExternalStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekAliasedIncsWithMaxCount(maxCount);
    protected override AliasedIncludeExternalStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadAliasedIncsWithMaxCount(maxCount);

    protected override AliasedIncludeExternalStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekAliasedIncsWithMaxCount(maxCount, out AliasedIncludeExternalStruct[] values));
        return values;
    }

    protected override AliasedIncludeExternalStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadAliasedIncsWithMaxCount(maxCount, out AliasedIncludeExternalStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeekAliasedIncsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryReadAliasedIncsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteAliasedIncsWithoutLength(values);
    protected override AliasedIncludeExternalStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekAliasedIncs(count);
    protected override AliasedIncludeExternalStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadAliasedIncs(count);

    protected override AliasedIncludeExternalStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekAliasedIncs(count, out AliasedIncludeExternalStruct[] v));
        return v;
    }

    protected override AliasedIncludeExternalStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadAliasedIncs(count, out AliasedIncludeExternalStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteAliasedIncs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeekAliasedIncs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryReadAliasedIncs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteAliasedIncsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeekAliasedIncs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryReadAliasedIncs(count, destination)); }

    protected override Type StructType => typeof(AliasedIncludeExternalStruct);

    [Fact]
    public void IgnoredMember_ShouldNotAffectEquality() {
        AliasedIncludeExternalStruct original = new() { Included = 42, Ignored = 100 };
        AliasedIncludeExternalStruct modified = new() { Included = 42, Ignored = 200 };

        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeCtx = new(buffer);
        writeCtx.WriteAliasedInc(original);

        ReadContext readCtx = new(buffer);
        AliasedIncludeExternalStruct readOriginal = readCtx.ReadAliasedInc();

        writeCtx = new WriteContext(buffer);
        writeCtx.WriteAliasedInc(modified);

        readCtx = new ReadContext(buffer);
        AliasedIncludeExternalStruct readModified = readCtx.ReadAliasedInc();

        Assert.Equal(original.Included, readOriginal.Included);
        Assert.Equal(modified.Included, readModified.Included);
        Assert.Equal(readOriginal.Included, readModified.Included);
    }

    protected override TryReadOperationSet<AliasedIncludeExternalStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out AliasedIncludeExternalStruct v) => c.TryPeekAliasedInc(out v),
        TryReadValue = (ReadContext c, out AliasedIncludeExternalStruct v) => c.TryReadAliasedInc(out v),
        TryPeekArrayWithLength = (ReadContext c, out AliasedIncludeExternalStruct[] v) => c.TryPeekAliasedIncs(out v),
        TryReadArrayWithLength = (ReadContext c, out AliasedIncludeExternalStruct[] v) => c.TryReadAliasedIncs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out AliasedIncludeExternalStruct[] v) => c.TryPeekAliasedIncs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out AliasedIncludeExternalStruct[] v) => c.TryReadAliasedIncs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<AliasedIncludeExternalStruct> d) => c.TryPeekAliasedIncs(d),
        TryReadSpanWithLength = (ReadContext c, Span<AliasedIncludeExternalStruct> d) => c.TryReadAliasedIncs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<AliasedIncludeExternalStruct> d) => c.TryPeekAliasedIncs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<AliasedIncludeExternalStruct> d) => c.TryReadAliasedIncs(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out AliasedIncludeExternalStruct[] v) => c.TryPeekAliasedIncsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out AliasedIncludeExternalStruct[] v) => c.TryReadAliasedIncsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<AliasedIncludeExternalStruct> d) => c.TryPeekAliasedIncsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<AliasedIncludeExternalStruct> d) => c.TryReadAliasedIncsWithMaxCount(maxCount, d),
    };
}
