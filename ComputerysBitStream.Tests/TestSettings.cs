using ComputerysBitStream.Extras.Settings;

[assembly: DefaultBitStreamSettings(typeof(ComputerysBitStream.Tests.ITestSettings))]

namespace ComputerysBitStream.Tests;

[BitStreamSettings]
#if !BITSTREAM_SOURCE_GENERATOR
[BitStreamSerializer(typeof(PrimitiveDateTimeExtensions))]
[BitStreamSerializer(typeof(PrimitiveQuantizedFloatExtensions))]
[BitStreamSerializer(typeof(PrimitiveQuantizedDoubleExtensions))]
[BitStreamSerializer(typeof(PrimitiveQuantizedDecimalExtensions))]
#endif
public interface ITestSettings : IGameExtrasSettings { }
