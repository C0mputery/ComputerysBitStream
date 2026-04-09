using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(float), BitHelper.FloatSize)]
    public static class RawFloatExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.FloatSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(float value) => (uint)BitConverter.SingleToInt32Bits(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float FromBits(ulong value) => BitConverter.Int32BitsToSingle((int)(uint)value);

        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloatRaw(this ref WriteContext context, float value) { context.WriteBitsRaw(AsBits(value), BitHelper.FloatSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloatsRaw(this ref WriteContext context, ReadOnlySpan<float> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<float, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingFloats = values.Length % NumberOfValuesInUlong;
            if (remainingFloats != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingFloats; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingFloats + i])) << (i * BitHelper.FloatSize);
                }
                context.WriteBitsRaw(lastPacked, remainingFloats * BitHelper.FloatSize);
            }
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PeekFloatRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.FloatSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloatRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.FloatSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] PeekFloatArrayRaw(this ref ReadContext context, int count) {
            float[] result = new float[count];
            Span<float> span = result.AsSpan();
            context.PeekFloatSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ReadFloatArrayRaw(this ref ReadContext context, int count) {
            float[] result = new float[count];
            Span<float> span = result.AsSpan();
            context.ReadFloatSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekFloatSpanRaw(this ref ReadContext context, int count, ref Span<float> destination) {
            int originalPosition = context.Position;
            context.ReadFloatSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadFloatSpanRaw(this ref ReadContext context, int count, ref Span<float> destination) {
            Span<float> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<float, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingFloats = count % NumberOfValuesInUlong;
            if (remainingFloats != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingFloats * BitHelper.FloatSize);
                for (int i = 0; i < remainingFloats; i++) {
                    destination[count - remainingFloats + i] = FromBits(lastPacked >> (i * BitHelper.FloatSize));
                }
            }
        }
    }
}
