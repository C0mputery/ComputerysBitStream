namespace ComputerysBitStream.Tests.Utilities;

public abstract class StructTestSuite<T> : SerializationTestSuite<T> {
    protected abstract Type StructType { get; }

    protected sealed override Type MetadataStructType => StructType;
}
