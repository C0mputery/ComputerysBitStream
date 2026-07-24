using System.Numerics;
using ComputerysBitStream.Extras.Proxies.Numerics;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Extras.Proxies;

public class Vector2Tests : StructTestSuite<Vector2> {
    protected override Vector2 Value => new(1.0f, 2.0f);

    protected override Vector2[] Values => [
        new(1.0f, 2.0f),
        new(3.0f, 4.0f),
        new(5.0f, 6.0f)
    ];

    protected override int? ExpectedFixedSizeBits => 64;

    protected override Type StructType => typeof(Vector2Proxy);

    protected override SerializationOperations<Vector2> Operations { get; } = new() {
        Write = (ref WriteContext context, Vector2 value) => context.WriteVector2(value),
        Peek = (ReadContext context) => context.PeekVector2(),
        Read = (ReadContext context) => context.ReadVector2(),
        TryPeek = (ReadContext context, out Vector2 value) => context.TryPeekVector2(out value),
        TryRead = (ReadContext context, out Vector2 value) => context.TryReadVector2(out value),
        WriteSpan = (ref WriteContext context, Span<Vector2> values) => context.WriteVector2s(values),
        PeekSpan = (ReadContext context, Span<Vector2> destination) => context.PeekVector2s(destination),
        ReadSpan = (ReadContext context, Span<Vector2> destination) => context.ReadVector2s(destination),
        TryPeekSpan = (ReadContext context, Span<Vector2> destination) => context.TryPeekVector2s(destination),
        TryReadSpan = (ReadContext context, Span<Vector2> destination) => context.TryReadVector2s(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Vector2> values) => context.WriteVector2sWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.PeekVector2s(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.ReadVector2s(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.TryPeekVector2s(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.TryReadVector2s(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.PeekVector2sWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.ReadVector2sWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.TryPeekVector2sWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.TryReadVector2sWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, Vector2[] values) => context.WriteVector2s(values),
        PeekArray = (ReadContext context) => context.PeekVector2s(),
        ReadArray = (ReadContext context) => context.ReadVector2s(),
        TryPeekArray = (ReadContext context, out Vector2[] values) => context.TryPeekVector2s(out values),
        TryReadArray = (ReadContext context, out Vector2[] values) => context.TryReadVector2s(out values),
        WriteArrayWithoutLength = (ref WriteContext context, Vector2[] values) => context.WriteVector2sWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVector2s(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVector2s(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Vector2[] values) => context.TryPeekVector2s(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Vector2[] values) => context.TryReadVector2s(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVector2sWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVector2sWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Vector2[] values) => context.TryPeekVector2sWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Vector2[] values) => context.TryReadVector2sWithMaxCount(maxCount, out values),
    };
}

public class Vector3Tests : StructTestSuite<Vector3> {
    protected override Vector3 Value => new(1.0f, 2.0f, 3.0f);

    protected override Vector3[] Values => [
        new(1.0f, 2.0f, 3.0f),
        new(4.0f, 5.0f, 6.0f),
        new(7.0f, 8.0f, 9.0f)
    ];

    protected override int? ExpectedFixedSizeBits => 96;

    protected override Type StructType => typeof(Vector3Proxy);

    protected override SerializationOperations<Vector3> Operations { get; } = new() {
        Write = (ref WriteContext context, Vector3 value) => context.WriteVector3(value),
        Peek = (ReadContext context) => context.PeekVector3(),
        Read = (ReadContext context) => context.ReadVector3(),
        TryPeek = (ReadContext context, out Vector3 value) => context.TryPeekVector3(out value),
        TryRead = (ReadContext context, out Vector3 value) => context.TryReadVector3(out value),
        WriteSpan = (ref WriteContext context, Span<Vector3> values) => context.WriteVector3s(values),
        PeekSpan = (ReadContext context, Span<Vector3> destination) => context.PeekVector3s(destination),
        ReadSpan = (ReadContext context, Span<Vector3> destination) => context.ReadVector3s(destination),
        TryPeekSpan = (ReadContext context, Span<Vector3> destination) => context.TryPeekVector3s(destination),
        TryReadSpan = (ReadContext context, Span<Vector3> destination) => context.TryReadVector3s(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Vector3> values) => context.WriteVector3sWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.PeekVector3s(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.ReadVector3s(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.TryPeekVector3s(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.TryReadVector3s(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.PeekVector3sWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.ReadVector3sWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.TryPeekVector3sWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.TryReadVector3sWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, Vector3[] values) => context.WriteVector3s(values),
        PeekArray = (ReadContext context) => context.PeekVector3s(),
        ReadArray = (ReadContext context) => context.ReadVector3s(),
        TryPeekArray = (ReadContext context, out Vector3[] values) => context.TryPeekVector3s(out values),
        TryReadArray = (ReadContext context, out Vector3[] values) => context.TryReadVector3s(out values),
        WriteArrayWithoutLength = (ref WriteContext context, Vector3[] values) => context.WriteVector3sWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVector3s(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVector3s(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Vector3[] values) => context.TryPeekVector3s(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Vector3[] values) => context.TryReadVector3s(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVector3sWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVector3sWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Vector3[] values) => context.TryPeekVector3sWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Vector3[] values) => context.TryReadVector3sWithMaxCount(maxCount, out values),
    };
}

public class Vector4Tests : StructTestSuite<Vector4> {
    protected override Vector4 Value => new(1.0f, 2.0f, 3.0f, 4.0f);

    protected override Vector4[] Values => [
        new(1.0f, 2.0f, 3.0f, 4.0f),
        new(5.0f, 6.0f, 7.0f, 8.0f),
        new(9.0f, 10.0f, 11.0f, 12.0f)
    ];

    protected override int? ExpectedFixedSizeBits => 128;

    protected override Type StructType => typeof(Vector4Proxy);

    protected override SerializationOperations<Vector4> Operations { get; } = new() {
        Write = (ref WriteContext context, Vector4 value) => context.WriteVector4(value),
        Peek = (ReadContext context) => context.PeekVector4(),
        Read = (ReadContext context) => context.ReadVector4(),
        TryPeek = (ReadContext context, out Vector4 value) => context.TryPeekVector4(out value),
        TryRead = (ReadContext context, out Vector4 value) => context.TryReadVector4(out value),
        WriteSpan = (ref WriteContext context, Span<Vector4> values) => context.WriteVector4s(values),
        PeekSpan = (ReadContext context, Span<Vector4> destination) => context.PeekVector4s(destination),
        ReadSpan = (ReadContext context, Span<Vector4> destination) => context.ReadVector4s(destination),
        TryPeekSpan = (ReadContext context, Span<Vector4> destination) => context.TryPeekVector4s(destination),
        TryReadSpan = (ReadContext context, Span<Vector4> destination) => context.TryReadVector4s(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Vector4> values) => context.WriteVector4sWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.PeekVector4s(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.ReadVector4s(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.TryPeekVector4s(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.TryReadVector4s(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.PeekVector4sWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.ReadVector4sWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.TryPeekVector4sWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.TryReadVector4sWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, Vector4[] values) => context.WriteVector4s(values),
        PeekArray = (ReadContext context) => context.PeekVector4s(),
        ReadArray = (ReadContext context) => context.ReadVector4s(),
        TryPeekArray = (ReadContext context, out Vector4[] values) => context.TryPeekVector4s(out values),
        TryReadArray = (ReadContext context, out Vector4[] values) => context.TryReadVector4s(out values),
        WriteArrayWithoutLength = (ref WriteContext context, Vector4[] values) => context.WriteVector4sWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekVector4s(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadVector4s(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Vector4[] values) => context.TryPeekVector4s(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Vector4[] values) => context.TryReadVector4s(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekVector4sWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadVector4sWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Vector4[] values) => context.TryPeekVector4sWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Vector4[] values) => context.TryReadVector4sWithMaxCount(maxCount, out values),
    };
}

public class QuaternionTests : StructTestSuite<Quaternion> {
    protected override Quaternion Value => new(1.0f, 2.0f, 3.0f, 4.0f);

    protected override Quaternion[] Values => [
        new(1.0f, 2.0f, 3.0f, 4.0f),
        new(5.0f, 6.0f, 7.0f, 8.0f),
        new(9.0f, 10.0f, 11.0f, 12.0f)
    ];

    protected override int? ExpectedFixedSizeBits => 128;

    protected override Type StructType => typeof(QuaternionProxy);

    protected override SerializationOperations<Quaternion> Operations { get; } = new() {
        Write = (ref WriteContext context, Quaternion value) => context.WriteQuaternion(value),
        Peek = (ReadContext context) => context.PeekQuaternion(),
        Read = (ReadContext context) => context.ReadQuaternion(),
        TryPeek = (ReadContext context, out Quaternion value) => context.TryPeekQuaternion(out value),
        TryRead = (ReadContext context, out Quaternion value) => context.TryReadQuaternion(out value),
        WriteSpan = (ref WriteContext context, Span<Quaternion> values) => context.WriteQuaternions(values),
        PeekSpan = (ReadContext context, Span<Quaternion> destination) => context.PeekQuaternions(destination),
        ReadSpan = (ReadContext context, Span<Quaternion> destination) => context.ReadQuaternions(destination),
        TryPeekSpan = (ReadContext context, Span<Quaternion> destination) => context.TryPeekQuaternions(destination),
        TryReadSpan = (ReadContext context, Span<Quaternion> destination) => context.TryReadQuaternions(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Quaternion> values) => context.WriteQuaternionsWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuaternions(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuaternions(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.TryPeekQuaternions(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.TryReadQuaternions(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.PeekQuaternionsWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.ReadQuaternionsWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.TryPeekQuaternionsWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.TryReadQuaternionsWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, Quaternion[] values) => context.WriteQuaternions(values),
        PeekArray = (ReadContext context) => context.PeekQuaternions(),
        ReadArray = (ReadContext context) => context.ReadQuaternions(),
        TryPeekArray = (ReadContext context, out Quaternion[] values) => context.TryPeekQuaternions(out values),
        TryReadArray = (ReadContext context, out Quaternion[] values) => context.TryReadQuaternions(out values),
        WriteArrayWithoutLength = (ref WriteContext context, Quaternion[] values) => context.WriteQuaternionsWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuaternions(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuaternions(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Quaternion[] values) => context.TryPeekQuaternions(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Quaternion[] values) => context.TryReadQuaternions(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuaternionsWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuaternionsWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Quaternion[] values) => context.TryPeekQuaternionsWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Quaternion[] values) => context.TryReadQuaternionsWithMaxCount(maxCount, out values),
    };
}

// ReSharper disable once InconsistentNaming
public class Matrix4x4Tests : StructTestSuite<Matrix4x4> {
    protected override Matrix4x4 Value => new(
        1.0f, 2.0f, 3.0f, 4.0f,
        5.0f, 6.0f, 7.0f, 8.0f,
        9.0f, 10.0f, 11.0f, 12.0f,
        13.0f, 14.0f, 15.0f, 16.0f
    );

    protected override Matrix4x4[] Values => [
        new(
            1.0f, 2.0f, 3.0f, 4.0f,
            5.0f, 6.0f, 7.0f, 8.0f,
            9.0f, 10.0f, 11.0f, 12.0f,
            13.0f, 14.0f, 15.0f, 16.0f
        )
    ];

    protected override int? ExpectedFixedSizeBits => 512;

    protected override Type StructType => typeof(Matrix4x4Proxy);

    protected override SerializationOperations<Matrix4x4> Operations { get; } = new() {
        Write = (ref WriteContext context, Matrix4x4 value) => context.WriteMatrix4x4(value),
        Peek = (ReadContext context) => context.PeekMatrix4x4(),
        Read = (ReadContext context) => context.ReadMatrix4x4(),
        TryPeek = (ReadContext context, out Matrix4x4 value) => context.TryPeekMatrix4x4(out value),
        TryRead = (ReadContext context, out Matrix4x4 value) => context.TryReadMatrix4x4(out value),
        WriteSpan = (ref WriteContext context, Span<Matrix4x4> values) => context.WriteMatrix4x4s(values),
        PeekSpan = (ReadContext context, Span<Matrix4x4> destination) => context.PeekMatrix4x4s(destination),
        ReadSpan = (ReadContext context, Span<Matrix4x4> destination) => context.ReadMatrix4x4s(destination),
        TryPeekSpan = (ReadContext context, Span<Matrix4x4> destination) => context.TryPeekMatrix4x4s(destination),
        TryReadSpan = (ReadContext context, Span<Matrix4x4> destination) => context.TryReadMatrix4x4s(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Matrix4x4> values) => context.WriteMatrix4x4sWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekMatrix4x4s(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadMatrix4x4s(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.TryPeekMatrix4x4s(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.TryReadMatrix4x4s(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.PeekMatrix4x4sWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.ReadMatrix4x4sWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.TryPeekMatrix4x4sWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.TryReadMatrix4x4sWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, Matrix4x4[] values) => context.WriteMatrix4x4s(values),
        PeekArray = (ReadContext context) => context.PeekMatrix4x4s(),
        ReadArray = (ReadContext context) => context.ReadMatrix4x4s(),
        TryPeekArray = (ReadContext context, out Matrix4x4[] values) => context.TryPeekMatrix4x4s(out values),
        TryReadArray = (ReadContext context, out Matrix4x4[] values) => context.TryReadMatrix4x4s(out values),
        WriteArrayWithoutLength = (ref WriteContext context, Matrix4x4[] values) => context.WriteMatrix4x4sWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekMatrix4x4s(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadMatrix4x4s(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Matrix4x4[] values) => context.TryPeekMatrix4x4s(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Matrix4x4[] values) => context.TryReadMatrix4x4s(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekMatrix4x4sWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadMatrix4x4sWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Matrix4x4[] values) => context.TryPeekMatrix4x4sWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Matrix4x4[] values) => context.TryReadMatrix4x4sWithMaxCount(maxCount, out values),
    };
}

public class PlaneTests : StructTestSuite<Plane> {
    protected override Plane Value => new(new Vector3(0.0f, 1.0f, 0.0f), 5.0f);

    protected override Plane[] Values => [
        new(new Vector3(0.0f, 1.0f, 0.0f), 1.0f),
        new(new Vector3(1.0f, 0.0f, 0.0f), 2.0f),
        new(new Vector3(0.0f, 0.0f, 1.0f), 3.0f)
    ];

    protected override int? ExpectedFixedSizeBits => 128;

    protected override Type StructType => typeof(PlaneProxy);

    protected override SerializationOperations<Plane> Operations { get; } = new() {
        Write = (ref WriteContext context, Plane value) => context.WritePlane(value),
        Peek = (ReadContext context) => context.PeekPlane(),
        Read = (ReadContext context) => context.ReadPlane(),
        TryPeek = (ReadContext context, out Plane value) => context.TryPeekPlane(out value),
        TryRead = (ReadContext context, out Plane value) => context.TryReadPlane(out value),
        WriteSpan = (ref WriteContext context, Span<Plane> values) => context.WritePlanes(values),
        PeekSpan = (ReadContext context, Span<Plane> destination) => context.PeekPlanes(destination),
        ReadSpan = (ReadContext context, Span<Plane> destination) => context.ReadPlanes(destination),
        TryPeekSpan = (ReadContext context, Span<Plane> destination) => context.TryPeekPlanes(destination),
        TryReadSpan = (ReadContext context, Span<Plane> destination) => context.TryReadPlanes(destination),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Plane> values) => context.WritePlanesWithoutLength(values),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.PeekPlanes(count, destination),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.ReadPlanes(count, destination),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.TryPeekPlanes(count, destination),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.TryReadPlanes(count, destination),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.PeekPlanesWithMaxCount(maxCount, destination),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.ReadPlanesWithMaxCount(maxCount, destination),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.TryPeekPlanesWithMaxCount(maxCount, destination),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.TryReadPlanesWithMaxCount(maxCount, destination),
        WriteArray = (ref WriteContext context, Plane[] values) => context.WritePlanes(values),
        PeekArray = (ReadContext context) => context.PeekPlanes(),
        ReadArray = (ReadContext context) => context.ReadPlanes(),
        TryPeekArray = (ReadContext context, out Plane[] values) => context.TryPeekPlanes(out values),
        TryReadArray = (ReadContext context, out Plane[] values) => context.TryReadPlanes(out values),
        WriteArrayWithoutLength = (ref WriteContext context, Plane[] values) => context.WritePlanesWithoutLength(values),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekPlanes(count),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadPlanes(count),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Plane[] values) => context.TryPeekPlanes(count, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Plane[] values) => context.TryReadPlanes(count, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekPlanesWithMaxCount(maxCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadPlanesWithMaxCount(maxCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Plane[] values) => context.TryPeekPlanesWithMaxCount(maxCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Plane[] values) => context.TryReadPlanesWithMaxCount(maxCount, out values),
    };
}
