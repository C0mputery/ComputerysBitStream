using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawULongExtensionsTests {
    [Theory]
    [InlineData(ulong.MaxValue)]
    [InlineData(0UL)]
    [InlineData(1UL)]
    [InlineData(9999999999999UL)]
    [InlineData(9223372036854775808UL)]
    public void WriteULongRaw_ReadULongRaw_RoundTrip(ulong value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadULongRaw());
    }

    [Fact]
    public void WriteULongRaw_PeekULongRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongRaw(999999UL);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(999999UL, reader.PeekULongRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteULongRaw_ReadULongRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongRaw(1UL);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadULongRaw();
        Assert.Equal(64, reader.Position);
    }

    [Fact]
    public void WriteULongRaw_ReadULongRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongRaw(ulong.MinValue);
        writer.WriteULongRaw(42UL);
        writer.WriteULongRaw(ulong.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(ulong.MinValue, reader.ReadULongRaw());
        Assert.Equal(42UL, reader.ReadULongRaw());
        Assert.Equal(ulong.MaxValue, reader.ReadULongRaw());
    }

    [Fact]
    public void WriteULongsRaw_ReadULongArrayRaw() {
        ulong[] values = { ulong.MinValue, 1UL, 42UL, ulong.MaxValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        ulong[] result = reader.ReadULongArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteULongsRaw_PeekULongArrayRaw_DoesNotAdvancePosition() {
        ulong[] values = { 111UL, 222UL, 333UL };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        ulong[] result = reader.PeekULongArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteULongsRaw_ReadULongSpanRaw() {
        ulong[] values = { 1UL, 2UL, 3UL };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ulong> destination = stackalloc ulong[values.Length];
        reader.ReadULongSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteULongsRaw_PeekULongSpanRaw_DoesNotAdvancePosition() {
        ulong[] values = { 500UL, 1000UL, 1500UL };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ulong> destination = stackalloc ulong[values.Length];
        reader.PeekULongSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteULongRaw(42UL);
        Assert.Equal(64, writer.Position);
    }
}
