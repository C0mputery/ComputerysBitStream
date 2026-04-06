using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class DoubleExtensionsTests : ExtensionTestSuite<double> {
    protected override double Value => 1.23;
    protected override double[] Values => [1.23, 4.56, 1.23, 1.23, 4.56];

    protected override void WriteRaw(WriteContext context, double value) => context.WriteDoubleRaw(value);
    protected override double PeekRaw(ReadContext context) => context.PeekDoubleRaw();
    protected override double ReadRaw(ReadContext context) => context.ReadDoubleRaw();
    protected override void Write(WriteContext context, double value) => context.WriteDouble(value);
    protected override double Peek(ReadContext context) => context.PeekDouble();
    protected override double Read(ReadContext context) => context.ReadDouble();
    protected override void WriteAlias(WriteContext context, double value) => context.Write(value);
    protected override double PeekAlias(ReadContext context) { context.Peek(out double v); return v; }
    protected override double ReadAlias(ReadContext context) { context.Read(out double v); return v; }
    protected override double TryPeek(ReadContext context) { Assert.True(context.TryPeek(out double v)); return v; }
    protected override double TryRead(ReadContext context) { Assert.True(context.TryRead(out double v)); return v; }
    protected override double TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out double v)); return v; }
    protected override double TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out double v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<double> values) => context.WriteDoublesRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<double> destination) => context.PeekDoubleSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<double> destination) => context.ReadDoubleSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<double> values) => context.WriteDoublesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<double> destination) => context.PeekDoubles(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<double> destination) => context.ReadDoubles(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<double> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<double> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<double> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<double> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<double> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<double> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<double> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<double> values) => context.WriteDoubles(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<double> destination) => context.PeekDoubles(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<double> destination) => context.ReadDoubles(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<double> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<double> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<double> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<double> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<double> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<double> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<double> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, double[] values) => context.WriteDoublesRaw(values);
    protected override double[] PeekArrayRaw(ReadContext context, int count) => context.PeekDoubleArrayRaw(count);
    protected override double[] ReadArrayRaw(ReadContext context, int count) => context.ReadDoubleArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, double[] values) => context.WriteDoublesWithoutLength(values);
    protected override double[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekDoubles(count);
    protected override double[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadDoubles(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, double[] values) => context.WriteWithoutLength(values);
    protected override double[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out double[] values); return values; }
    protected override double[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out double[] values); return values; }
    protected override double[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekDoubles(count, out double[] values)); return values; }
    protected override double[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadDoubles(count, out double[] values)); return values; }
    protected override double[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out double[] values)); return values; }
    protected override double[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out double[] values)); return values; }

    protected override void WriteArray(WriteContext context, double[] values) => context.WriteDoubles(values);
    protected override double[] PeekArrayWithLength(ReadContext context) => context.PeekDoubles();
    protected override double[] ReadArrayWithLength(ReadContext context) => context.ReadDoubles();
    protected override void WriteArrayAlias(WriteContext context, double[] values) => context.Write(values);
    protected override double[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out double[] values); return values; }
    protected override double[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out double[] values); return values; }
    protected override double[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekDoubles(out double[] values)); return values; }
    protected override double[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadDoubles(out double[] values)); return values; }
    protected override double[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out double[] values)); return values; }
    protected override double[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out double[] values)); return values; }
}
