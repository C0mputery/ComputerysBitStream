namespace ComputerysBitStream.Tests.Extensions;

public class RawCharExtensionsTests {
    [Fact]
    public void WriteAndReadCharRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        char valueToWrite = 'Z';

        writeCtx.WriteCharRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        char peekedValue = readCtx.PeekCharRaw();
        char readValue = readCtx.ReadCharRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadCharSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        char[] values = ['a', 'b', '1', '\\', '\n'];

        writeCtx.WriteCharsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        char[] peekedValues = readCtx.PeekCharArrayRaw(values.Length);
        char[] readValues = new char[values.Length];
        Span<char> readSpan = readValues.AsSpan();
        readCtx.ReadCharSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}