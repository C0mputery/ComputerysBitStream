using System.Numerics;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

namespace ComputerysBitStream.Extras.Proxies.Numerics {
    [BitStreamProxyStruct(typeof(Plane), typeof(IGameExtrasSettings))]
    public static partial class PlaneProxy {
        public static Vector3 Normal;
        public static float D;
    }
}
