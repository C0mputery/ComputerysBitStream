using System;
using System.Runtime.CompilerServices;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Helpers;

namespace ComputerysBitStream {
    public ref partial struct ReadContext {
        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool PeekBitPrimitive() {
            int ulongIndex = (int)(Position / BitHelper.ULongSize);
            int bitOffset = (int)(Position % BitHelper.ULongSize);
            return (LoadUlong(ulongIndex) & (1UL << bitOffset)) != 0;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBitPrimitive() {
            bool value = PeekBitPrimitive();
            Position++;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong PeekBitsPrimitive(int bitCount) {
            int ulongBitOffset = (int)(Position % BitHelper.ULongSize);
            int bitsAvailableInCurrent = BitHelper.ULongSize - ulongBitOffset;
            bool fitsInSingleUlong = bitCount <= bitsAvailableInCurrent;

            bool ulongAligned = ulongBitOffset == 0;
            if (ulongAligned && bitCount == BitHelper.ULongSize) {
                return LoadUlong((int)(Position / BitHelper.ULongSize));
            }

            if (!fitsInSingleUlong && (ulongAligned || Position % BitHelper.ByteSize == 0) && bitCount % BitHelper.ByteSize == 0) {
                int byteCount = bitCount / BitHelper.ByteSize;
                int byteIndex = (int)(Position / BitHelper.ByteSize);
                return LoadUlongViaByteAlignedRead(byteIndex, byteCount);
            }

            int ulongIndex = (int)(Position / BitHelper.ULongSize);
            ulong valueMask = bitCount == BitHelper.ULongSize ? ulong.MaxValue : (1UL << bitCount) - 1;
            ulong result = (LoadUlong(ulongIndex) >> ulongBitOffset);

            if (bitCount > bitsAvailableInCurrent) { result |= (LoadUlong(ulongIndex + 1) << bitsAvailableInCurrent); }

            return result & valueMask;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadBitsPrimitive(int bitCount) {
            ulong value = PeekBitsPrimitive(bitCount);
            Position += bitCount;
            return value;
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void PeekBitsPrimitive(int bitCount, Span<ulong> destination) {
            int ulongBitOffset = (int)(Position % BitHelper.ULongSize);
            int fullUlongs = bitCount / BitHelper.ULongSize;
            int remainingBits = bitCount % BitHelper.ULongSize;
            int ulongIndex = (int)(Position / BitHelper.ULongSize);

            if (ulongBitOffset == 0) {
                if (fullUlongs > 0) {
                    LoadUlongs(ulongIndex, destination, fullUlongs);
                    ulongIndex += fullUlongs;
                }

                if (remainingBits > 0) {
                    ulong valueMask = (1UL << remainingBits) - 1;
                    destination[fullUlongs] = LoadUlong(ulongIndex) & valueMask;
                }

                return;
            }

            if (ulongBitOffset % BitHelper.ByteSize == 0) {
                int byteCount = bitCount / BitHelper.ByteSize;
                if (byteCount > 0) {
                    LoadUlongsViaByteAlignedRead((int)(Position / BitHelper.ByteSize), destination, byteCount);
                }

                int remainingByteBits = bitCount % BitHelper.ByteSize;
                if (remainingByteBits > 0) {
                    long remainingPosition = Position + byteCount * BitHelper.ByteSize;
                    int remainingUlongIndex = (int)(remainingPosition / BitHelper.ULongSize);
                    int remainingBitOffset = (int)(remainingPosition % BitHelper.ULongSize);
                    ulong valueMask = (1UL << remainingByteBits) - 1;
                    ulong result = (LoadUlong(remainingUlongIndex) >> remainingBitOffset) & valueMask;

                    int bitsAvailableInCurrent = BitHelper.ULongSize - remainingBitOffset;
                    if (remainingByteBits > bitsAvailableInCurrent) {
                        result |= (LoadUlong(remainingUlongIndex + 1) << bitsAvailableInCurrent) & valueMask;
                    }

                    int destUlongIndex = byteCount / 8;
                    int destBitOffset = (byteCount % 8) * BitHelper.ByteSize;
                    ulong lowerMask = destBitOffset == 0 ? 0 : (1UL << destBitOffset) - 1;
                    destination[destUlongIndex] = (destination[destUlongIndex] & lowerMask) | (result << destBitOffset);
                }

                return;
            }

            int inverseBitOffset = BitHelper.ULongSize - ulongBitOffset;
            for (int i = 0; i < fullUlongs; i++) {
                destination[i] = (LoadUlong(ulongIndex) >> ulongBitOffset) | (LoadUlong(ulongIndex + 1) << inverseBitOffset);
                ulongIndex++;
            }

            if (remainingBits > 0) {
                ulong valueMask = (1UL << remainingBits) - 1;
                ulong result = (LoadUlong(ulongIndex) >> ulongBitOffset) & valueMask;

                int bitsAvailableInCurrent = BitHelper.ULongSize - ulongBitOffset;
                if (remainingBits > bitsAvailableInCurrent) {
                    result |= (LoadUlong(ulongIndex + 1) << bitsAvailableInCurrent) & valueMask;
                }

                destination[fullUlongs] = result;
            }
        }

        /// <inheritdoc cref="BitStreamPrimitiveDocumentation.Usage"/>
        [BitStreamRestrictedPrimitiveMethod]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBitsPrimitive(int bitCount, Span<ulong> destination) {
            PeekBitsPrimitive(bitCount, destination);
            Position += bitCount;
        }
    }
}
