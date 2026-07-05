using System.Numerics;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

namespace ComputerysBitStream.Extras.Proxies.Numerics {
    [BitStreamProxyStruct(typeof(Vector3), typeof(IGameExtrasSettings))]
    public static partial class Vector3Proxy {
        public static float X;
        public static float Y;
        public static float Z;
    }
}
