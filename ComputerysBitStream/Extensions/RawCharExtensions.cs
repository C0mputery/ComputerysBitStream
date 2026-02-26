using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

[BitStreamType(typeof(char), BitSizes.CharSize)]
public static class RawCharExtensions {
    private const int NumberOfValuesInUlong = BitSizes.ULongSize / BitSizes.CharSize;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong AsBits(char value) => value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char FromBits(ulong value) => (char)value;

    [BitStreamRaw(BitStreamRawRole.Write)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteCharRaw(this ref WriteContext context, char value) { context.WriteBitsRaw(AsBits(value), BitSizes.CharSize); }
    
    [BitStreamRaw(BitStreamRawRole.WriteSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteCharsRaw(this ref WriteContext context, ReadOnlySpan<char> values) {
        ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<char, ulong>(values);
        int totalUlongs = ulongs.Length;
        context.WriteBitsRaw(ulongs, totalUlongs * BitSizes.ULongSize);

        int remainingChars = values.Length % NumberOfValuesInUlong;
        if (remainingChars != 0) {
            ulong lastPacked = 0;
            for (int i = 0; i < remainingChars; i++) {
                lastPacked |= (AsBits(values[values.Length - remainingChars + i])) << (i * BitSizes.CharSize);
            }
            context.WriteBitsRaw(lastPacked, remainingChars * BitSizes.CharSize);
        }
    }

    [BitStreamRaw(BitStreamRawRole.Peek)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char PeekCharRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitSizes.CharSize)); }

    [BitStreamRaw(BitStreamRawRole.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ReadCharRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitSizes.CharSize)); }

    [BitStreamRaw(BitStreamRawRole.PeekArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char[] PeekCharArrayRaw(this ref ReadContext context, int count) {
        char[] result = new char[count];
        Span<char> span = result.AsSpan();
        context.PeekCharSpanRaw(count, ref span);
        return result;
    }

    [BitStreamRaw(BitStreamRawRole.ReadArray)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char[] ReadCharArrayRaw(this ref ReadContext context, int count) {
        char[] result = new char[count];
        Span<char> span = result.AsSpan();
        context.ReadCharSpanRaw(count, ref span);
        return result;
    }
    
    [BitStreamRaw(BitStreamRawRole.PeekSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PeekCharSpanRaw(this ref ReadContext context, int count, ref Span<char> destination) {
        int originalPosition = context.Position;
        context.ReadCharSpanRaw(count, ref destination);
        context.Position = originalPosition;
    }

    [BitStreamRaw(BitStreamRawRole.ReadSpan)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadCharSpanRaw(this ref ReadContext context, int count, ref Span<char> destination) {
        Span<char> targetSpan = destination.Slice(0, count);
        Span<ulong> ulongs = MemoryMarshal.Cast<char, ulong>(targetSpan);
        int totalUlongs = ulongs.Length;

        context.ReadBitsRaw(totalUlongs * BitSizes.ULongSize, ulongs);

        int remainingChars = count % NumberOfValuesInUlong;
        if (remainingChars != 0) {
            ulong lastPacked = context.ReadBitsRaw(remainingChars * BitSizes.CharSize);
            for (int i = 0; i < remainingChars; i++) {
                destination[count - remainingChars + i] = FromBits(lastPacked >> (i * BitSizes.CharSize));
            }
        }
    }
}
