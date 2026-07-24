using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class StringStructTests : StructTestSuite<StringStruct> {
    protected override StringStruct Value => new() { Id = 42, Name = "player" };

    protected override StringStruct[] Values => [
        new() { Id = 1, Name = "alpha" },
        new() { Id = 2, Name = "" },
        new() { Id = 3, Name = "cafÃ©" },
    ];

    protected override Type StructType => typeof(StringStruct);

    protected override SerializationOperations<StringStruct> Operations { get; } = new() {
        Write = (ref WriteContext context, StringStruct value) => context.WriteStringStruct(value),
        Peek = (ReadContext context) => context.PeekStringStruct(),
        Read = (ReadContext context) => context.ReadStringStruct(),
        TryPeek = (ReadContext context, out StringStruct value) => context.TryPeekStringStruct(out value),
        TryRead = (ReadContext context, out StringStruct value) => context.TryReadStringStruct(out value),
        WriteSpan = (ref WriteContext context, Span<StringStruct> values) => context.WriteStringStructs(values),
        PeekSpan = (ReadContext context, Span<StringStruct> destination) => context.PeekStringStructs(destination),
        ReadSpan = (ReadContext context, Span<StringStruct> destination) => context.ReadStringStructs(destination),
        TryPeekSpan = (ReadContext context, Span<StringStruct> destination) => context.TryPeekStringStructs(destination),
        TryReadSpan = (ReadContext context, Span<StringStruct> destination) => context.TryReadStringStructs(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<StringStruct> values) => context.WriteStringStructsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<StringStruct> destination) => context.PeekStringStructs(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<StringStruct> destination) => context.ReadStringStructs(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<StringStruct> destination) => context.TryPeekStringStructs(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<StringStruct> destination) => context.TryReadStringStructs(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<StringStruct> destination) => context.PeekStringStructsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<StringStruct> destination) => context.ReadStringStructsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<StringStruct> destination) => context.TryPeekStringStructsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<StringStruct> destination) => context.TryReadStringStructsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, StringStruct[] values) => context.WriteStringStructs(values),
        PeekArray = (ReadContext context) => context.PeekStringStructs(),
        ReadArray = (ReadContext context) => context.ReadStringStructs(),
        TryPeekArray = (ReadContext context, out StringStruct[] values) => context.TryPeekStringStructs(out values),
        TryReadArray = (ReadContext context, out StringStruct[] values) => context.TryReadStringStructs(out values),
        WriteArrayWithoutLength = (ref WriteContext context, StringStruct[] values) => context.WriteStringStructsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekStringStructs(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadStringStructs(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out StringStruct[] values) => context.TryPeekStringStructs(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out StringStruct[] values) => context.TryReadStringStructs(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekStringStructsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadStringStructsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out StringStruct[] values) => context.TryPeekStringStructsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out StringStruct[] values) => context.TryReadStringStructsWithMaxCount(maxCount, out values),
    };
}
