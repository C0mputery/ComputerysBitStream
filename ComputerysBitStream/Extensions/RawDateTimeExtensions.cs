using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(DateTime), BitHelper.DateTimeSize)]
    public static class RawDateTimeExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(DateTime value) => (ulong)value.ToBinary();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DateTime FromBits(ulong value) => DateTime.FromBinary((long)value);

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDateTimeRaw(this ref WriteContext context, DateTime value) { context.WriteBitsRaw(AsBits(value), BitHelper.DateTimeSize); }

        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDateTimesRaw(this ref WriteContext context, ReadOnlySpan<DateTime> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<DateTime, ulong>(values);
            context.WriteBitsRaw(ulongs, ulongs.Length * BitHelper.ULongSize);
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime PeekDateTimeRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.DateTimeSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ReadDateTimeRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.DateTimeSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime[] PeekDateTimeArrayRaw(this ref ReadContext context, int count) {
            DateTime[] result = new DateTime[count];
            Span<DateTime> span = result.AsSpan();
            context.PeekDateTimeSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime[] ReadDateTimeArrayRaw(this ref ReadContext context, int count) {
            DateTime[] result = new DateTime[count];
            Span<DateTime> span = result.AsSpan();
            context.ReadDateTimeSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekDateTimeSpanRaw(this ref ReadContext context, int count, ref Span<DateTime> destination) {
            int originalPosition = context.Position;
            context.ReadDateTimeSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadDateTimeSpanRaw(this ref ReadContext context, int count, ref Span<DateTime> destination) {
            Span<DateTime> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<DateTime, ulong>(targetSpan);
            context.ReadBitsRaw(ulongs.Length * BitHelper.ULongSize, ulongs);
        }
    }
}
