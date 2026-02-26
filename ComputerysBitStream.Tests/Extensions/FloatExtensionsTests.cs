using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class FloatExtensionsTests {
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
    public void WriteFloat_ReadFloat_RoundTrip(float value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloat(value);

        ReadContext reader = new ReadContext(buffer);
        float result = reader.ReadFloat();
        if (float.IsNaN(value)) Assert.True(float.IsNaN(result));
        else Assert.Equal(value, result);
    }

    [Fact]
    public void WriteFloat_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(3.14f);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(3.14f, reader.ReadFloat());
    }

    [Fact]
    public void PeekFloat_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloat(1.5f);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1.5f, reader.PeekFloat());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadFloat_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloat(1f);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadFloat();
        Assert.Equal(32, reader.Position);
    }

    [Fact]
    public void TryReadFloat_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloat(2.5f);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadFloat(out float value));
        Assert.Equal(2.5f, value);
    }

    [Fact]
    public void TryReadFloat_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadFloat(out float value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadFloat_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadFloat());
    }

    [Fact]
    public void WriteFloat_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteFloat(1f); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteFloats_ReadFloats_WithLengthPrefix_RoundTrip() {
        float[] values = { -1.5f, 0f, 1.5f, float.MaxValue, float.MinValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloats(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.ReadFloats();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteFloats_PeekFloats_DoesNotAdvancePosition() {
        float[] values = { 1.1f, 2.2f };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloats(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.PeekFloats();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteFloatsWithoutLength_ReadFloats_Count_RoundTrip() {
        float[] values = { -0.5f, 0f, 0.5f };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        float[] result = reader.ReadFloats(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadFloats_ReturnsTrue_WhenSpaceAvailable() {
        float[] values = { 1f, 2f };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloats(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadFloats(out float[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteFloats_ReadFloatsSpan_WithLengthPrefix_RoundTrip() {
        float[] values = { -1f, 0f, 1f };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloats(values);

        ReadContext reader = new ReadContext(buffer);
        Span<float> destination = stackalloc float[values.Length];
        reader.ReadFloats(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteFloatsWithoutLength_ReadFloatsSpan_Count_RoundTrip() {
        float[] values = { 3.14f, 2.72f };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloatsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<float> destination = stackalloc float[values.Length];
        reader.ReadFloats(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteFloat_ReadFloat_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteFloat(float.MinValue);
        writer.WriteFloat(0f);
        writer.WriteFloat(float.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(float.MinValue, reader.ReadFloat());
        Assert.Equal(0f, reader.ReadFloat());
        Assert.Equal(float.MaxValue, reader.ReadFloat());
    }
}
