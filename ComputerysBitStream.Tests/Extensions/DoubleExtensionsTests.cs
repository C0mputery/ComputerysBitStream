namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class DoubleExtensionsTests : PrimitiveSerializationTestSuite<double> {
    protected override double Value => 1.23;
    protected override double[] Values => [1.23, 4.56, 1.23, 1.23, 4.56];

    protected override void WritePrimitive(ref WriteContext context, double value) => context.WriteDoublePrimitive(value);
    protected override double PeekPrimitive(ReadContext context) => context.PeekDoublePrimitive();
    protected override double ReadPrimitive(ReadContext context) => context.ReadDoublePrimitive();
    protected override void Write(ref WriteContext context, double value) => context.WriteDouble(value);
    protected override double Peek(ReadContext context) => context.PeekDouble();
    protected override double Read(ReadContext context) => context.ReadDouble();

    protected override double TryPeek(ReadContext context) {
        Assert.True(context.TryPeekDouble(out double v));
        return v;
    }

    protected override double TryRead(ReadContext context) {
        Assert.True(context.TryReadDouble(out double v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<double> values) => context.WriteDoublesPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<double> destination) => context.PeekDoubleSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<double> destination) => context.ReadDoubleSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<double> values) => context.WriteDoublesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<double> destination) => context.PeekDoubles(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<double> destination) => context.ReadDoubles(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<double> destination) { Assert.True(context.TryPeekDoubles(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<double> destination) { Assert.True(context.TryReadDoubles(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<double> values) => context.WriteDoubles(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<double> destination) => context.PeekDoubles(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<double> destination) => context.ReadDoubles(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<double> destination) { Assert.True(context.TryPeekDoubles(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<double> destination) { Assert.True(context.TryReadDoubles(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, double[] values) => context.WriteDoublesPrimitive(values);
    protected override double[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekDoubleArrayPrimitive(count);
    protected override double[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadDoubleArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, double[] values) => context.WriteDoublesWithoutLength(values);
    protected override double[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekDoubles(count);
    protected override double[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadDoubles(count);

    protected override double[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekDoubles(count, out double[] values));
        return values;
    }

    protected override double[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadDoubles(count, out double[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, double[] values) => context.WriteDoubles(values);
    protected override double[] PeekArrayWithLength(ReadContext context) => context.PeekDoubles();
    protected override double[] ReadArrayWithLength(ReadContext context) => context.ReadDoubles();

    protected override double[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekDoubles(out double[] values));
        return values;
    }

    protected override double[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadDoubles(out double[] values));
        return values;
    }

    protected override double[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekDoublesWithMaxCount(maxCount);
    protected override double[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadDoublesWithMaxCount(maxCount);

    protected override double[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekDoublesWithMaxCount(maxCount, out double[] values));
        return values;
    }

    protected override double[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadDoublesWithMaxCount(maxCount, out double[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<double> destination) => context.PeekDoublesWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<double> destination) => context.ReadDoublesWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<double> destination) { Assert.True(context.TryPeekDoublesWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<double> destination) { Assert.True(context.TryReadDoublesWithMaxCount(maxCount, destination)); }

    protected override TryReadOperationSet<double> TryOperations => new() {
        TryPeekValue = (ReadContext c, out double v) => c.TryPeekDouble(out v),
        TryReadValue = (ReadContext c, out double v) => c.TryReadDouble(out v),
        TryPeekArrayWithLength = (ReadContext c, out double[] v) => c.TryPeekDoubles(out v),
        TryReadArrayWithLength = (ReadContext c, out double[] v) => c.TryReadDoubles(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out double[] v) => c.TryPeekDoubles(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out double[] v) => c.TryReadDoubles(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<double> d) => c.TryPeekDoubles(d),
        TryReadSpanWithLength = (ReadContext c, Span<double> d) => c.TryReadDoubles(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<double> d) => c.TryPeekDoubles(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<double> d) => c.TryReadDoubles(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out double[] v) => c.TryPeekDoublesWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out double[] v) => c.TryReadDoublesWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<double> d) => c.TryPeekDoublesWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<double> d) => c.TryReadDoublesWithMaxCount(maxCount, d),
    };
}
