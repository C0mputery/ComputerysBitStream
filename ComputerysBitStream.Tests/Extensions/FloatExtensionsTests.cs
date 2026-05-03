using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class FloatExtensionsTests : ExtensionTestSuite<float> {
    protected override float Value => 1.23f;
    protected override float[] Values => [1.23f, 4.56f, 1.23f, 1.23f, 4.56f];

    protected override void WriteRaw(WriteContext context, float value) => context.WriteFloatRaw(value);
    protected override float PeekRaw(ReadContext context) => context.PeekFloatRaw();
    protected override float ReadRaw(ReadContext context) => context.ReadFloatRaw();
    protected override void Write(WriteContext context, float value) => context.WriteFloat(value);
    protected override float Peek(ReadContext context) => context.PeekFloat();
    protected override float Read(ReadContext context) => context.ReadFloat();
    protected override void WriteAlias(WriteContext context, float value) => context.Write(value);
    protected override float PeekAlias(ReadContext context) { context.Peek(out float v); return v; }
    protected override float ReadAlias(ReadContext context) { context.Read(out float v); return v; }
    protected override float TryPeek(ReadContext context) { Assert.True(context.TryPeek(out float v)); return v; }
    protected override float TryRead(ReadContext context) { Assert.True(context.TryRead(out float v)); return v; }
    protected override float TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out float v)); return v; }
    protected override float TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out float v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<float> values) => context.WriteFloatsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, Span<float> destination) => context.PeekFloatSpanRaw(count, destination);
    protected override void ReadSpanRaw(ReadContext context, int count, Span<float> destination) => context.ReadFloatSpanRaw(count, destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<float> values) => context.WriteFloatsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<float> destination) => context.PeekFloats(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<float> destination) => context.ReadFloats(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<float> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<float> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<float> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryRead(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<float> destination) { Assert.True(context.TryRead(count, destination)); }
    protected override void WriteSpan(WriteContext context, Span<float> values) => context.WriteFloats(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<float> destination) => context.PeekFloats(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<float> destination) => context.ReadFloats(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<float> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, Span<float> destination) => context.Peek(destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, Span<float> destination) => context.Read(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<float> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<float> destination) { Assert.True(context.TryRead(destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, Span<float> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, Span<float> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteArrayRaw(WriteContext context, float[] values) => context.WriteFloatsRaw(values);
    protected override float[] PeekArrayRaw(ReadContext context, int count) => context.PeekFloatArrayRaw(count);
    protected override float[] ReadArrayRaw(ReadContext context, int count) => context.ReadFloatArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, float[] values) => context.WriteFloatsWithoutLength(values);
    protected override float[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekFloats(count);
    protected override float[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadFloats(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, float[] values) => context.WriteWithoutLength(values);
    protected override float[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out float[] values); return values; }
    protected override float[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out float[] values); return values; }
    protected override float[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekFloats(count, out float[] values)); return values; }
    protected override float[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadFloats(count, out float[] values)); return values; }
    protected override float[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out float[] values)); return values; }
    protected override float[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out float[] values)); return values; }

    protected override void WriteArray(WriteContext context, float[] values) => context.WriteFloats(values);
    protected override float[] PeekArrayWithLength(ReadContext context) => context.PeekFloats();
    protected override float[] ReadArrayWithLength(ReadContext context) => context.ReadFloats();
    protected override void WriteArrayAlias(WriteContext context, float[] values) => context.Write(values);
    protected override float[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out float[] values); return values; }
    protected override float[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out float[] values); return values; }
    protected override float[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekFloats(out float[] values)); return values; }
    protected override float[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadFloats(out float[] values)); return values; }
    protected override float[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out float[] values)); return values; }
    protected override float[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out float[] values)); return values; }
}
