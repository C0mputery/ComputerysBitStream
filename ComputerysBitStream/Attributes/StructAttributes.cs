#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class BitStreamStructAttribute : Attribute {
        public BitStreamStructAttribute(params Type[] settings) { }

        public BitStreamStructAttribute(string alias, params Type[] settings) { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructIncludeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructIgnoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class BitStreamStructQuantizedRangeAttribute : Attribute {
        public BitStreamStructQuantizedRangeAttribute(string minMember, string maxMember, int bitCount) { }

        public BitStreamStructQuantizedRangeAttribute(Type source, string minMember, string maxMember, int bitCount) { }

        public BitStreamStructQuantizedRangeAttribute(Type minSource, string minMember, Type maxSource, string maxMember, int bitCount) { }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamProxyStructAttribute : Attribute {
        public BitStreamProxyStructAttribute(Type targetStruct, params Type[] settings) { }

        public BitStreamProxyStructAttribute(Type targetStruct, string alias, params Type[] settings) { }
    }
}
