using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawFloatExtensionsTests {
    [Theory]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    [InlineData(float.Epsilon)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(3.14159f)]
    [InlineData(-2.71828f)]
    public void WriteFloatRaw_ReadFloatRaw_RoundTrip(float value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatRaw(value);

        ReadContext reader = new ReadContext(buffer);
        float result = reader.ReadFloatRaw();
        if (float.IsNaN(value))
            Assert.True(float.IsNaN(result));
        else
            Assert.Equal(value, result);
    }

    [Fact]
    public void WriteFloatRaw_PeekFloatRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatRaw(3.14f);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(3.14f, reader.PeekFloatRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteFloatRaw_ReadFloatRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatRaw(1f);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadFloatRaw();
        Assert.Equal(32, reader.Position);
    }

    [Fact]
    public void WriteFloatRaw_ReadFloatRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatRaw(1.1f);
        writer.WriteFloatRaw(2.2f);
        writer.WriteFloatRaw(3.3f);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1.1f, reader.ReadFloatRaw());
        Assert.Equal(2.2f, reader.ReadFloatRaw());
        Assert.Equal(3.3f, reader.ReadFloatRaw());
    }

    [Fact]
    public void WriteFloatsRaw_ReadFloatArrayRaw_SmallCount() {
        float[] values = { -1.5f, 0f, 1.5f, float.MinValue, float.MaxValue };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.ReadFloatArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteFloatsRaw_ReadFloatArrayRaw_LargeCount() {
        // 5 floats: 2 per ulong, 1 remainder
        float[] values = new float[5];
        for (int i = 0; i < values.Length; i++) values[i] = i * 1.111f;

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.ReadFloatArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteFloatsRaw_PeekFloatArrayRaw_DoesNotAdvancePosition() {
        float[] values = { 1.1f, 2.2f, 3.3f };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.PeekFloatArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteFloatsRaw_ReadFloatSpanRaw() {
        float[] values = { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<float> destination = stackalloc float[values.Length];
        reader.ReadFloatSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteFloatsRaw_PeekFloatSpanRaw_DoesNotAdvancePosition() {
        float[] values = { 10.5f, 20.5f, 30.5f };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<float> destination = stackalloc float[values.Length];
        reader.PeekFloatSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteFloatsRaw_ExactlyUlongBoundary() {
        // 2 floats = exactly 1 ulong (2 * 32 = 64)
        float[] values = { 1.23f, -4.56f };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.ReadFloatArrayRaw(2);
        Assert.Equal(values, result);
    }
}
