using System;
using System.Runtime.CompilerServices;
using System.Text;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;
using ComputerysBitStream.Primitives.FixedSize;

namespace ComputerysBitStream.Primitives.VariableLength {
    // we use var length bcs for uints you'd need to write 2,097,152 chars to even equal the number of bytes in the fixed length one
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamPrimitive(typeof(string), PrimitiveSerializationMode.VariableLength)]
    public static class PrimitiveStringExtensions {
        private static readonly UTF8Encoding Utf8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringPrimitive(this ref WriteContext context, string? value) {
            if (string.IsNullOrEmpty(value)) {
                context.WriteVariableLengthUIntPrimitive(0);
                return;
            }

            int byteCount = Utf8.GetByteCount(value);
            context.WriteVariableLengthUIntPrimitive((uint)byteCount);
            WriteUtf8Bytes(ref context, value, byteCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringsPrimitive(this ref WriteContext context, ReadOnlySpan<string?> values) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteStringPrimitive(values[i]);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string PeekStringPrimitive(this ref ReadContext context) {
            long originalPosition = context.Position;
            string value = context.ReadStringPrimitive();
            context.Position = originalPosition;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadStringPrimitive(this ref ReadContext context) {
            uint byteCount = context.ReadVariableLengthUIntPrimitive();
            if (byteCount == 0) { return string.Empty; }

            return ReadUtf8String(ref context, (int)byteCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleTryRead"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.TryRead)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadStringPrimitive(this ref ReadContext context, out string value) {
            long startPosition = context.Position;
            if (!context.TryReadVariableLengthUIntPrimitive(out uint byteCount)) {
                value = string.Empty;
                return false;
            }

            int payloadBits = (int)byteCount * BitHelper.ByteSize;
            if (byteCount > 0 && context.IsInsufficientSpace(payloadBits)) {
                context.Position = startPosition;
                value = string.Empty;
                return false;
            }

            if (byteCount == 0) {
                value = string.Empty;
                return true;
            }

            try {
                value = ReadUtf8String(ref context, (int)byteCount);
                return true;
            }
            catch (DecoderFallbackException) {
                context.Position = startPosition;
                value = string.Empty;
                return false;
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] PeekStringArrayPrimitive(this ref ReadContext context, int count) {
            string[] result = new string[count];
            context.PeekStringSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] ReadStringArrayPrimitive(this ref ReadContext context, int count) {
            string[] result = new string[count];
            context.ReadStringSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekStringSpanPrimitive(this ref ReadContext context, int count, Span<string> destination) {
            long originalPosition = context.Position;
            context.ReadStringSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadStringSpanPrimitive(this ref ReadContext context, int count, Span<string> destination) {
            Span<string> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadStringPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.RoleSize"/>
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Size)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetStringSize(string? value) {
            int byteCount = string.IsNullOrEmpty(value) ? 0 : Utf8.GetByteCount(value);
            return VariableLengthEncodingHelper.GetUInt32SizeInBits((uint)byteCount) + byteCount * BitHelper.ByteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteUtf8Bytes(ref WriteContext context, string value, int byteCount) {
            byte[] buffer = new byte[byteCount];
            Utf8.GetBytes(value, buffer);
            context.WriteBytesPrimitive(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ReadUtf8String(ref ReadContext context, int byteCount) {
            byte[] buffer = new byte[byteCount];
            context.ReadByteSpanPrimitive(byteCount, buffer);
            return Utf8.GetString(buffer);
        }
    }
}
