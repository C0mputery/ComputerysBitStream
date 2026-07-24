using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class ContainerStructTests : StructTestSuite<ContainerStruct> {
    protected override ContainerStruct Value => new() { RawValue = 42, Nested = new() { Value = 99 } };

    protected override ContainerStruct[] Values => [
        new() { RawValue = 1, Nested = new() { Value = 10 } },
        new() { RawValue = 2, Nested = new() { Value = 20 } },
        new() { RawValue = 3, Nested = new() { Value = 30 } }
    ];

    protected override int? ExpectedFixedSizeBits => 64;
    protected override Type StructType => typeof(ContainerStruct);

    [Fact]
    public void NestedStruct_ShouldBeFixedSize() {
        Assert.Equal(32, StructMetadataAssertions.GetMetadataSize(typeof(NestedStruct)));
        Assert.True(StructMetadataAssertions.IsFixedSize(typeof(NestedStruct)));
    }

    protected override SerializationOperations<ContainerStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, ContainerStruct value) => context.WriteContainerStruct(value),
        Peek = (ReadContext context) => context.PeekContainerStruct(),
        Read = (ReadContext context) => context.ReadContainerStruct(),
        TryPeek = (ReadContext context, out ContainerStruct value) => context.TryPeekContainerStruct(out value),
        TryRead = (ReadContext context, out ContainerStruct value) => context.TryReadContainerStruct(out value),
        WriteSpan = (ref WriteContext context, Span<ContainerStruct> values) => context.WriteContainerStructs(values),
        PeekSpan = (ReadContext context, Span<ContainerStruct> destination) => context.PeekContainerStructs(destination),
        ReadSpan = (ReadContext context, Span<ContainerStruct> destination) => context.ReadContainerStructs(destination),
        TryPeekSpan = (ReadContext context, Span<ContainerStruct> destination) => context.TryPeekContainerStructs(destination),
        TryReadSpan = (ReadContext context, Span<ContainerStruct> destination) => context.TryReadContainerStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<ContainerStruct> values) => context.WriteContainerStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<ContainerStruct> destination) => context.PeekContainerStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<ContainerStruct> destination) => context.ReadContainerStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<ContainerStruct> destination) => context.TryPeekContainerStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<ContainerStruct> destination) => context.TryReadContainerStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ContainerStruct> destination) => context.PeekContainerStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ContainerStruct> destination) => context.ReadContainerStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<ContainerStruct> destination) => context.TryPeekContainerStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<ContainerStruct> destination) => context.TryReadContainerStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, ContainerStruct[] values) => context.WriteContainerStructs(values),
        PeekArray = (ReadContext context) => context.PeekContainerStructs(),
        ReadArray = (ReadContext context) => context.ReadContainerStructs(),
        TryPeekArray = (ReadContext context, out ContainerStruct[] values) => context.TryPeekContainerStructs(out values),
        TryReadArray = (ReadContext context, out ContainerStruct[] values) => context.TryReadContainerStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, ContainerStruct[] values) => context.WriteContainerStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekContainerStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadContainerStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out ContainerStruct[] values) => context.TryPeekContainerStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out ContainerStruct[] values) => context.TryReadContainerStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekContainerStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadContainerStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out ContainerStruct[] values) => context.TryPeekContainerStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out ContainerStruct[] values) => context.TryReadContainerStructsWithMaxCount(maxCount, out values),
    };
}
