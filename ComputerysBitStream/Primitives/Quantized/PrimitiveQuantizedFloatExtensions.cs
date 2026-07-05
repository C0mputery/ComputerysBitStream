using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.Quantized {
    [BitStreamQuantizedPrimitive(QuantizedEncodingHelper.MinimumBits, BitHelper.FloatSize)]
    [BitStreamPrimitive(typeof(float), "QuantizedFloat", PrimitiveSerializationMode.Quantized)]
    public static class PrimitiveQuantizedFloatExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(float value, float min, float max, int bitCount) {
            float normalized = (value - min) / (max - min);
            if (normalized <= 0f) {
                return 0;
            }

            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            if (normalized >= 1f) {
                return maxValue;
            }

            return (ulong)(normalized * maxValue + QuantizedEncodingHelper.RoundBiasFloat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float FromBits(ulong value, float min, float max, int bitCount) {
            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            float normalized = value / (float)maxValue;
            return min + normalized * (max - min);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedFloatPrimitive(this ref WriteContext context, float value, float min, float max, int bitCount) {
            context.WriteBitsPrimitive(AsBits(value, min, max, bitCount), bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedFloatsPrimitive(this ref WriteContext context, ReadOnlySpan<float> values, float min, float max, int bitCount) {
            int valuesPerUlong = BitHelper.ULongSize / bitCount;
            int totalFullUlongs = values.Length / valuesPerUlong;
            int remainingValues = values.Length % valuesPerUlong;

            for (int i = 0; i < totalFullUlongs; i++) {
                ulong packed = 0;
                for (int j = 0; j < valuesPerUlong; j++) {
                    packed |= AsBits(values[i * valuesPerUlong + j], min, max, bitCount) << (j * bitCount);
                }

                context.WriteBitsPrimitive(packed, valuesPerUlong * bitCount);
            }

            if (remainingValues != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingValues; i++) {
                    lastPacked |= AsBits(values[values.Length - remainingValues + i], min, max, bitCount) << (i * bitCount);
                }

                context.WriteBitsPrimitive(lastPacked, remainingValues * bitCount);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PeekQuantizedFloatPrimitive(this ref ReadContext context, float min, float max, int bitCount) {
            return FromBits(context.PeekBitsPrimitive(bitCount), min, max, bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadQuantizedFloatPrimitive(this ref ReadContext context, float min, float max, int bitCount) {
            return FromBits(context.ReadBitsPrimitive(bitCount), min, max, bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] PeekQuantizedFloatArrayPrimitive(this ref ReadContext context, int count, float min, float max, int bitCount) {
            float[] result = new float[count];
            context.PeekQuantizedFloatSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ReadQuantizedFloatArrayPrimitive(this ref ReadContext context, int count, float min, float max, int bitCount) {
            float[] result = new float[count];
            context.ReadQuantizedFloatSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedFloatSpanPrimitive(this ref ReadContext context, int count, Span<float> destination, float min, float max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedFloatSpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedFloatSpanPrimitive(this ref ReadContext context, int count, Span<float> destination, float min, float max, int bitCount) {
            Span<float> destinationSlice = destination.Slice(0, count);
            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            int valuesPerUlong = BitHelper.ULongSize / bitCount;
            int totalFullUlongs = count / valuesPerUlong;
            int remainingValues = count % valuesPerUlong;

            for (int i = 0; i < totalFullUlongs; i++) {
                ulong packed = context.ReadBitsPrimitive(valuesPerUlong * bitCount);
                for (int j = 0; j < valuesPerUlong; j++) {
                    destinationSlice[i * valuesPerUlong + j] = FromBits((packed >> (j * bitCount)) & maxValue, min, max, bitCount);
                }
            }

            if (remainingValues != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingValues * bitCount);
                for (int i = 0; i < remainingValues; i++) {
                    destinationSlice[count - remainingValues + i] = FromBits((lastPacked >> (i * bitCount)) & maxValue, min, max, bitCount);
                }
            }
        }
    }
}
