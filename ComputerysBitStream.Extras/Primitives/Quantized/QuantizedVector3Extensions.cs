using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;

namespace ComputerysBitStream.Extras.Primitives.Quantized {
    [BitStreamQuantizedPrimitive(3, 96)]
    [BitStreamPrimitive(typeof(Vector3), "QuantizedVector3", PrimitiveSerializationMode.Quantized)]
    public static class QuantizedVector3Extensions {
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector3Primitive(this ref WriteContext context, Vector3 value, Vector3 min, Vector3 max, int bitCount) {
            context.WriteQuantizedFloatPrimitive(value.X, min.X, max.X, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Y, min.Y, max.Y, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Z, min.Z, max.Z, bitCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector3Primitive(this ref WriteContext context, Vector3 value, float min, float max, int bitCount) {
            context.WriteQuantizedVector3Primitive(value, new Vector3(min), new Vector3(max), bitCount);
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedVector3sPrimitive(this ref WriteContext context, ReadOnlySpan<Vector3> values, Vector3 min, Vector3 max, int bitCount) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteQuantizedVector3Primitive(values[i], min, max, bitCount);
            }
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PeekQuantizedVector3Primitive(this ref ReadContext context, Vector3 min, Vector3 max, int bitCount) {
            return new Vector3(
                context.PeekQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Y, max.Y, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Z, max.Z, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReadQuantizedVector3Primitive(this ref ReadContext context, Vector3 min, Vector3 max, int bitCount) {
            return new Vector3(
                context.ReadQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Y, max.Y, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Z, max.Z, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3[] PeekQuantizedVector3ArrayPrimitive(this ref ReadContext context, int count, Vector3 min, Vector3 max, int bitCount) {
            Vector3[] result = new Vector3[count];
            context.PeekQuantizedVector3SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3[] ReadQuantizedVector3ArrayPrimitive(this ref ReadContext context, int count, Vector3 min, Vector3 max, int bitCount) {
            Vector3[] result = new Vector3[count];
            context.ReadQuantizedVector3SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedVector3SpanPrimitive(this ref ReadContext context, int count, Span<Vector3> destination, Vector3 min, Vector3 max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedVector3SpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedVector3SpanPrimitive(this ref ReadContext context, int count, Span<Vector3> destination, Vector3 min, Vector3 max, int bitCount) {
            Span<Vector3> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadQuantizedVector3Primitive(min, max, bitCount);
            }
        }
    }
}
