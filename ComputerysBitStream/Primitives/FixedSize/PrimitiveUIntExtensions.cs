using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.UIntSize)]
    [BitStreamPrimitive(typeof(uint), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveUIntExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.UIntSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(uint value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FromBits(ulong value) => (uint)value;

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUIntPrimitive(this ref WriteContext context, uint value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.UIntSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUIntsPrimitive(this ref WriteContext context, ReadOnlySpan<uint> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<uint, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                int index = ulongIndex * NumberOfValuesInUlong;
                ulong packedUlong = values[index];
                for (int i = 1; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= (ulong)values[index + i] << (i * BitHelper.UIntSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingUInts = values.Length % NumberOfValuesInUlong;
            if (remainingUInts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingUInts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingUInts + i])) << (i * BitHelper.UIntSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingUInts * BitHelper.UIntSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint PeekUIntPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.UIntSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUIntPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.UIntSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] PeekUIntArrayPrimitive(this ref ReadContext context, int count) {
            uint[] result = new uint[count];
            context.PeekUIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] ReadUIntArrayPrimitive(this ref ReadContext context, int count) {
            uint[] result = new uint[count];
            context.ReadUIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekUIntSpanPrimitive(this ref ReadContext context, int count, Span<uint> destination) {
            long originalPosition = context.Position;
            context.ReadUIntSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadUIntSpanPrimitive(this ref ReadContext context, int count, Span<uint> destination) {
            Span<uint> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<uint, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.UIntSize));
                }
            }
#endif
            int remainingUInts = count % NumberOfValuesInUlong;
            if (remainingUInts != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingUInts * BitHelper.UIntSize);
                for (int i = 0; i < remainingUInts; i++) {
                    destination[count - remainingUInts + i] = FromBits(lastPacked >> (i * BitHelper.UIntSize));
                }
            }
        }
    }
}
