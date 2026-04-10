using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [BitStreamType(typeof(ushort), BitHelper.UShortSize)]
    public static class RawUShortExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.UShortSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(ushort value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort FromBits(ulong value) => (ushort)value;

        [BitStreamRaw(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUShortRaw(this ref WriteContext context, ushort value) { context.WriteBitsRaw(AsBits(value), BitHelper.UShortSize); }
    
        [BitStreamRaw(BitStreamRawRole.WriteSpan)]
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

        [BitStreamRaw(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort PeekUShortRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.UShortSize)); }

        [BitStreamRaw(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUShortRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.UShortSize)); }

        [BitStreamRaw(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] PeekUShortArrayRaw(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            Span<ushort> span = result.AsSpan();
            context.PeekUShortSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRaw(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] ReadUShortArrayRaw(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            Span<ushort> span = result.AsSpan();
            context.ReadUShortSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRaw(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekUShortSpanRaw(this ref ReadContext context, int count, ref Span<ushort> destination) {
            int originalPosition = context.Position;
            context.ReadUShortSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRaw(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadUShortSpanRaw(this ref ReadContext context, int count, ref Span<ushort> destination) {
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
