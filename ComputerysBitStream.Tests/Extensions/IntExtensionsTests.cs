using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class IntExtensionsTests {
    // ── Scalar Write / Read round-trip ──────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(123456789)]
    [InlineData(-123456789)]
    public void WriteInt_ReadInt_RoundTrip(int value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadInt());
    }

    [Fact]
    public void WriteInt_WriteOverload_ReadInt_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42, reader.ReadInt());
    }

    [Fact]
    public void ReadInt_OutOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(42);

        ReadContext reader = new ReadContext(buffer);
        reader.Read(out int value);
        Assert.Equal(42, value);
    }

    // ── Scalar Peek ─────────────────────────────────────────────────

    [Fact]
    public void PeekInt_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(42);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42, reader.PeekInt());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void PeekInt_OutOverload_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(42);

        ReadContext reader = new ReadContext(buffer);
        reader.Peek(out int value);
        Assert.Equal(42, value);
        Assert.Equal(0, reader.Position);
    }

    // ── Scalar Read advances position ───────────────────────────────

    [Fact]
    public void ReadInt_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadInt();
        Assert.Equal(32, reader.Position);
    }

    // ── TryPeek / TryRead scalar ────────────────────────────────────

    [Fact]
    public void TryPeekInt_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(99);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekInt(out int value));
        Assert.Equal(99, value);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryPeekInt_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryPeekInt(out int value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryReadInt_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(-55);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadInt(out int value));
        Assert.Equal(-55, value);
        Assert.Equal(32, reader.Position);
    }

    [Fact]
    public void TryReadInt_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadInt(out int value));
        Assert.Equal(default, value);
    }

    // ── Scalar insufficient space behavior ──────────────────────────

    [Fact]
    public void ReadInt_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadInt());
    }

    [Fact]
    public void PeekInt_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.PeekInt());
    }

    [Fact]
    public void WriteInt_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteInt(42); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    // ── Write position advancement ──────────────────────────────────

    [Fact]
    public void WriteInt_AdvancesWritePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(1);
        Assert.Equal(32, writer.Position);
        writer.WriteInt(2);
        Assert.Equal(64, writer.Position);
    }

    // ── Array with length prefix ────────────────────────────────────

    [Fact]
    public void WriteInts_ReadInts_WithLengthPrefix_RoundTrip() {
        int[] values = { -100, 0, 100, int.MinValue, int.MaxValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.ReadInts();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteInts_PeekInts_WithLengthPrefix_DoesNotAdvancePosition() {
        int[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.PeekInts();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteInts_ReadInts_AdvancesPosition() {
        int[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadInts();
        // int header (32 bits) + 3 ints * 32 bits = 128 bits
        Assert.Equal(32 + 3 * 32, reader.Position);
    }

    [Fact]
    public void WriteInts_EmptyArray_RoundTrip() {
        int[] values = Array.Empty<int>();
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.ReadInts();
        Assert.Empty(result);
    }

    // ── TryRead / TryPeek array with length prefix ──────────────────

    [Fact]
    public void TryReadInts_ReturnsTrue_WhenSpaceAvailable() {
        int[] values = { 10, 20, 30 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadInts(out int[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadInts_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadInts(out int[] result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryPeekInts_ReturnsTrue_WhenSpaceAvailable() {
        int[] values = { 1, -1 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekInts(out int[] result));
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    // ── Array without length prefix ─────────────────────────────────

    [Fact]
    public void WriteIntsWithoutLength_ReadInts_Count_RoundTrip() {
        int[] values = { -100, 0, 100 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.ReadInts(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteIntsWithoutLength_PeekInts_Count_DoesNotAdvancePosition() {
        int[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        int[] result = reader.PeekInts(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryReadInts_Count_ReturnsTrue_WhenSpaceAvailable() {
        int[] values = { 5, 10 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadInts(values.Length, out int[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void ReadInts_Count_NegativeCount_ReturnsEmpty() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        Assert.Empty(reader.ReadInts(-1));
    }

    // ── Span with length prefix ─────────────────────────────────────

    [Fact]
    public void WriteInts_ReadIntsSpan_WithLengthPrefix_RoundTrip() {
        int[] values = { -1, 0, 1 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        reader.ReadInts(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteInts_PeekIntsSpan_WithLengthPrefix_DoesNotAdvancePosition() {
        int[] values = { 100, -100 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        reader.PeekInts(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryReadIntsSpan_WithLengthPrefix_ReturnsTrue() {
        int[] values = { 42, -42 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInts(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        Assert.True(reader.TryReadInts(ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    // ── Span without length prefix ──────────────────────────────────

    [Fact]
    public void WriteIntsWithoutLength_ReadIntsSpan_Count_RoundTrip() {
        int[] values = { 1, -2, 3 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        reader.ReadInts(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteIntsWithoutLength_PeekIntsSpan_Count_DoesNotAdvancePosition() {
        int[] values = { 10, 20 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        reader.PeekInts(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryReadIntsSpan_Count_ReturnsTrue() {
        int[] values = { 5, -5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        Assert.True(reader.TryReadInts(values.Length, ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void TryPeekIntsSpan_Count_ReturnsTrue() {
        int[] values = { 7, -7 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<int> destination = stackalloc int[values.Length];
        Assert.True(reader.TryPeekInts(values.Length, ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    // ── Write array throws on insufficient space ────────────────────

    [Fact]
    public void WriteInts_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        int[] values = { 1, 2, 3 };
        bool threw = false;
        try { writer.WriteInts(values); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteIntsWithoutLength_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        int[] values = { 1, 2, 3 };
        bool threw = false;
        try { writer.WriteIntsWithoutLength(values); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    // ── Multiple sequential values ──────────────────────────────────

    [Fact]
    public void WriteInt_ReadInt_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteInt(int.MinValue);
        writer.WriteInt(0);
        writer.WriteInt(int.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(int.MinValue, reader.ReadInt());
        Assert.Equal(0, reader.ReadInt());
        Assert.Equal(int.MaxValue, reader.ReadInt());
    }
}
