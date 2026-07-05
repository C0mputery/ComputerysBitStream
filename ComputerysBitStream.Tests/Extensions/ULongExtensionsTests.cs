namespace ComputerysBitStream.Tests.Extensions;

[BitStreamPrimitiveContext]
public class ULongExtensionsTests : ExtensionTestSuite<ulong> {
        protected override ulong Value => 42ul;
        protected override ulong[] Values => [42ul, 100ul, 42ul, 42ul, 100ul];

    protected override void WritePrimitive(ref WriteContext context, ulong value) => context.WriteULongPrimitive(value);
    protected override ulong PeekPrimitive(ReadContext context) => context.PeekULongPrimitive();
    protected override ulong ReadPrimitive(ReadContext context) => context.ReadULongPrimitive();
    protected override void Write(ref WriteContext context, ulong value) => context.WriteULong(value);
    protected override ulong Peek(ReadContext context) => context.PeekULong();
    protected override ulong Read(ReadContext context) => context.ReadULong();
    protected override ulong TryPeek(ReadContext context) { Assert.True(context.TryPeekULong(out ulong v)); return v; }
    protected override ulong TryRead(ReadContext context) { Assert.True(context.TryReadULong(out ulong v)); return v; }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<ulong> values) => context.WriteULongsPrimitive(values);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<ulong> destination) => context.PeekULongSpanPrimitive(count, destination);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<ulong> destination) => context.ReadULongSpanPrimitive(count, destination);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<ulong> values) => context.WriteULongsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) => context.PeekULongs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) => context.ReadULongs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) { Assert.True(context.TryPeekULongs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<ulong> destination) { Assert.True(context.TryReadULongs(count, destination)); }
    protected override void WriteSpan(ref WriteContext context, Span<ulong> values) => context.WriteULongs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<ulong> destination) => context.PeekULongs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<ulong> destination) => context.ReadULongs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<ulong> destination) { Assert.True(context.TryPeekULongs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<ulong> destination) { Assert.True(context.TryReadULongs(destination)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, ulong[] values) => context.WriteULongsPrimitive(values);
    protected override ulong[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekULongArrayPrimitive(count);
    protected override ulong[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadULongArrayPrimitive(count);
    protected override void WriteArrayWithoutLength(ref WriteContext context, ulong[] values) => context.WriteULongsWithoutLength(values);
    protected override ulong[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekULongs(count);
    protected override ulong[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadULongs(count);
    protected override ulong[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekULongs(count, out ulong[] values)); return values; }
    protected override ulong[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadULongs(count, out ulong[] values)); return values; }

    protected override void WriteArray(ref WriteContext context, ulong[] values) => context.WriteULongs(values);
    protected override ulong[] PeekArrayWithLength(ReadContext context) => context.PeekULongs();
    protected override ulong[] ReadArrayWithLength(ReadContext context) => context.ReadULongs();
    protected override ulong[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekULongs(out ulong[] values)); return values; }
    protected override ulong[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadULongs(out ulong[] values)); return values; }
    protected override TryReadOperationSet<ulong> TryOperations => new() {
        TryPeekValue = (ReadContext c, out ulong v) => c.TryPeekULong(out v),
        TryReadValue = (ReadContext c, out ulong v) => c.TryReadULong(out v),
        TryPeekArrayWithLength = (ReadContext c, out ulong[] v) => c.TryPeekULongs(out v),
        TryReadArrayWithLength = (ReadContext c, out ulong[] v) => c.TryReadULongs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out ulong[] v) => c.TryPeekULongs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out ulong[] v) => c.TryReadULongs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<ulong> d) => c.TryPeekULongs(d),
        TryReadSpanWithLength = (ReadContext c, Span<ulong> d) => c.TryReadULongs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<ulong> d) => c.TryPeekULongs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<ulong> d) => c.TryReadULongs(count, d),
    };
}
