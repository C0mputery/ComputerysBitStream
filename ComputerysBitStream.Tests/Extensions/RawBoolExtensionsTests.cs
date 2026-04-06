namespace ComputerysBitStream.Tests.Extensions;

public class RawBoolExtensionsTests {
    [Fact]
    public void WriteAndReadBoolRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        bool valueToWrite = true;

        writeCtx.WriteBoolRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        bool peekedValue = readCtx.PeekBoolRaw();
        bool readValue = readCtx.ReadBoolRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadBoolSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        bool[] values = [true, false, true, true, false];

        writeCtx.WriteBoolsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        bool[] peekedValues = readCtx.PeekBoolArrayRaw(values.Length);
        bool[] readValues = new bool[values.Length];
        Span<bool> readSpan = readValues.AsSpan();
        readCtx.ReadBoolSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}