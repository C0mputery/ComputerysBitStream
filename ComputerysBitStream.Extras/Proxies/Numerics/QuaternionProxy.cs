using System.Numerics;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

namespace ComputerysBitStream.Extras.Proxies.Numerics {
    [BitStreamProxyStruct(typeof(Quaternion), typeof(IGameExtrasSettings))]
    public static partial class QuaternionProxy {
        public static float X;
        public static float Y;
        public static float Z;
        public static float W;
    }
}
