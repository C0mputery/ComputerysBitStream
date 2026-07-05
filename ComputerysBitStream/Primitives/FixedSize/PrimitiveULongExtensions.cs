using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    [BitStreamFixedSizePrimitive(BitHelper.ULongSize)]
    [BitStreamPrimitive(typeof(ulong), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveULongExtensions {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteULongPrimitive(this ref WriteContext context, ulong value) {
            context.WriteBitsPrimitive(value, BitHelper.ULongSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteULongsPrimitive(this ref WriteContext context, ReadOnlySpan<ulong> values) {
            context.WriteBitsPrimitive(values, values.Length * BitHelper.ULongSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong PeekULongPrimitive(this ref ReadContext context) {
            return context.PeekBitsPrimitive(BitHelper.ULongSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadULongPrimitive(this ref ReadContext context) {
            return context.ReadBitsPrimitive(BitHelper.ULongSize);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong[] PeekULongArrayPrimitive(this ref ReadContext context, int count) {
            ulong[] result = new ulong[count];
            context.PeekULongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong[] ReadULongArrayPrimitive(this ref ReadContext context, int count) {
            ulong[] result = new ulong[count];
            context.ReadULongSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekULongSpanPrimitive(this ref ReadContext context, int count, Span<ulong> destination) {
            long originalPosition = context.Position;
            context.ReadULongSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadULongSpanPrimitive(this ref ReadContext context, int count, Span<ulong> destination) {
            Span<ulong> destinationSlice = destination.Slice(0, count);
            context.ReadBitsPrimitive(count * BitHelper.ULongSize, destinationSlice);
        }
    }
}
