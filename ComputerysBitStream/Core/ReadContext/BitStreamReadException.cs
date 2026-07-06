using System;

namespace ComputerysBitStream {
    /// <summary>Thrown when a checked read operation fails after a <c>Try*</c> wrapper returns <c>false</c>.</summary>
    public sealed class BitStreamReadException : Exception {
        /// <summary>Type name from the failing read call.</summary>
        public string Type { get; }
        /// <summary>Bits remaining at the failure position.</summary>
        public long AvailableBits { get; }
        /// <summary>Read position in bits when the failure was detected.</summary>
        public long Position { get; }

        /// <summary>Creates an exception describing a failed read of <paramref name="type"/>.</summary>
        /// <param name="type">Label passed to the read helper.</param>
        /// <param name="availableBits">Bits remaining in the context.</param>
        /// <param name="position">Read position in bits.</param>
        public BitStreamReadException(string type, long availableBits, long position) : base($"TryRead returned false for {type} at position {position}. Available bits: {availableBits}.") {
            Type = type;
            AvailableBits = availableBits;
            Position = position;
        }
    }
}
