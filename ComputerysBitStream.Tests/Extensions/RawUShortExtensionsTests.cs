using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawUShortExtensionsTests : ExtensionTestSuite<ushort> {
    protected override ushort SingleValue => (ushort)42;
    protected override ushort[] SpanValues => [(ushort)42, (ushort)100, (ushort)42, (ushort)42, (ushort)100];

    protected override void WriteRaw(WriteContext context, ushort value) => context.WriteUShortRaw(value);
    protected override ushort PeekRaw(ReadContext context) => context.PeekUShortRaw();
    protected override ushort ReadRaw(ReadContext context) => context.ReadUShortRaw();
    protected override void Write(WriteContext context, ushort value) => context.WriteUShort(value);
    protected override ushort Peek(ReadContext context) => context.PeekUShort();
    protected override ushort Read(ReadContext context) => context.ReadUShort();
    protected override void WriteAlias(WriteContext context, ushort value) => context.Write(value);
    protected override ushort PeekAlias(ReadContext context) { context.Peek(out ushort v); return v; }
    protected override ushort ReadAlias(ReadContext context) { context.Read(out ushort v); return v; }
    protected override ushort TryPeek(ReadContext context) { Assert.True(context.TryPeek(out ushort v)); return v; }
    protected override ushort TryRead(ReadContext context) { Assert.True(context.TryRead(out ushort v)); return v; }
    protected override ushort TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out ushort v)); return v; }
    protected override ushort TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out ushort v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<ushort> values) => context.WriteUShortsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<ushort> destination) => context.PeekUShortSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<ushort> destination) => context.ReadUShortSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<ushort> values) => context.WriteUShortsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<ushort> destination) => context.PeekUShorts(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<ushort> destination) => context.ReadUShorts(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<ushort> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ushort> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ushort> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<ushort> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<ushort> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ushort> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ushort> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<ushort> values) => context.WriteUShorts(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<ushort> destination) => context.PeekUShorts(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<ushort> destination) => context.ReadUShorts(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<ushort> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<ushort> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<ushort> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<ushort> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<ushort> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<ushort> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<ushort> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, ushort[] values) => context.WriteUShortsRaw(values);
    protected override ushort[] PeekArrayRaw(ReadContext context, int count) => context.PeekUShortArrayRaw(count);
    protected override ushort[] ReadArrayRaw(ReadContext context, int count) => context.ReadUShortArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, ushort[] values) => context.WriteUShortsWithoutLength(values);
    protected override ushort[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekUShorts(count);
    protected override ushort[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadUShorts(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, ushort[] values) => context.WriteWithoutLength(values);
    protected override ushort[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out ushort[] values); return values; }
    protected override ushort[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out ushort[] values); return values; }
    protected override ushort[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekUShorts(count, out ushort[] values)); return values; }
    protected override ushort[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadUShorts(count, out ushort[] values)); return values; }
    protected override ushort[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out ushort[] values)); return values; }
    protected override ushort[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out ushort[] values)); return values; }

    protected override void WriteArray(WriteContext context, ushort[] values) => context.WriteUShorts(values);
    protected override ushort[] PeekArrayWithLength(ReadContext context) => context.PeekUShorts();
    protected override ushort[] ReadArrayWithLength(ReadContext context) => context.ReadUShorts();
    protected override void WriteArrayAlias(WriteContext context, ushort[] values) => context.Write(values);
    protected override ushort[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out ushort[] values); return values; }
    protected override ushort[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out ushort[] values); return values; }
    protected override ushort[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekUShorts(out ushort[] values)); return values; }
    protected override ushort[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadUShorts(out ushort[] values)); return values; }
    protected override ushort[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out ushort[] values)); return values; }
    protected override ushort[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out ushort[] values)); return values; }
}
