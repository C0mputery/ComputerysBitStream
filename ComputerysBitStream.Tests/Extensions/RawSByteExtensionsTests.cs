using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class RawSByteExtensionsTests : ExtensionTestSuite<sbyte> {
    protected override sbyte SingleValue => 42;
    protected override sbyte[] SpanValues => [42, -42, 42, 42, -42];

    protected override void WriteRaw(WriteContext context, sbyte value) => context.WriteSByteRaw(value);
    protected override sbyte PeekRaw(ReadContext context) => context.PeekSByteRaw();
    protected override sbyte ReadRaw(ReadContext context) => context.ReadSByteRaw();
    protected override void Write(WriteContext context, sbyte value) => context.WriteSByte(value);
    protected override sbyte Peek(ReadContext context) => context.PeekSByte();
    protected override sbyte Read(ReadContext context) => context.ReadSByte();
    protected override void WriteAlias(WriteContext context, sbyte value) => context.Write(value);
    protected override sbyte PeekAlias(ReadContext context) { context.Peek(out sbyte v); return v; }
    protected override sbyte ReadAlias(ReadContext context) { context.Read(out sbyte v); return v; }
    protected override sbyte TryPeek(ReadContext context) { Assert.True(context.TryPeek(out sbyte v)); return v; }
    protected override sbyte TryRead(ReadContext context) { Assert.True(context.TryRead(out sbyte v)); return v; }
    protected override sbyte TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out sbyte v)); return v; }
    protected override sbyte TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out sbyte v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<sbyte> values) => context.WriteSBytesRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<sbyte> destination) => context.PeekSByteSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<sbyte> destination) => context.ReadSByteSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<sbyte> values) => context.WriteSBytesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<sbyte> destination) => context.PeekSBytes(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<sbyte> destination) => context.ReadSBytes(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<sbyte> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<sbyte> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<sbyte> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<sbyte> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<sbyte> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<sbyte> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<sbyte> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<sbyte> values) => context.WriteSBytes(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<sbyte> destination) => context.PeekSBytes(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<sbyte> destination) => context.ReadSBytes(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<sbyte> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<sbyte> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<sbyte> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<sbyte> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<sbyte> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<sbyte> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<sbyte> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, sbyte[] values) => context.WriteSBytesRaw(values);
    protected override sbyte[] PeekArrayRaw(ReadContext context, int count) => context.PeekSByteArrayRaw(count);
    protected override sbyte[] ReadArrayRaw(ReadContext context, int count) => context.ReadSByteArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, sbyte[] values) => context.WriteSBytesWithoutLength(values);
    protected override sbyte[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekSBytes(count);
    protected override sbyte[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadSBytes(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, sbyte[] values) => context.WriteWithoutLength(values);
    protected override sbyte[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out sbyte[] values); return values; }
    protected override sbyte[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out sbyte[] values); return values; }
    protected override sbyte[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekSBytes(count, out sbyte[] values)); return values; }
    protected override sbyte[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadSBytes(count, out sbyte[] values)); return values; }
    protected override sbyte[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out sbyte[] values)); return values; }
    protected override sbyte[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out sbyte[] values)); return values; }

    protected override void WriteArray(WriteContext context, sbyte[] values) => context.WriteSBytes(values);
    protected override sbyte[] PeekArrayWithLength(ReadContext context) => context.PeekSBytes();
    protected override sbyte[] ReadArrayWithLength(ReadContext context) => context.ReadSBytes();
    protected override void WriteArrayAlias(WriteContext context, sbyte[] values) => context.Write(values);
    protected override sbyte[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out sbyte[] values); return values; }
    protected override sbyte[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out sbyte[] values); return values; }
    protected override sbyte[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekSBytes(out sbyte[] values)); return values; }
    protected override sbyte[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadSBytes(out sbyte[] values)); return values; }
    protected override sbyte[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out sbyte[] values)); return values; }
    protected override sbyte[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out sbyte[] values)); return values; }
}
