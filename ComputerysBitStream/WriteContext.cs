using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream;

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
    public int GetRemainingCapacity() => Capacity - Position;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public WriteContext(Span<ulong> buffer) {
        Buffer = buffer;
        Position = 0;
        Capacity = buffer.Length * 64;
    }
    
    /// <summary>
    /// Writes a single bit to the buffer.
    /// Assumes there is enough space in the buffer, caller must ensure this.
    /// </summary>
    /// <param name="bit"> The bit to write.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBitRaw(bool bit) {
        int ulongIndex = Position / BitSizes.ULongSize;
        int currentBitInUlong = Position % BitSizes.ULongSize;
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
    /// Assumes count is between 1 and 64, inclusive, caller must ensure this.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBitsRaw(ulong value, int count) {
        int ulongIndex = Position / BitSizes.ULongSize;
        int bitOffset = Position % BitSizes.ULongSize;

        if (bitOffset != 0 || count != 64) {
            ulong valueMask = count == BitSizes.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
            value &= valueMask;

            ulong mask = valueMask << bitOffset;
            Buffer[ulongIndex] = (Buffer[ulongIndex] & ~mask)
                                 |
                                 (value << bitOffset);

            int bitsUsedInCurrent = BitSizes.ULongSize - bitOffset;
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
            Position += 64;
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
        int fullUlongs = count / BitSizes.ULongSize;
        int remainingBits = count % BitSizes.ULongSize;
        
        int ulongIndex = Position / BitSizes.ULongSize;
        int bitOffset = Position % BitSizes.ULongSize;

        if (bitOffset > 0) {
            int invBitOffset = BitSizes.ULongSize - bitOffset;
            ulong preserveMask = (1UL << bitOffset) - 1;

            for (int i = 0; i < fullUlongs; i++) {
                ulong val = source[i];

                Buffer[ulongIndex] = (Buffer[ulongIndex] & preserveMask) | (val << bitOffset);
                Buffer[ulongIndex + 1] = (Buffer[ulongIndex + 1] & ~preserveMask) | (val >> invBitOffset);
                
                ulongIndex++;
            }
        } else {
            for (int i = 0; i < fullUlongs; i++) {
                Buffer[ulongIndex] = source[i]; 
                ulongIndex++;
            }
        }

        if (remainingBits > 0) {
            ulong val = source[fullUlongs];
            
            ulong valMask = (1UL << remainingBits) - 1;
            val &= valMask;

            ulong mask = valMask << bitOffset;
            Buffer[ulongIndex] = (Buffer[ulongIndex] & ~mask) | (val << bitOffset);

            int bitsUsedInCurrent = BitSizes.ULongSize - bitOffset;
            if (remainingBits > bitsUsedInCurrent) {
                int bitsOverflow = remainingBits - bitsUsedInCurrent;
                ulong nextMask = (1UL << bitsOverflow) - 1;
                Buffer[ulongIndex + 1] = (Buffer[ulongIndex + 1] & ~nextMask) | (val >> bitsUsedInCurrent);
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
    /// <returns></returns>
    public Span<byte> ToByte() {
        int relevantUlongs = (Position + 63) / 64;
        Span<ulong> relevantBuffer = Buffer.Slice(0, relevantUlongs);
        int totalBytes = (Position + 7) / 8;
        return MemoryMarshal.Cast<ulong, byte>(relevantBuffer).Slice(0, totalBytes);
    }
    
    /// <summary>
    /// Throws an InsufficientWriteSpaceException if there is no space for the bits needed.
    /// </summary>
    /// <param name="type">Type name that will be in the Exception.</param>
    /// <param name="bitsNeeded"> The size of the thing you are writing</param>
    /// <exception cref="InsufficientWriteSpaceException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ThrowIfNoSpace(string type, int bitsNeeded) {
        int remainingCapacity = GetRemainingCapacity();
        if (remainingCapacity < bitsNeeded) { throw new InsufficientWriteSpaceException(type, bitsNeeded, remainingCapacity); }
    }
}

public class InsufficientWriteSpaceException(string type, int requiredBits, int availableBits) : 
    Exception($"Insufficient space to write {type}. Required bits: {requiredBits}, Available bits: {availableBits}.");