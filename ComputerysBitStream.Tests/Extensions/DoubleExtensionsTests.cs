using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class DoubleExtensionsTests {
    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(-1.0)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    [InlineData(double.Epsilon)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(3.141592653589793)]
    public void WriteDouble_ReadDouble_RoundTrip(double value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDouble(value);

        ReadContext reader = new ReadContext(buffer);
        double result = reader.ReadDouble();
        if (double.IsNaN(value)) Assert.True(double.IsNaN(result));
        else Assert.Equal(value, result);
    }

    [Fact]
    public void WriteDouble_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(3.14);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(3.14, reader.ReadDouble());
    }

    [Fact]
    public void PeekDouble_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDouble(1.5);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1.5, reader.PeekDouble());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadDouble_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDouble(1.0);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadDouble();
        Assert.Equal(64, reader.Position);
    }

    [Fact]
    public void TryReadDouble_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDouble(2.5);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadDouble(out double value));
        Assert.Equal(2.5, value);
    }

    [Fact]
    public void TryReadDouble_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadDouble(out double value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadDouble_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadDouble());
    }

    [Fact]
    public void WriteDouble_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteDouble(1.0); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteDoubles_ReadDoubles_WithLengthPrefix_RoundTrip() {
        double[] values = { -1.5, 0.0, 1.5, double.MaxValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubles(values);

        ReadContext reader = new ReadContext(buffer);
        double[] result = reader.ReadDoubles();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteDoubles_PeekDoubles_DoesNotAdvancePosition() {
        double[] values = { 1.1, 2.2 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubles(values);

        ReadContext reader = new ReadContext(buffer);
        double[] result = reader.PeekDoubles();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteDoublesWithoutLength_ReadDoubles_Count_RoundTrip() {
        double[] values = { -0.5, 0.0, 0.5 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoublesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        double[] result = reader.ReadDoubles(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadDoubles_ReturnsTrue_WhenSpaceAvailable() {
        double[] values = { 1.0, 2.0 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubles(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadDoubles(out double[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteDoubles_ReadDoublesSpan_WithLengthPrefix_RoundTrip() {
        double[] values = { -1.0, 0.0, 1.0 };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubles(values);

        ReadContext reader = new ReadContext(buffer);
        Span<double> destination = stackalloc double[values.Length];
        reader.ReadDoubles(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteDoublesWithoutLength_ReadDoublesSpan_Count_RoundTrip() {
        double[] values = { 3.14, 2.72 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoublesWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<double> destination = stackalloc double[values.Length];
        reader.ReadDoubles(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteDouble_ReadDouble_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDouble(double.MinValue);
        writer.WriteDouble(0.0);
        writer.WriteDouble(double.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(double.MinValue, reader.ReadDouble());
        Assert.Equal(0.0, reader.ReadDouble());
        Assert.Equal(double.MaxValue, reader.ReadDouble());
    }
}
