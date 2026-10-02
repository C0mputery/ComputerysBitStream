namespace ComputerysBitStream.Tests.Utilities;

public abstract class VariableLengthExtensionTestSuite<T> : PrimitiveSerializationTestSuite<T> {
    protected abstract int GetSize(T value);

    [Fact]
    public void Size_ShouldMatchActualBitsWritten() {
        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext context = new(buffer);
        Operations.Write(ref context, Value);
        Assert.Equal(GetSize(Value), context.Position);
    }
}
