namespace ComputerysBitStream.Tests;

public class BitOffsetRange : TheoryData<int> {
    public BitOffsetRange() { AddRange(Enumerable.Range(0, 16)); }
}

public class ZeroBitOffsetRange : TheoryData<int> {
    public ZeroBitOffsetRange() { Add(0); }
}
