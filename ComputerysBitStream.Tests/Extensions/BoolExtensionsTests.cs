namespace ComputerysBitStream.Tests.Extensions;

public class BoolExtensionsTests : ExtensionTestSuite<bool> {
    protected override bool Value => true;
    protected override bool[] Values => [true, false, true, true, false];

    protected override void WriteRaw(WriteContext context, bool value) => context.WriteBoolRaw(value);
    protected override bool PeekRaw(ReadContext context) => context.PeekBoolRaw();
    protected override bool ReadRaw(ReadContext context) => context.ReadBoolRaw();
    protected override void Write(WriteContext context, bool value) => context.WriteBool(value);
    protected override bool Peek(ReadContext context) => context.PeekBool();
    protected override bool Read(ReadContext context) => context.ReadBool();
    protected override void WriteAlias(WriteContext context, bool value) => context.Write(value);
    protected override bool PeekAlias(ReadContext context) { context.Peek(out bool v); return v; }
    protected override bool ReadAlias(ReadContext context) { context.Read(out bool v); return v; }
    protected override bool TryPeek(ReadContext context) { Assert.True(context.TryPeek(out bool v)); return v; }
    protected override bool TryRead(ReadContext context) { Assert.True(context.TryRead(out bool v)); return v; }
    protected override bool TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out bool v)); return v; }
    protected override bool TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out bool v)); return v; }

    protected override void WriteSpanRaw(WriteContext context, Span<bool> values) => context.WriteBoolsRaw(values);
    protected override void PeekSpanRaw(ReadContext context, int count, ref Span<bool> destination) => context.PeekBoolSpanRaw(count, ref destination);
    protected override void ReadSpanRaw(ReadContext context, int count, ref Span<bool> destination) => context.ReadBoolSpanRaw(count, ref destination);
    protected override void WriteSpanWithoutLength(WriteContext context, Span<bool> values) => context.WriteBoolsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, ref Span<bool> destination) => context.PeekBools(count, ref destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, ref Span<bool> destination) => context.ReadBools(count, ref destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<bool> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<bool> destination) => context.Peek(count, ref destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<bool> destination) => context.Read(count, ref destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, ref Span<bool> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, ref Span<bool> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, ref Span<bool> destination) { Assert.True(context.TryPeek(count, ref destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, ref Span<bool> destination) { Assert.True(context.TryRead(count, ref destination)); }
    protected override void WriteSpan(WriteContext context, Span<bool> values) => context.WriteBools(values);
    protected override void PeekSpanWithLength(ReadContext context, ref Span<bool> destination) => context.PeekBools(ref destination);
    protected override void ReadSpanWithLength(ReadContext context, ref Span<bool> destination) => context.ReadBools(ref destination);
    protected override void WriteSpanAlias(WriteContext context, Span<bool> values) => context.Write(values);
    protected override void PeekSpanWithLengthAlias(ReadContext context, ref Span<bool> destination) => context.Peek(ref destination);
    protected override void ReadSpanWithLengthAlias(ReadContext context, ref Span<bool> destination) => context.Read(ref destination);
    protected override void TryPeekSpanWithLength(ReadContext context, ref Span<bool> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, ref Span<bool> destination) { Assert.True(context.TryRead(ref destination)); }
    protected override void TryPeekSpanWithLengthAlias(ReadContext context, ref Span<bool> destination) { Assert.True(context.TryPeek(ref destination)); }
    protected override void TryReadSpanWithLengthAlias(ReadContext context, ref Span<bool> destination) { Assert.True(context.TryRead(ref destination)); }

    protected override void WriteArrayRaw(WriteContext context, bool[] values) => context.WriteBoolsRaw(values);
    protected override bool[] PeekArrayRaw(ReadContext context, int count) => context.PeekBoolArrayRaw(count);
    protected override bool[] ReadArrayRaw(ReadContext context, int count) => context.ReadBoolArrayRaw(count);
    protected override void WriteArrayWithoutLength(WriteContext context, bool[] values) => context.WriteBoolsWithoutLength(values);
    protected override bool[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekBools(count);
    protected override bool[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadBools(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, bool[] values) => context.WriteWithoutLength(values);
    protected override bool[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out bool[] values); return values; }
    protected override bool[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out bool[] values); return values; }
    protected override bool[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekBools(count, out bool[] values)); return values; }
    protected override bool[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadBools(count, out bool[] values)); return values; }
    protected override bool[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out bool[] values)); return values; }
    protected override bool[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out bool[] values)); return values; }

    protected override void WriteArray(WriteContext context, bool[] values) => context.WriteBools(values);
    protected override bool[] PeekArrayWithLength(ReadContext context) => context.PeekBools();
    protected override bool[] ReadArrayWithLength(ReadContext context) => context.ReadBools();
    protected override void WriteArrayAlias(WriteContext context, bool[] values) => context.Write(values);
    protected override bool[] PeekArrayWithLengthAlias(ReadContext context) { context.Peek(out bool[] values); return values; }
    protected override bool[] ReadArrayWithLengthAlias(ReadContext context) { context.Read(out bool[] values); return values; }
    protected override bool[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekBools(out bool[] values)); return values; }
    protected override bool[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadBools(out bool[] values)); return values; }
    protected override bool[] TryPeekArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryPeek(out bool[] values)); return values; }
    protected override bool[] TryReadArrayWithLengthAlias(ReadContext context) { Assert.True(context.TryRead(out bool[] values)); return values; }
}
