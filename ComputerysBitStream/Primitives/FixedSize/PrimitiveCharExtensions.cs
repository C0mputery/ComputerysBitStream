using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.CharSize)]
    [BitStreamPrimitive(typeof(char), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveCharExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.CharSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(char value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char FromBits(ulong value) => (char)value;

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteCharPrimitive(this ref WriteContext context, char value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.CharSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteCharsPrimitive(this ref WriteContext context, ReadOnlySpan<char> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<char, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = 0;
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= AsBits(values[index + i]) << (i * BitHelper.CharSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingChars = values.Length % NumberOfValuesInUlong;
            if (remainingChars != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingChars; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingChars + i])) << (i * BitHelper.CharSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingChars * BitHelper.CharSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char PeekCharPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.CharSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadCharPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.CharSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] PeekCharArrayPrimitive(this ref ReadContext context, int count) {
            char[] result = new char[count];
            context.PeekCharSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] ReadCharArrayPrimitive(this ref ReadContext context, int count) {
            char[] result = new char[count];
            context.ReadCharSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekCharSpanPrimitive(this ref ReadContext context, int count, Span<char> destination) {
            long originalPosition = context.Position;
            context.ReadCharSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadCharSpanPrimitive(this ref ReadContext context, int count, Span<char> destination) {
            Span<char> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<char, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.CharSize));
                }
            }
#endif
            int remainingChars = count % NumberOfValuesInUlong;
            if (remainingChars != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingChars * BitHelper.CharSize);
                for (int i = 0; i < remainingChars; i++) {
                    destination[count - remainingChars + i] = FromBits(lastPacked >> (i * BitHelper.CharSize));
                }
            }
        }
    }
}
