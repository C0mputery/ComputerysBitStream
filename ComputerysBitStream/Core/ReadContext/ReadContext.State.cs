using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    public ref partial struct ReadContext {
        /// <summary>Returns <see cref="Capacity"/> minus <see cref="Position"/> in bits.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long GetRemainingCapacity() => Capacity - Position;

        /// <summary>Bit index within the current ulong (<c>Position % 64</c>).</summary>
        public readonly int BitOffset => BitHelper.GetBitOffset(Position);

        /// <summary><c>true</c> when <see cref="BitOffset"/> is zero.</summary>
        public readonly bool IsUlongAligned => BitOffset == 0;

        /// <summary><c>true</c> when <see cref="Position"/> is a multiple of 8.</summary>
        public readonly bool IsByteAligned => Position % BitHelper.ByteSize == 0;

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToBytePrimitive() { Position += BitHelper.PaddingBitsToAlign(Position, BitHelper.ByteSize); }

        /// <summary>Advances <see cref="Position"/> to the next byte boundary after a capacity check.</summary>
        /// <remarks>Unlike <see cref="AlignToBytePrimitive"/>, throws <see cref="InsufficientReadSpaceException"/> when padding bits are not available.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToByte() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ByteSize);
            if (padding != 0) {
                ThrowIfInsufficientSpace("byte alignment", padding);
                AlignToBytePrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToUlongPrimitive() { Position += BitHelper.PaddingBitsToAlign(Position, BitHelper.ULongSize); }

        /// <summary>Advances <see cref="Position"/> to the next ulong boundary after a capacity check.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AlignToUlong() {
            int padding = BitHelper.PaddingBitsToAlign(Position, BitHelper.ULongSize);
            if (padding != 0) {
                ThrowIfInsufficientSpace("ulong alignment", padding);
                AlignToUlongPrimitive();
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCapacityPrimitive(long capacity) { Capacity = capacity; }

        /// <summary>Sets <see cref="Capacity"/> after validating against the backing buffer and current position.</summary>
        /// <param name="capacity">New active bit capacity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCapacity(long capacity) {
            ThrowIfInvalidCapacity(capacity, (long)Buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(Position, capacity);
            SetCapacityPrimitive(capacity);
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPositionPrimitive(long position) { Position = position; }

        /// <summary>Sets <see cref="Position"/> after validating against <see cref="Capacity"/>.</summary>
        /// <param name="position">New read position in bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPosition(long position) {
            ThrowIfInvalidPosition(position, Capacity);
            SetPositionPrimitive(position);
        }

        /// <summary>Returns whether at least <paramref name="bitCount"/> bits remain.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HasSpaceRemaining(int bitCount) { return GetRemainingCapacity() >= bitCount; }

        /// <summary>Returns whether fewer than <paramref name="bitCount"/> bits remain.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsInsufficientSpace(int bitCount) { return GetRemainingCapacity() < bitCount; }

        /// <summary>Throws <see cref="InsufficientReadSpaceException"/> when fewer than <paramref name="bitCount"/> bits remain.</summary>
        /// <param name="type">Label used in the exception message.</param>
        /// <param name="bitCount">Required bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfInsufficientSpace(string type, int bitCount) {
            long availableBits = GetRemainingCapacity();
            if (availableBits < bitCount) { throw new InsufficientReadSpaceException(type, bitCount, availableBits, Position); }
        }

        /// <summary>Throws <see cref="BitStreamReadException"/> for a failed read of <paramref name="type"/>.</summary>
        /// <param name="type">Label used in the exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfReadFailed(string type) {
            throw new BitStreamReadException(type, GetRemainingCapacity(), Position);
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
