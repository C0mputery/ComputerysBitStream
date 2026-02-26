using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class SByteExtensionsTests {
    [Theory]
    [InlineData((sbyte)0)]
    [InlineData((sbyte)1)]
    [InlineData((sbyte)-1)]
    [InlineData(sbyte.MinValue)]
    [InlineData(sbyte.MaxValue)]
    public void WriteSByte_ReadSByte_RoundTrip(sbyte value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByte(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadSByte());
    }

    [Fact]
    public void WriteSByte_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write((sbyte)-42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((sbyte)-42, reader.ReadSByte());
    }

    [Fact]
    public void PeekSByte_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByte(-1);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((sbyte)-1, reader.PeekSByte());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadSByte_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByte(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadSByte();
        Assert.Equal(8, reader.Position);
    }

    [Fact]
    public void TryReadSByte_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByte(-100);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadSByte(out sbyte value));
        Assert.Equal((sbyte)-100, value);
    }

    [Fact]
    public void TryReadSByte_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadSByte(out sbyte value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadSByte_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadSByte());
    }

    [Fact]
    public void WriteSByte_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteSByte(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteSBytes_ReadSBytes_WithLengthPrefix_RoundTrip() {
        sbyte[] values = { -128, -1, 0, 1, 127 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytes(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.ReadSBytes();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteSBytes_PeekSBytes_DoesNotAdvancePosition() {
        sbyte[] values = { -1, 0, 1 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytes(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.PeekSBytes();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteSBytesWithoutLength_ReadSBytes_Count_RoundTrip() {
        sbyte[] values = { -50, 0, 50 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.ReadSBytes(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadSBytes_ReturnsTrue_WhenSpaceAvailable() {
        sbyte[] values = { -10, 10 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytes(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadSBytes(out sbyte[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadSBytes_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadSBytes(out sbyte[] result));
        Assert.Empty(result);
    }

    [Fact]
    public void WriteSBytes_ReadSBytesSpan_WithLengthPrefix_RoundTrip() {
        sbyte[] values = { -5, 0, 5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytes(values);

        ReadContext reader = new ReadContext(buffer);
        Span<sbyte> destination = stackalloc sbyte[values.Length];
        reader.ReadSBytes(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteSBytesWithoutLength_ReadSBytesSpan_Count_RoundTrip() {
        sbyte[] values = { -3, 0, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<sbyte> destination = stackalloc sbyte[values.Length];
        reader.ReadSBytes(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteSByte_ReadSByte_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByte(sbyte.MinValue);
        writer.WriteSByte(0);
        writer.WriteSByte(sbyte.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(sbyte.MinValue, reader.ReadSByte());
        Assert.Equal((sbyte)0, reader.ReadSByte());
        Assert.Equal(sbyte.MaxValue, reader.ReadSByte());
    }
}
