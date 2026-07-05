using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.Quantized;

namespace ComputerysBitStream.Extras.Primitives.Quantized {
    [BitStreamQuantizedPrimitive(4, 128)]
    [BitStreamPrimitive(typeof(Plane), "QuantizedPlane", PrimitiveSerializationMode.Quantized)]
    public static class QuantizedPlaneExtensions {
        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Write)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedPlanePrimitive(this ref WriteContext context, Plane value, Plane min, Plane max, int bitCount) {
            context.WriteQuantizedFloatPrimitive(value.Normal.X, min.Normal.X, max.Normal.X, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Normal.Y, min.Normal.Y, max.Normal.Y, bitCount);
            context.WriteQuantizedFloatPrimitive(value.Normal.Z, min.Normal.Z, max.Normal.Z, bitCount);
            context.WriteQuantizedFloatPrimitive(value.D, min.D, max.D, bitCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedPlanePrimitive(this ref WriteContext context, Plane value, Vector3 normalMin, Vector3 normalMax, float dMin, float dMax, int bitCount) {
            context.WriteQuantizedPlanePrimitive(
                value,
                new Plane(normalMin, dMin),
                new Plane(normalMax, dMax),
                bitCount
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedPlanePrimitive(this ref WriteContext context, Plane value, float min, float max, int bitCount) {
            context.WriteQuantizedPlanePrimitive(value, new Plane(new Vector3(min), min), new Plane(new Vector3(max), max), bitCount);
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.WriteSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuantizedPlanesPrimitive(this ref WriteContext context, ReadOnlySpan<Plane> values, Plane min, Plane max, int bitCount) {
            for (int i = 0; i < values.Length; i++) {
                context.WriteQuantizedPlanePrimitive(values[i], min, max, bitCount);
            }
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Peek)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane PeekQuantizedPlanePrimitive(this ref ReadContext context, Plane min, Plane max, int bitCount) {
            return new Plane(
                new Vector3(
                    context.PeekQuantizedFloatPrimitive(min.Normal.X, max.Normal.X, bitCount),
                    context.PeekQuantizedFloatPrimitive(min.Normal.Y, max.Normal.Y, bitCount),
                    context.PeekQuantizedFloatPrimitive(min.Normal.Z, max.Normal.Z, bitCount)
                ),
                context.PeekQuantizedFloatPrimitive(min.D, max.D, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane ReadQuantizedPlanePrimitive(this ref ReadContext context, Plane min, Plane max, int bitCount) {
            return new Plane(
                new Vector3(
                    context.ReadQuantizedFloatPrimitive(min.Normal.X, max.Normal.X, bitCount),
                    context.ReadQuantizedFloatPrimitive(min.Normal.Y, max.Normal.Y, bitCount),
                    context.ReadQuantizedFloatPrimitive(min.Normal.Z, max.Normal.Z, bitCount)
                ),
                context.ReadQuantizedFloatPrimitive(min.D, max.D, bitCount)
            );
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane[] PeekQuantizedPlaneArrayPrimitive(this ref ReadContext context, int count, Plane min, Plane max, int bitCount) {
            Plane[] result = new Plane[count];
            context.PeekQuantizedPlaneSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadArray)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane[] ReadQuantizedPlaneArrayPrimitive(this ref ReadContext context, int count, Plane min, Plane max, int bitCount) {
            Plane[] result = new Plane[count];
            context.ReadQuantizedPlaneSpanPrimitive(count, result, min, max, bitCount);
            return result;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.PeekSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekQuantizedPlaneSpanPrimitive(this ref ReadContext context, int count, Span<Plane> destination, Plane min, Plane max, int bitCount) {
            long originalPosition = context.Position;
            context.ReadQuantizedPlaneSpanPrimitive(count, destination, min, max, bitCount);
            context.Position = originalPosition;
        }

        [BitStreamPrimitiveMethod(BitStreamPrimitiveRole.ReadSpan)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadQuantizedPlaneSpanPrimitive(this ref ReadContext context, int count, Span<Plane> destination, Plane min, Plane max, int bitCount) {
            Span<Plane> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < count; i++) {
                destinationSlice[i] = context.ReadQuantizedPlanePrimitive(min, max, bitCount);
            }
        }
    }
}
