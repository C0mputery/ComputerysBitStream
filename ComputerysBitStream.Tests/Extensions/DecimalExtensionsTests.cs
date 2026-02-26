using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class DecimalExtensionsTests {
    [Fact]
    public void WriteDecimal_ReadDecimal_Zero() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(0m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(0m, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_ReadDecimal_One() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(1m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1m, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_ReadDecimal_NegativeOne() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(-1m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(-1m, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_ReadDecimal_MinValue() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(decimal.MinValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(decimal.MinValue, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_ReadDecimal_MaxValue() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(decimal.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(decimal.MaxValue, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_ReadDecimal_Precision() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(3.14159265358979323846m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(3.14159265358979323846m, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_WriteOverload_RoundTrip() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.Write(42.5m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(42.5m, reader.ReadDecimal());
    }

    [Fact]
    public void PeekDecimal_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(1.5m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1.5m, reader.PeekDecimal());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void ReadDecimal_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(1m);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadDecimal();
        Assert.Equal(128, reader.Position);
    }

    [Fact]
    public void TryReadDecimal_ReturnsTrue_WhenSpaceAvailable() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(99.99m);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadDecimal(out decimal value));
        Assert.Equal(99.99m, value);
    }

    [Fact]
    public void TryReadDecimal_ReturnsFalse_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.False(reader.TryReadDecimal(out decimal value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void ReadDecimal_ReturnsDefault_WhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        ReadContext reader = new ReadContext(buffer);
        reader.SetCapacityRaw(0);
        Assert.Equal(default, reader.ReadDecimal());
    }

    [Fact]
    public void WriteDecimal_ThrowsWhenInsufficientSpace() {
        Span<ulong> buffer = stackalloc ulong[1];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.SetPositionRaw(64);
        bool threw = false;
        try { writer.WriteDecimal(1m); } catch (InsufficientWriteSpaceException) { threw = true; }
        Assert.True(threw);
    }

    [Fact]
    public void WriteDecimals_ReadDecimals_WithLengthPrefix_RoundTrip() {
        decimal[] values = { -1.5m, 0m, 1.5m, decimal.MaxValue };
        Span<ulong> buffer = stackalloc ulong[16];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimals(values);

        ReadContext reader = new ReadContext(buffer);
        decimal[] result = reader.ReadDecimals();
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteDecimals_PeekDecimals_DoesNotAdvancePosition() {
        decimal[] values = { 1.1m, 2.2m };
        Span<ulong> buffer = stackalloc ulong[16];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimals(values);

        ReadContext reader = new ReadContext(buffer);
        decimal[] result = reader.PeekDecimals();
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteDecimalsWithoutLength_ReadDecimals_Count_RoundTrip() {
        decimal[] values = { -0.5m, 0m, 0.5m };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        decimal[] result = reader.ReadDecimals(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void TryReadDecimals_ReturnsTrue_WhenSpaceAvailable() {
        decimal[] values = { 1m, 2m };
        Span<ulong> buffer = stackalloc ulong[16];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimals(values);

        ReadContext reader = new ReadContext(buffer);
        Assert.True(reader.TryReadDecimals(out decimal[] result));
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteDecimals_ReadDecimalsSpan_WithLengthPrefix_RoundTrip() {
        decimal[] values = { -1m, 0m, 1m };
        Span<ulong> buffer = stackalloc ulong[16];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimals(values);

        ReadContext reader = new ReadContext(buffer);
        Span<decimal> destination = stackalloc decimal[values.Length];
        reader.ReadDecimals(ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteDecimalsWithoutLength_ReadDecimalsSpan_Count_RoundTrip() {
        decimal[] values = { 3.14m, 2.72m };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalsWithoutLength(values);

        ReadContext reader = new ReadContext(buffer);
        Span<decimal> destination = stackalloc decimal[values.Length];
        reader.ReadDecimals(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteDecimal_ReadDecimal_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimal(decimal.MinValue);
        writer.WriteDecimal(0m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(decimal.MinValue, reader.ReadDecimal());
        Assert.Equal(0m, reader.ReadDecimal());
    }
}
