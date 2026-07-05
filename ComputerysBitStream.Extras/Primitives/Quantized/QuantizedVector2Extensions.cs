using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;

namespace ComputerysBitStream.Extras.Primitives.Quantized {
    [BitStreamQuantizedPrimitive(2, 64)]
    [BitStreamPrimitive(typeof(Vector2), "QuantizedVector2", PrimitiveSerializationMode.Quantized)]
    public static class QuantizedVector2Extensions {
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector2Primitive(this ref WriteContext context, Vector2 value, Vector2 min, Vector2 max, int bitCount) {
            context.WriteQuantizedFloatPrimitive(value.X, min.X, max.X, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Y, min.Y, max.Y, bitCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector2Primitive(this ref WriteContext context, Vector2 value, float min, float max, int bitCount) {
            context.WriteQuantizedVector2Primitive(value, new Vector2(min), new Vector2(max), bitCount);
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once InconsistentNaming
        public static void WriteQuantizedVector2sPrimitive(this ref WriteContext context, ReadOnlySpan<Vector2> values, Vector2 min, Vector2 max, int bitCount) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteQuantizedVector2Primitive(values[i], min, max, bitCount);
            }
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PeekQuantizedVector2Primitive(this ref ReadContext context, Vector2 min, Vector2 max, int bitCount) {
            return new Vector2(
                context.PeekQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Y, max.Y, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ReadQuantizedVector2Primitive(this ref ReadContext context, Vector2 min, Vector2 max, int bitCount) {
            return new Vector2(
                context.ReadQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Y, max.Y, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2[] PeekQuantizedVector2ArrayPrimitive(this ref ReadContext context, int count, Vector2 min, Vector2 max, int bitCount) {
            Vector2[] result = new Vector2[count];
            context.PeekQuantizedVector2SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2[] ReadQuantizedVector2ArrayPrimitive(this ref ReadContext context, int count, Vector2 min, Vector2 max, int bitCount) {
            Vector2[] result = new Vector2[count];
            context.ReadQuantizedVector2SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedVector2SpanPrimitive(this ref ReadContext context, int count, Span<Vector2> destination, Vector2 min, Vector2 max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedVector2SpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedVector2SpanPrimitive(this ref ReadContext context, int count, Span<Vector2> destination, Vector2 min, Vector2 max, int bitCount) {
            Span<Vector2> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadQuantizedVector2Primitive(min, max, bitCount);
            }
        }
    }
}
