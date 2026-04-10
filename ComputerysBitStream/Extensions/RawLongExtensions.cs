using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [BitStreamType(typeof(long), BitHelper.LongSize)]
    public static class RawLongExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(long value) => (ulong)value;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long FromBits(ulong value) => (long)value;

        [BitStreamRaw(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLongRaw(this ref WriteContext context, long value) { context.WriteBitsRaw(AsBits(value), BitHelper.LongSize); }
    
        [BitStreamRaw(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLongsRaw(this ref WriteContext context, ReadOnlySpan<long> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(values);
            context.WriteBitsRaw(ulongs, ulongs.Length * BitHelper.ULongSize);
        }

        [BitStreamRaw(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PeekLongRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.LongSize)); }

        [BitStreamRaw(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLongRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.LongSize)); }

        [BitStreamRaw(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] PeekLongArrayRaw(this ref ReadContext context, int count) {
            long[] result = new long[count];
            Span<long> span = result.AsSpan();
            context.PeekLongSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRaw(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] ReadLongArrayRaw(this ref ReadContext context, int count) {
            long[] result = new long[count];
            Span<long> span = result.AsSpan();
            context.ReadLongSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRaw(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekLongSpanRaw(this ref ReadContext context, int count, ref Span<long> destination) {
            int originalPosition = context.Position;
            context.ReadLongSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRaw(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadLongSpanRaw(this ref ReadContext context, int count, ref Span<long> destination) {
            Span<long> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(targetSpan);
            context.ReadBitsRaw(ulongs.Length * BitHelper.ULongSize, ulongs);
        }
    }
}
