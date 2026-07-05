using System.Numerics;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

namespace ComputerysBitStream.Extras.Proxies.Numerics {
    [BitStreamProxyStruct(typeof(Vector2), typeof(IGameExtrasSettings))]
    public static partial class Vector2Proxy {
        public static float X;
        public static float Y;
    }
}
