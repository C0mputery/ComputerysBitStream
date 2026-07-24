using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthByteExtensionsTests : VariableLengthExtensionTestSuite<byte> {
    protected override byte Value => 42;
    protected override byte[] Values => [42, 0, 200];
    protected override int GetSize(byte value) => PrimitiveVariableLengthByteExtensions.GetVariableLengthByteSize(value);

    protected override SerializationOperations<byte> Operations { get; } = new() {
        Write = (ref WriteContext context, byte value) => context.WriteVariableLengthByte(value),
        Peek = (ReadContext context) => context.PeekVariableLengthByte(),
        Read = (ReadContext context) => context.ReadVariableLengthByte(),
        TryPeek = (ReadContext context, out byte value) => context.TryPeekVariableLengthByte(out value),
        TryRead = (ReadContext context, out byte value) => context.TryReadVariableLengthByte(out value),
        WriteSpan = (ref WriteContext context, Span<byte> values) => context.WriteVariableLengthBytes(values),
        PeekSpan = (ReadContext context, Span<byte> destination) => context.PeekVariableLengthBytes(destination),
        ReadSpan = (ReadContext context, Span<byte> destination) => context.ReadVariableLengthBytes(destination),
        TryPeekSpan = (ReadContext context, Span<byte> destination) => context.TryPeekVariableLengthBytes(destination),
        TryReadSpan = (ReadContext context, Span<byte> destination) => context.TryReadVariableLengthBytes(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<byte> values) => context.WriteVariableLengthBytesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.PeekVariableLengthBytes(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.ReadVariableLengthBytes(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.TryPeekVariableLengthBytes(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.TryReadVariableLengthBytes(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.PeekVariableLengthBytesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.ReadVariableLengthBytesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.TryPeekVariableLengthBytesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.TryReadVariableLengthBytesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, byte[] values) => context.WriteVariableLengthBytes(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthBytes(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthBytes(),
        TryPeekArray = (ReadContext context, out byte[] values) => context.TryPeekVariableLengthBytes(out values),
        TryReadArray = (ReadContext context, out byte[] values) => context.TryReadVariableLengthBytes(out values),
        WriteArrayWithoutLength = (ref WriteContext context, byte[] values) => context.WriteVariableLengthBytesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthBytes(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthBytes(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out byte[] values) => context.TryPeekVariableLengthBytes(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out byte[] values) => context.TryReadVariableLengthBytes(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthBytesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthBytesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out byte[] values) => context.TryPeekVariableLengthBytesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out byte[] values) => context.TryReadVariableLengthBytesWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<byte> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, byte value) => context.WriteVariableLengthBytePrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthBytePrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthBytePrimitive(),
        WriteSpan = (ref WriteContext context, Span<byte> values) => context.WriteVariableLengthBytesPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<byte> destination) => context.PeekVariableLengthByteSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<byte> destination) => context.ReadVariableLengthByteSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, byte[] values) => context.WriteVariableLengthBytesPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthByteArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthByteArrayPrimitive(count),
    };
}
