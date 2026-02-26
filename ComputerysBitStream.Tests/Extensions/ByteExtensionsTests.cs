using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class ByteExtensionsTests {
    // ── Scalar Write / Read round-trip ──────────────────────────────

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    [InlineData(byte.MaxValue)]
    [InlineData((byte)127)]
    [InlineData((byte)128)]
    public void WriteByte_ReadByte_RoundTrip(byte value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadByte());
    }

    [Fact]
    public void WriteByte_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write((byte)42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((byte)42, reader.ReadByte());
    }

    [Fact]
    public void ReadByte_OutOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(99);

        ReadContext reader = new ReadContext(buffer);
        reader.Read(out byte value);
        Assert.Equal((byte)99, value);
    }

    // ── Scalar Peek ─────────────────────────────────────────────────

    [Fact]
    public void PeekByte_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((byte)42, reader.PeekByte());
        Assert.Equal(0, reader.Position);
    }

    // ── Scalar Read advances position ───────────────────────────────

    [Fact]
    public void ReadByte_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadByte();
        Assert.Equal(8, reader.Position);
    }

    // ── TryPeek / TryRead scalar ────────────────────────────────────

    [Fact]
    public void TryPeekByte_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(55);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekByte(out byte value));
        Assert.Equal((byte)55, value);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryPeekByte_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryPeekByte(out byte value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryReadByte_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(77);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadByte(out byte value));
        Assert.Equal((byte)77, value);
        Assert.Equal(8, reader.Position);
    }

    [Fact]
    public void TryReadByte_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadByte(out byte value));
        Assert.Equal(default, value);
    }

    // ── Scalar insufficient space behavior ──────────────────────────

    [Fact]
    public void ReadByte_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadByte());
    }

    [Fact]
    public void WriteByte_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteByte(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    // ── Write position advancement ──────────────────────────────────

    [Fact]
    public void WriteByte_AdvancesWritePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(1);
        Assert.Equal(8, writer.Position);
    }

    // ── Array with length prefix ────────────────────────────────────

    [Fact]
    public void WriteBytes_ReadBytes_WithLengthPrefix_RoundTrip() {
        byte[] values = { 0, 1, 127, 128, 255 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.ReadBytes();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBytes_PeekBytes_WithLengthPrefix_DoesNotAdvancePosition() {
        byte[] values = { 10, 20, 30 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.PeekBytes();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteBytes_ReadBytes_AdvancesPosition() {
        byte[] values = { 10, 20, 30 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadBytes();
        Assert.Equal(32 + 3 * 8, reader.Position);
    }

    [Fact]
    public void WriteBytes_EmptyArray_RoundTrip() {
        byte[] values = Array.Empty<byte>();
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.ReadBytes();
        Assert.Empty(result);
    }

    // ── TryRead / TryPeek array with length prefix ──────────────────

    [Fact]
    public void TryReadBytes_ReturnsTrue_WhenSpaceAvailable() {
        byte[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadBytes(out byte[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadBytes_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadBytes(out byte[] result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryPeekBytes_ReturnsTrue_WhenSpaceAvailable() {
        byte[] values = { 5, 10 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekBytes(out byte[] result));
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    // ── Array without length prefix ─────────────────────────────────

    [Fact]
    public void WriteBytesWithoutLength_ReadBytes_Count_RoundTrip() {
        byte[] values = { 0, 128, 255 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        byte[] result = reader.ReadBytes(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadBytes_Count_ReturnsTrue_WhenSpaceAvailable() {
        byte[] values = { 10, 20 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadBytes(values.Length, out byte[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void ReadBytes_Count_NegativeCount_ReturnsEmpty() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        Assert.Empty(reader.ReadBytes(-1));
    }

    // ── Span with length prefix ─────────────────────────────────────

    [Fact]
    public void WriteBytes_ReadBytesSpan_WithLengthPrefix_RoundTrip() {
        byte[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        Span<byte> destination = stackalloc byte[values.Length];
        reader.ReadBytes(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteBytes_PeekBytesSpan_WithLengthPrefix_DoesNotAdvancePosition() {
        byte[] values = { 4, 5, 6 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytes(values);

        ReadContext reader = new ReadContext(buffer);
        Span<byte> destination = stackalloc byte[values.Length];
        reader.PeekBytes(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    // ── Span without length prefix ──────────────────────────────────

    [Fact]
    public void WriteBytesWithoutLength_ReadBytesSpan_Count_RoundTrip() {
        byte[] values = { 7, 8, 9 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBytesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<byte> destination = stackalloc byte[values.Length];
        reader.ReadBytes(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    // ── Write array throws on insufficient space ────────────────────

    [Fact]
    public void WriteBytes_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        byte[] values = new byte[10];
        bool threw = false;
        try { writer.WriteBytes(values); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    // ── Multiple sequential values ──────────────────────────────────

    [Fact]
    public void WriteByte_ReadByte_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteByte(0);
        writer.WriteByte(127);
        writer.WriteByte(255);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((byte)0, reader.ReadByte());
        Assert.Equal((byte)127, reader.ReadByte());
        Assert.Equal((byte)255, reader.ReadByte());
    }
}
