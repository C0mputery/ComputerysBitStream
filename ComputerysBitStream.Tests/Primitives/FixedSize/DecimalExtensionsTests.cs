using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class DecimalExtensionsTests : PrimitiveSerializationTestSuite<decimal> {
    protected override decimal Value => 1.23m;
    protected override decimal[] Values => [1.23m, 4.56m, 1.23m, 1.23m, 4.56m];

    protected override SerializationOperations<decimal> Operations { get; } = new() {
        Write = (ref WriteContext context, decimal value) => context.WriteDecimal(value),
        Peek = (ReadContext context) => context.PeekDecimal(),
        Read = (ReadContext context) => context.ReadDecimal(),
        TryPeek = (ReadContext context, out decimal value) => context.TryPeekDecimal(out value),
        TryRead = (ReadContext context, out decimal value) => context.TryReadDecimal(out value),
        WriteSpan = (ref WriteContext context, Span<decimal> values) => context.WriteDecimals(values),
        PeekSpan = (ReadContext context, Span<decimal> destination) => context.PeekDecimals(destination),
        ReadSpan = (ReadContext context, Span<decimal> destination) => context.ReadDecimals(destination),
        TryPeekSpan = (ReadContext context, Span<decimal> destination) => context.TryPeekDecimals(destination),
        TryReadSpan = (ReadContext context, Span<decimal> destination) => context.TryReadDecimals(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<decimal> values) => context.WriteDecimalsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.PeekDecimals(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.ReadDecimals(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.TryPeekDecimals(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<decimal> destination) => context.TryReadDecimals(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.PeekDecimalsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.ReadDecimalsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.TryPeekDecimalsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<decimal> destination) => context.TryReadDecimalsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, decimal[] values) => context.WriteDecimals(values),
        PeekArray = (ReadContext context) => context.PeekDecimals(),
        ReadArray = (ReadContext context) => context.ReadDecimals(),
        TryPeekArray = (ReadContext context, out decimal[] values) => context.TryPeekDecimals(out values),
        TryReadArray = (ReadContext context, out decimal[] values) => context.TryReadDecimals(out values),
        WriteArrayWithoutLength = (ref WriteContext context, decimal[] values) => context.WriteDecimalsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekDecimals(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadDecimals(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out decimal[] values) => context.TryPeekDecimals(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out decimal[] values) => context.TryReadDecimals(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekDecimalsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadDecimalsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out decimal[] values) => context.TryPeekDecimalsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out decimal[] values) => context.TryReadDecimalsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<decimal> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, decimal value) => context.WriteDecimalPrimitive(value),
        Peek = (ReadContext context) => context.PeekDecimalPrimitive(),
        Read = (ReadContext context) => context.ReadDecimalPrimitive(),
        WriteSpan = (ref WriteContext context, Span<decimal> values) => context.WriteDecimalsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<decimal> destination) => context.PeekDecimalSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<decimal> destination) => context.ReadDecimalSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, decimal[] values) => context.WriteDecimalsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekDecimalArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadDecimalArrayPrimitive(count),
    };
}
