using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class UIntExtensionsTests {
    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(uint.MaxValue)]
    [InlineData(2147483648u)]
    public void WriteUInt_ReadUInt_RoundTrip(uint value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInt(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadUInt());
    }

    [Fact]
    public void WriteUInt_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(42u);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42u, reader.ReadUInt());
    }

    [Fact]
    public void PeekUInt_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInt(99);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(99u, reader.PeekUInt());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadUInt_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInt(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadUInt();
        Assert.Equal(32, reader.Position);
    }

    [Fact]
    public void TryReadUInt_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInt(555);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadUInt(out uint value));
        Assert.Equal(555u, value);
    }

    [Fact]
    public void TryReadUInt_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadUInt(out uint value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadUInt_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadUInt());
    }

    [Fact]
    public void WriteUInt_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteUInt(1); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteUInts_ReadUInts_WithLengthPrefix_RoundTrip() {
        uint[] values = { 0, 1, uint.MaxValue, 2147483648 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInts(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.ReadUInts();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUInts_PeekUInts_DoesNotAdvancePosition() {
        uint[] values = { 10, 20 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInts(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.PeekUInts();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUIntsWithoutLength_ReadUInts_Count_RoundTrip() {
        uint[] values = { 100, 200, 300 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.ReadUInts(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadUInts_ReturnsTrue_WhenSpaceAvailable() {
        uint[] values = { 5, 10 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInts(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadUInts(out uint[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUInts_ReadUIntsSpan_WithLengthPrefix_RoundTrip() {
        uint[] values = { 1, 2, 3 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInts(values);

        ReadContext reader = new ReadContext(buffer);
        Span<uint> destination = stackalloc uint[values.Length];
        reader.ReadUInts(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteUIntsWithoutLength_ReadUIntsSpan_Count_RoundTrip() {
        uint[] values = { 7, 8 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<uint> destination = stackalloc uint[values.Length];
        reader.ReadUInts(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteUInt_ReadUInt_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUInt(0);
        writer.WriteUInt(uint.MaxValue / 2);
        writer.WriteUInt(uint.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(0u, reader.ReadUInt());
        Assert.Equal(uint.MaxValue / 2, reader.ReadUInt());
        Assert.Equal(uint.MaxValue, reader.ReadUInt());
    }
}
