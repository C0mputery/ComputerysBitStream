using System;

namespace ComputerysBitStream;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BitStreamType(Type type, int size) : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class WriteRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class WriteSpanRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class PeakRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ReadRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class PeakArrayRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ReadArrayRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class PeakSpanRawAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ReadSpanRawAttribute : Attribute;
