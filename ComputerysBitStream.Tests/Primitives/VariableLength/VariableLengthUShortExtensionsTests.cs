using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthUShortExtensionsTests : VariableLengthExtensionTestSuite<ushort> {
    protected override ushort Value => 42;
    protected override ushort[] Values => [42, 0, 50000];
    protected override int GetSize(ushort value) => PrimitiveVariableLengthUShortExtensions.GetVariableLengthUShortSize(value);

    protected override SerializationOperations<ushort> Operations { get; } = new() {
        Write = (ref WriteContext context, ushort value) => context.WriteVariableLengthUShort(value),
        Peek = (ReadContext context) => context.PeekVariableLengthUShort(),
        Read = (ReadContext context) => context.ReadVariableLengthUShort(),
        TryPeek = (ReadContext context, out ushort value) => context.TryPeekVariableLengthUShort(out value),
        TryRead = (ReadContext context, out ushort value) => context.TryReadVariableLengthUShort(out value),
        WriteSpan = (ref WriteContext context, Span<ushort> values) => context.WriteVariableLengthUShorts(values),
        PeekSpan = (ReadContext context, Span<ushort> destination) => context.PeekVariableLengthUShorts(destination),
        ReadSpan = (ReadContext context, Span<ushort> destination) => context.ReadVariableLengthUShorts(destination),
        TryPeekSpan = (ReadContext context, Span<ushort> destination) => context.TryPeekVariableLengthUShorts(destination),
        TryReadSpan = (ReadContext context, Span<ushort> destination) => context.TryReadVariableLengthUShorts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<ushort> values) => context.WriteVariableLengthUShortsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.PeekVariableLengthUShorts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.ReadVariableLengthUShorts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.TryPeekVariableLengthUShorts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<ushort> destination) => context.TryReadVariableLengthUShorts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.PeekVariableLengthUShortsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.ReadVariableLengthUShortsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.TryPeekVariableLengthUShortsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ushort> destination) => context.TryReadVariableLengthUShortsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, ushort[] values) => context.WriteVariableLengthUShorts(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthUShorts(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthUShorts(),
        TryPeekArray = (ReadContext context, out ushort[] values) => context.TryPeekVariableLengthUShorts(out values),
        TryReadArray = (ReadContext context, out ushort[] values) => context.TryReadVariableLengthUShorts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, ushort[] values) => context.WriteVariableLengthUShortsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthUShorts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthUShorts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out ushort[] values) => context.TryPeekVariableLengthUShorts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out ushort[] values) => context.TryReadVariableLengthUShorts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthUShortsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthUShortsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out ushort[] values) => context.TryPeekVariableLengthUShortsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out ushort[] values) => context.TryReadVariableLengthUShortsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<ushort> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, ushort value) => context.WriteVariableLengthUShortPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthUShortPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthUShortPrimitive(),
        WriteSpan = (ref WriteContext context, Span<ushort> values) => context.WriteVariableLengthUShortsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<ushort> destination) => context.PeekVariableLengthUShortSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<ushort> destination) => context.ReadVariableLengthUShortSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, ushort[] values) => context.WriteVariableLengthUShortsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthUShortArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthUShortArrayPrimitive(count),
    };
}
