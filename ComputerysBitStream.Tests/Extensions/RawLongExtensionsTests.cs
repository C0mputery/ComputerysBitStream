using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawLongExtensionsTests {
    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    [InlineData(123456789012345L)]
    [InlineData(-123456789012345L)]
    public void WriteLongRaw_ReadLongRaw_RoundTrip(long value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadLongRaw());
    }

    [Fact]
    public void WriteLongRaw_PeekLongRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongRaw(-999999L);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(-999999L, reader.PeekLongRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteLongRaw_ReadLongRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongRaw(1L);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadLongRaw();
        Assert.Equal(64, reader.Position);
    }

    [Fact]
    public void WriteLongRaw_ReadLongRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongRaw(long.MinValue);
        writer.WriteLongRaw(0L);
        writer.WriteLongRaw(long.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(long.MinValue, reader.ReadLongRaw());
        Assert.Equal(0L, reader.ReadLongRaw());
        Assert.Equal(long.MaxValue, reader.ReadLongRaw());
    }

    [Fact]
    public void WriteLongsRaw_ReadLongArrayRaw() {
        long[] values = { long.MinValue, -1L, 0L, 1L, long.MaxValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        long[] result = reader.ReadLongArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteLongsRaw_PeekLongArrayRaw_DoesNotAdvancePosition() {
        long[] values = { 111L, -222L, 333L };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        long[] result = reader.PeekLongArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteLongsRaw_ReadLongSpanRaw() {
        long[] values = { 1L, -2L, 3L };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<long> destination = stackalloc long[values.Length];
        reader.ReadLongSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteLongsRaw_PeekLongSpanRaw_DoesNotAdvancePosition() {
        long[] values = { 500L, -500L, 0L };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<long> destination = stackalloc long[values.Length];
        reader.PeekLongSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteLongRaw(42L);
        Assert.Equal(64, writer.Position);
    }
}
