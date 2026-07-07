namespace ComputerysBitStream.Tests.Extras;

public class Vector2Tests : StructTestSuite<Vector2> {
    protected override Vector2 Value => new(1.0f, 2.0f);

    protected override Vector2[] Values => [
        new(1.0f, 2.0f),
        new(3.0f, 4.0f),
        new(5.0f, 6.0f)
    ];

    protected override int? ExpectedFixedSizeBits => 64;

    protected override void Write(ref WriteContext context, Vector2 value) => context.WriteVector2(value);
    protected override Vector2 Peek(ReadContext context) => context.PeekVector2();
    protected override Vector2 Read(ReadContext context) => context.ReadVector2();

    protected override Vector2 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVector2(out Vector2 v));
        return v;
    }

    protected override Vector2 TryRead(ReadContext context) {
        Assert.True(context.TryReadVector2(out Vector2 v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, Vector2[] values) => context.WriteVector2s(values);
    protected override Vector2[] PeekArrayWithLength(ReadContext context) => context.PeekVector2s();
    protected override Vector2[] ReadArrayWithLength(ReadContext context) => context.ReadVector2s();

    protected override Vector2[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVector2s(out Vector2[] v));
        return v;
    }

    protected override Vector2[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVector2s(out Vector2[] v));
        return v;
    }

    protected override Vector2[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekVector2sWithMaxCount(maxCount);
    protected override Vector2[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadVector2sWithMaxCount(maxCount);

    protected override Vector2[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekVector2sWithMaxCount(maxCount, out Vector2[] values));
        return values;
    }

    protected override Vector2[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadVector2sWithMaxCount(maxCount, out Vector2[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector2> destination) => context.PeekVector2sWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector2> destination) => context.ReadVector2sWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector2> destination) { Assert.True(context.TryPeekVector2sWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector2> destination) { Assert.True(context.TryReadVector2sWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, Vector2[] values) => context.WriteVector2sWithoutLength(values);
    protected override Vector2[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVector2s(count);
    protected override Vector2[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVector2s(count);

    protected override Vector2[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVector2s(count, out Vector2[] v));
        return v;
    }

    protected override Vector2[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVector2s(count, out Vector2[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<Vector2> values) => context.WriteVector2s(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<Vector2> destination) => context.PeekVector2s(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<Vector2> destination) => context.ReadVector2s(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryPeekVector2s(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryReadVector2s(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Vector2> values) => context.WriteVector2sWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) => context.PeekVector2s(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) => context.ReadVector2s(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryPeekVector2s(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryReadVector2s(count, destination)); }

    protected override Type StructType => typeof(Vector2Proxy);

    protected override TryReadOperationSet<Vector2> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Vector2 v) => c.TryPeekVector2(out v),
        TryReadValue = (ReadContext c, out Vector2 v) => c.TryReadVector2(out v),
        TryPeekArrayWithLength = (ReadContext c, out Vector2[] v) => c.TryPeekVector2s(out v),
        TryReadArrayWithLength = (ReadContext c, out Vector2[] v) => c.TryReadVector2s(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Vector2[] v) => c.TryPeekVector2s(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Vector2[] v) => c.TryReadVector2s(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Vector2> d) => c.TryPeekVector2s(d),
        TryReadSpanWithLength = (ReadContext c, Span<Vector2> d) => c.TryReadVector2s(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Vector2> d) => c.TryPeekVector2s(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Vector2> d) => c.TryReadVector2s(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out Vector2[] v) => c.TryPeekVector2sWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out Vector2[] v) => c.TryReadVector2sWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<Vector2> d) => c.TryPeekVector2sWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<Vector2> d) => c.TryReadVector2sWithMaxCount(maxCount, d),
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

    protected override void Write(ref WriteContext context, Vector3 value) => context.WriteVector3(value);
    protected override Vector3 Peek(ReadContext context) => context.PeekVector3();
    protected override Vector3 Read(ReadContext context) => context.ReadVector3();

    protected override Vector3 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVector3(out Vector3 v));
        return v;
    }

    protected override Vector3 TryRead(ReadContext context) {
        Assert.True(context.TryReadVector3(out Vector3 v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, Vector3[] values) => context.WriteVector3s(values);
    protected override Vector3[] PeekArrayWithLength(ReadContext context) => context.PeekVector3s();
    protected override Vector3[] ReadArrayWithLength(ReadContext context) => context.ReadVector3s();

    protected override Vector3[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVector3s(out Vector3[] v));
        return v;
    }

    protected override Vector3[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVector3s(out Vector3[] v));
        return v;
    }

    protected override Vector3[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekVector3sWithMaxCount(maxCount);
    protected override Vector3[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadVector3sWithMaxCount(maxCount);

    protected override Vector3[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekVector3sWithMaxCount(maxCount, out Vector3[] values));
        return values;
    }

    protected override Vector3[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadVector3sWithMaxCount(maxCount, out Vector3[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector3> destination) => context.PeekVector3sWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector3> destination) => context.ReadVector3sWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector3> destination) { Assert.True(context.TryPeekVector3sWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector3> destination) { Assert.True(context.TryReadVector3sWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, Vector3[] values) => context.WriteVector3sWithoutLength(values);
    protected override Vector3[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVector3s(count);
    protected override Vector3[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVector3s(count);

    protected override Vector3[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVector3s(count, out Vector3[] v));
        return v;
    }

    protected override Vector3[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVector3s(count, out Vector3[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<Vector3> values) => context.WriteVector3s(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<Vector3> destination) => context.PeekVector3s(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<Vector3> destination) => context.ReadVector3s(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryPeekVector3s(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryReadVector3s(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Vector3> values) => context.WriteVector3sWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) => context.PeekVector3s(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) => context.ReadVector3s(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryPeekVector3s(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryReadVector3s(count, destination)); }

    protected override Type StructType => typeof(Vector3Proxy);

    protected override TryReadOperationSet<Vector3> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Vector3 v) => c.TryPeekVector3(out v),
        TryReadValue = (ReadContext c, out Vector3 v) => c.TryReadVector3(out v),
        TryPeekArrayWithLength = (ReadContext c, out Vector3[] v) => c.TryPeekVector3s(out v),
        TryReadArrayWithLength = (ReadContext c, out Vector3[] v) => c.TryReadVector3s(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Vector3[] v) => c.TryPeekVector3s(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Vector3[] v) => c.TryReadVector3s(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Vector3> d) => c.TryPeekVector3s(d),
        TryReadSpanWithLength = (ReadContext c, Span<Vector3> d) => c.TryReadVector3s(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Vector3> d) => c.TryPeekVector3s(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Vector3> d) => c.TryReadVector3s(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out Vector3[] v) => c.TryPeekVector3sWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out Vector3[] v) => c.TryReadVector3sWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<Vector3> d) => c.TryPeekVector3sWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<Vector3> d) => c.TryReadVector3sWithMaxCount(maxCount, d),
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

    protected override void Write(ref WriteContext context, Vector4 value) => context.WriteVector4(value);
    protected override Vector4 Peek(ReadContext context) => context.PeekVector4();
    protected override Vector4 Read(ReadContext context) => context.ReadVector4();

    protected override Vector4 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekVector4(out Vector4 v));
        return v;
    }

    protected override Vector4 TryRead(ReadContext context) {
        Assert.True(context.TryReadVector4(out Vector4 v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, Vector4[] values) => context.WriteVector4s(values);
    protected override Vector4[] PeekArrayWithLength(ReadContext context) => context.PeekVector4s();
    protected override Vector4[] ReadArrayWithLength(ReadContext context) => context.ReadVector4s();

    protected override Vector4[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekVector4s(out Vector4[] v));
        return v;
    }

    protected override Vector4[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadVector4s(out Vector4[] v));
        return v;
    }

    protected override Vector4[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekVector4sWithMaxCount(maxCount);
    protected override Vector4[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadVector4sWithMaxCount(maxCount);

    protected override Vector4[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekVector4sWithMaxCount(maxCount, out Vector4[] values));
        return values;
    }

    protected override Vector4[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadVector4sWithMaxCount(maxCount, out Vector4[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector4> destination) => context.PeekVector4sWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector4> destination) => context.ReadVector4sWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector4> destination) { Assert.True(context.TryPeekVector4sWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Vector4> destination) { Assert.True(context.TryReadVector4sWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, Vector4[] values) => context.WriteVector4sWithoutLength(values);
    protected override Vector4[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekVector4s(count);
    protected override Vector4[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadVector4s(count);

    protected override Vector4[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekVector4s(count, out Vector4[] v));
        return v;
    }

    protected override Vector4[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadVector4s(count, out Vector4[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<Vector4> values) => context.WriteVector4s(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<Vector4> destination) => context.PeekVector4s(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<Vector4> destination) => context.ReadVector4s(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryPeekVector4s(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryReadVector4s(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Vector4> values) => context.WriteVector4sWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) => context.PeekVector4s(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) => context.ReadVector4s(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryPeekVector4s(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryReadVector4s(count, destination)); }

    protected override Type StructType => typeof(Vector4Proxy);

    protected override TryReadOperationSet<Vector4> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Vector4 v) => c.TryPeekVector4(out v),
        TryReadValue = (ReadContext c, out Vector4 v) => c.TryReadVector4(out v),
        TryPeekArrayWithLength = (ReadContext c, out Vector4[] v) => c.TryPeekVector4s(out v),
        TryReadArrayWithLength = (ReadContext c, out Vector4[] v) => c.TryReadVector4s(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Vector4[] v) => c.TryPeekVector4s(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Vector4[] v) => c.TryReadVector4s(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Vector4> d) => c.TryPeekVector4s(d),
        TryReadSpanWithLength = (ReadContext c, Span<Vector4> d) => c.TryReadVector4s(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Vector4> d) => c.TryPeekVector4s(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Vector4> d) => c.TryReadVector4s(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out Vector4[] v) => c.TryPeekVector4sWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out Vector4[] v) => c.TryReadVector4sWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<Vector4> d) => c.TryPeekVector4sWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<Vector4> d) => c.TryReadVector4sWithMaxCount(maxCount, d),
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

    protected override void Write(ref WriteContext context, Quaternion value) => context.WriteQuaternion(value);
    protected override Quaternion Peek(ReadContext context) => context.PeekQuaternion();
    protected override Quaternion Read(ReadContext context) => context.ReadQuaternion();

    protected override Quaternion TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuaternion(out Quaternion v));
        return v;
    }

    protected override Quaternion TryRead(ReadContext context) {
        Assert.True(context.TryReadQuaternion(out Quaternion v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, Quaternion[] values) => context.WriteQuaternions(values);
    protected override Quaternion[] PeekArrayWithLength(ReadContext context) => context.PeekQuaternions();
    protected override Quaternion[] ReadArrayWithLength(ReadContext context) => context.ReadQuaternions();

    protected override Quaternion[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuaternions(out Quaternion[] v));
        return v;
    }

    protected override Quaternion[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuaternions(out Quaternion[] v));
        return v;
    }

    protected override Quaternion[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekQuaternionsWithMaxCount(maxCount);
    protected override Quaternion[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadQuaternionsWithMaxCount(maxCount);

    protected override Quaternion[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekQuaternionsWithMaxCount(maxCount, out Quaternion[] values));
        return values;
    }

    protected override Quaternion[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadQuaternionsWithMaxCount(maxCount, out Quaternion[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Quaternion> destination) => context.PeekQuaternionsWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Quaternion> destination) => context.ReadQuaternionsWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Quaternion> destination) { Assert.True(context.TryPeekQuaternionsWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Quaternion> destination) { Assert.True(context.TryReadQuaternionsWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, Quaternion[] values) => context.WriteQuaternionsWithoutLength(values);
    protected override Quaternion[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuaternions(count);
    protected override Quaternion[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuaternions(count);

    protected override Quaternion[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuaternions(count, out Quaternion[] v));
        return v;
    }

    protected override Quaternion[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuaternions(count, out Quaternion[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<Quaternion> values) => context.WriteQuaternions(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<Quaternion> destination) => context.PeekQuaternions(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<Quaternion> destination) => context.ReadQuaternions(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryPeekQuaternions(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryReadQuaternions(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Quaternion> values) => context.WriteQuaternionsWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuaternions(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuaternions(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryPeekQuaternions(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryReadQuaternions(count, destination)); }

    protected override Type StructType => typeof(QuaternionProxy);

    protected override TryReadOperationSet<Quaternion> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Quaternion v) => c.TryPeekQuaternion(out v),
        TryReadValue = (ReadContext c, out Quaternion v) => c.TryReadQuaternion(out v),
        TryPeekArrayWithLength = (ReadContext c, out Quaternion[] v) => c.TryPeekQuaternions(out v),
        TryReadArrayWithLength = (ReadContext c, out Quaternion[] v) => c.TryReadQuaternions(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Quaternion[] v) => c.TryPeekQuaternions(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Quaternion[] v) => c.TryReadQuaternions(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Quaternion> d) => c.TryPeekQuaternions(d),
        TryReadSpanWithLength = (ReadContext c, Span<Quaternion> d) => c.TryReadQuaternions(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Quaternion> d) => c.TryPeekQuaternions(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Quaternion> d) => c.TryReadQuaternions(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out Quaternion[] v) => c.TryPeekQuaternionsWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out Quaternion[] v) => c.TryReadQuaternionsWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<Quaternion> d) => c.TryPeekQuaternionsWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<Quaternion> d) => c.TryReadQuaternionsWithMaxCount(maxCount, d),
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

    protected override void Write(ref WriteContext context, Matrix4x4 value) => context.WriteMatrix4x4(value);
    protected override Matrix4x4 Peek(ReadContext context) => context.PeekMatrix4x4();
    protected override Matrix4x4 Read(ReadContext context) => context.ReadMatrix4x4();

    protected override Matrix4x4 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekMatrix4x4(out Matrix4x4 v));
        return v;
    }

    protected override Matrix4x4 TryRead(ReadContext context) {
        Assert.True(context.TryReadMatrix4x4(out Matrix4x4 v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, Matrix4x4[] values) => context.WriteMatrix4x4s(values);
    protected override Matrix4x4[] PeekArrayWithLength(ReadContext context) => context.PeekMatrix4x4s();
    protected override Matrix4x4[] ReadArrayWithLength(ReadContext context) => context.ReadMatrix4x4s();

    protected override Matrix4x4[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekMatrix4x4s(out Matrix4x4[] v));
        return v;
    }

    protected override Matrix4x4[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadMatrix4x4s(out Matrix4x4[] v));
        return v;
    }

    protected override Matrix4x4[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekMatrix4x4sWithMaxCount(maxCount);
    protected override Matrix4x4[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadMatrix4x4sWithMaxCount(maxCount);

    protected override Matrix4x4[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekMatrix4x4sWithMaxCount(maxCount, out Matrix4x4[] values));
        return values;
    }

    protected override Matrix4x4[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadMatrix4x4sWithMaxCount(maxCount, out Matrix4x4[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.PeekMatrix4x4sWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.ReadMatrix4x4sWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Matrix4x4> destination) { Assert.True(context.TryPeekMatrix4x4sWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Matrix4x4> destination) { Assert.True(context.TryReadMatrix4x4sWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, Matrix4x4[] values) => context.WriteMatrix4x4sWithoutLength(values);
    protected override Matrix4x4[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekMatrix4x4s(count);
    protected override Matrix4x4[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadMatrix4x4s(count);

    protected override Matrix4x4[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekMatrix4x4s(count, out Matrix4x4[] v));
        return v;
    }

    protected override Matrix4x4[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadMatrix4x4s(count, out Matrix4x4[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<Matrix4x4> values) => context.WriteMatrix4x4s(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<Matrix4x4> destination) => context.PeekMatrix4x4s(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<Matrix4x4> destination) => context.ReadMatrix4x4s(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryPeekMatrix4x4s(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryReadMatrix4x4s(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Matrix4x4> values) => context.WriteMatrix4x4sWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekMatrix4x4s(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadMatrix4x4s(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryPeekMatrix4x4s(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryReadMatrix4x4s(count, destination)); }

    protected override Type StructType => typeof(Matrix4x4Proxy);

    protected override TryReadOperationSet<Matrix4x4> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Matrix4x4 v) => c.TryPeekMatrix4x4(out v),
        TryReadValue = (ReadContext c, out Matrix4x4 v) => c.TryReadMatrix4x4(out v),
        TryPeekArrayWithLength = (ReadContext c, out Matrix4x4[] v) => c.TryPeekMatrix4x4s(out v),
        TryReadArrayWithLength = (ReadContext c, out Matrix4x4[] v) => c.TryReadMatrix4x4s(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Matrix4x4[] v) => c.TryPeekMatrix4x4s(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Matrix4x4[] v) => c.TryReadMatrix4x4s(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Matrix4x4> d) => c.TryPeekMatrix4x4s(d),
        TryReadSpanWithLength = (ReadContext c, Span<Matrix4x4> d) => c.TryReadMatrix4x4s(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Matrix4x4> d) => c.TryPeekMatrix4x4s(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Matrix4x4> d) => c.TryReadMatrix4x4s(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out Matrix4x4[] v) => c.TryPeekMatrix4x4sWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out Matrix4x4[] v) => c.TryReadMatrix4x4sWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<Matrix4x4> d) => c.TryPeekMatrix4x4sWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<Matrix4x4> d) => c.TryReadMatrix4x4sWithMaxCount(maxCount, d),
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

    protected override void Write(ref WriteContext context, Plane value) => context.WritePlane(value);
    protected override Plane Peek(ReadContext context) => context.PeekPlane();
    protected override Plane Read(ReadContext context) => context.ReadPlane();

    protected override Plane TryPeek(ReadContext context) {
        Assert.True(context.TryPeekPlane(out Plane v));
        return v;
    }

    protected override Plane TryRead(ReadContext context) {
        Assert.True(context.TryReadPlane(out Plane v));
        return v;
    }

    protected override void WriteArray(ref WriteContext context, Plane[] values) => context.WritePlanes(values);
    protected override Plane[] PeekArrayWithLength(ReadContext context) => context.PeekPlanes();
    protected override Plane[] ReadArrayWithLength(ReadContext context) => context.ReadPlanes();

    protected override Plane[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekPlanes(out Plane[] v));
        return v;
    }

    protected override Plane[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadPlanes(out Plane[] v));
        return v;
    }

    protected override Plane[] PeekArrayWithMaxCount(ReadContext context, int maxCount) => context.PeekPlanesWithMaxCount(maxCount);
    protected override Plane[] ReadArrayWithMaxCount(ReadContext context, int maxCount) => context.ReadPlanesWithMaxCount(maxCount);

    protected override Plane[] TryPeekArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryPeekPlanesWithMaxCount(maxCount, out Plane[] values));
        return values;
    }

    protected override Plane[] TryReadArrayWithMaxCount(ReadContext context, int maxCount) {
        Assert.True(context.TryReadPlanesWithMaxCount(maxCount, out Plane[] values));
        return values;
    }

    protected override void PeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Plane> destination) => context.PeekPlanesWithMaxCount(maxCount, destination);
    protected override void ReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Plane> destination) => context.ReadPlanesWithMaxCount(maxCount, destination);
    protected override void TryPeekSpanWithMaxCount(ReadContext context, int maxCount, Span<Plane> destination) { Assert.True(context.TryPeekPlanesWithMaxCount(maxCount, destination)); }
    protected override void TryReadSpanWithMaxCount(ReadContext context, int maxCount, Span<Plane> destination) { Assert.True(context.TryReadPlanesWithMaxCount(maxCount, destination)); }

    protected override void WriteArrayWithoutLength(ref WriteContext context, Plane[] values) => context.WritePlanesWithoutLength(values);
    protected override Plane[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekPlanes(count);
    protected override Plane[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadPlanes(count);

    protected override Plane[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekPlanes(count, out Plane[] v));
        return v;
    }

    protected override Plane[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadPlanes(count, out Plane[] v));
        return v;
    }

    protected override void WriteSpan(ref WriteContext context, Span<Plane> values) => context.WritePlanes(values);
    protected override void PeekSpanWithLength(ReadContext context, Span<Plane> destination) => context.PeekPlanes(destination);
    protected override void ReadSpanWithLength(ReadContext context, Span<Plane> destination) => context.ReadPlanes(destination);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Plane> destination) { Assert.True(context.TryPeekPlanes(destination)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Plane> destination) { Assert.True(context.TryReadPlanes(destination)); }

    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Plane> values) => context.WritePlanesWithoutLength(values);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) => context.PeekPlanes(count, destination);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) => context.ReadPlanes(count, destination);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryPeekPlanes(count, destination)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryReadPlanes(count, destination)); }

    protected override Type StructType => typeof(PlaneProxy);

    protected override TryReadOperationSet<Plane> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Plane v) => c.TryPeekPlane(out v),
        TryReadValue = (ReadContext c, out Plane v) => c.TryReadPlane(out v),
        TryPeekArrayWithLength = (ReadContext c, out Plane[] v) => c.TryPeekPlanes(out v),
        TryReadArrayWithLength = (ReadContext c, out Plane[] v) => c.TryReadPlanes(out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Plane[] v) => c.TryPeekPlanes(count, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Plane[] v) => c.TryReadPlanes(count, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Plane> d) => c.TryPeekPlanes(d),
        TryReadSpanWithLength = (ReadContext c, Span<Plane> d) => c.TryReadPlanes(d),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Plane> d) => c.TryPeekPlanes(count, d),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Plane> d) => c.TryReadPlanes(count, d),
        TryPeekArrayWithMaxCount = (ReadContext c, int maxCount, out Plane[] v) => c.TryPeekPlanesWithMaxCount(maxCount, out v),
        TryReadArrayWithMaxCount = (ReadContext c, int maxCount, out Plane[] v) => c.TryReadPlanesWithMaxCount(maxCount, out v),
        TryPeekSpanWithMaxCount = (ReadContext c, int maxCount, Span<Plane> d) => c.TryPeekPlanesWithMaxCount(maxCount, d),
        TryReadSpanWithMaxCount = (ReadContext c, int maxCount, Span<Plane> d) => c.TryReadPlanesWithMaxCount(maxCount, d),
    };
}
