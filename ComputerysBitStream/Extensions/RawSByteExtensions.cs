using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(sbyte), BitSizes.SByteSize)]
public static class RawSByteExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.SByteSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(sbyte value) => (byte)value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static sbyte FromBits(ulong value) => (sbyte)(byte)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSByteRaw(this ref WriteContext context, sbyte value) { context.WriteBitsRaw(AsBits(value), BitSizes.SByteSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSBytesRaw(this ref WriteContext context, ReadOnlySpan<sbyte> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<sbyte, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingSBytes = values.Length % NumberOfValuesInUlong;
        if (remainingSBytes != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingSBytes; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingSBytes + i])) << (i * BitSizes.SByteSize);
            }
            context.WriteBitsRaw(lastPacked, remainingSBytes * BitSizes.SByteSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte PeekSByteRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.SByteSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte ReadSByteRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.SByteSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte[] PeekSByteArrayRaw(this ref ReadContext context, int count) {
        sbyte[] result = new sbyte[count];
        Span<sbyte> span = result.AsSpan();
        context.PeekSByteSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte[] ReadSByteArrayRaw(this ref ReadContext context, int count) {
        sbyte[] result = new sbyte[count];
        Span<sbyte> span = result.AsSpan();
        context.ReadSByteSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekSByteSpanRaw(this ref ReadContext context, int count, ref Span<sbyte> destination) {
        int originalPosition = context.Position;
        context.ReadSByteSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadSByteSpanRaw(this ref ReadContext context, int count, ref Span<sbyte> destination) {
        Span<sbyte> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<sbyte, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingSBytes = count % NumberOfValuesInUlong;
        if (remainingSBytes != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingSBytes * BitSizes.SByteSize);
            for (int i = 0; i < remainingSBytes; i++) {
                destination[count - remainingSBytes + i] = FromBits(lastPacked >> (i * BitSizes.SByteSize));
            }
        }
    }
}
