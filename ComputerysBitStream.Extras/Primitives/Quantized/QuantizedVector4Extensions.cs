using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;

namespace ComputerysBitStream.Extras.Primitives.Quantized {
    [BitStreamQuantizedPrimitive(4, 128)]
    [BitStreamPrimitive(typeof(Vector4), "QuantizedVector4", PrimitiveSerializationMode.Quantized)]
    public static class QuantizedVector4Extensions {
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector4Primitive(this ref WriteContext context, Vector4 value, Vector4 min, Vector4 max, int bitCount) {
            context.WriteQuantizedFloatPrimitive(value.X, min.X, max.X, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Y, min.Y, max.Y, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Z, min.Z, max.Z, bitCount);
            context.WriteQuantizedFloatPrimitive(value.W, min.W, max.W, bitCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector4Primitive(this ref WriteContext context, Vector4 value, float min, float max, int bitCount) {
            context.WriteQuantizedVector4Primitive(value, new Vector4(min), new Vector4(max), bitCount);
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once InconsistentNaming
        public static void WriteQuantizedVector4sPrimitive(this ref WriteContext context, ReadOnlySpan<Vector4> values, Vector4 min, Vector4 max, int bitCount) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteQuantizedVector4Primitive(values[i], min, max, bitCount);
            }
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 PeekQuantizedVector4Primitive(this ref ReadContext context, Vector4 min, Vector4 max, int bitCount) {
            return new Vector4(
                context.PeekQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Y, max.Y, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Z, max.Z, bitCount),
                context.PeekQuantizedFloatPrimitive(min.W, max.W, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ReadQuantizedVector4Primitive(this ref ReadContext context, Vector4 min, Vector4 max, int bitCount) {
            return new Vector4(
                context.ReadQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Y, max.Y, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Z, max.Z, bitCount),
                context.ReadQuantizedFloatPrimitive(min.W, max.W, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4[] PeekQuantizedVector4ArrayPrimitive(this ref ReadContext context, int count, Vector4 min, Vector4 max, int bitCount) {
            Vector4[] result = new Vector4[count];
            context.PeekQuantizedVector4SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4[] ReadQuantizedVector4ArrayPrimitive(this ref ReadContext context, int count, Vector4 min, Vector4 max, int bitCount) {
            Vector4[] result = new Vector4[count];
            context.ReadQuantizedVector4SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedVector4SpanPrimitive(this ref ReadContext context, int count, Span<Vector4> destination, Vector4 min, Vector4 max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedVector4SpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedVector4SpanPrimitive(this ref ReadContext context, int count, Span<Vector4> destination, Vector4 min, Vector4 max, int bitCount) {
            Span<Vector4> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadQuantizedVector4Primitive(min, max, bitCount);
            }
        }
    }
}
