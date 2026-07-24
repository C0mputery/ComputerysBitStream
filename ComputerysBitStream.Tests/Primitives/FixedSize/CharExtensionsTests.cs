using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.FixedSize;

[BitStreamPrimitiveContext]
public class CharExtensionsTests : PrimitiveSerializationTestSuite<char> {
    protected override char Value => 'a';
    protected override char[] Values => ['a', 'b', 'a', 'a', 'b'];

    protected override SerializationOperations<char> Operations { get; } = new() {
        Write = (ref WriteContext context, char value) => context.WriteChar(value),
        Peek = (ReadContext context) => context.PeekChar(),
        Read = (ReadContext context) => context.ReadChar(),
        TryPeek = (ReadContext context, out char value) => context.TryPeekChar(out value),
        TryRead = (ReadContext context, out char value) => context.TryReadChar(out value),
        WriteSpan = (ref WriteContext context, Span<char> values) => context.WriteChars(values),
        PeekSpan = (ReadContext context, Span<char> destination) => context.PeekChars(destination),
        ReadSpan = (ReadContext context, Span<char> destination) => context.ReadChars(destination),
        TryPeekSpan = (ReadContext context, Span<char> destination) => context.TryPeekChars(destination),
        TryReadSpan = (ReadContext context, Span<char> destination) => context.TryReadChars(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<char> values) => context.WriteCharsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<char> destination) => context.PeekChars(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<char> destination) => context.ReadChars(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<char> destination) => context.TryPeekChars(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<char> destination) => context.TryReadChars(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<char> destination) => context.PeekCharsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<char> destination) => context.ReadCharsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<char> destination) => context.TryPeekCharsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<char> destination) => context.TryReadCharsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, char[] values) => context.WriteChars(values),
        PeekArray = (ReadContext context) => context.PeekChars(),
        ReadArray = (ReadContext context) => context.ReadChars(),
        TryPeekArray = (ReadContext context, out char[] values) => context.TryPeekChars(out values),
        TryReadArray = (ReadContext context, out char[] values) => context.TryReadChars(out values),
        WriteArrayWithoutLength = (ref WriteContext context, char[] values) => context.WriteCharsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekChars(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadChars(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out char[] values) => context.TryPeekChars(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out char[] values) => context.TryReadChars(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekCharsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadCharsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out char[] values) => context.TryPeekCharsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out char[] values) => context.TryReadCharsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<char> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, char value) => context.WriteCharPrimitive(value),
        Peek = (ReadContext context) => context.PeekCharPrimitive(),
        Read = (ReadContext context) => context.ReadCharPrimitive(),
        WriteSpan = (ref WriteContext context, Span<char> values) => context.WriteCharsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<char> destination) => context.PeekCharSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<char> destination) => context.ReadCharSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, char[] values) => context.WriteCharsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekCharArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadCharArrayPrimitive(count),
    };
}
