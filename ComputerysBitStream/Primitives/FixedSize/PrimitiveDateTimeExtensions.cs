using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.DateTimeSize)]
    [BitStreamPrimitive(typeof(DateTime), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveDateTimeExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(DateTime value) => (ulong)value.ToBinary();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DateTime FromBits(ulong value) => DateTime.FromBinary((long)value);

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDateTimePrimitive(this ref WriteContext context, DateTime value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.DateTimeSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDateTimesPrimitive(this ref WriteContext context, ReadOnlySpan<DateTime> values) {
            foreach (DateTime dateTime in values) {
                context.WriteBitsPrimitive(AsBits(dateTime), BitHelper.ULongSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime PeekDateTimePrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.DateTimeSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ReadDateTimePrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.DateTimeSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime[] PeekDateTimeArrayPrimitive(this ref ReadContext context, int count) {
            DateTime[] result = new DateTime[count];
            context.PeekDateTimeSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime[] ReadDateTimeArrayPrimitive(this ref ReadContext context, int count) {
            DateTime[] result = new DateTime[count];
            context.ReadDateTimeSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekDateTimeSpanPrimitive(this ref ReadContext context, int count, Span<DateTime> destination) {
            long originalPosition = context.Position;
            context.ReadDateTimeSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadDateTimeSpanPrimitive(this ref ReadContext context, int count, Span<DateTime> destination) {
            Span<DateTime> destinationSlice = destination.Slice(0, count);
            for (int valueIndex = 0; valueIndex < count; valueIndex++) {
                destinationSlice[valueIndex] = FromBits(context.ReadBitsPrimitive(BitHelper.ULongSize));
            }
        }
    }
}
