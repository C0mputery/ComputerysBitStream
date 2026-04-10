using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [BitStreamType(typeof(int), BitHelper.IntSize)]
    public static class RawIntExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.IntSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(int value) => (uint)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FromBits(ulong value) => (int)(uint)value;

        [BitStreamRaw(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIntRaw(this ref WriteContext context, int value) { context.WriteBitsRaw(AsBits(value), BitHelper.IntSize); }
    
        [BitStreamRaw(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIntsRaw(this ref WriteContext context, ReadOnlySpan<int> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingInts = values.Length % NumberOfValuesInUlong;
            if (remainingInts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingInts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingInts + i])) << (i * BitHelper.IntSize);
                }
                context.WriteBitsRaw(lastPacked, remainingInts * BitHelper.IntSize);
            }
        }

        [BitStreamRaw(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PeekIntRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.IntSize)); }

        [BitStreamRaw(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadIntRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.IntSize)); }

        [BitStreamRaw(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] PeekIntArrayRaw(this ref ReadContext context, int count) {
            int[] result = new int[count];
            Span<int> span = result.AsSpan();
            context.PeekIntSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRaw(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] ReadIntArrayRaw(this ref ReadContext context, int count) {
            int[] result = new int[count];
            Span<int> span = result.AsSpan();
            context.ReadIntSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRaw(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekIntSpanRaw(this ref ReadContext context, int count, ref Span<int> destination) {
            int originalPosition = context.Position;
            context.ReadIntSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRaw(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadIntSpanRaw(this ref ReadContext context, int count, ref Span<int> destination) {
            Span<int> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingInts = count % NumberOfValuesInUlong;
            if (remainingInts != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingInts * BitHelper.IntSize);
                for (int i = 0; i < remainingInts; i++) {
                    destination[count - remainingInts + i] = FromBits(lastPacked >> (i * BitHelper.IntSize));
                }
            }
        }
    }
}
