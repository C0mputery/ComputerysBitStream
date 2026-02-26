using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(long), BitSizes.LongSize)]
public static class RawLongExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.LongSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(long value) => (ulong)value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long FromBits(ulong value) => (long)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLongRaw(this ref WriteContext context, long value) { context.WriteBitsRaw(AsBits(value), BitSizes.LongSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLongsRaw(this ref WriteContext context, ReadOnlySpan<long> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(values);
        context.WriteBitsRaw(ulongs, ulongs.Length * BitSizes.ULongSize);
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long PeekLongRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.LongSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadLongRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.LongSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long[] PeekLongArrayRaw(this ref ReadContext context, int count) {
        long[] result = new long[count];
        Span<long> span = result.AsSpan();
        context.PeekLongSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long[] ReadLongArrayRaw(this ref ReadContext context, int count) {
        long[] result = new long[count];
        Span<long> span = result.AsSpan();
        context.ReadLongSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekLongSpanRaw(this ref ReadContext context, int count, ref Span<long> destination) {
        int originalPosition = context.Position;
        context.ReadLongSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadLongSpanRaw(this ref ReadContext context, int count, ref Span<long> destination) {
        Span<long> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(targetSpan);
        context.ReadBitsRaw(ulongs.Length * BitSizes.ULongSize, ulongs);
    }
}
