using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthShortExtensionsTests : VariableLengthExtensionTestSuite<short> {
    protected override short Value => 42;
    protected override short[] Values => [42, 0, -1000];
    protected override int GetSize(short value) => PrimitiveVariableLengthShortExtensions.GetVariableLengthShortSize(value);

    protected override SerializationOperations<short> Operations { get; } = new() {
        Write = (ref WriteContext context, short value) => context.WriteVariableLengthShort(value),
        Peek = (ReadContext context) => context.PeekVariableLengthShort(),
        Read = (ReadContext context) => context.ReadVariableLengthShort(),
        TryPeek = (ReadContext context, out short value) => context.TryPeekVariableLengthShort(out value),
        TryRead = (ReadContext context, out short value) => context.TryReadVariableLengthShort(out value),
        WriteSpan = (ref WriteContext context, Span<short> values) => context.WriteVariableLengthShorts(values),
        PeekSpan = (ReadContext context, Span<short> destination) => context.PeekVariableLengthShorts(destination),
        ReadSpan = (ReadContext context, Span<short> destination) => context.ReadVariableLengthShorts(destination),
        TryPeekSpan = (ReadContext context, Span<short> destination) => context.TryPeekVariableLengthShorts(destination),
        TryReadSpan = (ReadContext context, Span<short> destination) => context.TryReadVariableLengthShorts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<short> values) => context.WriteVariableLengthShortsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.PeekVariableLengthShorts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.ReadVariableLengthShorts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.TryPeekVariableLengthShorts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<short> destination) => context.TryReadVariableLengthShorts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.PeekVariableLengthShortsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.ReadVariableLengthShortsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.TryPeekVariableLengthShortsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<short> destination) => context.TryReadVariableLengthShortsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, short[] values) => context.WriteVariableLengthShorts(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthShorts(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthShorts(),
        TryPeekArray = (ReadContext context, out short[] values) => context.TryPeekVariableLengthShorts(out values),
        TryReadArray = (ReadContext context, out short[] values) => context.TryReadVariableLengthShorts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, short[] values) => context.WriteVariableLengthShortsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthShorts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthShorts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out short[] values) => context.TryPeekVariableLengthShorts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out short[] values) => context.TryReadVariableLengthShorts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthShortsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthShortsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out short[] values) => context.TryPeekVariableLengthShortsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out short[] values) => context.TryReadVariableLengthShortsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<short> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, short value) => context.WriteVariableLengthShortPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthShortPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthShortPrimitive(),
        WriteSpan = (ref WriteContext context, Span<short> values) => context.WriteVariableLengthShortsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<short> destination) => context.PeekVariableLengthShortSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<short> destination) => context.ReadVariableLengthShortSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, short[] values) => context.WriteVariableLengthShortsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthShortArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthShortArrayPrimitive(count),
    };
}
