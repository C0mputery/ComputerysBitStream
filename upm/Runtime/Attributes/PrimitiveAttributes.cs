#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    /// <summary>Marks a static class as a custom primitive serializer. Methods are tagged with <see cref="BitStreamPrimitiveMethodAttribute"/>.</summary>
    /// <remarks>
    /// <para>See <see cref="BitStreamPrimitiveAuthorDocumentation.AuthoringOverview"/> for the full workflow and <see cref="BitStreamPrimitiveAuthorDocumentation.BuiltInReferenceImplementations"/> for examples in this assembly.</para>
    /// <para>The class must be <c>public static</c> (<c>CBS009</c>, <c>CBS020</c>). Pair the mode with <see cref="BitStreamFixedSizePrimitiveAttribute"/> or <see cref="BitStreamQuantizedPrimitiveAttribute"/> as required (<c>CBS015</c>).</para>
    /// <para>Register the extension class on a <see cref="BitStreamSettingsAttribute"/> interface via <see cref="BitStreamSerializerAttribute"/>. Duplicate definitions report <c>CBS002</c>; two settings entries for the same target type and <see cref="PrimitiveSerializationMode"/> report <c>CBS055</c>; a struct that reuses a primitive alias reports <c>CBS039</c>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamPrimitiveAttribute : Attribute {
        /// <summary>Uses the target type name as the generated method alias.</summary>
        /// <param name="target">CLR type this primitive serializes.</param>
        /// <param name="serializationMode">Fixed-size, quantized, or variable-length encoding.</param>
        /// <param name="settings">Optional settings interfaces that must include this primitive.</param>
        public BitStreamPrimitiveAttribute(Type target, PrimitiveSerializationMode serializationMode, params Type[] settings) { }

        /// <summary>Uses a custom generated method alias.</summary>
        /// <param name="target">CLR type this primitive serializes.</param>
        /// <param name="alias">Prefix for generated <c>Write*</c> / <c>Read*</c> methods.</param>
        /// <param name="serializationMode">Fixed-size, quantized, or variable-length encoding.</param>
        /// <param name="settings">Optional settings interfaces that must include this primitive.</param>
        public BitStreamPrimitiveAttribute(Type target, string alias, PrimitiveSerializationMode serializationMode, params Type[] settings) { }
    }

    /// <summary>Encoding mode for a <see cref="BitStreamPrimitiveAttribute"/> class.</summary>
    public enum PrimitiveSerializationMode : int {
        /// <summary>Every value uses a fixed number of bits. Requires <see cref="BitStreamFixedSizePrimitiveAttribute"/>.</summary>
        FixedSize,

        /// <summary>Values map into a bit range between caller-supplied <c>min</c> and <c>max</c>. Requires <see cref="BitStreamQuantizedPrimitiveAttribute"/>.</summary>
        Quantized,

        /// <summary>Bit length depends on the value. Requires <see cref="BitStreamPrimitiveRole.Size"/> and <see cref="BitStreamPrimitiveRole.TryRead"/> methods.</summary>
        VariableLength
    }

    /// <summary>Declares the fixed bit width for a <see cref="BitStreamPrimitiveAttribute"/> with <see cref="PrimitiveSerializationMode.FixedSize"/>.</summary>
    /// <remarks>Required companion for fixed-size primitives (<c>CBS015</c> when missing).</remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamFixedSizePrimitiveAttribute : Attribute {
        /// <param name="size">Bits written per value. Must be greater than zero (<c>CBS008</c>).</param>
        public BitStreamFixedSizePrimitiveAttribute(int size) { }
    }

    /// <summary>Declares the allowed <c>bitCount</c> range for a quantized primitive.</summary>
    /// <remarks>
    /// <para>Required companion for quantized primitives (<c>CBS015</c> when missing).</para>
    /// <para>Generated wrappers validate <c>bitCount</c> against this range. Implementation methods append <c>min</c>, <c>max</c>, and <c>bitCount</c> per <see cref="BitStreamPrimitiveAuthorDocumentation.QuantizedParameters"/>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamQuantizedPrimitiveAttribute : Attribute {
        /// <param name="minimumBits">Smallest <c>bitCount</c> accepted on generated methods. Must satisfy <c>0 &lt; minimum &lt;= maximum</c> (<c>CBS016</c>).</param>
        /// <param name="maximumBits">Largest <c>bitCount</c> accepted on generated methods.</param>
        public BitStreamQuantizedPrimitiveAttribute(int minimumBits, int maximumBits) { }
    }

    /// <summary>Marks an implementation method on a <see cref="BitStreamPrimitiveAttribute"/> class.</summary>
    /// <remarks>
    /// <para>The generator validates the signature for the supplied <see cref="BitStreamPrimitiveRole"/>. Mismatches report <c>CBS010</c> with the expected signature in the diagnostic text.</para>
    /// <para>Each role may appear at most once per class (<c>CBS001</c>). Methods must be <c>public static</c> (<c>CBS007</c>).</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class BitStreamPrimitiveMethodAttribute : BitStreamRestrictedPrimitiveMethodAttribute {
        /// <param name="role">Which generated wrapper method this implementation backs.</param>
        public BitStreamPrimitiveMethodAttribute(BitStreamPrimitiveRole role) { }
    }

    /// <summary>Role of a method on a <see cref="BitStreamPrimitiveAttribute"/> implementation class.</summary>
    public enum BitStreamPrimitiveRole : int {
        /// <summary>Write one value. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleWrite"/>.</summary>
        Write,

        /// <summary>Write a span without a length prefix. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleWriteSpan"/>.</summary>
        WriteSpan,

        /// <summary>Read one value without advancing position. See <see cref="BitStreamPrimitiveAuthorDocumentation.RolePeek"/>.</summary>
        Peek,

        /// <summary>Read one value and advance position. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleRead"/>.</summary>
        Read,

        /// <summary>Attempt to read one variable-length value. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleTryRead"/>.</summary>
        TryRead,

        /// <summary>Peek an array when the caller supplies the count. See <see cref="BitStreamPrimitiveAuthorDocumentation.RolePeekArray"/>.</summary>
        PeekArray,

        /// <summary>Read an array when the caller supplies the count. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleReadArray"/>.</summary>
        ReadArray,

        /// <summary>Peek into a span when the caller supplies the count. See <see cref="BitStreamPrimitiveAuthorDocumentation.RolePeekSpan"/>.</summary>
        PeekSpan,

        /// <summary>Read into a span when the caller supplies the count. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleReadSpan"/>.</summary>
        ReadSpan,

        /// <summary>Return the encoded bit length of one value. See <see cref="BitStreamPrimitiveAuthorDocumentation.RoleSize"/>.</summary>
        Size,
    }

    /// <summary>Marks methods that may only be called from primitive implementation code.</summary>
    /// <remarks>Applied to <see cref="BitStreamPrimitiveMethodAttribute"/> and low-level context helpers. See <see cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>.</remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BitStreamRestrictedPrimitiveMethodAttribute : Attribute { }

    /// <summary>Marks a type whose methods may call <c>*Primitive</c> APIs on <see cref="ComputerysBitStream.ReadContext"/> and <see cref="ComputerysBitStream.WriteContext"/>.</summary>
    /// <remarks>
    /// <para>Applied to <see cref="ComputerysBitStream.ReadContext"/>, <see cref="ComputerysBitStream.WriteContext"/>, encoding helpers, and <see cref="BitStreamPrimitiveAttribute"/> implementation classes.</para>
    /// <para>Call sites outside these contexts get analyzer warning <c>CBS031</c>. See <see cref="BitStreamPrimitiveAuthorDocumentation.PrimitiveContextUsage"/>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class BitStreamPrimitiveContextAttribute : Attribute { }
}
