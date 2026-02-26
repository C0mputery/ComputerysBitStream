using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class BoolExtensionsTests {
    // ── Scalar Write / Read round-trip ──────────────────────────────

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void WriteBool_ReadBool_RoundTrip(bool value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadBool());
    }

    [Fact]
    public void WriteBool_WriteOverload_ReadBool_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.ReadBool());
    }

    [Fact]
    public void ReadBool_OutOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        reader.Read(out bool value);
        Assert.True(value);
    }

    // ── Scalar Peek ─────────────────────────────────────────────────

    [Fact]
    public void PeekBool_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.PeekBool());
        Assert.Equal(0, reader.Position);
        Assert.True(reader.PeekBool());
    }

    [Fact]
    public void PeekBool_OutOverload_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        reader.Peek(out bool value);
        Assert.True(value);
        Assert.Equal(0, reader.Position);
    }

    // ── Scalar Read advances position ───────────────────────────────

    [Fact]
    public void ReadBool_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadBool();
        Assert.Equal(1, reader.Position);
    }

    // ── TryPeek / TryRead scalar ────────────────────────────────────

    [Fact]
    public void TryPeekBool_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekBool(out bool value));
        Assert.True(value);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryPeekBool_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryPeekBool(out bool value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryPeek_OutOverload_DelegatesToTryPeekBool() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(false);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeek(out bool value));
        Assert.False(value);
    }

    [Fact]
    public void TryReadBool_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadBool(out bool value));
        Assert.True(value);
        Assert.Equal(1, reader.Position);
    }

    [Fact]
    public void TryReadBool_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadBool(out bool value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryRead_OutOverload_DelegatesToTryReadBool() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryRead(out bool value));
        Assert.True(value);
    }

    // ── Scalar insufficient space behavior ──────────────────────────

    [Fact]
    public void ReadBool_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadBool());
    }

    [Fact]
    public void PeekBool_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.PeekBool());
    }

    [Fact]
    public void WriteBool_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteBool(true); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    // ── Write position advancement ──────────────────────────────────

    [Fact]
    public void WriteBool_AdvancesWritePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);
        Assert.Equal(1, writer.Position);
        writer.WriteBool(false);
        Assert.Equal(2, writer.Position);
    }

    // ── Array with length prefix ────────────────────────────────────

    [Fact]
    public void WriteBools_ReadBools_WithLengthPrefix_RoundTrip() {
        bool[] values = { true, false, true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBools();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBools_WriteOverload_ReadBools_RoundTrip() {
        bool[] values = { true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(new ReadOnlySpan<bool>(values));

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBools();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBools_ReadBools_OutOverload_RoundTrip() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        reader.Read(out bool[] result);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBools_PeekBools_WithLengthPrefix_DoesNotAdvancePosition() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.PeekBools();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteBools_ReadBools_AdvancesPosition() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadBools();
        // int header (32 bits) + 3 bools (3 bits) = 35 bits
        Assert.Equal(32 + 3, reader.Position);
    }

    [Fact]
    public void WriteBools_EmptyArray_RoundTrip() {
        bool[] values = Array.Empty<bool>();
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBools();
        Assert.Empty(result);
    }

    // ── TryRead / TryPeek array with length prefix ──────────────────

    [Fact]
    public void TryReadBools_ReturnsTrue_WhenSpaceAvailable() {
        bool[] values = { true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadBools(out bool[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadBools_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadBools(out bool[] result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryPeekBools_ReturnsTrue_WhenSpaceAvailable() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekBools(out bool[] result));
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryPeekBools_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryPeekBools(out bool[] result));
        Assert.Empty(result);
    }

    // ── Array without length prefix ─────────────────────────────────

    [Fact]
    public void WriteBoolsWithoutLength_ReadBools_Count_RoundTrip() {
        bool[] values = { true, false, true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBools(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBoolsWithoutLength_WriteWithoutLengthOverload_RoundTrip() {
        bool[] values = { true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteWithoutLength(new ReadOnlySpan<bool>(values));

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBools(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBoolsWithoutLength_PeekBools_Count_DoesNotAdvancePosition() {
        bool[] values = { false, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.PeekBools(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryReadBools_Count_ReturnsTrue_WhenSpaceAvailable() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadBools(values.Length, out bool[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadBools_Count_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadBools(5, out bool[] result));
        Assert.Empty(result);
    }

    [Fact]
    public void TryPeekBools_Count_ReturnsTrue_WhenSpaceAvailable() {
        bool[] values = { true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryPeekBools(values.Length, out bool[] result));
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadBools_Count_NegativeCount_ReturnsEmpty() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        Assert.Empty(reader.ReadBools(-1));
    }

    [Fact]
    public void PeekBools_Count_NegativeCount_ReturnsEmpty() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        Assert.Empty(reader.PeekBools(-1));
    }

    // ── Span with length prefix ─────────────────────────────────────

    [Fact]
    public void WriteBools_ReadBoolsSpan_WithLengthPrefix_RoundTrip() {
        bool[] values = { true, false, true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        reader.ReadBools(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteBools_PeekBoolsSpan_WithLengthPrefix_DoesNotAdvancePosition() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        reader.PeekBools(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryReadBoolsSpan_WithLengthPrefix_ReturnsTrue() {
        bool[] values = { true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        Assert.True(reader.TryReadBools(ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void TryPeekBoolsSpan_WithLengthPrefix_ReturnsTrue() {
        bool[] values = { true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBools(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        Assert.True(reader.TryPeekBools(ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    // ── Span without length prefix ──────────────────────────────────

    [Fact]
    public void WriteBoolsWithoutLength_ReadBoolsSpan_Count_RoundTrip() {
        bool[] values = { false, true, false, true, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        reader.ReadBools(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteBoolsWithoutLength_PeekBoolsSpan_Count_DoesNotAdvancePosition() {
        bool[] values = { true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        reader.PeekBools(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void TryReadBoolsSpan_Count_ReturnsTrue() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        Assert.True(reader.TryReadBools(values.Length, ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void TryPeekBoolsSpan_Count_ReturnsTrue() {
        bool[] values = { false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        Assert.True(reader.TryPeekBools(values.Length, ref destination));
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    // ── Write array throws on insufficient space ────────────────────

    [Fact]
    public void WriteBools_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        // 64 bits total, need 32 (int header) + 100 * 1 = 132 bits
        bool[] values = new bool[100];
        bool threw = false;
        try { writer.WriteBools(values); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteBoolsWithoutLength_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        // 64 bits total, need 100 bits
        bool[] values = new bool[100];
        bool threw = false;
        try { writer.WriteBoolsWithoutLength(values); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    // ── Multiple sequential values ──────────────────────────────────

    [Fact]
    public void WriteBool_ReadBool_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBool(true);
        writer.WriteBool(false);
        writer.WriteBool(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.ReadBool());
        Assert.False(reader.ReadBool());
        Assert.True(reader.ReadBool());
    }
}
