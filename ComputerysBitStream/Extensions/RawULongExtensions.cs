using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(ulong), BitSizes.ULongSize)]
public static class RawULongExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.ULongSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(ulong value) => value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong FromBits(ulong value) => value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteULongRaw(this ref WriteContext context, ulong value) { context.WriteBitsRaw(AsBits(value), BitSizes.ULongSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteULongsRaw(this ref WriteContext context, ReadOnlySpan<ulong> values) {
        context.WriteBitsRaw(values, values.Length * BitSizes.ULongSize);
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong PeekULongRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.ULongSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadULongRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.ULongSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong[] PeekULongArrayRaw(this ref ReadContext context, int count) {
        ulong[] result = new ulong[count];
        Span<ulong> span = result.AsSpan();
        context.PeekULongSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong[] ReadULongArrayRaw(this ref ReadContext context, int count) {
        ulong[] result = new ulong[count];
        Span<ulong> span = result.AsSpan();
        context.ReadULongSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekULongSpanRaw(this ref ReadContext context, int count, ref Span<ulong> destination) {
        int originalPosition = context.Position;
        context.ReadULongSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadULongSpanRaw(this ref ReadContext context, int count, ref Span<ulong> destination) {
        Span<ulong> targetSpan = destination.Slice(0, count);
        context.ReadBitsRaw(count * BitSizes.ULongSize, targetSpan);
    }
}
