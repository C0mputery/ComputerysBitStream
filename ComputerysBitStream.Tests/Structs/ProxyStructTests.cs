using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class ExternalPlainStructTests : StructTestSuite<ExternalPlainStruct> {
    protected override ExternalPlainStruct Value => new() { X = 42, Y = 3.14f };

    protected override ExternalPlainStruct[] Values => [
        new() { X = 1, Y = 1.0f },
        new() { X = 2, Y = 2.0f },
        new() { X = 3, Y = 3.0f }
    ];

    protected override Type StructType => typeof(ExternalPlainStruct);

    protected override SerializationOperations<ExternalPlainStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, ExternalPlainStruct value) => context.WriteExternalPlainStruct(value),
        Peek = (ReadContext context) => context.PeekExternalPlainStruct(),
        Read = (ReadContext context) => context.ReadExternalPlainStruct(),
        TryPeek = (ReadContext context, out ExternalPlainStruct value) => context.TryPeekExternalPlainStruct(out value),
        TryRead = (ReadContext context, out ExternalPlainStruct value) => context.TryReadExternalPlainStruct(out value),
        WriteSpan = (ref WriteContext context, Span<ExternalPlainStruct> values) => context.WriteExternalPlainStructs(values),
        PeekSpan = (ReadContext context, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructs(destination),
        ReadSpan = (ReadContext context, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructs(destination),
        TryPeekSpan = (ReadContext context, Span<ExternalPlainStruct> destination) => context.TryPeekExternalPlainStructs(destination),
        TryReadSpan = (ReadContext context, Span<ExternalPlainStruct> destination) => context.TryReadExternalPlainStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<ExternalPlainStruct> values) => context.WriteExternalPlainStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.TryPeekExternalPlainStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<ExternalPlainStruct> destination) => context.TryReadExternalPlainStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) => context.PeekExternalPlainStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) => context.ReadExternalPlainStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) => context.TryPeekExternalPlainStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ExternalPlainStruct> destination) => context.TryReadExternalPlainStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, ExternalPlainStruct[] values) => context.WriteExternalPlainStructs(values),
        PeekArray = (ReadContext context) => context.PeekExternalPlainStructs(),
        ReadArray = (ReadContext context) => context.ReadExternalPlainStructs(),
        TryPeekArray = (ReadContext context, out ExternalPlainStruct[] values) => context.TryPeekExternalPlainStructs(out values),
        TryReadArray = (ReadContext context, out ExternalPlainStruct[] values) => context.TryReadExternalPlainStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, ExternalPlainStruct[] values) => context.WriteExternalPlainStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekExternalPlainStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadExternalPlainStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out ExternalPlainStruct[] values) => context.TryPeekExternalPlainStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out ExternalPlainStruct[] values) => context.TryReadExternalPlainStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekExternalPlainStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadExternalPlainStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out ExternalPlainStruct[] values) => context.TryPeekExternalPlainStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out ExternalPlainStruct[] values) => context.TryReadExternalPlainStructsWithMaxCount(maxCount, out values),
    };
}

public class AnotherExternalStructTests : StructTestSuite<AnotherExternalStruct> {
    protected override AnotherExternalStruct Value => new() { Flag = true };

    protected override AnotherExternalStruct[] Values => [
        new() { Flag = true },
        new() { Flag = false },
        new() { Flag = true }
    ];

    protected override Type StructType => typeof(AnotherExternalStruct);

    protected override SerializationOperations<AnotherExternalStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, AnotherExternalStruct value) => context.WriteAnotherExternalStruct(value),
        Peek = (ReadContext context) => context.PeekAnotherExternalStruct(),
        Read = (ReadContext context) => context.ReadAnotherExternalStruct(),
        TryPeek = (ReadContext context, out AnotherExternalStruct value) => context.TryPeekAnotherExternalStruct(out value),
        TryRead = (ReadContext context, out AnotherExternalStruct value) => context.TryReadAnotherExternalStruct(out value),
        WriteSpan = (ref WriteContext context, Span<AnotherExternalStruct> values) => context.WriteAnotherExternalStructs(values),
        PeekSpan = (ReadContext context, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructs(destination),
        ReadSpan = (ReadContext context, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructs(destination),
        TryPeekSpan = (ReadContext context, Span<AnotherExternalStruct> destination) => context.TryPeekAnotherExternalStructs(destination),
        TryReadSpan = (ReadContext context, Span<AnotherExternalStruct> destination) => context.TryReadAnotherExternalStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<AnotherExternalStruct> values) => context.WriteAnotherExternalStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.TryPeekAnotherExternalStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<AnotherExternalStruct> destination) => context.TryReadAnotherExternalStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) => context.PeekAnotherExternalStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) => context.ReadAnotherExternalStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) => context.TryPeekAnotherExternalStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<AnotherExternalStruct> destination) => context.TryReadAnotherExternalStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, AnotherExternalStruct[] values) => context.WriteAnotherExternalStructs(values),
        PeekArray = (ReadContext context) => context.PeekAnotherExternalStructs(),
        ReadArray = (ReadContext context) => context.ReadAnotherExternalStructs(),
        TryPeekArray = (ReadContext context, out AnotherExternalStruct[] values) => context.TryPeekAnotherExternalStructs(out values),
        TryReadArray = (ReadContext context, out AnotherExternalStruct[] values) => context.TryReadAnotherExternalStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, AnotherExternalStruct[] values) => context.WriteAnotherExternalStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekAnotherExternalStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadAnotherExternalStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out AnotherExternalStruct[] values) => context.TryPeekAnotherExternalStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out AnotherExternalStruct[] values) => context.TryReadAnotherExternalStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekAnotherExternalStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadAnotherExternalStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out AnotherExternalStruct[] values) => context.TryPeekAnotherExternalStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out AnotherExternalStruct[] values) => context.TryReadAnotherExternalStructsWithMaxCount(maxCount, out values),
    };
}

public class CaseTestStructTests : StructTestSuite<CaseTestStruct> {
    protected override CaseTestStruct Value => new() { Value = 123 };

    protected override CaseTestStruct[] Values => [
        new() { Value = 1 },
        new() { Value = 2 },
        new() { Value = 3 }
    ];

    protected override Type StructType => typeof(CaseTestStruct);

    protected override SerializationOperations<CaseTestStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, CaseTestStruct value) => context.WriteCaseTestStruct(value),
        Peek = (ReadContext context) => context.PeekCaseTestStruct(),
        Read = (ReadContext context) => context.ReadCaseTestStruct(),
        TryPeek = (ReadContext context, out CaseTestStruct value) => context.TryPeekCaseTestStruct(out value),
        TryRead = (ReadContext context, out CaseTestStruct value) => context.TryReadCaseTestStruct(out value),
        WriteSpan = (ref WriteContext context, Span<CaseTestStruct> values) => context.WriteCaseTestStructs(values),
        PeekSpan = (ReadContext context, Span<CaseTestStruct> destination) => context.PeekCaseTestStructs(destination),
        ReadSpan = (ReadContext context, Span<CaseTestStruct> destination) => context.ReadCaseTestStructs(destination),
        TryPeekSpan = (ReadContext context, Span<CaseTestStruct> destination) => context.TryPeekCaseTestStructs(destination),
        TryReadSpan = (ReadContext context, Span<CaseTestStruct> destination) => context.TryReadCaseTestStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<CaseTestStruct> values) => context.WriteCaseTestStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<CaseTestStruct> destination) => context.PeekCaseTestStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<CaseTestStruct> destination) => context.ReadCaseTestStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<CaseTestStruct> destination) => context.TryPeekCaseTestStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<CaseTestStruct> destination) => context.TryReadCaseTestStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<CaseTestStruct> destination) => context.PeekCaseTestStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<CaseTestStruct> destination) => context.ReadCaseTestStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<CaseTestStruct> destination) => context.TryPeekCaseTestStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<CaseTestStruct> destination) => context.TryReadCaseTestStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, CaseTestStruct[] values) => context.WriteCaseTestStructs(values),
        PeekArray = (ReadContext context) => context.PeekCaseTestStructs(),
        ReadArray = (ReadContext context) => context.ReadCaseTestStructs(),
        TryPeekArray = (ReadContext context, out CaseTestStruct[] values) => context.TryPeekCaseTestStructs(out values),
        TryReadArray = (ReadContext context, out CaseTestStruct[] values) => context.TryReadCaseTestStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, CaseTestStruct[] values) => context.WriteCaseTestStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekCaseTestStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadCaseTestStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out CaseTestStruct[] values) => context.TryPeekCaseTestStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out CaseTestStruct[] values) => context.TryReadCaseTestStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekCaseTestStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadCaseTestStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out CaseTestStruct[] values) => context.TryPeekCaseTestStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out CaseTestStruct[] values) => context.TryReadCaseTestStructsWithMaxCount(maxCount, out values),
    };
}
