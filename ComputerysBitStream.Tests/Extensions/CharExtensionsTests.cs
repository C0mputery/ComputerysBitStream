namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class CharExtensionsTests : PrimitiveSerializationTestSuite<char> {
    protected override char Value => 'a';
    protected override char[] Values => ['a', 'b', 'a', 'a', 'b'];

    protected override void WritePrimitive(ref WriteContext context, char value) => context.WriteCharPrimitive(value);
    protected override char PeekPrimitive(ReadContext context) => context.PeekCharPrimitive();
    protected override char ReadPrimitive(ReadContext context) => context.ReadCharPrimitive();
    protected override void Write(ref WriteContext context, char value) => context.WriteChar(value);
    protected override char Peek(ReadContext context) => context.PeekChar();
    protected override char Read(ReadContext context) => context.ReadChar();

    protected override char TryPeek(ReadContext context) {
        Assert.True(context.TryPeekChar(out char v));
        return v;
    }

    protected override char TryRead(ReadContext context) {
        Assert.True(context.TryReadChar(out char v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<char> values) => context.WriteCharsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<char> destination) => context.PeekCharSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<char> destination) => context.ReadCharSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<char> values) => context.WriteCharsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<char> destination) => context.PeekChars(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<char> destination) => context.ReadChars(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<char> destination) { Assert.True(context.TryPeekChars(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<char> destination) { Assert.True(context.TryReadChars(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<char> values) => context.WriteChars(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<char> destination) => context.PeekChars(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<char> destination) => context.ReadChars(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<char> destination) { Assert.True(context.TryPeekChars(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<char> destination) { Assert.True(context.TryReadChars(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, char[] values) => context.WriteCharsPrimitive(values);
    protected override char[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekCharArrayPrimitive(count);
    protected override char[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadCharArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, char[] values) => context.WriteCharsWithoutLength(values);
    protected override char[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekChars(count);
    protected override char[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadChars(count);

    protected override char[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekChars(count, out char[] values));
        return values;
    }

    protected override char[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadChars(count, out char[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, char[] values) => context.WriteChars(values);
    protected override char[] PeekArrayWithLength(ReadContext context) => context.PeekChars();
    protected override char[] ReadArrayWithLength(ReadContext context) => context.ReadChars();

    protected override char[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekChars(out char[] values));
        return values;
    }

    protected override char[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadChars(out char[] values));
        return values;
    }

    protected override TryReadOperationSet<char> TryOperations => new() {
        TryPeekValue = (ReadContext c, out char v) => c.TryPeekChar(out v),
        TryReadValue = (ReadContext c, out char v) => c.TryReadChar(out v),
        TryPeekArrayWithLength = (ReadContext c, out char[] v) => c.TryPeekChars(out v),
        TryReadArrayWithLength = (ReadContext c, out char[] v) => c.TryReadChars(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out char[] v) => c.TryPeekChars(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out char[] v) => c.TryReadChars(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<char> d) => c.TryPeekChars(d),
        TryReadSpanWithLength = (ReadContext c, Span<char> d) => c.TryReadChars(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<char> d) => c.TryPeekChars(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<char> d) => c.TryReadChars(count, d),
    };
}
