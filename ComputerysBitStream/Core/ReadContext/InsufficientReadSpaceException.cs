using System;

namespace ComputerysBitStream {
    /// <summary>Thrown when a read needs more bits than <see cref="ReadContext.GetRemainingCapacity"/> provides.</summary>
    public sealed class InsufficientReadSpaceException : Exception {
        /// <summary>Type name from the failing read call.</summary>
        public string Type { get; }
        /// <summary>Bits required for the operation.</summary>
        public int RequiredBits { get; }
        /// <summary>Bits remaining when the check ran.</summary>
        public long AvailableBits { get; }
        /// <summary>Read position in bits when the check ran.</summary>
        public long Position { get; }

        /// <summary>Creates an exception for a short read buffer.</summary>
        /// <param name="type">Label passed to the read helper.</param>
        /// <param name="requiredBits">Bits required.</param>
        /// <param name="availableBits">Bits remaining.</param>
        /// <param name="position">Read position in bits.</param>
        public InsufficientReadSpaceException(string type, int requiredBits, long availableBits, long position) : base($"Insufficient space to read {type}. Required bits: {requiredBits}, Available bits: {availableBits}, Position: {position}.") {
            Type = type;
            RequiredBits = requiredBits;
            AvailableBits = availableBits;
            Position = position;
        }
    }
}
