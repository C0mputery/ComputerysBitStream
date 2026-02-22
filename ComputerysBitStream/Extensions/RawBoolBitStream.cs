using System;
using System.Runtime.CompilerServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(bool), BitSizes.BoolSize)]
public static class RawBoolBitStream {
    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteBoolRaw(this ref WriteContext context, bool value) { context.WriteBitRaw(value); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteBoolsRaw(this ref WriteContext context, ReadOnlySpan<bool> values) {
        const int numberOfValuesInUlong = BitSizes.ULongSize / BitSizes.BoolSize;
        int count = values.Length;
        int processed = 0;

        while (processed + numberOfValuesInUlong <= count) {
            ulong packed = 0;
            for (int i = 0; i < numberOfValuesInUlong; i++) {
                packed |= (ulong)(values[processed + i] ? 1 : 0) << i;
            }
            context.WriteBitsRaw(packed, BitSizes.ULongSize);
            processed += numberOfValuesInUlong;
        }

        int remaining = count - processed;
        if (remaining > 0) {
            ulong packed = 0;
            for (int i = 0; i < remaining; i++) {
                packed |= (ulong)(values[processed + i] ? 1 : 0) << i;
            }
            context.WriteBitsRaw(packed, remaining * BitSizes.BoolSize);
        }
    }
    
    [BitStreamRaw(BitStreamRawRole.Peak)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool PeakBoolRaw(this ref ReadContext context) { return context.PeakBitRaw(); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBoolRaw(this ref ReadContext context) { return context.ReadBitRaw(); }
    
    [BitStreamRaw(BitStreamRawRole.PeakArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool[] PeakBoolArrayRaw(this ref ReadContext context, int count) {
        bool[] result = new bool[count];
        Span<bool> span = result.AsSpan();
        context.PeakBoolSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool[] ReadBoolArrayRaw(this ref ReadContext context, int count) {
        bool[] result = new bool[count];
        Span<bool> span = result.AsSpan();
        context.ReadBoolSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeakSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeakBoolSpanRaw(this ref ReadContext context, int count, ref Span<bool> result) {
        int originalPosition = context.Position;
        context.ReadBoolSpanRaw(count, ref result);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadBoolSpanRaw(this ref ReadContext context, int count, ref Span<bool> result) {
        const int numberOfValuesInUlong = BitSizes.ULongSize / BitSizes.BoolSize;
        int processed = 0;

        while (processed + numberOfValuesInUlong <= count) {
            ulong packed = context.ReadBitsRaw(BitSizes.ULongSize);
            for (int i = 0; i < numberOfValuesInUlong; i++) {
                result[processed + i] = (packed & (1UL << i)) != 0;
            }
            processed += numberOfValuesInUlong;
        }

        int remaining = count - processed;
        if (remaining > 0) {
            ulong packed = context.ReadBitsRaw(remaining * BitSizes.BoolSize);
            for (int i = 0; i < remaining; i++) {
                result[processed + i] = (packed & (1UL << i)) != 0;
            }
        }
    }
}
