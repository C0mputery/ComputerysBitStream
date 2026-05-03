using System.Numerics;
using Xunit;

namespace ComputerysBitStream.Tests;

public class Vector2Tests : StructTestSuite<Vector2> {
    protected override Vector2 Value => new(1.0f, 2.0f);
    protected override Vector2[] Values => [
        new(1.0f, 2.0f),
        new(3.0f, 4.0f),
        new(5.0f, 6.0f)
    ];
    protected override int? ExpectedFixedSizeBits => 64;

    protected override void WriteNamed(WriteContext context, Vector2 value) => context.WriteVector2(value);
    protected override Vector2 PeekNamed(ReadContext context) => context.PeekVector2();
    protected override Vector2 ReadNamed(ReadContext context) => context.ReadVector2();
    protected override void WriteAlias(WriteContext context, Vector2 value) => context.Write(value);
    protected override Vector2 PeekAlias(ReadContext context) { context.Peek(out Vector2 v); return v; }
    protected override Vector2 ReadAlias(ReadContext context) { context.Read(out Vector2 v); return v; }
    protected override Vector2 TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekVector2(out Vector2 v)); return v; }
    protected override Vector2 TryReadNamed(ReadContext context) { Assert.True(context.TryReadVector2(out Vector2 v)); return v; }
    protected override Vector2 TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Vector2 v)); return v; }
    protected override Vector2 TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Vector2 v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Vector2[] values) => context.WriteVector2s(values);
    protected override Vector2[] PeekArrayNamed(ReadContext context) => context.PeekVector2s();
    protected override Vector2[] ReadArrayNamed(ReadContext context) => context.ReadVector2s();
    protected override void WriteArrayAlias(WriteContext context, Vector2[] values) => context.Write(values);
    protected override Vector2[] PeekArrayAlias(ReadContext context) { context.Peek(out Vector2[] v); return v; }
    protected override Vector2[] ReadArrayAlias(ReadContext context) { context.Read(out Vector2[] v); return v; }
    protected override Vector2[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekVector2s(out Vector2[] v)); return v; }
    protected override Vector2[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadVector2s(out Vector2[] v)); return v; }
    protected override Vector2[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Vector2[] v)); return v; }
    protected override Vector2[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Vector2[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Vector2[] values) => context.WriteVector2sWithoutLength(values);
    protected override Vector2[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekVector2s(count);
    protected override Vector2[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadVector2s(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Vector2[] values) => context.WriteWithoutLength(values);
    protected override Vector2[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Vector2[] v); return v; }
    protected override Vector2[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Vector2[] v); return v; }
    protected override Vector2[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekVector2s(count, out Vector2[] v)); return v; }
    protected override Vector2[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadVector2s(count, out Vector2[] v)); return v; }
    protected override Vector2[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Vector2[] v)); return v; }
    protected override Vector2[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Vector2[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Vector2> values) => context.WriteVector2s(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Vector2> destination) => context.PeekVector2s(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Vector2> destination) => context.ReadVector2s(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Vector2> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Vector2> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Vector2> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryPeekVector2s(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryReadVector2s(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Vector2> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Vector2> values) => context.WriteVector2sWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector2> destination) => context.PeekVector2s(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector2> destination) => context.ReadVector2s(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Vector2> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector2> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector2> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryPeekVector2s(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryReadVector2s(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector2> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Vector2 value) => value.GetVector2SizeInBits();
    protected override bool IsFixedSizeStruct(Vector2 value) => value.IsVector2FixedSizeStruct();
}

public class Vector3Tests : StructTestSuite<Vector3> {
    protected override Vector3 Value => new(1.0f, 2.0f, 3.0f);
    protected override Vector3[] Values => [
        new(1.0f, 2.0f, 3.0f),
        new(4.0f, 5.0f, 6.0f),
        new(7.0f, 8.0f, 9.0f)
    ];
    protected override int? ExpectedFixedSizeBits => 96;

    protected override void WriteNamed(WriteContext context, Vector3 value) => context.WriteVector3(value);
    protected override Vector3 PeekNamed(ReadContext context) => context.PeekVector3();
    protected override Vector3 ReadNamed(ReadContext context) => context.ReadVector3();
    protected override void WriteAlias(WriteContext context, Vector3 value) => context.Write(value);
    protected override Vector3 PeekAlias(ReadContext context) { context.Peek(out Vector3 v); return v; }
    protected override Vector3 ReadAlias(ReadContext context) { context.Read(out Vector3 v); return v; }
    protected override Vector3 TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekVector3(out Vector3 v)); return v; }
    protected override Vector3 TryReadNamed(ReadContext context) { Assert.True(context.TryReadVector3(out Vector3 v)); return v; }
    protected override Vector3 TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Vector3 v)); return v; }
    protected override Vector3 TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Vector3 v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Vector3[] values) => context.WriteVector3s(values);
    protected override Vector3[] PeekArrayNamed(ReadContext context) => context.PeekVector3s();
    protected override Vector3[] ReadArrayNamed(ReadContext context) => context.ReadVector3s();
    protected override void WriteArrayAlias(WriteContext context, Vector3[] values) => context.Write(values);
    protected override Vector3[] PeekArrayAlias(ReadContext context) { context.Peek(out Vector3[] v); return v; }
    protected override Vector3[] ReadArrayAlias(ReadContext context) { context.Read(out Vector3[] v); return v; }
    protected override Vector3[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekVector3s(out Vector3[] v)); return v; }
    protected override Vector3[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadVector3s(out Vector3[] v)); return v; }
    protected override Vector3[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Vector3[] v)); return v; }
    protected override Vector3[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Vector3[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Vector3[] values) => context.WriteVector3sWithoutLength(values);
    protected override Vector3[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekVector3s(count);
    protected override Vector3[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadVector3s(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Vector3[] values) => context.WriteWithoutLength(values);
    protected override Vector3[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Vector3[] v); return v; }
    protected override Vector3[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Vector3[] v); return v; }
    protected override Vector3[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekVector3s(count, out Vector3[] v)); return v; }
    protected override Vector3[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadVector3s(count, out Vector3[] v)); return v; }
    protected override Vector3[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Vector3[] v)); return v; }
    protected override Vector3[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Vector3[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Vector3> values) => context.WriteVector3s(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Vector3> destination) => context.PeekVector3s(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Vector3> destination) => context.ReadVector3s(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Vector3> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Vector3> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Vector3> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryPeekVector3s(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryReadVector3s(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Vector3> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Vector3> values) => context.WriteVector3sWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector3> destination) => context.PeekVector3s(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector3> destination) => context.ReadVector3s(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Vector3> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector3> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector3> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryPeekVector3s(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryReadVector3s(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector3> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Vector3 value) => value.GetVector3SizeInBits();
    protected override bool IsFixedSizeStruct(Vector3 value) => value.IsVector3FixedSizeStruct();
}

public class Vector4Tests : StructTestSuite<Vector4> {
    protected override Vector4 Value => new(1.0f, 2.0f, 3.0f, 4.0f);
    protected override Vector4[] Values => [
        new(1.0f, 2.0f, 3.0f, 4.0f),
        new(5.0f, 6.0f, 7.0f, 8.0f),
        new(9.0f, 10.0f, 11.0f, 12.0f)
    ];
    protected override int? ExpectedFixedSizeBits => 128;

    protected override void WriteNamed(WriteContext context, Vector4 value) => context.WriteVector4(value);
    protected override Vector4 PeekNamed(ReadContext context) => context.PeekVector4();
    protected override Vector4 ReadNamed(ReadContext context) => context.ReadVector4();
    protected override void WriteAlias(WriteContext context, Vector4 value) => context.Write(value);
    protected override Vector4 PeekAlias(ReadContext context) { context.Peek(out Vector4 v); return v; }
    protected override Vector4 ReadAlias(ReadContext context) { context.Read(out Vector4 v); return v; }
    protected override Vector4 TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekVector4(out Vector4 v)); return v; }
    protected override Vector4 TryReadNamed(ReadContext context) { Assert.True(context.TryReadVector4(out Vector4 v)); return v; }
    protected override Vector4 TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Vector4 v)); return v; }
    protected override Vector4 TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Vector4 v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Vector4[] values) => context.WriteVector4s(values);
    protected override Vector4[] PeekArrayNamed(ReadContext context) => context.PeekVector4s();
    protected override Vector4[] ReadArrayNamed(ReadContext context) => context.ReadVector4s();
    protected override void WriteArrayAlias(WriteContext context, Vector4[] values) => context.Write(values);
    protected override Vector4[] PeekArrayAlias(ReadContext context) { context.Peek(out Vector4[] v); return v; }
    protected override Vector4[] ReadArrayAlias(ReadContext context) { context.Read(out Vector4[] v); return v; }
    protected override Vector4[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekVector4s(out Vector4[] v)); return v; }
    protected override Vector4[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadVector4s(out Vector4[] v)); return v; }
    protected override Vector4[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Vector4[] v)); return v; }
    protected override Vector4[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Vector4[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Vector4[] values) => context.WriteVector4sWithoutLength(values);
    protected override Vector4[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekVector4s(count);
    protected override Vector4[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadVector4s(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Vector4[] values) => context.WriteWithoutLength(values);
    protected override Vector4[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Vector4[] v); return v; }
    protected override Vector4[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Vector4[] v); return v; }
    protected override Vector4[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekVector4s(count, out Vector4[] v)); return v; }
    protected override Vector4[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadVector4s(count, out Vector4[] v)); return v; }
    protected override Vector4[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Vector4[] v)); return v; }
    protected override Vector4[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Vector4[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Vector4> values) => context.WriteVector4s(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Vector4> destination) => context.PeekVector4s(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Vector4> destination) => context.ReadVector4s(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Vector4> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Vector4> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Vector4> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryPeekVector4s(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryReadVector4s(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Vector4> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Vector4> values) => context.WriteVector4sWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector4> destination) => context.PeekVector4s(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector4> destination) => context.ReadVector4s(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Vector4> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector4> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector4> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryPeekVector4s(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryReadVector4s(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Vector4> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Vector4 value) => value.GetVector4SizeInBits();
    protected override bool IsFixedSizeStruct(Vector4 value) => value.IsVector4FixedSizeStruct();
}

public class QuaternionTests : StructTestSuite<Quaternion> {
    protected override Quaternion Value => new(1.0f, 2.0f, 3.0f, 4.0f);
    protected override Quaternion[] Values => [
        new(1.0f, 2.0f, 3.0f, 4.0f),
        new(5.0f, 6.0f, 7.0f, 8.0f),
        new(9.0f, 10.0f, 11.0f, 12.0f)
    ];
    protected override int? ExpectedFixedSizeBits => 128;

    protected override void WriteNamed(WriteContext context, Quaternion value) => context.WriteQuaternion(value);
    protected override Quaternion PeekNamed(ReadContext context) => context.PeekQuaternion();
    protected override Quaternion ReadNamed(ReadContext context) => context.ReadQuaternion();
    protected override void WriteAlias(WriteContext context, Quaternion value) => context.Write(value);
    protected override Quaternion PeekAlias(ReadContext context) { context.Peek(out Quaternion v); return v; }
    protected override Quaternion ReadAlias(ReadContext context) { context.Read(out Quaternion v); return v; }
    protected override Quaternion TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekQuaternion(out Quaternion v)); return v; }
    protected override Quaternion TryReadNamed(ReadContext context) { Assert.True(context.TryReadQuaternion(out Quaternion v)); return v; }
    protected override Quaternion TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Quaternion v)); return v; }
    protected override Quaternion TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Quaternion v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Quaternion[] values) => context.WriteQuaternions(values);
    protected override Quaternion[] PeekArrayNamed(ReadContext context) => context.PeekQuaternions();
    protected override Quaternion[] ReadArrayNamed(ReadContext context) => context.ReadQuaternions();
    protected override void WriteArrayAlias(WriteContext context, Quaternion[] values) => context.Write(values);
    protected override Quaternion[] PeekArrayAlias(ReadContext context) { context.Peek(out Quaternion[] v); return v; }
    protected override Quaternion[] ReadArrayAlias(ReadContext context) { context.Read(out Quaternion[] v); return v; }
    protected override Quaternion[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekQuaternions(out Quaternion[] v)); return v; }
    protected override Quaternion[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadQuaternions(out Quaternion[] v)); return v; }
    protected override Quaternion[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Quaternion[] v)); return v; }
    protected override Quaternion[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Quaternion[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Quaternion[] values) => context.WriteQuaternionsWithoutLength(values);
    protected override Quaternion[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekQuaternions(count);
    protected override Quaternion[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadQuaternions(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Quaternion[] values) => context.WriteWithoutLength(values);
    protected override Quaternion[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Quaternion[] v); return v; }
    protected override Quaternion[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Quaternion[] v); return v; }
    protected override Quaternion[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekQuaternions(count, out Quaternion[] v)); return v; }
    protected override Quaternion[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadQuaternions(count, out Quaternion[] v)); return v; }
    protected override Quaternion[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Quaternion[] v)); return v; }
    protected override Quaternion[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Quaternion[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Quaternion> values) => context.WriteQuaternions(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Quaternion> destination) => context.PeekQuaternions(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Quaternion> destination) => context.ReadQuaternions(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Quaternion> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Quaternion> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Quaternion> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryPeekQuaternions(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryReadQuaternions(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Quaternion> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Quaternion> values) => context.WriteQuaternionsWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Quaternion> destination) => context.PeekQuaternions(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Quaternion> destination) => context.ReadQuaternions(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Quaternion> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Quaternion> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Quaternion> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryPeekQuaternions(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryReadQuaternions(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Quaternion> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Quaternion value) => value.GetQuaternionSizeInBits();
    protected override bool IsFixedSizeStruct(Quaternion value) => value.IsQuaternionFixedSizeStruct();
}

public class Matrix3x2Tests : StructTestSuite<Matrix3x2> {
    protected override Matrix3x2 Value => new(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f);
    protected override Matrix3x2[] Values => [
        new(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f),
        new(7.0f, 8.0f, 9.0f, 10.0f, 11.0f, 12.0f),
        new(13.0f, 14.0f, 15.0f, 16.0f, 17.0f, 18.0f)
    ];
    protected override int? ExpectedFixedSizeBits => 192;

    protected override void WriteNamed(WriteContext context, Matrix3x2 value) => context.WriteMatrix3x2(value);
    protected override Matrix3x2 PeekNamed(ReadContext context) => context.PeekMatrix3x2();
    protected override Matrix3x2 ReadNamed(ReadContext context) => context.ReadMatrix3x2();
    protected override void WriteAlias(WriteContext context, Matrix3x2 value) => context.Write(value);
    protected override Matrix3x2 PeekAlias(ReadContext context) { context.Peek(out Matrix3x2 v); return v; }
    protected override Matrix3x2 ReadAlias(ReadContext context) { context.Read(out Matrix3x2 v); return v; }
    protected override Matrix3x2 TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekMatrix3x2(out Matrix3x2 v)); return v; }
    protected override Matrix3x2 TryReadNamed(ReadContext context) { Assert.True(context.TryReadMatrix3x2(out Matrix3x2 v)); return v; }
    protected override Matrix3x2 TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Matrix3x2 v)); return v; }
    protected override Matrix3x2 TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Matrix3x2 v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Matrix3x2[] values) => context.WriteMatrix3x2s(values);
    protected override Matrix3x2[] PeekArrayNamed(ReadContext context) => context.PeekMatrix3x2s();
    protected override Matrix3x2[] ReadArrayNamed(ReadContext context) => context.ReadMatrix3x2s();
    protected override void WriteArrayAlias(WriteContext context, Matrix3x2[] values) => context.Write(values);
    protected override Matrix3x2[] PeekArrayAlias(ReadContext context) { context.Peek(out Matrix3x2[] v); return v; }
    protected override Matrix3x2[] ReadArrayAlias(ReadContext context) { context.Read(out Matrix3x2[] v); return v; }
    protected override Matrix3x2[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekMatrix3x2s(out Matrix3x2[] v)); return v; }
    protected override Matrix3x2[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadMatrix3x2s(out Matrix3x2[] v)); return v; }
    protected override Matrix3x2[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Matrix3x2[] v)); return v; }
    protected override Matrix3x2[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Matrix3x2[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Matrix3x2[] values) => context.WriteMatrix3x2sWithoutLength(values);
    protected override Matrix3x2[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekMatrix3x2s(count);
    protected override Matrix3x2[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadMatrix3x2s(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Matrix3x2[] values) => context.WriteWithoutLength(values);
    protected override Matrix3x2[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Matrix3x2[] v); return v; }
    protected override Matrix3x2[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Matrix3x2[] v); return v; }
    protected override Matrix3x2[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekMatrix3x2s(count, out Matrix3x2[] v)); return v; }
    protected override Matrix3x2[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadMatrix3x2s(count, out Matrix3x2[] v)); return v; }
    protected override Matrix3x2[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Matrix3x2[] v)); return v; }
    protected override Matrix3x2[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Matrix3x2[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Matrix3x2> values) => context.WriteMatrix3x2s(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Matrix3x2> destination) => context.PeekMatrix3x2s(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Matrix3x2> destination) => context.ReadMatrix3x2s(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Matrix3x2> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Matrix3x2> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Matrix3x2> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Matrix3x2> destination) { Assert.True(context.TryPeekMatrix3x2s(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Matrix3x2> destination) { Assert.True(context.TryReadMatrix3x2s(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Matrix3x2> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Matrix3x2> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Matrix3x2> values) => context.WriteMatrix3x2sWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix3x2> destination) => context.PeekMatrix3x2s(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix3x2> destination) => context.ReadMatrix3x2s(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Matrix3x2> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix3x2> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix3x2> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix3x2> destination) { Assert.True(context.TryPeekMatrix3x2s(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix3x2> destination) { Assert.True(context.TryReadMatrix3x2s(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix3x2> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix3x2> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Matrix3x2 value) => value.GetMatrix3x2SizeInBits();
    protected override bool IsFixedSizeStruct(Matrix3x2 value) => value.IsMatrix3x2FixedSizeStruct();
}

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

    protected override void WriteNamed(WriteContext context, Matrix4x4 value) => context.WriteMatrix4x4(value);
    protected override Matrix4x4 PeekNamed(ReadContext context) => context.PeekMatrix4x4();
    protected override Matrix4x4 ReadNamed(ReadContext context) => context.ReadMatrix4x4();
    protected override void WriteAlias(WriteContext context, Matrix4x4 value) => context.Write(value);
    protected override Matrix4x4 PeekAlias(ReadContext context) { context.Peek(out Matrix4x4 v); return v; }
    protected override Matrix4x4 ReadAlias(ReadContext context) { context.Read(out Matrix4x4 v); return v; }
    protected override Matrix4x4 TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekMatrix4x4(out Matrix4x4 v)); return v; }
    protected override Matrix4x4 TryReadNamed(ReadContext context) { Assert.True(context.TryReadMatrix4x4(out Matrix4x4 v)); return v; }
    protected override Matrix4x4 TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Matrix4x4 v)); return v; }
    protected override Matrix4x4 TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Matrix4x4 v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Matrix4x4[] values) => context.WriteMatrix4x4s(values);
    protected override Matrix4x4[] PeekArrayNamed(ReadContext context) => context.PeekMatrix4x4s();
    protected override Matrix4x4[] ReadArrayNamed(ReadContext context) => context.ReadMatrix4x4s();
    protected override void WriteArrayAlias(WriteContext context, Matrix4x4[] values) => context.Write(values);
    protected override Matrix4x4[] PeekArrayAlias(ReadContext context) { context.Peek(out Matrix4x4[] v); return v; }
    protected override Matrix4x4[] ReadArrayAlias(ReadContext context) { context.Read(out Matrix4x4[] v); return v; }
    protected override Matrix4x4[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekMatrix4x4s(out Matrix4x4[] v)); return v; }
    protected override Matrix4x4[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadMatrix4x4s(out Matrix4x4[] v)); return v; }
    protected override Matrix4x4[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Matrix4x4[] v)); return v; }
    protected override Matrix4x4[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Matrix4x4[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Matrix4x4[] values) => context.WriteMatrix4x4sWithoutLength(values);
    protected override Matrix4x4[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekMatrix4x4s(count);
    protected override Matrix4x4[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadMatrix4x4s(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Matrix4x4[] values) => context.WriteWithoutLength(values);
    protected override Matrix4x4[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Matrix4x4[] v); return v; }
    protected override Matrix4x4[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Matrix4x4[] v); return v; }
    protected override Matrix4x4[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekMatrix4x4s(count, out Matrix4x4[] v)); return v; }
    protected override Matrix4x4[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadMatrix4x4s(count, out Matrix4x4[] v)); return v; }
    protected override Matrix4x4[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Matrix4x4[] v)); return v; }
    protected override Matrix4x4[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Matrix4x4[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Matrix4x4> values) => context.WriteMatrix4x4s(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Matrix4x4> destination) => context.PeekMatrix4x4s(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Matrix4x4> destination) => context.ReadMatrix4x4s(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Matrix4x4> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Matrix4x4> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Matrix4x4> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryPeekMatrix4x4s(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryReadMatrix4x4s(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Matrix4x4> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Matrix4x4> values) => context.WriteMatrix4x4sWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix4x4> destination) => context.PeekMatrix4x4s(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix4x4> destination) => context.ReadMatrix4x4s(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Matrix4x4> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix4x4> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix4x4> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryPeekMatrix4x4s(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryReadMatrix4x4s(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Matrix4x4> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Matrix4x4 value) => value.GetMatrix4x4SizeInBits();
    protected override bool IsFixedSizeStruct(Matrix4x4 value) => value.IsMatrix4x4FixedSizeStruct();
}

public class PlaneTests : StructTestSuite<Plane> {
    protected override Plane Value => new(new Vector3(0.0f, 1.0f, 0.0f), 5.0f);
    protected override Plane[] Values => [
        new(new Vector3(0.0f, 1.0f, 0.0f), 1.0f),
        new(new Vector3(1.0f, 0.0f, 0.0f), 2.0f),
        new(new Vector3(0.0f, 0.0f, 1.0f), 3.0f)
    ];
    protected override int? ExpectedFixedSizeBits => 128;

    protected override void WriteNamed(WriteContext context, Plane value) => context.WritePlane(value);
    protected override Plane PeekNamed(ReadContext context) => context.PeekPlane();
    protected override Plane ReadNamed(ReadContext context) => context.ReadPlane();
    protected override void WriteAlias(WriteContext context, Plane value) => context.Write(value);
    protected override Plane PeekAlias(ReadContext context) { context.Peek(out Plane v); return v; }
    protected override Plane ReadAlias(ReadContext context) { context.Read(out Plane v); return v; }
    protected override Plane TryPeekNamed(ReadContext context) { Assert.True(context.TryPeekPlane(out Plane v)); return v; }
    protected override Plane TryReadNamed(ReadContext context) { Assert.True(context.TryReadPlane(out Plane v)); return v; }
    protected override Plane TryPeekAlias(ReadContext context) { Assert.True(context.TryPeek(out Plane v)); return v; }
    protected override Plane TryReadAlias(ReadContext context) { Assert.True(context.TryRead(out Plane v)); return v; }

    protected override void WriteArrayNamed(WriteContext context, Plane[] values) => context.WritePlanes(values);
    protected override Plane[] PeekArrayNamed(ReadContext context) => context.PeekPlanes();
    protected override Plane[] ReadArrayNamed(ReadContext context) => context.ReadPlanes();
    protected override void WriteArrayAlias(WriteContext context, Plane[] values) => context.Write(values);
    protected override Plane[] PeekArrayAlias(ReadContext context) { context.Peek(out Plane[] v); return v; }
    protected override Plane[] ReadArrayAlias(ReadContext context) { context.Read(out Plane[] v); return v; }
    protected override Plane[] TryPeekArrayNamed(ReadContext context) { Assert.True(context.TryPeekPlanes(out Plane[] v)); return v; }
    protected override Plane[] TryReadArrayNamed(ReadContext context) { Assert.True(context.TryReadPlanes(out Plane[] v)); return v; }
    protected override Plane[] TryPeekArrayAlias(ReadContext context) { Assert.True(context.TryPeek(out Plane[] v)); return v; }
    protected override Plane[] TryReadArrayAlias(ReadContext context) { Assert.True(context.TryRead(out Plane[] v)); return v; }

    protected override void WriteArrayWithoutLengthNamed(WriteContext context, Plane[] values) => context.WritePlanesWithoutLength(values);
    protected override Plane[] PeekArrayWithoutLengthNamed(ReadContext context, int count) => context.PeekPlanes(count);
    protected override Plane[] ReadArrayWithoutLengthNamed(ReadContext context, int count) => context.ReadPlanes(count);
    protected override void WriteArrayWithoutLengthAlias(WriteContext context, Plane[] values) => context.WriteWithoutLength(values);
    protected override Plane[] PeekArrayWithoutLengthAlias(ReadContext context, int count) { context.Peek(count, out Plane[] v); return v; }
    protected override Plane[] ReadArrayWithoutLengthAlias(ReadContext context, int count) { context.Read(count, out Plane[] v); return v; }
    protected override Plane[] TryPeekArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryPeekPlanes(count, out Plane[] v)); return v; }
    protected override Plane[] TryReadArrayWithoutLengthNamed(ReadContext context, int count) { Assert.True(context.TryReadPlanes(count, out Plane[] v)); return v; }
    protected override Plane[] TryPeekArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryPeek(count, out Plane[] v)); return v; }
    protected override Plane[] TryReadArrayWithoutLengthAlias(ReadContext context, int count) { Assert.True(context.TryRead(count, out Plane[] v)); return v; }

    protected override void WriteSpanNamed(WriteContext context, Span<Plane> values) => context.WritePlanes(values);
    protected override void PeekSpanNamed(ReadContext context, Span<Plane> destination) => context.PeekPlanes(destination);
    protected override void ReadSpanNamed(ReadContext context, Span<Plane> destination) => context.ReadPlanes(destination);
    protected override void WriteSpanAlias(WriteContext context, Span<Plane> values) => context.Write(values);
    protected override void PeekSpanAlias(ReadContext context, Span<Plane> destination) => context.Peek(destination);
    protected override void ReadSpanAlias(ReadContext context, Span<Plane> destination) => context.Read(destination);
    protected override void TryPeekSpanNamed(ReadContext context, Span<Plane> destination) { Assert.True(context.TryPeekPlanes(destination)); }
    protected override void TryReadSpanNamed(ReadContext context, Span<Plane> destination) { Assert.True(context.TryReadPlanes(destination)); }
    protected override void TryPeekSpanAlias(ReadContext context, Span<Plane> destination) { Assert.True(context.TryPeek(destination)); }
    protected override void TryReadSpanAlias(ReadContext context, Span<Plane> destination) { Assert.True(context.TryRead(destination)); }

    protected override void WriteSpanWithoutLengthNamed(WriteContext context, Span<Plane> values) => context.WritePlanesWithoutLength(values);
    protected override void PeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Plane> destination) => context.PeekPlanes(count, destination);
    protected override void ReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Plane> destination) => context.ReadPlanes(count, destination);
    protected override void WriteSpanWithoutLengthAlias(WriteContext context, Span<Plane> values) => context.WriteWithoutLength(values);
    protected override void PeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Plane> destination) => context.Peek(count, destination);
    protected override void ReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Plane> destination) => context.Read(count, destination);
    protected override void TryPeekSpanWithoutLengthNamed(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryPeekPlanes(count, destination)); }
    protected override void TryReadSpanWithoutLengthNamed(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryReadPlanes(count, destination)); }
    protected override void TryPeekSpanWithoutLengthAlias(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryPeek(count, destination)); }
    protected override void TryReadSpanWithoutLengthAlias(ReadContext context, int count, Span<Plane> destination) { Assert.True(context.TryRead(count, destination)); }

    protected override int GetSizeInBits(Plane value) => value.GetPlaneSizeInBits();
    protected override bool IsFixedSizeStruct(Plane value) => value.IsPlaneFixedSizeStruct();
}
