using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream.Primitives.FixedSize {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamFixedSizePrimitive(BitHelper.BoolSize)]
    [BitStreamPrimitive(typeof(bool), PrimitiveSerializationMode.FixedSize)]
    public static class PrimitiveBoolExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.BoolSize;

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBoolPrimitive(this ref WriteContext context, bool value) {
            context.WriteBitPrimitive(value);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBoolsPrimitive(this ref WriteContext context, ReadOnlySpan<bool> values) {
            int count = values.Length;
            int processed = 0;

            while (processed + NumberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packed |= (values[processed + i] ? 1UL : 0UL) << i;
                }

                context.WriteBitsPrimitive(packed, BitHelper.ULongSize);

                processed += NumberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (values[processed + i] ? 1UL : 0UL) << i;
                }

                context.WriteBitsPrimitive(packed, remaining * BitHelper.BoolSize);
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PeekBoolPrimitive(this ref ReadContext context) {
            return context.PeekBitPrimitive();
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolPrimitive(this ref ReadContext context) {
            return context.ReadBitPrimitive();
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] PeekBoolArrayPrimitive(this ref ReadContext context, int count) {
            bool[] result = new bool[count];
            context.PeekBoolSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] ReadBoolArrayPrimitive(this ref ReadContext context, int count) {
            bool[] result = new bool[count];
            context.ReadBoolSpanPrimitive(count, result);
            return result;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekBoolSpanPrimitive(this ref ReadContext context, int count, Span<bool> destination) {
            long originalPosition = context.Position;
            context.ReadBoolSpanPrimitive(count, destination);
            context.Position = originalPosition;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadBoolSpanPrimitive(this ref ReadContext context, int count, Span<bool> destination) {
            int processed = 0;

            while (processed + NumberOfValuesInUlong <= count) {
                ulong packed = context.ReadBitsPrimitive(BitHelper.ULongSize);
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destination[processed + i] = (packed & (1UL << i)) != 0UL;
                }

                processed += NumberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = context.ReadBitsPrimitive(remaining * BitHelper.BoolSize);
                for (int i = 0; i < remaining; i++) {
                    destination[processed + i] = (packed & (1UL << i)) != 0UL;
                }
            }
        }
    }
}
