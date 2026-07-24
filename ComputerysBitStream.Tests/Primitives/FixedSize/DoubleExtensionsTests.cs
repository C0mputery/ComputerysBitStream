using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class DoubleExtensionsTests : PrimitiveSerializationTestSuite<double> {
    protected override double Value => 1.23;
    protected override double[] Values => [1.23, 4.56, 1.23, 1.23, 4.56];

    protected override SerializationOperations<double> Operations { get; } = new() {
        Write = (ref WriteContext context, double value) => context.WriteDouble(value),
        Peek = (ReadContext context) => context.PeekDouble(),
        Read = (ReadContext context) => context.ReadDouble(),
        TryPeek = (ReadContext context, out double value) => context.TryPeekDouble(out value),
        TryRead = (ReadContext context, out double value) => context.TryReadDouble(out value),
        WriteSpan = (ref WriteContext context, Span<double> values) => context.WriteDoubles(values),
        PeekSpan = (ReadContext context, Span<double> destination) => context.PeekDoubles(destination),
        ReadSpan = (ReadContext context, Span<double> destination) => context.ReadDoubles(destination),
        TryPeekSpan = (ReadContext context, Span<double> destination) => context.TryPeekDoubles(destination),
        TryReadSpan = (ReadContext context, Span<double> destination) => context.TryReadDoubles(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<double> values) => context.WriteDoublesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.PeekDoubles(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.ReadDoubles(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.TryPeekDoubles(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<double> destination) => context.TryReadDoubles(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.PeekDoublesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.ReadDoublesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.TryPeekDoublesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<double> destination) => context.TryReadDoublesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, double[] values) => context.WriteDoubles(values),
        PeekArray = (ReadContext context) => context.PeekDoubles(),
        ReadArray = (ReadContext context) => context.ReadDoubles(),
        TryPeekArray = (ReadContext context, out double[] values) => context.TryPeekDoubles(out values),
        TryReadArray = (ReadContext context, out double[] values) => context.TryReadDoubles(out values),
        WriteArrayWithoutLength = (ref WriteContext context, double[] values) => context.WriteDoublesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekDoubles(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadDoubles(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out double[] values) => context.TryPeekDoubles(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out double[] values) => context.TryReadDoubles(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekDoublesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadDoublesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out double[] values) => context.TryPeekDoublesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out double[] values) => context.TryReadDoublesWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<double> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, double value) => context.WriteDoublePrimitive(value),
        Peek = (ReadContext context) => context.PeekDoublePrimitive(),
        Read = (ReadContext context) => context.ReadDoublePrimitive(),
        WriteSpan = (ref WriteContext context, Span<double> values) => context.WriteDoublesPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<double> destination) => context.PeekDoubleSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<double> destination) => context.ReadDoubleSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, double[] values) => context.WriteDoublesPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekDoubleArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadDoubleArrayPrimitive(count),
    };
}
