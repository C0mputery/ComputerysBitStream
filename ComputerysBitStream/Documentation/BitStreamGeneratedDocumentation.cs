using System.ComponentModel;

namespace ComputerysBitStream {
    /// <summary>Shared XML documentation for source-generated read and write extension methods.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class BitStreamGeneratedDocumentation {
        /// <summary>Writes one value after checking <see cref="WriteContext.GetRemainingCapacity"/>.</summary>
        /// <remarks>
        /// <para>Throws <see cref="InsufficientWriteCapacityException"/> when the buffer is too short.</para>
        /// <para>Quantized overloads also take <c>min</c>, <c>max</c>, and <c>bitCount</c>. Use the same three arguments on read.</para>
        /// </remarks>
        public static void WriteValue() { }

        /// <summary>Writes a length-prefixed sequence: a fixed-size <c>int</c> count, then each element.</summary>
        /// <remarks>Throws <see cref="InsufficientWriteCapacityException"/> when the buffer cannot hold the count plus all elements.</remarks>
        public static void WriteValuesWithLength() { }

        /// <summary>Writes each element with no length prefix.</summary>
        /// <remarks>The matching read method takes an explicit <c>count</c> argument. Throws <see cref="InsufficientWriteCapacityException"/> when the buffer is too short.</remarks>
        public static void WriteValuesWithoutLength() { }

        /// <summary>Reads one value and advances <see cref="ReadContext.Position"/>.</summary>
        /// <remarks>Throws <see cref="InsufficientReadSpaceException"/> or <see cref="BitStreamReadException"/> when the read fails.</remarks>
        public static void ReadValue() { }

        /// <summary>Reads one value without advancing <see cref="ReadContext.Position"/>.</summary>
        /// <remarks>Throws <see cref="InsufficientReadSpaceException"/> or <see cref="BitStreamReadException"/> when the read fails.</remarks>
        public static void PeekValue() { }

        /// <summary>Attempts to read one value. Returns <c>false</c> when the buffer does not hold a complete encoded value.</summary>
        /// <remarks>Does not throw for short buffers. Advances <see cref="ReadContext.Position"/> only on success.</remarks>
        public static void TryReadValue() { }

        /// <summary>Attempts to read one value without advancing position. Returns <c>false</c> when the buffer is too short.</summary>
        public static void TryPeekValue() { }

        /// <summary>Reads a length-prefixed array: reads a fixed-size <c>int</c> count from the stream, then that many elements.</summary>
        /// <remarks>Throws <see cref="BitStreamReadException"/> when the length prefix or any element cannot be read.</remarks>
        public static void ReadValuesWithLength() { }

        /// <summary>Peeks a length-prefixed array without advancing position past the peeked elements.</summary>
        /// <remarks>Throws <see cref="BitStreamReadException"/> when the length prefix or any element cannot be read.</remarks>
        public static void PeekValuesWithLength() { }

        /// <summary>Attempts to read a length-prefixed array. Returns <c>false</c> when the count or any element is incomplete.</summary>
        public static void TryReadValuesWithLength() { }

        /// <summary>Attempts to peek a length-prefixed array. Returns <c>false</c> when the count or any element is incomplete.</summary>
        public static void TryPeekValuesWithLength() { }

        /// <summary>Reads a caller-supplied number of elements. The count is not stored in the stream.</summary>
        /// <remarks>Throws <see cref="BitStreamReadException"/> when any element cannot be read.</remarks>
        public static void ReadValuesWithCount() { }

        /// <summary>Peeks a caller-supplied number of elements without leaving the read position past them.</summary>
        public static void PeekValuesWithCount() { }

        /// <summary>Attempts to read a caller-supplied number of elements. Returns <c>false</c> when any element is incomplete.</summary>
        public static void TryReadValuesWithCount() { }

        /// <summary>Attempts to peek a caller-supplied number of elements. Returns <c>false</c> when any element is incomplete.</summary>
        public static void TryPeekValuesWithCount() { }

        /// <summary>Reads a length-prefixed sequence into a span. The span must be at least as long as the encoded count.</summary>
        public static void ReadValuesIntoSpanWithLength() { }

        /// <summary>Peeks a length-prefixed sequence into a span.</summary>
        public static void PeekValuesIntoSpanWithLength() { }

        /// <summary>Attempts to read a length-prefixed sequence into a span.</summary>
        public static void TryReadValuesIntoSpanWithLength() { }

        /// <summary>Attempts to peek a length-prefixed sequence into a span.</summary>
        public static void TryPeekValuesIntoSpanWithLength() { }

        /// <summary>Reads a caller-supplied number of elements into a span.</summary>
        public static void ReadValuesIntoSpanWithCount() { }

        /// <summary>Peeks a caller-supplied number of elements into a span.</summary>
        public static void PeekValuesIntoSpanWithCount() { }

        /// <summary>Attempts to read a caller-supplied number of elements into a span.</summary>
        public static void TryReadValuesIntoSpanWithCount() { }

        /// <summary>Attempts to peek a caller-supplied number of elements into a span.</summary>
        public static void TryPeekValuesIntoSpanWithCount() { }

        /// <summary>Returns the encoded size in bits for one value. For variable-length types, the size depends on the value.</summary>
        public static void GetSizeInBits() { }
    }
}
