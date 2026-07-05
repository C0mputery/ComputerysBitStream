using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ComputerysBitStream.Helpers;
#if BITSTREAM_HOST_BIG_ENDIAN
using System.Buffers.Binary;
#endif

namespace ComputerysBitStream {
    public ref partial struct WriteContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MergeUlong(int index, ulong value, int bitOffset, int bitCount) {
#if BITSTREAM_SUPPORT_THREAD_SAFE
            if (ThreadSafe) {
                ThreadSafeMergeUlong(index, value, bitOffset, bitCount);
                return;
            }
#endif
            ulong valueMask = bitCount == BitHelper.ULongSize ? ulong.MaxValue : (1UL << bitCount) - 1;
            value &= valueMask;
            ulong mask = valueMask << bitOffset;
            ulong bufferUlong = LoadUlong(index);
            StoreUlong(index, (bufferUlong & ~mask) | (value << bitOffset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StoreUlongs(int destinationStartingUlongIndex, ReadOnlySpan<ulong> source, int ulongCount) {
#if BITSTREAM_SUPPORT_THREAD_SAFE
            if (ThreadSafe) {
                ExchangeFullUlongs(destinationStartingUlongIndex, source, ulongCount);
                return;
            }
#endif

#if !BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            source.Slice(0, ulongCount).CopyTo(Buffer.Slice(destinationStartingUlongIndex, ulongCount));
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER) && !BITSTREAM_HOST_BIG_ENDIAN
            Span<ulong> buffer = Buffer;
            ref byte destinationBytes = ref Unsafe.As<ulong, byte>(ref buffer[destinationStartingUlongIndex]);
            ref byte sourceBytes = ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(source));
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)(ulongCount * sizeof(ulong)));
#elif BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            source.Slice(0, ulongCount).CopyTo(Buffer.Slice(destinationStartingUlongIndex, ulongCount));
            Span<ulong> destinationSlice = Buffer.Slice(destinationStartingUlongIndex, ulongCount);
            for (int i = 0; i < destinationSlice.Length; i++) { destinationSlice[i] = BinaryPrimitives.ReverseEndianness(destinationSlice[i]); }
#elif BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Span<ulong> buffer = Buffer;
            ref byte destinationBytes = ref Unsafe.As<ulong, byte>(ref buffer[destinationStartingUlongIndex]);
            ref byte sourceBytes = ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(source));
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)(ulongCount * sizeof(ulong)));
            Span<ulong> destinationSlice = Buffer.Slice(destinationStartingUlongIndex, ulongCount);
            for (int i = 0; i < destinationSlice.Length; i++) { destinationSlice[i] = BinaryPrimitives.ReverseEndianness(destinationSlice[i]); }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly ulong LoadUlong(int index) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            return Buffer[index];
#elif BITSTREAM_HOST_BIG_ENDIAN
            return BinaryPrimitives.ReverseEndianness(Buffer[index]);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StoreUlong(int index, ulong value) {
#if BITSTREAM_SUPPORT_THREAD_SAFE
            if (ThreadSafe) {
                ExchangeFullUlong(index, value);
                return;
            }
#endif
#if !BITSTREAM_HOST_BIG_ENDIAN
            Buffer[index] = value;
#elif BITSTREAM_HOST_BIG_ENDIAN
            Buffer[index] = BinaryPrimitives.ReverseEndianness(value);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MergeUlongViaByteAlignedWrite(int byteIndex, ulong value, int byteCount) {
            Span<byte> bytes = stackalloc byte[8];

#if !BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            MemoryMarshal.Write(bytes, ref value);
#elif !BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(bytes), value);
#elif BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ulong storedValue = BinaryPrimitives.ReverseEndianness(value);
            MemoryMarshal.Write(bytes, ref storedValue);
#elif BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(bytes), BinaryPrimitives.ReverseEndianness(value));
#endif

#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            bytes.Slice(0, byteCount).CopyTo(MemoryMarshal.Cast<ulong, byte>(Buffer).Slice(byteIndex, byteCount));
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Span<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(Buffer);
            ref byte destinationBytes = ref bufferAsBytes[byteIndex];
            ref byte sourceBytes = ref MemoryMarshal.GetReference(bytes);
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)byteCount);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MergeUlongsViaByteAlignedWrite(int byteIndex, ReadOnlySpan<ulong> source, int byteCount) {
#if !BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<byte> sourceBytes = MemoryMarshal.Cast<ulong, byte>(source).Slice(0, byteCount);
            sourceBytes.CopyTo(MemoryMarshal.Cast<ulong, byte>(Buffer).Slice(byteIndex, byteCount));
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER) && !BITSTREAM_HOST_BIG_ENDIAN
            Span<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(Buffer);
            ref byte destinationBytes = ref bufferAsBytes[byteIndex];
            ref byte sourceBytes = ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(source));
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)byteCount);
#elif BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Span<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(Buffer);
            Span<byte> destination = bufferAsBytes.Slice(byteIndex, byteCount);
            Span<byte> storageUlongBytes = stackalloc byte[8];
            int sourceUlongCount = (byteCount + 7) >> 3;

            for (int i = 0; i < sourceUlongCount; i++) {
                ulong reversed = BinaryPrimitives.ReverseEndianness(source[i]);
                MemoryMarshal.Write(storageUlongBytes, ref reversed);
                int streamByteStart = i << 3;
                int copyLength = Math.Min(8, byteCount - streamByteStart);
                storageUlongBytes.Slice(0, copyLength).CopyTo(destination.Slice(streamByteStart, copyLength));
            }
#elif BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Span<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(Buffer);
            Span<byte> destination = bufferAsBytes.Slice(byteIndex, byteCount);
            Span<byte> storageUlongBytes = stackalloc byte[8];
            int sourceUlongCount = (byteCount + 7) >> 3;

            for (int i = 0; i < sourceUlongCount; i++) {
                ulong reversed = BinaryPrimitives.ReverseEndianness(source[i]);
                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(storageUlongBytes), reversed);
                int streamByteStart = i << 3;
                int copyLength = Math.Min(8, byteCount - streamByteStart);
                storageUlongBytes.Slice(0, copyLength).CopyTo(destination.Slice(streamByteStart, copyLength));
            }
#endif
        }
    }
}
