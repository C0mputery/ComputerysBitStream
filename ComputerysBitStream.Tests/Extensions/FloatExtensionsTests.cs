namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class FloatExtensionsTests : ExtensionTestSuite<float> {
    protected override float Value => 1.23f;
    protected override float[] Values => [1.23f, 4.56f, 1.23f, 1.23f, 4.56f];

    protected override void WritePrimitive(ref WriteContext context, float value) => context.WriteFloatPrimitive(value);
    protected override float PeekPrimitive(ReadContext context) => context.PeekFloatPrimitive();
    protected override float ReadPrimitive(ReadContext context) => context.ReadFloatPrimitive();
    protected override void Write(ref WriteContext context, float value) => context.WriteFloat(value);
    protected override float Peek(ReadContext context) => context.PeekFloat();
    protected override float Read(ReadContext context) => context.ReadFloat();

    protected override float TryPeek(ReadContext context) {
        Assert.True(context.TryPeekFloat(out float v));
        return v;
    }

    protected override float TryRead(ReadContext context) {
        Assert.True(context.TryReadFloat(out float v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<float> values) => context.WriteFloatsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<float> destination) => context.PeekFloatSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<float> destination) => context.ReadFloatSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<float> values) => context.WriteFloatsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<float> destination) => context.PeekFloats(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<float> destination) => context.ReadFloats(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryPeekFloats(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryReadFloats(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<float> values) => context.WriteFloats(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<float> destination) => context.PeekFloats(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<float> destination) => context.ReadFloats(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<float> destination) { Assert.True(context.TryPeekFloats(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<float> destination) { Assert.True(context.TryReadFloats(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, float[] values) => context.WriteFloatsPrimitive(values);
    protected override float[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekFloatArrayPrimitive(count);
    protected override float[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadFloatArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, float[] values) => context.WriteFloatsWithoutLength(values);
    protected override float[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekFloats(count);
    protected override float[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadFloats(count);

    protected override float[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekFloats(count, out float[] values));
        return values;
    }

    protected override float[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadFloats(count, out float[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, float[] values) => context.WriteFloats(values);
    protected override float[] PeekArrayWithLength(ReadContext context) => context.PeekFloats();
    protected override float[] ReadArrayWithLength(ReadContext context) => context.ReadFloats();

    protected override float[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekFloats(out float[] values));
        return values;
    }

    protected override float[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadFloats(out float[] values));
        return values;
    }

    protected override TryReadOperationSet<float> TryOperations => new() {
        TryPeekValue = (ReadContext c, out float v) => c.TryPeekFloat(out v),
        TryReadValue = (ReadContext c, out float v) => c.TryReadFloat(out v),
        TryPeekArrayWithLength = (ReadContext c, out float[] v) => c.TryPeekFloats(out v),
        TryReadArrayWithLength = (ReadContext c, out float[] v) => c.TryReadFloats(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out float[] v) => c.TryPeekFloats(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out float[] v) => c.TryReadFloats(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<float> d) => c.TryPeekFloats(d),
        TryReadSpanWithLength = (ReadContext c, Span<float> d) => c.TryReadFloats(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<float> d) => c.TryPeekFloats(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<float> d) => c.TryReadFloats(count, d),
    };
}
