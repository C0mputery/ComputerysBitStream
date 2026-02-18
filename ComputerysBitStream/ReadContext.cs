using System;
using System.Runtime.CompilerServices;

namespace ComputerysBitStream;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadContext(ReadOnlySpan<ulong> buffer) {
        Buffer = buffer;
        Position = 0;
        Capacity = buffer.Length * 64;
    }
    
    /// <summary>
    /// Remaining capacity in bits.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetRemainingCapacity() => Capacity - Position;
    
    /// <summary>
    /// Peaks the next bit in the buffer, without moving the position.
    /// Assumes there is enough space remaining, caller must ensure this.
    /// </summary>
    /// <returns>bit</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool PeakBitRaw() {
        int ulongIndex = Position / BitSizes.ULongSize;
        int bitOffset = Position % BitSizes.ULongSize;
        return (Buffer[ulongIndex] & (1UL << bitOffset)) != 0;
    }
    
    /// <summary>
    /// Reads the next bit in the buffer.
    /// Assumes there is enough space remaining, caller must ensure this.
    /// </summary>
    /// <returns>bit</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ReadBitRaw() {
        bool value = PeakBitRaw();
        Position++;
        return value;
    }
    
    /// <summary>
    /// Peaks the next bits in the buffer, without moving the position.
    /// Assumes there is enough space remaining, caller must ensure this.
    /// </summary>
    /// <param name="count">
    /// The number of bits to read.
    /// Assumes count is between 1 and 64, inclusive, caller must ensure this.
    /// </param>
    /// <returns>bits</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong PeakBitsRaw(int count) {
        int ulongIndex = Position / BitSizes.ULongSize;
        int bitOffset = Position % BitSizes.ULongSize;

        if (bitOffset != 0 || count != 64) {
            ulong valueMask = count == BitSizes.ULongSize ? ulong.MaxValue : (1UL << count) - 1;
            ulong result = (Buffer[ulongIndex] >> bitOffset);

            int bitsAvailableInCurrent = BitSizes.ULongSize - bitOffset;
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
        ulong value = PeakBitsRaw(count);
        Position += count;
        return value;
    }
    
    /// <summary>
    /// Peaks the next bits in the buffer, without moving the position.
    /// Assumes there is enough space remaining, caller must ensure this.
    /// </summary>
    /// <param name="count">
    /// The number of bits to peak.
    /// Assumes count can fit within the buffer, caller must ensure this.
    /// </param>
    /// <param name="buffer"> output </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PeakBitsRaw(int count, Span<ulong> buffer) {
        int fullUlongs = count / BitSizes.ULongSize;
        int remainingBits = count % BitSizes.ULongSize;
        
        int ulongIndex = Position / BitSizes.ULongSize;
        int bitOffset = Position % BitSizes.ULongSize;

        if (bitOffset > 0) {
            int invBitOffset = BitSizes.ULongSize - bitOffset;

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

            int bitsAvailableInCurrent = BitSizes.ULongSize - bitOffset;
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
        PeakBitsRaw(count, buffer);
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
    public bool HasSpaceRemainingRaw(int bits) { return GetRemainingCapacity() >= bits; }
}