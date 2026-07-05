using ComputerysBitStream;
using ComputerysBitStream.Attributes;
#if !BITSTREAM_SOURCE_GENERATOR
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Primitives.Quantized;
using ComputerysBitStream.Primitives.VariableLength;
#endif

[assembly: DefaultBitStreamSettings(typeof(IDefaultSettings))]

namespace ComputerysBitStream {
    [BitStreamSettings]
#if !BITSTREAM_SOURCE_GENERATOR
    [BitStreamSerializer(typeof(PrimitiveBoolExtensions))]
    [BitStreamSerializer(typeof(PrimitiveByteExtensions))]
    [BitStreamSerializer(typeof(PrimitiveCharExtensions))]
    [BitStreamSerializer(typeof(PrimitiveDateTimeExtensions))]
    [BitStreamSerializer(typeof(PrimitiveDecimalExtensions))]
    [BitStreamSerializer(typeof(PrimitiveDoubleExtensions))]
    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    [BitStreamSerializer(typeof(PrimitiveIntExtensions))]
    [BitStreamSerializer(typeof(PrimitiveLongExtensions))]
    [BitStreamSerializer(typeof(PrimitiveQuantizedDecimalExtensions))]
    [BitStreamSerializer(typeof(PrimitiveQuantizedDoubleExtensions))]
    [BitStreamSerializer(typeof(PrimitiveQuantizedFloatExtensions))]
    [BitStreamSerializer(typeof(PrimitiveSByteExtensions))]
    [BitStreamSerializer(typeof(PrimitiveShortExtensions))]
    [BitStreamSerializer(typeof(PrimitiveUIntExtensions))]
    [BitStreamSerializer(typeof(PrimitiveULongExtensions))]
    [BitStreamSerializer(typeof(PrimitiveUShortExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthByteExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthIntExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthLongExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthSByteExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthShortExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthUIntExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthULongExtensions))]
    [BitStreamSerializer(typeof(PrimitiveVariableLengthUShortExtensions))]
    [BitStreamSerializer(typeof(PrimitiveStringExtensions))]
#endif
    public interface IDefaultSettings { }
}
