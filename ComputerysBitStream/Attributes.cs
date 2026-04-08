#pragma warning disable CS9113 // Parameter is unread.

using System;

namespace ComputerysBitStream {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BitStreamTypeAttribute : Attribute {
        public BitStreamTypeAttribute(Type type, int size) { Type = type; Size = size; }
        public BitStreamTypeAttribute(Type type, int size, string targetTypeName) : this(type, size) { TargetTypeName = targetTypeName; }
        public Type Type { get; }
        public int Size { get; }
        public string? TargetTypeName { get; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class BitStreamRawAttribute : Attribute {
        public BitStreamRawAttribute(BitStreamRawRole role) { }
    }

    public enum BitStreamRawRole : int {
        Write,
        WriteSpan,
        Peek,
        Read,
        PeekArray,
        ReadArray,
        PeekSpan,
        ReadSpan
    }
}