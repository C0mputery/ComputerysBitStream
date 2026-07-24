using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthSByteExtensionsTests : VariableLengthExtensionTestSuite<sbyte> {
    protected override sbyte Value => 42;
    protected override sbyte[] Values => [42, 0, -100];
    protected override int GetSize(sbyte value) => PrimitiveVariableLengthSByteExtensions.GetVariableLengthSByteSize(value);

    protected override SerializationOperations<sbyte> Operations { get; } = new() {
        Write = (ref WriteContext context, sbyte value) => context.WriteVariableLengthSByte(value),
        Peek = (ReadContext context) => context.PeekVariableLengthSByte(),
        Read = (ReadContext context) => context.ReadVariableLengthSByte(),
        TryPeek = (ReadContext context, out sbyte value) => context.TryPeekVariableLengthSByte(out value),
        TryRead = (ReadContext context, out sbyte value) => context.TryReadVariableLengthSByte(out value),
        WriteSpan = (ref WriteContext context, Span<sbyte> values) => context.WriteVariableLengthSBytes(values),
        PeekSpan = (ReadContext context, Span<sbyte> destination) => context.PeekVariableLengthSBytes(destination),
        ReadSpan = (ReadContext context, Span<sbyte> destination) => context.ReadVariableLengthSBytes(destination),
        TryPeekSpan = (ReadContext context, Span<sbyte> destination) => context.TryPeekVariableLengthSBytes(destination),
        TryReadSpan = (ReadContext context, Span<sbyte> destination) => context.TryReadVariableLengthSBytes(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<sbyte> values) => context.WriteVariableLengthSBytesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.PeekVariableLengthSBytes(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.ReadVariableLengthSBytes(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.TryPeekVariableLengthSBytes(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.TryReadVariableLengthSBytes(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.PeekVariableLengthSBytesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.ReadVariableLengthSBytesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.TryPeekVariableLengthSBytesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.TryReadVariableLengthSBytesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, sbyte[] values) => context.WriteVariableLengthSBytes(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthSBytes(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthSBytes(),
        TryPeekArray = (ReadContext context, out sbyte[] values) => context.TryPeekVariableLengthSBytes(out values),
        TryReadArray = (ReadContext context, out sbyte[] values) => context.TryReadVariableLengthSBytes(out values),
        WriteArrayWithoutLength = (ref WriteContext context, sbyte[] values) => context.WriteVariableLengthSBytesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthSBytes(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthSBytes(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out sbyte[] values) => context.TryPeekVariableLengthSBytes(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out sbyte[] values) => context.TryReadVariableLengthSBytes(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthSBytesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthSBytesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out sbyte[] values) => context.TryPeekVariableLengthSBytesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out sbyte[] values) => context.TryReadVariableLengthSBytesWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<sbyte> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, sbyte value) => context.WriteVariableLengthSBytePrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthSBytePrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthSBytePrimitive(),
        WriteSpan = (ref WriteContext context, Span<sbyte> values) => context.WriteVariableLengthSBytesPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<sbyte> destination) => context.PeekVariableLengthSByteSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<sbyte> destination) => context.ReadVariableLengthSByteSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, sbyte[] values) => context.WriteVariableLengthSBytesPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthSByteArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthSByteArrayPrimitive(count),
    };
}
