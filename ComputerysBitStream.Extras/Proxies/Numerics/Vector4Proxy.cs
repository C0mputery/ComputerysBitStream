using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

namespace ComputerysBitStream.Extras.Proxies.Numerics {
    [BitStreamProxyStruct(typeof(System.Numerics.Vector4), typeof(IGameExtrasSettings))]
    public static partial class Vector4Proxy {
        public static float X;
        public static float Y;
        public static float Z;
        public static float W;
    }
}
