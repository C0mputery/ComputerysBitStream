using System.Runtime.CompilerServices;

namespace ComputerysBitStream.Helpers {
    internal static class QuantizedEncodingHelper {
        public const int MinimumBits = 1;
        public const int FullRangeBits = 64;
        public const float RoundBiasFloat = 0.5f;
        public const double RoundBiasDouble = 0.5d;
        public const decimal RoundBiasDecimal = 0.5m;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong MaxQuantizedValue(int bitCount) => bitCount == FullRangeBits ? ulong.MaxValue : (1UL << bitCount) - 1;
    }
}
