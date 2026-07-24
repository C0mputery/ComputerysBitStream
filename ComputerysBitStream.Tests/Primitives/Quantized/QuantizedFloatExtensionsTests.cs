using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.Quantized;

[BitStreamPrimitiveContext]
public class QuantizedFloatExtensionsTests : QuantizedExtensionTestSuite<float> {
    private const float Min = 0f;
    private const float Max = 100f;
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override float Value => 50f;
    protected override float[] Values => [0f, 50f, 100f];

    protected override void AssertValuesEqual(float expected, float actual) {
        Assert.Equal(expected, actual, Precision);
    }

    protected override SerializationOperations<float> Operations { get; } = new() {
        Write = (ref WriteContext context, float value) => context.WriteQuantizedFloat(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedFloat(Min, Max, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedFloat(Min, Max, BitCount),
        TryPeek = (ReadContext context, out float value) => context.TryPeekQuantizedFloat(Min, Max, BitCount, out value),
        TryRead = (ReadContext context, out float value) => context.TryReadQuantizedFloat(Min, Max, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<float> values) => context.WriteQuantizedFloats(values, Min, Max, BitCount),
        PeekSpan = (ReadContext context, Span<float> destination) => context.PeekQuantizedFloats(destination, Min, Max, BitCount),
        ReadSpan = (ReadContext context, Span<float> destination) => context.ReadQuantizedFloats(destination, Min, Max, BitCount),
        TryPeekSpan = (ReadContext context, Span<float> destination) => context.TryPeekQuantizedFloats(destination, Min, Max, BitCount),
        TryReadSpan = (ReadContext context, Span<float> destination) => context.TryReadQuantizedFloats(destination, Min, Max, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<float> values) => context.WriteQuantizedFloatsWithoutLength(values, Min, Max, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.PeekQuantizedFloats(count, destination, Min, Max, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.ReadQuantizedFloats(count, destination, Min, Max, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.TryPeekQuantizedFloats(count, destination, Min, Max, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<float> destination) => context.TryReadQuantizedFloats(count, destination, Min, Max, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.PeekQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.ReadQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.TryPeekQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<float> destination) => context.TryReadQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        WriteArray = (ref WriteContext context, float[] values) => context.WriteQuantizedFloats(values, Min, Max, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedFloats(Min, Max, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedFloats(Min, Max, BitCount),
        TryPeekArray = (ReadContext context, out float[] values) => context.TryPeekQuantizedFloats(Min, Max, BitCount, out values),
        TryReadArray = (ReadContext context, out float[] values) => context.TryReadQuantizedFloats(Min, Max, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, float[] values) => context.WriteQuantizedFloatsWithoutLength(values, Min, Max, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedFloats(count, Min, Max, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedFloats(count, Min, Max, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out float[] values) => context.TryPeekQuantizedFloats(count, Min, Max, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out float[] values) => context.TryReadQuantizedFloats(count, Min, Max, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out float[] values) => context.TryPeekQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out float[] values) => context.TryReadQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<float> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, float value) => context.WriteQuantizedFloatPrimitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedFloatPrimitive(Min, Max, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedFloatPrimitive(Min, Max, BitCount),
        WriteSpan = (ref WriteContext context, Span<float> values) => context.WriteQuantizedFloatsPrimitive(values, Min, Max, BitCount),
        PeekSpan = (ReadContext context, int count, Span<float> destination) => context.PeekQuantizedFloatSpanPrimitive(count, destination, Min, Max, BitCount),
        ReadSpan = (ReadContext context, int count, Span<float> destination) => context.ReadQuantizedFloatSpanPrimitive(count, destination, Min, Max, BitCount),
        WriteArray = (ref WriteContext context, float[] values) => context.WriteQuantizedFloatsPrimitive(values, Min, Max, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedFloatArrayPrimitive(count, Min, Max, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedFloatArrayPrimitive(count, Min, Max, BitCount),
    };
}
