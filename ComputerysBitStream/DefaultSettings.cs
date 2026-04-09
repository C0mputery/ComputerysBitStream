using ComputerysBitStream;

[assembly: DefaultBitStreamSettings(typeof(IDefaultSettings))]

namespace ComputerysBitStream {
    [BitStreamSettings]
#if !COMPUTERYS_BITSTREAM_GENERATOR
    [BitStreamSetting(typeof(RawBoolExtensions))]
    [BitStreamSetting(typeof(RawByteExtensions))]
    [BitStreamSetting(typeof(RawCharExtensions))]
    [BitStreamSetting(typeof(RawDecimalExtensions))]
    [BitStreamSetting(typeof(RawDoubleExtensions))]
    [BitStreamSetting(typeof(RawFloatExtensions))]
    [BitStreamSetting(typeof(RawIntExtensions))]
    [BitStreamSetting(typeof(RawLongExtensions))]
    [BitStreamSetting(typeof(RawSByteExtensions))]
    [BitStreamSetting(typeof(RawShortExtensions))]
    [BitStreamSetting(typeof(RawUIntExtensions))]
    [BitStreamSetting(typeof(RawULongExtensions))]
    [BitStreamSetting(typeof(RawUShortExtensions))]
#endif
    public interface IDefaultSettings { }
}