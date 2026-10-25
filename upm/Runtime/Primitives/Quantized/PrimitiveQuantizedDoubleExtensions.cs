using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.Quantized {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamQuantizedPrimitive(QuantizedEncodingHelper.MinimumBits, BitHelper.DoubleSize)]
    [BitStreamPrimitive(typeof(double), "QuantizedDouble", PrimitiveSerializationMode.Quantized)]
    public static class PrimitiveQuantizedDoubleExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(double value, double min, double max, int bitCount) {
            double normalized = (value - min) / (max - min);
            if (normalized <= 0d) {
                return 0;
            }

            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            if (normalized >= 1d) {
                return maxValue;
            }

            return (ulong)(normalized * maxValue + QuantizedEncodingHelper.RoundBiasDouble);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double FromBits(ulong value, double min, double max, int bitCount) {
            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            double normalized = value / (double)maxValue;
            return min + normalized * (max - min);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedDoublePrimitive(this ref WriteContext context, double value, double min, double max, int bitCount) {
            context.WriteBitsPrimitive(AsBits(value, min, max, bitCount), bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedDoublesPrimitive(this ref WriteContext context, ReadOnlySpan<double> values, double min, double max, int bitCount) {
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

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PeekQuantizedDoublePrimitive(this ref ReadContext context, double min, double max, int bitCount) {
            return FromBits(context.PeekBitsPrimitive(bitCount), min, max, bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadQuantizedDoublePrimitive(this ref ReadContext context, double min, double max, int bitCount) {
            return FromBits(context.ReadBitsPrimitive(bitCount), min, max, bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] PeekQuantizedDoubleArrayPrimitive(this ref ReadContext context, int count, double min, double max, int bitCount) {
            double[] result = new double[count];
            context.PeekQuantizedDoubleSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] ReadQuantizedDoubleArrayPrimitive(this ref ReadContext context, int count, double min, double max, int bitCount) {
            double[] result = new double[count];
            context.ReadQuantizedDoubleSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedDoubleSpanPrimitive(this ref ReadContext context, int count, Span<double> destination, double min, double max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedDoubleSpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedDoubleSpanPrimitive(this ref ReadContext context, int count, Span<double> destination, double min, double max, int bitCount) {
            Span<double> destinationSlice = destination.Slice(0, count);
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
