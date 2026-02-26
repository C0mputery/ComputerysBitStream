using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class UShortExtensionsTests {
    [Theory]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData(ushort.MaxValue)]
    [InlineData((ushort)32768)]
    public void WriteUShort_ReadUShort_RoundTrip(ushort value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShort(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadUShort());
    }

    [Fact]
    public void WriteUShort_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write((ushort)42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((ushort)42, reader.ReadUShort());
    }

    [Fact]
    public void PeekUShort_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShort(999);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((ushort)999, reader.PeekUShort());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadUShort_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShort(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadUShort();
        Assert.Equal(16, reader.Position);
    }

    [Fact]
    public void TryReadUShort_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShort(1000);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadUShort(out ushort value));
        Assert.Equal((ushort)1000, value);
    }

    [Fact]
    public void TryReadUShort_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadUShort(out ushort value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadUShort_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadUShort());
    }

    [Fact]
    public void WriteUShort_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteUShort(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteUShorts_ReadUShorts_WithLengthPrefix_RoundTrip() {
        ushort[] values = { 0, 1, 32768, ushort.MaxValue };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShorts(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.ReadUShorts();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUShorts_PeekUShorts_DoesNotAdvancePosition() {
        ushort[] values = { 10, 20 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShorts(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.PeekUShorts();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUShortsWithoutLength_ReadUShorts_Count_RoundTrip() {
        ushort[] values = { 100, 200, 300 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.ReadUShorts(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadUShorts_ReturnsTrue_WhenSpaceAvailable() {
        ushort[] values = { 5, 10 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShorts(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadUShorts(out ushort[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUShorts_ReadUShortsSpan_WithLengthPrefix_RoundTrip() {
        ushort[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShorts(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ushort> destination = stackalloc ushort[values.Length];
        reader.ReadUShorts(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteUShortsWithoutLength_ReadUShortsSpan_Count_RoundTrip() {
        ushort[] values = { 7, 8 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ushort> destination = stackalloc ushort[values.Length];
        reader.ReadUShorts(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteUShort_ReadUShort_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShort(0);
        writer.WriteUShort(1000);
        writer.WriteUShort(ushort.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((ushort)0, reader.ReadUShort());
        Assert.Equal((ushort)1000, reader.ReadUShort());
        Assert.Equal(ushort.MaxValue, reader.ReadUShort());
    }
}
