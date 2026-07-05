using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.IntSize)]
    [BitStreamPrimitive(typeof(int), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveIntExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.IntSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(int value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (uint)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<int, uint>(ref value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FromBits(ulong value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (int)(uint)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            uint bits = (uint)value;
            return Unsafe.As<uint, int>(ref bits);
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIntPrimitive(this ref WriteContext context, int value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.IntSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIntsPrimitive(this ref WriteContext context, ReadOnlySpan<int> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                int index = ulongIndex * NumberOfValuesInUlong;
                ulong packedUlong = (uint)values[index];
                for (int i = 1; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= (ulong)(uint)values[index + i] << (i * BitHelper.IntSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingInts = values.Length % NumberOfValuesInUlong;
            if (remainingInts != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingInts; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingInts + i])) << (i * BitHelper.IntSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingInts * BitHelper.IntSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PeekIntPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.IntSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadIntPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.IntSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] PeekIntArrayPrimitive(this ref ReadContext context, int count) {
            int[] result = new int[count];
            context.PeekIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] ReadIntArrayPrimitive(this ref ReadContext context, int count) {
            int[] result = new int[count];
            context.ReadIntSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekIntSpanPrimitive(this ref ReadContext context, int count, Span<int> destination) {
            long originalPosition = context.Position;
            context.ReadIntSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadIntSpanPrimitive(this ref ReadContext context, int count, Span<int> destination) {
            Span<int> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<int, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.IntSize));
                }
            }
#endif
            int remainingInts = count % NumberOfValuesInUlong;
            if (remainingInts != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingInts * BitHelper.IntSize);
                for (int i = 0; i < remainingInts; i++) {
                    destination[count - remainingInts + i] = FromBits(lastPacked >> (i * BitHelper.IntSize));
                }
            }
        }
    }
}
