using ComputerysBitStream.Attributes;
using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

[BitStreamPrimitiveContext]
public class VariableLengthStructPrimitiveTests : VariableLengthExtensionTestSuite<VariableLengthStruct> {
    protected override VariableLengthStruct Value => new() { A = 42, B = true };

    protected override VariableLengthStruct[] Values => [
        new() { A = 42, B = true },
        new() { A = 0, B = false },
        new() { A = -100000, B = true }
    ];

    protected override int GetSize(VariableLengthStruct value) =>
        VariableLengthStructStructPrimitiveExtensions.GetVariableLengthStructStructSize(value);

    protected override SerializationOperations<VariableLengthStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, VariableLengthStruct value) => context.WriteVariableLengthStruct(value),
        Peek = (ReadContext context) => context.PeekVariableLengthStruct(),
        Read = (ReadContext context) => context.ReadVariableLengthStruct(),
        TryPeek = (ReadContext context, out VariableLengthStruct value) => context.TryPeekVariableLengthStruct(out value),
        TryRead = (ReadContext context, out VariableLengthStruct value) => context.TryReadVariableLengthStruct(out value),
        WriteSpan = (ref WriteContext context, Span<VariableLengthStruct> values) => context.WriteVariableLengthStructs(values),
        PeekSpan = (ReadContext context, Span<VariableLengthStruct> destination) => context.PeekVariableLengthStructs(destination),
        ReadSpan = (ReadContext context, Span<VariableLengthStruct> destination) => context.ReadVariableLengthStructs(destination),
        TryPeekSpan = (ReadContext context, Span<VariableLengthStruct> destination) => context.TryPeekVariableLengthStructs(destination),
        TryReadSpan = (ReadContext context, Span<VariableLengthStruct> destination) => context.TryReadVariableLengthStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<VariableLengthStruct> values) => context.WriteVariableLengthStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<VariableLengthStruct> destination) => context.PeekVariableLengthStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<VariableLengthStruct> destination) => context.ReadVariableLengthStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<VariableLengthStruct> destination) => context.TryPeekVariableLengthStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<VariableLengthStruct> destination) => context.TryReadVariableLengthStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<VariableLengthStruct> destination) => context.PeekVariableLengthStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<VariableLengthStruct> destination) => context.ReadVariableLengthStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<VariableLengthStruct> destination) => context.TryPeekVariableLengthStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<VariableLengthStruct> destination) => context.TryReadVariableLengthStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, VariableLengthStruct[] values) => context.WriteVariableLengthStructs(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthStructs(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthStructs(),
        TryPeekArray = (ReadContext context, out VariableLengthStruct[] values) => context.TryPeekVariableLengthStructs(out values),
        TryReadArray = (ReadContext context, out VariableLengthStruct[] values) => context.TryReadVariableLengthStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, VariableLengthStruct[] values) => context.WriteVariableLengthStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out VariableLengthStruct[] values) => context.TryPeekVariableLengthStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out VariableLengthStruct[] values) => context.TryReadVariableLengthStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out VariableLengthStruct[] values) => context.TryPeekVariableLengthStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out VariableLengthStruct[] values) => context.TryReadVariableLengthStructsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<VariableLengthStruct> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, VariableLengthStruct value) => context.WriteVariableLengthStructStructPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthStructStructPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthStructStructPrimitive(),
        WriteSpan = (ref WriteContext context, Span<VariableLengthStruct> values) => context.WriteVariableLengthStructsStructPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<VariableLengthStruct> destination) => context.PeekVariableLengthStructStructSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<VariableLengthStruct> destination) => context.ReadVariableLengthStructStructSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, VariableLengthStruct[] values) => context.WriteVariableLengthStructsStructPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthStructStructArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthStructStructArrayPrimitive(count),
    };
}
