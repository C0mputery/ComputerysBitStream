using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(long), BitHelper.LongSize)]
    public static class RawLongExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(long value) => (ulong)value;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long FromBits(ulong value) => (long)value;

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLongRaw(this ref WriteContext context, long value) { context.WriteBitsRaw(AsBits(value), BitHelper.LongSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLongsRaw(this ref WriteContext context, ReadOnlySpan<long> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(values);
            context.WriteBitsRaw(ulongs, ulongs.Length * BitHelper.ULongSize);
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PeekLongRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.LongSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLongRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.LongSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] PeekLongArrayRaw(this ref ReadContext context, int count) {
            long[] result = new long[count];
            Span<long> span = result.AsSpan();
            context.PeekLongSpanRaw(count, span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] ReadLongArrayRaw(this ref ReadContext context, int count) {
            long[] result = new long[count];
            Span<long> span = result.AsSpan();
            context.ReadLongSpanRaw(count, span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekLongSpanRaw(this ref ReadContext context, int count, Span<long> destination) {
            int originalPosition = context.Position;
            context.ReadLongSpanRaw(count, destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadLongSpanRaw(this ref ReadContext context, int count, Span<long> destination) {
            Span<long> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(targetSpan);
            context.ReadBitsRaw(ulongs.Length * BitHelper.ULongSize, ulongs);
        }
    }
}
