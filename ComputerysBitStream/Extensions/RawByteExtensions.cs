using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(byte), BitSizes.ByteSize)]
public static class RawByteExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.ByteSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(byte value) => value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte FromBits(ulong value) => (byte)value;
    
    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteByteRaw(this ref WriteContext context, byte value) { context.WriteBitsRaw(AsBits(value), BitSizes.ByteSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteBytesRaw(this ref WriteContext context, ReadOnlySpan<byte> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<byte, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingBytes = values.Length % NumberOfValuesInUlong;
        if (remainingBytes != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingBytes; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingBytes + i])) << (i * BitSizes.ByteSize);
            }
            context.WriteBitsRaw(lastPacked, remainingBytes * BitSizes.ByteSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte PeekByteRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.ByteSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByteRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.ByteSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] PeekByteArrayRaw(this ref ReadContext context, int count) {
        byte[] result = new byte[count];
        Span<byte> span = result.AsSpan();
        context.PeekByteSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ReadByteArrayRaw(this ref ReadContext context, int count) {
        byte[] result = new byte[count];
        Span<byte> span = result.AsSpan();
        context.ReadByteSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekByteSpanRaw(this ref ReadContext context, int count, ref Span<byte> destination) {
        int originalPosition = context.Position;
        context.ReadByteSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadByteSpanRaw(this ref ReadContext context, int count, ref Span<byte> destination) {
        Span<byte> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<byte, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingBytes = count % NumberOfValuesInUlong;
        if (remainingBytes != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingBytes * BitSizes.ByteSize);
            for (int i = 0; i < remainingBytes; i++) {
                destination[count - remainingBytes + i] = FromBits(lastPacked >> (i * BitSizes.ByteSize));
            }
        }
    }
}
