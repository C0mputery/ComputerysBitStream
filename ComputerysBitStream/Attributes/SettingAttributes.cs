#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BitStreamSerializerAttribute : Attribute {
        public BitStreamSerializerAttribute(Type type) { }
    }

    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class BitStreamSettingsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class DefaultBitStreamSettingsAttribute : Attribute {
        public DefaultBitStreamSettingsAttribute(params Type[] settings) { }
    }
}
