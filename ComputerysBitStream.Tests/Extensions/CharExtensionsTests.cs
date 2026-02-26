using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class CharExtensionsTests {
    [Theory]
    [InlineData('A')]
    [InlineData('Z')]
    [InlineData('\0')]
    [InlineData(char.MaxValue)]
    [InlineData('\u00E9')]
    public void WriteChar_ReadChar_RoundTrip(char value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChar(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadChar());
    }

    [Fact]
    public void WriteChar_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write('X');

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal('X', reader.ReadChar());
    }

    [Fact]
    public void PeekChar_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChar('A');

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal('A', reader.PeekChar());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadChar_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChar('A');

        ReadContext reader = new ReadContext(buffer);
        reader.ReadChar();
        Assert.Equal(16, reader.Position);
    }

    [Fact]
    public void TryReadChar_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChar('B');

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadChar(out char value));
        Assert.Equal('B', value);
    }

    [Fact]
    public void TryReadChar_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadChar(out char value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadChar_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadChar());
    }

    [Fact]
    public void WriteChar_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteChar('A'); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteChars_ReadChars_WithLengthPrefix_RoundTrip() {
        char[] values = { 'H', 'e', 'l', 'l', 'o' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChars(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.ReadChars();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteChars_PeekChars_DoesNotAdvancePosition() {
        char[] values = { 'A', 'B' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChars(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.PeekChars();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteCharsWithoutLength_ReadChars_Count_RoundTrip() {
        char[] values = { 'X', 'Y', 'Z' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        char[] result = reader.ReadChars(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadChars_ReturnsTrue_WhenSpaceAvailable() {
        char[] values = { 'A', 'B' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChars(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadChars(out char[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteChars_ReadCharsSpan_WithLengthPrefix_RoundTrip() {
        char[] values = { 'a', 'b', 'c' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChars(values);

        ReadContext reader = new ReadContext(buffer);
        Span<char> destination = stackalloc char[values.Length];
        reader.ReadChars(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteCharsWithoutLength_ReadCharsSpan_Count_RoundTrip() {
        char[] values = { 'd', 'e', 'f' };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteCharsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<char> destination = stackalloc char[values.Length];
        reader.ReadChars(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteChar_ReadChar_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteChar('A');
        writer.WriteChar('\0');
        writer.WriteChar(char.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal('A', reader.ReadChar());
        Assert.Equal('\0', reader.ReadChar());
        Assert.Equal(char.MaxValue, reader.ReadChar());
    }
}
