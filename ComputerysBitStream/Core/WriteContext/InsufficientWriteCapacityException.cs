using System;

namespace ComputerysBitStream {
    public sealed class InsufficientWriteCapacityException : Exception {
        public string Operation { get; }
        public int RequiredBits { get; }
        public long AvailableBits { get; }
        public long Position { get; }

        public InsufficientWriteCapacityException(string operation, int requiredBits, long availableBits, long position) : base($"Insufficient capacity in write context for {operation}. Required bits: {requiredBits}, Available bits: {availableBits}, Position: {position}.") {
            Operation = operation;
            RequiredBits = requiredBits;
            AvailableBits = availableBits;
            Position = position;
        }
    }
}
