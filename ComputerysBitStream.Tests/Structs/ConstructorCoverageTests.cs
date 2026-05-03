using Xunit;

namespace ComputerysBitStream.Tests;

public class AliasedStructTests : StructTestSuite<AliasedStruct> {
    protected override AliasedStruct Value => new() { A = 42, B = 3.14f };
    protected override AliasedStruct[] Values => [
        new() { A = 1, B = 1.0f },
        new() { A = 2, B = 2.0f },
        new() { A = 3, B = 3.0f }
    ];

    protected override void WriteNamed(WriteContext context, AliasedStruct value) => context.WriteAliased(value);
    protected override AliasedStruct PeekNamed(ReadContext context) => context.PeekAliased();
    protected override AliasedStruct ReadNamed(ReadContext context) => context.ReadAliased();
    protected override void WriteAlias(WriteContext context, AliasedStruct value) => context.Write(value);
    protected override AliasedStruct PeekAlias(ReadContext context) { context.Peek(out AliasedStruct v); return v; }
    protected override AliasedStruct ReadAlias(ReadContext context) { context.Read(out AliasedStruct v); return v; }
    protected override AliasedStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekAliased(out AliasedStruct v)); return v; }
    protected override AliasedStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadAliased(out AliasedStruct v)); return v; }
    protected override AliasedStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out AliasedStruct v)); return v; }
    protected override AliasedStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out AliasedStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, AliasedStruct[] values) => context.WriteAliaseds(values);
    protected override AliasedStruct[] PeekArrayNamed(ReadContext context) => context.PeekAliaseds();
    protected override AliasedStruct[] ReadArrayNamed(ReadContext context) => context.ReadAliaseds();
    protected override void WriteArrayAlias(WriteContext context, AliasedStruct[] values) => context.Write(values);
    protected override AliasedStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out AliasedStruct[] v); return v; }
    protected override AliasedStruct[] ReadArrayAlias(ReadContext context) { context.Read(out AliasedStruct[] v); return v; }
    protected override AliasedStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekAliaseds(out AliasedStruct[] v)); return v; }
    protected override AliasedStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadAliaseds(out AliasedStruct[] v)); return v; }
    protected override AliasedStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out AliasedStruct[] v)); return v; }
    protected override AliasedStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out AliasedStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, AliasedStruct[] values) => context.WriteAliasedsWithoutLength(values);
    protected override AliasedStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekAliaseds(count);
    protected override AliasedStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadAliaseds(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, AliasedStruct[] values) => context.WriteWithoutLength(values);
    protected override AliasedStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out AliasedStruct[] v); return v; }
    protected override AliasedStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out AliasedStruct[] v); return v; }
    protected override AliasedStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekAliaseds(count, out AliasedStruct[] v)); return v; }
    protected override AliasedStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadAliaseds(count, out AliasedStruct[] v)); return v; }
    protected override AliasedStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out AliasedStruct[] v)); return v; }
    protected override AliasedStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out AliasedStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<AliasedStruct> values) => context.WriteAliaseds(values);
    protected override void PeekSpanNamed(ReadContext context, Span<AliasedStruct> destination) => context.PeekAliaseds(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<AliasedStruct> destination) => context.ReadAliaseds(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<AliasedStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<AliasedStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<AliasedStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<AliasedStruct> destination) { Assert.True(context.TryPeekAliaseds(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<AliasedStruct> destination) { Assert.True(context.TryReadAliaseds(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<AliasedStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<AliasedStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<AliasedStruct> values) => context.WriteAliasedsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedStruct> destination) => context.PeekAliaseds(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedStruct> destination) => context.ReadAliaseds(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<AliasedStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedStruct> destination) { Assert.True(context.TryPeekAliaseds(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedStruct> destination) { Assert.True(context.TryReadAliaseds(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(AliasedStruct value) => value.GetAliasedSizeInBits();
    protected override bool IsFixedSizeStruct(AliasedStruct value) => value.IsAliasedFixedSizeStruct();
}

public class AliasedExternalStructTests : StructTestSuite<AliasedExternalStruct> {
    protected override AliasedExternalStruct Value => new() { X = 99, Y = true };
    protected override AliasedExternalStruct[] Values => [
        new() { X = 1, Y = true },
        new() { X = 2, Y = false },
        new() { X = 3, Y = true }
    ];

    protected override void WriteNamed(WriteContext context, AliasedExternalStruct value) => context.WriteAliasedExt(value);
    protected override AliasedExternalStruct PeekNamed(ReadContext context) => context.PeekAliasedExt();
    protected override AliasedExternalStruct ReadNamed(ReadContext context) => context.ReadAliasedExt();
    protected override void WriteAlias(WriteContext context, AliasedExternalStruct value) => context.Write(value);
    protected override AliasedExternalStruct PeekAlias(ReadContext context) { context.Peek(out AliasedExternalStruct v); return v; }
    protected override AliasedExternalStruct ReadAlias(ReadContext context) { context.Read(out AliasedExternalStruct v); return v; }
    protected override AliasedExternalStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekAliasedExt(out AliasedExternalStruct v)); return v; }
    protected override AliasedExternalStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadAliasedExt(out AliasedExternalStruct v)); return v; }
    protected override AliasedExternalStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out AliasedExternalStruct v)); return v; }
    protected override AliasedExternalStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out AliasedExternalStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, AliasedExternalStruct[] values) => context.WriteAliasedExts(values);
    protected override AliasedExternalStruct[] PeekArrayNamed(ReadContext context) => context.PeekAliasedExts();
    protected override AliasedExternalStruct[] ReadArrayNamed(ReadContext context) => context.ReadAliasedExts();
    protected override void WriteArrayAlias(WriteContext context, AliasedExternalStruct[] values) => context.Write(values);
    protected override AliasedExternalStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out AliasedExternalStruct[] v); return v; }
    protected override AliasedExternalStruct[] ReadArrayAlias(ReadContext context) { context.Read(out AliasedExternalStruct[] v); return v; }
    protected override AliasedExternalStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekAliasedExts(out AliasedExternalStruct[] v)); return v; }
    protected override AliasedExternalStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadAliasedExts(out AliasedExternalStruct[] v)); return v; }
    protected override AliasedExternalStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out AliasedExternalStruct[] v)); return v; }
    protected override AliasedExternalStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out AliasedExternalStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, AliasedExternalStruct[] values) => context.WriteAliasedExtsWithoutLength(values);
    protected override AliasedExternalStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekAliasedExts(count);
    protected override AliasedExternalStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadAliasedExts(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, AliasedExternalStruct[] values) => context.WriteWithoutLength(values);
    protected override AliasedExternalStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out AliasedExternalStruct[] v); return v; }
    protected override AliasedExternalStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out AliasedExternalStruct[] v); return v; }
    protected override AliasedExternalStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekAliasedExts(count, out AliasedExternalStruct[] v)); return v; }
    protected override AliasedExternalStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadAliasedExts(count, out AliasedExternalStruct[] v)); return v; }
    protected override AliasedExternalStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out AliasedExternalStruct[] v)); return v; }
    protected override AliasedExternalStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out AliasedExternalStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<AliasedExternalStruct> values) => context.WriteAliasedExts(values);
    protected override void PeekSpanNamed(ReadContext context, Span<AliasedExternalStruct> destination) => context.PeekAliasedExts(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<AliasedExternalStruct> destination) => context.ReadAliasedExts(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<AliasedExternalStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<AliasedExternalStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<AliasedExternalStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeekAliasedExts(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<AliasedExternalStruct> destination) { Assert.True(context.TryReadAliasedExts(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<AliasedExternalStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<AliasedExternalStruct> values) => context.WriteAliasedExtsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.PeekAliasedExts(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.ReadAliasedExts(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<AliasedExternalStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedExternalStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeekAliasedExts(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedExternalStruct> destination) { Assert.True(context.TryReadAliasedExts(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedExternalStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedExternalStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(AliasedExternalStruct value) => value.GetAliasedExtSizeInBits();
    protected override bool IsFixedSizeStruct(AliasedExternalStruct value) => value.IsAliasedExtFixedSizeStruct();
}

public class AliasedIncludeExternalStructTests : StructTestSuite<AliasedIncludeExternalStruct> {
    protected override AliasedIncludeExternalStruct Value => new() { Included = 7, Ignored = 0 };
    protected override AliasedIncludeExternalStruct[] Values => [
        new() { Included = 1, Ignored = 0 },
        new() { Included = 2, Ignored = 0 },
        new() { Included = 3, Ignored = 0 }
    ];

    protected override void WriteNamed(WriteContext context, AliasedIncludeExternalStruct value) => context.WriteAliasedInc(value);
    protected override AliasedIncludeExternalStruct PeekNamed(ReadContext context) => context.PeekAliasedInc();
    protected override AliasedIncludeExternalStruct ReadNamed(ReadContext context) => context.ReadAliasedInc();
    protected override void WriteAlias(WriteContext context, AliasedIncludeExternalStruct value) => context.Write(value);
    protected override AliasedIncludeExternalStruct PeekAlias(ReadContext context) { context.Peek(out AliasedIncludeExternalStruct v); return v; }
    protected override AliasedIncludeExternalStruct ReadAlias(ReadContext context) { context.Read(out AliasedIncludeExternalStruct v); return v; }
    protected override AliasedIncludeExternalStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekAliasedInc(out AliasedIncludeExternalStruct v)); return v; }
    protected override AliasedIncludeExternalStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadAliasedInc(out AliasedIncludeExternalStruct v)); return v; }
    protected override AliasedIncludeExternalStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out AliasedIncludeExternalStruct v)); return v; }
    protected override AliasedIncludeExternalStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out AliasedIncludeExternalStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteAliasedIncs(values);
    protected override AliasedIncludeExternalStruct[] PeekArrayNamed(ReadContext context) => context.PeekAliasedIncs();
    protected override AliasedIncludeExternalStruct[] ReadArrayNamed(ReadContext context) => context.ReadAliasedIncs();
    protected override void WriteArrayAlias(WriteContext context, AliasedIncludeExternalStruct[] values) => context.Write(values);
    protected override AliasedIncludeExternalStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out AliasedIncludeExternalStruct[] v); return v; }
    protected override AliasedIncludeExternalStruct[] ReadArrayAlias(ReadContext context) { context.Read(out AliasedIncludeExternalStruct[] v); return v; }
    protected override AliasedIncludeExternalStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekAliasedIncs(out AliasedIncludeExternalStruct[] v)); return v; }
    protected override AliasedIncludeExternalStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadAliasedIncs(out AliasedIncludeExternalStruct[] v)); return v; }
    protected override AliasedIncludeExternalStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out AliasedIncludeExternalStruct[] v)); return v; }
    protected override AliasedIncludeExternalStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out AliasedIncludeExternalStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteAliasedIncsWithoutLength(values);
    protected override AliasedIncludeExternalStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekAliasedIncs(count);
    protected override AliasedIncludeExternalStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadAliasedIncs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, AliasedIncludeExternalStruct[] values) => context.WriteWithoutLength(values);
    protected override AliasedIncludeExternalStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out AliasedIncludeExternalStruct[] v); return v; }
    protected override AliasedIncludeExternalStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out AliasedIncludeExternalStruct[] v); return v; }
    protected override AliasedIncludeExternalStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekAliasedIncs(count, out AliasedIncludeExternalStruct[] v)); return v; }
    protected override AliasedIncludeExternalStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadAliasedIncs(count, out AliasedIncludeExternalStruct[] v)); return v; }
    protected override AliasedIncludeExternalStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out AliasedIncludeExternalStruct[] v)); return v; }
    protected override AliasedIncludeExternalStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out AliasedIncludeExternalStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteAliasedIncs(values);
    protected override void PeekSpanNamed(ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncs(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncs(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<AliasedIncludeExternalStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeekAliasedIncs(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryReadAliasedIncs(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteAliasedIncsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.PeekAliasedIncs(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.ReadAliasedIncs(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<AliasedIncludeExternalStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeekAliasedIncs(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryReadAliasedIncs(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AliasedIncludeExternalStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(AliasedIncludeExternalStruct value) => value.GetAliasedIncSizeInBits();
    protected override bool IsFixedSizeStruct(AliasedIncludeExternalStruct value) => value.IsAliasedIncFixedSizeStruct();

    [Fact]
    public void IgnoredMember_ShouldNotAffectEquality() {
        AliasedIncludeExternalStruct original = new() { Included = 42, Ignored = 100 };
        AliasedIncludeExternalStruct modified = new() { Included = 42, Ignored = 200 };

        ulong[] buffer = new ulong[16];
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
}
