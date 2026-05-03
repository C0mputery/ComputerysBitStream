using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(ushort), BitHelper.UShortSize)]
    public static class RawUShortExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.UShortSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(ushort value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort FromBits(ulong value) => (ushort)value;

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUShortRaw(this ref WriteContext context, ushort value) { context.WriteBitsRaw(AsBits(value), BitHelper.UShortSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUShortsRaw(this ref WriteContext context, ReadOnlySpan<ushort> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<ushort, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingUShorts = values.Length % NumberOfValuesInUlong;
            if (remainingUShorts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingUShorts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingUShorts + i])) << (i * BitHelper.UShortSize);
                }
                context.WriteBitsRaw(lastPacked, remainingUShorts * BitHelper.UShortSize);
            }
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort PeekUShortRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.UShortSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUShortRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.UShortSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] PeekUShortArrayRaw(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            Span<ushort> span = result.AsSpan();
            context.PeekUShortSpanRaw(count, span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] ReadUShortArrayRaw(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            Span<ushort> span = result.AsSpan();
            context.ReadUShortSpanRaw(count, span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekUShortSpanRaw(this ref ReadContext context, int count, Span<ushort> destination) {
            int originalPosition = context.Position;
            context.ReadUShortSpanRaw(count, destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadUShortSpanRaw(this ref ReadContext context, int count, Span<ushort> destination) {
            Span<ushort> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<ushort, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingUShorts = count % NumberOfValuesInUlong;
            if (remainingUShorts != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingUShorts * BitHelper.UShortSize);
                for (int i = 0; i < remainingUShorts; i++) {
                    destination[count - remainingUShorts + i] = FromBits(lastPacked >> (i * BitHelper.UShortSize));
                }
            }
        }
    }
}
