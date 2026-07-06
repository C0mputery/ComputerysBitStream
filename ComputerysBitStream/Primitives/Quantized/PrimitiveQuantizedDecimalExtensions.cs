using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.Quantized {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamQuantizedPrimitive(QuantizedEncodingHelper.MinimumBits, BitHelper.ULongSize)]
    [BitStreamPrimitive(typeof(decimal), "QuantizedDecimal", PrimitiveSerializationMode.Quantized)]
    public static class PrimitiveQuantizedDecimalExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(decimal value, decimal min, decimal max, int bitCount) {
            decimal normalized = (value - min) / (max - min);
            if (normalized <= 0m) {
                return 0;
            }

            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            if (normalized >= 1m) {
                return maxValue;
            }

            return (ulong)(normalized * maxValue + QuantizedEncodingHelper.RoundBiasDecimal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static decimal FromBits(ulong value, decimal min, decimal max, int bitCount) {
            ulong maxValue = QuantizedEncodingHelper.MaxQuantizedValue(bitCount);
            decimal normalized = value / (decimal)maxValue;
            return min + normalized * (max - min);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedDecimalPrimitive(this ref WriteContext context, decimal value, decimal min, decimal max, int bitCount) {
            context.WriteBitsPrimitive(AsBits(value, min, max, bitCount), bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedDecimalsPrimitive(this ref WriteContext context, ReadOnlySpan<decimal> values, decimal min, decimal max, int bitCount) {
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
        public static decimal PeekQuantizedDecimalPrimitive(this ref ReadContext context, decimal min, decimal max, int bitCount) {
            return FromBits(context.PeekBitsPrimitive(bitCount), min, max, bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal ReadQuantizedDecimalPrimitive(this ref ReadContext context, decimal min, decimal max, int bitCount) {
            return FromBits(context.ReadBitsPrimitive(bitCount), min, max, bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal[] PeekQuantizedDecimalArrayPrimitive(this ref ReadContext context, int count, decimal min, decimal max, int bitCount) {
            decimal[] result = new decimal[count];
            context.PeekQuantizedDecimalSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal[] ReadQuantizedDecimalArrayPrimitive(this ref ReadContext context, int count, decimal min, decimal max, int bitCount) {
            decimal[] result = new decimal[count];
            context.ReadQuantizedDecimalSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedDecimalSpanPrimitive(this ref ReadContext context, int count, Span<decimal> destination, decimal min, decimal max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedDecimalSpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedDecimalSpanPrimitive(this ref ReadContext context, int count, Span<decimal> destination, decimal min, decimal max, int bitCount) {
            Span<decimal> destinationSlice = destination.Slice(0, count);
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
