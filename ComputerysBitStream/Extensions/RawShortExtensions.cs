using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(short), BitHelper.ShortSize)]
    public static class RawShortExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.ShortSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(short value) => (ushort)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static short FromBits(ulong value) => (short)(ushort)value;

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteShortRaw(this ref WriteContext context, short value) { context.WriteBitsRaw(AsBits(value), BitHelper.ShortSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteShortsRaw(this ref WriteContext context, ReadOnlySpan<short> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<short, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingShorts = values.Length % NumberOfValuesInUlong;
            if (remainingShorts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingShorts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingShorts + i])) << (i * BitHelper.ShortSize);
                }
                context.WriteBitsRaw(lastPacked, remainingShorts * BitHelper.ShortSize);
            }
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short PeekShortRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.ShortSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadShortRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.ShortSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short[] PeekShortArrayRaw(this ref ReadContext context, int count) {
            short[] result = new short[count];
            Span<short> span = result.AsSpan();
            context.PeekShortSpanRaw(count, span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short[] ReadShortArrayRaw(this ref ReadContext context, int count) {
            short[] result = new short[count];
            Span<short> span = result.AsSpan();
            context.ReadShortSpanRaw(count, span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekShortSpanRaw(this ref ReadContext context, int count, Span<short> destination) {
            int originalPosition = context.Position;
            context.ReadShortSpanRaw(count, destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadShortSpanRaw(this ref ReadContext context, int count, Span<short> destination) {
            Span<short> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<short, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingShorts = count % NumberOfValuesInUlong;
            if (remainingShorts != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingShorts * BitHelper.ShortSize);
                for (int i = 0; i < remainingShorts; i++) {
                    destination[count - remainingShorts + i] = FromBits(lastPacked >> (i * BitHelper.ShortSize));
                }
            }
        }
    }
}
