namespace ComputerysBitStream.Tests;

public class ContainerStructTests : StructTestSuite<ContainerStruct> {
    protected override ContainerStruct Value => new() { RawValue = 42, Nested = new() { Value = 99 } };
    protected override ContainerStruct[] Values => [
        new() { RawValue = 1, Nested = new() { Value = 10 } },
        new() { RawValue = 2, Nested = new() { Value = 20 } },
        new() { RawValue = 3, Nested = new() { Value = 30 } }
    ];
    protected override int? ExpectedFixedSizeBits => 64;
    protected override void Write(ref WriteContext context, ContainerStruct value) => context.WriteContainerStruct(value);
    protected override ContainerStruct Peek(ReadContext context) => context.PeekContainerStruct();
    protected override ContainerStruct Read(ReadContext context) => context.ReadContainerStruct();
    protected override ContainerStruct TryPeek(ReadContext context) { Assert.True(context.TryPeekContainerStruct(out ContainerStruct v)); return v; }
    protected override ContainerStruct TryRead(ReadContext context) { Assert.True(context.TryReadContainerStruct(out ContainerStruct v)); return v; }

    protected override void WriteArray(ref WriteContext context, ContainerStruct[] values) => context.WriteContainerStructs(values);
    protected override ContainerStruct[] PeekArrayWithLength(ReadContext context) => context.PeekContainerStructs();
    protected override ContainerStruct[] ReadArrayWithLength(ReadContext context) => context.ReadContainerStructs();
    protected override ContainerStruct[] TryPeekArrayWithLength(ReadContext context) { Assert.True(context.TryPeekContainerStructs(out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryReadArrayWithLength(ReadContext context) { Assert.True(context.TryReadContainerStructs(out ContainerStruct[] v)); return v; }

    protected override void WriteArrayWithoutLength(ref WriteContext context, ContainerStruct[] values) => context.WriteContainerStructsWithoutLength(values);
    protected override ContainerStruct[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekContainerStructs(count);
    protected override ContainerStruct[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadContainerStructs(count);
    protected override ContainerStruct[] TryPeekArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryPeekContainerStructs(count, out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryReadArrayWithoutLength(ReadContext context, int count) { Assert.True(context.TryReadContainerStructs(count, out ContainerStruct[] v)); return v; }

    protected override void WriteSpan(ref WriteContext context, Span<ContainerStruct> values) => context.WriteContainerStructs(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<ContainerStruct> destination) => context.PeekContainerStructs(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<ContainerStruct> destination) => context.ReadContainerStructs(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<ContainerStruct> destination) { Assert.True(context.TryPeekContainerStructs(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<ContainerStruct> destination) { Assert.True(context.TryReadContainerStructs(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<ContainerStruct> values) => context.WriteContainerStructsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<ContainerStruct> destination) => context.PeekContainerStructs(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<ContainerStruct> destination) => context.ReadContainerStructs(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<ContainerStruct> destination) { Assert.True(context.TryPeekContainerStructs(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<ContainerStruct> destination) { Assert.True(context.TryReadContainerStructs(count, destination)); }

    protected override Type StructType => typeof(ContainerStruct);

    [Fact]
    public void NestedStruct_ShouldBeFixedSize() {
        Assert.Equal(32, StructMetadataAssertions.GetMetadataSize(typeof(NestedStruct)));
        Assert.True(StructMetadataAssertions.IsFixedSize(typeof(NestedStruct)));
    }
    protected override TryReadOperationSet<ContainerStruct> TryOperations => new() {
        TryPeekValue = (ReadContext c, out ContainerStruct v) => c.TryPeekContainerStruct(out v),
        TryReadValue = (ReadContext c, out ContainerStruct v) => c.TryReadContainerStruct(out v),
        TryPeekArrayWithLength = (ReadContext c, out ContainerStruct[] v) => c.TryPeekContainerStructs(out v),
        TryReadArrayWithLength = (ReadContext c, out ContainerStruct[] v) => c.TryReadContainerStructs(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out ContainerStruct[] v) => c.TryPeekContainerStructs(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out ContainerStruct[] v) => c.TryReadContainerStructs(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<ContainerStruct> d) => c.TryPeekContainerStructs(d),
        TryReadSpanWithLength = (ReadContext c, Span<ContainerStruct> d) => c.TryReadContainerStructs(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<ContainerStruct> d) => c.TryPeekContainerStructs(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<ContainerStruct> d) => c.TryReadContainerStructs(count, d),
    };
}