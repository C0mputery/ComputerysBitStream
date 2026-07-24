using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class SimpleStructTests : StructTestSuite<SimpleStruct> {
    protected override SimpleStruct Value => new() { X = 42, Y = 3.14f, Z = true };

    protected override SimpleStruct[] Values => [
        new() { X = 1, Y = 1.0f, Z = true },
        new() { X = 2, Y = 2.0f, Z = false },
        new() { X = 3, Y = 3.0f, Z = true }
    ];

    protected override Type StructType => typeof(SimpleStruct);

    protected override SerializationOperations<SimpleStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, SimpleStruct value) => context.WriteSimpleStruct(value),
        Peek = (ReadContext context) => context.PeekSimpleStruct(),
        Read = (ReadContext context) => context.ReadSimpleStruct(),
        TryPeek = (ReadContext context, out SimpleStruct value) => context.TryPeekSimpleStruct(out value),
        TryRead = (ReadContext context, out SimpleStruct value) => context.TryReadSimpleStruct(out value),
        WriteSpan = (ref WriteContext context, Span<SimpleStruct> values) => context.WriteSimpleStructs(values),
        PeekSpan = (ReadContext context, Span<SimpleStruct> destination) => context.PeekSimpleStructs(destination),
        ReadSpan = (ReadContext context, Span<SimpleStruct> destination) => context.ReadSimpleStructs(destination),
        TryPeekSpan = (ReadContext context, Span<SimpleStruct> destination) => context.TryPeekSimpleStructs(destination),
        TryReadSpan = (ReadContext context, Span<SimpleStruct> destination) => context.TryReadSimpleStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<SimpleStruct> values) => context.WriteSimpleStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<SimpleStruct> destination) => context.PeekSimpleStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<SimpleStruct> destination) => context.ReadSimpleStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<SimpleStruct> destination) => context.TryPeekSimpleStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<SimpleStruct> destination) => context.TryReadSimpleStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<SimpleStruct> destination) => context.PeekSimpleStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<SimpleStruct> destination) => context.ReadSimpleStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<SimpleStruct> destination) => context.TryPeekSimpleStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<SimpleStruct> destination) => context.TryReadSimpleStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, SimpleStruct[] values) => context.WriteSimpleStructs(values),
        PeekArray = (ReadContext context) => context.PeekSimpleStructs(),
        ReadArray = (ReadContext context) => context.ReadSimpleStructs(),
        TryPeekArray = (ReadContext context, out SimpleStruct[] values) => context.TryPeekSimpleStructs(out values),
        TryReadArray = (ReadContext context, out SimpleStruct[] values) => context.TryReadSimpleStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, SimpleStruct[] values) => context.WriteSimpleStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekSimpleStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadSimpleStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out SimpleStruct[] values) => context.TryPeekSimpleStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out SimpleStruct[] values) => context.TryReadSimpleStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekSimpleStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadSimpleStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out SimpleStruct[] values) => context.TryPeekSimpleStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out SimpleStruct[] values) => context.TryReadSimpleStructsWithMaxCount(maxCount, out values),
    };
}
