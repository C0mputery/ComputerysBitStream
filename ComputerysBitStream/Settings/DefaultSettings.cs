using ComputerysBitStream;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_SOURCE_GENERATOR
using ComputerysBitStream.Primitives.FixedSize;
#endif

[assembly: DefaultBitStreamSettings(typeof(IDefaultSettings))]

namespace ComputerysBitStream {
    [BitStreamSettings]
#if !BITSTREAM_SOURCE_GENERATOR
    [BitStreamSerializer(typeof(PrimitiveBoolExtensions))]
    [BitStreamSerializer(typeof(PrimitiveByteExtensions))]
    [BitStreamSerializer(typeof(PrimitiveCharExtensions))]
    [BitStreamSerializer(typeof(PrimitiveDecimalExtensions))]
    [BitStreamSerializer(typeof(PrimitiveDoubleExtensions))]
    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    [BitStreamSerializer(typeof(PrimitiveIntExtensions))]
    [BitStreamSerializer(typeof(PrimitiveLongExtensions))]
    [BitStreamSerializer(typeof(PrimitiveSByteExtensions))]
    [BitStreamSerializer(typeof(PrimitiveShortExtensions))]
    [BitStreamSerializer(typeof(PrimitiveUIntExtensions))]
    [BitStreamSerializer(typeof(PrimitiveULongExtensions))]
    [BitStreamSerializer(typeof(PrimitiveUShortExtensions))]
#endif
    public interface IDefaultSettings { }
}
