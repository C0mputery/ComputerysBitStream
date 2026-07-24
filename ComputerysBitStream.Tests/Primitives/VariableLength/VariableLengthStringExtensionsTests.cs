using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Primitives.VariableLength;

[BitStreamPrimitiveContext]
public class VariableLengthStringExtensionsTests : VariableLengthExtensionTestSuite<string> {
    protected override string Value => "hello";
    protected override string[] Values => ["hello", "", "cafÃ©", "ðŸŽ®"];
    protected override int GetSize(string value) => PrimitiveStringExtensions.GetStringSize(value);

    [Fact]
    public void WriteAndReadNull_ShouldReturnEmptyString() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        writeContext.WriteString(null!);

        ReadContext readContext = new(buffer, 0, writeContext.Position);
        Assert.Equal(string.Empty, readContext.ReadString());
    }

    protected override SerializationOperations<string> Operations { get; } = new() {
        Write = (ref WriteContext context, string value) => context.WriteString(value),
        Peek = (ReadContext context) => context.PeekString(),
        Read = (ReadContext context) => context.ReadString(),
        TryPeek = (ReadContext context, out string value) => context.TryPeekString(out value),
        TryRead = (ReadContext context, out string value) => context.TryReadString(out value),
        WriteSpan = (ref WriteContext context, Span<string> values) => context.WriteStrings(values),
        PeekSpan = (ReadContext context, Span<string> destination) => context.PeekStrings(destination),
        ReadSpan = (ReadContext context, Span<string> destination) => context.ReadStrings(destination),
        TryPeekSpan = (ReadContext context, Span<string> destination) => context.TryPeekStrings(destination),
        TryReadSpan = (ReadContext context, Span<string> destination) => context.TryReadStrings(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<string> values) => context.WriteStringsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<string> destination) => context.PeekStrings(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<string> destination) => context.ReadStrings(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<string> destination) => context.TryPeekStrings(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<string> destination) => context.TryReadStrings(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<string> destination) => context.PeekStringsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<string> destination) => context.ReadStringsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<string> destination) => context.TryPeekStringsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<string> destination) => context.TryReadStringsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, string[] values) => context.WriteStrings(values),
        PeekArray = (ReadContext context) => context.PeekStrings(),
        ReadArray = (ReadContext context) => context.ReadStrings(),
        TryPeekArray = (ReadContext context, out string[] values) => context.TryPeekStrings(out values),
        TryReadArray = (ReadContext context, out string[] values) => context.TryReadStrings(out values),
        WriteArrayWithoutLength = (ref WriteContext context, string[] values) => context.WriteStringsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekStrings(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadStrings(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out string[] values) => context.TryPeekStrings(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out string[] values) => context.TryReadStrings(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekStringsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadStringsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out string[] values) => context.TryPeekStringsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out string[] values) => context.TryReadStringsWithMaxCount(maxCount, out values),
    };

    protected override PrimitiveSerializationOperations<string> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, string value) => context.WriteStringPrimitive(value),
        Peek = (ReadContext context) => context.PeekStringPrimitive(),
        Read = (ReadContext context) => context.ReadStringPrimitive(),
        WriteSpan = (ref WriteContext context, Span<string> values) => context.WriteStringsPrimitive(values),
        PeekSpan = (ReadContext context, int count, Span<string> destination) => context.PeekStringSpanPrimitive(count, destination),
        ReadSpan = (ReadContext context, int count, Span<string> destination) => context.ReadStringSpanPrimitive(count, destination),
        WriteArray = (ref WriteContext context, string[] values) => context.WriteStringsPrimitive(values),
        PeekArray = (ReadContext context, int count) => context.PeekStringArrayPrimitive(count),
        ReadArray = (ReadContext context, int count) => context.ReadStringArrayPrimitive(count),
    };
}
