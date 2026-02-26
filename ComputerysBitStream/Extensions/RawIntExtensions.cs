using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(int), BitSizes.IntSize)]
public static class RawIntExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.IntSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(int value) => (uint)value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int FromBits(ulong value) => (int)(uint)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteIntRaw(this ref WriteContext context, int value) { context.WriteBitsRaw(AsBits(value), BitSizes.IntSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteIntsRaw(this ref WriteContext context, ReadOnlySpan<int> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingInts = values.Length % NumberOfValuesInUlong;
        if (remainingInts != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingInts; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingInts + i])) << (i * BitSizes.IntSize);
            }
            context.WriteBitsRaw(lastPacked, remainingInts * BitSizes.IntSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PeekIntRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.IntSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadIntRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.IntSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] PeekIntArrayRaw(this ref ReadContext context, int count) {
        int[] result = new int[count];
        Span<int> span = result.AsSpan();
        context.PeekIntSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] ReadIntArrayRaw(this ref ReadContext context, int count) {
        int[] result = new int[count];
        Span<int> span = result.AsSpan();
        context.ReadIntSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekIntSpanRaw(this ref ReadContext context, int count, ref Span<int> destination) {
        int originalPosition = context.Position;
        context.ReadIntSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadIntSpanRaw(this ref ReadContext context, int count, ref Span<int> destination) {
        Span<int> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingInts = count % NumberOfValuesInUlong;
        if (remainingInts != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingInts * BitSizes.IntSize);
            for (int i = 0; i < remainingInts; i++) {
                destination[count - remainingInts + i] = FromBits(lastPacked >> (i * BitSizes.IntSize));
            }
        }
    }
}
