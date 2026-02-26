using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(ushort), BitSizes.UShortSize)]
public static class RawUShortExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.UShortSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(ushort value) => value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ushort FromBits(ulong value) => (ushort)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUShortRaw(this ref WriteContext context, ushort value) { context.WriteBitsRaw(AsBits(value), BitSizes.UShortSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUShortsRaw(this ref WriteContext context, ReadOnlySpan<ushort> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<ushort, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingUShorts = values.Length % NumberOfValuesInUlong;
        if (remainingUShorts != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingUShorts; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingUShorts + i])) << (i * BitSizes.UShortSize);
            }
            context.WriteBitsRaw(lastPacked, remainingUShorts * BitSizes.UShortSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort PeekUShortRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.UShortSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShortRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.UShortSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort[] PeekUShortArrayRaw(this ref ReadContext context, int count) {
        ushort[] result = new ushort[count];
        Span<ushort> span = result.AsSpan();
        context.PeekUShortSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort[] ReadUShortArrayRaw(this ref ReadContext context, int count) {
        ushort[] result = new ushort[count];
        Span<ushort> span = result.AsSpan();
        context.ReadUShortSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekUShortSpanRaw(this ref ReadContext context, int count, ref Span<ushort> destination) {
        int originalPosition = context.Position;
        context.ReadUShortSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadUShortSpanRaw(this ref ReadContext context, int count, ref Span<ushort> destination) {
        Span<ushort> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<ushort, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingUShorts = count % NumberOfValuesInUlong;
        if (remainingUShorts != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingUShorts * BitSizes.UShortSize);
            for (int i = 0; i < remainingUShorts; i++) {
                destination[count - remainingUShorts + i] = FromBits(lastPacked >> (i * BitSizes.UShortSize));
            }
        }
    }
}
