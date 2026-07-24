using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.Quantized;

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

    protected override SerializationOperations<double> Operations { get; } = new() {
        Write = (ref WriteContext context, double value) => context.WriteQuantizedDouble(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedDouble(Min, Max, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedDouble(Min, Max, BitCount),
        TryPeek = (ReadContext context, out double value) => context.TryPeekQuantizedDouble(Min, Max, BitCount, out value),
        TryRead = (ReadContext context, out double value) => context.TryReadQuantizedDouble(Min, Max, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<double> values) => context.WriteQuantizedDoubles(values, Min, Max, BitCount),
        PeekSpan = (ReadContext context, Span<double> destination) => context.PeekQuantizedDoubles(destination, Min, Max, BitCount),
        ReadSpan = (ReadContext context, Span<double> destination) => context.ReadQuantizedDoubles(destination, Min, Max, BitCount),
        TryPeekSpan = (ReadContext context, Span<double> destination) => context.TryPeekQuantizedDoubles(destination, Min, Max, BitCount),
        TryReadSpan = (ReadContext context, Span<double> destination) => context.TryReadQuantizedDoubles(destination, Min, Max, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<double> values) => context.WriteQuantizedDoublesWithoutLength(values, Min, Max, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.PeekQuantizedDoubles(count, destination, Min, Max, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.ReadQuantizedDoubles(count, destination, Min, Max, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.TryPeekQuantizedDoubles(count, destination, Min, Max, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.TryReadQuantizedDoubles(count, destination, Min, Max, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.PeekQuantizedDoublesWithMaxCount(maxCount, destination, Min, Max, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.ReadQuantizedDoublesWithMaxCount(maxCount, destination, Min, Max, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.TryPeekQuantizedDoublesWithMaxCount(maxCount, destination, Min, Max, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.TryReadQuantizedDoublesWithMaxCount(maxCount, destination, Min, Max, BitCount),
        WriteArray = (ref WriteContext context, double[] values) => context.WriteQuantizedDoubles(values, Min, Max, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedDoubles(Min, Max, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedDoubles(Min, Max, BitCount),
        TryPeekArray = (ReadContext context, out double[] values) => context.TryPeekQuantizedDoubles(Min, Max, BitCount, out values),
        TryReadArray = (ReadContext context, out double[] values) => context.TryReadQuantizedDoubles(Min, Max, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, double[] values) => context.WriteQuantizedDoublesWithoutLength(values, Min, Max, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedDoubles(count, Min, Max, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedDoubles(count, Min, Max, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out double[] values) => context.TryPeekQuantizedDoubles(count, Min, Max, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out double[] values) => context.TryReadQuantizedDoubles(count, Min, Max, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedDoublesWithMaxCount(maxCount, Min, Max, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedDoublesWithMaxCount(maxCount, Min, Max, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out double[] values) => context.TryPeekQuantizedDoublesWithMaxCount(maxCount, Min, Max, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out double[] values) => context.TryReadQuantizedDoublesWithMaxCount(maxCount, Min, Max, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<double> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, double value) => context.WriteQuantizedDoublePrimitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedDoublePrimitive(Min, Max, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedDoublePrimitive(Min, Max, BitCount),
        WriteSpan = (ref WriteContext context, Span<double> values) => context.WriteQuantizedDoublesPrimitive(values, Min, Max, BitCount),
        PeekSpan = (ReadContext context, int count, Span<double> destination) => context.PeekQuantizedDoubleSpanPrimitive(count, destination, Min, Max, BitCount),
        ReadSpan = (ReadContext context, int count, Span<double> destination) => context.ReadQuantizedDoubleSpanPrimitive(count, destination, Min, Max, BitCount),
        WriteArray = (ref WriteContext context, double[] values) => context.WriteQuantizedDoublesPrimitive(values, Min, Max, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedDoubleArrayPrimitive(count, Min, Max, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedDoubleArrayPrimitive(count, Min, Max, BitCount),
    };
}
