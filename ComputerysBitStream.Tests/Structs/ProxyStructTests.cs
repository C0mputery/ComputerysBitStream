namespace ComputerysBitStream.Tests.Structs;

public class ExternalPlainStructTests : StructTestSuite<ExternalPlainStruct> {
    protected override ExternalPlainStruct Value => new() { X = 42, Y = 3.14f };

    protected override ExternalPlainStruct[] Values => [
        new() { X = 1, Y = 1.0f },
        new() { X = 2, Y = 2.0f },
        new() { X = 3, Y = 3.0f }
    ];

    protected override void Write(ref WriteContext context, ExternalPlainStruct value) => context.WriteExternalPlainStruct(value);
    protected override ExternalPlainStruct Peek(ReadContext context) => context.PeekExternalPlainStruct();
    protected override ExternalPlainStruct Read(ReadContext context) => context.ReadExternalPlainStruct();

    protected override ExternalPlainStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekExternalPlainStruct(out ExternalPlainStruct v));
        return v;
    }

    protected override ExternalPlainStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadExternalPlainStruct(out ExternalPlainStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, ExternalPlainStruct[] values) => context.WriteExternalPlainStructs(values);
    protected override ExternalPlainStruct[] PeekArrayWithLength(ReadContext context) => context.PeekExternalPlainStructs();
    protected override ExternalPlainStruct[] ReadArrayWithLength(ReadContext context) => context.ReadExternalPlainStructs();

    protected override ExternalPlainStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekExternalPlainStructs(out ExternalPlainStruct[] v));
        return v;
    }

    protected override ExternalPlainStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadExternalPlainStructs(out ExternalPlainStruct[] v));
        return v;
    }

    protected override ExternalPlainStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekExternalPlainStructsWithMaxCount(maxCount);
    protected override ExternalPlainStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadExternalPlainStructsWithMaxCount(maxCount);

    protected override ExternalPlainStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekExternalPlainStructsWithMaxCount(maxCount, out ExternalPlainStruct[] values));
        return values;
    }

    protected override ExternalPlainStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadExternalPlainStructsWithMaxCount(maxCount, out ExternalPlainStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeekExternalPlainStructsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) { Assert.True(context.TryReadExternalPlainStructsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, ExternalPlainStruct[] values) => context.WriteExternalPlainStructsWithoutLength(values);
    protected override ExternalPlainStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekExternalPlainStructs(count);
    protected override ExternalPlainStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadExternalPlainStructs(count);

    protected override ExternalPlainStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekExternalPlainStructs(count, out ExternalPlainStruct[] v));
        return v;
    }

    protected override ExternalPlainStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadExternalPlainStructs(count, out ExternalPlainStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<ExternalPlainStruct> values) => context.WriteExternalPlainStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeekExternalPlainStructs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<ExternalPlainStruct> destination) { Assert.True(context.TryReadExternalPlainStructs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<ExternalPlainStruct> values) => context.WriteExternalPlainStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<ExternalPlainStruct> destination) { Assert.True(context.TryPeekExternalPlainStructs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<ExternalPlainStruct> destination) { Assert.True(context.TryReadExternalPlainStructs(count, destination)); }

    protected override Type StructType => typeof(ExternalPlainStruct);

    protected override TryReadOperationSet<ExternalPlainStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out ExternalPlainStruct v) => c.TryPeekExternalPlainStruct(out v),
        TryReadValue = (ReadContext c, out ExternalPlainStruct v) => c.TryReadExternalPlainStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out ExternalPlainStruct[] v) => c.TryPeekExternalPlainStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out ExternalPlainStruct[] v) => c.TryReadExternalPlainStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out ExternalPlainStruct[] v) => c.TryPeekExternalPlainStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out ExternalPlainStruct[] v) => c.TryReadExternalPlainStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<ExternalPlainStruct> d) => c.TryPeekExternalPlainStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<ExternalPlainStruct> d) => c.TryReadExternalPlainStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<ExternalPlainStruct> d) => c.TryPeekExternalPlainStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<ExternalPlainStruct> d) => c.TryReadExternalPlainStructs(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out ExternalPlainStruct[] v) => c.TryPeekExternalPlainStructsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out ExternalPlainStruct[] v) => c.TryReadExternalPlainStructsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<ExternalPlainStruct> d) => c.TryPeekExternalPlainStructsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<ExternalPlainStruct> d) => c.TryReadExternalPlainStructsWithMaxCount(maxCount, d),
    };
}

public class AnotherExternalStructTests : StructTestSuite<AnotherExternalStruct> {
    protected override AnotherExternalStruct Value => new() { Flag = true };

    protected override AnotherExternalStruct[] Values => [
        new() { Flag = true },
        new() { Flag = false },
        new() { Flag = true }
    ];

    protected override void Write(ref WriteContext context, AnotherExternalStruct value) => context.WriteAnotherExternalStruct(value);
    protected override AnotherExternalStruct Peek(ReadContext context) => context.PeekAnotherExternalStruct();
    protected override AnotherExternalStruct Read(ReadContext context) => context.ReadAnotherExternalStruct();

    protected override AnotherExternalStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekAnotherExternalStruct(out AnotherExternalStruct v));
        return v;
    }

    protected override AnotherExternalStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadAnotherExternalStruct(out AnotherExternalStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, AnotherExternalStruct[] values) => context.WriteAnotherExternalStructs(values);
    protected override AnotherExternalStruct[] PeekArrayWithLength(ReadContext context) => context.PeekAnotherExternalStructs();
    protected override AnotherExternalStruct[] ReadArrayWithLength(ReadContext context) => context.ReadAnotherExternalStructs();

    protected override AnotherExternalStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekAnotherExternalStructs(out AnotherExternalStruct[] v));
        return v;
    }

    protected override AnotherExternalStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadAnotherExternalStructs(out AnotherExternalStruct[] v));
        return v;
    }

    protected override AnotherExternalStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekAnotherExternalStructsWithMaxCount(maxCount);
    protected override AnotherExternalStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadAnotherExternalStructsWithMaxCount(maxCount);

    protected override AnotherExternalStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekAnotherExternalStructsWithMaxCount(maxCount, out AnotherExternalStruct[] values));
        return values;
    }

    protected override AnotherExternalStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadAnotherExternalStructsWithMaxCount(maxCount, out AnotherExternalStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeekAnotherExternalStructsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) { Assert.True(context.TryReadAnotherExternalStructsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, AnotherExternalStruct[] values) => context.WriteAnotherExternalStructsWithoutLength(values);
    protected override AnotherExternalStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekAnotherExternalStructs(count);
    protected override AnotherExternalStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadAnotherExternalStructs(count);

    protected override AnotherExternalStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekAnotherExternalStructs(count, out AnotherExternalStruct[] v));
        return v;
    }

    protected override AnotherExternalStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadAnotherExternalStructs(count, out AnotherExternalStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<AnotherExternalStruct> values) => context.WriteAnotherExternalStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeekAnotherExternalStructs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<AnotherExternalStruct> destination) { Assert.True(context.TryReadAnotherExternalStructs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<AnotherExternalStruct> values) => context.WriteAnotherExternalStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<AnotherExternalStruct> destination) { Assert.True(context.TryPeekAnotherExternalStructs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<AnotherExternalStruct> destination) { Assert.True(context.TryReadAnotherExternalStructs(count, destination)); }

    protected override Type StructType => typeof(AnotherExternalStruct);

    protected override TryReadOperationSet<AnotherExternalStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out AnotherExternalStruct v) => c.TryPeekAnotherExternalStruct(out v),
        TryReadValue = (ReadContext c, out AnotherExternalStruct v) => c.TryReadAnotherExternalStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out AnotherExternalStruct[] v) => c.TryPeekAnotherExternalStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out AnotherExternalStruct[] v) => c.TryReadAnotherExternalStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out AnotherExternalStruct[] v) => c.TryPeekAnotherExternalStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out AnotherExternalStruct[] v) => c.TryReadAnotherExternalStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<AnotherExternalStruct> d) => c.TryPeekAnotherExternalStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<AnotherExternalStruct> d) => c.TryReadAnotherExternalStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<AnotherExternalStruct> d) => c.TryPeekAnotherExternalStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<AnotherExternalStruct> d) => c.TryReadAnotherExternalStructs(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out AnotherExternalStruct[] v) => c.TryPeekAnotherExternalStructsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out AnotherExternalStruct[] v) => c.TryReadAnotherExternalStructsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<AnotherExternalStruct> d) => c.TryPeekAnotherExternalStructsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<AnotherExternalStruct> d) => c.TryReadAnotherExternalStructsWithMaxCount(maxCount, d),
    };
}

public class CaseTestStructTests : StructTestSuite<CaseTestStruct> {
    protected override CaseTestStruct Value => new() { Value = 123 };

    protected override CaseTestStruct[] Values => [
        new() { Value = 1 },
        new() { Value = 2 },
        new() { Value = 3 }
    ];

    protected override void Write(ref WriteContext context, CaseTestStruct value) => context.WriteCaseTestStruct(value);
    protected override CaseTestStruct Peek(ReadContext context) => context.PeekCaseTestStruct();
    protected override CaseTestStruct Read(ReadContext context) => context.ReadCaseTestStruct();

    protected override CaseTestStruct TryPeek(ReadContext context) {
        Assert.True(context.TryPeekCaseTestStruct(out CaseTestStruct v));
        return v;
    }

    protected override CaseTestStruct TryRead(ReadContext context) {
        Assert.True(context.TryReadCaseTestStruct(out CaseTestStruct v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, CaseTestStruct[] values) => context.WriteCaseTestStructs(values);
    protected override CaseTestStruct[] PeekArrayWithLength(ReadContext context) => context.PeekCaseTestStructs();
    protected override CaseTestStruct[] ReadArrayWithLength(ReadContext context) => context.ReadCaseTestStructs();

    protected override CaseTestStruct[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekCaseTestStructs(out CaseTestStruct[] v));
        return v;
    }

    protected override CaseTestStruct[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadCaseTestStructs(out CaseTestStruct[] v));
        return v;
    }

    protected override CaseTestStruct[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekCaseTestStructsWithMaxCount(maxCount);
    protected override CaseTestStruct[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadCaseTestStructsWithMaxCount(maxCount);

    protected override CaseTestStruct[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekCaseTestStructsWithMaxCount(maxCount, out CaseTestStruct[] values));
        return values;
    }

    protected override CaseTestStruct[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadCaseTestStructsWithMaxCount(maxCount, out CaseTestStruct[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<CaseTestStruct> destination) => context.PeekCaseTestStructsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<CaseTestStruct> destination) => context.ReadCaseTestStructsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<CaseTestStruct> destination) { Assert.True(context.TryPeekCaseTestStructsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<CaseTestStruct> destination) { Assert.True(context.TryReadCaseTestStructsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, CaseTestStruct[] values) => context.WriteCaseTestStructsWithoutLength(values);
    protected override CaseTestStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekCaseTestStructs(count);
    protected override CaseTestStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadCaseTestStructs(count);

    protected override CaseTestStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekCaseTestStructs(count, out CaseTestStruct[] v));
        return v;
    }

    protected override CaseTestStruct[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadCaseTestStructs(count, out CaseTestStruct[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<CaseTestStruct> values) => context.WriteCaseTestStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<CaseTestStruct> destination) => context.PeekCaseTestStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<CaseTestStruct> destination) => context.ReadCaseTestStructs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<CaseTestStruct> destination) { Assert.True(context.TryPeekCaseTestStructs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<CaseTestStruct> destination) { Assert.True(context.TryReadCaseTestStructs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<CaseTestStruct> values) => context.WriteCaseTestStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<CaseTestStruct> destination) => context.PeekCaseTestStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<CaseTestStruct> destination) => context.ReadCaseTestStructs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<CaseTestStruct> destination) { Assert.True(context.TryPeekCaseTestStructs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<CaseTestStruct> destination) { Assert.True(context.TryReadCaseTestStructs(count, destination)); }

    protected override Type StructType => typeof(CaseTestStruct);

    protected override TryReadOperationSet<CaseTestStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out CaseTestStruct v) => c.TryPeekCaseTestStruct(out v),
        TryReadValue = (ReadContext c, out CaseTestStruct v) => c.TryReadCaseTestStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out CaseTestStruct[] v) => c.TryPeekCaseTestStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out CaseTestStruct[] v) => c.TryReadCaseTestStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out CaseTestStruct[] v) => c.TryPeekCaseTestStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out CaseTestStruct[] v) => c.TryReadCaseTestStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<CaseTestStruct> d) => c.TryPeekCaseTestStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<CaseTestStruct> d) => c.TryReadCaseTestStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<CaseTestStruct> d) => c.TryPeekCaseTestStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<CaseTestStruct> d) => c.TryReadCaseTestStructs(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out CaseTestStruct[] v) => c.TryPeekCaseTestStructsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out CaseTestStruct[] v) => c.TryReadCaseTestStructsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<CaseTestStruct> d) => c.TryPeekCaseTestStructsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<CaseTestStruct> d) => c.TryReadCaseTestStructsWithMaxCount(maxCount, d),
    };
}
