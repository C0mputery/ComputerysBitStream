#pragma warning disable CS9113 // Parameter is unread.
// ReSharper disable UnusedParameter.Local

using System;

namespace ComputerysBitStream.Attributes {
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamPrimitiveAttribute : Attribute {
        public BitStreamPrimitiveAttribute(Type target, PrimitiveSerializationMode serializationMode, params Type[] settings) { }

        public BitStreamPrimitiveAttribute(Type target, string alias, PrimitiveSerializationMode serializationMode, params Type[] settings) { }
    }

    public enum PrimitiveSerializationMode : int {
        FixedSize,
        Quantized,
        VariableLength
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamFixedSizePrimitiveAttribute : Attribute {
        public BitStreamFixedSizePrimitiveAttribute(int size) { }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class BitStreamQuantizedPrimitiveAttribute : Attribute {
        public BitStreamQuantizedPrimitiveAttribute(int minimumBits, int maximumBits) { }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class BitStreamPrimitiveMethodAttribute : BitStreamRestrictedPrimitiveMethodAttribute {
        public BitStreamPrimitiveMethodAttribute(BitStreamPrimitiveRole role) { }
    }

    public enum BitStreamPrimitiveRole : int {
        Write,
        WriteSpan,
        Peek,
        Read,
        TryRead,
        PeekArray,
        ReadArray,
        PeekSpan,
        ReadSpan,
        Size,
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BitStreamRestrictedPrimitiveMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class BitStreamPrimitiveContextAttribute : Attribute { }
}
