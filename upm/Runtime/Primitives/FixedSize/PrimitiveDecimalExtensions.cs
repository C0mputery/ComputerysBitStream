using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.DecimalSize)]
    [BitStreamPrimitive(typeof(decimal), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveDecimalExtensions {
        private const int PartsPerDecimal = 4;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteBits(ref WriteContext context, decimal value) {
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<int> parts = MemoryMarshal.Cast<decimal, int>(MemoryMarshal.CreateReadOnlySpan(ref value, 1));
            context.WriteIntsPrimitive(parts);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ref int parts = ref Unsafe.As<decimal, int>(ref value);
            context.WriteIntsPrimitive(MemoryMarshal.CreateReadOnlySpan(ref parts, PartsPerDecimal));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static decimal ReadBits(ref ReadContext context) {
            decimal value = default;
#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Span<int> parts = MemoryMarshal.Cast<decimal, int>(MemoryMarshal.CreateSpan(ref value, 1));
            context.ReadIntSpanPrimitive(PartsPerDecimal, parts);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ref int parts = ref Unsafe.As<decimal, int>(ref value);
            context.ReadIntSpanPrimitive(PartsPerDecimal, MemoryMarshal.CreateSpan(ref parts, PartsPerDecimal));
#endif
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDecimalPrimitive(this ref WriteContext context, decimal value) {
            WriteBits(ref context, value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDecimalsPrimitive(this ref WriteContext context, ReadOnlySpan<decimal> values) {
            ReadOnlySpan<int> parts = MemoryMarshal.Cast<decimal, int>(values);
            context.WriteIntsPrimitive(parts);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal PeekDecimalPrimitive(this ref ReadContext context) {
            long originalPosition = context.Position;
            decimal value = ReadBits(ref context);
            context.Position = originalPosition;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal ReadDecimalPrimitive(this ref ReadContext context) {
            return ReadBits(ref context);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal[] PeekDecimalArrayPrimitive(this ref ReadContext context, int count) {
            decimal[] result = new decimal[count];
            context.PeekDecimalSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal[] ReadDecimalArrayPrimitive(this ref ReadContext context, int count) {
            decimal[] result = new decimal[count];
            context.ReadDecimalSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekDecimalSpanPrimitive(this ref ReadContext context, int count, Span<decimal> destination) {
            long originalPosition = context.Position;
            context.ReadDecimalSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadDecimalSpanPrimitive(this ref ReadContext context, int count, Span<decimal> destination) {
            Span<decimal> destinationSlice = destination.Slice(0, count);
            context.ReadIntSpanPrimitive(count * PartsPerDecimal, MemoryMarshal.Cast<decimal, int>(destinationSlice));
        }
    }
}
