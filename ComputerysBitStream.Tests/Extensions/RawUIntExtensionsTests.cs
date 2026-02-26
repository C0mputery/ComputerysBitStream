using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawUIntExtensionsTests {
    [Theory]
    [InlineData(uint.MaxValue)]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(123456789u)]
    [InlineData(2147483648u)]
    public void WriteUIntRaw_ReadUIntRaw_RoundTrip(uint value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadUIntRaw());
    }

    [Fact]
    public void WriteUIntRaw_PeekUIntRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntRaw(999u);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(999u, reader.PeekUIntRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUIntRaw_ReadUIntRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntRaw(1u);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadUIntRaw();
        Assert.Equal(32, reader.Position);
    }

    [Fact]
    public void WriteUIntRaw_ReadUIntRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntRaw(uint.MinValue);
        writer.WriteUIntRaw(1000u);
        writer.WriteUIntRaw(uint.MaxValue);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(uint.MinValue, reader.ReadUIntRaw());
        Assert.Equal(1000u, reader.ReadUIntRaw());
        Assert.Equal(uint.MaxValue, reader.ReadUIntRaw());
    }

    [Fact]
    public void WriteUIntsRaw_ReadUIntArrayRaw_SmallCount() {
        uint[] values = { 0, 100, 200, uint.MaxValue, 2147483648 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.ReadUIntArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUIntsRaw_ReadUIntArrayRaw_LargeCount() {
        uint[] values = new uint[5];
        for (int i = 0; i < values.Length; i++) values[i] = (uint)(i * 858993459);

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.ReadUIntArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteUIntsRaw_PeekUIntArrayRaw_DoesNotAdvancePosition() {
        uint[] values = { 111u, 222u, 333u };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.PeekUIntArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUIntsRaw_ReadUIntSpanRaw() {
        uint[] values = { 10u, 20u, 30u, 40u, 50u };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<uint> destination = stackalloc uint[values.Length];
        reader.ReadUIntSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteUIntsRaw_PeekUIntSpanRaw_DoesNotAdvancePosition() {
        uint[] values = { 500u, 1000u, 1500u };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<uint> destination = stackalloc uint[values.Length];
        reader.PeekUIntSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteUIntsRaw_ExactlyUlongBoundary() {
        uint[] values = { 12345u, 67890u };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteUIntsRaw(values);

        ReadContext reader = new ReadContext(buffer);
        uint[] result = reader.ReadUIntArrayRaw(2);
        Assert.Equal(values, result);
    }
}
