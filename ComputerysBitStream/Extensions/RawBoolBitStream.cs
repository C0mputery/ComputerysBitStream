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
                if (values[processed + i]) {
                    packed |= (1UL << i);
                }
            }
            context.WriteBitsRaw(packed, BitSizes.ULongSize);
            processed += numberOfValuesInUlong;
        }

        int remaining = count - processed;
        if (remaining > 0) {
            ulong packed = 0;
            for (int i = 0; i < remaining; i++) {
                if (values[processed + i]) {
                    packed |= (1UL << i);
                }
            }
            context.WriteBitsRaw(packed, remaining * BitSizes.BoolSize);
        }
    }
    
    [BitStreamRaw(BitStreamRawRole.Peak)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool PeakBoolRaw(this ref ReadContext context) { return context.PeakBitRaw(); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBoolRaw(this ref ReadContext context) {
        bool value = context.PeakBoolRaw();
        context.Position += BitSizes.BoolSize;
        return value;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeakArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool[] PeakBoolArrayRaw(this ref ReadContext context, int count) {
        bool[] result = new bool[count];
        for (int i = 0; i < count; i++) { result[i] = context.PeakBoolRaw(); }
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool[] ReadBoolArrayRaw(this ref ReadContext context, int count) {
        bool[] values = context.PeakBoolArrayRaw(count);
        context.Position += count * BitSizes.BoolSize;
        return values;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeakSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeakBoolSpanRaw(this ref ReadContext context, int count, ref Span<bool> result) {
        for (int i = 0; i < count; i++) { result[i] = context.PeakBoolRaw(); }
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadBoolSpanRaw(this ref ReadContext context, int count, ref Span<bool> result) {
        context.PeakBoolSpanRaw(count, ref result);
        context.Position += count * BitSizes.BoolSize;
    }
}
