using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawCharExtensionsTests {
    [Theory]
    [InlineData('A')]
    [InlineData('Z')]
    [InlineData('a')]
    [InlineData('z')]
    [InlineData('0')]
    [InlineData('\0')]
    [InlineData(char.MaxValue)]
    public void WriteCharRaw_ReadCharRaw_RoundTrip(char value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadCharRaw());
    }

    [Fact]
    public void WriteCharRaw_PeekCharRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharRaw('X');

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal('X', reader.PeekCharRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteCharRaw_ReadCharRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharRaw('A');

        ReadContext reader = new ReadContext(buffer);
        reader.ReadCharRaw();
        Assert.Equal(16, reader.Position);
    }

    [Fact]
    public void WriteCharRaw_ReadCharRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharRaw('H');
        writer.WriteCharRaw('e');
        writer.WriteCharRaw('l');
        writer.WriteCharRaw('l');
        writer.WriteCharRaw('o');

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal('H', reader.ReadCharRaw());
        Assert.Equal('e', reader.ReadCharRaw());
        Assert.Equal('l', reader.ReadCharRaw());
        Assert.Equal('l', reader.ReadCharRaw());
        Assert.Equal('o', reader.ReadCharRaw());
    }

    [Fact]
    public void WriteCharsRaw_ReadCharArrayRaw_SmallCount() {
        char[] values = { 'H', 'e', 'l', 'l', 'o' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.ReadCharArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteCharsRaw_ReadCharArrayRaw_LargeCount() {
        // 10 chars: 4 per ulong, exercises remainder
        char[] values = new char[10];
        for (int i = 0; i < values.Length; i++) values[i] = (char)('A' + i);

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.ReadCharArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteCharsRaw_PeekCharArrayRaw_DoesNotAdvancePosition() {
        char[] values = { 'X', 'Y', 'Z' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.PeekCharArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteCharsRaw_ReadCharSpanRaw() {
        char[] values = { 'a', 'b', 'c', 'd', 'e' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<char> destination = stackalloc char[values.Length];
        reader.ReadCharSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteCharsRaw_PeekCharSpanRaw_DoesNotAdvancePosition() {
        char[] values = { '1', '2', '3' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<char> destination = stackalloc char[values.Length];
        reader.PeekCharSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteCharsRaw_ExactlyUlongBoundary() {
        // 4 chars = exactly 1 ulong (4 * 16 = 64 bits)
        char[] values = { 'A', 'B', 'C', 'D' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.ReadCharArrayRaw(4);
        Assert.Equal(values, result);
    }
}
