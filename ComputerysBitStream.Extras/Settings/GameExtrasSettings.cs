using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Primitives.Quantized;
using ComputerysBitStream.Extras.Proxies.Numerics;

[assembly: DefaultBitStreamSettings(typeof(ComputerysBitStream.Extras.Settings.IGameExtrasSettings))]

namespace ComputerysBitStream.Extras.Settings {
    [BitStreamSettings]
#if !BITSTREAM_SOURCE_GENERATOR
    [BitStreamSerializer(typeof(Vector2Proxy))]
    [BitStreamSerializer(typeof(Vector3Proxy))]
    [BitStreamSerializer(typeof(Vector4Proxy))]
    [BitStreamSerializer(typeof(QuaternionProxy))]
    [BitStreamSerializer(typeof(PlaneProxy))]
    [BitStreamSerializer(typeof(Matrix4x4Proxy))]
    [BitStreamSerializer(typeof(QuantizedVector2Extensions))]
    [BitStreamSerializer(typeof(QuantizedVector3Extensions))]
    [BitStreamSerializer(typeof(QuantizedVector4Extensions))]
    [BitStreamSerializer(typeof(QuantizedQuaternionExtensions))]
    [BitStreamSerializer(typeof(QuantizedPlaneExtensions))]
    [BitStreamSerializer(typeof(QuantizedMatrix4x4Extensions))]
#endif
    public interface IGameExtrasSettings : IDefaultSettings { }
}
