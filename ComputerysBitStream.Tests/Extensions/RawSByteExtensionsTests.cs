using System;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests.Extensions;

public class RawSByteExtensionsTests {
    [Theory]
    [InlineData((sbyte)0)]
    [InlineData((sbyte)1)]
    [InlineData((sbyte)-1)]
    [InlineData(sbyte.MinValue)]
    [InlineData(sbyte.MaxValue)]
    [InlineData((sbyte)-42)]
    [InlineData((sbyte)42)]
    public void WriteSByteRaw_ReadSByteRaw_RoundTrip(sbyte value) {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByteRaw(value);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(value, reader.ReadSByteRaw());
    }

    [Fact]
    public void WriteSByteRaw_PeekSByteRaw_DoesNotAdvancePosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByteRaw(-50);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal((sbyte)-50, reader.PeekSByteRaw());
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteSByteRaw_ReadSByteRaw_AdvancesPosition() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByteRaw(1);

        ReadContext reader = new ReadContext(buffer);
        reader.ReadSByteRaw();
        Assert.Equal(8, reader.Position);
    }

    [Fact]
    public void WriteSByteRaw_ReadSByteRaw_MultipleValues() {
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSByteRaw(-128);
        writer.WriteSByteRaw(0);
        writer.WriteSByteRaw(127);

        ReadContext reader = new ReadContext(buffer);
        Assert.Equal(sbyte.MinValue, reader.ReadSByteRaw());
        Assert.Equal((sbyte)0, reader.ReadSByteRaw());
        Assert.Equal(sbyte.MaxValue, reader.ReadSByteRaw());
    }

    [Fact]
    public void WriteSBytesRaw_ReadSByteArrayRaw_SmallCount() {
        sbyte[] values = { -1, 0, 1, -128, 127 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.ReadSByteArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteSBytesRaw_ReadSByteArrayRaw_LargeCount() {
        sbyte[] values = new sbyte[20];
        for (int i = 0; i < values.Length; i++) values[i] = (sbyte)(i * 7 - 64);

        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.ReadSByteArrayRaw(values.Length);
        Assert.Equal(values, result);
    }

    [Fact]
    public void WriteSBytesRaw_PeekSByteArrayRaw_DoesNotAdvancePosition() {
        sbyte[] values = { -10, 20, -30 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.PeekSByteArrayRaw(values.Length);
        Assert.Equal(values, result);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteSBytesRaw_ReadSByteSpanRaw() {
        sbyte[] values = { -5, 10, -15, 20, -25 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<sbyte> destination = stackalloc sbyte[values.Length];
        reader.ReadSByteSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
    }

    [Fact]
    public void WriteSBytesRaw_PeekSByteSpanRaw_DoesNotAdvancePosition() {
        sbyte[] values = { -100, 50, 0 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        Span<sbyte> destination = stackalloc sbyte[values.Length];
        reader.PeekSByteSpanRaw(values.Length, ref destination);
        for (int i = 0; i < values.Length; i++) Assert.Equal(values[i], destination[i]);
        Assert.Equal(0, reader.Position);
    }

    [Fact]
    public void WriteSBytesRaw_ExactlyUlongBoundary() {
        sbyte[] values = { -1, 2, -3, 4, -5, 6, -7, 8 };
        Span<ulong> buffer = stackalloc ulong[4];
        buffer.Clear();
        WriteContext writer = new WriteContext(buffer);
        writer.WriteSBytesRaw(values);

        ReadContext reader = new ReadContext(buffer);
        sbyte[] result = reader.ReadSByteArrayRaw(8);
        Assert.Equal(values, result);
    }
}
