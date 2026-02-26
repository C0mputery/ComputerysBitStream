using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(uint), BitSizes.UIntSize)]
public static class RawUIntExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.UIntSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(uint value) => value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint FromBits(ulong value) => (uint)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUIntRaw(this ref WriteContext context, uint value) { context.WriteBitsRaw(AsBits(value), BitSizes.UIntSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUIntsRaw(this ref WriteContext context, ReadOnlySpan<uint> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<uint, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingUInts = values.Length % NumberOfValuesInUlong;
        if (remainingUInts != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingUInts; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingUInts + i])) << (i * BitSizes.UIntSize);
            }
            context.WriteBitsRaw(lastPacked, remainingUInts * BitSizes.UIntSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint PeekUIntRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.UIntSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUIntRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.UIntSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint[] PeekUIntArrayRaw(this ref ReadContext context, int count) {
        uint[] result = new uint[count];
        Span<uint> span = result.AsSpan();
        context.PeekUIntSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint[] ReadUIntArrayRaw(this ref ReadContext context, int count) {
        uint[] result = new uint[count];
        Span<uint> span = result.AsSpan();
        context.ReadUIntSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekUIntSpanRaw(this ref ReadContext context, int count, ref Span<uint> destination) {
        int originalPosition = context.Position;
        context.ReadUIntSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadUIntSpanRaw(this ref ReadContext context, int count, ref Span<uint> destination) {
        Span<uint> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<uint, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingUInts = count % NumberOfValuesInUlong;
        if (remainingUInts != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingUInts * BitSizes.UIntSize);
            for (int i = 0; i < remainingUInts; i++) {
                destination[count - remainingUInts + i] = FromBits(lastPacked >> (i * BitSizes.UIntSize));
            }
        }
    }
}
