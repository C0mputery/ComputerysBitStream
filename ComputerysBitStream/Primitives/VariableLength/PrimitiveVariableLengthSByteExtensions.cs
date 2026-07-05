using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    [BitStreamPrimitive(typeof(sbyte), "VariableLengthSByte", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthSByteExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthSBytePrimitive(this ref WriteContext context, sbyte value) {
            context.WriteVariableLengthBytePrimitive(ZigZagEncodingHelper.EncodeSByte(value));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthSBytesPrimitive(this ref WriteContext context, ReadOnlySpan<sbyte> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthSBytePrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte PeekVariableLengthSBytePrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeSByte(context.PeekVariableLengthBytePrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadVariableLengthSBytePrimitive(this ref ReadContext context) {
            return ZigZagEncodingHelper.DecodeSByte(context.ReadVariableLengthBytePrimitive());
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthSBytePrimitive(this ref ReadContext context, out sbyte value) {
            bool success = context.TryReadVariableLengthBytePrimitive(out byte encoded);
            value = ZigZagEncodingHelper.DecodeSByte(encoded);
            return success;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte[] PeekVariableLengthSByteArrayPrimitive(this ref ReadContext context, int count) {
            sbyte[] result = new sbyte[count];
            context.PeekVariableLengthSByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte[] ReadVariableLengthSByteArrayPrimitive(this ref ReadContext context, int count) {
            sbyte[] result = new sbyte[count];
            context.ReadVariableLengthSByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthSByteSpanPrimitive(this ref ReadContext context, int count, Span<sbyte> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthSByteSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthSByteSpanPrimitive(this ref ReadContext context, int count, Span<sbyte> destination) {
            Span<sbyte> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthSBytePrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthSByteSize(sbyte value) {
            return VariableLengthEncodingHelper.GetUInt32SizeInBits(ZigZagEncodingHelper.EncodeSByte(value));
        }
    }
}
