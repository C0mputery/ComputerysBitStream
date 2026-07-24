namespace ComputerysBitStream.Tests.Utilities;

public abstract class ExtrasQuantizedNumericsTestSuite<T> : QuantizedExtensionTestSuite<T> {
    protected override bool SupportsOutOfBoundsTests => false;
}
