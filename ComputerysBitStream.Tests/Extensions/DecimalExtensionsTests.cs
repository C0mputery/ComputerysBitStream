using System;
using Xunit;

namespace ComputerysBitStream.Tests.Extensions;

public class DecimalExtensionsTests : ExtensionTestSuite<decimal> {
    protected override decimal Value => 1.23m;
    protected override decimal[] Values => [1.23m, 4.56m, 1.23m, 1.23m, 4.56m];

    protected override void WriteRaw(WriteContext context, decimal value) => context.WriteDecimalRaw(value);
    protected override decimal PeekRaw(ReadContext context) => context.PeekDecimalRaw();
    protected override decimal ReadRaw(ReadContext context) => context.ReadDecimalRaw();
    protected override void Write(WriteContext context, decimal value) => context.WriteDecimal(value);
    protected override decimal Peek(ReadContext context) => context.PeekDecimal();
    protected override decimal Read(ReadContext context) => context.ReadDecimal();
    protected override void WriteAlias(WriteContext context, decimal value) => context.Write(value);
    protected override decimal PeekAlias(ReadContext context) { context.Peek(out decimal v); return v; }
    protected override decimal ReadAlias(ReadContext context) { context.Read(out decimal v); return v; }
    protected override decimal TryPeek(ReadContext context) { Assert.True(context.TryPeek(out decimal v)); return v; }
    protected override decimal TryRead(ReadContext context) { Assert.True(context.TryRead(out decimal v)); return v; }
    protected override decimal TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out decimal v)); return v; }
    protected override decimal TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out decimal v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<decimal> values) => context.WriteDecimalsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<decimal> destination) => context.PeekDecimalSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<decimal> destination) => context.ReadDecimalSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<decimal> values) => context.WriteDecimalsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<decimal> destination) => context.PeekDecimals(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<decimal> destination) => context.ReadDecimals(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<decimal> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<decimal> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<decimal> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<decimal> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<decimal> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<decimal> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<decimal> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<decimal> values) => context.WriteDecimals(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<decimal> destination) => context.PeekDecimals(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<decimal> destination) => context.ReadDecimals(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<decimal> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<decimal> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<decimal> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<decimal> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<decimal> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<decimal> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<decimal> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, decimal[] values) => context.WriteDecimalsRaw(values);
    protected override decimal[] PeekArrayRaw(ReadContext context, int count) => context.PeekDecimalArrayRaw(count);
    protected override decimal[] ReadArrayRaw(ReadContext context, int count) => context.ReadDecimalArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, decimal[] values) => context.WriteDecimalsWithoutLength(values);
    protected override decimal[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekDecimals(count);
    protected override decimal[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadDecimals(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, decimal[] values) => context.WriteWithoutLength(values);
    protected override decimal[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out decimal[] values); return values; }
    protected override decimal[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out decimal[] values); return values; }
    protected override decimal[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekDecimals(count, out decimal[] values)); return values; }
    protected override decimal[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadDecimals(count, out decimal[] values)); return values; }
    protected override decimal[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out decimal[] values)); return values; }
    protected override decimal[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out decimal[] values)); return values; }

    protected override void WriteArray(WriteContext context, decimal[] values) => context.WriteDecimals(values);
    protected override decimal[] PeekArrayWithLength(ReadContext context) => context.PeekDecimals();
    protected override decimal[] ReadArrayWithLength(ReadContext context) => context.ReadDecimals();
    protected override void WriteArrayAlias(WriteContext context, decimal[] values) => context.Write(values);
    protected override decimal[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out decimal[] values); return values; }
    protected override decimal[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out decimal[] values); return values; }
    protected override decimal[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekDecimals(out decimal[] values)); return values; }
    protected override decimal[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadDecimals(out decimal[] values)); return values; }
    protected override decimal[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out decimal[] values)); return values; }
    protected override decimal[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out decimal[] values)); return values; }
}
