using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class ULongExtensionsTests {
    [Theory]
    [InlineData(0UL)]
    [InlineData(1UL)]
    [InlineData(ulong.MaxValue)]
    [InlineData(9223372036854775808UL)]
    public void WriteULong_ReadULong_RoundTrip(ulong value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULong(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadULong());
    }

    [Fact]
    public void WriteULong_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(42UL);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42UL, reader.ReadULong());
    }

    [Fact]
    public void PeekULong_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULong(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42UL, reader.PeekULong());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadULong_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULong(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadULong();
        Assert.Equal(64, reader.Position);
    }

    [Fact]
    public void TryReadULong_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULong(999);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadULong(out ulong value));
        Assert.Equal(999UL, value);
    }

    [Fact]
    public void TryReadULong_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadULong(out ulong value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadULong_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadULong());
    }

    [Fact]
    public void WriteULong_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteULong(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteULongs_ReadULongs_WithLengthPrefix_RoundTrip() {
        ulong[] values = { 0, 1, ulong.MaxValue, 9223372036854775808 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongs(values);

        ReadContext reader = new ReadContext(buffer);
        ulong[] result = reader.ReadULongs();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteULongs_PeekULongs_DoesNotAdvancePosition() {
        ulong[] values = { 10, 20 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongs(values);

        ReadContext reader = new ReadContext(buffer);
        ulong[] result = reader.PeekULongs();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteULongsWithoutLength_ReadULongs_Count_RoundTrip() {
        ulong[] values = { 100, 200, 300 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        ulong[] result = reader.ReadULongs(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadULongs_ReturnsTrue_WhenSpaceAvailable() {
        ulong[] values = { 5, 10 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongs(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadULongs(out ulong[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteULongs_ReadULongsSpan_WithLengthPrefix_RoundTrip() {
        ulong[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongs(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ulong> destination = stackalloc ulong[values.Length];
        reader.ReadULongs(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteULongsWithoutLength_ReadULongsSpan_Count_RoundTrip() {
        ulong[] values = { 7, 8 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ulong> destination = stackalloc ulong[values.Length];
        reader.ReadULongs(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteULong_ReadULong_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULong(0);
        writer.WriteULong(ulong.MaxValue / 2);
        writer.WriteULong(ulong.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(0UL, reader.ReadULong());
        Assert.Equal(ulong.MaxValue / 2, reader.ReadULong());
        Assert.Equal(ulong.MaxValue, reader.ReadULong());
    }
}
