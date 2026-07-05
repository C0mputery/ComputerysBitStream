using System;

namespace ComputerysBitStream {
    public sealed class BitStreamReadException : Exception {
        public string Type { get; }
        public long AvailableBits { get; }
        public long Position { get; }

        public BitStreamReadException(string type, long availableBits, long position) : base($"Failed to read {type} at position {position}. Available bits: {availableBits}.") {
            Type = type;
            AvailableBits = availableBits;
            Position = position;
        }
    }
}
