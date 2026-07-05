namespace ComputerysBitStream.Tests.Extras;

public abstract class ExtrasQuantizedNumericsTestSuite<T> : QuantizedExtensionTestSuite<T> {
    protected override bool SupportsOutOfBoundsTests => false;
}

[BitStreamPrimitiveContext]
public class QuantizedVector2ExtensionsTests : ExtrasQuantizedNumericsTestSuite<Vector2> {
    private const float Min = 0f;
    private const float Max = 100f;
    private static readonly Vector2 MinRange = new(Min, Min);
    private static readonly Vector2 MaxRange = new(Max, Max);
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override Vector2 Value => new(50f, 50f);
    protected override Vector2[] Values => [new(0f, 0f), new(50f, 50f), new(100f, 100f)];

    protected override void AssertValuesEqual(Vector2 expected, Vector2 actual) {
        Assert.Equal(expected.X, actual.X, Precision);
        Assert.Equal(expected.Y, actual.Y, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, Vector2 value) => context.WriteQuantizedVector2Primitive(value, Min, Max, BitCount);
    protected override Vector2 PeekPrimitive(ReadContext context) => context.PeekQuantizedVector2Primitive(MinRange, MaxRange, BitCount);
    protected override Vector2 ReadPrimitive(ReadContext context) => context.ReadQuantizedVector2Primitive(MinRange, MaxRange, BitCount);
    protected override void Write(ref WriteContext context, Vector2 value) => context.WriteQuantizedVector2(value, MinRange, MaxRange, BitCount);
    protected override Vector2 Peek(ReadContext context) => context.PeekQuantizedVector2(MinRange, MaxRange, BitCount);
    protected override Vector2 Read(ReadContext context) => context.ReadQuantizedVector2(MinRange, MaxRange, BitCount);

    protected override Vector2 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedVector2(MinRange, MaxRange, BitCount, out Vector2 v));
        return v;
    }

    protected override Vector2 TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedVector2(MinRange, MaxRange, BitCount, out Vector2 v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<Vector2> values) => context.WriteQuantizedVector2sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<Vector2> destination) => context.PeekQuantizedVector2SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<Vector2> destination) => context.ReadQuantizedVector2SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Vector2> values) => context.WriteQuantizedVector2sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) => context.PeekQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) => context.ReadQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryPeekQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryReadQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<Vector2> values) => context.WriteQuantizedVector2s(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<Vector2> destination) => context.PeekQuantizedVector2s(destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<Vector2> destination) => context.ReadQuantizedVector2s(destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryPeekQuantizedVector2s(destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryReadQuantizedVector2s(destination, MinRange, MaxRange, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, Vector2[] values) => context.WriteQuantizedVector2sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override Vector2[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedVector2ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override Vector2[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedVector2ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, Vector2[] values) => context.WriteQuantizedVector2sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override Vector2[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedVector2s(count, MinRange, MaxRange, BitCount);
    protected override Vector2[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedVector2s(count, MinRange, MaxRange, BitCount);

    protected override Vector2[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedVector2s(count, MinRange, MaxRange, BitCount, out Vector2[] values));
        return values;
    }

    protected override Vector2[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedVector2s(count, MinRange, MaxRange, BitCount, out Vector2[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, Vector2[] values) => context.WriteQuantizedVector2s(values, MinRange, MaxRange, BitCount);
    protected override Vector2[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedVector2s(MinRange, MaxRange, BitCount);
    protected override Vector2[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedVector2s(MinRange, MaxRange, BitCount);

    protected override Vector2[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedVector2s(MinRange, MaxRange, BitCount, out Vector2[] values));
        return values;
    }

    protected override Vector2[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedVector2s(MinRange, MaxRange, BitCount, out Vector2[] values));
        return values;
    }

    protected override TryReadOperationSet<Vector2> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Vector2 v) => c.TryPeekQuantizedVector2(MinRange, MaxRange, BitCount, out v),
        TryReadValue = (ReadContext c, out Vector2 v) => c.TryReadQuantizedVector2(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out Vector2[] v) => c.TryPeekQuantizedVector2s(MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out Vector2[] v) => c.TryReadQuantizedVector2s(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Vector2[] v) => c.TryPeekQuantizedVector2s(count, MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Vector2[] v) => c.TryReadQuantizedVector2s(count, MinRange, MaxRange, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Vector2> d) => c.TryPeekQuantizedVector2s(d, MinRange, MaxRange, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<Vector2> d) => c.TryReadQuantizedVector2s(d, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Vector2> d) => c.TryPeekQuantizedVector2s(count, d, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Vector2> d) => c.TryReadQuantizedVector2s(count, d, MinRange, MaxRange, BitCount),
    };
}

[BitStreamPrimitiveContext]
public class QuantizedVector3ExtensionsTests : ExtrasQuantizedNumericsTestSuite<Vector3> {
    private const float Min = 0f;
    private const float Max = 100f;
    private static readonly Vector3 MinRange = new(Min, Min, Min);
    private static readonly Vector3 MaxRange = new(Max, Max, Max);
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override Vector3 Value => new(50f, 50f, 50f);
    protected override Vector3[] Values => [new(0f, 0f, 0f), new(50f, 50f, 50f), new(100f, 100f, 100f)];

    protected override void AssertValuesEqual(Vector3 expected, Vector3 actual) {
        Assert.Equal(expected.X, actual.X, Precision);
        Assert.Equal(expected.Y, actual.Y, Precision);
        Assert.Equal(expected.Z, actual.Z, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, Vector3 value) => context.WriteQuantizedVector3Primitive(value, Min, Max, BitCount);
    protected override Vector3 PeekPrimitive(ReadContext context) => context.PeekQuantizedVector3Primitive(MinRange, MaxRange, BitCount);
    protected override Vector3 ReadPrimitive(ReadContext context) => context.ReadQuantizedVector3Primitive(MinRange, MaxRange, BitCount);
    protected override void Write(ref WriteContext context, Vector3 value) => context.WriteQuantizedVector3(value, MinRange, MaxRange, BitCount);
    protected override Vector3 Peek(ReadContext context) => context.PeekQuantizedVector3(MinRange, MaxRange, BitCount);
    protected override Vector3 Read(ReadContext context) => context.ReadQuantizedVector3(MinRange, MaxRange, BitCount);

    protected override Vector3 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedVector3(MinRange, MaxRange, BitCount, out Vector3 v));
        return v;
    }

    protected override Vector3 TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedVector3(MinRange, MaxRange, BitCount, out Vector3 v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<Vector3> values) => context.WriteQuantizedVector3sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<Vector3> destination) => context.PeekQuantizedVector3SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<Vector3> destination) => context.ReadQuantizedVector3SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Vector3> values) => context.WriteQuantizedVector3sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) => context.PeekQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) => context.ReadQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryPeekQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryReadQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<Vector3> values) => context.WriteQuantizedVector3s(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<Vector3> destination) => context.PeekQuantizedVector3s(destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<Vector3> destination) => context.ReadQuantizedVector3s(destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryPeekQuantizedVector3s(destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryReadQuantizedVector3s(destination, MinRange, MaxRange, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, Vector3[] values) => context.WriteQuantizedVector3sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override Vector3[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedVector3ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override Vector3[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedVector3ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, Vector3[] values) => context.WriteQuantizedVector3sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override Vector3[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedVector3s(count, MinRange, MaxRange, BitCount);
    protected override Vector3[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedVector3s(count, MinRange, MaxRange, BitCount);

    protected override Vector3[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedVector3s(count, MinRange, MaxRange, BitCount, out Vector3[] values));
        return values;
    }

    protected override Vector3[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedVector3s(count, MinRange, MaxRange, BitCount, out Vector3[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, Vector3[] values) => context.WriteQuantizedVector3s(values, MinRange, MaxRange, BitCount);
    protected override Vector3[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedVector3s(MinRange, MaxRange, BitCount);
    protected override Vector3[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedVector3s(MinRange, MaxRange, BitCount);

    protected override Vector3[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedVector3s(MinRange, MaxRange, BitCount, out Vector3[] values));
        return values;
    }

    protected override Vector3[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedVector3s(MinRange, MaxRange, BitCount, out Vector3[] values));
        return values;
    }

    protected override TryReadOperationSet<Vector3> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Vector3 v) => c.TryPeekQuantizedVector3(MinRange, MaxRange, BitCount, out v),
        TryReadValue = (ReadContext c, out Vector3 v) => c.TryReadQuantizedVector3(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out Vector3[] v) => c.TryPeekQuantizedVector3s(MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out Vector3[] v) => c.TryReadQuantizedVector3s(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Vector3[] v) => c.TryPeekQuantizedVector3s(count, MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Vector3[] v) => c.TryReadQuantizedVector3s(count, MinRange, MaxRange, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Vector3> d) => c.TryPeekQuantizedVector3s(d, MinRange, MaxRange, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<Vector3> d) => c.TryReadQuantizedVector3s(d, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Vector3> d) => c.TryPeekQuantizedVector3s(count, d, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Vector3> d) => c.TryReadQuantizedVector3s(count, d, MinRange, MaxRange, BitCount),
    };
}

[BitStreamPrimitiveContext]
public class QuantizedVector4ExtensionsTests : ExtrasQuantizedNumericsTestSuite<Vector4> {
    private const float Min = 0f;
    private const float Max = 100f;
    private static readonly Vector4 MinRange = new(Min, Min, Min, Min);
    private static readonly Vector4 MaxRange = new(Max, Max, Max, Max);
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override Vector4 Value => new(50f, 50f, 50f, 50f);
    protected override Vector4[] Values => [new(0f, 0f, 0f, 0f), new(50f, 50f, 50f, 50f), new(100f, 100f, 100f, 100f)];

    protected override void AssertValuesEqual(Vector4 expected, Vector4 actual) {
        Assert.Equal(expected.X, actual.X, Precision);
        Assert.Equal(expected.Y, actual.Y, Precision);
        Assert.Equal(expected.Z, actual.Z, Precision);
        Assert.Equal(expected.W, actual.W, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, Vector4 value) => context.WriteQuantizedVector4Primitive(value, Min, Max, BitCount);
    protected override Vector4 PeekPrimitive(ReadContext context) => context.PeekQuantizedVector4Primitive(MinRange, MaxRange, BitCount);
    protected override Vector4 ReadPrimitive(ReadContext context) => context.ReadQuantizedVector4Primitive(MinRange, MaxRange, BitCount);
    protected override void Write(ref WriteContext context, Vector4 value) => context.WriteQuantizedVector4(value, MinRange, MaxRange, BitCount);
    protected override Vector4 Peek(ReadContext context) => context.PeekQuantizedVector4(MinRange, MaxRange, BitCount);
    protected override Vector4 Read(ReadContext context) => context.ReadQuantizedVector4(MinRange, MaxRange, BitCount);

    protected override Vector4 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedVector4(MinRange, MaxRange, BitCount, out Vector4 v));
        return v;
    }

    protected override Vector4 TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedVector4(MinRange, MaxRange, BitCount, out Vector4 v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<Vector4> values) => context.WriteQuantizedVector4sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<Vector4> destination) => context.PeekQuantizedVector4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<Vector4> destination) => context.ReadQuantizedVector4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Vector4> values) => context.WriteQuantizedVector4sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) => context.PeekQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) => context.ReadQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryPeekQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryReadQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<Vector4> values) => context.WriteQuantizedVector4s(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<Vector4> destination) => context.PeekQuantizedVector4s(destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<Vector4> destination) => context.ReadQuantizedVector4s(destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryPeekQuantizedVector4s(destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryReadQuantizedVector4s(destination, MinRange, MaxRange, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, Vector4[] values) => context.WriteQuantizedVector4sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override Vector4[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedVector4ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override Vector4[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedVector4ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, Vector4[] values) => context.WriteQuantizedVector4sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override Vector4[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedVector4s(count, MinRange, MaxRange, BitCount);
    protected override Vector4[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedVector4s(count, MinRange, MaxRange, BitCount);

    protected override Vector4[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedVector4s(count, MinRange, MaxRange, BitCount, out Vector4[] values));
        return values;
    }

    protected override Vector4[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedVector4s(count, MinRange, MaxRange, BitCount, out Vector4[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, Vector4[] values) => context.WriteQuantizedVector4s(values, MinRange, MaxRange, BitCount);
    protected override Vector4[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedVector4s(MinRange, MaxRange, BitCount);
    protected override Vector4[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedVector4s(MinRange, MaxRange, BitCount);

    protected override Vector4[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedVector4s(MinRange, MaxRange, BitCount, out Vector4[] values));
        return values;
    }

    protected override Vector4[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedVector4s(MinRange, MaxRange, BitCount, out Vector4[] values));
        return values;
    }

    protected override TryReadOperationSet<Vector4> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Vector4 v) => c.TryPeekQuantizedVector4(MinRange, MaxRange, BitCount, out v),
        TryReadValue = (ReadContext c, out Vector4 v) => c.TryReadQuantizedVector4(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out Vector4[] v) => c.TryPeekQuantizedVector4s(MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out Vector4[] v) => c.TryReadQuantizedVector4s(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Vector4[] v) => c.TryPeekQuantizedVector4s(count, MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Vector4[] v) => c.TryReadQuantizedVector4s(count, MinRange, MaxRange, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Vector4> d) => c.TryPeekQuantizedVector4s(d, MinRange, MaxRange, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<Vector4> d) => c.TryReadQuantizedVector4s(d, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Vector4> d) => c.TryPeekQuantizedVector4s(count, d, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Vector4> d) => c.TryReadQuantizedVector4s(count, d, MinRange, MaxRange, BitCount),
    };
}

[BitStreamPrimitiveContext]
public class QuantizedQuaternionExtensionsTests : ExtrasQuantizedNumericsTestSuite<Quaternion> {
    private const float Min = 0f;
    private const float Max = 100f;
    private static readonly Quaternion MinRange = new(Min, Min, Min, Min);
    private static readonly Quaternion MaxRange = new(Max, Max, Max, Max);
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override Quaternion Value => new(50f, 50f, 50f, 50f);
    protected override Quaternion[] Values => [new(0f, 0f, 0f, 0f), new(50f, 50f, 50f, 50f), new(100f, 100f, 100f, 100f)];

    protected override void AssertValuesEqual(Quaternion expected, Quaternion actual) {
        Assert.Equal(expected.X, actual.X, Precision);
        Assert.Equal(expected.Y, actual.Y, Precision);
        Assert.Equal(expected.Z, actual.Z, Precision);
        Assert.Equal(expected.W, actual.W, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, Quaternion value) => context.WriteQuantizedQuaternionPrimitive(value, Min, Max, BitCount);
    protected override Quaternion PeekPrimitive(ReadContext context) => context.PeekQuantizedQuaternionPrimitive(MinRange, MaxRange, BitCount);
    protected override Quaternion ReadPrimitive(ReadContext context) => context.ReadQuantizedQuaternionPrimitive(MinRange, MaxRange, BitCount);
    protected override void Write(ref WriteContext context, Quaternion value) => context.WriteQuantizedQuaternion(value, MinRange, MaxRange, BitCount);
    protected override Quaternion Peek(ReadContext context) => context.PeekQuantizedQuaternion(MinRange, MaxRange, BitCount);
    protected override Quaternion Read(ReadContext context) => context.ReadQuantizedQuaternion(MinRange, MaxRange, BitCount);

    protected override Quaternion TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedQuaternion(MinRange, MaxRange, BitCount, out Quaternion v));
        return v;
    }

    protected override Quaternion TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedQuaternion(MinRange, MaxRange, BitCount, out Quaternion v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<Quaternion> values) => context.WriteQuantizedQuaternionsPrimitive(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuantizedQuaternionSpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuantizedQuaternionSpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Quaternion> values) => context.WriteQuantizedQuaternionsWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryPeekQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryReadQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<Quaternion> values) => context.WriteQuantizedQuaternions(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<Quaternion> destination) => context.PeekQuantizedQuaternions(destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<Quaternion> destination) => context.ReadQuantizedQuaternions(destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryPeekQuantizedQuaternions(destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryReadQuantizedQuaternions(destination, MinRange, MaxRange, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, Quaternion[] values) => context.WriteQuantizedQuaternionsPrimitive(values, MinRange, MaxRange, BitCount);
    protected override Quaternion[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedQuaternionArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override Quaternion[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedQuaternionArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, Quaternion[] values) => context.WriteQuantizedQuaternionsWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override Quaternion[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedQuaternions(count, MinRange, MaxRange, BitCount);
    protected override Quaternion[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedQuaternions(count, MinRange, MaxRange, BitCount);

    protected override Quaternion[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedQuaternions(count, MinRange, MaxRange, BitCount, out Quaternion[] values));
        return values;
    }

    protected override Quaternion[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedQuaternions(count, MinRange, MaxRange, BitCount, out Quaternion[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, Quaternion[] values) => context.WriteQuantizedQuaternions(values, MinRange, MaxRange, BitCount);
    protected override Quaternion[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedQuaternions(MinRange, MaxRange, BitCount);
    protected override Quaternion[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedQuaternions(MinRange, MaxRange, BitCount);

    protected override Quaternion[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedQuaternions(MinRange, MaxRange, BitCount, out Quaternion[] values));
        return values;
    }

    protected override Quaternion[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedQuaternions(MinRange, MaxRange, BitCount, out Quaternion[] values));
        return values;
    }

    protected override TryReadOperationSet<Quaternion> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Quaternion v) => c.TryPeekQuantizedQuaternion(MinRange, MaxRange, BitCount, out v),
        TryReadValue = (ReadContext c, out Quaternion v) => c.TryReadQuantizedQuaternion(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out Quaternion[] v) => c.TryPeekQuantizedQuaternions(MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out Quaternion[] v) => c.TryReadQuantizedQuaternions(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Quaternion[] v) => c.TryPeekQuantizedQuaternions(count, MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Quaternion[] v) => c.TryReadQuantizedQuaternions(count, MinRange, MaxRange, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Quaternion> d) => c.TryPeekQuantizedQuaternions(d, MinRange, MaxRange, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<Quaternion> d) => c.TryReadQuantizedQuaternions(d, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Quaternion> d) => c.TryPeekQuantizedQuaternions(count, d, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Quaternion> d) => c.TryReadQuantizedQuaternions(count, d, MinRange, MaxRange, BitCount),
    };
}

[BitStreamPrimitiveContext]
public class QuantizedPlaneExtensionsTests : ExtrasQuantizedNumericsTestSuite<Plane> {
    private const float Min = 0f;
    private const float Max = 100f;
    private static readonly Plane MinRange = new Plane(new Vector3(Min, Min, Min), Min);
    private static readonly Plane MaxRange = new Plane(new Vector3(Max, Max, Max), Max);
    private const int BitCount = 8;

    protected override int Precision => 0;
    protected override Plane Value => new Plane(new Vector3(50f, 50f, 50f), 50f);
    protected override Plane[] Values => [new Plane(new Vector3(0f, 0f, 0f), 0f), new Plane(new Vector3(50f, 50f, 50f), 50f), new Plane(new Vector3(100f, 100f, 100f), 100f)];

    protected override void AssertValuesEqual(Plane expected, Plane actual) {
        Assert.Equal(expected.Normal.X, actual.Normal.X, Precision);
        Assert.Equal(expected.Normal.Y, actual.Normal.Y, Precision);
        Assert.Equal(expected.Normal.Z, actual.Normal.Z, Precision);
        Assert.Equal(expected.D, actual.D, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, Plane value) => context.WriteQuantizedPlanePrimitive(value, Min, Max, BitCount);
    protected override Plane PeekPrimitive(ReadContext context) => context.PeekQuantizedPlanePrimitive(MinRange, MaxRange, BitCount);
    protected override Plane ReadPrimitive(ReadContext context) => context.ReadQuantizedPlanePrimitive(MinRange, MaxRange, BitCount);
    protected override void Write(ref WriteContext context, Plane value) => context.WriteQuantizedPlane(value, MinRange, MaxRange, BitCount);
    protected override Plane Peek(ReadContext context) => context.PeekQuantizedPlane(MinRange, MaxRange, BitCount);
    protected override Plane Read(ReadContext context) => context.ReadQuantizedPlane(MinRange, MaxRange, BitCount);

    protected override Plane TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedPlane(MinRange, MaxRange, BitCount, out Plane v));
        return v;
    }

    protected override Plane TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedPlane(MinRange, MaxRange, BitCount, out Plane v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<Plane> values) => context.WriteQuantizedPlanesPrimitive(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<Plane> destination) => context.PeekQuantizedPlaneSpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<Plane> destination) => context.ReadQuantizedPlaneSpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Plane> values) => context.WriteQuantizedPlanesWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) => context.PeekQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) => context.ReadQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryPeekQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryReadQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<Plane> values) => context.WriteQuantizedPlanes(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<Plane> destination) => context.PeekQuantizedPlanes(destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<Plane> destination) => context.ReadQuantizedPlanes(destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Plane> destination) { Assert.True(context.TryPeekQuantizedPlanes(destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Plane> destination) { Assert.True(context.TryReadQuantizedPlanes(destination, MinRange, MaxRange, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, Plane[] values) => context.WriteQuantizedPlanesPrimitive(values, MinRange, MaxRange, BitCount);
    protected override Plane[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedPlaneArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override Plane[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedPlaneArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, Plane[] values) => context.WriteQuantizedPlanesWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override Plane[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedPlanes(count, MinRange, MaxRange, BitCount);
    protected override Plane[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedPlanes(count, MinRange, MaxRange, BitCount);

    protected override Plane[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedPlanes(count, MinRange, MaxRange, BitCount, out Plane[] values));
        return values;
    }

    protected override Plane[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedPlanes(count, MinRange, MaxRange, BitCount, out Plane[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, Plane[] values) => context.WriteQuantizedPlanes(values, MinRange, MaxRange, BitCount);
    protected override Plane[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedPlanes(MinRange, MaxRange, BitCount);
    protected override Plane[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedPlanes(MinRange, MaxRange, BitCount);

    protected override Plane[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedPlanes(MinRange, MaxRange, BitCount, out Plane[] values));
        return values;
    }

    protected override Plane[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedPlanes(MinRange, MaxRange, BitCount, out Plane[] values));
        return values;
    }

    protected override TryReadOperationSet<Plane> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Plane v) => c.TryPeekQuantizedPlane(MinRange, MaxRange, BitCount, out v),
        TryReadValue = (ReadContext c, out Plane v) => c.TryReadQuantizedPlane(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out Plane[] v) => c.TryPeekQuantizedPlanes(MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out Plane[] v) => c.TryReadQuantizedPlanes(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Plane[] v) => c.TryPeekQuantizedPlanes(count, MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Plane[] v) => c.TryReadQuantizedPlanes(count, MinRange, MaxRange, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Plane> d) => c.TryPeekQuantizedPlanes(d, MinRange, MaxRange, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<Plane> d) => c.TryReadQuantizedPlanes(d, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Plane> d) => c.TryPeekQuantizedPlanes(count, d, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Plane> d) => c.TryReadQuantizedPlanes(count, d, MinRange, MaxRange, BitCount),
    };
}

[BitStreamPrimitiveContext]
// ReSharper disable once InconsistentNaming
public class QuantizedMatrix4x4ExtensionsTests : ExtrasQuantizedNumericsTestSuite<Matrix4x4> {
    private const float Min = 0f;
    private const float Max = 100f;
    private static readonly Matrix4x4 MinRange = new Matrix4x4(Min, Min, Min, Min, Min, Min, Min, Min, Min, Min, Min, Min, Min, Min, Min, Min);
    private static readonly Matrix4x4 MaxRange = new Matrix4x4(Max, Max, Max, Max, Max, Max, Max, Max, Max, Max, Max, Max, Max, Max, Max, Max);
    private const int BitCount = 16;

    protected override int Precision => 0;
    protected override Matrix4x4 Value => new Matrix4x4(50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f);
    protected override Matrix4x4[] Values => [new Matrix4x4(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f), new Matrix4x4(50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f), new Matrix4x4(100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f)];

    protected override void AssertValuesEqual(Matrix4x4 expected, Matrix4x4 actual) {
        Assert.Equal(expected.M11, actual.M11, Precision);
        Assert.Equal(expected.M12, actual.M12, Precision);
        Assert.Equal(expected.M13, actual.M13, Precision);
        Assert.Equal(expected.M14, actual.M14, Precision);
        Assert.Equal(expected.M21, actual.M21, Precision);
        Assert.Equal(expected.M22, actual.M22, Precision);
        Assert.Equal(expected.M23, actual.M23, Precision);
        Assert.Equal(expected.M24, actual.M24, Precision);
        Assert.Equal(expected.M31, actual.M31, Precision);
        Assert.Equal(expected.M32, actual.M32, Precision);
        Assert.Equal(expected.M33, actual.M33, Precision);
        Assert.Equal(expected.M34, actual.M34, Precision);
        Assert.Equal(expected.M41, actual.M41, Precision);
        Assert.Equal(expected.M42, actual.M42, Precision);
        Assert.Equal(expected.M43, actual.M43, Precision);
        Assert.Equal(expected.M44, actual.M44, Precision);
    }

    protected override void WritePrimitive(ref WriteContext context, Matrix4x4 value) => context.WriteQuantizedMatrix4x4Primitive(value, Min, Max, BitCount);
    protected override Matrix4x4 PeekPrimitive(ReadContext context) => context.PeekQuantizedMatrix4x4Primitive(MinRange, MaxRange, BitCount);
    protected override Matrix4x4 ReadPrimitive(ReadContext context) => context.ReadQuantizedMatrix4x4Primitive(MinRange, MaxRange, BitCount);
    protected override void Write(ref WriteContext context, Matrix4x4 value) => context.WriteQuantizedMatrix4x4(value, MinRange, MaxRange, BitCount);
    protected override Matrix4x4 Peek(ReadContext context) => context.PeekQuantizedMatrix4x4(MinRange, MaxRange, BitCount);
    protected override Matrix4x4 Read(ReadContext context) => context.ReadQuantizedMatrix4x4(MinRange, MaxRange, BitCount);

    protected override Matrix4x4 TryPeek(ReadContext context) {
        Assert.True(context.TryPeekQuantizedMatrix4x4(MinRange, MaxRange, BitCount, out Matrix4x4 v));
        return v;
    }

    protected override Matrix4x4 TryRead(ReadContext context) {
        Assert.True(context.TryReadQuantizedMatrix4x4(MinRange, MaxRange, BitCount, out Matrix4x4 v));
        return v;
    }

    protected override void WriteSpanPrimitive(ref WriteContext context, Span<Matrix4x4> values) => context.WriteQuantizedMatrix4x4sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanPrimitive(ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanPrimitive(ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount);
    protected override void WriteSpanWithoutLength(ref WriteContext context, Span<Matrix4x4> values) => context.WriteQuantizedMatrix4x4sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryPeekQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithoutLength(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryReadQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount)); }
    protected override void WriteSpan(ref WriteContext context, Span<Matrix4x4> values) => context.WriteQuantizedMatrix4x4s(values, MinRange, MaxRange, BitCount);
    protected override void PeekSpanWithLength(ReadContext context, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount);
    protected override void ReadSpanWithLength(ReadContext context, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount);
    protected override void TryPeekSpanWithLength(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryPeekQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount)); }
    protected override void TryReadSpanWithLength(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryReadQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount)); }

    protected override void WriteArrayPrimitive(ref WriteContext context, Matrix4x4[] values) => context.WriteQuantizedMatrix4x4sPrimitive(values, MinRange, MaxRange, BitCount);
    protected override Matrix4x4[] PeekArrayPrimitive(ReadContext context, int count) => context.PeekQuantizedMatrix4x4ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override Matrix4x4[] ReadArrayPrimitive(ReadContext context, int count) => context.ReadQuantizedMatrix4x4ArrayPrimitive(count, MinRange, MaxRange, BitCount);
    protected override void WriteArrayWithoutLength(ref WriteContext context, Matrix4x4[] values) => context.WriteQuantizedMatrix4x4sWithoutLength(values, MinRange, MaxRange, BitCount);
    protected override Matrix4x4[] PeekArrayWithoutLength(ReadContext context, int count) => context.PeekQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount);
    protected override Matrix4x4[] ReadArrayWithoutLength(ReadContext context, int count) => context.ReadQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount);

    protected override Matrix4x4[] TryPeekArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryPeekQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount, out Matrix4x4[] values));
        return values;
    }

    protected override Matrix4x4[] TryReadArrayWithoutLength(ReadContext context, int count) {
        Assert.True(context.TryReadQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount, out Matrix4x4[] values));
        return values;
    }

    protected override void WriteArray(ref WriteContext context, Matrix4x4[] values) => context.WriteQuantizedMatrix4x4s(values, MinRange, MaxRange, BitCount);
    protected override Matrix4x4[] PeekArrayWithLength(ReadContext context) => context.PeekQuantizedMatrix4x4s(MinRange, MaxRange, BitCount);
    protected override Matrix4x4[] ReadArrayWithLength(ReadContext context) => context.ReadQuantizedMatrix4x4s(MinRange, MaxRange, BitCount);

    protected override Matrix4x4[] TryPeekArrayWithLength(ReadContext context) {
        Assert.True(context.TryPeekQuantizedMatrix4x4s(MinRange, MaxRange, BitCount, out Matrix4x4[] values));
        return values;
    }

    protected override Matrix4x4[] TryReadArrayWithLength(ReadContext context) {
        Assert.True(context.TryReadQuantizedMatrix4x4s(MinRange, MaxRange, BitCount, out Matrix4x4[] values));
        return values;
    }

    protected override TryReadOperationSet<Matrix4x4> TryOperations => new() {
        TryPeekValue = (ReadContext c, out Matrix4x4 v) => c.TryPeekQuantizedMatrix4x4(MinRange, MaxRange, BitCount, out v),
        TryReadValue = (ReadContext c, out Matrix4x4 v) => c.TryReadQuantizedMatrix4x4(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithLength = (ReadContext c, out Matrix4x4[] v) => c.TryPeekQuantizedMatrix4x4s(MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithLength = (ReadContext c, out Matrix4x4[] v) => c.TryReadQuantizedMatrix4x4s(MinRange, MaxRange, BitCount, out v),
        TryPeekArrayWithoutLength = (ReadContext c, int count, out Matrix4x4[] v) => c.TryPeekQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount, out v),
        TryReadArrayWithoutLength = (ReadContext c, int count, out Matrix4x4[] v) => c.TryReadQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount, out v),
        TryPeekSpanWithLength = (ReadContext c, Span<Matrix4x4> d) => c.TryPeekQuantizedMatrix4x4s(d, MinRange, MaxRange, BitCount),
        TryReadSpanWithLength = (ReadContext c, Span<Matrix4x4> d) => c.TryReadQuantizedMatrix4x4s(d, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext c, int count, Span<Matrix4x4> d) => c.TryPeekQuantizedMatrix4x4s(count, d, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext c, int count, Span<Matrix4x4> d) => c.TryReadQuantizedMatrix4x4s(count, d, MinRange, MaxRange, BitCount),
    };
}
