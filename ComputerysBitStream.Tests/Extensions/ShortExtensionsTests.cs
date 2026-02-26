using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class ShortExtensionsTests {
    [Theory]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-1)]
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue)]
    public void WriteShort_ReadShort_RoundTrip(short value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShort(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadShort());
    }

    [Fact]
    public void WriteShort_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write((short)-42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((short)-42, reader.ReadShort());
    }

    [Fact]
    public void PeekShort_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShort(999);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((short)999, reader.PeekShort());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadShort_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShort(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadShort();
        Assert.Equal(16, reader.Position);
    }

    [Fact]
    public void TryReadShort_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShort(-100);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadShort(out short value));
        Assert.Equal((short)-100, value);
    }

    [Fact]
    public void TryReadShort_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadShort(out short value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadShort_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadShort());
    }

    [Fact]
    public void WriteShort_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteShort(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteShorts_ReadShorts_WithLengthPrefix_RoundTrip() {
        short[] values = { short.MinValue, -1, 0, 1, short.MaxValue };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShorts(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.ReadShorts();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteShorts_PeekShorts_DoesNotAdvancePosition() {
        short[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShorts(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.PeekShorts();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteShortsWithoutLength_ReadShorts_Count_RoundTrip() {
        short[] values = { -500, 0, 500 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.ReadShorts(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadShorts_ReturnsTrue_WhenSpaceAvailable() {
        short[] values = { -10, 10 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShorts(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadShorts(out short[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteShorts_ReadShortsSpan_WithLengthPrefix_RoundTrip() {
        short[] values = { -1, 0, 1 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShorts(values);

        ReadContext reader = new ReadContext(buffer);
        Span<short> destination = stackalloc short[values.Length];
        reader.ReadShorts(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteShortsWithoutLength_ReadShortsSpan_Count_RoundTrip() {
        short[] values = { 100, -100 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<short> destination = stackalloc short[values.Length];
        reader.ReadShorts(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteShort_ReadShort_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShort(short.MinValue);
        writer.WriteShort(0);
        writer.WriteShort(short.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(short.MinValue, reader.ReadShort());
        Assert.Equal((short)0, reader.ReadShort());
        Assert.Equal(short.MaxValue, reader.ReadShort());
    }
}
