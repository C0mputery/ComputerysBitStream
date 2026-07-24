using System.Numerics;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Primitives.Quantized;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Extras.Primitives;

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

    protected override SerializationOperations<Vector2> Operations { get; } = new() {
        Write = (ref WriteContext context, Vector2 value) => context.WriteQuantizedVector2(value, MinRange, MaxRange, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedVector2(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedVector2(MinRange, MaxRange, BitCount),
        TryPeek = (ReadContext context, out Vector2 value) => context.TryPeekQuantizedVector2(MinRange, MaxRange, BitCount, out value),
        TryRead = (ReadContext context, out Vector2 value) => context.TryReadQuantizedVector2(MinRange, MaxRange, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<Vector2> values) => context.WriteQuantizedVector2s(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, Span<Vector2> destination) => context.PeekQuantizedVector2s(destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, Span<Vector2> destination) => context.ReadQuantizedVector2s(destination, MinRange, MaxRange, BitCount),
        TryPeekSpan = (ReadContext context, Span<Vector2> destination) => context.TryPeekQuantizedVector2s(destination, MinRange, MaxRange, BitCount),
        TryReadSpan = (ReadContext context, Span<Vector2> destination) => context.TryReadQuantizedVector2s(destination, MinRange, MaxRange, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Vector2> values) => context.WriteQuantizedVector2sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.PeekQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.ReadQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.TryPeekQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Vector2> destination) => context.TryReadQuantizedVector2s(count, destination, MinRange, MaxRange, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.PeekQuantizedVector2sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.ReadQuantizedVector2sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.TryPeekQuantizedVector2sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector2> destination) => context.TryReadQuantizedVector2sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Vector2[] values) => context.WriteQuantizedVector2s(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedVector2s(MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedVector2s(MinRange, MaxRange, BitCount),
        TryPeekArray = (ReadContext context, out Vector2[] values) => context.TryPeekQuantizedVector2s(MinRange, MaxRange, BitCount, out values),
        TryReadArray = (ReadContext context, out Vector2[] values) => context.TryReadQuantizedVector2s(MinRange, MaxRange, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, Vector2[] values) => context.WriteQuantizedVector2sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedVector2s(count, MinRange, MaxRange, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedVector2s(count, MinRange, MaxRange, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Vector2[] values) => context.TryPeekQuantizedVector2s(count, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Vector2[] values) => context.TryReadQuantizedVector2s(count, MinRange, MaxRange, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedVector2sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedVector2sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Vector2[] values) => context.TryPeekQuantizedVector2sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Vector2[] values) => context.TryReadQuantizedVector2sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<Vector2> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, Vector2 value) => context.WriteQuantizedVector2Primitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedVector2Primitive(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedVector2Primitive(MinRange, MaxRange, BitCount),
        WriteSpan = (ref WriteContext context, Span<Vector2> values) => context.WriteQuantizedVector2sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, int count, Span<Vector2> destination) => context.PeekQuantizedVector2SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, int count, Span<Vector2> destination) => context.ReadQuantizedVector2SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Vector2[] values) => context.WriteQuantizedVector2sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedVector2ArrayPrimitive(count, MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedVector2ArrayPrimitive(count, MinRange, MaxRange, BitCount),
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

    protected override SerializationOperations<Vector3> Operations { get; } = new() {
        Write = (ref WriteContext context, Vector3 value) => context.WriteQuantizedVector3(value, MinRange, MaxRange, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedVector3(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedVector3(MinRange, MaxRange, BitCount),
        TryPeek = (ReadContext context, out Vector3 value) => context.TryPeekQuantizedVector3(MinRange, MaxRange, BitCount, out value),
        TryRead = (ReadContext context, out Vector3 value) => context.TryReadQuantizedVector3(MinRange, MaxRange, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<Vector3> values) => context.WriteQuantizedVector3s(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, Span<Vector3> destination) => context.PeekQuantizedVector3s(destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, Span<Vector3> destination) => context.ReadQuantizedVector3s(destination, MinRange, MaxRange, BitCount),
        TryPeekSpan = (ReadContext context, Span<Vector3> destination) => context.TryPeekQuantizedVector3s(destination, MinRange, MaxRange, BitCount),
        TryReadSpan = (ReadContext context, Span<Vector3> destination) => context.TryReadQuantizedVector3s(destination, MinRange, MaxRange, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Vector3> values) => context.WriteQuantizedVector3sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.PeekQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.ReadQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.TryPeekQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Vector3> destination) => context.TryReadQuantizedVector3s(count, destination, MinRange, MaxRange, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.PeekQuantizedVector3sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.ReadQuantizedVector3sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.TryPeekQuantizedVector3sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector3> destination) => context.TryReadQuantizedVector3sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Vector3[] values) => context.WriteQuantizedVector3s(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedVector3s(MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedVector3s(MinRange, MaxRange, BitCount),
        TryPeekArray = (ReadContext context, out Vector3[] values) => context.TryPeekQuantizedVector3s(MinRange, MaxRange, BitCount, out values),
        TryReadArray = (ReadContext context, out Vector3[] values) => context.TryReadQuantizedVector3s(MinRange, MaxRange, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, Vector3[] values) => context.WriteQuantizedVector3sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedVector3s(count, MinRange, MaxRange, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedVector3s(count, MinRange, MaxRange, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Vector3[] values) => context.TryPeekQuantizedVector3s(count, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Vector3[] values) => context.TryReadQuantizedVector3s(count, MinRange, MaxRange, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedVector3sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedVector3sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Vector3[] values) => context.TryPeekQuantizedVector3sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Vector3[] values) => context.TryReadQuantizedVector3sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<Vector3> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, Vector3 value) => context.WriteQuantizedVector3Primitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedVector3Primitive(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedVector3Primitive(MinRange, MaxRange, BitCount),
        WriteSpan = (ref WriteContext context, Span<Vector3> values) => context.WriteQuantizedVector3sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, int count, Span<Vector3> destination) => context.PeekQuantizedVector3SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, int count, Span<Vector3> destination) => context.ReadQuantizedVector3SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Vector3[] values) => context.WriteQuantizedVector3sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedVector3ArrayPrimitive(count, MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedVector3ArrayPrimitive(count, MinRange, MaxRange, BitCount),
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

    protected override SerializationOperations<Vector4> Operations { get; } = new() {
        Write = (ref WriteContext context, Vector4 value) => context.WriteQuantizedVector4(value, MinRange, MaxRange, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedVector4(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedVector4(MinRange, MaxRange, BitCount),
        TryPeek = (ReadContext context, out Vector4 value) => context.TryPeekQuantizedVector4(MinRange, MaxRange, BitCount, out value),
        TryRead = (ReadContext context, out Vector4 value) => context.TryReadQuantizedVector4(MinRange, MaxRange, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<Vector4> values) => context.WriteQuantizedVector4s(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, Span<Vector4> destination) => context.PeekQuantizedVector4s(destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, Span<Vector4> destination) => context.ReadQuantizedVector4s(destination, MinRange, MaxRange, BitCount),
        TryPeekSpan = (ReadContext context, Span<Vector4> destination) => context.TryPeekQuantizedVector4s(destination, MinRange, MaxRange, BitCount),
        TryReadSpan = (ReadContext context, Span<Vector4> destination) => context.TryReadQuantizedVector4s(destination, MinRange, MaxRange, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Vector4> values) => context.WriteQuantizedVector4sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.PeekQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.ReadQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.TryPeekQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Vector4> destination) => context.TryReadQuantizedVector4s(count, destination, MinRange, MaxRange, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.PeekQuantizedVector4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.ReadQuantizedVector4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.TryPeekQuantizedVector4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Vector4> destination) => context.TryReadQuantizedVector4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Vector4[] values) => context.WriteQuantizedVector4s(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedVector4s(MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedVector4s(MinRange, MaxRange, BitCount),
        TryPeekArray = (ReadContext context, out Vector4[] values) => context.TryPeekQuantizedVector4s(MinRange, MaxRange, BitCount, out values),
        TryReadArray = (ReadContext context, out Vector4[] values) => context.TryReadQuantizedVector4s(MinRange, MaxRange, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, Vector4[] values) => context.WriteQuantizedVector4sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedVector4s(count, MinRange, MaxRange, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedVector4s(count, MinRange, MaxRange, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Vector4[] values) => context.TryPeekQuantizedVector4s(count, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Vector4[] values) => context.TryReadQuantizedVector4s(count, MinRange, MaxRange, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedVector4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedVector4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Vector4[] values) => context.TryPeekQuantizedVector4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Vector4[] values) => context.TryReadQuantizedVector4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<Vector4> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, Vector4 value) => context.WriteQuantizedVector4Primitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedVector4Primitive(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedVector4Primitive(MinRange, MaxRange, BitCount),
        WriteSpan = (ref WriteContext context, Span<Vector4> values) => context.WriteQuantizedVector4sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, int count, Span<Vector4> destination) => context.PeekQuantizedVector4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, int count, Span<Vector4> destination) => context.ReadQuantizedVector4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Vector4[] values) => context.WriteQuantizedVector4sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedVector4ArrayPrimitive(count, MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedVector4ArrayPrimitive(count, MinRange, MaxRange, BitCount),
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

    protected override SerializationOperations<Quaternion> Operations { get; } = new() {
        Write = (ref WriteContext context, Quaternion value) => context.WriteQuantizedQuaternion(value, MinRange, MaxRange, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedQuaternion(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedQuaternion(MinRange, MaxRange, BitCount),
        TryPeek = (ReadContext context, out Quaternion value) => context.TryPeekQuantizedQuaternion(MinRange, MaxRange, BitCount, out value),
        TryRead = (ReadContext context, out Quaternion value) => context.TryReadQuantizedQuaternion(MinRange, MaxRange, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<Quaternion> values) => context.WriteQuantizedQuaternions(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, Span<Quaternion> destination) => context.PeekQuantizedQuaternions(destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, Span<Quaternion> destination) => context.ReadQuantizedQuaternions(destination, MinRange, MaxRange, BitCount),
        TryPeekSpan = (ReadContext context, Span<Quaternion> destination) => context.TryPeekQuantizedQuaternions(destination, MinRange, MaxRange, BitCount),
        TryReadSpan = (ReadContext context, Span<Quaternion> destination) => context.TryReadQuantizedQuaternions(destination, MinRange, MaxRange, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Quaternion> values) => context.WriteQuantizedQuaternionsWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.TryPeekQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Quaternion> destination) => context.TryReadQuantizedQuaternions(count, destination, MinRange, MaxRange, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.PeekQuantizedQuaternionsWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.ReadQuantizedQuaternionsWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.TryPeekQuantizedQuaternionsWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Quaternion> destination) => context.TryReadQuantizedQuaternionsWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Quaternion[] values) => context.WriteQuantizedQuaternions(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedQuaternions(MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedQuaternions(MinRange, MaxRange, BitCount),
        TryPeekArray = (ReadContext context, out Quaternion[] values) => context.TryPeekQuantizedQuaternions(MinRange, MaxRange, BitCount, out values),
        TryReadArray = (ReadContext context, out Quaternion[] values) => context.TryReadQuantizedQuaternions(MinRange, MaxRange, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, Quaternion[] values) => context.WriteQuantizedQuaternionsWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedQuaternions(count, MinRange, MaxRange, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedQuaternions(count, MinRange, MaxRange, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Quaternion[] values) => context.TryPeekQuantizedQuaternions(count, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Quaternion[] values) => context.TryReadQuantizedQuaternions(count, MinRange, MaxRange, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedQuaternionsWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedQuaternionsWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Quaternion[] values) => context.TryPeekQuantizedQuaternionsWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Quaternion[] values) => context.TryReadQuantizedQuaternionsWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<Quaternion> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, Quaternion value) => context.WriteQuantizedQuaternionPrimitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedQuaternionPrimitive(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedQuaternionPrimitive(MinRange, MaxRange, BitCount),
        WriteSpan = (ref WriteContext context, Span<Quaternion> values) => context.WriteQuantizedQuaternionsPrimitive(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuantizedQuaternionSpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuantizedQuaternionSpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Quaternion[] values) => context.WriteQuantizedQuaternionsPrimitive(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedQuaternionArrayPrimitive(count, MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedQuaternionArrayPrimitive(count, MinRange, MaxRange, BitCount),
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

    protected override SerializationOperations<Plane> Operations { get; } = new() {
        Write = (ref WriteContext context, Plane value) => context.WriteQuantizedPlane(value, MinRange, MaxRange, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedPlane(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedPlane(MinRange, MaxRange, BitCount),
        TryPeek = (ReadContext context, out Plane value) => context.TryPeekQuantizedPlane(MinRange, MaxRange, BitCount, out value),
        TryRead = (ReadContext context, out Plane value) => context.TryReadQuantizedPlane(MinRange, MaxRange, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<Plane> values) => context.WriteQuantizedPlanes(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, Span<Plane> destination) => context.PeekQuantizedPlanes(destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, Span<Plane> destination) => context.ReadQuantizedPlanes(destination, MinRange, MaxRange, BitCount),
        TryPeekSpan = (ReadContext context, Span<Plane> destination) => context.TryPeekQuantizedPlanes(destination, MinRange, MaxRange, BitCount),
        TryReadSpan = (ReadContext context, Span<Plane> destination) => context.TryReadQuantizedPlanes(destination, MinRange, MaxRange, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Plane> values) => context.WriteQuantizedPlanesWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.PeekQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.ReadQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.TryPeekQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Plane> destination) => context.TryReadQuantizedPlanes(count, destination, MinRange, MaxRange, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.PeekQuantizedPlanesWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.ReadQuantizedPlanesWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.TryPeekQuantizedPlanesWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Plane> destination) => context.TryReadQuantizedPlanesWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Plane[] values) => context.WriteQuantizedPlanes(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedPlanes(MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedPlanes(MinRange, MaxRange, BitCount),
        TryPeekArray = (ReadContext context, out Plane[] values) => context.TryPeekQuantizedPlanes(MinRange, MaxRange, BitCount, out values),
        TryReadArray = (ReadContext context, out Plane[] values) => context.TryReadQuantizedPlanes(MinRange, MaxRange, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, Plane[] values) => context.WriteQuantizedPlanesWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedPlanes(count, MinRange, MaxRange, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedPlanes(count, MinRange, MaxRange, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Plane[] values) => context.TryPeekQuantizedPlanes(count, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Plane[] values) => context.TryReadQuantizedPlanes(count, MinRange, MaxRange, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedPlanesWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedPlanesWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Plane[] values) => context.TryPeekQuantizedPlanesWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Plane[] values) => context.TryReadQuantizedPlanesWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<Plane> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, Plane value) => context.WriteQuantizedPlanePrimitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedPlanePrimitive(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedPlanePrimitive(MinRange, MaxRange, BitCount),
        WriteSpan = (ref WriteContext context, Span<Plane> values) => context.WriteQuantizedPlanesPrimitive(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, int count, Span<Plane> destination) => context.PeekQuantizedPlaneSpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, int count, Span<Plane> destination) => context.ReadQuantizedPlaneSpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Plane[] values) => context.WriteQuantizedPlanesPrimitive(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedPlaneArrayPrimitive(count, MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedPlaneArrayPrimitive(count, MinRange, MaxRange, BitCount),
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

    protected override SerializationOperations<Matrix4x4> Operations { get; } = new() {
        Write = (ref WriteContext context, Matrix4x4 value) => context.WriteQuantizedMatrix4x4(value, MinRange, MaxRange, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedMatrix4x4(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedMatrix4x4(MinRange, MaxRange, BitCount),
        TryPeek = (ReadContext context, out Matrix4x4 value) => context.TryPeekQuantizedMatrix4x4(MinRange, MaxRange, BitCount, out value),
        TryRead = (ReadContext context, out Matrix4x4 value) => context.TryReadQuantizedMatrix4x4(MinRange, MaxRange, BitCount, out value),
        WriteSpan = (ref WriteContext context, Span<Matrix4x4> values) => context.WriteQuantizedMatrix4x4s(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount),
        TryPeekSpan = (ReadContext context, Span<Matrix4x4> destination) => context.TryPeekQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount),
        TryReadSpan = (ReadContext context, Span<Matrix4x4> destination) => context.TryReadQuantizedMatrix4x4s(destination, MinRange, MaxRange, BitCount),
        WriteSpanWithoutLength = (ref WriteContext context, Span<Matrix4x4> values) => context.WriteQuantizedMatrix4x4sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.TryPeekQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithoutLength = (ReadContext context, int count, Span<Matrix4x4> destination) => context.TryReadQuantizedMatrix4x4s(count, destination, MinRange, MaxRange, BitCount),
        PeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        ReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryPeekSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.TryPeekQuantizedMatrix4x4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        TryReadSpanWithMaxCount = (ReadContext context, int maxCount, Span<Matrix4x4> destination) => context.TryReadQuantizedMatrix4x4sWithMaxCount(maxCount, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Matrix4x4[] values) => context.WriteQuantizedMatrix4x4s(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context) => context.PeekQuantizedMatrix4x4s(MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context) => context.ReadQuantizedMatrix4x4s(MinRange, MaxRange, BitCount),
        TryPeekArray = (ReadContext context, out Matrix4x4[] values) => context.TryPeekQuantizedMatrix4x4s(MinRange, MaxRange, BitCount, out values),
        TryReadArray = (ReadContext context, out Matrix4x4[] values) => context.TryReadQuantizedMatrix4x4s(MinRange, MaxRange, BitCount, out values),
        WriteArrayWithoutLength = (ref WriteContext context, Matrix4x4[] values) => context.WriteQuantizedMatrix4x4sWithoutLength(values, MinRange, MaxRange, BitCount),
        PeekArrayWithoutLength = (ReadContext context, int count) => context.PeekQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount),
        ReadArrayWithoutLength = (ReadContext context, int count) => context.ReadQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount),
        TryPeekArrayWithoutLength = (ReadContext context, int count, out Matrix4x4[] values) => context.TryPeekQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithoutLength = (ReadContext context, int count, out Matrix4x4[] values) => context.TryReadQuantizedMatrix4x4s(count, MinRange, MaxRange, BitCount, out values),
        PeekArrayWithMaxCount = (ReadContext context, int maxCount) => context.PeekQuantizedMatrix4x4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        ReadArrayWithMaxCount = (ReadContext context, int maxCount) => context.ReadQuantizedMatrix4x4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount),
        TryPeekArrayWithMaxCount = (ReadContext context, int maxCount, out Matrix4x4[] values) => context.TryPeekQuantizedMatrix4x4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
        TryReadArrayWithMaxCount = (ReadContext context, int maxCount, out Matrix4x4[] values) => context.TryReadQuantizedMatrix4x4sWithMaxCount(maxCount, MinRange, MaxRange, BitCount, out values),
    };

    protected override PrimitiveSerializationOperations<Matrix4x4> PrimitiveOperations { get; } = new() {
        Write = (ref WriteContext context, Matrix4x4 value) => context.WriteQuantizedMatrix4x4Primitive(value, Min, Max, BitCount),
        Peek = (ReadContext context) => context.PeekQuantizedMatrix4x4Primitive(MinRange, MaxRange, BitCount),
        Read = (ReadContext context) => context.ReadQuantizedMatrix4x4Primitive(MinRange, MaxRange, BitCount),
        WriteSpan = (ref WriteContext context, Span<Matrix4x4> values) => context.WriteQuantizedMatrix4x4sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekSpan = (ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekQuantizedMatrix4x4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        ReadSpan = (ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadQuantizedMatrix4x4SpanPrimitive(count, destination, MinRange, MaxRange, BitCount),
        WriteArray = (ref WriteContext context, Matrix4x4[] values) => context.WriteQuantizedMatrix4x4sPrimitive(values, MinRange, MaxRange, BitCount),
        PeekArray = (ReadContext context, int count) => context.PeekQuantizedMatrix4x4ArrayPrimitive(count, MinRange, MaxRange, BitCount),
        ReadArray = (ReadContext context, int count) => context.ReadQuantizedMatrix4x4ArrayPrimitive(count, MinRange, MaxRange, BitCount),
    };
}
