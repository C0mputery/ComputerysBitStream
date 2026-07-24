using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class UShortExtensionsTests : PrimitiveSerializationTestSuite<ushort> {
    protected override ushort Value => 42;
    protected override ushort[] Values => [42, 100, 42, 42, 100];

    protected override SerializationOperations<ushort> Operations { get; } = new() {
        Write = (ref WriteContext context, ushort value) => context.WriteUShort(value),
        Peek = (ReadContext context) => context.PeekUShort(),
        Read = (ReadContext context) => context.ReadUShort(),
        TryPeek = (ReadContext context, out ushort value) => context.TryPeekUShort(out value),
        TryRead = (ReadContext context, out ushort value) => context.TryReadUShort(out value),
        WriteSpan = (ref WriteContext context, Span<ushort> values) => context.WriteUShorts(values),
        PeekSpan = (ReadContext context, Span<ushort> destination) => context.PeekUShorts(destination),
        ReadSpan = (ReadContext context, Span<ushort> destination) => context.ReadUShorts(destination),
        TryPeekSpan = (ReadContext context, Span<ushort> destination) => context.TryPeekUShorts(destination),
        TryReadSpan = (ReadContext context, Span<ushort> destination) => context.TryReadUShorts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<ushort> values) => context.WriteUShortsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.PeekUShorts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.ReadUShorts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.TryPeekUShorts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.TryReadUShorts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.PeekUShortsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.ReadUShortsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.TryPeekUShortsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.TryReadUShortsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, ushort[] values) => context.WriteUShorts(values),
        PeekArray = (ReadContext context) => context.PeekUShorts(),
        ReadArray = (ReadContext context) => context.ReadUShorts(),
        TryPeekArray = (ReadContext context, out ushort[] values) => context.TryPeekUShorts(out values),
        TryReadArray = (ReadContext context, out ushort[] values) => context.TryReadUShorts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, ushort[] values) => context.WriteUShortsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekUShorts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadUShorts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out ushort[] values) => context.TryPeekUShorts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out ushort[] values) => context.TryReadUShorts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekUShortsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadUShortsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out ushort[] values) => context.TryPeekUShortsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out ushort[] values) => context.TryReadUShortsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<ushort> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, ushort value) => context.WriteUShortPrimitive(value),
        Peek = (ReadContext context) => context.PeekUShortPrimitive(),
        Read = (ReadContext context) => context.ReadUShortPrimitive(),
        WriteSpan = (ref WriteContext context, Span<ushort> values) => context.WriteUShortsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<ushort> destination) => context.PeekUShortSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<ushort> destination) => context.ReadUShortSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, ushort[] values) => context.WriteUShortsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekUShortArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadUShortArrayPrimitive(count),
    };
}
