using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    [BitStreamPrimitive(typeof(int), "VariableLengthInt", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthIntExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthIntPrimitive(this ref WriteContext context, int value) {
            context.WriteVariableLengthUIntPrimitive(ZigZagEncodingHelper.EncodeInt(value));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthIntsPrimitive(this ref WriteContext context, ReadOnlySpan<int> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthIntPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PeekVariableLengthIntPrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeInt(context.PeekVariableLengthUIntPrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadVariableLengthIntPrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeInt(context.ReadVariableLengthUIntPrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthIntPrimitive(this ref ReadContext context, out int value) {
            bool success = context.TryReadVariableLengthUIntPrimitive(out uint encoded);
            value = ZigZagEncodingHelper.DecodeInt(encoded);
            return success;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] PeekVariableLengthIntArrayPrimitive(this ref ReadContext context, int count) {
            int[] result = new int[count];
            context.PeekVariableLengthIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] ReadVariableLengthIntArrayPrimitive(this ref ReadContext context, int count) {
            int[] result = new int[count];
            context.ReadVariableLengthIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthIntSpanPrimitive(this ref ReadContext context, int count, Span<int> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthIntSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthIntSpanPrimitive(this ref ReadContext context, int count, Span<int> destination) {
            Span<int> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthIntPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthIntSize(int value) {
            return VariableLengthEncodingHelper.GetUInt32SizeInBits(ZigZagEncodingHelper.EncodeInt(value));
        }
    }
}
