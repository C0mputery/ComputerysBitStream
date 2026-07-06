#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    /// <summary>Marks a static class as a custom primitive serializer. Methods are tagged with <see cref="BitStreamPrimitiveMethodAttribute"/>.</summary>
    /// <remarks>
    /// <para>The class must be <c>public static</c> and pair with <see cref="BitStreamFixedSizePrimitiveAttribute"/>, <see cref="BitStreamQuantizedPrimitiveAttribute"/>, or variable-length rules (<see cref="PrimitiveSerializationMode.VariableLength"/> requires <c>Size</c> and <c>TryRead</c> roles).</para>
    /// <para>Register the extension class on a <see cref="BitStreamSettingsAttribute"/> interface via <see cref="BitStreamSerializerAttribute"/>.</para>
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
        /// <summary>Every value uses a fixed number of bits.</summary>
        FixedSize,
        /// <summary>Values map into a bit range between caller-supplied <c>min</c> and <c>max</c>.</summary>
        Quantized,
        /// <summary>Bit length depends on the value. Requires <see cref="BitStreamPrimitiveRole.Size"/> and <see cref="BitStreamPrimitiveRole.TryRead"/> methods.</summary>
        VariableLength
    }

    /// <summary>Declares the fixed bit width for a <see cref="BitStreamPrimitiveAttribute"/> with <see cref="PrimitiveSerializationMode.FixedSize"/>.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamFixedSizePrimitiveAttribute : Attribute {
        /// <param name="size">Bits written per value. Must be greater than zero (<c>CBS008</c>).</param>
        public BitStreamFixedSizePrimitiveAttribute(int size) { }
    }

    /// <summary>Declares the allowed <c>bitCount</c> range for a quantized primitive.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamQuantizedPrimitiveAttribute : Attribute {
        /// <param name="minimumBits">Smallest <c>bitCount</c> accepted on generated methods.</param>
        /// <param name="maximumBits">Largest <c>bitCount</c> accepted on generated methods.</param>
        public BitStreamQuantizedPrimitiveAttribute(int minimumBits, int maximumBits) { }
    }

    /// <summary>Marks an implementation method on a <see cref="BitStreamPrimitiveAttribute"/> class. The generator validates the signature for the supplied <see cref="BitStreamPrimitiveRole"/>.</summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class BitStreamPrimitiveMethodAttribute : BitStreamRestrictedPrimitiveMethodAttribute {
        /// <param name="role">Which generated wrapper method this implementation backs.</param>
        public BitStreamPrimitiveMethodAttribute(BitStreamPrimitiveRole role) { }
    }

    /// <summary>Role of a method on a <see cref="BitStreamPrimitiveAttribute"/> implementation class.</summary>
    public enum BitStreamPrimitiveRole : int {
        /// <summary>Write one value (<c>Write*Primitive</c>).</summary>
        Write,
        /// <summary>Write a span of values without a length prefix.</summary>
        WriteSpan,
        /// <summary>Read one value without advancing position.</summary>
        Peek,
        /// <summary>Read one value and advance position.</summary>
        Read,
        /// <summary>Attempt to read one variable-length value. Required for <see cref="PrimitiveSerializationMode.VariableLength"/>.</summary>
        TryRead,
        /// <summary>Read an array when the caller supplies the count.</summary>
        PeekArray,
        /// <summary>Read an array when the caller supplies the count.</summary>
        ReadArray,
        /// <summary>Read into a span when the caller supplies the count.</summary>
        PeekSpan,
        /// <summary>Read into a span when the caller supplies the count.</summary>
        ReadSpan,
        /// <summary>Return the encoded bit length of one value. Required for <see cref="PrimitiveSerializationMode.VariableLength"/>.</summary>
        Size,
    }

    /// <summary>Base attribute for methods that may only be called from primitive implementation code.</summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BitStreamRestrictedPrimitiveMethodAttribute : Attribute { }

    /// <summary>Marks a type whose methods may call <c>*Primitive</c> APIs on <see cref="ReadContext"/> and <see cref="WriteContext"/> without analyzer warning <c>CBS031</c>.</summary>
    /// <remarks>Applied to <see cref="ComputerysBitStream.ReadContext"/>, <see cref="ComputerysBitStream.WriteContext"/>, and primitive helper types.</remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class BitStreamPrimitiveContextAttribute : Attribute { }
}
