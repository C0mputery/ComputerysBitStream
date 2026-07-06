using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.DoubleSize)]
    [BitStreamPrimitive(typeof(double), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveDoubleExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(double value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (ulong)BitConverter.DoubleToInt64Bits(value);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<double, ulong>(ref value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double FromBits(ulong value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return BitConverter.Int64BitsToDouble((long)value);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<ulong, double>(ref value);
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDoublePrimitive(this ref WriteContext context, double value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.DoubleSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDoublesPrimitive(this ref WriteContext context, ReadOnlySpan<double> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<double, ulong>(values);
            context.WriteBitsPrimitive(ulongs, ulongs.Length * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            for (int valueIndex = 0; valueIndex < values.Length; valueIndex++) {
                context.WriteBitsPrimitive(AsBits(values[valueIndex]), BitHelper.ULongSize);
            }
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PeekDoublePrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.DoubleSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDoublePrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.DoubleSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] PeekDoubleArrayPrimitive(this ref ReadContext context, int count) {
            double[] result = new double[count];
            context.PeekDoubleSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] ReadDoubleArrayPrimitive(this ref ReadContext context, int count) {
            double[] result = new double[count];
            context.ReadDoubleSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekDoubleSpanPrimitive(this ref ReadContext context, int count, Span<double> destination) {
            long originalPosition = context.Position;
            context.ReadDoubleSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadDoubleSpanPrimitive(this ref ReadContext context, int count, Span<double> destination) {
            Span<double> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<double, ulong>(destinationSlice);
            context.ReadBitsPrimitive(ulongs.Length * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            for (int valueIndex = 0; valueIndex < count; valueIndex++) {
                destinationSlice[valueIndex] = FromBits(context.ReadBitsPrimitive(BitHelper.ULongSize));
            }
#endif
        }
    }
}
