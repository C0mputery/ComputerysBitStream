namespace ComputerysBitStream.Tests.VariableLength;

[BitStreamPrimitiveContext]
public class StringExtensionsTests : VariableLengthExtensionTestSuite<string> {
    protected override string Value => "hello";
    protected override string[] Values => ["hello", "", "café", "🎮"];
    protected override int GetSize(string value) => PrimitiveStringExtensions.GetStringSize(value);

    protected override void WritePrimitive(ref WriteContext context, string value) => context.WriteStringPrimitive(value);
    protected override string PeekPrimitive(ReadContext context) => context.PeekStringPrimitive();
    protected override string ReadPrimitive(ReadContext context) => context.ReadStringPrimitive();
    protected override void Write(ref WriteContext context, string value) => context.WriteString(value);
    protected override string Peek(ReadContext context) => context.PeekString();
    protected override string Read(ReadContext context) => context.ReadString();

    protected override string TryPeek(ReadContext context) {
        Assert.True(context.TryPeekString(out string v));
        return v;
    }

    protected override string TryRead(ReadContext context) {
        Assert.True(context.TryReadString(out string v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<string> values) => context.WriteStringsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<string> destination) => context.PeekStringSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<string> destination) => context.ReadStringSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<string> values) => context.WriteStringsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<string> destination) => context.PeekStrings(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<string> destination) => context.ReadStrings(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<string> destination) { Assert.True(context.TryPeekStrings(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<string> destination) { Assert.True(context.TryReadStrings(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<string> values) => context.WriteStrings(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<string> destination) => context.PeekStrings(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<string> destination) => context.ReadStrings(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<string> destination) { Assert.True(context.TryPeekStrings(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<string> destination) { Assert.True(context.TryReadStrings(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, string[] values) => context.WriteStringsPrimitive(values);
    protected override string[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekStringArrayPrimitive(count);
    protected override string[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadStringArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, string[] values) => context.WriteStringsWithoutLength(values);
    protected override string[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekStrings(count);
    protected override string[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadStrings(count);

    protected override string[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekStrings(count, out string[] values));
        return values;
    }

    protected override string[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadStrings(count, out string[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, string[] values) => context.WriteStrings(values);
    protected override string[] PeekArrayWithLength(ReadContext context) => context.PeekStrings();
    protected override string[] ReadArrayWithLength(ReadContext context) => context.ReadStrings();

    protected override string[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekStrings(out string[] values));
        return values;
    }

    protected override string[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadStrings(out string[] values));
        return values;
    }

    protected override TryReadOperationSet<string> TryOperations => new() {
        TryPeekValue = (ReadContext c, out string v) => c.TryPeekString(out v),
        TryReadValue = (ReadContext c, out string v) => c.TryReadString(out v),
        TryPeekArrayWithLength = (ReadContext c, out string[] v) => c.TryPeekStrings(out v),
        TryReadArrayWithLength = (ReadContext c, out string[] v) => c.TryReadStrings(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out string[] v) => c.TryPeekStrings(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out string[] v) => c.TryReadStrings(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<string> d) => c.TryPeekStrings(d),
        TryReadSpanWithLength = (ReadContext c, Span<string> d) => c.TryReadStrings(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<string> d) => c.TryPeekStrings(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<string> d) => c.TryReadStrings(count, d),
    };

    [Fact]
    public void WriteAndReadNull_ShouldReturnEmptyString() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeContext = new(buffer);
        writeContext.WriteString(null!);

        ReadContext readContext = new(buffer, 0, writeContext.Position);
        Assert.Equal(string.Empty, readContext.ReadString());
    }
}
