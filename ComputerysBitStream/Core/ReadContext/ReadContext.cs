using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    /// <summary>
    /// Read cursor over a <see cref="ReadOnlySpan{T}"/> of <see cref="ulong"/> backing storage. Positions and limits are in bits.
    /// </summary>
    /// <remarks>
    /// <para>Construct with a buffer spanning the bits to read. Use generated <c>Read*</c>, <c>Peek*</c>, and <c>Try*</c> extension methods for typed values.</para>
    /// <para><see cref="Capacity"/> can be smaller than the backing buffer when only a prefix is valid data. It is mutable via <see cref="SetCapacity(long)"/>.</para>
    /// </remarks>
    [BitStreamPrimitiveContext]
    public ref partial struct ReadContext {
        /// <summary>Backing ulong span. May be longer than the active bit capacity.</summary>
        public readonly ReadOnlySpan<ulong> Buffer;

        /// <summary>Current read position in bits from the start of the buffer.</summary>
        public long Position;

        /// <summary>Number of bits that may be read. Defaults to <c>buffer.Length * 64</c> and can be reduced with <see cref="SetCapacity(long)"/>.</summary>
        public long Capacity;

        /// <summary>Starts at position 0 with capacity equal to the full buffer.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer) {
            Buffer = buffer;
            Position = 0;
            Capacity = (long)buffer.Length * BitHelper.ULongSize;
        }

        /// <summary>Starts at <paramref name="position"/> with capacity equal to the full buffer.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        /// <param name="position">Initial position in bits. Must be between 0 and the buffer capacity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer, long position) {
            long capacity = (long)buffer.Length * BitHelper.ULongSize;
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
        }

        /// <summary>Starts at <paramref name="position"/> with an explicit bit capacity no larger than the buffer.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        /// <param name="position">Initial position in bits.</param>
        /// <param name="capacity">Active bit capacity. Must not exceed <c>buffer.Length * 64</c>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer, long position, long capacity) {
            ThrowIfInvalidCapacity(capacity, (long)buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
        }
    }
}
