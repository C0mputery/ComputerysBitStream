using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    [BitStreamPrimitiveContext]
    public ref partial struct ReadContext {
        public readonly ReadOnlySpan<ulong> Buffer;
        public long Position;
        public long Capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer) {
            Buffer = buffer;
            Position = 0;
            Capacity = buffer.Length * BitHelper.ULongSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer, long position) {
            long capacity = buffer.Length * BitHelper.ULongSize;
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadContext(ReadOnlySpan<ulong> buffer, long position, long capacity) {
            ThrowIfInvalidCapacity(capacity, buffer.Length * BitHelper.ULongSize);
            ThrowIfInvalidPosition(position, capacity);
            Buffer = buffer;
            Position = position;
            Capacity = capacity;
        }
    }
}
