using System;
using System.Runtime.CompilerServices;

namespace ComputerysBitStream {
    /// <summary>
    /// Represents a bit-level read cursor over a <see cref="ReadOnlySpan{UInt64}"/> buffer.
    /// The struct stores the underlying buffer, the current bit position and the total
    /// capacity in bits.
    /// Assumes extension methods are used to access underlying data.
    /// </summary>
    public ref struct ReadContext {
        /// <summary>
        /// Underlying buffer.
        /// </summary>
        public readonly ReadOnlySpan<ulong> Buffer;
    
        /// <summary>
        /// Current position in bits.
        /// </summary>
        public int Position;
    
        /// <summary>
        /// Total capacity in bits.
        /// </summary>
        public int Capacity;

        /// <summary>
        /// Initializes a new <see cref="ReadContext"/> over the specified buffer.
        /// The initial position is set to zero and the capacity is set to <c>buffer.Length * 64</c>.
        /// </summary>
        /// <param name="buffer">The underlying buffer of 64-bit words to read bits from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer) {
            Buffer = buffer;
            Position = 0;
            Capacity = buffer.Length * 64;
        }

        /// <summary>
        /// Initializes a new <see cref="ReadContext"/> over the specified buffer with
        /// the provided starting bit <paramref name="position"/>.
        /// The capacity is set to <c>buffer.Length * 64</c>.
        /// </summary>
        /// <param name="buffer">The underlying buffer of 64-bit words to read bits from.</param>
        /// <param name="position">Initial bit position within the buffer.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer, int position) {
            Buffer = buffer;
            Position = position;
            Capacity = buffer.Length * 64;
        }

        /// <summary>
        /// Initializes a new <see cref="ReadContext"/> over the specified buffer with
        /// the provided starting bit <paramref name="position"/> and explicit <paramref name="capacity"/>.
        /// Use this overload when only part of the underlying buffer should be considered readable.
        /// </summary>
        /// <param name="buffer">The underlying buffer of 64-bit words to read bits from.</param>
        /// <param name="position">Initial bit position within the buffer.</param>
        /// <param name="capacity">Total readable capacity in bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer, int position, int capacity) {
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
        }

        /// <summary>
        /// Remaining capacity in bits.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int GetRemainingCapacity() => Capacity - Position;
    
        /// <summary>
        /// Peeks the next bit in the buffer, without moving the position.
        /// Assumes there is enough space remaining, caller must ensure this.
        /// </summary>
        /// <returns>bit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool PeekBitRaw() {
            int ulongIndex = Position / BitHelper.ULongSize;
            int bitOffset = Position % BitHelper.ULongSize;
            return (Buffer[ulongIndex] & (1UL << bitOffset)) != 0;
        }
    
        /// <summary>
        /// Reads the next bit in the buffer.
        /// Assumes there is enough space remaining, caller must ensure this.
        /// </summary>
        /// <returns>bit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBitRaw() {
            bool value = PeekBitRaw();
            Position++;
            return value;
        }
    
        /// <summary>
        /// Peeks the next bits in the buffer, without moving the position.
        /// Assumes there is enough space remaining, caller must ensure this.
        /// </summary>
        /// <param name="count">
        /// The number of bits to read.
        /// Assumes count is between 1 and 64, inclusive, caller must ensure this.
        /// </param>
        /// <returns>bits</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong PeekBitsRaw(int count) {
            int ulongIndex = Position / BitHelper.ULongSize;
            int bitOffset = Position % BitHelper.ULongSize;

            if (bitOffset != 0 || count != 64) {
                ulong valueMask = count == BitHelper.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
                ulong result = (Buffer[ulongIndex] >> bitOffset);

                int bitsAvailableInCurrent = BitHelper.ULongSize - bitOffset;
                if (count > bitsAvailableInCurrent) {
                    result |= (Buffer[ulongIndex + 1] << bitsAvailableInCurrent);
                }

                return result & valueMask;
            } else { return Buffer[ulongIndex]; }
        }

        /// <summary>
        /// Reads the next bits in the buffer.
        /// Assumes there is enough space remaining, caller must ensure this.
        /// </summary>
        /// <param name="count">
        /// The number of bits to write.
        /// Assumes count is between 1 and 64, inclusive, caller must ensure this.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadBitsRaw(int count) {
            ulong value = PeekBitsRaw(count);
            Position += count;
            return value;
        }
    
        /// <summary>
        /// Peeks the next bits in the buffer, without moving the position.
        /// Assumes there is enough space remaining, caller must ensure this.
        /// </summary>
        /// <param name="count">
        /// The number of bits to peek.
        /// Assumes count can fit within the buffer, caller must ensure this.
        /// </param>
        /// <param name="buffer"> output </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void PeekBitsRaw(int count, Span<ulong> buffer) {
            int fullUlongs = count / BitHelper.ULongSize;
            int remainingBits = count % BitHelper.ULongSize;
        
            int ulongIndex = Position / BitHelper.ULongSize;
            int bitOffset = Position % BitHelper.ULongSize;

            if (bitOffset > 0) {
                int invBitOffset = BitHelper.ULongSize - bitOffset;

                for (int i = 0; i < fullUlongs; i++) {
                    buffer[i] = (Buffer[ulongIndex] >> bitOffset) | (Buffer[ulongIndex + 1] << invBitOffset);
                    ulongIndex++;
                }
            } else {
                for (int i = 0; i < fullUlongs; i++) {
                    buffer[i] = Buffer[ulongIndex];
                    ulongIndex++;
                }
            }

            if (remainingBits > 0) {
                ulong valueMask = (1UL << remainingBits) - 1;
                ulong result = (Buffer[ulongIndex] >> bitOffset) & valueMask;

                int bitsAvailableInCurrent = BitHelper.ULongSize - bitOffset;
                if (remainingBits > bitsAvailableInCurrent) {
                    result |= (Buffer[ulongIndex + 1] << bitsAvailableInCurrent) & valueMask;
                }

                buffer[fullUlongs] = result;
            }
        }
    
        /// <summary>
        /// Reads the next bits in the buffer.
        /// Assumes there is enough space remaining, caller must ensure this.
        /// </summary>
        /// <param name="count">
        /// The number of bits to read.
        /// Assumes count can fit within the buffer, caller must ensure this.
        /// </param>
        /// <param name="buffer"> output </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBitsRaw(int count, Span<ulong> buffer) { 
            PeekBitsRaw(count, buffer);
            Position += count;
        }
    
        /// <summary>
        /// Sets the capacity in bits.
        /// Assumes the given capacity is valid, caller must ensure this.
        /// </summary>
        /// <param name="capacity">Capacity in bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCapacityRaw(int capacity) { Capacity = capacity; }

        /// <summary>
        /// Sets the position in bits.
        /// Assumes the given position is valid, caller must ensure this.
        /// </summary>
        /// <param name="position">Position in bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPositionRaw(int position) { Position = position; }
    
        /// <summary>
        /// Checks if there is the number of bits in the capacity.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool HasSpaceRemaining(int bits) { return GetRemainingCapacity() >= bits; }
    
        /// <summary>
        /// Checks if there is not enough space for the specified number of bits.
        /// </summary>
        /// <param name="bits">The number of bits to check.</param>
        /// <returns>True if there is not enough space remaining.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsInsufficientSpace(int bits) { return GetRemainingCapacity() < bits; }
    }
}