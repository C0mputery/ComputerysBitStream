using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(bool), BitHelper.BoolSize)]
    public static class RawBoolExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.BoolSize;
    
        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBoolRaw(this ref WriteContext context, bool value) { context.WriteBitRaw(value); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBoolsRaw(this ref WriteContext context, ReadOnlySpan<bool> values) {
            int count = values.Length;
            int processed = 0;

            while (processed + NumberOfValuesInUlong <= count) {
                ulong packed = 0;
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    packed |= (values[processed + i] ? 1UL : 0UL) << i;
                }
                context.WriteBitsRaw(packed, BitHelper.ULongSize);
                processed += NumberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = 0;
                for (int i = 0; i < remaining; i++) {
                    packed |= (values[processed + i] ? 1UL : 0UL) << i;
                }
                context.WriteBitsRaw(packed, remaining * BitHelper.BoolSize);
            }
        }
    
        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PeekBoolRaw(this ref ReadContext context) { return context.PeekBitRaw(); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolRaw(this ref ReadContext context) { return context.ReadBitRaw(); }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] PeekBoolArrayRaw(this ref ReadContext context, int count) {
            bool[] result = new bool[count];
            Span<bool> span = result.AsSpan();
            context.PeekBoolSpanRaw(count, ref span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] ReadBoolArrayRaw(this ref ReadContext context, int count) {
            bool[] result = new bool[count];
            Span<bool> span = result.AsSpan();
            context.ReadBoolSpanRaw(count, ref span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekBoolSpanRaw(this ref ReadContext context, int count, ref Span<bool> destination) {
            int originalPosition = context.Position;
            context.ReadBoolSpanRaw(count, ref destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadBoolSpanRaw(this ref ReadContext context, int count, ref Span<bool> destination) {
            int processed = 0;

            while (processed + NumberOfValuesInUlong <= count) {
                ulong packed = context.ReadBitsRaw(BitHelper.ULongSize);
                for (int i = 0; i < NumberOfValuesInUlong; i++) {
                    destination[processed + i] = (packed & (1UL << i)) != 0UL;
                }
                processed += NumberOfValuesInUlong;
            }

            int remaining = count - processed;
            if (remaining > 0) {
                ulong packed = context.ReadBitsRaw(remaining * BitHelper.BoolSize);
                for (int i = 0; i < remaining; i++) {
                    destination[processed + i] = (packed & (1UL << i)) != 0UL;
                }
            }
        }
    }
}
