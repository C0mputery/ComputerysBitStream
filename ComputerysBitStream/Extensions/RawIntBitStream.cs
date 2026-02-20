using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(int), BitSizes.IntSize)]
public static class RawIntBitStream {
    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteIntRaw(this ref WriteContext context, int value) { context.WriteBitsRaw((uint)value, BitSizes.IntSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteIntsRaw(this ref WriteContext context, ReadOnlySpan<int> values) {
        const int numberOfValuesInUlong = BitSizes.ULongSize / BitSizes.IntSize;

        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingInts = values.Length % numberOfValuesInUlong;
        if (remainingInts != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingInts; i++) {
                lastPacked |= (ulong)(uint)values[values.Length - remainingInts + i] << (i * BitSizes.IntSize);
            }
            context.WriteBitsRaw(lastPacked, remainingInts * BitSizes.IntSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peak)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PeakIntRaw(this ref ReadContext context) { return (int)context.PeakBitsRaw(BitSizes.IntSize); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadIntRaw(this ref ReadContext context) {
        int value = context.PeakIntRaw();
        context.Position += BitSizes.IntSize;
        return value;
    }

    [BitStreamRaw(BitStreamRawRole.PeakArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] PeakIntArrayRaw(this ref ReadContext context, int count) {
        int[] result = new int[count];
        for (int i = 0; i < count; i++) { result[i] = context.PeakIntRaw(); }
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] ReadIntArrayRaw(this ref ReadContext context, int count) {
        int[] values = context.PeakIntArrayRaw(count);
        context.Position += count * BitSizes.IntSize;
        return values;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeakSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeakIntSpanRaw(this ref ReadContext context, int count, ref Span<int> result) {
        for (int i = 0; i < count; i++) { result[i] = context.PeakIntRaw(); }
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadIntSpanRaw(this ref ReadContext context, int count, ref Span<int> result) {
        context.PeakIntSpanRaw(count, ref result);
        context.Position += count * BitSizes.IntSize;
    }
}
