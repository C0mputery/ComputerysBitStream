using Xunit;

namespace ComputerysBitStream.Tests;

public class ContainerStructTests : StructTestSuite<ContainerStruct> {
    protected override ContainerStruct Value => new() { RawValue = 42, Nested = new() { Value = 99 } };
    protected override ContainerStruct[] Values => [
        new() { RawValue = 1, Nested = new() { Value = 10 } },
        new() { RawValue = 2, Nested = new() { Value = 20 } },
        new() { RawValue = 3, Nested = new() { Value = 30 } }
    ];
    protected override int? ExpectedFixedSizeBits => 64;

    protected override void WriteNamed(WriteContext context, ContainerStruct value) => context.WriteContainerStruct(value);
    protected override ContainerStruct PeekNamed(ReadContext context) => context.PeekContainerStruct();
    protected override ContainerStruct ReadNamed(ReadContext context) => context.ReadContainerStruct();
    protected override void WriteAlias(WriteContext context, ContainerStruct value) => context.Write(value);
    protected override ContainerStruct PeekAlias(ReadContext context) { context.Peek(out ContainerStruct v); return v; }
    protected override ContainerStruct ReadAlias(ReadContext context) { context.Read(out ContainerStruct v); return v; }
    protected override ContainerStruct TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekContainerStruct(out ContainerStruct v)); return v; }
    protected override ContainerStruct TryReadNamed(ReadContext context) { Assert.True(context.TryReadContainerStruct(out ContainerStruct v)); return v; }
    protected override ContainerStruct TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out ContainerStruct v)); return v; }
    protected override ContainerStruct TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out ContainerStruct v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, ContainerStruct[] values) => context.WriteContainerStructs(values);
    protected override ContainerStruct[] PeekArrayNamed(ReadContext context) => context.PeekContainerStructs();
    protected override ContainerStruct[] ReadArrayNamed(ReadContext context) => context.ReadContainerStructs();
    protected override void WriteArrayAlias(WriteContext context, ContainerStruct[] values) => context.Write(values);
    protected override ContainerStruct[] PeekArrayAlias(ReadContext context) { context.Peek(out ContainerStruct[] v); return v; }
    protected override ContainerStruct[] ReadArrayAlias(ReadContext context) { context.Read(out ContainerStruct[] v); return v; }
    protected override ContainerStruct[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekContainerStructs(out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadContainerStructs(out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out ContainerStruct[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, ContainerStruct[] values) => context.WriteContainerStructsWithoutLength(values);
    protected override ContainerStruct[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekContainerStructs(count);
    protected override ContainerStruct[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadContainerStructs(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, ContainerStruct[] values) => context.WriteWithoutLength(values);
    protected override ContainerStruct[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out ContainerStruct[] v); return v; }
    protected override ContainerStruct[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out ContainerStruct[] v); return v; }
    protected override ContainerStruct[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekContainerStructs(count, out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadContainerStructs(count, out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out ContainerStruct[] v)); return v; }
    protected override ContainerStruct[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out ContainerStruct[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<ContainerStruct> values) => context.WriteContainerStructs(values);
    protected override void PeekSpanNamed(ReadContext context, Span<ContainerStruct> destination) => context.PeekContainerStructs(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<ContainerStruct> destination) => context.ReadContainerStructs(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<ContainerStruct> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<ContainerStruct> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<ContainerStruct> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<ContainerStruct> destination) { Assert.True(context.TryPeekContainerStructs(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<ContainerStruct> destination) { Assert.True(context.TryReadContainerStructs(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<ContainerStruct> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<ContainerStruct> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<ContainerStruct> values) => context.WriteContainerStructsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<ContainerStruct> destination) => context.PeekContainerStructs(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<ContainerStruct> destination) => context.ReadContainerStructs(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<ContainerStruct> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<ContainerStruct> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<ContainerStruct> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<ContainerStruct> destination) { Assert.True(context.TryPeekContainerStructs(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<ContainerStruct> destination) { Assert.True(context.TryReadContainerStructs(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<ContainerStruct> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<ContainerStruct> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(ContainerStruct value) => value.GetContainerStructSizeInBits();
    protected override bool IsFixedSizeStruct(ContainerStruct value) => value.IsContainerStructFixedSizeStruct();

    [Fact]
    public void NestedStruct_ShouldBeFixedSize() {
        NestedStruct nested = new() { Value = 123 };
        int size = nested.GetNestedStructSizeInBits();
        Assert.Equal(32, size);
        Assert.True(nested.IsNestedStructFixedSizeStruct());
    }
}
