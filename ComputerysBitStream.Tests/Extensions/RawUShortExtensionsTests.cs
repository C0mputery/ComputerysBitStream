using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawUShortExtensionsTests {
    [Theory]
    [InlineData(ushort.MaxValue)]
    [InlineData((ushort)0)]
    [InlineData((ushort)1)]
    [InlineData((ushort)12345)]
    [InlineData((ushort)32768)]
    public void WriteUShortRaw_ReadUShortRaw_RoundTrip(ushort value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadUShortRaw());
    }

    [Fact]
    public void WriteUShortRaw_PeekUShortRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortRaw(999);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((ushort)999, reader.PeekUShortRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUShortRaw_ReadUShortRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortRaw(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadUShortRaw();
        Assert.Equal(16, reader.Position);
    }

    [Fact]
    public void WriteUShortRaw_ReadUShortRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortRaw(ushort.MinValue);
        writer.WriteUShortRaw(1000);
        writer.WriteUShortRaw(ushort.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(ushort.MinValue, reader.ReadUShortRaw());
        Assert.Equal((ushort)1000, reader.ReadUShortRaw());
        Assert.Equal(ushort.MaxValue, reader.ReadUShortRaw());
    }

    [Fact]
    public void WriteUShortsRaw_ReadUShortArrayRaw_SmallCount() {
        ushort[] values = { 0, 100, 200, 65535, 32768 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.ReadUShortArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUShortsRaw_ReadUShortArrayRaw_LargeCount() {
        ushort[] values = new ushort[10];
        for (int i = 0; i < values.Length; i++) values[i] = (ushort)(i * 6553);

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.ReadUShortArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUShortsRaw_PeekUShortArrayRaw_DoesNotAdvancePosition() {
        ushort[] values = { 111, 222, 333 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.PeekUShortArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUShortsRaw_ReadUShortSpanRaw() {
        ushort[] values = { 10, 20, 30, 40, 50 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ushort> destination = stackalloc ushort[values.Length];
        reader.ReadUShortSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteUShortsRaw_PeekUShortSpanRaw_DoesNotAdvancePosition() {
        ushort[] values = { 500, 1000, 1500 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<ushort> destination = stackalloc ushort[values.Length];
        reader.PeekUShortSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUShortsRaw_ExactlyUlongBoundary() {
        ushort[] values = { 1, 2, 3, 4 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUShortsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        ushort[] result = reader.ReadUShortArrayRaw(4);
        Assert.Equal(values, result);
    }
}
