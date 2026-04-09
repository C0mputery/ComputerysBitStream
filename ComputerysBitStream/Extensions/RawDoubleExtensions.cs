using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(double), BitHelper.DoubleSize)]
    public static class RawDoubleExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(double value) => (ulong)BitConverter.DoubleToInt64Bits(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double FromBits(ulong value) => BitConverter.Int64BitsToDouble((long)value);
    
        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDoubleRaw(this ref WriteContext context, double value) { context.WriteBitsRaw(AsBits(value), BitHelper.DoubleSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDoublesRaw(this ref WriteContext context, ReadOnlySpan<double> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<double, ulong>(values);
            context.WriteBitsRaw(ulongs, ulongs.Length * BitHelper.ULongSize);
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PeekDoubleRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.DoubleSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDoubleRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.DoubleSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] PeekDoubleArrayRaw(this ref ReadContext context, int count) {
            double[] result = new double[count];
            Span<double> span = result.AsSpan();
            context.PeekDoubleSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] ReadDoubleArrayRaw(this ref ReadContext context, int count) {
            double[] result = new double[count];
            Span<double> span = result.AsSpan();
            context.ReadDoubleSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekDoubleSpanRaw(this ref ReadContext context, int count, ref Span<double> destination) {
            int originalPosition = context.Position;
            context.ReadDoubleSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadDoubleSpanRaw(this ref ReadContext context, int count, ref Span<double> destination) {
            Span<double> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<double, ulong>(targetSpan);
            context.ReadBitsRaw(ulongs.Length * BitHelper.ULongSize, ulongs);
        }
    }
}
