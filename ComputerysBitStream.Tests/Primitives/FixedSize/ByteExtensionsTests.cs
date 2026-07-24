using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class ByteExtensionsTests : PrimitiveSerializationTestSuite<byte> {
    protected override byte Value => 42;
    protected override byte[] Values => [42, 100, 42, 42, 100];

    protected override SerializationOperations<byte> Operations { get; } = new() {
        Write = (ref WriteContext context, byte value) => context.WriteByte(value),
        Peek = (ReadContext context) => context.PeekByte(),
        Read = (ReadContext context) => context.ReadByte(),
        TryPeek = (ReadContext context, out byte value) => context.TryPeekByte(out value),
        TryRead = (ReadContext context, out byte value) => context.TryReadByte(out value),
        WriteSpan = (ref WriteContext context, Span<byte> values) => context.WriteBytes(values),
        PeekSpan = (ReadContext context, Span<byte> destination) => context.PeekBytes(destination),
        ReadSpan = (ReadContext context, Span<byte> destination) => context.ReadBytes(destination),
        TryPeekSpan = (ReadContext context, Span<byte> destination) => context.TryPeekBytes(destination),
        TryReadSpan = (ReadContext context, Span<byte> destination) => context.TryReadBytes(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<byte> values) => context.WriteBytesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.PeekBytes(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.ReadBytes(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.TryPeekBytes(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<byte> destination) => context.TryReadBytes(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.PeekBytesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.ReadBytesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.TryPeekBytesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<byte> destination) => context.TryReadBytesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, byte[] values) => context.WriteBytes(values),
        PeekArray = (ReadContext context) => context.PeekBytes(),
        ReadArray = (ReadContext context) => context.ReadBytes(),
        TryPeekArray = (ReadContext context, out byte[] values) => context.TryPeekBytes(out values),
        TryReadArray = (ReadContext context, out byte[] values) => context.TryReadBytes(out values),
        WriteArrayWithoutLength = (ref WriteContext context, byte[] values) => context.WriteBytesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekBytes(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadBytes(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out byte[] values) => context.TryPeekBytes(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out byte[] values) => context.TryReadBytes(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekBytesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadBytesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out byte[] values) => context.TryPeekBytesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out byte[] values) => context.TryReadBytesWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<byte> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, byte value) => context.WriteBytePrimitive(value),
        Peek = (ReadContext context) => context.PeekBytePrimitive(),
        Read = (ReadContext context) => context.ReadBytePrimitive(),
        WriteSpan = (ref WriteContext context, Span<byte> values) => context.WriteBytesPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<byte> destination) => context.PeekByteSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<byte> destination) => context.ReadByteSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, byte[] values) => context.WriteBytesPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekByteArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadByteArrayPrimitive(count),
    };
}
