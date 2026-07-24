namespace ComputerysBitStream.Tests.Utilities;

public abstract class VariableLengthExtensionTestSuite<T> : PrimitiveSerializationTestSuite<T> {
    protected abstract int GetSize(T value);

    protected sealed override int? GetEncodedSize(T value) => GetSize(value);
}
