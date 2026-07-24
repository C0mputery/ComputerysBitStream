using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthIntExtensionsTests : VariableLengthExtensionTestSuite<int> {
    protected override int Value => 42;
    protected override int[] Values => [42, 0, -100000];
    protected override int GetSize(int value) => PrimitiveVariableLengthIntExtensions.GetVariableLengthIntSize(value);

    protected override SerializationOperations<int> Operations { get; } = new() {
        Write = (ref WriteContext context, int value) => context.WriteVariableLengthInt(value),
        Peek = (ReadContext context) => context.PeekVariableLengthInt(),
        Read = (ReadContext context) => context.ReadVariableLengthInt(),
        TryPeek = (ReadContext context, out int value) => context.TryPeekVariableLengthInt(out value),
        TryRead = (ReadContext context, out int value) => context.TryReadVariableLengthInt(out value),
        WriteSpan = (ref WriteContext context, Span<int> values) => context.WriteVariableLengthInts(values),
        PeekSpan = (ReadContext context, Span<int> destination) => context.PeekVariableLengthInts(destination),
        ReadSpan = (ReadContext context, Span<int> destination) => context.ReadVariableLengthInts(destination),
        TryPeekSpan = (ReadContext context, Span<int> destination) => context.TryPeekVariableLengthInts(destination),
        TryReadSpan = (ReadContext context, Span<int> destination) => context.TryReadVariableLengthInts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<int> values) => context.WriteVariableLengthIntsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.PeekVariableLengthInts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.ReadVariableLengthInts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.TryPeekVariableLengthInts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<int> destination) => context.TryReadVariableLengthInts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.PeekVariableLengthIntsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.ReadVariableLengthIntsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.TryPeekVariableLengthIntsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<int> destination) => context.TryReadVariableLengthIntsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, int[] values) => context.WriteVariableLengthInts(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthInts(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthInts(),
        TryPeekArray = (ReadContext context, out int[] values) => context.TryPeekVariableLengthInts(out values),
        TryReadArray = (ReadContext context, out int[] values) => context.TryReadVariableLengthInts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, int[] values) => context.WriteVariableLengthIntsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthInts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthInts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out int[] values) => context.TryPeekVariableLengthInts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out int[] values) => context.TryReadVariableLengthInts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthIntsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthIntsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out int[] values) => context.TryPeekVariableLengthIntsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out int[] values) => context.TryReadVariableLengthIntsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<int> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, int value) => context.WriteVariableLengthIntPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthIntPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthIntPrimitive(),
        WriteSpan = (ref WriteContext context, Span<int> values) => context.WriteVariableLengthIntsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<int> destination) => context.PeekVariableLengthIntSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<int> destination) => context.ReadVariableLengthIntSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, int[] values) => context.WriteVariableLengthIntsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthIntArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthIntArrayPrimitive(count),
    };
}
