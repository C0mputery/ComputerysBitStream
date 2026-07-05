using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.ShortSize)]
    [BitStreamPrimitive(typeof(short), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveShortExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.ShortSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(short value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (ushort)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<short, ushort>(ref value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static short FromBits(ulong value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (short)(ushort)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ushort bits = (ushort)value;
            return Unsafe.As<ushort, short>(ref bits);
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteShortPrimitive(this ref WriteContext context, short value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.ShortSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteShortsPrimitive(this ref WriteContext context, ReadOnlySpan<short> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<short, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = 0;
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= AsBits(values[index + i]) << (i * BitHelper.ShortSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingShorts = values.Length % NumberOfValuesInUlong;
            if (remainingShorts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingShorts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingShorts + i])) << (i * BitHelper.ShortSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingShorts * BitHelper.ShortSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short PeekShortPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.ShortSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadShortPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.ShortSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short[] PeekShortArrayPrimitive(this ref ReadContext context, int count) {
            short[] result = new short[count];
            context.PeekShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short[] ReadShortArrayPrimitive(this ref ReadContext context, int count) {
            short[] result = new short[count];
            context.ReadShortSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekShortSpanPrimitive(this ref ReadContext context, int count, Span<short> destination) {
            long originalPosition = context.Position;
            context.ReadShortSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadShortSpanPrimitive(this ref ReadContext context, int count, Span<short> destination) {
            Span<short> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<short, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.ShortSize));
                }
            }
#endif
            int remainingShorts = count % NumberOfValuesInUlong;
            if (remainingShorts != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingShorts * BitHelper.ShortSize);
                for (int i = 0; i < remainingShorts; i++) {
                    destination[count - remainingShorts + i] = FromBits(lastPacked >> (i * BitHelper.ShortSize));
                }
            }
        }
    }
}
