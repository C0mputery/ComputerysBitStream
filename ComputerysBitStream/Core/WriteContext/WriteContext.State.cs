using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    public ref partial struct WriteContext {
        /// <summary>Returns <see cref="Capacity"/> minus <see cref="Position"/> in bits.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long GetRemainingCapacity() => Capacity - Position;

        /// <summary>Bit index within the current ulong (<c>Position % 64</c>).</summary>
        public readonly int BitOffset => BitHelper.GetBitOffset(Position);
        /// <summary><c>true</c> when <see cref="BitOffset"/> is zero.</summary>
        public readonly bool IsUlongAligned => BitOffset == 0;
        /// <summary><c>true</c> when <see cref="Position"/> is a multiple of 8.</summary>
        public readonly bool IsByteAligned => Position % BitHelper.ByteSize == 0;

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToBytePrimitive() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ByteSize);
            if (padding != 0) { WriteBitsPrimitive(0, padding); }
        }

        /// <summary>Writes zero padding bits to reach the next byte boundary after a capacity check.</summary>
        /// <remarks>Unlike <see cref="AlignToBytePrimitive"/>, throws <see cref="InsufficientWriteCapacityException"/> when padding bits are not available.</remarks>
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

        /// <summary>Writes zero padding bits to reach the next ulong boundary after a capacity check.</summary>
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

        /// <summary>Advances <see cref="Position"/> by <paramref name="bitCount"/> after a capacity check. Does not write payload bits.</summary>
        /// <param name="bitCount">Bits to reserve. Must be non-negative.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReserveBits(int bitCount) {
            if (bitCount < 0) { throw new ArgumentOutOfRangeException(nameof(bitCount), bitCount, "Bit count must be non-negative."); }
            ThrowIfInsufficientSpace("reserved bits", bitCount);
            ReserveBitsPrimitive(bitCount);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        public void SetPositionPrimitive(long position) { Position = position; }

        /// <summary>Sets <see cref="Position"/> after validating against <see cref="Capacity"/>.</summary>
        /// <param name="position">New write position in bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPosition(long position) {
            ThrowIfInvalidPosition(position, Capacity);
            SetPositionPrimitive(position);
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Span<byte> WrittenBytesSpanPrimitive() { return MemoryMarshal.Cast<ulong, byte>(Buffer).Slice(0, BitHelper.BitsToBytes(Position)); }

        /// <summary>Returns a byte span covering bits written so far, rounding up to a byte boundary for the view.</summary>
        /// <remarks>Temporarily aligns to a byte boundary, copies the view, then restores <see cref="Position"/>.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetWrittenBytes() {
            if (!IsByteAligned) {
                long bitsWritten = Position;
                AlignToByte();
                SetPosition(bitsWritten);
            }

            return WrittenBytesSpanPrimitive();
        }

        /// <summary>Read-only view of <see cref="GetWrittenBytes"/>.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> GetWrittenBytesReadonly() { return GetWrittenBytes(); }

        /// <summary>Allocates a byte array copy of <see cref="GetWrittenBytesReadonly"/>.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToByteArray() { return GetWrittenBytesReadonly().ToArray(); }

        /// <summary>Throws <see cref="InsufficientWriteCapacityException"/> when fewer than <paramref name="bitCount"/> bits remain.</summary>
        /// <param name="operation">Label used in the exception message.</param>
        /// <param name="bitCount">Required bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfInsufficientSpace(string operation, int bitCount) {
            long availableBits = GetRemainingCapacity();
            if (availableBits < bitCount) { throw new InsufficientWriteCapacityException(operation, bitCount, availableBits, Position); }
        }

        /// <summary>Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="position"/> exceeds <see cref="Capacity"/>.</summary>
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
