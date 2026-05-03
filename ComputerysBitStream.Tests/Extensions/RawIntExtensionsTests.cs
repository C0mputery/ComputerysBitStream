using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawIntExtensionsTests : ExtensionTestSuite<int> {
    protected override int Value => 42;
    protected override int[] Values => [42, -42, 42, 42, -42];

    protected override void WriteRaw(WriteContext context, int value) => context.WriteIntRaw(value);
    protected override int PeekRaw(ReadContext context) => context.PeekIntRaw();
    protected override int ReadRaw(ReadContext context) => context.ReadIntRaw();
    protected override void Write(WriteContext context, int value) => context.WriteInt(value);
    protected override int Peek(ReadContext context) => context.PeekInt();
    protected override int Read(ReadContext context) => context.ReadInt();
    protected override void WriteAlias(WriteContext context, int value) => context.Write(value);
    protected override int PeekAlias(ReadContext context) { context.Peek(out int v); return v; }
    protected override int ReadAlias(ReadContext context) { context.Read(out int v); return v; }
    protected override int TryPeek(ReadContext context) { Assert.True(context.TryPeek(out int v)); return v; }
    protected override int TryRead(ReadContext context) { Assert.True(context.TryRead(out int v)); return v; }
    protected override int TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out int v)); return v; }
    protected override int TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out int v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<int> values) => context.WriteIntsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, Span<int> destination) => context.PeekIntSpanRaw(count, destination);
    protected override void ReadSpanRaw(ReadContext context, int count, Span<int> destination) => context.ReadIntSpanRaw(count, destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<int> values) => context.WriteIntsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<int> destination) => context.PeekInts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<int> destination) => context.ReadInts(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<int> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<int> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<int> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryRead(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<int> destination) { Assert.True(context.TryRead(count, destination)); }
    protected override void WriteSpan(WriteContext context, Span<int> values) => context.WriteInts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<int> destination) => context.PeekInts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<int> destination) => context.ReadInts(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<int> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, Span<int> destination) => context.Peek(destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, Span<int> destination) => context.Read(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<int> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<int> destination) { Assert.True(context.TryRead(destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, Span<int> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, Span<int> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteArrayRaw(WriteContext context, int[] values) => context.WriteIntsRaw(values);
    protected override int[] PeekArrayRaw(ReadContext context, int count) => context.PeekIntArrayRaw(count);
    protected override int[] ReadArrayRaw(ReadContext context, int count) => context.ReadIntArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, int[] values) => context.WriteIntsWithoutLength(values);
    protected override int[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekInts(count);
    protected override int[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadInts(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, int[] values) => context.WriteWithoutLength(values);
    protected override int[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out int[] values); return values; }
    protected override int[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out int[] values); return values; }
    protected override int[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekInts(count, out int[] values)); return values; }
    protected override int[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadInts(count, out int[] values)); return values; }
    protected override int[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out int[] values)); return values; }
    protected override int[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out int[] values)); return values; }

    protected override void WriteArray(WriteContext context, int[] values) => context.WriteInts(values);
    protected override int[] PeekArrayWithLength(ReadContext context) => context.PeekInts();
    protected override int[] ReadArrayWithLength(ReadContext context) => context.ReadInts();
    protected override void WriteArrayAlias(WriteContext context, int[] values) => context.Write(values);
    protected override int[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out int[] values); return values; }
    protected override int[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out int[] values); return values; }
    protected override int[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekInts(out int[] values)); return values; }
    protected override int[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadInts(out int[] values)); return values; }
    protected override int[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out int[] values)); return values; }
    protected override int[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out int[] values)); return values; }
}
