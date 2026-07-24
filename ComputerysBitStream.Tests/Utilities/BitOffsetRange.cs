namespace ComputerysBitStream.Tests.Utilities;

public class BitOffsetRange : TheoryData<int> {
    public BitOffsetRange() { AddRange(Enumerable.Range(0, 128)); }
}

public sealed class ZeroBitOffsetRange : TheoryData<int> {
    public ZeroBitOffsetRange() { Add(0); }
}
