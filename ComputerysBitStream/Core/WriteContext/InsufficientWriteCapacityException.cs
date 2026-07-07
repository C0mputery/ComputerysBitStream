using System;

namespace ComputerysBitStream {
    /// <summary>Thrown when a write needs more bits than <see cref="WriteContext.GetRemainingCapacity"/> provides.</summary>
    public sealed class InsufficientWriteCapacityException : Exception {
        /// <summary>Operation label from the failing write call.</summary>
        public string Operation { get; }

        /// <summary>Bits required for the operation.</summary>
        public int RequiredBits { get; }

        /// <summary>Bits remaining when the check ran.</summary>
        public long AvailableBits { get; }

        /// <summary>Write position in bits when the check ran.</summary>
        public long Position { get; }

        /// <summary>Creates an exception for a short write buffer.</summary>
        /// <param name="operation">Label passed to the write helper.</param>
        /// <param name="requiredBits">Bits required.</param>
        /// <param name="availableBits">Bits remaining.</param>
        /// <param name="position">Write position in bits.</param>
        public InsufficientWriteCapacityException(string operation, int requiredBits, long availableBits, long position) : base($"Insufficient capacity in write context for {operation}. Required bits: {requiredBits}, Available bits: {availableBits}, Position: {position}.") {
            Operation = operation;
            RequiredBits = requiredBits;
            AvailableBits = availableBits;
            Position = position;
        }
    }
}
