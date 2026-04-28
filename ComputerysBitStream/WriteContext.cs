using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    /// <summary>
    /// Represents a writable bit-level buffer context used to write bits into an
    /// underlying <see cref="Span{UInt64}"/> buffer. The context tracks the current
    /// write position in bits and the total bit capacity of the provided buffer.
    /// Assumes extension methods are used to access underlying data.
    /// </summary>
    public ref struct WriteContext {
        /// <summary>
        /// Underlying buffer.
        /// </summary>
        public readonly Span<ulong> Buffer;
    
        /// <summary>
        /// Current position in bits.
        /// </summary>
        public int Position;
    
        /// <summary>
        /// Total capacity in bits.
        /// </summary>
        public readonly int Capacity;
    
        /// <summary>
        /// Remaining capacity in bits.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int GetRemainingCapacity() => Capacity - Position;

        /// <summary>
        /// Initializes a new <see cref="WriteContext"/> that uses the provided
        /// <paramref name="buffer"/> as its storage. The initial <see cref="Position"/>
        /// is set to 0 and <see cref="Capacity"/> is computed as <c>buffer.Length * BitHelper.ULongSize</c>.
        /// </summary>
        /// <param name="buffer">The underlying buffer of <see cref="BitHelper.ULongSize"/>-bit words used to store bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer) {
            Buffer = buffer;
            Position = 0;
            Capacity = buffer.Length * BitHelper.ULongSize;
        }

        /// <summary>
        /// Initializes a new <see cref="WriteContext"/> that uses the provided
        /// <paramref name="buffer"/> and sets the initial bit <paramref name="position"/>.
        /// <see cref="Capacity"/> is computed as <c>buffer.Length * BitHelper.ULongSize</c>.
        /// </summary>
        /// <param name="buffer">The underlying buffer of <see cref="BitHelper.ULongSize"/>-bit words used to store bits.</param>
        /// <param name="position">The initial bit position within the buffer.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, int position) {
            Buffer = buffer;
            Position = position;
            Capacity = buffer.Length * BitHelper.ULongSize;
        }

        /// <summary>
        /// Initializes a new <see cref="WriteContext"/> that uses the provided
        /// <paramref name="buffer"/>, initial bit <paramref name="position"/>, and an explicit
        /// <paramref name="capacity"/> in bits. Use this overload when the effective
        /// capacity differs from the full buffer length multiplied by <see cref="BitHelper.ULongSize"/>.
        /// </summary>
        /// <param name="buffer">The underlying buffer of <see cref="BitHelper.ULongSize"/>-bit words used to store bits.</param>
        /// <param name="position">The initial bit position within the buffer.</param>
        /// <param name="capacity">The total capacity in bits available for writing.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, int position, int capacity) {
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
        }

        /// <summary>
        /// Writes a single bit to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="bit"> The bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBitRaw(bool bit) {
            int ulongIndex = Position / BitHelper.ULongSize;
            int currentBitInUlong = Position % BitHelper.ULongSize;
            ulong mask = 1UL << currentBitInUlong; // only 1 at the bit position we want to write, zeros elsewhere
            ulong bitValue = bit ? 1UL : 0UL; // convert bool to ulong (0 or 1)
            Buffer[ulongIndex] = (Buffer[ulongIndex] & ~mask) // sets the bit at bitInUlong to 0
                                 | // if 1 on any side is 1, result is 1, else 0
                                 (bitValue << currentBitInUlong); // set the bit at bitInUlong to bitValue

            Position++;
        }
        
        /// <summary>
        /// Writes the given number of bits from the value to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="value"> The value containing the bits to write.</param>
        /// <param name="count">
        /// The number of bits to write.
        /// Assumes count is between <see cref="BitHelper.BoolSize"/> and <see cref="BitHelper.ULongSize"/>, inclusive, caller must ensure this.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBitsRaw(ulong value, int count) {
            int ulongIndex = Position / BitHelper.ULongSize;
            int bitOffset = Position % BitHelper.ULongSize;

            if (bitOffset != 0 || count != BitHelper.ULongSize) {
                ulong valueMask = count == BitHelper.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
                value &= valueMask;

                ulong mask = valueMask << bitOffset;
                Buffer[ulongIndex] = (Buffer[ulongIndex] & ~mask)
                                     |
                                     (value << bitOffset);

                int bitsUsedInCurrent = BitHelper.ULongSize - bitOffset;
                if (count > bitsUsedInCurrent) {
                    int bitsRemaining = count - bitsUsedInCurrent;
                    ulong nextMask = (1UL << bitsRemaining) - 1;
                    int ulongOverflow = ulongIndex + 1;
                    Buffer[ulongOverflow] = (Buffer[ulongOverflow] & ~nextMask)
                                            |
                                            (value >> bitsUsedInCurrent);
                }

                Position += count;
            } else {
                Buffer[ulongIndex] = value;
                Position += BitHelper.ULongSize;
            }
        }
    
        /// <summary>
        /// Writes the given number of bits from the source to the buffer.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="source">The source data to write.</param>
        /// <param name="count"> The number of bits to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBitsRaw(ReadOnlySpan<ulong> source, int count) {
            int fullUlongs = count / BitHelper.ULongSize;
            int remainingBits = count % BitHelper.ULongSize;
        
            int ulongIndex = Position / BitHelper.ULongSize;
            int bitOffset = Position % BitHelper.ULongSize;

            if (bitOffset > 0) {
                int invBitOffset = BitHelper.ULongSize - bitOffset;
                ulong preserveMask = (1UL << bitOffset) - 1;

                for (int i = 0; i < fullUlongs; i++) {
                    ulong value = source[i];

                    Buffer[ulongIndex] = (Buffer[ulongIndex] & preserveMask) | (value << bitOffset);
                    Buffer[ulongIndex + 1] = (Buffer[ulongIndex + 1] & ~preserveMask) | (value >> invBitOffset);
                
                    ulongIndex++;
                }
            } else {
                for (int i = 0; i < fullUlongs; i++) {
                    Buffer[ulongIndex] = source[i]; 
                    ulongIndex++;
                }
            }

            if (remainingBits > 0) {
                ulong value = source[fullUlongs];
            
                ulong valueMask = (1UL << remainingBits) - 1;
                value &= valueMask;

                ulong mask = valueMask << bitOffset;
                Buffer[ulongIndex] = (Buffer[ulongIndex] & ~mask) | (value << bitOffset);

                int bitsUsedInCurrent = BitHelper.ULongSize - bitOffset;
                if (remainingBits > bitsUsedInCurrent) {
                    int bitsOverflow = remainingBits - bitsUsedInCurrent;
                    ulong nextMask = (1UL << bitsOverflow) - 1;
                    Buffer[ulongIndex + 1] = (Buffer[ulongIndex + 1] & ~nextMask) | (value >> bitsUsedInCurrent);
                }
            }
        
            Position += count;
        }
    
        /// <summary>
        /// Reserves the given number of bits in the buffer by advancing the bit position.
        /// Does not write any data meaning if using a pooled buffer, the reserved bits may contain old data.
        /// Assumes there is enough space in the buffer, caller must ensure this.
        /// </summary>
        /// <param name="count"> The number of bits to reserve.</param>
        public void ReserveBitsRaw(int count) { Position += count; }
        
        /// <summary>
        /// Sets the current bit position in the buffer.
        /// Assumes the given position is valid, caller must ensure this.
        /// </summary>
        /// <param name="position"> The bit position to set.</param>
        public void SetPositionRaw(int position) { Position = position; }

        /// <summary>
        /// Gets a span of bytes representing the written data in the buffer.
        /// This will include garbage bits in the last byte if the total number of bits written is not a multiple of 8.
        /// </summary>
        /// <returns>A byte span that may have garbage data in the last byte.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Span<byte> ToBytesRaw() { return MemoryMarshal.Cast<ulong, byte>(Buffer).Slice(0, BitHelper.BitsToBytes(Position)); }

        /// <summary>
        /// Gets a span of bytes representing the written data in the buffer with garbage bits removed.
        /// </summary>
        /// <returns>A byte span without garbage data in the last byte.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> ToBytes() {
            int totalBytes = BitHelper.BitsToBytes(Position);
            Span<byte> span = MemoryMarshal.Cast<ulong, byte>(Buffer).Slice(0, totalBytes);
            
            int usedBitsInLastByte = Position & BitHelper.OneLessThanByteSize;
            if (usedBitsInLastByte != 0) {
                span[totalBytes - 1] &= (byte)((1 << usedBitsInLastByte) - 1);
            }

            return span;
        }
    
        /// <summary>
        /// Throws an InsufficientWriteSpaceException if there is no space for the bits needed.
        /// </summary>
        /// <param name="type">Type name that will be in the Exception.</param>
        /// <param name="bitsNeeded"> The size of the thing you are writing</param>
        /// <exception cref="InsufficientWriteSpaceException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ThrowIfNoSpace(string type, int bitsNeeded) {
            int remainingCapacity = GetRemainingCapacity();
            if (remainingCapacity < bitsNeeded) { throw new InsufficientWriteSpaceException(type, bitsNeeded, remainingCapacity); }
        }
    }

    public class InsufficientWriteSpaceException : Exception {
        public InsufficientWriteSpaceException(string type, int requiredBits, int availableBits) : base($"Insufficient space to write {type}. Required bits: {requiredBits}, Available bits: {availableBits}.") { }
    }
}