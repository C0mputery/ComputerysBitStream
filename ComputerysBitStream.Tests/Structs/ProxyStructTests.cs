using Xunit;

namespace ComputerysBitStream.Tests;

public class ExternalPlainStructTests : StructTestSuite<ExternalPlainStruct> {
    protected override ExternalPlainStruct Value => new() { X = 42, Y = 3.14f };
    protected override ExternalPlainStruct[] Values => [
        new() { X = 1, Y = 1.0f },
        new() { X = 2, Y = 2.0f },
        new() { X = 3, Y = 3.0f }
    ];

    protected override void WriteNamed(WriteContext context, ExternalPlainStruct value) => context.WriteExternalPlainStruct(value);
    protected override ExternalPlainStruct PeekNamed(ReadContext context) => context.PeekExternalPlainStruct();
    protected override ExternalPlainStruct ReadNamed(ReadContext context) => context.ReadExternalPlainStruct();
    protected override void WriteAlias(WriteContext context, ExternalPlainStruct value) => context.Write(value);
    protected override ExternalPlainStruct PeekAlias(ReadContext context) { context.Peek(out ExternalPlainStruct v); return v; }
    protected override ExternalPlainStruct ReadAlias(ReadContext context) { context.Read(out ExternalPlainStruct v); return v; }
    protected override ExternalPlainStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekExternalPlainStruct(out ExternalPlainStruct v)); return v; }
    protected override ExternalPlainStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadExternalPlainStruct(out ExternalPlainStruct v)); return v; }
    protected override ExternalPlainStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out ExternalPlainStruct v)); return v; }
    protected override ExternalPlainStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out ExternalPlainStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, ExternalPlainStruct[] values) => context.WriteExternalPlainStructs(values);
    protected override ExternalPlainStruct[] PeekArrayNamed(ReadContext context) => context.PeekExternalPlainStructs();
    protected override ExternalPlainStruct[] ReadArrayNamed(ReadContext context) => context.ReadExternalPlainStructs();
    protected override void WriteArrayAlias(WriteContext context, ExternalPlainStruct[] values) => context.Write(values);
    protected override ExternalPlainStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out ExternalPlainStruct[] v); return v; }
    protected override ExternalPlainStruct[] ReadArrayAlias(ReadContext context) { context.Read(out ExternalPlainStruct[] v); return v; }
    protected override ExternalPlainStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekExternalPlainStructs(out ExternalPlainStruct[] v)); return v; }
    protected override ExternalPlainStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadExternalPlainStructs(out ExternalPlainStruct[] v)); return v; }
    protected override ExternalPlainStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out ExternalPlainStruct[] v)); return v; }
    protected override ExternalPlainStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out ExternalPlainStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, ExternalPlainStruct[] values) => context.WriteExternalPlainStructsWithoutLength(values);
    protected override ExternalPlainStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekExternalPlainStructs(count);
    protected override ExternalPlainStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadExternalPlainStructs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, ExternalPlainStruct[] values) => context.WriteWithoutLength(values);
    protected override ExternalPlainStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out ExternalPlainStruct[] v); return v; }
    protected override ExternalPlainStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out ExternalPlainStruct[] v); return v; }
    protected override ExternalPlainStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekExternalPlainStructs(count, out ExternalPlainStruct[] v)); return v; }
    protected override ExternalPlainStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadExternalPlainStructs(count, out ExternalPlainStruct[] v)); return v; }
    protected override ExternalPlainStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out ExternalPlainStruct[] v)); return v; }
    protected override ExternalPlainStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out ExternalPlainStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<ExternalPlainStruct> values) => context.WriteExternalPlainStructs(values);
    protected override void PeekSpanNamed(ReadContext context, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructs(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructs(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<ExternalPlainStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<ExternalPlainStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<ExternalPlainStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeekExternalPlainStructs(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<ExternalPlainStruct> destination) { Assert.True(context.TryReadExternalPlainStructs(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<ExternalPlainStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<ExternalPlainStruct> values) => context.WriteExternalPlainStructsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructs(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructs(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<ExternalPlainStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeekExternalPlainStructs(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<ExternalPlainStruct> destination) { Assert.True(context.TryReadExternalPlainStructs(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<ExternalPlainStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(ExternalPlainStruct value) => value.GetExternalPlainStructSizeInBits();
    protected override bool IsFixedSizeStruct(ExternalPlainStruct value) => value.IsExternalPlainStructFixedSizeStruct();
}

public class AnotherExternalStructTests : StructTestSuite<AnotherExternalStruct> {
    protected override AnotherExternalStruct Value => new() { Flag = true };
    protected override AnotherExternalStruct[] Values => [
        new() { Flag = true },
        new() { Flag = false },
        new() { Flag = true }
    ];

    protected override void WriteNamed(WriteContext context, AnotherExternalStruct value) => context.WriteAnotherExternalStruct(value);
    protected override AnotherExternalStruct PeekNamed(ReadContext context) => context.PeekAnotherExternalStruct();
    protected override AnotherExternalStruct ReadNamed(ReadContext context) => context.ReadAnotherExternalStruct();
    protected override void WriteAlias(WriteContext context, AnotherExternalStruct value) => context.Write(value);
    protected override AnotherExternalStruct PeekAlias(ReadContext context) { context.Peek(out AnotherExternalStruct v); return v; }
    protected override AnotherExternalStruct ReadAlias(ReadContext context) { context.Read(out AnotherExternalStruct v); return v; }
    protected override AnotherExternalStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekAnotherExternalStruct(out AnotherExternalStruct v)); return v; }
    protected override AnotherExternalStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadAnotherExternalStruct(out AnotherExternalStruct v)); return v; }
    protected override AnotherExternalStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out AnotherExternalStruct v)); return v; }
    protected override AnotherExternalStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out AnotherExternalStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, AnotherExternalStruct[] values) => context.WriteAnotherExternalStructs(values);
    protected override AnotherExternalStruct[] PeekArrayNamed(ReadContext context) => context.PeekAnotherExternalStructs();
    protected override AnotherExternalStruct[] ReadArrayNamed(ReadContext context) => context.ReadAnotherExternalStructs();
    protected override void WriteArrayAlias(WriteContext context, AnotherExternalStruct[] values) => context.Write(values);
    protected override AnotherExternalStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out AnotherExternalStruct[] v); return v; }
    protected override AnotherExternalStruct[] ReadArrayAlias(ReadContext context) { context.Read(out AnotherExternalStruct[] v); return v; }
    protected override AnotherExternalStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekAnotherExternalStructs(out AnotherExternalStruct[] v)); return v; }
    protected override AnotherExternalStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadAnotherExternalStructs(out AnotherExternalStruct[] v)); return v; }
    protected override AnotherExternalStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out AnotherExternalStruct[] v)); return v; }
    protected override AnotherExternalStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out AnotherExternalStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, AnotherExternalStruct[] values) => context.WriteAnotherExternalStructsWithoutLength(values);
    protected override AnotherExternalStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekAnotherExternalStructs(count);
    protected override AnotherExternalStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadAnotherExternalStructs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, AnotherExternalStruct[] values) => context.WriteWithoutLength(values);
    protected override AnotherExternalStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out AnotherExternalStruct[] v); return v; }
    protected override AnotherExternalStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out AnotherExternalStruct[] v); return v; }
    protected override AnotherExternalStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekAnotherExternalStructs(count, out AnotherExternalStruct[] v)); return v; }
    protected override AnotherExternalStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadAnotherExternalStructs(count, out AnotherExternalStruct[] v)); return v; }
    protected override AnotherExternalStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out AnotherExternalStruct[] v)); return v; }
    protected override AnotherExternalStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out AnotherExternalStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<AnotherExternalStruct> values) => context.WriteAnotherExternalStructs(values);
    protected override void PeekSpanNamed(ReadContext context, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructs(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructs(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<AnotherExternalStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<AnotherExternalStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<AnotherExternalStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeekAnotherExternalStructs(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<AnotherExternalStruct> destination) { Assert.True(context.TryReadAnotherExternalStructs(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<AnotherExternalStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<AnotherExternalStruct> values) => context.WriteAnotherExternalStructsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructs(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructs(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<AnotherExternalStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeekAnotherExternalStructs(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<AnotherExternalStruct> destination) { Assert.True(context.TryReadAnotherExternalStructs(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<AnotherExternalStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(AnotherExternalStruct value) => value.GetAnotherExternalStructSizeInBits();
    protected override bool IsFixedSizeStruct(AnotherExternalStruct value) => value.IsAnotherExternalStructFixedSizeStruct();
}

public class CaseTestStructTests : StructTestSuite<CaseTestStruct> {
    protected override CaseTestStruct Value => new() { Value = 123 };
    protected override CaseTestStruct[] Values => [
        new() { Value = 1 },
        new() { Value = 2 },
        new() { Value = 3 }
    ];

    protected override void WriteNamed(WriteContext context, CaseTestStruct value) => context.WriteCaseTestStruct(value);
    protected override CaseTestStruct PeekNamed(ReadContext context) => context.PeekCaseTestStruct();
    protected override CaseTestStruct ReadNamed(ReadContext context) => context.ReadCaseTestStruct();
    protected override void WriteAlias(WriteContext context, CaseTestStruct value) => context.Write(value);
    protected override CaseTestStruct PeekAlias(ReadContext context) { context.Peek(out CaseTestStruct v); return v; }
    protected override CaseTestStruct ReadAlias(ReadContext context) { context.Read(out CaseTestStruct v); return v; }
    protected override CaseTestStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekCaseTestStruct(out CaseTestStruct v)); return v; }
    protected override CaseTestStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadCaseTestStruct(out CaseTestStruct v)); return v; }
    protected override CaseTestStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out CaseTestStruct v)); return v; }
    protected override CaseTestStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out CaseTestStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, CaseTestStruct[] values) => context.WriteCaseTestStructs(values);
    protected override CaseTestStruct[] PeekArrayNamed(ReadContext context) => context.PeekCaseTestStructs();
    protected override CaseTestStruct[] ReadArrayNamed(ReadContext context) => context.ReadCaseTestStructs();
    protected override void WriteArrayAlias(WriteContext context, CaseTestStruct[] values) => context.Write(values);
    protected override CaseTestStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out CaseTestStruct[] v); return v; }
    protected override CaseTestStruct[] ReadArrayAlias(ReadContext context) { context.Read(out CaseTestStruct[] v); return v; }
    protected override CaseTestStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekCaseTestStructs(out CaseTestStruct[] v)); return v; }
    protected override CaseTestStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadCaseTestStructs(out CaseTestStruct[] v)); return v; }
    protected override CaseTestStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out CaseTestStruct[] v)); return v; }
    protected override CaseTestStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out CaseTestStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, CaseTestStruct[] values) => context.WriteCaseTestStructsWithoutLength(values);
    protected override CaseTestStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekCaseTestStructs(count);
    protected override CaseTestStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadCaseTestStructs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, CaseTestStruct[] values) => context.WriteWithoutLength(values);
    protected override CaseTestStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out CaseTestStruct[] v); return v; }
    protected override CaseTestStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out CaseTestStruct[] v); return v; }
    protected override CaseTestStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekCaseTestStructs(count, out CaseTestStruct[] v)); return v; }
    protected override CaseTestStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadCaseTestStructs(count, out CaseTestStruct[] v)); return v; }
    protected override CaseTestStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out CaseTestStruct[] v)); return v; }
    protected override CaseTestStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out CaseTestStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<CaseTestStruct> values) => context.WriteCaseTestStructs(values);
    protected override void PeekSpanNamed(ReadContext context, Span<CaseTestStruct> destination) => context.PeekCaseTestStructs(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<CaseTestStruct> destination) => context.ReadCaseTestStructs(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<CaseTestStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<CaseTestStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<CaseTestStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<CaseTestStruct> destination) { Assert.True(context.TryPeekCaseTestStructs(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<CaseTestStruct> destination) { Assert.True(context.TryReadCaseTestStructs(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<CaseTestStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<CaseTestStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<CaseTestStruct> values) => context.WriteCaseTestStructsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<CaseTestStruct> destination) => context.PeekCaseTestStructs(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<CaseTestStruct> destination) => context.ReadCaseTestStructs(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<CaseTestStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<CaseTestStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<CaseTestStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<CaseTestStruct> destination) { Assert.True(context.TryPeekCaseTestStructs(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<CaseTestStruct> destination) { Assert.True(context.TryReadCaseTestStructs(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<CaseTestStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<CaseTestStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(CaseTestStruct value) => value.GetCaseTestStructSizeInBits();
    protected override bool IsFixedSizeStruct(CaseTestStruct value) => value.IsCaseTestStructFixedSizeStruct();
}
