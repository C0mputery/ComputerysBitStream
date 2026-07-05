#if BITSTREAM_SUPPORT_THREAD_SAFE
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using ComputerysBitStream.Helpers;
#if BITSTREAM_HOST_BIG_ENDIAN
using System.Buffers.Binary;
#endif
#if !NET5_0_OR_GREATER && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
using System.Runtime.InteropServices;
#endif

namespace ComputerysBitStream {
    public ref partial struct WriteContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ExchangeFullUlong(int index, ulong value) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            ulong storedValue = value;
#elif BITSTREAM_HOST_BIG_ENDIAN
            ulong storedValue = BinaryPrimitives.ReverseEndianness(value);
#endif

            ulong storedUlong = Volatile.Read(ref Buffer[index]);
            while (true) {
                ulong previousStoredUlong = CompareExchangeUlong(index, storedValue, storedUlong);
                if (previousStoredUlong == storedUlong) { return; }

                storedUlong = previousStoredUlong;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ExchangeFullUlongs(int destinationIndex, ReadOnlySpan<ulong> source, int ulongCount) {
            ReadOnlySpan<ulong> sourceSlice = source.Slice(0, ulongCount);
            for (int i = 0; i < ulongCount; i++) { ExchangeFullUlong(destinationIndex + i, sourceSlice[i]); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThreadSafeMergeUlong(int index, ulong value, int bitOffset, int bitCount) {
            if (bitOffset == 0 && bitCount == BitHelper.ULongSize) {
                ExchangeFullUlong(index, value);
                return;
            }

            ulong valueMask = bitCount == BitHelper.ULongSize ? ulong.MaxValue : (1UL << bitCount) - 1;
            value &= valueMask;
            ulong mask = valueMask << bitOffset;

            ulong storedUlong = Volatile.Read(ref Buffer[index]);
            while (true) {
#if !BITSTREAM_HOST_BIG_ENDIAN
                ulong loadedUlong = storedUlong;
                ulong mergedUlong = (loadedUlong & ~mask) | (value << bitOffset);
#elif BITSTREAM_HOST_BIG_ENDIAN
                ulong loadedUlong = BinaryPrimitives.ReverseEndianness(storedUlong);
                ulong mergedUlong = BinaryPrimitives.ReverseEndianness((loadedUlong & ~mask) | (value << bitOffset));
#endif

                ulong previousStoredUlong = CompareExchangeUlong(index, mergedUlong, storedUlong);
                if (previousStoredUlong == storedUlong) { return; }

                storedUlong = previousStoredUlong;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong CompareExchangeUlong(int index, ulong newStoredUlong, ulong expectedStoredUlong) {
            ref ulong bufferUlong = ref Buffer[index];
#if NET5_0_OR_GREATER
            return Interlocked.CompareExchange(ref bufferUlong, newStoredUlong, expectedStoredUlong);
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ref long bufferUlongAsLong = ref Unsafe.As<ulong, long>(ref bufferUlong);
            return (ulong)Interlocked.CompareExchange(ref bufferUlongAsLong, (long)newStoredUlong, (long)expectedStoredUlong);
#elif !NET5_0_OR_GREATER && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ref long bufferUlongAsLong = ref MemoryMarshal.Cast<ulong, long>(Buffer)[index];
            return (ulong)Interlocked.CompareExchange(ref bufferUlongAsLong, (long)newStoredUlong, (long)expectedStoredUlong);
#endif
        }
    }
}

#endif
