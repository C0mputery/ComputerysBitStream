namespace ComputerysBitStream.Tests.Utilities;

public abstract class StructTestSuite<T> : SerializationTestSuite<T> {
    protected abstract Type StructType { get; }

    protected abstract int ExpectedMetadataSize { get; }

    [Fact]
    public void ShouldReportExpectedMetadataSize() {
        Assert.Equal(ExpectedMetadataSize, StructMetadataAssertions.GetMetadataSize(StructType));
    }
}
