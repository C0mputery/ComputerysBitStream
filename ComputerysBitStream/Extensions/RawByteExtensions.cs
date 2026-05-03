using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputerysBitStream {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [BitStreamRawType(typeof(byte), BitHelper.ByteSize)]
    public static class RawByteExtensions {
        private const int NumberOfValuesInUlong = BitHelper.ULongSize / BitHelper.ByteSize;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong AsBits(byte value) => value;
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte FromBits(ulong value) => (byte)value;
    
        [BitStreamRawMethod(BitStreamRawRole.Write)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByteRaw(this ref WriteContext context, byte value) { context.WriteBitsRaw(AsBits(value), BitHelper.ByteSize); }
    
        [BitStreamRawMethod(BitStreamRawRole.WriteSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBytesRaw(this ref WriteContext context, ReadOnlySpan<byte> values) {
            ReadOnlySpan<ulong> ulongs = MemoryMarshal.Cast<byte, ulong>(values);
            int totalUlongs = ulongs.Length;
            context.WriteBitsRaw(ulongs, totalUlongs * BitHelper.ULongSize);

            int remainingBytes = values.Length % NumberOfValuesInUlong;
            if (remainingBytes != 0) {
                ulong lastPacked = 0;
                for (int i = 0; i < remainingBytes; i++) {
                    lastPacked |= (AsBits(values[values.Length - remainingBytes + i])) << (i * BitHelper.ByteSize);
                }
                context.WriteBitsRaw(lastPacked, remainingBytes * BitHelper.ByteSize);
            }
        }

        [BitStreamRawMethod(BitStreamRawRole.Peek)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte PeekByteRaw(this ref ReadContext context) { return FromBits(context.PeekBitsRaw(BitHelper.ByteSize)); }

        [BitStreamRawMethod(BitStreamRawRole.Read)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadByteRaw(this ref ReadContext context) { return FromBits(context.ReadBitsRaw(BitHelper.ByteSize)); }

        [BitStreamRawMethod(BitStreamRawRole.PeekArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] PeekByteArrayRaw(this ref ReadContext context, int count) {
            byte[] result = new byte[count];
            Span<byte> span = result.AsSpan();
            context.PeekByteSpanRaw(count, span);
            return result;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadArray)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadByteArrayRaw(this ref ReadContext context, int count) {
            byte[] result = new byte[count];
            Span<byte> span = result.AsSpan();
            context.ReadByteSpanRaw(count, span);
            return result;
        }
    
        [BitStreamRawMethod(BitStreamRawRole.PeekSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PeekByteSpanRaw(this ref ReadContext context, int count, Span<byte> destination) {
            int originalPosition = context.Position;
            context.ReadByteSpanRaw(count, destination);
            context.Position = originalPosition;
        }

        [BitStreamRawMethod(BitStreamRawRole.ReadSpan)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadByteSpanRaw(this ref ReadContext context, int count, Span<byte> destination) {
            Span<byte> targetSpan = destination.Slice(0, count);
            Span<ulong> ulongs = MemoryMarshal.Cast<byte, ulong>(targetSpan);
            int totalUlongs = ulongs.Length;

            context.ReadBitsRaw(totalUlongs * BitHelper.ULongSize, ulongs);

            int remainingBytes = count % NumberOfValuesInUlong;
            if (remainingBytes != 0) {
                ulong lastPacked = context.ReadBitsRaw(remainingBytes * BitHelper.ByteSize);
                for (int i = 0; i < remainingBytes; i++) {
                    destination[count - remainingBytes + i] = FromBits(lastPacked >> (i * BitHelper.ByteSize));
                }
            }
        }
    }
}
