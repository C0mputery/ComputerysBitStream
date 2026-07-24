using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.Quantized;

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

    protected override SerializationOperations<decimal> Operations { get; } = new() {
        Write = (ref WriteContext context, decimal value) => context.WriteQuantizedDecimal(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedDecimal(Min, Max, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedDecimal(Min, Max, BitCount),
        TryPeek = (ReadContext context, out decimal value) => context.TryPeekQuantizedDecimal(Min, Max, BitCount, out value),
        TryRead = (ReadContext context, out decimal value) => context.TryReadQuantizedDecimal(Min, Max, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<decimal> values) => context.WriteQuantizedDecimals(values, Min, Max, BitCount),
        PeekSpan = (ReadContext context, Span<decimal> destination) => context.PeekQuantizedDecimals(destination, Min, Max, BitCount),
        ReadSpan = (ReadContext context, Span<decimal> destination) => context.ReadQuantizedDecimals(destination, Min, Max, BitCount),
        TryPeekSpan = (ReadContext context, Span<decimal> destination) => context.TryPeekQuantizedDecimals(destination, Min, Max, BitCount),
        TryReadSpan = (ReadContext context, Span<decimal> destination) => context.TryReadQuantizedDecimals(destination, Min, Max, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<decimal> values) => context.WriteQuantizedDecimalsWithoutLength(values, Min, Max, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.PeekQuantizedDecimals(count, destination, Min, Max, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.ReadQuantizedDecimals(count, destination, Min, Max, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.TryPeekQuantizedDecimals(count, destination, Min, Max, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.TryReadQuantizedDecimals(count, destination, Min, Max, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.PeekQuantizedDecimalsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.ReadQuantizedDecimalsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.TryPeekQuantizedDecimalsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.TryReadQuantizedDecimalsWithMaxCount(maxCount, destination, Min, Max, BitCount),
        WriteArray = (ref WriteContext context, decimal[] values) => context.WriteQuantizedDecimals(values, Min, Max, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedDecimals(Min, Max, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedDecimals(Min, Max, BitCount),
        TryPeekArray = (ReadContext context, out decimal[] values) => context.TryPeekQuantizedDecimals(Min, Max, BitCount, out values),
        TryReadArray = (ReadContext context, out decimal[] values) => context.TryReadQuantizedDecimals(Min, Max, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, decimal[] values) => context.WriteQuantizedDecimalsWithoutLength(values, Min, Max, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedDecimals(count, Min, Max, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedDecimals(count, Min, Max, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out decimal[] values) => context.TryPeekQuantizedDecimals(count, Min, Max, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out decimal[] values) => context.TryReadQuantizedDecimals(count, Min, Max, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedDecimalsWithMaxCount(maxCount, Min, Max, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedDecimalsWithMaxCount(maxCount, Min, Max, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out decimal[] values) => context.TryPeekQuantizedDecimalsWithMaxCount(maxCount, Min, Max, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out decimal[] values) => context.TryReadQuantizedDecimalsWithMaxCount(maxCount, Min, Max, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<decimal> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, decimal value) => context.WriteQuantizedDecimalPrimitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedDecimalPrimitive(Min, Max, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedDecimalPrimitive(Min, Max, BitCount),
        WriteSpan = (ref WriteContext context, Span<decimal> values) => context.WriteQuantizedDecimalsPrimitive(values, Min, Max, BitCount),
        PeekSpan = (ReadContext context, int count, Span<decimal> destination) => context.PeekQuantizedDecimalSpanPrimitive(count, destination, Min, Max, BitCount),
        ReadSpan = (ReadContext context, int count, Span<decimal> destination) => context.ReadQuantizedDecimalSpanPrimitive(count, destination, Min, Max, BitCount),
        WriteArray = (ref WriteContext context, decimal[] values) => context.WriteQuantizedDecimalsPrimitive(values, Min, Max, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedDecimalArrayPrimitive(count, Min, Max, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedDecimalArrayPrimitive(count, Min, Max, BitCount),
    };
}
