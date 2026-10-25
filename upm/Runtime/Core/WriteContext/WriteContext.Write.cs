using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    public ref partial struct WriteContext {
        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBitPrimitive(bool bit) {
            int ulongIndex = (int)(Position / BitHelper.ULongSize);
            int currentBitInUlong = (int)(Position % BitHelper.ULongSize);
            ulong bitValue = bit ? 1UL : 0UL;
            MergeUlong(ulongIndex, bitValue, currentBitInUlong, 1);

            Position++;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBitsPrimitive(ulong value, int bitCount) {
            int ulongBitOffset = (int)(Position % BitHelper.ULongSize);
            int bitsUsedInCurrent = BitHelper.ULongSize - ulongBitOffset;
            bool fitsInSingleUlong = bitCount <= bitsUsedInCurrent;

            bool ulongAligned = ulongBitOffset == 0;
            if (ulongAligned && bitCount == BitHelper.ULongSize) {
                StoreUlong((int)(Position / BitHelper.ULongSize), value);
                Position += BitHelper.ULongSize;
                return;
            }

#if BITSTREAM_SUPPORT_THREAD_SAFE
            if (!ThreadSafe) {
#endif
            if (!fitsInSingleUlong && (ulongAligned || Position % BitHelper.ByteSize == 0) && bitCount % BitHelper.ByteSize == 0) {
                int byteCount = bitCount / BitHelper.ByteSize;
                int byteIndex = (int)(Position / BitHelper.ByteSize);
                MergeUlongViaByteAlignedWrite(byteIndex, value, byteCount);
                Position += bitCount;
                return;
            }
#if BITSTREAM_SUPPORT_THREAD_SAFE
            }
#endif

            int ulongIndex = (int)(Position / BitHelper.ULongSize);
            int firstBitCount = bitCount <= bitsUsedInCurrent ? bitCount : bitsUsedInCurrent;
            MergeUlong(ulongIndex, value, ulongBitOffset, firstBitCount);
            if (bitCount > bitsUsedInCurrent) {
                int bitsRemaining = bitCount - bitsUsedInCurrent;
                MergeUlong(ulongIndex + 1, value >> bitsUsedInCurrent, 0, bitsRemaining);
            }

            Position += bitCount;
        }

        /// <inheritdoc cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBitsPrimitive(ReadOnlySpan<ulong> source, int bitCount) {
            int ulongBitOffset = (int)(Position % BitHelper.ULongSize);
            int fullUlongs = bitCount / BitHelper.ULongSize;
            int remainingBits = bitCount % BitHelper.ULongSize;
            int ulongIndex = (int)(Position / BitHelper.ULongSize);

            if (ulongBitOffset == 0) {
                StoreUlongs(ulongIndex, source, fullUlongs);
                Position += BitHelper.ULongSize * fullUlongs;
                if (remainingBits == 0) { return; }

                MergeUlong(ulongIndex + fullUlongs, source[fullUlongs], 0, remainingBits);
                Position += remainingBits;

                return;
            }

#if BITSTREAM_SUPPORT_THREAD_SAFE
            if (!ThreadSafe) {
#endif
            if (ulongBitOffset % BitHelper.ByteSize == 0) {
                int byteCount = bitCount / BitHelper.ByteSize;
                if (byteCount > 0) {
                    MergeUlongsViaByteAlignedWrite((int)(Position / BitHelper.ByteSize), source, byteCount);
                    Position += byteCount * BitHelper.ByteSize;
                }

                int remainingByteBits = bitCount % BitHelper.ByteSize;
                if (remainingByteBits > 0) {
                    ulong value = source[byteCount / 8] >> ((byteCount % 8) * BitHelper.ByteSize);
                    MergeUlong((int)(Position / BitHelper.ULongSize), value, (int)(Position % BitHelper.ULongSize), remainingByteBits);
                    Position += remainingByteBits;
                }

                return;
            }
#if BITSTREAM_SUPPORT_THREAD_SAFE
            }
#endif

            int inverseBitOffset = BitHelper.ULongSize - ulongBitOffset;
            for (int i = 0; i < fullUlongs; i++) {
                ulong value = source[i];
                MergeUlong(ulongIndex, value, ulongBitOffset, inverseBitOffset);
                MergeUlong(ulongIndex + 1, value >> inverseBitOffset, 0, ulongBitOffset);
                ulongIndex++;
            }

            Position += fullUlongs * BitHelper.ULongSize;

            if (remainingBits > 0) {
                ulong value = source[fullUlongs];
                if (remainingBits <= inverseBitOffset) {
                    MergeUlong(ulongIndex, value, ulongBitOffset, remainingBits);
                }
                else {
                    MergeUlong(ulongIndex, value, ulongBitOffset, inverseBitOffset);
                    MergeUlong(ulongIndex + 1, value >> inverseBitOffset, 0, remainingBits - inverseBitOffset);
                }

                Position += remainingBits;
            }
        }
    }
}
