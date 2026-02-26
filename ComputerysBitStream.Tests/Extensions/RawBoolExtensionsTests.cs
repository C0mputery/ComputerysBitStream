using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawBoolExtensionsTests {
    [Fact]
    public void WriteBoolRaw_ReadBoolRaw_True() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolRaw(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.ReadBoolRaw());
    }

    [Fact]
    public void WriteBoolRaw_ReadBoolRaw_False() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolRaw(false);

        ReadContext reader = new ReadContext(buffer);
        Assert.False(reader.ReadBoolRaw());
    }

    [Fact]
    public void WriteBoolRaw_PeekBoolRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolRaw(true);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.PeekBoolRaw());
        Assert.Equal(0, reader.Position);
        Assert.True(reader.PeekBoolRaw());
    }

    [Fact]
    public void WriteBoolRaw_ReadBoolRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolRaw(true);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadBoolRaw();
        Assert.Equal(1, reader.Position);
    }

    [Fact]
    public void WriteBoolRaw_ReadBoolRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolRaw(true);
        writer.WriteBoolRaw(false);
        writer.WriteBoolRaw(true);
        writer.WriteBoolRaw(true);
        writer.WriteBoolRaw(false);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.ReadBoolRaw());
        Assert.False(reader.ReadBoolRaw());
        Assert.True(reader.ReadBoolRaw());
        Assert.True(reader.ReadBoolRaw());
        Assert.False(reader.ReadBoolRaw());
    }

    [Fact]
    public void WriteBoolsRaw_ReadBoolArrayRaw_SmallCount() {
        bool[] values = { true, false, true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBoolArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBoolsRaw_ReadBoolArrayRaw_LargeCount() {
        // 100 bools exercises both the full-ulong path and remainder path
        bool[] values = new bool[100];
        for (int i = 0; i < values.Length; i++) values[i] = i % 3 == 0;

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBoolArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteBoolsRaw_PeekBoolArrayRaw_DoesNotAdvancePosition() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.PeekBoolArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteBoolsRaw_ReadBoolSpanRaw() {
        bool[] values = { false, true, false, true, true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        reader.ReadBoolSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteBoolsRaw_PeekBoolSpanRaw_DoesNotAdvancePosition() {
        bool[] values = { true, true, false };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<bool> destination = stackalloc bool[values.Length];
        reader.PeekBoolSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteBoolsRaw_ExactlyUlongBoundary() {
        // 64 bools = exactly 1 ulong worth
        bool[] values = new bool[64];
        for (int i = 0; i < 64; i++) values[i] = i % 2 == 0;

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        bool[] result = reader.ReadBoolArrayRaw(64);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        Assert.Equal(0, writer.Position);
        writer.WriteBoolRaw(true);
        Assert.Equal(1, writer.Position);
        writer.WriteBoolRaw(false);
        Assert.Equal(2, writer.Position);
    }

    [Fact]
    public void WriteBoolsRaw_WritePosition_AdvancesCorrectly() {
        bool[] values = { true, false, true };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteBoolsRaw(values);
        Assert.Equal(3, writer.Position);
    }
}
