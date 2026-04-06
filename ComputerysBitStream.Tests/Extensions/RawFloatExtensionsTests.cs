namespace ComputerysBitStream.Tests.Extensions;

public class RawFloatExtensionsTests {
    [Fact]
    public void WriteAndReadFloatRaw_ShouldReturnIdenticalValue() {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer);
        float valueToWrite = 12.34f;

        writeCtx.WriteFloatRaw(valueToWrite);

        ReadContext readCtx = new(buffer);
        float peekedValue = readCtx.PeekFloatRaw();
        float readValue = readCtx.ReadFloatRaw();

        Assert.Equal(valueToWrite, peekedValue);
        Assert.Equal(valueToWrite, readValue);
        Assert.Equal(writeCtx.Position, readCtx.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)] // Unaligned offset to test bit shifting
    public void WriteAndReadFloatSpanRaw_ShouldReturnIdenticalSpan(int initialOffset) {
        ulong[] buffer = new ulong[16];
        WriteContext writeCtx = new(buffer, initialOffset);
        float[] values = [1.1f, -2.2f, float.MaxValue, float.MinValue, float.NaN];

        writeCtx.WriteFloatsRaw(values);

        ReadContext readCtx = new(buffer, initialOffset);
        float[] peekedValues = readCtx.PeekFloatArrayRaw(values.Length);
        float[] readValues = new float[values.Length];
        Span<float> readSpan = readValues.AsSpan();
        readCtx.ReadFloatSpanRaw(values.Length, ref readSpan);

        Assert.Equal(values, peekedValues);
        Assert.Equal(values, readValues);
    }
}