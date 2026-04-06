using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class CharExtensionsTests : ExtensionTestSuite<char> {
    protected override char SingleValue => 'a';
    protected override char[] SpanValues => ['a', 'b', 'a', 'a', 'b'];

    protected override void WriteRaw(WriteContext context, char value) => context.WriteCharRaw(value);
    protected override char PeekRaw(ReadContext context) => context.PeekCharRaw();
    protected override char ReadRaw(ReadContext context) => context.ReadCharRaw();
    protected override void Write(WriteContext context, char value) => context.WriteChar(value);
    protected override char Peek(ReadContext context) => context.PeekChar();
    protected override char Read(ReadContext context) => context.ReadChar();
    protected override void WriteAlias(WriteContext context, char value) => context.Write(value);
    protected override char PeekAlias(ReadContext context) { context.Peek(out char v); return v; }
    protected override char ReadAlias(ReadContext context) { context.Read(out char v); return v; }
    protected override char TryPeek(ReadContext context) { Assert.True(context.TryPeek(out char v)); return v; }
    protected override char TryRead(ReadContext context) { Assert.True(context.TryRead(out char v)); return v; }
    protected override char TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out char v)); return v; }
    protected override char TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out char v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<char> values) => context.WriteCharsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<char> destination) => context.PeekCharSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<char> destination) => context.ReadCharSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<char> values) => context.WriteCharsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<char> destination) => context.PeekChars(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<char> destination) => context.ReadChars(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<char> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<char> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<char> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<char> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<char> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<char> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<char> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<char> values) => context.WriteChars(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<char> destination) => context.PeekChars(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<char> destination) => context.ReadChars(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<char> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<char> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<char> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<char> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<char> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<char> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<char> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, char[] values) => context.WriteCharsRaw(values);
    protected override char[] PeekArrayRaw(ReadContext context, int count) => context.PeekCharArrayRaw(count);
    protected override char[] ReadArrayRaw(ReadContext context, int count) => context.ReadCharArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, char[] values) => context.WriteCharsWithoutLength(values);
    protected override char[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekChars(count);
    protected override char[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadChars(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, char[] values) => context.WriteWithoutLength(values);
    protected override char[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out char[] values); return values; }
    protected override char[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out char[] values); return values; }
    protected override char[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekChars(count, out char[] values)); return values; }
    protected override char[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadChars(count, out char[] values)); return values; }
    protected override char[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out char[] values)); return values; }
    protected override char[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out char[] values)); return values; }

    protected override void WriteArray(WriteContext context, char[] values) => context.WriteChars(values);
    protected override char[] PeekArrayWithLength(ReadContext context) => context.PeekChars();
    protected override char[] ReadArrayWithLength(ReadContext context) => context.ReadChars();
    protected override void WriteArrayAlias(WriteContext context, char[] values) => context.Write(values);
    protected override char[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out char[] values); return values; }
    protected override char[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out char[] values); return values; }
    protected override char[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekChars(out char[] values)); return values; }
    protected override char[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadChars(out char[] values)); return values; }
    protected override char[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out char[] values)); return values; }
    protected override char[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out char[] values)); return values; }
}
