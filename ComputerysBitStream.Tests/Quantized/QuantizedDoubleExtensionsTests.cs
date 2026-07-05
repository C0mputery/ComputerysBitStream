namespace ComputerysBitStream.Tests.Quantized;

[BitStreamPrimitiveContext]
public class QuantizedDoubleExtensionsTests : QuantizedExtensionTestSuite<double> {
    private const double Min = 0d;
    private const double Max = 100d;
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override double Value => 50d;
    protected override double[] Values => [0d, 50d, 100d];

    protected override void AssertValuesEqual(double expected, double actual) {
        Assert.Equal(expected, actual, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, double value) => context.WriteQuantizedDoublePrimitive(value, Min, Max, BitCount);
    protected override double PeekPrimitive(ReadContext context) => context.PeekQuantizedDoublePrimitive(Min, Max, BitCount);
    protected override double ReadPrimitive(ReadContext context) => context.ReadQuantizedDoublePrimitive(Min, Max, BitCount);
    protected override void Write(ref WriteContext context, double value) => context.WriteQuantizedDouble(value, Min, Max, BitCount);
    protected override double Peek(ReadContext context) => context.PeekQuantizedDouble(Min, Max, BitCount);
    protected override double Read(ReadContext context) => context.ReadQuantizedDouble(Min, Max, BitCount);
    protected override double TryPeek(ReadContext context) { Assert.True(context.TryPeekQuantizedDouble(Min, Max, BitCount, out double v)); return v; }
    protected override double TryRead(ReadContext context) { Assert.True(context.TryReadQuantizedDouble(Min, Max, BitCount, out double v)); return v; }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<double> values) => context.WriteQuantizedDoublesPrimitive(values, Min, Max, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<double> destination) => context.PeekQuantizedDoubleSpanPrimitive(count, destination, Min, Max, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<double> destination) => context.ReadQuantizedDoubleSpanPrimitive(count, destination, Min, Max, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<double> values) => context.WriteQuantizedDoublesWithoutLength(values, Min, Max, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<double> destination) => context.PeekQuantizedDoubles(count, destination, Min, Max, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<double> destination) => context.ReadQuantizedDoubles(count, destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<double> destination) { Assert.True(context.TryPeekQuantizedDoubles(count, destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<double> destination) { Assert.True(context.TryReadQuantizedDoubles(count, destination, Min, Max, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<double> values) => context.WriteQuantizedDoubles(values, Min, Max, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<double> destination) => context.PeekQuantizedDoubles(destination, Min, Max, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<double> destination) => context.ReadQuantizedDoubles(destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<double> destination) { Assert.True(context.TryPeekQuantizedDoubles(destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<double> destination) { Assert.True(context.TryReadQuantizedDoubles(destination, Min, Max, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, double[] values) => context.WriteQuantizedDoublesPrimitive(values, Min, Max, BitCount);
    protected override double[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedDoubleArrayPrimitive(count, Min, Max, BitCount);
    protected override double[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedDoubleArrayPrimitive(count, Min, Max, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, double[] values) => context.WriteQuantizedDoublesWithoutLength(values, Min, Max, BitCount);
    protected override double[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedDoubles(count, Min, Max, BitCount);
    protected override double[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedDoubles(count, Min, Max, BitCount);
    protected override double[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekQuantizedDoubles(count, Min, Max, BitCount, out double[] values)); return values; }
    protected override double[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadQuantizedDoubles(count, Min, Max, BitCount, out double[] values)); return values; }

    protected override void WriteArray(ref WriteContext context, double[] values) => context.WriteQuantizedDoubles(values, Min, Max, BitCount);
    protected override double[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedDoubles(Min, Max, BitCount);
    protected override double[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedDoubles(Min, Max, BitCount);
    protected override double[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekQuantizedDoubles(Min, Max, BitCount, out double[] values)); return values; }
    protected override double[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadQuantizedDoubles(Min, Max, BitCount, out double[] values)); return values; }
    protected override TryReadOperationSet<double> TryOperations => new() {
        TryPeekValue = (ReadContext c, out double v) => c.TryPeekQuantizedDouble(Min, Max, BitCount, out v),
        TryReadValue = (ReadContext c, out double v) => c.TryReadQuantizedDouble(Min, Max, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out double[] v) => c.TryPeekQuantizedDoubles(Min, Max, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out double[] v) => c.TryReadQuantizedDoubles(Min, Max, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out double[] v) => c.TryPeekQuantizedDoubles(count, Min, Max, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out double[] v) => c.TryReadQuantizedDoubles(count, Min, Max, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<double> d) => c.TryPeekQuantizedDoubles(d, Min, Max, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<double> d) => c.TryReadQuantizedDoubles(d, Min, Max, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<double> d) => c.TryPeekQuantizedDoubles(count, d, Min, Max, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<double> d) => c.TryReadQuantizedDoubles(count, d, Min, Max, BitCount),
    };
}
