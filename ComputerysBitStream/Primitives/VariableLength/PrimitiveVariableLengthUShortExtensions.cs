using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    [BitStreamPrimitive(typeof(ushort), "VariableLengthUShort", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthUShortExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthUShortPrimitive(this ref WriteContext context, ushort value) {
            VariableLengthEncodingHelper.WriteUInt32(ref context, value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthUShortsPrimitive(this ref WriteContext context, ReadOnlySpan<ushort> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthUShortPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort PeekVariableLengthUShortPrimitive(this ref ReadContext context) {
            long originalPosition = context.Position;
            ushort value = context.ReadVariableLengthUShortPrimitive();
            context.Position = originalPosition;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadVariableLengthUShortPrimitive(this ref ReadContext context) {
            return (ushort)VariableLengthEncodingHelper.ReadUInt32(ref context);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthUShortPrimitive(this ref ReadContext context, out ushort value) {
            bool success = VariableLengthEncodingHelper.TryReadUInt32(ref context, out uint encoded);
            value = (ushort)encoded;
            return success;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] PeekVariableLengthUShortArrayPrimitive(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            context.PeekVariableLengthUShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] ReadVariableLengthUShortArrayPrimitive(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            context.ReadVariableLengthUShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthUShortSpanPrimitive(this ref ReadContext context, int count, Span<ushort> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthUShortSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthUShortSpanPrimitive(this ref ReadContext context, int count, Span<ushort> destination) {
            Span<ushort> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthUShortPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthUShortSize(ushort value) {
            return VariableLengthEncodingHelper.GetUInt32SizeInBits(value);
        }
    }
}
