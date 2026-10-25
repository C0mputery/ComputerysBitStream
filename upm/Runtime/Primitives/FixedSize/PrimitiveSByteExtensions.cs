using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_HOST_BIG_ENDIAN
using System.Runtime.InteropServices;
#endif
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.SByteSize)]
    [BitStreamPrimitive(typeof(sbyte), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveSByteExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.SByteSize;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(sbyte value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (byte)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.As<sbyte, byte>(ref value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static sbyte FromBits(ulong value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return (sbyte)(byte)value;
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            byte bits = (byte)value;
            return Unsafe.As<byte, sbyte>(ref bits);
#endif
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSBytePrimitive(this ref WriteContext context, sbyte value) {
            context.WriteBitsPrimitive(AsBits(value), BitHelper.SByteSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSBytesPrimitive(this ref WriteContext context, ReadOnlySpan<sbyte> values) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<sbyte, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsPrimitive(ulongs, totalUlongs * BitHelper.ULongSize);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = values.Length / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = 0;
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packedUlong |= AsBits(values[index + i]) << (i * BitHelper.SByteSize);
                }

                context.WriteBitsPrimitive(packedUlong, BitHelper.ULongSize);
            }
#endif
            int remainingSBytes = values.Length % NumberOfValuesInUlong;
            if (remainingSBytes != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingSBytes; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingSBytes + i])) << (i * BitHelper.SByteSize);
                }

                context.WriteBitsPrimitive(lastPacked, remainingSBytes * BitHelper.SByteSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte PeekSBytePrimitive(this ref ReadContext context) {
            return FromBits(context.PeekBitsPrimitive(BitHelper.SByteSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSBytePrimitive(this ref ReadContext context) {
            return FromBits(context.ReadBitsPrimitive(BitHelper.SByteSize));
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte[] PeekSByteArrayPrimitive(this ref ReadContext context, int count) {
            sbyte[] result = new sbyte[count];
            context.PeekSByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte[] ReadSByteArrayPrimitive(this ref ReadContext context, int count) {
            sbyte[] result = new sbyte[count];
            context.ReadSByteSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekSByteSpanPrimitive(this ref ReadContext context, int count, Span<sbyte> destination) {
            long originalPosition = context.Position;
            context.ReadSByteSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadSByteSpanPrimitive(this ref ReadContext context, int count, Span<sbyte> destination) {
            Span<sbyte> destinationSlice = destination.Slice(0, count);
#if !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> ulongs = MemoryMarshal.Cast<sbyte, ulong>(destinationSlice);
            int totalUlongs = ulongs.Length;
            context.ReadBitsPrimitive(totalUlongs * BitHelper.ULongSize, ulongs);
#elif BITSTREAM_HOST_BIG_ENDIAN
            int totalUlongs = count / NumberOfValuesInUlong;
            for (int ulongIndex = 0; ulongIndex < totalUlongs; ulongIndex++) {
                ulong packedUlong = context.ReadBitsPrimitive(BitHelper.ULongSize);
                int index = ulongIndex * NumberOfValuesInUlong;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destinationSlice[index + i] = FromBits(packedUlong >> (i * BitHelper.SByteSize));
                }
            }
#endif
            int remainingSBytes = count % NumberOfValuesInUlong;
            if (remainingSBytes != 0) {
                ulong lastPacked = context.ReadBitsPrimitive(remainingSBytes * BitHelper.SByteSize);
                for (int i = 0; i < remainingSBytes; i++) {
                    destination[count - remainingSBytes + i] = FromBits(lastPacked >> (i * BitHelper.SByteSize));
                }
            }
        }
    }
}
