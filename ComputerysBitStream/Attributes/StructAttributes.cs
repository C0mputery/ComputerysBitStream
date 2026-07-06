#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    /// <summary>
    /// Marks a <c>partial</c> struct or record struct for source generation of <c>Write*</c> and <c>Read*</c> extension methods.
    /// </summary>
    /// <remarks>
    /// <para>Public properties with public getters and writable setters (including <c>init</c>) serialize by default. Read-only properties are skipped unless a <see cref="BitStreamProxyStructAttribute"/> mirror supplies a writable setter. Public fields are skipped unless marked with <see cref="BitStreamStructIncludeAttribute"/>.</para>
    /// <para>Pass one or more <see cref="BitStreamSettingsAttribute"/> interfaces to merge serializers beyond assembly defaults. Global defaults come from <see cref="DefaultBitStreamSettingsAttribute"/> or <see cref="ComputerysBitStream.IDefaultSettings"/>.</para>
    /// <para>A nested <see cref="BitStreamStructAttribute"/> type is not registered automatically. Add <see cref="BitStreamSerializerAttribute"/> for the nested type on a settings interface. If the nested type is missing from settings, the build reports <c>CBS043</c>. If it is registered but fails to resolve, the build reports <c>CBS036</c>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class BitStreamStructAttribute : Attribute {
        /// <summary>Uses the struct type name as the method alias and merges the given settings interfaces.</summary>
        /// <param name="settings"><see cref="BitStreamSettingsAttribute"/> interfaces whose serializers apply to this struct and its members.</param>
        public BitStreamStructAttribute(params Type[] settings) { }

        /// <summary>Uses a custom method alias (for example <c>Player</c> emits <c>WritePlayer</c> / <c>ReadPlayer</c>).</summary>
        /// <param name="alias">Prefix for generated extension method names.</param>
        /// <param name="settings"><see cref="BitStreamSettingsAttribute"/> interfaces whose serializers apply to this struct and its members.</param>
        public BitStreamStructAttribute(string alias, params Type[] settings) { }
    }

    /// <summary>Includes a public field in serialization. Fields are excluded by default.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructIncludeAttribute : Attribute { }

    /// <summary>Excludes a public property from serialization. Properties are included by default.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructIgnoreAttribute : Attribute { }

    /// <summary>Serializes the member with the variable-length serializer registered in effective settings for its CLR type.</summary>
    /// <remarks>Reports <c>CBS042</c> when no variable-length serializer is registered. Cannot be combined with <see cref="BitStreamStructQuantizedAttribute"/> (<c>CBS045</c>).</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructVariableLengthAttribute : Attribute { }

    /// <summary>Serializes the member with the quantized serializer registered in effective settings for its CLR type.</summary>
    /// <remarks>
    /// <para>Reports <c>CBS038</c> when no quantized serializer is registered. Reports <c>CBS044</c> when settings include a quantized serializer for the member type but this attribute is missing. Cannot be combined with <see cref="BitStreamStructVariableLengthAttribute"/> (<c>CBS045</c>).</para>
    /// <para><c>minMember</c> and <c>maxMember</c> must name accessible <c>const</c> or <c>static readonly</c> members on the struct or on types passed to the constructors.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructQuantizedAttribute : Attribute {
        /// <summary>Reads <c>min</c> and <c>max</c> from members on the containing struct.</summary>
        /// <param name="minMember">Name of the minimum bound member.</param>
        /// <param name="maxMember">Name of the maximum bound member.</param>
        /// <param name="bitCount">Number of bits used to store the quantized value.</param>
        public BitStreamStructQuantizedAttribute(string minMember, string maxMember, int bitCount) { }

        /// <summary>Reads <c>min</c> and <c>max</c> from members on <paramref name="source"/>.</summary>
        /// <param name="source">Type that declares both bound members.</param>
        /// <param name="minMember">Name of the minimum bound member.</param>
        /// <param name="maxMember">Name of the maximum bound member.</param>
        /// <param name="bitCount">Number of bits used to store the quantized value.</param>
        public BitStreamStructQuantizedAttribute(Type source, string minMember, string maxMember, int bitCount) { }

        /// <summary>Reads <c>min</c> from <paramref name="minSource"/> and <c>max</c> from <paramref name="maxSource"/>.</summary>
        /// <param name="minSource">Type that declares the minimum bound member.</param>
        /// <param name="minMember">Name of the minimum bound member.</param>
        /// <param name="maxSource">Type that declares the maximum bound member.</param>
        /// <param name="maxMember">Name of the maximum bound member.</param>
        /// <param name="bitCount">Number of bits used to store the quantized value.</param>
        public BitStreamStructQuantizedAttribute(Type minSource, string minMember, Type maxSource, string maxMember, int bitCount) { }
    }

    /// <summary>
    /// Marks a static proxy class that serializes an external struct the generator cannot annotate directly.
    /// </summary>
    /// <remarks>
    /// <para>Each public static field or property on the proxy is serialized. Register the proxy type with <see cref="BitStreamSerializerAttribute"/> on a settings interface, not the target struct type.</para>
    /// <para>The proxy class must be <c>public static partial</c>. The target must be a struct (<c>CBS012</c> if it is not).</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamProxyStructAttribute : Attribute {
        /// <summary>Uses the proxy class name as the method alias.</summary>
        /// <param name="targetStruct">External struct type being proxied.</param>
        /// <param name="settings">Optional settings interfaces merged for this proxy.</param>
        public BitStreamProxyStructAttribute(Type targetStruct, params Type[] settings) { }

        /// <summary>Uses a custom method alias for generated extensions.</summary>
        /// <param name="targetStruct">External struct type being proxied.</param>
        /// <param name="alias">Prefix for generated extension method names.</param>
        /// <param name="settings">Optional settings interfaces merged for this proxy.</param>
        public BitStreamProxyStructAttribute(Type targetStruct, string alias, params Type[] settings) { }
    }
}
