using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    [BitStreamPrimitiveContext]
    public ref partial struct WriteContext {
        public readonly Span<ulong> Buffer;
        public long Position;
        public readonly long Capacity;
#if BITSTREAM_SUPPORT_THREAD_SAFE
        public bool ThreadSafe; // TODO, it would be better if rather than a toggle, there was a thread safety write context object so that there was no runtime cost.
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer) {
            Buffer = buffer;
            Position = 0;
            Capacity = buffer.Length * BitHelper.ULongSize;
#if BITSTREAM_SUPPORT_THREAD_SAFE
            ThreadSafe = false;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, long position) {
            long capacity = buffer.Length * BitHelper.ULongSize;
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
#if BITSTREAM_SUPPORT_THREAD_SAFE
            ThreadSafe = false;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, long position, long capacity) {
            ThrowIfInvalidCapacity(capacity, buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
#if BITSTREAM_SUPPORT_THREAD_SAFE
            ThreadSafe = false;
#endif
        }

#if BITSTREAM_SUPPORT_THREAD_SAFE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WriteContext(Span<ulong> buffer, long position, long capacity, bool threadSafe) {
            ThrowIfInvalidCapacity(capacity, buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
            ThreadSafe = threadSafe;
        }
#endif
    }
}
