using System.Numerics;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

namespace ComputerysBitStream.Extras.Proxies.Numerics {
    [BitStreamProxyStruct(typeof(Matrix4x4), typeof(IGameExtrasSettings))]
    public static partial class Matrix4x4Proxy {
        public static float M11;
        public static float M12;
        public static float M13;
        public static float M14;
        public static float M21;
        public static float M22;
        public static float M23;
        public static float M24;
        public static float M31;
        public static float M32;
        public static float M33;
        public static float M34;
        public static float M41;
        public static float M42;
        public static float M43;
        public static float M44;
    }
}
