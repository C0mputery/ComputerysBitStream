using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawShortExtensionsTests {
    [Theory]
    [InlineData((short)0)]
    [InlineData((short)1)]
    [InlineData((short)-1)]
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue)]
    [InlineData((short)12345)]
    [InlineData((short)-12345)]
    public void WriteShortRaw_ReadShortRaw_RoundTrip(short value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadShortRaw());
    }

    [Fact]
    public void WriteShortRaw_PeekShortRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortRaw(-999);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((short)-999, reader.PeekShortRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteShortRaw_ReadShortRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortRaw(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadShortRaw();
        Assert.Equal(16, reader.Position);
    }

    [Fact]
    public void WriteShortRaw_ReadShortRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortRaw(short.MinValue);
        writer.WriteShortRaw(0);
        writer.WriteShortRaw(short.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(short.MinValue, reader.ReadShortRaw());
        Assert.Equal((short)0, reader.ReadShortRaw());
        Assert.Equal(short.MaxValue, reader.ReadShortRaw());
    }

    [Fact]
    public void WriteShortsRaw_ReadShortArrayRaw_SmallCount() {
        short[] values = { -100, 0, 100, short.MinValue, short.MaxValue };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.ReadShortArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteShortsRaw_ReadShortArrayRaw_LargeCount() {
        // 10 shorts: 4 per ulong, exercises remainder
        short[] values = new short[10];
        for (int i = 0; i < values.Length; i++) values[i] = (short)(i * 1000 - 5000);

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.ReadShortArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteShortsRaw_PeekShortArrayRaw_DoesNotAdvancePosition() {
        short[] values = { 111, -222, 333 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.PeekShortArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteShortsRaw_ReadShortSpanRaw() {
        short[] values = { 1, -2, 3, -4, 5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<short> destination = stackalloc short[values.Length];
        reader.ReadShortSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteShortsRaw_PeekShortSpanRaw_DoesNotAdvancePosition() {
        short[] values = { 500, -500, 0 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<short> destination = stackalloc short[values.Length];
        reader.PeekShortSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteShortsRaw_ExactlyUlongBoundary() {
        // 4 shorts = exactly 1 ulong (4 * 16 = 64)
        short[] values = { 1, -2, 3, -4 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        short[] result = reader.ReadShortArrayRaw(4);
        Assert.Equal(values, result);
    }
}
