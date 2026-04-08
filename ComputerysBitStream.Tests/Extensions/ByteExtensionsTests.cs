using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class ByteExtensionsTests : ExtensionTestSuite<byte> {
    protected override byte Value => 42;
    protected override byte[] Values => [42, 100, 42, 42, 100];

    protected override void WriteRaw(WriteContext context, byte value) => context.WriteByteRaw(value);
    protected override byte PeekRaw(ReadContext context) => context.PeekByteRaw();
    protected override byte ReadRaw(ReadContext context) => context.ReadByteRaw();
    protected override void Write(WriteContext context, byte value) => context.WriteByte(value);
    protected override byte Peek(ReadContext context) => context.PeekByte();
    protected override byte Read(ReadContext context) => context.ReadByte();
    protected override void WriteAlias(WriteContext context, byte value) => context.Write(value);
    protected override byte PeekAlias(ReadContext context) { context.Peek(out byte v); return v; }
    protected override byte ReadAlias(ReadContext context) { context.Read(out byte v); return v; }
    protected override byte TryPeek(ReadContext context) { Assert.True(context.TryPeek(out byte v)); return v; }
    protected override byte TryRead(ReadContext context) { Assert.True(context.TryRead(out byte v)); return v; }
    protected override byte TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out byte v)); return v; }
    protected override byte TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out byte v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<byte> values) => context.WriteBytesRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<byte> destination) => context.PeekByteSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<byte> destination) => context.ReadByteSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<byte> values) => context.WriteBytesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<byte> destination) => context.PeekBytes(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<byte> destination) => context.ReadBytes(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<byte> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<byte> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<byte> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<byte> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<byte> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<byte> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<byte> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<byte> values) => context.WriteBytes(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<byte> destination) => context.PeekBytes(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<byte> destination) => context.ReadBytes(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<byte> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<byte> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<byte> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<byte> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<byte> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<byte> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<byte> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, byte[] values) => context.WriteBytesRaw(values);
    protected override byte[] PeekArrayRaw(ReadContext context, int count) => context.PeekByteArrayRaw(count);
    protected override byte[] ReadArrayRaw(ReadContext context, int count) => context.ReadByteArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, byte[] values) => context.WriteBytesWithoutLength(values);
    protected override byte[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekBytes(count);
    protected override byte[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadBytes(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, byte[] values) => context.WriteWithoutLength(values);
    protected override byte[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out byte[] values); return values; }
    protected override byte[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out byte[] values); return values; }
    protected override byte[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekBytes(count, out byte[] values)); return values; }
    protected override byte[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadBytes(count, out byte[] values)); return values; }
    protected override byte[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out byte[] values)); return values; }
    protected override byte[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out byte[] values)); return values; }

    protected override void WriteArray(WriteContext context, byte[] values) => context.WriteBytes(values);
    protected override byte[] PeekArrayWithLength(ReadContext context) => context.PeekBytes();
    protected override byte[] ReadArrayWithLength(ReadContext context) => context.ReadBytes();
    protected override void WriteArrayAlias(WriteContext context, byte[] values) => context.Write(values);
    protected override byte[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out byte[] values); return values; }
    protected override byte[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out byte[] values); return values; }
    protected override byte[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekBytes(out byte[] values)); return values; }
    protected override byte[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadBytes(out byte[] values)); return values; }
    protected override byte[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out byte[] values)); return values; }
    protected override byte[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out byte[] values)); return values; }
}
