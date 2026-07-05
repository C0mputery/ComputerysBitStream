using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    public ref partial struct ReadContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long GetRemainingCapacity() => Capacity - Position;

        public readonly int BitOffset => BitHelper.GetBitOffset(Position);

        public readonly bool IsUlongAligned => BitOffset == 0;

        public readonly bool IsByteAligned => Position % BitHelper.ByteSize == 0;

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToBytePrimitive() { Position += BitHelper.PaddingBitsToAlign(Position, BitHelper.ByteSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToByte() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ByteSize);
            if (padding != 0) {
                ThrowIfInsufficientSpace("byte alignment", padding);
                AlignToBytePrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToUlongPrimitive() { Position += BitHelper.PaddingBitsToAlign(Position, BitHelper.ULongSize); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToUlong() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ULongSize);
            if (padding != 0) {
                ThrowIfInsufficientSpace("ulong alignment", padding);
                AlignToUlongPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCapacityPrimitive(long capacity) { Capacity = capacity; }

        public void SetCapacity(long capacity) {
            ThrowIfInvalidCapacity(capacity, Buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(Position, capacity);
            SetCapacityPrimitive(capacity);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPositionPrimitive(long position) { Position = position; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPosition(long position) {
            ThrowIfInvalidPosition(position, Capacity);
            SetPositionPrimitive(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HasSpaceRemaining(int bitCount) { return GetRemainingCapacity() >= bitCount; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsInsufficientSpace(int bitCount) { return GetRemainingCapacity() < bitCount; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfInsufficientSpace(string type, int bitCount) {
            long availableBits = GetRemainingCapacity();
            if (availableBits < bitCount) { throw new InsufficientReadSpaceException(type, bitCount, availableBits, Position); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfReadFailed(string type) {
            throw new BitStreamReadException(type, GetRemainingCapacity(), Position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfInvalidPosition(long position) { ThrowIfInvalidPosition(position, Capacity); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowIfInvalidPosition(long position, long capacity) {
            if ((ulong)position > (ulong)capacity) { throw new ArgumentOutOfRangeException(nameof(position), position, $"Position must be between 0 and {capacity}."); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowIfInvalidCapacity(long capacity, long maxCapacity) {
            if ((ulong)capacity > (ulong)maxCapacity) { throw new ArgumentOutOfRangeException(nameof(capacity), capacity, $"Capacity must be between 0 and {maxCapacity}."); }
        }
    }
}
