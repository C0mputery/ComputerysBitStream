using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.FloatSize)]
    [BitStreamPrimitive(typeof(float), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveFloatExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.FloatSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(float value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (uint)BitConverter.SingleToInt32Bits(value);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<float, uint>(ref value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float FromBits(ulong value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return BitConverter.Int32BitsToSingle((int)(uint)value);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            uint bits = (uint)value;
            return Unsafe.As<uint, float>(ref bits);
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloatPrimitive(this ref WriteContext context, float value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.FloatSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFloatsPrimitive(this ref WriteContext context, ReadOnlySpan<float> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<float, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                int index = ulongIndex * NumberOfValuesInUlong;
                ulong packedUlong = AsBits(values[index]);
                for (int i = 1; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= AsBits(values[index + i]) << (i * BitHelper.FloatSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingFloats = values.Length % NumberOfValuesInUlong;
            if (remainingFloats != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingFloats; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingFloats + i])) << (i * BitHelper.FloatSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingFloats * BitHelper.FloatSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PeekFloatPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.FloatSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloatPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.FloatSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] PeekFloatArrayPrimitive(this ref ReadContext context, int count) {
            float[] result = new float[count];
            context.PeekFloatSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ReadFloatArrayPrimitive(this ref ReadContext context, int count) {
            float[] result = new float[count];
            context.ReadFloatSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekFloatSpanPrimitive(this ref ReadContext context, int count, Span<float> destination) {
            long originalPosition = context.Position;
            context.ReadFloatSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadFloatSpanPrimitive(this ref ReadContext context, int count, Span<float> destination) {
            Span<float> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<float, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.FloatSize));
                }
            }
#endif
            int remainingFloats = count % NumberOfValuesInUlong;
            if (remainingFloats != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingFloats * BitHelper.FloatSize);
                for (int i = 0; i < remainingFloats; i++) {
                    destination[count - remainingFloats + i] = FromBits(lastPacked >> (i * BitHelper.FloatSize));
                }
            }
        }
    }
}
