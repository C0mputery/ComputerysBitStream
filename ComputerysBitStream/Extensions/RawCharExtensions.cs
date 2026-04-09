using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(char), BitHelper.CharSize)]
    public static class RawCharExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.CharSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(char value) => value;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char FromBits(ulong value) => (char)value;

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteCharRaw(this ref WriteContext context, char value) { context.WriteBitsRaw(AsBits(value), BitHelper.CharSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteCharsRaw(this ref WriteContext context, ReadOnlySpan<char> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<char, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingChars = values.Length % NumberOfValuesInUlong;
            if (remainingChars != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingChars; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingChars + i])) << (i * BitHelper.CharSize);
                }
                context.WriteBitsRaw(lastPacked, remainingChars * BitHelper.CharSize);
            }
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char PeekCharRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.CharSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadCharRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.CharSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] PeekCharArrayRaw(this ref ReadContext context, int count) {
            char[] result = new char[count];
            Span<char> span = result.AsSpan();
            context.PeekCharSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] ReadCharArrayRaw(this ref ReadContext context, int count) {
            char[] result = new char[count];
            Span<char> span = result.AsSpan();
            context.ReadCharSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekCharSpanRaw(this ref ReadContext context, int count, ref Span<char> destination) {
            int originalPosition = context.Position;
            context.ReadCharSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadCharSpanRaw(this ref ReadContext context, int count, ref Span<char> destination) {
            Span<char> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<char, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingChars = count % NumberOfValuesInUlong;
            if (remainingChars != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingChars * BitHelper.CharSize);
                for (int i = 0; i < remainingChars; i++) {
                    destination[count - remainingChars + i] = FromBits(lastPacked >> (i * BitHelper.CharSize));
                }
            }
        }
    }
}
