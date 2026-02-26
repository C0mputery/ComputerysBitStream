using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(float), BitSizes.FloatSize)]
public static class RawFloatExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.FloatSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(float value) {
        return Unsafe.As<float, uint>(ref value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float FromBits(ulong value) {
        uint temp = (uint)value;
        return Unsafe.As<uint, float>(ref temp);
    }
    
    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteFloatRaw(this ref WriteContext context, float value) { context.WriteBitsRaw(AsBits(value), BitSizes.FloatSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteFloatsRaw(this ref WriteContext context, ReadOnlySpan<float> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<float, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingFloats = values.Length % NumberOfValuesInUlong;
        if (remainingFloats != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingFloats; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingFloats + i])) << (i * BitSizes.FloatSize);
            }
            context.WriteBitsRaw(lastPacked, remainingFloats * BitSizes.FloatSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PeekFloatRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.FloatSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloatRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.FloatSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float[] PeekFloatArrayRaw(this ref ReadContext context, int count) {
        float[] result = new float[count];
        Span<float> span = result.AsSpan();
        context.PeekFloatSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float[] ReadFloatArrayRaw(this ref ReadContext context, int count) {
        float[] result = new float[count];
        Span<float> span = result.AsSpan();
        context.ReadFloatSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekFloatSpanRaw(this ref ReadContext context, int count, ref Span<float> destination) {
        int originalPosition = context.Position;
        context.ReadFloatSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadFloatSpanRaw(this ref ReadContext context, int count, ref Span<float> destination) {
        Span<float> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<float, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingFloats = count % NumberOfValuesInUlong;
        if (remainingFloats != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingFloats * BitSizes.FloatSize);
            for (int i = 0; i < remainingFloats; i++) {
                destination[count - remainingFloats + i] = FromBits(lastPacked >> (i * BitSizes.FloatSize));
            }
        }
    }
}
