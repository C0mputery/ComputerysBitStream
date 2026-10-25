using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    /// <summary>
    /// Write cursor over a <see cref="Span{T}"/> of <see cref="ulong"/> backing storage. Positions and limits are in bits.
    /// </summary>
    /// <remarks>
    /// <para>Construct with a buffer spanning the bits to write. Use generated <c>Write*</c> extension methods for typed values.</para>
    /// <para><see cref="Capacity"/> is fixed at construction. <see cref="GetWrittenBytes"/> returns a byte view of bits written so far.</para>
    /// </remarks>
    [BitStreamPrimitiveContext]
    public ref partial struct WriteContext {
        /// <summary>Backing ulong span.</summary>
        public readonly Span<ulong> Buffer;

        /// <summary>Current write position in bits from the start of the buffer.</summary>
        public long Position;

        /// <summary>Maximum number of bits that may be written. Set at construction and not mutable.</summary>
        public readonly long Capacity;
#if BITSTREAM_SUPPORT_THREAD_SAFE
        /// <summary>When <c>true</c>, concurrent writes use synchronization. Compile with <c>BITSTREAM_SUPPORT_THREAD_SAFE</c>.</summary>
        public bool ThreadSafe; // TODO, it would be better if rather than a toggle, there was a thread safety write context object so that there was no runtime cost.
#endif

        /// <summary>Starts at position 0 with capacity equal to the full buffer.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer) {
            Buffer = buffer;
            Position = 0;
            Capacity = (long)buffer.Length * BitHelper.ULongSize;
#if BITSTREAM_SUPPORT_THREAD_SAFE
            ThreadSafe = false;
#endif
        }

        /// <summary>Starts at <paramref name="position"/> with capacity equal to the full buffer.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        /// <param name="position">Initial position in bits.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, long position) {
            long capacity = (long)buffer.Length * BitHelper.ULongSize;
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
#if BITSTREAM_SUPPORT_THREAD_SAFE
            ThreadSafe = false;
#endif
        }

        /// <summary>Starts at <paramref name="position"/> with an explicit bit capacity no larger than the buffer.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        /// <param name="position">Initial position in bits.</param>
        /// <param name="capacity">Maximum writable bits. Must not exceed <c>buffer.Length * 64</c>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, long position, long capacity) {
            ThrowIfInvalidCapacity(capacity, (long)buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
#if BITSTREAM_SUPPORT_THREAD_SAFE
            ThreadSafe = false;
#endif
        }

#if BITSTREAM_SUPPORT_THREAD_SAFE
        /// <summary>Starts at <paramref name="position"/> with explicit capacity and thread-safety mode.</summary>
        /// <param name="buffer">Backing storage viewed as ulongs.</param>
        /// <param name="position">Initial position in bits.</param>
        /// <param name="capacity">Maximum writable bits.</param>
        /// <param name="threadSafe">Whether write operations synchronize across threads.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, long position, long capacity, bool threadSafe) {
            ThrowIfInvalidCapacity(capacity, (long)buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
            ThreadSafe = threadSafe;
        }
#endif
    }
}
