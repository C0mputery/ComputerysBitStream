using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;
using ComputerysBitStream.Primitives.FixedSize;
using ComputerysBitStream.Primitives.Quantized;
using ComputerysBitStream.Primitives.VariableLength;

[assembly: DefaultBitStreamSettings(typeof(ComputerysBitStream.Tests.ITestSettings))]

namespace ComputerysBitStream.Tests;

[BitStreamSettings]
#if !BITSTREAM_SOURCE_GENERATOR
[BitStreamSerializer(typeof(PrimitiveDateTimeExtensions))]
[BitStreamSerializer(typeof(PrimitiveQuantizedFloatExtensions))]
[BitStreamSerializer(typeof(PrimitiveQuantizedDoubleExtensions))]
[BitStreamSerializer(typeof(PrimitiveQuantizedDecimalExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthByteExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthSByteExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthShortExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthUShortExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthIntExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthUIntExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthLongExtensions))]
[BitStreamSerializer(typeof(PrimitiveVariableLengthULongExtensions))]
#endif
public interface ITestSettings : IGameExtrasSettings { }
