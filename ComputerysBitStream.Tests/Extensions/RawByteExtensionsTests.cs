using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawByteExtensionsTests {
    [Fact]
    public void WriteByteRaw_ReadByteRaw_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByteRaw(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((byte)42, reader.ReadByteRaw());
    }

    [Theory]
    [InlineData(byte.MinValue)]
    [InlineData(byte.MaxValue)]
    [InlineData(0)]
    [InlineData(127)]
    [InlineData(128)]
    public void WriteByteRaw_ReadByteRaw_BoundaryValues(byte value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByteRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadByteRaw());
    }

    [Fact]
    public void WriteByteRaw_PeekByteRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByteRaw(99);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((byte)99, reader.PeekByteRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteByteRaw_ReadByteRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByteRaw(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadByteRaw();
        Assert.Equal(8, reader.Position);
    }

    [Fact]
    public void WriteByteRaw_ReadByteRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByteRaw(10);
        writer.WriteByteRaw(20);
        writer.WriteByteRaw(30);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((byte)10, reader.ReadByteRaw());
        Assert.Equal((byte)20, reader.ReadByteRaw());
        Assert.Equal((byte)30, reader.ReadByteRaw());
    }

    [Fact]
    public void WriteBytesRaw_ReadByteArrayRaw_SmallCount() {
        byte[] values = { 1, 2, 3, 4, 5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.ReadByteArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBytesRaw_ReadByteArrayRaw_LargeCount() {
        // 20 bytes: 8 fit in 1 ulong, exercises remainder path
        byte[] values = new byte[20];
        for (int i = 0; i < values.Length; i++) values[i] = (byte)(i * 13);

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.ReadByteArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBytesRaw_PeekByteArrayRaw_DoesNotAdvancePosition() {
        byte[] values = { 10, 20, 30 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.PeekByteArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteBytesRaw_ReadByteSpanRaw() {
        byte[] values = { 5, 10, 15, 20, 25, 30, 35 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<byte> destination = stackalloc byte[values.Length];
        reader.ReadByteSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteBytesRaw_PeekByteSpanRaw_DoesNotAdvancePosition() {
        byte[] values = { 100, 200, 255 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<byte> destination = stackalloc byte[values.Length];
        reader.PeekByteSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteBytesRaw_ExactlyUlongBoundary() {
        // 8 bytes = exactly 1 ulong worth
        byte[] values = { 1, 2, 3, 4, 5, 6, 7, 8 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.ReadByteArrayRaw(8);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByteRaw(1);
        Assert.Equal(8, writer.Position);
    }

    [Fact]
    public void WriteBytesRaw_WritePosition_AdvancesCorrectly() {
        byte[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesRaw(values);
        Assert.Equal(24, writer.Position);
    }
}
