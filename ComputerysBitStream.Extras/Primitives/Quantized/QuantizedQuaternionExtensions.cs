using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;

namespace ComputerysBitStream.Extras.Primitives.Quantized {
    [BitStreamQuantizedPrimitive(4, 128)]
    [BitStreamPrimitive(typeof(Quaternion), "QuantizedQuaternion", PrimitiveSerializationMode.Quantized)]
    public static class QuantizedQuaternionExtensions {
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedQuaternionPrimitive(this ref WriteContext context, Quaternion value, Quaternion min, Quaternion max, int bitCount) {
            context.WriteQuantizedFloatPrimitive(value.X, min.X, max.X, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Y, min.Y, max.Y, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Z, min.Z, max.Z, bitCount);
            context.WriteQuantizedFloatPrimitive(value.W, min.W, max.W, bitCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedQuaternionPrimitive(this ref WriteContext context, Quaternion value, float min, float max, int bitCount) {
            context.WriteQuantizedQuaternionPrimitive(value, new Quaternion(min, min, min, min), new Quaternion(max, max, max, max), bitCount);
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedQuaternionsPrimitive(this ref WriteContext context, ReadOnlySpan<Quaternion> values, Quaternion min, Quaternion max, int bitCount) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteQuantizedQuaternionPrimitive(values[i], min, max, bitCount);
            }
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion PeekQuantizedQuaternionPrimitive(this ref ReadContext context, Quaternion min, Quaternion max, int bitCount) {
            return new Quaternion(
                context.PeekQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Y, max.Y, bitCount),
                context.PeekQuantizedFloatPrimitive(min.Z, max.Z, bitCount),
                context.PeekQuantizedFloatPrimitive(min.W, max.W, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ReadQuantizedQuaternionPrimitive(this ref ReadContext context, Quaternion min, Quaternion max, int bitCount) {
            return new Quaternion(
                context.ReadQuantizedFloatPrimitive(min.X, max.X, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Y, max.Y, bitCount),
                context.ReadQuantizedFloatPrimitive(min.Z, max.Z, bitCount),
                context.ReadQuantizedFloatPrimitive(min.W, max.W, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion[] PeekQuantizedQuaternionArrayPrimitive(this ref ReadContext context, int count, Quaternion min, Quaternion max, int bitCount) {
            Quaternion[] result = new Quaternion[count];
            context.PeekQuantizedQuaternionSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion[] ReadQuantizedQuaternionArrayPrimitive(this ref ReadContext context, int count, Quaternion min, Quaternion max, int bitCount) {
            Quaternion[] result = new Quaternion[count];
            context.ReadQuantizedQuaternionSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedQuaternionSpanPrimitive(this ref ReadContext context, int count, Span<Quaternion> destination, Quaternion min, Quaternion max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedQuaternionSpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedQuaternionSpanPrimitive(this ref ReadContext context, int count, Span<Quaternion> destination, Quaternion min, Quaternion max, int bitCount) {
            Span<Quaternion> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadQuantizedQuaternionPrimitive(min, max, bitCount);
            }
        }
    }
}
