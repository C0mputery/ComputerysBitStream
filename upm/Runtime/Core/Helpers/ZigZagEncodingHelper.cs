using System.Runtime.CompilerServices;

namespace ComputerysBitStream.Helpers {
    internal static class ZigZagEncodingHelper {
        private const int IntSignShift = sizeof(int) * 8 - 1;
        private const int LongSignShift = sizeof(long) * 8 - 1;
        private const int ShortSignShift = sizeof(short) * 8 - 1;
        private const int SByteSignShift = sizeof(sbyte) * 8 - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint EncodeInt(int value) => (uint)((value << 1) ^ (value >> IntSignShift));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DecodeInt(uint value) => (int)((value >> 1) ^ (uint)(-(int)(value & 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong EncodeLong(long value) => (ulong)((value << 1) ^ (value >> LongSignShift));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long DecodeLong(ulong value) => (long)((value >> 1) ^ (ulong)(-(long)(value & 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort EncodeShort(short value) => (ushort)((value << 1) ^ (value >> ShortSignShift));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short DecodeShort(ushort value) => (short)((value >> 1) ^ (ushort)(-(short)(value & 1)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte EncodeSByte(sbyte value) => (byte)((value << 1) ^ (value >> SByteSignShift));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte DecodeSByte(byte value) => (sbyte)((value >> 1) ^ (byte)(-(sbyte)(value & 1)));
    }
}
