#pragma warning disable CS9113 // Parameter is unread.

using System;

namespace ComputerysBitStream;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class BitStreamTypeAttribute(Type type, int size) : Attribute;

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