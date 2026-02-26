using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(double), BitSizes.DoubleSize)]
public static class RawDoubleExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.DoubleSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(double value) => Unsafe.As<double, ulong>(ref value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FromBits(ulong value) => Unsafe.As<ulong, double>(ref value);

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDoubleRaw(this ref WriteContext context, double value) { context.WriteBitsRaw(AsBits(value), BitSizes.DoubleSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDoublesRaw(this ref WriteContext context, ReadOnlySpan<double> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<double, ulong>(values);
        context.WriteBitsRaw(ulongs, ulongs.Length * BitSizes.ULongSize);
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double PeekDoubleRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.DoubleSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDoubleRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.DoubleSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double[] PeekDoubleArrayRaw(this ref ReadContext context, int count) {
        double[] result = new double[count];
        Span<double> span = result.AsSpan();
        context.PeekDoubleSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double[] ReadDoubleArrayRaw(this ref ReadContext context, int count) {
        double[] result = new double[count];
        Span<double> span = result.AsSpan();
        context.ReadDoubleSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekDoubleSpanRaw(this ref ReadContext context, int count, ref Span<double> destination) {
        int originalPosition = context.Position;
        context.ReadDoubleSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadDoubleSpanRaw(this ref ReadContext context, int count, ref Span<double> destination) {
        Span<double> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<double, ulong>(targetSpan);
        context.ReadBitsRaw(ulongs.Length * BitSizes.ULongSize, ulongs);
    }
}
