using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class UIntExtensionsTests : PrimitiveSerializationTestSuite<uint> {
    protected override uint Value => 42u;
    protected override uint[] Values => [42u, 100u, 42u, 42u, 100u];

    protected override SerializationOperations<uint> Operations { get; } = new() {
        Write = (ref WriteContext context, uint value) => context.WriteUInt(value),
        Peek = (ReadContext context) => context.PeekUInt(),
        Read = (ReadContext context) => context.ReadUInt(),
        TryPeek = (ReadContext context, out uint value) => context.TryPeekUInt(out value),
        TryRead = (ReadContext context, out uint value) => context.TryReadUInt(out value),
        WriteSpan = (ref WriteContext context, Span<uint> values) => context.WriteUInts(values),
        PeekSpan = (ReadContext context, Span<uint> destination) => context.PeekUInts(destination),
        ReadSpan = (ReadContext context, Span<uint> destination) => context.ReadUInts(destination),
        TryPeekSpan = (ReadContext context, Span<uint> destination) => context.TryPeekUInts(destination),
        TryReadSpan = (ReadContext context, Span<uint> destination) => context.TryReadUInts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<uint> values) => context.WriteUIntsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.PeekUInts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.ReadUInts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.TryPeekUInts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.TryReadUInts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.PeekUIntsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.ReadUIntsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.TryPeekUIntsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.TryReadUIntsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, uint[] values) => context.WriteUInts(values),
        PeekArray = (ReadContext context) => context.PeekUInts(),
        ReadArray = (ReadContext context) => context.ReadUInts(),
        TryPeekArray = (ReadContext context, out uint[] values) => context.TryPeekUInts(out values),
        TryReadArray = (ReadContext context, out uint[] values) => context.TryReadUInts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, uint[] values) => context.WriteUIntsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekUInts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadUInts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out uint[] values) => context.TryPeekUInts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out uint[] values) => context.TryReadUInts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekUIntsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadUIntsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out uint[] values) => context.TryPeekUIntsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out uint[] values) => context.TryReadUIntsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<uint> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, uint value) => context.WriteUIntPrimitive(value),
        Peek = (ReadContext context) => context.PeekUIntPrimitive(),
        Read = (ReadContext context) => context.ReadUIntPrimitive(),
        WriteSpan = (ref WriteContext context, Span<uint> values) => context.WriteUIntsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<uint> destination) => context.PeekUIntSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<uint> destination) => context.ReadUIntSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, uint[] values) => context.WriteUIntsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekUIntArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadUIntArrayPrimitive(count),
    };
}
