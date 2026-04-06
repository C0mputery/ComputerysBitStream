using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawULongExtensionsTests : ExtensionTestSuite<ulong> {
    protected override ulong Value => 42ul;
    protected override ulong[] Values => [42ul, 100ul, 42ul, 42ul, 100ul];

    protected override void WriteRaw(WriteContext context, ulong value) => context.WriteULongRaw(value);
    protected override ulong PeekRaw(ReadContext context) => context.PeekULongRaw();
    protected override ulong ReadRaw(ReadContext context) => context.ReadULongRaw();
    protected override void Write(WriteContext context, ulong value) => context.WriteULong(value);
    protected override ulong Peek(ReadContext context) => context.PeekULong();
    protected override ulong Read(ReadContext context) => context.ReadULong();
    protected override void WriteAlias(WriteContext context, ulong value) => context.Write(value);
    protected override ulong PeekAlias(ReadContext context) { context.Peek(out ulong v); return v; }
    protected override ulong ReadAlias(ReadContext context) { context.Read(out ulong v); return v; }
    protected override ulong TryPeek(ReadContext context) { Assert.True(context.TryPeek(out ulong v)); return v; }
    protected override ulong TryRead(ReadContext context) { Assert.True(context.TryRead(out ulong v)); return v; }
    protected override ulong TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out ulong v)); return v; }
    protected override ulong TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out ulong v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<ulong> values) => context.WriteULongsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<ulong> destination) => context.PeekULongSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<ulong> destination) => context.ReadULongSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<ulong> values) => context.WriteULongsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<ulong> destination) => context.PeekULongs(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<ulong> destination) => context.ReadULongs(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<ulong> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ulong> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ulong> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<ulong> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<ulong> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ulong> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<ulong> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<ulong> values) => context.WriteULongs(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<ulong> destination) => context.PeekULongs(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<ulong> destination) => context.ReadULongs(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<ulong> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<ulong> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<ulong> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<ulong> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<ulong> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<ulong> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<ulong> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, ulong[] values) => context.WriteULongsRaw(values);
    protected override ulong[] PeekArrayRaw(ReadContext context, int count) => context.PeekULongArrayRaw(count);
    protected override ulong[] ReadArrayRaw(ReadContext context, int count) => context.ReadULongArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, ulong[] values) => context.WriteULongsWithoutLength(values);
    protected override ulong[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekULongs(count);
    protected override ulong[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadULongs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, ulong[] values) => context.WriteWithoutLength(values);
    protected override ulong[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out ulong[] values); return values; }
    protected override ulong[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out ulong[] values); return values; }
    protected override ulong[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekULongs(count, out ulong[] values)); return values; }
    protected override ulong[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadULongs(count, out ulong[] values)); return values; }
    protected override ulong[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out ulong[] values)); return values; }
    protected override ulong[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out ulong[] values)); return values; }

    protected override void WriteArray(WriteContext context, ulong[] values) => context.WriteULongs(values);
    protected override ulong[] PeekArrayWithLength(ReadContext context) => context.PeekULongs();
    protected override ulong[] ReadArrayWithLength(ReadContext context) => context.ReadULongs();
    protected override void WriteArrayAlias(WriteContext context, ulong[] values) => context.Write(values);
    protected override ulong[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out ulong[] values); return values; }
    protected override ulong[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out ulong[] values); return values; }
    protected override ulong[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekULongs(out ulong[] values)); return values; }
    protected override ulong[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadULongs(out ulong[] values)); return values; }
    protected override ulong[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out ulong[] values)); return values; }
    protected override ulong[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out ulong[] values)); return values; }
}
