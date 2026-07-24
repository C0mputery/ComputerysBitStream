using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class AliasedStructTests : StructTestSuite<AliasedStruct> {
    protected override AliasedStruct Value => new() { A = 42, B = 3.14f };

    protected override AliasedStruct[] Values => [
        new() { A = 1, B = 1.0f },
        new() { A = 2, B = 2.0f },
        new() { A = 3, B = 3.0f }
    ];

    protected override Type StructType => typeof(AliasedStruct);

    protected override SerializationOperations<AliasedStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, AliasedStruct value) => context.WriteAliased(value),
        Peek = (ReadContext context) => context.PeekAliased(),
        Read = (ReadContext context) => context.ReadAliased(),
        TryPeek = (ReadContext context, out AliasedStruct value) => context.TryPeekAliased(out value),
        TryRead = (ReadContext context, out AliasedStruct value) => context.TryReadAliased(out value),
        WriteSpan = (ref WriteContext context, Span<AliasedStruct> values) => context.WriteAliaseds(values),
        PeekSpan = (ReadContext context, Span<AliasedStruct> destination) => context.PeekAliaseds(destination),
        ReadSpan = (ReadContext context, Span<AliasedStruct> destination) => context.ReadAliaseds(destination),
        TryPeekSpan = (ReadContext context, Span<AliasedStruct> destination) => context.TryPeekAliaseds(destination),
        TryReadSpan = (ReadContext context, Span<AliasedStruct> destination) => context.TryReadAliaseds(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<AliasedStruct> values) => context.WriteAliasedsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<AliasedStruct> destination) => context.PeekAliaseds(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<AliasedStruct> destination) => context.ReadAliaseds(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<AliasedStruct> destination) => context.TryPeekAliaseds(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<AliasedStruct> destination) => context.TryReadAliaseds(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedStruct> destination) => context.PeekAliasedsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedStruct> destination) => context.ReadAliasedsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedStruct> destination) => context.TryPeekAliasedsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedStruct> destination) => context.TryReadAliasedsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, AliasedStruct[] values) => context.WriteAliaseds(values),
        PeekArray = (ReadContext context) => context.PeekAliaseds(),
        ReadArray = (ReadContext context) => context.ReadAliaseds(),
        TryPeekArray = (ReadContext context, out AliasedStruct[] values) => context.TryPeekAliaseds(out values),
        TryReadArray = (ReadContext context, out AliasedStruct[] values) => context.TryReadAliaseds(out values),
        WriteArrayWithoutLength = (ref WriteContext context, AliasedStruct[] values) => context.WriteAliasedsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekAliaseds(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadAliaseds(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out AliasedStruct[] values) => context.TryPeekAliaseds(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out AliasedStruct[] values) => context.TryReadAliaseds(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekAliasedsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadAliasedsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out AliasedStruct[] values) => context.TryPeekAliasedsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out AliasedStruct[] values) => context.TryReadAliasedsWithMaxCount(maxCount, out values),
    };
}

public class AliasedExternalStructTests : StructTestSuite<AliasedExternalStruct> {
    protected override AliasedExternalStruct Value => new() { X = 99, Y = true };

    protected override AliasedExternalStruct[] Values => [
        new() { X = 1, Y = true },
        new() { X = 2, Y = false },
        new() { X = 3, Y = true }
    ];

    protected override Type StructType => typeof(AliasedExternalStruct);

    protected override SerializationOperations<AliasedExternalStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, AliasedExternalStruct value) => context.WriteAliasedExt(value),
        Peek = (ReadContext context) => context.PeekAliasedExt(),
        Read = (ReadContext context) => context.ReadAliasedExt(),
        TryPeek = (ReadContext context, out AliasedExternalStruct value) => context.TryPeekAliasedExt(out value),
        TryRead = (ReadContext context, out AliasedExternalStruct value) => context.TryReadAliasedExt(out value),
        WriteSpan = (ref WriteContext context, Span<AliasedExternalStruct> values) => context.WriteAliasedExts(values),
        PeekSpan = (ReadContext context, Span<AliasedExternalStruct> destination) => context.PeekAliasedExts(destination),
        ReadSpan = (ReadContext context, Span<AliasedExternalStruct> destination) => context.ReadAliasedExts(destination),
        TryPeekSpan = (ReadContext context, Span<AliasedExternalStruct> destination) => context.TryPeekAliasedExts(destination),
        TryReadSpan = (ReadContext context, Span<AliasedExternalStruct> destination) => context.TryReadAliasedExts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<AliasedExternalStruct> values) => context.WriteAliasedExtsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.PeekAliasedExts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.ReadAliasedExts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.TryPeekAliasedExts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.TryReadAliasedExts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) => context.PeekAliasedExtsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) => context.ReadAliasedExtsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) => context.TryPeekAliasedExtsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedExternalStruct> destination) => context.TryReadAliasedExtsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, AliasedExternalStruct[] values) => context.WriteAliasedExts(values),
        PeekArray = (ReadContext context) => context.PeekAliasedExts(),
        ReadArray = (ReadContext context) => context.ReadAliasedExts(),
        TryPeekArray = (ReadContext context, out AliasedExternalStruct[] values) => context.TryPeekAliasedExts(out values),
        TryReadArray = (ReadContext context, out AliasedExternalStruct[] values) => context.TryReadAliasedExts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, AliasedExternalStruct[] values) => context.WriteAliasedExtsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekAliasedExts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadAliasedExts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out AliasedExternalStruct[] values) => context.TryPeekAliasedExts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out AliasedExternalStruct[] values) => context.TryReadAliasedExts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekAliasedExtsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadAliasedExtsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out AliasedExternalStruct[] values) => context.TryPeekAliasedExtsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out AliasedExternalStruct[] values) => context.TryReadAliasedExtsWithMaxCount(maxCount, out values),
    };
}

public class AliasedIncludeExternalStructTests : StructTestSuite<AliasedIncludeExternalStruct> {
    protected override AliasedIncludeExternalStruct Value => new() { Included = 7, Ignored = 0 };

    protected override AliasedIncludeExternalStruct[] Values => [
        new() { Included = 1, Ignored = 0 },
        new() { Included = 2, Ignored = 0 },
        new() { Included = 3, Ignored = 0 }
    ];

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

    protected override SerializationOperations<AliasedIncludeExternalStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, AliasedIncludeExternalStruct value) => context.WriteAliasedInc(value),
        Peek = (ReadContext context) => context.PeekAliasedInc(),
        Read = (ReadContext context) => context.ReadAliasedInc(),
        TryPeek = (ReadContext context, out AliasedIncludeExternalStruct value) => context.TryPeekAliasedInc(out value),
        TryRead = (ReadContext context, out AliasedIncludeExternalStruct value) => context.TryReadAliasedInc(out value),
        WriteSpan = (ref WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteAliasedIncs(values),
        PeekSpan = (ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncs(destination),
        ReadSpan = (ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncs(destination),
        TryPeekSpan = (ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.TryPeekAliasedIncs(destination),
        TryReadSpan = (ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.TryReadAliasedIncs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteAliasedIncsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.TryPeekAliasedIncs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.TryReadAliasedIncs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) => context.TryPeekAliasedIncsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AliasedIncludeExternalStruct> destination) => context.TryReadAliasedIncsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteAliasedIncs(values),
        PeekArray = (ReadContext context) => context.PeekAliasedIncs(),
        ReadArray = (ReadContext context) => context.ReadAliasedIncs(),
        TryPeekArray = (ReadContext context, out AliasedIncludeExternalStruct[] values) => context.TryPeekAliasedIncs(out values),
        TryReadArray = (ReadContext context, out AliasedIncludeExternalStruct[] values) => context.TryReadAliasedIncs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteAliasedIncsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekAliasedIncs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadAliasedIncs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out AliasedIncludeExternalStruct[] values) => context.TryPeekAliasedIncs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out AliasedIncludeExternalStruct[] values) => context.TryReadAliasedIncs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekAliasedIncsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadAliasedIncsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out AliasedIncludeExternalStruct[] values) => context.TryPeekAliasedIncsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out AliasedIncludeExternalStruct[] values) => context.TryReadAliasedIncsWithMaxCount(maxCount, out values),
    };
}
