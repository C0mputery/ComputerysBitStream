using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    [BitStreamPrimitive(typeof(short), "VariableLengthShort", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthShortExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthShortPrimitive(this ref WriteContext context, short value) {
            context.WriteVariableLengthUShortPrimitive(ZigZagEncodingHelper.EncodeShort(value));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthShortsPrimitive(this ref WriteContext context, ReadOnlySpan<short> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthShortPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short PeekVariableLengthShortPrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeShort(context.PeekVariableLengthUShortPrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadVariableLengthShortPrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeShort(context.ReadVariableLengthUShortPrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthShortPrimitive(this ref ReadContext context, out short value) {
            bool success = context.TryReadVariableLengthUShortPrimitive(out ushort encoded);
            value = ZigZagEncodingHelper.DecodeShort(encoded);
            return success;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short[] PeekVariableLengthShortArrayPrimitive(this ref ReadContext context, int count) {
            short[] result = new short[count];
            context.PeekVariableLengthShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short[] ReadVariableLengthShortArrayPrimitive(this ref ReadContext context, int count) {
            short[] result = new short[count];
            context.ReadVariableLengthShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthShortSpanPrimitive(this ref ReadContext context, int count, Span<short> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthShortSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthShortSpanPrimitive(this ref ReadContext context, int count, Span<short> destination) {
            Span<short> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthShortPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthShortSize(short value) {
            return VariableLengthEncodingHelper.GetUInt32SizeInBits(ZigZagEncodingHelper.EncodeShort(value));
        }
    }
}
