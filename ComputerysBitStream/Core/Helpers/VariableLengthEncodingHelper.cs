using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Helpers {
    [BitStreamPrimitiveContext]
    internal static class VariableLengthEncodingHelper {
        private const byte ContinuationBit = 0x80;
        private const byte PayloadMask = 0x7F;
        private const int PayloadBits = 7;
        private const int ChunkBits = BitHelper.ByteSize;

        private const int MaxUInt32Chunks = 5;
        private const int MaxUInt64Chunks = 10;

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32(ref WriteContext context, uint value) {
            while (value >= ContinuationBit) {
                ulong chunk = (value & PayloadMask) | ContinuationBit;
                context.WriteBitsPrimitive(chunk, ChunkBits);

                value >>= PayloadBits;
            }

            context.WriteBitsPrimitive(value, ChunkBits);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(ref ReadContext context) {
            if (!TryReadUInt32(ref context, out uint value)) {
                context.ThrowIfReadFailed("variable-length uint32");
            }

            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt32(ref ReadContext context, out uint value) {
            value = 0;
            int shift = 0;
            long startPosition = context.Position;

            for (int i = 0; i < MaxUInt32Chunks; i++) {
                if (context.IsInsufficientSpace(ChunkBits)) { break; }

                uint chunk = (uint)context.ReadBitsPrimitive(ChunkBits);
                value |= (chunk & PayloadMask) << shift;
                if ((chunk & ContinuationBit) == 0) { return true; }

                shift += PayloadBits;
            }

            context.Position = startPosition;
            value = 0;
            return false;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64(ref WriteContext context, ulong value) {
            while (value >= ContinuationBit) {
                ulong chunk = (value & PayloadMask) | ContinuationBit;
                context.WriteBitsPrimitive(chunk, ChunkBits);

                value >>= PayloadBits;
            }

            context.WriteBitsPrimitive(value, ChunkBits);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64(ref ReadContext context) {
            if (!TryReadUInt64(ref context, out ulong value)) {
                context.ThrowIfReadFailed("variable-length uint64");
            }

            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt64(ref ReadContext context, out ulong value) {
            value = 0;
            int shift = 0;
            long startPosition = context.Position;

            for (int i = 0; i < MaxUInt64Chunks; i++) {
                if (context.IsInsufficientSpace(ChunkBits)) { break; }

                ulong chunk = context.ReadBitsPrimitive(ChunkBits);
                value |= (chunk & PayloadMask) << shift;
                if ((chunk & ContinuationBit) == 0) { return true; }

                shift += PayloadBits;
            }

            context.Position = startPosition;
            value = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetUInt32SizeInBits(uint encodedValue) {
            int bits = 0;
            while (encodedValue >= ContinuationBit) {
                bits += ChunkBits;
                encodedValue >>= PayloadBits;
            }

            return bits + ChunkBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetUInt64SizeInBits(ulong encodedValue) {
            int bits = 0;
            while (encodedValue >= ContinuationBit) {
                bits += ChunkBits;
                encodedValue >>= PayloadBits;
            }

            return bits + ChunkBits;
        }
    }
}
