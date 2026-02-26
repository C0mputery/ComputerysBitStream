using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class LongExtensionsTests {
    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    [InlineData(1234567890123456789L)]
    public void WriteLong_ReadLong_RoundTrip(long value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLong(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadLong());
    }

    [Fact]
    public void WriteLong_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(42L);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42L, reader.ReadLong());
    }

    [Fact]
    public void PeekLong_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLong(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42L, reader.PeekLong());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadLong_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLong(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadLong();
        Assert.Equal(64, reader.Position);
    }

    [Fact]
    public void TryReadLong_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLong(-999);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadLong(out long value));
        Assert.Equal(-999L, value);
    }

    [Fact]
    public void TryReadLong_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadLong(out long value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadLong_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadLong());
    }

    [Fact]
    public void WriteLong_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteLong(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteLongs_ReadLongs_WithLengthPrefix_RoundTrip() {
        long[] values = { long.MinValue, -1, 0, 1, long.MaxValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongs(values);

        ReadContext reader = new ReadContext(buffer);
        long[] result = reader.ReadLongs();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteLongs_PeekLongs_DoesNotAdvancePosition() {
        long[] values = { 1, -1 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongs(values);

        ReadContext reader = new ReadContext(buffer);
        long[] result = reader.PeekLongs();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteLongsWithoutLength_ReadLongs_Count_RoundTrip() {
        long[] values = { -100, 0, 100 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        long[] result = reader.ReadLongs(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadLongs_ReturnsTrue_WhenSpaceAvailable() {
        long[] values = { 10, -10 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongs(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadLongs(out long[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteLongs_ReadLongsSpan_WithLengthPrefix_RoundTrip() {
        long[] values = { -1, 0, 1 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongs(values);

        ReadContext reader = new ReadContext(buffer);
        Span<long> destination = stackalloc long[values.Length];
        reader.ReadLongs(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteLongsWithoutLength_ReadLongsSpan_Count_RoundTrip() {
        long[] values = { 5, -5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<long> destination = stackalloc long[values.Length];
        reader.ReadLongs(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteLong_ReadLong_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLong(long.MinValue);
        writer.WriteLong(0);
        writer.WriteLong(long.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(long.MinValue, reader.ReadLong());
        Assert.Equal(0L, reader.ReadLong());
        Assert.Equal(long.MaxValue, reader.ReadLong());
    }
}
