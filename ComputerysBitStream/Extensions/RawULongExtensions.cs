using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(ulong), BitHelper.ULongSize)]
    public static class RawULongExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(ulong value) => value;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong FromBits(ulong value) => value;

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteULongRaw(this ref WriteContext context, ulong value) { context.WriteBitsRaw(AsBits(value), BitHelper.ULongSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteULongsRaw(this ref WriteContext context, ReadOnlySpan<ulong> values) {
            context.WriteBitsRaw(values, values.Length * BitHelper.ULongSize);
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong PeekULongRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.ULongSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadULongRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.ULongSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong[] PeekULongArrayRaw(this ref ReadContext context, int count) {
            ulong[] result = new ulong[count];
            Span<ulong> span = result.AsSpan();
            context.PeekULongSpanRaw(count, span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong[] ReadULongArrayRaw(this ref ReadContext context, int count) {
            ulong[] result = new ulong[count];
            Span<ulong> span = result.AsSpan();
            context.ReadULongSpanRaw(count, span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekULongSpanRaw(this ref ReadContext context, int count, Span<ulong> destination) {
            int originalPosition = context.Position;
            context.ReadULongSpanRaw(count, destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadULongSpanRaw(this ref ReadContext context, int count, Span<ulong> destination) {
            Span<ulong> targetSpan = destination.Slice(0, count);
            context.ReadBitsRaw(count * BitHelper.ULongSize, targetSpan);
        }
    }
}
