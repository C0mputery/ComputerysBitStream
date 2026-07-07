namespace ComputerysBitStream.Tests;

public abstract class QuantizedExtensionTestSuite<T> : PrimitiveSerializationTestSuite<T> {
    protected abstract int Precision { get; }

    public new static IEnumerable<object[]> InitialOffsetData() => [[0]];
}
