using System.ComponentModel;

namespace ComputerysBitStream {
    /// <summary>XML documentation for authoring custom <see cref="Attributes.BitStreamPrimitiveAttribute"/> serializers.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class BitStreamPrimitiveAuthorDocumentation {
        /// <summary>Steps to add a custom primitive serializer.</summary>
        /// <remarks>
        /// <para>1. Declare a <c>public static</c> class with <see cref="Attributes.BitStreamPrimitiveAttribute"/> and the companion attribute for the mode (<see cref="Attributes.BitStreamFixedSizePrimitiveAttribute"/>, <see cref="Attributes.BitStreamQuantizedPrimitiveAttribute"/>, or variable-length with no size attribute).</para>
        /// <para>2. Implement one <c>public static</c> method per <see cref="Attributes.BitStreamPrimitiveRole"/>, each marked with <see cref="Attributes.BitStreamPrimitiveMethodAttribute"/>. Signatures must match the role templates documented on this type; mismatches report <c>CBS010</c>.</para>
        /// <para>3. Call <c>*Primitive</c> methods on <see cref="WriteContext"/> and <see cref="ReadContext"/> inside the implementation. Those calls skip bounds checks; generated <c>Write*</c> / <c>Read*</c> wrappers add checks for callers.</para>
        /// <para>4. Register the class on a <see cref="Attributes.BitStreamSettingsAttribute"/> interface with <see cref="Attributes.BitStreamSerializerAttribute"/>.</para>
        /// <para>Fixed-size primitives require <see cref="Attributes.BitStreamFixedSizePrimitiveAttribute"/>. Quantized primitives require <see cref="Attributes.BitStreamQuantizedPrimitiveAttribute"/>. Variable-length primitives require <see cref="Attributes.BitStreamPrimitiveRole.Size"/> and <see cref="Attributes.BitStreamPrimitiveRole.TryRead"/>; other modes must not define those roles (<c>CBS013</c>, <c>CBS019</c>, <c>CBS033</c>, <c>CBS034</c>).</para>
        /// </remarks>
        public static void AuthoringOverview() { }

        /// <summary>Calling <c>*Primitive</c> APIs outside a primitive implementation context.</summary>
        /// <remarks>
        /// <para>May only be invoked from types marked with <see cref="Attributes.BitStreamPrimitiveAttribute"/> or <see cref="Attributes.BitStreamPrimitiveContextAttribute"/>. Other call sites get analyzer warning <c>CBS031</c>.</para>
        /// <para>Library callers should use generated <c>Write*</c>, <c>Read*</c>, <c>Peek*</c>, and <c>Try*</c> extension methods instead.</para>
        /// </remarks>
        public static void PrimitiveContextUsage() { }

        /// <summary>Quantized write/read roles append three parameters after the fixed-size shape.</summary>
        /// <remarks>Append <c>, T min, T max, int bitCount</c> to <see cref="Attributes.BitStreamPrimitiveRole.Write"/>, <see cref="Attributes.BitStreamPrimitiveRole.WriteSpan"/>, <see cref="Attributes.BitStreamPrimitiveRole.Peek"/>, <see cref="Attributes.BitStreamPrimitiveRole.Read"/>, array, and span roles. <see cref="Attributes.BitStreamPrimitiveRole.Size"/> and <see cref="Attributes.BitStreamPrimitiveRole.TryRead"/> keep the non-quantized signatures.</remarks>
        public static void QuantizedParameters() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.Write"/> implementation shape.</summary>
        /// <remarks><c>public static void MethodName(this ref WriteContext context, T value)</c></remarks>
        public static void RoleWrite() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.WriteSpan"/> implementation shape.</summary>
        /// <remarks><c>public static void MethodName(this ref WriteContext context, ReadOnlySpan&lt;T&gt; values)</c></remarks>
        public static void RoleWriteSpan() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.Peek"/> implementation shape.</summary>
        /// <remarks><c>public static T MethodName(this ref ReadContext context)</c></remarks>
        public static void RolePeek() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.Read"/> implementation shape.</summary>
        /// <remarks><c>public static T MethodName(this ref ReadContext context)</c></remarks>
        public static void RoleRead() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.TryRead"/> implementation shape. Required for variable-length primitives.</summary>
        /// <remarks><c>public static bool MethodName(this ref ReadContext context, out T value)</c>. Return <c>false</c> when the buffer does not hold a complete value; do not throw for short buffers.</remarks>
        public static void RoleTryRead() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.PeekArray"/> implementation shape.</summary>
        /// <remarks><c>public static T[] MethodName(this ref ReadContext context, int count)</c></remarks>
        public static void RolePeekArray() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.ReadArray"/> implementation shape.</summary>
        /// <remarks><c>public static T[] MethodName(this ref ReadContext context, int count)</c></remarks>
        public static void RoleReadArray() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.PeekSpan"/> implementation shape.</summary>
        /// <remarks><c>public static void MethodName(this ref ReadContext context, int count, Span&lt;T&gt; destination)</c></remarks>
        public static void RolePeekSpan() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.ReadSpan"/> implementation shape.</summary>
        /// <remarks><c>public static void MethodName(this ref ReadContext context, int count, Span&lt;T&gt; destination)</c></remarks>
        public static void RoleReadSpan() { }

        /// <summary><see cref="Attributes.BitStreamPrimitiveRole.Size"/> implementation shape. Required for variable-length primitives.</summary>
        /// <remarks><c>public static int MethodName(T value)</c>. Not an extension method. Returns the encoded bit length for one value.</remarks>
        public static void RoleSize() { }

        /// <summary>Built-in primitive extension classes in this library.</summary>
        /// <remarks>Types under <c>ComputerysBitStream.Primitives</c> are reference implementations of <see cref="AuthoringOverview"/>.</remarks>
        public static void BuiltInReferenceImplementations() { }
    }
}
