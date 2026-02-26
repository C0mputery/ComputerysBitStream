using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawDoubleExtensionsTests {
    [Theory]
    [InlineData(0d)]
    [InlineData(1d)]
    [InlineData(-1d)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    [InlineData(double.Epsilon)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(3.141592653589793d)]
    [InlineData(-2.718281828459045d)]
    public void WriteDoubleRaw_ReadDoubleRaw_RoundTrip(double value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubleRaw(value);

        ReadContext reader = new ReadContext(buffer);
        double result = reader.ReadDoubleRaw();
        if (double.IsNaN(value))
            Assert.True(double.IsNaN(result));
        else
            Assert.Equal(value, result);
    }

    [Fact]
    public void WriteDoubleRaw_PeekDoubleRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubleRaw(3.14d);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(3.14d, reader.PeekDoubleRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteDoubleRaw_ReadDoubleRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubleRaw(1d);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadDoubleRaw();
        Assert.Equal(64, reader.Position);
    }

    [Fact]
    public void WriteDoubleRaw_ReadDoubleRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubleRaw(1.1d);
        writer.WriteDoubleRaw(2.2d);
        writer.WriteDoubleRaw(3.3d);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1.1d, reader.ReadDoubleRaw());
        Assert.Equal(2.2d, reader.ReadDoubleRaw());
        Assert.Equal(3.3d, reader.ReadDoubleRaw());
    }

    [Fact]
    public void WriteDoublesRaw_ReadDoubleArrayRaw() {
        double[] values = { double.MinValue, -1.5d, 0d, 1.5d, double.MaxValue };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoublesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        double[] result = reader.ReadDoubleArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteDoublesRaw_PeekDoubleArrayRaw_DoesNotAdvancePosition() {
        double[] values = { 1.1d, 2.2d, 3.3d };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoublesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        double[] result = reader.PeekDoubleArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteDoublesRaw_ReadDoubleSpanRaw() {
        double[] values = { 0.1d, 0.2d, 0.3d };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoublesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<double> destination = stackalloc double[values.Length];
        reader.ReadDoubleSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteDoublesRaw_PeekDoubleSpanRaw_DoesNotAdvancePosition() {
        double[] values = { 10.5d, 20.5d, 30.5d };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoublesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<double> destination = stackalloc double[values.Length];
        reader.PeekDoubleSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDoubleRaw(42d);
        Assert.Equal(64, writer.Position);
    }
}
