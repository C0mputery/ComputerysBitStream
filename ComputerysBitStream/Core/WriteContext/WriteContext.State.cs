using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    public ref partial struct WriteContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long GetRemainingCapacity() => Capacity - Position;

        public readonly int BitOffset => BitHelper.GetBitOffset(Position);
        public readonly bool IsUlongAligned => BitOffset == 0;
        public readonly bool IsByteAligned => Position % BitHelper.ByteSize == 0;

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToBytePrimitive() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ByteSize);
            if (padding != 0) { WriteBitsPrimitive(0, padding); }
        }

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
        public void AlignToUlongPrimitive() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ULongSize);
            if (padding != 0) { WriteBitsPrimitive(0, padding); }
        }

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
        public void ReserveBitsPrimitive(int bitCount) { Position += bitCount; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReserveBits(int bitCount) {
            if (bitCount < 0) { throw new ArgumentOutOfRangeException(nameof(bitCount), bitCount, "Bit count must be non-negative."); }
            ThrowIfInsufficientSpace("reserved bits", bitCount);
            ReserveBitsPrimitive(bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        public void SetPositionPrimitive(long position) { Position = position; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPosition(long position) {
            ThrowIfInvalidPosition(position, Capacity);
            SetPositionPrimitive(position);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Span<byte> WrittenBytesSpanPrimitive() { return MemoryMarshal.Cast<ulong, byte>(Buffer).Slice(0, BitHelper.BitsToBytes(Position)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetWrittenBytes() {
            if (!IsByteAligned) {
                long bitsWritten = Position;
                AlignToByte();
                SetPosition(bitsWritten);
            }

            return WrittenBytesSpanPrimitive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> GetWrittenBytesReadonly() { return GetWrittenBytes(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToByteArray() { return GetWrittenBytesReadonly().ToArray(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfInsufficientSpace(string operation, int bitCount) {
            long availableBits = GetRemainingCapacity();
            if (availableBits < bitCount) { throw new InsufficientWriteCapacityException(operation, bitCount, availableBits, Position); }
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
