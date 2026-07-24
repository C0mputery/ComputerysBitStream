using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthUIntExtensionsTests : VariableLengthExtensionTestSuite<uint> {
    protected override uint Value => 42u;
    protected override uint[] Values => [42u, 0u, 100000u];
    protected override int GetSize(uint value) => PrimitiveVariableLengthUIntExtensions.GetVariableLengthUIntSize(value);

    protected override SerializationOperations<uint> Operations { get; } = new() {
        Write = (ref WriteContext context, uint value) => context.WriteVariableLengthUInt(value),
        Peek = (ReadContext context) => context.PeekVariableLengthUInt(),
        Read = (ReadContext context) => context.ReadVariableLengthUInt(),
        TryPeek = (ReadContext context, out uint value) => context.TryPeekVariableLengthUInt(out value),
        TryRead = (ReadContext context, out uint value) => context.TryReadVariableLengthUInt(out value),
        WriteSpan = (ref WriteContext context, Span<uint> values) => context.WriteVariableLengthUInts(values),
        PeekSpan = (ReadContext context, Span<uint> destination) => context.PeekVariableLengthUInts(destination),
        ReadSpan = (ReadContext context, Span<uint> destination) => context.ReadVariableLengthUInts(destination),
        TryPeekSpan = (ReadContext context, Span<uint> destination) => context.TryPeekVariableLengthUInts(destination),
        TryReadSpan = (ReadContext context, Span<uint> destination) => context.TryReadVariableLengthUInts(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<uint> values) => context.WriteVariableLengthUIntsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.PeekVariableLengthUInts(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.ReadVariableLengthUInts(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.TryPeekVariableLengthUInts(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<uint> destination) => context.TryReadVariableLengthUInts(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.PeekVariableLengthUIntsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.ReadVariableLengthUIntsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.TryPeekVariableLengthUIntsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<uint> destination) => context.TryReadVariableLengthUIntsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, uint[] values) => context.WriteVariableLengthUInts(values),
        PeekArray = (ReadContext context) => context.PeekVariableLengthUInts(),
        ReadArray = (ReadContext context) => context.ReadVariableLengthUInts(),
        TryPeekArray = (ReadContext context, out uint[] values) => context.TryPeekVariableLengthUInts(out values),
        TryReadArray = (ReadContext context, out uint[] values) => context.TryReadVariableLengthUInts(out values),
        WriteArrayWithoutLength = (ref WriteContext context, uint[] values) => context.WriteVariableLengthUIntsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVariableLengthUInts(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVariableLengthUInts(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out uint[] values) => context.TryPeekVariableLengthUInts(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out uint[] values) => context.TryReadVariableLengthUInts(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVariableLengthUIntsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVariableLengthUIntsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out uint[] values) => context.TryPeekVariableLengthUIntsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out uint[] values) => context.TryReadVariableLengthUIntsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<uint> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, uint value) => context.WriteVariableLengthUIntPrimitive(value),
        Peek = (ReadContext context) => context.PeekVariableLengthUIntPrimitive(),
        Read = (ReadContext context) => context.ReadVariableLengthUIntPrimitive(),
        WriteSpan = (ref WriteContext context, Span<uint> values) => context.WriteVariableLengthUIntsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<uint> destination) => context.PeekVariableLengthUIntSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<uint> destination) => context.ReadVariableLengthUIntSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, uint[] values) => context.WriteVariableLengthUIntsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekVariableLengthUIntArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadVariableLengthUIntArrayPrimitive(count),
    };
}
