using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;

// ReSharper disable InconsistentNaming

namespace ComputerysBitStream.Extras.Primitives.Quantized {
    /// <summary>Built-in reference implementation of <see cref="BitStreamPrimitiveAttribute"/>. See <see cref="BitStreamPrimitiveAuthorDocumentation"/>.</summary>
    [BitStreamQuantizedPrimitive(16, 512)]
    [BitStreamPrimitive(typeof(Matrix4x4), "QuantizedMatrix4x4", PrimitiveSerializationMode.Quantized)]
    public static class QuantizedMatrix4x4Extensions {
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedMatrix4x4Primitive(this ref WriteContext context, Matrix4x4 value, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            context.WriteQuantizedFloatPrimitive(value.M11, min.M11, max.M11, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M12, min.M12, max.M12, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M13, min.M13, max.M13, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M14, min.M14, max.M14, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M21, min.M21, max.M21, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M22, min.M22, max.M22, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M23, min.M23, max.M23, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M24, min.M24, max.M24, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M31, min.M31, max.M31, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M32, min.M32, max.M32, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M33, min.M33, max.M33, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M34, min.M34, max.M34, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M41, min.M41, max.M41, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M42, min.M42, max.M42, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M43, min.M43, max.M43, bitCount);
            context.WriteQuantizedFloatPrimitive(value.M44, min.M44, max.M44, bitCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedMatrix4x4Primitive(this ref WriteContext context, Matrix4x4 value, float min, float max, int bitCount) {
            Matrix4x4 minMatrix = new Matrix4x4(min, min, min, min, min, min, min, min, min, min, min, min, min, min, min, min);
            Matrix4x4 maxMatrix = new Matrix4x4(max, max, max, max, max, max, max, max, max, max, max, max, max, max, max, max);
            context.WriteQuantizedMatrix4x4Primitive(value, minMatrix, maxMatrix, bitCount);
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedMatrix4x4sPrimitive(this ref WriteContext context, ReadOnlySpan<Matrix4x4> values, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteQuantizedMatrix4x4Primitive(values[i], min, max, bitCount);
            }
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 PeekQuantizedMatrix4x4Primitive(this ref ReadContext context, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            return new Matrix4x4(
                context.ReadQuantizedFloatPrimitive(min.M11, max.M11, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M12, max.M12, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M13, max.M13, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M14, max.M14, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M21, max.M21, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M22, max.M22, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M23, max.M23, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M24, max.M24, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M31, max.M31, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M32, max.M32, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M33, max.M33, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M34, max.M34, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M41, max.M41, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M42, max.M42, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M43, max.M43, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M44, max.M44, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 ReadQuantizedMatrix4x4Primitive(this ref ReadContext context, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            return new Matrix4x4(
                context.ReadQuantizedFloatPrimitive(min.M11, max.M11, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M12, max.M12, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M13, max.M13, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M14, max.M14, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M21, max.M21, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M22, max.M22, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M23, max.M23, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M24, max.M24, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M31, max.M31, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M32, max.M32, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M33, max.M33, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M34, max.M34, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M41, max.M41, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M42, max.M42, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M43, max.M43, bitCount),
                context.ReadQuantizedFloatPrimitive(min.M44, max.M44, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4[] PeekQuantizedMatrix4x4ArrayPrimitive(this ref ReadContext context, int count, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            Matrix4x4[] result = new Matrix4x4[count];
            context.PeekQuantizedMatrix4x4SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4[] ReadQuantizedMatrix4x4ArrayPrimitive(this ref ReadContext context, int count, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            Matrix4x4[] result = new Matrix4x4[count];
            context.ReadQuantizedMatrix4x4SpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedMatrix4x4SpanPrimitive(this ref ReadContext context, int count, Span<Matrix4x4> destination, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedMatrix4x4SpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedMatrix4x4SpanPrimitive(this ref ReadContext context, int count, Span<Matrix4x4> destination, Matrix4x4 min, Matrix4x4 max, int bitCount) {
            Span<Matrix4x4> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadQuantizedMatrix4x4Primitive(min, max, bitCount);
            }
        }
    }
}
