#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    /// <summary>Registers a serializer type with a settings interface or overrides the serializer for one struct member.</summary>
    /// <remarks>
    /// <para>On a <see cref="BitStreamSettingsAttribute"/> interface, register primitive extension classes (<c>typeof(PrimitiveIntExtensions)</c>), <see cref="BitStreamStructAttribute"/> types (<c>typeof(MyStruct)</c>), <see cref="BitStreamProxyStructAttribute"/> proxy classes, or types that already carry generated <see cref="BitStreamStructMetadataAttribute"/> from another assembly.</para>
    /// <para>On a field or property, picks the primitive extension class for that member only. Applies to primitive-typed members; nested structs still need settings registration.</para>
    /// <para>Multiple attributes on one member report <c>CBS026</c>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BitStreamSerializerAttribute : Attribute {
        /// <param name="type">Extension class, struct type, proxy class, or generated external struct metadata type to register or apply.</param>
        public BitStreamSerializerAttribute(Type type) { }
    }

    /// <summary>Marks an interface as a BitStream settings container. Serializers are listed with <see cref="BitStreamSerializerAttribute"/> on the same interface.</summary>
    /// <remarks>Pass the interface to <see cref="BitStreamStructAttribute"/> or <see cref="DefaultBitStreamSettingsAttribute"/> to include it in effective settings.</remarks>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class BitStreamSettingsAttribute : Attribute { }

    /// <summary>Registers default settings interfaces for the whole assembly.</summary>
    /// <remarks>
    /// <para>When omitted, the generator uses <see cref="ComputerysBitStream.IDefaultSettings"/> if the assembly defines that interface.</para>
    /// <para>More than one assembly-level attribute reports <c>CBS003</c>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class DefaultBitStreamSettingsAttribute : Attribute {
        /// <param name="settings"><see cref="BitStreamSettingsAttribute"/> interfaces merged into assembly-wide defaults.</param>
        public DefaultBitStreamSettingsAttribute(params Type[] settings) { }
    }
}
