using System.Numerics;
using ComputerysBitStream;

namespace ComputerysBitStream.Tests;

[BitStreamProxyStruct(typeof(Vector2), [nameof(Vector2.X), nameof(Vector2.Y)], null)]
public static partial class Vector2Proxy;

[BitStreamProxyStruct(typeof(Vector3), [nameof(Vector3.X), nameof(Vector3.Y), nameof(Vector3.Z)], null)]
public static partial class Vector3Proxy;

[BitStreamProxyStruct(typeof(Vector4), [nameof(Vector4.X), nameof(Vector4.Y), nameof(Vector4.Z), nameof(Vector4.W)], null)]
public static partial class Vector4Proxy;

[BitStreamProxyStruct(typeof(Quaternion), [nameof(Quaternion.X), nameof(Quaternion.Y), nameof(Quaternion.Z), nameof(Quaternion.W)], null)]
public static partial class QuaternionProxy;

[BitStreamProxyStruct(typeof(Matrix3x2), [
    nameof(Matrix3x2.M11), nameof(Matrix3x2.M12),
    nameof(Matrix3x2.M21), nameof(Matrix3x2.M22),
    nameof(Matrix3x2.M31), nameof(Matrix3x2.M32)
], null)]
public static partial class Matrix3x2Proxy;

[BitStreamProxyStruct(typeof(Matrix4x4), [
    nameof(Matrix4x4.M11), nameof(Matrix4x4.M12), nameof(Matrix4x4.M13), nameof(Matrix4x4.M14),
    nameof(Matrix4x4.M21), nameof(Matrix4x4.M22), nameof(Matrix4x4.M23), nameof(Matrix4x4.M24),
    nameof(Matrix4x4.M31), nameof(Matrix4x4.M32), nameof(Matrix4x4.M33), nameof(Matrix4x4.M34),
    nameof(Matrix4x4.M41), nameof(Matrix4x4.M42), nameof(Matrix4x4.M43), nameof(Matrix4x4.M44)
], null)]
public static partial class Matrix4x4Proxy;

[BitStreamSettings]
[BitStreamSetting(typeof(Vector3Proxy))]
public interface IVector3Settings : IDefaultSettings;

[BitStreamProxyStruct(typeof(Plane), [nameof(Plane.Normal), nameof(Plane.D)], null, typeof(IVector3Settings))]
public static partial class PlaneProxy;
