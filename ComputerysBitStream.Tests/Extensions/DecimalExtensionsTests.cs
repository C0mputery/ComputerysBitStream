namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class DecimalExtensionsTests : ExtensionTestSuite<decimal> {
    protected override decimal Value => 1.23m;
    protected override decimal[] Values => [1.23m, 4.56m, 1.23m, 1.23m, 4.56m];

    protected override void WritePrimitive(ref WriteContext context, decimal value) => context.WriteDecimalPrimitive(value);
    protected override decimal PeekPrimitive(ReadContext context) => context.PeekDecimalPrimitive();
    protected override decimal ReadPrimitive(ReadContext context) => context.ReadDecimalPrimitive();
    protected override void Write(ref WriteContext context, decimal value) => context.WriteDecimal(value);
    protected override decimal Peek(ReadContext context) => context.PeekDecimal();
    protected override decimal Read(ReadContext context) => context.ReadDecimal();

    protected override decimal TryPeek(ReadContext context) {
        Assert.True(context.TryPeekDecimal(out decimal v));
        return v;
    }

    protected override decimal TryRead(ReadContext context) {
        Assert.True(context.TryReadDecimal(out decimal v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<decimal> values) => context.WriteDecimalsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<decimal> destination) => context.PeekDecimalSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<decimal> destination) => context.ReadDecimalSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<decimal> values) => context.WriteDecimalsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) => context.PeekDecimals(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) => context.ReadDecimals(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) { Assert.True(context.TryPeekDecimals(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<decimal> destination) { Assert.True(context.TryReadDecimals(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<decimal> values) => context.WriteDecimals(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<decimal> destination) => context.PeekDecimals(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<decimal> destination) => context.ReadDecimals(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<decimal> destination) { Assert.True(context.TryPeekDecimals(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<decimal> destination) { Assert.True(context.TryReadDecimals(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, decimal[] values) => context.WriteDecimalsPrimitive(values);
    protected override decimal[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekDecimalArrayPrimitive(count);
    protected override decimal[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadDecimalArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, decimal[] values) => context.WriteDecimalsWithoutLength(values);
    protected override decimal[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekDecimals(count);
    protected override decimal[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadDecimals(count);

    protected override decimal[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekDecimals(count, out decimal[] values));
        return values;
    }

    protected override decimal[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadDecimals(count, out decimal[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, decimal[] values) => context.WriteDecimals(values);
    protected override decimal[] PeekArrayWithLength(ReadContext context) => context.PeekDecimals();
    protected override decimal[] ReadArrayWithLength(ReadContext context) => context.ReadDecimals();

    protected override decimal[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekDecimals(out decimal[] values));
        return values;
    }

    protected override decimal[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadDecimals(out decimal[] values));
        return values;
    }

    protected override TryReadOperationSet<decimal> TryOperations => new() {
        TryPeekValue = (ReadContext c, out decimal v) => c.TryPeekDecimal(out v),
        TryReadValue = (ReadContext c, out decimal v) => c.TryReadDecimal(out v),
        TryPeekArrayWithLength = (ReadContext c, out decimal[] v) => c.TryPeekDecimals(out v),
        TryReadArrayWithLength = (ReadContext c, out decimal[] v) => c.TryReadDecimals(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out decimal[] v) => c.TryPeekDecimals(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out decimal[] v) => c.TryReadDecimals(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<decimal> d) => c.TryPeekDecimals(d),
        TryReadSpanWithLength = (ReadContext c, Span<decimal> d) => c.TryReadDecimals(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<decimal> d) => c.TryPeekDecimals(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<decimal> d) => c.TryReadDecimals(count, d),
    };
}
