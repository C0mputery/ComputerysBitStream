using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawDecimalExtensionsTests {
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("79228162514264337593543950335")]   // decimal.MaxValue
    [InlineData("-79228162514264337593543950335")]  // decimal.MinValue
    [InlineData("3.14159265358979")]
    [InlineData("-2.71828182845904")]
    [InlineData("0.0000000000000001")]
    public void WriteDecimalRaw_ReadDecimalRaw_RoundTrip(string valueStr) {
        decimal value = decimal.Parse(valueStr);
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadDecimalRaw());
    }

    [Fact]
    public void WriteDecimalRaw_PeekDecimalRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalRaw(3.14m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(3.14m, reader.PeekDecimalRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteDecimalRaw_ReadDecimalRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalRaw(1m);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadDecimalRaw();
        Assert.Equal(128, reader.Position); // decimal = 128 bits
    }

    [Fact]
    public void WriteDecimalRaw_ReadDecimalRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalRaw(1.1m);
        writer.WriteDecimalRaw(2.2m);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(1.1m, reader.ReadDecimalRaw());
        Assert.Equal(2.2m, reader.ReadDecimalRaw());
    }

    [Fact]
    public void WriteDecimalsRaw_ReadDecimalArrayRaw() {
        decimal[] values = { decimal.MinValue, -1.5m, 0m, 1.5m, decimal.MaxValue };
        Span<ulong> buffer = stackalloc ulong[16];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        decimal[] result = reader.ReadDecimalArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteDecimalsRaw_PeekDecimalArrayRaw_DoesNotAdvancePosition() {
        decimal[] values = { 1.1m, 2.2m, 3.3m };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        decimal[] result = reader.PeekDecimalArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteDecimalsRaw_ReadDecimalSpanRaw() {
        decimal[] values = { 0.1m, 0.2m, 0.3m };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<decimal> destination = stackalloc decimal[values.Length];
        reader.ReadDecimalSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteDecimalsRaw_PeekDecimalSpanRaw_DoesNotAdvancePosition() {
        decimal[] values = { 10.5m, 20.5m, 30.5m };
        Span<ulong> buffer = stackalloc ulong[8];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<decimal> destination = stackalloc decimal[values.Length];
        reader.PeekDecimalSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WritePosition_AdvancesCorrectly() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteDecimalRaw(42m);
        Assert.Equal(128, writer.Position);
    }
}
