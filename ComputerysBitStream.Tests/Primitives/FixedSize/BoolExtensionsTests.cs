using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class BoolExtensionsTests : PrimitiveSerializationTestSuite<bool> {
    protected override bool Value => true;
    protected override bool[] Values => [true, false, true, true, false];

    protected override SerializationOperations<bool> Operations { get; } = new() {
        Write = (ref WriteContext context, bool value) => context.WriteBool(value),
        Peek = (ReadContext context) => context.PeekBool(),
        Read = (ReadContext context) => context.ReadBool(),
        TryPeek = (ReadContext context, out bool value) => context.TryPeekBool(out value),
        TryRead = (ReadContext context, out bool value) => context.TryReadBool(out value),
        WriteSpan = (ref WriteContext context, Span<bool> values) => context.WriteBools(values),
        PeekSpan = (ReadContext context, Span<bool> destination) => context.PeekBools(destination),
        ReadSpan = (ReadContext context, Span<bool> destination) => context.ReadBools(destination),
        TryPeekSpan = (ReadContext context, Span<bool> destination) => context.TryPeekBools(destination),
        TryReadSpan = (ReadContext context, Span<bool> destination) => context.TryReadBools(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<bool> values) => context.WriteBoolsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<bool> destination) => context.PeekBools(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<bool> destination) => context.ReadBools(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<bool> destination) => context.TryPeekBools(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<bool> destination) => context.TryReadBools(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<bool> destination) => context.PeekBoolsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<bool> destination) => context.ReadBoolsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<bool> destination) => context.TryPeekBoolsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<bool> destination) => context.TryReadBoolsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, bool[] values) => context.WriteBools(values),
        PeekArray = (ReadContext context) => context.PeekBools(),
        ReadArray = (ReadContext context) => context.ReadBools(),
        TryPeekArray = (ReadContext context, out bool[] values) => context.TryPeekBools(out values),
        TryReadArray = (ReadContext context, out bool[] values) => context.TryReadBools(out values),
        WriteArrayWithoutLength = (ref WriteContext context, bool[] values) => context.WriteBoolsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekBools(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadBools(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out bool[] values) => context.TryPeekBools(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out bool[] values) => context.TryReadBools(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekBoolsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadBoolsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out bool[] values) => context.TryPeekBoolsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out bool[] values) => context.TryReadBoolsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<bool> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, bool value) => context.WriteBoolPrimitive(value),
        Peek = (ReadContext context) => context.PeekBoolPrimitive(),
        Read = (ReadContext context) => context.ReadBoolPrimitive(),
        WriteSpan = (ref WriteContext context, Span<bool> values) => context.WriteBoolsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<bool> destination) => context.PeekBoolSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<bool> destination) => context.ReadBoolSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, bool[] values) => context.WriteBoolsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekBoolArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadBoolArrayPrimitive(count),
    };
}
