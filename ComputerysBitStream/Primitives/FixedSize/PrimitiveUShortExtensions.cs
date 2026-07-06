using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.UShortSize)]
    [BitStreamPrimitive(typeof(ushort), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveUShortExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.UShortSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(ushort value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort FromBits(ulong value) => (ushort)value;

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUShortPrimitive(this ref WriteContext context, ushort value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.UShortSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUShortsPrimitive(this ref WriteContext context, ReadOnlySpan<ushort> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<ushort, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = 0;
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= AsBits(values[index + i]) << (i * BitHelper.UShortSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingUShorts = values.Length % NumberOfValuesInUlong;
            if (remainingUShorts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingUShorts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingUShorts + i])) << (i * BitHelper.UShortSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingUShorts * BitHelper.UShortSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort PeekUShortPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.UShortSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUShortPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.UShortSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] PeekUShortArrayPrimitive(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            context.PeekUShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort[] ReadUShortArrayPrimitive(this ref ReadContext context, int count) {
            ushort[] result = new ushort[count];
            context.ReadUShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekUShortSpanPrimitive(this ref ReadContext context, int count, Span<ushort> destination) {
            long originalPosition = context.Position;
            context.ReadUShortSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadUShortSpanPrimitive(this ref ReadContext context, int count, Span<ushort> destination) {
            Span<ushort> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<ushort, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.UShortSize));
                }
            }
#endif
            int remainingUShorts = count % NumberOfValuesInUlong;
            if (remainingUShorts != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingUShorts * BitHelper.UShortSize);
                for (int i = 0; i < remainingUShorts; i++) {
                    destination[count - remainingUShorts + i] = FromBits(lastPacked >> (i * BitHelper.UShortSize));
                }
            }
        }
    }
}
