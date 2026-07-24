using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class FloatExtensionsTests : PrimitiveSerializationTestSuite<float> {
    protected override float Value => 1.23f;
    protected override float[] Values => [1.23f, 4.56f, 1.23f, 1.23f, 4.56f];

    protected override SerializationOperations<float> Operations { get; } = new() {
        Write = (ref WriteContext context, float value) => context.WriteFloat(value),
        Peek = (ReadContext context) => context.PeekFloat(),
        Read = (ReadContext context) => context.ReadFloat(),
        TryPeek = (ReadContext context, out float value) => context.TryPeekFloat(out value),
        TryRead = (ReadContext context, out float value) => context.TryReadFloat(out value),
        WriteSpan = (ref WriteContext context, Span<float> values) => context.WriteFloats(values),
        PeekSpan = (ReadContext context, Span<float> destination) => context.PeekFloats(destination),
        ReadSpan = (ReadContext context, Span<float> destination) => context.ReadFloats(destination),
        TryPeekSpan = (ReadContext context, Span<float> destination) => context.TryPeekFloats(destination),
        TryReadSpan = (ReadContext context, Span<float> destination) => context.TryReadFloats(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<float> values) => context.WriteFloatsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.PeekFloats(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.ReadFloats(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.TryPeekFloats(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.TryReadFloats(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.PeekFloatsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.ReadFloatsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.TryPeekFloatsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.TryReadFloatsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, float[] values) => context.WriteFloats(values),
        PeekArray = (ReadContext context) => context.PeekFloats(),
        ReadArray = (ReadContext context) => context.ReadFloats(),
        TryPeekArray = (ReadContext context, out float[] values) => context.TryPeekFloats(out values),
        TryReadArray = (ReadContext context, out float[] values) => context.TryReadFloats(out values),
        WriteArrayWithoutLength = (ref WriteContext context, float[] values) => context.WriteFloatsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekFloats(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadFloats(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out float[] values) => context.TryPeekFloats(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out float[] values) => context.TryReadFloats(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekFloatsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadFloatsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out float[] values) => context.TryPeekFloatsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out float[] values) => context.TryReadFloatsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<float> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, float value) => context.WriteFloatPrimitive(value),
        Peek = (ReadContext context) => context.PeekFloatPrimitive(),
        Read = (ReadContext context) => context.ReadFloatPrimitive(),
        WriteSpan = (ref WriteContext context, Span<float> values) => context.WriteFloatsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<float> destination) => context.PeekFloatSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<float> destination) => context.ReadFloatSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, float[] values) => context.WriteFloatsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekFloatArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadFloatArrayPrimitive(count),
    };
}
