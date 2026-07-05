namespace ComputerysBitStream.Tests;

public class CustomSettingsStructTests : StructTestSuite<CustomSettingsStruct> {
    protected override CustomSettingsStruct Value => new() { B = 42 };
    protected override CustomSettingsStruct[] Values => [
        new() { B = 1 },
        new() { B = 2 },
        new() { B = 3 }
    ];
    protected override int? ExpectedFixedSizeBits => 32;
    protected override void Write(ref WriteContext context, CustomSettingsStruct value) => context.WriteCustomSettingsStruct(value);
    protected override CustomSettingsStruct Peek(ReadContext context) => context.PeekCustomSettingsStruct();
    protected override CustomSettingsStruct Read(ReadContext context) => context.ReadCustomSettingsStruct();
    protected override CustomSettingsStruct TryPeek(ReadContext context) { Assert.True(context.TryPeekCustomSettingsStruct(out CustomSettingsStruct v)); return v; }
    protected override CustomSettingsStruct TryRead(ReadContext context) { Assert.True(context.TryReadCustomSettingsStruct(out CustomSettingsStruct v)); return v; }

    protected override void WriteArray(ref WriteContext context, CustomSettingsStruct[] values) => context.WriteCustomSettingsStructs(values);
    protected override CustomSettingsStruct[] PeekArrayWithLength(ReadContext context) => context.PeekCustomSettingsStructs();
    protected override CustomSettingsStruct[] ReadArrayWithLength(ReadContext context) => context.ReadCustomSettingsStructs();
    protected override CustomSettingsStruct[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekCustomSettingsStructs(out CustomSettingsStruct[] v)); return v; }
    protected override CustomSettingsStruct[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadCustomSettingsStructs(out CustomSettingsStruct[] v)); return v; }

    protected override void WriteArrayWithoutLength(ref WriteContext context, CustomSettingsStruct[] values) => context.WriteCustomSettingsStructsWithoutLength(values);
    protected override CustomSettingsStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekCustomSettingsStructs(count);
    protected override CustomSettingsStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadCustomSettingsStructs(count);
    protected override CustomSettingsStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekCustomSettingsStructs(count, out CustomSettingsStruct[] v)); return v; }
    protected override CustomSettingsStruct[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadCustomSettingsStructs(count, out CustomSettingsStruct[] v)); return v; }

    protected override void WriteSpan(ref WriteContext context, Span<CustomSettingsStruct> values) => context.WriteCustomSettingsStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<CustomSettingsStruct> destination) => context.PeekCustomSettingsStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<CustomSettingsStruct> destination) => context.ReadCustomSettingsStructs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<CustomSettingsStruct> destination) { Assert.True(context.TryPeekCustomSettingsStructs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<CustomSettingsStruct> destination) { Assert.True(context.TryReadCustomSettingsStructs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<CustomSettingsStruct> values) => context.WriteCustomSettingsStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<CustomSettingsStruct> destination) => context.PeekCustomSettingsStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<CustomSettingsStruct> destination) => context.ReadCustomSettingsStructs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<CustomSettingsStruct> destination) { Assert.True(context.TryPeekCustomSettingsStructs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<CustomSettingsStruct> destination) { Assert.True(context.TryReadCustomSettingsStructs(count, destination)); }

    protected override Type StructType => typeof(CustomSettingsStruct);
    protected override TryReadOperationSet<CustomSettingsStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out CustomSettingsStruct v) => c.TryPeekCustomSettingsStruct(out v),
        TryReadValue = (ReadContext c, out CustomSettingsStruct v) => c.TryReadCustomSettingsStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out CustomSettingsStruct[] v) => c.TryPeekCustomSettingsStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out CustomSettingsStruct[] v) => c.TryReadCustomSettingsStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out CustomSettingsStruct[] v) => c.TryPeekCustomSettingsStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out CustomSettingsStruct[] v) => c.TryReadCustomSettingsStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<CustomSettingsStruct> d) => c.TryPeekCustomSettingsStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<CustomSettingsStruct> d) => c.TryReadCustomSettingsStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<CustomSettingsStruct> d) => c.TryPeekCustomSettingsStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<CustomSettingsStruct> d) => c.TryReadCustomSettingsStructs(count, d),
    };
}