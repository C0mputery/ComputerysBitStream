using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawIntExtensionsTests {
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(123456789)]
    [InlineData(-123456789)]
    public void WriteIntRaw_ReadIntRaw_RoundTrip(int value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadIntRaw());
    }

    [Fact]
    public void WriteIntRaw_PeekIntRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntRaw(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42, reader.PeekIntRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteIntRaw_ReadIntRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntRaw(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadIntRaw();
        Assert.Equal(32, reader.Position);
    }

    [Fact]
    public void WriteIntRaw_ReadIntRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntRaw(int.MinValue);
        writer.WriteIntRaw(0);
        writer.WriteIntRaw(int.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(int.MinValue, reader.ReadIntRaw());
        Assert.Equal(0, reader.ReadIntRaw());
        Assert.Equal(int.MaxValue, reader.ReadIntRaw());
    }

    [Fact]
    public void WriteIntsRaw_ReadIntArrayRaw_SmallCount() {
        int[] values = { -100, 0, 100, int.MinValue, int.MaxValue };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.ReadIntArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteIntsRaw_ReadIntArrayRaw_LargeCount() {
        // 5 ints: 2 per ulong, 1 remainder
        int[] values = new int[5];
        for (int i = 0; i < values.Length; i++) values[i] = i * 100000 - 200000;

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.ReadIntArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteIntsRaw_PeekIntArrayRaw_DoesNotAdvancePosition() {
        int[] values = { 111, -222, 333 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.PeekIntArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteIntsRaw_ReadIntSpanRaw() {
        int[] values = { 1, -2, 3, -4, 5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        reader.ReadIntSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteIntsRaw_PeekIntSpanRaw_DoesNotAdvancePosition() {
        int[] values = { 500, -500, 0 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        reader.PeekIntSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteIntsRaw_ExactlyUlongBoundary() {
        // 2 ints = exactly 1 ulong (2 * 32 = 64)
        int[] values = { 12345, -67890 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.ReadIntArrayRaw(2);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntRaw(42);
        Assert.Equal(32, writer.Position);
    }
}
