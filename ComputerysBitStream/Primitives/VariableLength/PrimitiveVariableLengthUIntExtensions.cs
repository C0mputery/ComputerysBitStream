using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamPrimitive(typeof(uint), "VariableLengthUInt", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthUIntExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthUIntPrimitive(this ref WriteContext context, uint value) {
            VariableLengthEncodingHelper.WriteUInt32(ref context, value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthUIntsPrimitive(this ref WriteContext context, ReadOnlySpan<uint> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthUIntPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint PeekVariableLengthUIntPrimitive(this ref ReadContext context) {
            long originalPosition = context.Position;
            uint value = context.ReadVariableLengthUIntPrimitive();
            context.Position = originalPosition;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadVariableLengthUIntPrimitive(this ref ReadContext context) {
            return VariableLengthEncodingHelper.ReadUInt32(ref context);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleTryRead"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthUIntPrimitive(this ref ReadContext context, out uint value) {
            return VariableLengthEncodingHelper.TryReadUInt32(ref context, out value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] PeekVariableLengthUIntArrayPrimitive(this ref ReadContext context, int count) {
            uint[] result = new uint[count];
            context.PeekVariableLengthUIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] ReadVariableLengthUIntArrayPrimitive(this ref ReadContext context, int count) {
            uint[] result = new uint[count];
            context.ReadVariableLengthUIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthUIntSpanPrimitive(this ref ReadContext context, int count, Span<uint> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthUIntSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthUIntSpanPrimitive(this ref ReadContext context, int count, Span<uint> destination) {
            Span<uint> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthUIntPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleSize"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthUIntSize(uint value) {
            return VariableLengthEncodingHelper.GetUInt32SizeInBits(value);
        }
    }
}
