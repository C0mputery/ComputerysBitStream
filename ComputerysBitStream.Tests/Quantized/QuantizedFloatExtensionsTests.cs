namespace ComputerysBitStream.Tests.Quantized;

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

    protected override void WritePrimitive(ref WriteContext context, float value) => context.WriteQuantizedFloatPrimitive(value, Min, Max, BitCount);
    protected override float PeekPrimitive(ReadContext context) => context.PeekQuantizedFloatPrimitive(Min, Max, BitCount);
    protected override float ReadPrimitive(ReadContext context) => context.ReadQuantizedFloatPrimitive(Min, Max, BitCount);
    protected override void Write(ref WriteContext context, float value) => context.WriteQuantizedFloat(value, Min, Max, BitCount);
    protected override float Peek(ReadContext context) => context.PeekQuantizedFloat(Min, Max, BitCount);
    protected override float Read(ReadContext context) => context.ReadQuantizedFloat(Min, Max, BitCount);

    protected override float TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedFloat(Min, Max, BitCount, out float v));
        return v;
    }

    protected override float TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedFloat(Min, Max, BitCount, out float v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<float> values) => context.WriteQuantizedFloatsPrimitive(values, Min, Max, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<float> destination) => context.PeekQuantizedFloatSpanPrimitive(count, destination, Min, Max, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<float> destination) => context.ReadQuantizedFloatSpanPrimitive(count, destination, Min, Max, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<float> values) => context.WriteQuantizedFloatsWithoutLength(values, Min, Max, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<float> destination) => context.PeekQuantizedFloats(count, destination, Min, Max, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<float> destination) => context.ReadQuantizedFloats(count, destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryPeekQuantizedFloats(count, destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryReadQuantizedFloats(count, destination, Min, Max, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<float> values) => context.WriteQuantizedFloats(values, Min, Max, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<float> destination) => context.PeekQuantizedFloats(destination, Min, Max, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<float> destination) => context.ReadQuantizedFloats(destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<float> destination) { Assert.True(context.TryPeekQuantizedFloats(destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<float> destination) { Assert.True(context.TryReadQuantizedFloats(destination, Min, Max, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, float[] values) => context.WriteQuantizedFloatsPrimitive(values, Min, Max, BitCount);
    protected override float[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedFloatArrayPrimitive(count, Min, Max, BitCount);
    protected override float[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedFloatArrayPrimitive(count, Min, Max, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, float[] values) => context.WriteQuantizedFloatsWithoutLength(values, Min, Max, BitCount);
    protected override float[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedFloats(count, Min, Max, BitCount);
    protected override float[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedFloats(count, Min, Max, BitCount);

    protected override float[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedFloats(count, Min, Max, BitCount, out float[] values));
        return values;
    }

    protected override float[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedFloats(count, Min, Max, BitCount, out float[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, float[] values) => context.WriteQuantizedFloats(values, Min, Max, BitCount);
    protected override float[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedFloats(Min, Max, BitCount);
    protected override float[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedFloats(Min, Max, BitCount);

    protected override float[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedFloats(Min, Max, BitCount, out float[] values));
        return values;
    }

    protected override float[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedFloats(Min, Max, BitCount, out float[] values));
        return values;
    }

    protected override float[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount);
    protected override float[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount);

    protected override float[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount, out float[] values));
        return values;
    }

    protected override float[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount, out float[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<float> destination) => context.PeekQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<float> destination) => context.ReadQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<float> destination) { Assert.True(context.TryPeekQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<float> destination) { Assert.True(context.TryReadQuantizedFloatsWithMaxCount(maxCount, destination, Min, Max, BitCount)); }

    protected override TryReadOperationSet<float> TryOperations => new() {
        TryPeekValue = (ReadContext c, out float v) => c.TryPeekQuantizedFloat(Min, Max, BitCount, out v),
        TryReadValue = (ReadContext c, out float v) => c.TryReadQuantizedFloat(Min, Max, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out float[] v) => c.TryPeekQuantizedFloats(Min, Max, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out float[] v) => c.TryReadQuantizedFloats(Min, Max, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out float[] v) => c.TryPeekQuantizedFloats(count, Min, Max, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out float[] v) => c.TryReadQuantizedFloats(count, Min, Max, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<float> d) => c.TryPeekQuantizedFloats(d, Min, Max, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<float> d) => c.TryReadQuantizedFloats(d, Min, Max, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<float> d) => c.TryPeekQuantizedFloats(count, d, Min, Max, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<float> d) => c.TryReadQuantizedFloats(count, d, Min, Max, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out float[] v) => c.TryPeekQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out float[] v) => c.TryReadQuantizedFloatsWithMaxCount(maxCount, Min, Max, BitCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<float> d) => c.TryPeekQuantizedFloatsWithMaxCount(maxCount, d, Min, Max, BitCount),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<float> d) => c.TryReadQuantizedFloatsWithMaxCount(maxCount, d, Min, Max, BitCount),
    };
}
