using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamPrimitive(typeof(ulong), "VariableLengthULong", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthULongExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthULongPrimitive(this ref WriteContext context, ulong value) {
            VariableLengthEncodingHelper.WriteUInt64(ref context, value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthULongsPrimitive(this ref WriteContext context, ReadOnlySpan<ulong> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthULongPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong PeekVariableLengthULongPrimitive(this ref ReadContext context) {
            long originalPosition = context.Position;
            ulong value = context.ReadVariableLengthULongPrimitive();
            context.Position = originalPosition;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadVariableLengthULongPrimitive(this ref ReadContext context) {
            return VariableLengthEncodingHelper.ReadUInt64(ref context);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleTryRead"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthULongPrimitive(this ref ReadContext context, out ulong value) {
            return VariableLengthEncodingHelper.TryReadUInt64(ref context, out value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong[] PeekVariableLengthULongArrayPrimitive(this ref ReadContext context, int count) {
            ulong[] result = new ulong[count];
            context.PeekVariableLengthULongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong[] ReadVariableLengthULongArrayPrimitive(this ref ReadContext context, int count) {
            ulong[] result = new ulong[count];
            context.ReadVariableLengthULongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthULongSpanPrimitive(this ref ReadContext context, int count, Span<ulong> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthULongSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthULongSpanPrimitive(this ref ReadContext context, int count, Span<ulong> destination) {
            Span<ulong> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthULongPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleSize"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthULongSize(ulong value) {
            return VariableLengthEncodingHelper.GetUInt64SizeInBits(value);
        }
    }
}
