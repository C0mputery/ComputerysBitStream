using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    [BitStreamPrimitive(typeof(long), "VariableLengthLong", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthLongExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthLongPrimitive(this ref WriteContext context, long value) {
            context.WriteVariableLengthULongPrimitive(ZigZagEncodingHelper.EncodeLong(value));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthLongsPrimitive(this ref WriteContext context, ReadOnlySpan<long> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthLongPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PeekVariableLengthLongPrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeLong(context.PeekVariableLengthULongPrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadVariableLengthLongPrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeLong(context.ReadVariableLengthULongPrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthLongPrimitive(this ref ReadContext context, out long value) {
            bool success = context.TryReadVariableLengthULongPrimitive(out ulong encoded);
            value = ZigZagEncodingHelper.DecodeLong(encoded);
            return success;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] PeekVariableLengthLongArrayPrimitive(this ref ReadContext context, int count) {
            long[] result = new long[count];
            context.PeekVariableLengthLongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] ReadVariableLengthLongArrayPrimitive(this ref ReadContext context, int count) {
            long[] result = new long[count];
            context.ReadVariableLengthLongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthLongSpanPrimitive(this ref ReadContext context, int count, Span<long> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthLongSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthLongSpanPrimitive(this ref ReadContext context, int count, Span<long> destination) {
            Span<long> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthLongPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthLongSize(long value) {
            return VariableLengthEncodingHelper.GetUInt64SizeInBits(ZigZagEncodingHelper.EncodeLong(value));
        }
    }
}
