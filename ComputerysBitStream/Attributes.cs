#pragma warning disable CS9113 // Parameter is unread.

using System;

namespace ComputerysBitStream;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class BitStreamTypeAttribute(Type type, int size) : Attribute {
    public BitStreamTypeAttribute(Type type, int size, string targetTypeName) : this(type, size) { TargetTypeName = targetTypeName; }
    
    public Type Type { get; } = type;
    public int Size { get; } = size;
    public string? TargetTypeName { get; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class BitStreamRawAttribute(BitStreamRawRole role) : Attribute;

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