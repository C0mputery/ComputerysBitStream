using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.LongSize)]
    [BitStreamPrimitive(typeof(long), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveLongExtensions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(long value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (ulong)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<long, ulong>(ref value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long FromBits(ulong value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (long)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<ulong, long>(ref value);
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLongPrimitive(this ref WriteContext context, long value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.LongSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLongsPrimitive(this ref WriteContext context, ReadOnlySpan<long> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(values);
            context.WriteBitsPrimitive(ulongs, ulongs.Length * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            for (int valueIndex = 0; valueIndex < values.Length; valueIndex++) {
                context.WriteBitsPrimitive(AsBits(values[valueIndex]), BitHelper.ULongSize);
            }
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PeekLongPrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.LongSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLongPrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.LongSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] PeekLongArrayPrimitive(this ref ReadContext context, int count) {
            long[] result = new long[count];
            context.PeekLongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] ReadLongArrayPrimitive(this ref ReadContext context, int count) {
            long[] result = new long[count];
            context.ReadLongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekLongSpanPrimitive(this ref ReadContext context, int count, Span<long> destination) {
            long originalPosition = context.Position;
            context.ReadLongSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadLongSpanPrimitive(this ref ReadContext context, int count, Span<long> destination) {
            Span<long> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<long, ulong>(destinationSlice);
            context.ReadBitsPrimitive(ulongs.Length * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            for (int valueIndex = 0; valueIndex < count; valueIndex++) {
                destinationSlice[valueIndex] = FromBits(context.ReadBitsPrimitive(BitHelper.ULongSize));
            }
#endif
        }
    }
}
