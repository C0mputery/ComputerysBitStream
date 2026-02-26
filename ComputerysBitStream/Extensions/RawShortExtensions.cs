using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(short), BitSizes.ShortSize)]
public static class RawShortExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.ShortSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(short value) => (ushort)value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static short FromBits(ulong value) => (short)(ushort)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteShortRaw(this ref WriteContext context, short value) { context.WriteBitsRaw(AsBits(value), BitSizes.ShortSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteShortsRaw(this ref WriteContext context, ReadOnlySpan<short> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<short, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingShorts = values.Length % NumberOfValuesInUlong;
        if (remainingShorts != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingShorts; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingShorts + i])) << (i * BitSizes.ShortSize);
            }
            context.WriteBitsRaw(lastPacked, remainingShorts * BitSizes.ShortSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short PeekShortRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.ShortSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ReadShortRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.ShortSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short[] PeekShortArrayRaw(this ref ReadContext context, int count) {
        short[] result = new short[count];
        Span<short> span = result.AsSpan();
        context.PeekShortSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short[] ReadShortArrayRaw(this ref ReadContext context, int count) {
        short[] result = new short[count];
        Span<short> span = result.AsSpan();
        context.ReadShortSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekShortSpanRaw(this ref ReadContext context, int count, ref Span<short> destination) {
        int originalPosition = context.Position;
        context.ReadShortSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadShortSpanRaw(this ref ReadContext context, int count, ref Span<short> destination) {
        Span<short> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<short, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingShorts = count % NumberOfValuesInUlong;
        if (remainingShorts != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingShorts * BitSizes.ShortSize);
            for (int i = 0; i < remainingShorts; i++) {
                destination[count - remainingShorts + i] = FromBits(lastPacked >> (i * BitSizes.ShortSize));
            }
        }
    }
}
