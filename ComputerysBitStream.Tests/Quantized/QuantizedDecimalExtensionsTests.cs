namespace ComputerysBitStream.Tests.Quantized;

[BitStreamPrimitiveContext]
public class QuantizedDecimalExtensionsTests : QuantizedExtensionTestSuite<decimal> {
    private const decimal Min = 0m;
    private const decimal Max = 100m;
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override decimal Value => 50m;
    protected override decimal[] Values => [0m, 50m, 100m];

    protected override void AssertValuesEqual(decimal expected, decimal actual) {
        Assert.Equal(expected, actual, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, decimal value) => context.WriteQuantizedDecimalPrimitive(value, Min, Max, BitCount);
    protected override decimal PeekPrimitive(ReadContext context) => context.PeekQuantizedDecimalPrimitive(Min, Max, BitCount);
    protected override decimal ReadPrimitive(ReadContext context) => context.ReadQuantizedDecimalPrimitive(Min, Max, BitCount);
    protected override void Write(ref WriteContext context, decimal value) => context.WriteQuantizedDecimal(value, Min, Max, BitCount);
    protected override decimal Peek(ReadContext context) => context.PeekQuantizedDecimal(Min, Max, BitCount);
    protected override decimal Read(ReadContext context) => context.ReadQuantizedDecimal(Min, Max, BitCount);

    protected override decimal TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedDecimal(Min, Max, BitCount, out decimal v));
        return v;
    }

    protected override decimal TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedDecimal(Min, Max, BitCount, out decimal v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<decimal> values) => context.WriteQuantizedDecimalsPrimitive(values, Min, Max, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<decimal> destination) => context.PeekQuantizedDecimalSpanPrimitive(count, destination, Min, Max, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<decimal> destination) => context.ReadQuantizedDecimalSpanPrimitive(count, destination, Min, Max, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<decimal> values) => context.WriteQuantizedDecimalsWithoutLength(values, Min, Max, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) => context.PeekQuantizedDecimals(count, destination, Min, Max, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) => context.ReadQuantizedDecimals(count, destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) { Assert.True(context.TryPeekQuantizedDecimals(count, destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) { Assert.True(context.TryReadQuantizedDecimals(count, destination, Min, Max, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<decimal> values) => context.WriteQuantizedDecimals(values, Min, Max, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<decimal> destination) => context.PeekQuantizedDecimals(destination, Min, Max, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<decimal> destination) => context.ReadQuantizedDecimals(destination, Min, Max, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<decimal> destination) { Assert.True(context.TryPeekQuantizedDecimals(destination, Min, Max, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<decimal> destination) { Assert.True(context.TryReadQuantizedDecimals(destination, Min, Max, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, decimal[] values) => context.WriteQuantizedDecimalsPrimitive(values, Min, Max, BitCount);
    protected override decimal[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedDecimalArrayPrimitive(count, Min, Max, BitCount);
    protected override decimal[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedDecimalArrayPrimitive(count, Min, Max, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, decimal[] values) => context.WriteQuantizedDecimalsWithoutLength(values, Min, Max, BitCount);
    protected override decimal[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedDecimals(count, Min, Max, BitCount);
    protected override decimal[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedDecimals(count, Min, Max, BitCount);

    protected override decimal[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedDecimals(count, Min, Max, BitCount, out decimal[] values));
        return values;
    }

    protected override decimal[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedDecimals(count, Min, Max, BitCount, out decimal[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, decimal[] values) => context.WriteQuantizedDecimals(values, Min, Max, BitCount);
    protected override decimal[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedDecimals(Min, Max, BitCount);
    protected override decimal[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedDecimals(Min, Max, BitCount);

    protected override decimal[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedDecimals(Min, Max, BitCount, out decimal[] values));
        return values;
    }

    protected override decimal[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedDecimals(Min, Max, BitCount, out decimal[] values));
        return values;
    }

    protected override TryReadOperationSet<decimal> TryOperations => new() {
        TryPeekValue = (ReadContext c, out decimal v) => c.TryPeekQuantizedDecimal(Min, Max, BitCount, out v),
        TryReadValue = (ReadContext c, out decimal v) => c.TryReadQuantizedDecimal(Min, Max, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out decimal[] v) => c.TryPeekQuantizedDecimals(Min, Max, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out decimal[] v) => c.TryReadQuantizedDecimals(Min, Max, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out decimal[] v) => c.TryPeekQuantizedDecimals(count, Min, Max, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out decimal[] v) => c.TryReadQuantizedDecimals(count, Min, Max, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<decimal> d) => c.TryPeekQuantizedDecimals(d, Min, Max, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<decimal> d) => c.TryReadQuantizedDecimals(d, Min, Max, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<decimal> d) => c.TryPeekQuantizedDecimals(count, d, Min, Max, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<decimal> d) => c.TryReadQuantizedDecimals(count, d, Min, Max, BitCount),
    };
}
