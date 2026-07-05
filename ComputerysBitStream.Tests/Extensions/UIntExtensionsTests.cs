namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class UIntExtensionsTests : ExtensionTestSuite<uint> {
        protected override uint Value => 42u;
        protected override uint[] Values => [42u, 100u, 42u, 42u, 100u];

    protected override void WritePrimitive(ref WriteContext context, uint value) => context.WriteUIntPrimitive(value);
    protected override uint PeekPrimitive(ReadContext context) => context.PeekUIntPrimitive();
    protected override uint ReadPrimitive(ReadContext context) => context.ReadUIntPrimitive();
    protected override void Write(ref WriteContext context, uint value) => context.WriteUInt(value);
    protected override uint Peek(ReadContext context) => context.PeekUInt();
    protected override uint Read(ReadContext context) => context.ReadUInt();
    protected override uint TryPeek(ReadContext context) { Assert.True(context.TryPeekUInt(out uint v)); return v; }
    protected override uint TryRead(ReadContext context) { Assert.True(context.TryReadUInt(out uint v)); return v; }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<uint> values) => context.WriteUIntsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<uint> destination) => context.PeekUIntSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<uint> destination) => context.ReadUIntSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<uint> values) => context.WriteUIntsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<uint> destination) => context.PeekUInts(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<uint> destination) => context.ReadUInts(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<uint> destination) { Assert.True(context.TryPeekUInts(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<uint> destination) { Assert.True(context.TryReadUInts(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<uint> values) => context.WriteUInts(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<uint> destination) => context.PeekUInts(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<uint> destination) => context.ReadUInts(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<uint> destination) { Assert.True(context.TryPeekUInts(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<uint> destination) { Assert.True(context.TryReadUInts(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, uint[] values) => context.WriteUIntsPrimitive(values);
    protected override uint[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekUIntArrayPrimitive(count);
    protected override uint[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadUIntArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, uint[] values) => context.WriteUIntsWithoutLength(values);
    protected override uint[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekUInts(count);
    protected override uint[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadUInts(count);
    protected override uint[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekUInts(count, out uint[] values)); return values; }
    protected override uint[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadUInts(count, out uint[] values)); return values; }

    protected override void WriteArray(ref WriteContext context, uint[] values) => context.WriteUInts(values);
    protected override uint[] PeekArrayWithLength(ReadContext context) => context.PeekUInts();
    protected override uint[] ReadArrayWithLength(ReadContext context) => context.ReadUInts();
    protected override uint[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekUInts(out uint[] values)); return values; }
    protected override uint[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadUInts(out uint[] values)); return values; }
    protected override TryReadOperationSet<uint> TryOperations => new() {
        TryPeekValue = (ReadContext c, out uint v) => c.TryPeekUInt(out v),
        TryReadValue = (ReadContext c, out uint v) => c.TryReadUInt(out v),
        TryPeekArrayWithLength = (ReadContext c, out uint[] v) => c.TryPeekUInts(out v),
        TryReadArrayWithLength = (ReadContext c, out uint[] v) => c.TryReadUInts(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out uint[] v) => c.TryPeekUInts(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out uint[] v) => c.TryReadUInts(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<uint> d) => c.TryPeekUInts(d),
        TryReadSpanWithLength = (ReadContext c, Span<uint> d) => c.TryReadUInts(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<uint> d) => c.TryPeekUInts(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<uint> d) => c.TryReadUInts(count, d),
    };
}
