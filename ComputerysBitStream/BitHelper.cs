using System;
using System.Runtime.CompilerServices;

namespace ComputerysBitStream {
    internal static class BitHelper {
        private const int BitsPerByte = 8;
    
        public const int BoolSize = 1;

        public const int ByteSize = sizeof(byte) * BitsPerByte;
        public const int OneLessThanByteSize = ByteSize - 1;
        
        public const int SByteSize = sizeof(sbyte) * BitsPerByte;
        public const int CharSize = sizeof(char) * BitsPerByte;
        public const int ShortSize = sizeof(short) * BitsPerByte;
        public const int UShortSize = sizeof(ushort) * BitsPerByte;
        public const int IntSize = sizeof(int) * BitsPerByte;
        public const int UIntSize = sizeof(uint) * BitsPerByte;
        public const int LongSize = sizeof(long) * BitsPerByte;
        public const int ULongSize = sizeof(ulong) * BitsPerByte;
        public const int FloatSize = sizeof(float) * BitsPerByte;
        public const int DoubleSize = sizeof(double) * BitsPerByte;
        public const int DecimalSize = sizeof(decimal) * BitsPerByte;
        
        private const int LogValue = 3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BitsToBytes(int bits) => (bits + OneLessThanByteSize) >> LogValue;
    }
}