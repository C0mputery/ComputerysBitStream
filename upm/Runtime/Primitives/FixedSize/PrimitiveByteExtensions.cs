using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.ByteSize)]
    [BitStreamPrimitive(typeof(byte), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveByteExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.ByteSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(byte value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte FromBits(ulong value) => (byte)value;

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBytePrimitive(this ref WriteContext context, byte value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.ByteSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBytesPrimitive(this ref WriteContext context, ReadOnlySpan<byte> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<byte, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = 0;
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= AsBits(values[index + i]) << (i * BitHelper.ByteSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingBytes = values.Length % NumberOfValuesInUlong;
            if (remainingBytes != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingBytes; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingBytes + i])) << (i * BitHelper.ByteSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingBytes * BitHelper.ByteSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte PeekBytePrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.ByteSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadBytePrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.ByteSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] PeekByteArrayPrimitive(this ref ReadContext context, int count) {
            byte[] result = new byte[count];
            context.PeekByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadByteArrayPrimitive(this ref ReadContext context, int count) {
            byte[] result = new byte[count];
            context.ReadByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekByteSpanPrimitive(this ref ReadContext context, int count, Span<byte> destination) {
            long originalPosition = context.Position;
            context.ReadByteSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadByteSpanPrimitive(this ref ReadContext context, int count, Span<byte> destination) {
            Span<byte> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<byte, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.ByteSize));
                }
            }
#endif
            int remainingBytes = count % NumberOfValuesInUlong;
            if (remainingBytes != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingBytes * BitHelper.ByteSize);
                for (int i = 0; i < remainingBytes; i++) {
                    destination[count - remainingBytes + i] = FromBits(lastPacked >> (i * BitHelper.ByteSize));
                }
            }
        }
    }
}
