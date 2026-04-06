using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawUIntExtensionsTests : ExtensionTestSuite<uint> {
    protected override uint Value => 42u;
    protected override uint[] Values => [42u, 100u, 42u, 42u, 100u];

    protected override void WriteRaw(WriteContext context, uint value) => context.WriteUIntRaw(value);
    protected override uint PeekRaw(ReadContext context) => context.PeekUIntRaw();
    protected override uint ReadRaw(ReadContext context) => context.ReadUIntRaw();
    protected override void Write(WriteContext context, uint value) => context.WriteUInt(value);
    protected override uint Peek(ReadContext context) => context.PeekUInt();
    protected override uint Read(ReadContext context) => context.ReadUInt();
    protected override void WriteAlias(WriteContext context, uint value) => context.Write(value);
    protected override uint PeekAlias(ReadContext context) { context.Peek(out uint v); return v; }
    protected override uint ReadAlias(ReadContext context) { context.Read(out uint v); return v; }
    protected override uint TryPeek(ReadContext context) { Assert.True(context.TryPeek(out uint v)); return v; }
    protected override uint TryRead(ReadContext context) { Assert.True(context.TryRead(out uint v)); return v; }
    protected override uint TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out uint v)); return v; }
    protected override uint TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out uint v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<uint> values) => context.WriteUIntsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<uint> destination) => context.PeekUIntSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<uint> destination) => context.ReadUIntSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<uint> values) => context.WriteUIntsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<uint> destination) => context.PeekUInts(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<uint> destination) => context.ReadUInts(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<uint> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<uint> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<uint> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<uint> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<uint> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<uint> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<uint> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<uint> values) => context.WriteUInts(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<uint> destination) => context.PeekUInts(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<uint> destination) => context.ReadUInts(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<uint> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<uint> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<uint> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<uint> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<uint> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<uint> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<uint> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, uint[] values) => context.WriteUIntsRaw(values);
    protected override uint[] PeekArrayRaw(ReadContext context, int count) => context.PeekUIntArrayRaw(count);
    protected override uint[] ReadArrayRaw(ReadContext context, int count) => context.ReadUIntArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, uint[] values) => context.WriteUIntsWithoutLength(values);
    protected override uint[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekUInts(count);
    protected override uint[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadUInts(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, uint[] values) => context.WriteWithoutLength(values);
    protected override uint[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out uint[] values); return values; }
    protected override uint[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out uint[] values); return values; }
    protected override uint[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekUInts(count, out uint[] values)); return values; }
    protected override uint[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadUInts(count, out uint[] values)); return values; }
    protected override uint[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out uint[] values)); return values; }
    protected override uint[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out uint[] values)); return values; }

    protected override void WriteArray(WriteContext context, uint[] values) => context.WriteUInts(values);
    protected override uint[] PeekArrayWithLength(ReadContext context) => context.PeekUInts();
    protected override uint[] ReadArrayWithLength(ReadContext context) => context.ReadUInts();
    protected override void WriteArrayAlias(WriteContext context, uint[] values) => context.Write(values);
    protected override uint[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out uint[] values); return values; }
    protected override uint[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out uint[] values); return values; }
    protected override uint[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekUInts(out uint[] values)); return values; }
    protected override uint[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadUInts(out uint[] values)); return values; }
    protected override uint[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out uint[] values)); return values; }
    protected override uint[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out uint[] values)); return values; }
}
