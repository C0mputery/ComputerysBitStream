using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class SByteExtensionsTests : PrimitiveSerializationTestSuite<sbyte> {
    protected override sbyte Value => 42;
    protected override sbyte[] Values => [42, -42, 42, 42, -42];

    protected override SerializationOperations<sbyte> Operations { get; } = new() {
        Write = (ref WriteContext context, sbyte value) => context.WriteSByte(value),
        Peek = (ReadContext context) => context.PeekSByte(),
        Read = (ReadContext context) => context.ReadSByte(),
        TryPeek = (ReadContext context, out sbyte value) => context.TryPeekSByte(out value),
        TryRead = (ReadContext context, out sbyte value) => context.TryReadSByte(out value),
        WriteSpan = (ref WriteContext context, Span<sbyte> values) => context.WriteSBytes(values),
        PeekSpan = (ReadContext context, Span<sbyte> destination) => context.PeekSBytes(destination),
        ReadSpan = (ReadContext context, Span<sbyte> destination) => context.ReadSBytes(destination),
        TryPeekSpan = (ReadContext context, Span<sbyte> destination) => context.TryPeekSBytes(destination),
        TryReadSpan = (ReadContext context, Span<sbyte> destination) => context.TryReadSBytes(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<sbyte> values) => context.WriteSBytesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.PeekSBytes(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.ReadSBytes(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.TryPeekSBytes(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<sbyte> destination) => context.TryReadSBytes(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.PeekSBytesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.ReadSBytesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.TryPeekSBytesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<sbyte> destination) => context.TryReadSBytesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, sbyte[] values) => context.WriteSBytes(values),
        PeekArray = (ReadContext context) => context.PeekSBytes(),
        ReadArray = (ReadContext context) => context.ReadSBytes(),
        TryPeekArray = (ReadContext context, out sbyte[] values) => context.TryPeekSBytes(out values),
        TryReadArray = (ReadContext context, out sbyte[] values) => context.TryReadSBytes(out values),
        WriteArrayWithoutLength = (ref WriteContext context, sbyte[] values) => context.WriteSBytesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekSBytes(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadSBytes(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out sbyte[] values) => context.TryPeekSBytes(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out sbyte[] values) => context.TryReadSBytes(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekSBytesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadSBytesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out sbyte[] values) => context.TryPeekSBytesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out sbyte[] values) => context.TryReadSBytesWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<sbyte> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, sbyte value) => context.WriteSBytePrimitive(value),
        Peek = (ReadContext context) => context.PeekSBytePrimitive(),
        Read = (ReadContext context) => context.ReadSBytePrimitive(),
        WriteSpan = (ref WriteContext context, Span<sbyte> values) => context.WriteSBytesPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<sbyte> destination) => context.PeekSByteSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<sbyte> destination) => context.ReadSByteSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, sbyte[] values) => context.WriteSBytesPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekSByteArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadSByteArrayPrimitive(count),
    };
}
