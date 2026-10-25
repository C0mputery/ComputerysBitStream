using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.VariableLength {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamPrimitive(typeof(byte), "VariableLengthByte", PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveVariableLengthByteExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthBytePrimitive(this ref WriteContext context, byte value) {
            VariableLengthEncodingHelper.WriteUInt32(ref context, value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVariableLengthBytesPrimitive(this ref WriteContext context, ReadOnlySpan<byte> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteVariableLengthBytePrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte PeekVariableLengthBytePrimitive(this ref ReadContext context) {
            long originalPosition = context.Position;
            byte value = context.ReadVariableLengthBytePrimitive();
            context.Position = originalPosition;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadVariableLengthBytePrimitive(this ref ReadContext context) {
            return (byte)VariableLengthEncodingHelper.ReadUInt32(ref context);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleTryRead"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadVariableLengthBytePrimitive(this ref ReadContext context, out byte value) {
            bool success = VariableLengthEncodingHelper.TryReadUInt32(ref context, out uint encoded);
            value = (byte)encoded;
            return success;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] PeekVariableLengthByteArrayPrimitive(this ref ReadContext context, int count) {
            byte[] result = new byte[count];
            context.PeekVariableLengthByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadVariableLengthByteArrayPrimitive(this ref ReadContext context, int count) {
            byte[] result = new byte[count];
            context.ReadVariableLengthByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekVariableLengthByteSpanPrimitive(this ref ReadContext context, int count, Span<byte> destination) {
            long originalPosition = context.Position;
            context.ReadVariableLengthByteSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadVariableLengthByteSpanPrimitive(this ref ReadContext context, int count, Span<byte> destination) {
            Span<byte> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadVariableLengthBytePrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleSize"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetVariableLengthByteSize(byte value) {
            return VariableLengthEncodingHelper.GetUInt32SizeInBits(value);
        }
    }
}
