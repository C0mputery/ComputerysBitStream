using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawShortExtensionsTests : ExtensionTestSuite<short> {
    protected override short SingleValue => (short)42;
    protected override short[] SpanValues => [(short)42, (short)-42, (short)42, (short)42, (short)-42];

    protected override void WriteRaw(WriteContext context, short value) => context.WriteShortRaw(value);
    protected override short PeekRaw(ReadContext context) => context.PeekShortRaw();
    protected override short ReadRaw(ReadContext context) => context.ReadShortRaw();
    protected override void Write(WriteContext context, short value) => context.WriteShort(value);
    protected override short Peek(ReadContext context) => context.PeekShort();
    protected override short Read(ReadContext context) => context.ReadShort();
    protected override void WriteAlias(WriteContext context, short value) => context.Write(value);
    protected override short PeekAlias(ReadContext context) { context.Peek(out short v); return v; }
    protected override short ReadAlias(ReadContext context) { context.Read(out short v); return v; }
    protected override short TryPeek(ReadContext context) { Assert.True(context.TryPeek(out short v)); return v; }
    protected override short TryRead(ReadContext context) { Assert.True(context.TryRead(out short v)); return v; }
    protected override short TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out short v)); return v; }
    protected override short TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out short v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<short> values) => context.WriteShortsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<short> destination) => context.PeekShortSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<short> destination) => context.ReadShortSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<short> values) => context.WriteShortsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<short> destination) => context.PeekShorts(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<short> destination) => context.ReadShorts(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<short> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<short> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<short> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<short> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<short> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<short> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<short> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<short> values) => context.WriteShorts(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<short> destination) => context.PeekShorts(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<short> destination) => context.ReadShorts(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<short> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<short> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<short> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<short> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<short> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<short> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<short> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, short[] values) => context.WriteShortsRaw(values);
    protected override short[] PeekArrayRaw(ReadContext context, int count) => context.PeekShortArrayRaw(count);
    protected override short[] ReadArrayRaw(ReadContext context, int count) => context.ReadShortArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, short[] values) => context.WriteShortsWithoutLength(values);
    protected override short[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekShorts(count);
    protected override short[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadShorts(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, short[] values) => context.WriteWithoutLength(values);
    protected override short[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out short[] values); return values; }
    protected override short[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out short[] values); return values; }
    protected override short[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekShorts(count, out short[] values)); return values; }
    protected override short[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadShorts(count, out short[] values)); return values; }
    protected override short[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out short[] values)); return values; }
    protected override short[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out short[] values)); return values; }

    protected override void WriteArray(WriteContext context, short[] values) => context.WriteShorts(values);
    protected override short[] PeekArrayWithLength(ReadContext context) => context.PeekShorts();
    protected override short[] ReadArrayWithLength(ReadContext context) => context.ReadShorts();
    protected override void WriteArrayAlias(WriteContext context, short[] values) => context.Write(values);
    protected override short[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out short[] values); return values; }
    protected override short[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out short[] values); return values; }
    protected override short[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekShorts(out short[] values)); return values; }
    protected override short[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadShorts(out short[] values)); return values; }
    protected override short[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out short[] values)); return values; }
    protected override short[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out short[] values)); return values; }
}
