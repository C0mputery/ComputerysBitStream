using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if BITSTREAM_HOST_BIG_ENDIAN
using System.Buffers.Binary;
#endif

namespace ComputerysBitStream {
    public ref partial struct ReadContext {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly ulong LoadUlong(int index) {
#if !BITSTREAM_HOST_BIG_ENDIAN
            return Buffer[index];
#elif BITSTREAM_HOST_BIG_ENDIAN
            return BinaryPrimitives.ReverseEndianness(Buffer[index]);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void LoadUlongs(int sourceIndex, Span<ulong> destination, int count) {
#if !BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Buffer.Slice(sourceIndex, count).CopyTo(destination.Slice(0, count));
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER) && !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<ulong> buffer = Buffer;
            ref byte destinationBytes = ref Unsafe.As<ulong, byte>(ref destination[0]);
            ref ulong sourceUlong = ref Unsafe.Add(ref MemoryMarshal.GetReference(buffer), sourceIndex);
            ref byte sourceBytes = ref Unsafe.As<ulong, byte>(ref sourceUlong);
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)(count * sizeof(ulong)));
#elif BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            Buffer.Slice(sourceIndex, count).CopyTo(destination.Slice(0, count));
            Span<ulong> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < destinationSlice.Length; i++) { destinationSlice[i] = BinaryPrimitives.ReverseEndianness(destinationSlice[i]); }
#elif BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<ulong> buffer = Buffer;
            ref byte destinationBytes = ref Unsafe.As<ulong, byte>(ref destination[0]);
            ref ulong sourceUlong = ref Unsafe.Add(ref MemoryMarshal.GetReference(buffer), sourceIndex);
            ref byte sourceBytes = ref Unsafe.As<ulong, byte>(ref sourceUlong);
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)(count * sizeof(ulong)));
            Span<ulong> destinationSlice = destination.Slice(0, count);
            for (int i = 0; i < destinationSlice.Length; i++) { destinationSlice[i] = BinaryPrimitives.ReverseEndianness(destinationSlice[i]); }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly ulong LoadUlongViaByteAlignedRead(int byteIndex, int byteCount) {
            Span<byte> bytes = stackalloc byte[8];
            ReadOnlySpan<ulong> buffer = Buffer;

#if !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(buffer);
            bufferAsBytes.Slice(byteIndex, byteCount).CopyTo(bytes.Slice(0, byteCount));
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(buffer);
            ref byte destinationBytes = ref MemoryMarshal.GetReference(bytes);
            ref byte sourceBytes = ref Unsafe.Add(ref MemoryMarshal.GetReference(bufferAsBytes), byteIndex);
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)byteCount);
#endif

#if !BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return MemoryMarshal.Read<ulong>(bytes);
#elif !BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(bytes));
#elif BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<ulong>(bytes));
#elif BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            return BinaryPrimitives.ReverseEndianness(Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(bytes)));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void LoadUlongsViaByteAlignedRead(int byteIndex, Span<ulong> destination, int byteCount) {
            ReadOnlySpan<ulong> buffer = Buffer;

#if !BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(buffer);
            bufferAsBytes.Slice(byteIndex, byteCount).CopyTo(MemoryMarshal.Cast<ulong, byte>(destination).Slice(0, byteCount));
#elif (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER) && !BITSTREAM_HOST_BIG_ENDIAN
            ReadOnlySpan<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(buffer);
            ref byte destinationBytes = ref Unsafe.As<ulong, byte>(ref MemoryMarshal.GetReference(destination));
            ref byte sourceBytes = ref Unsafe.Add(ref MemoryMarshal.GetReference(bufferAsBytes), byteIndex);
            Unsafe.CopyBlockUnaligned(ref destinationBytes, ref sourceBytes, (uint)byteCount);
#elif BITSTREAM_HOST_BIG_ENDIAN && !(BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(buffer);
            ReadOnlySpan<byte> source = bufferAsBytes.Slice(byteIndex, byteCount);
            Span<byte> destinationBytes = MemoryMarshal.Cast<ulong, byte>(destination).Slice(0, byteCount);
            Span<byte> storageUlongBytes = stackalloc byte[8];
            int destinationUlongCount = (byteCount + 7) >> 3;

            for (int i = 0; i < destinationUlongCount; i++) {
                int streamByteStart = i << 3;
                int copyLength = Math.Min(8, byteCount - streamByteStart);
                source.Slice(streamByteStart, copyLength).CopyTo(storageUlongBytes.Slice(0, copyLength));
                ulong reversed = BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<ulong>(storageUlongBytes));
                MemoryMarshal.Write(storageUlongBytes, ref reversed);
                storageUlongBytes.Slice(0, copyLength).CopyTo(destinationBytes.Slice(streamByteStart, copyLength));
            }
#elif BITSTREAM_HOST_BIG_ENDIAN && (BITSTREAM_INCLUDES_SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE || NET7_0_OR_GREATER)
            ReadOnlySpan<byte> bufferAsBytes = MemoryMarshal.Cast<ulong, byte>(buffer);
            ReadOnlySpan<byte> source = bufferAsBytes.Slice(byteIndex, byteCount);
            Span<byte> destinationBytes = MemoryMarshal.Cast<ulong, byte>(destination).Slice(0, byteCount);
            Span<byte> storageUlongBytes = stackalloc byte[8];
            int destinationUlongCount = (byteCount + 7) >> 3;

            for (int i = 0; i < destinationUlongCount; i++) {
                int streamByteStart = i << 3;
                int copyLength = Math.Min(8, byteCount - streamByteStart);
                source.Slice(streamByteStart, copyLength).CopyTo(storageUlongBytes.Slice(0, copyLength));
                ulong reversed = BinaryPrimitives.ReverseEndianness(Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(storageUlongBytes)));
                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(storageUlongBytes), reversed);
                storageUlongBytes.Slice(0, copyLength).CopyTo(destinationBytes.Slice(streamByteStart, copyLength));
            }
#endif
        }
    }
}
