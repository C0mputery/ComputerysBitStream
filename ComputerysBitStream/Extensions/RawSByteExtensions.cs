using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [BitStreamType(typeof(sbyte), BitHelper.SByteSize)]
    public static class RawSByteExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.SByteSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(sbyte value) => (byte)value;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static sbyte FromBits(ulong value) => (sbyte)(byte)value;

        [BitStreamRaw(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSByteRaw(this ref WriteContext context, sbyte value) { context.WriteBitsRaw(AsBits(value), BitHelper.SByteSize); }
    
        [BitStreamRaw(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSBytesRaw(this ref WriteContext context, ReadOnlySpan<sbyte> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<sbyte, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingSBytes = values.Length % NumberOfValuesInUlong;
            if (remainingSBytes != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingSBytes; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingSBytes + i])) << (i * BitHelper.SByteSize);
                }
                context.WriteBitsRaw(lastPacked, remainingSBytes * BitHelper.SByteSize);
            }
        }

        [BitStreamRaw(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte PeekSByteRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.SByteSize)); }

        [BitStreamRaw(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSByteRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.SByteSize)); }

        [BitStreamRaw(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte[] PeekSByteArrayRaw(this ref ReadContext context, int count) {
            sbyte[] result = new sbyte[count];
            Span<sbyte> span = result.AsSpan();
            context.PeekSByteSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRaw(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte[] ReadSByteArrayRaw(this ref ReadContext context, int count) {
            sbyte[] result = new sbyte[count];
            Span<sbyte> span = result.AsSpan();
            context.ReadSByteSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRaw(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekSByteSpanRaw(this ref ReadContext context, int count, ref Span<sbyte> destination) {
            int originalPosition = context.Position;
            context.ReadSByteSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRaw(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadSByteSpanRaw(this ref ReadContext context, int count, ref Span<sbyte> destination) {
            Span<sbyte> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<sbyte, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingSBytes = count % NumberOfValuesInUlong;
            if (remainingSBytes != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingSBytes * BitHelper.SByteSize);
                for (int i = 0; i < remainingSBytes; i++) {
                    destination[count - remainingSBytes + i] = FromBits(lastPacked >> (i * BitHelper.SByteSize));
                }
            }
        }
    }
}
