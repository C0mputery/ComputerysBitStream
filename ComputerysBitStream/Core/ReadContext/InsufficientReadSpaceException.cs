using System;

namespace ComputerysBitStream {
    public sealed class InsufficientReadSpaceException : Exception {
        public string Type { get; }
        public int RequiredBits { get; }
        public long AvailableBits { get; }
        public long Position { get; }

        public InsufficientReadSpaceException(string type, int requiredBits, long availableBits, long position) : base($"Insufficient space to read {type}. Required bits: {requiredBits}, Available bits: {availableBits}, Position: {position}.") {
            Type = type;
            RequiredBits = requiredBits;
            AvailableBits = availableBits;
            Position = position;
        }
    }
}
