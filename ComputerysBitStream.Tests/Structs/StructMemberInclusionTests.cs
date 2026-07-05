namespace ComputerysBitStream.Tests;

public class StructMemberInclusionTests {
    [Fact]
    public void DefaultPublicProperties_AreSerialized() {
        MemberInclusionStruct original = new() {
            Health = 100,
            Speed = 4.5f,
            DebugOnly = 999,
            PublicField = 50,
            IncludedField = 60,
        };

        RoundTripTestHarness<MemberInclusionStruct>.AssertSingleValueRoundTrip(
            0,
            original,
            static (ref WriteContext context, MemberInclusionStruct value) => context.WriteMemberInclusionStruct(value),
            static context => context.PeekMemberInclusionStruct(),
            static context => context.ReadMemberInclusionStruct(),
            static (expected, actual) => {
                Assert.Equal(expected.Health, actual.Health);
                Assert.Equal(expected.Speed, actual.Speed);
                Assert.Equal(0, actual.DebugOnly);
                Assert.Equal(0, actual.PublicField);
                Assert.Equal(expected.IncludedField, actual.IncludedField);
            }
        );
    }

    [Fact]
    public void IgnoredProperty_DoesNotAffectSerializedSize() {
        MemberInclusionStruct withDebug = new() { Health = 1, Speed = 1f, DebugOnly = int.MaxValue, IncludedField = 3 };
        MemberInclusionStruct withoutDebug = new() { Health = 1, Speed = 1f, DebugOnly = 0, IncludedField = 3 };

        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext writeWith = new(buffer, 0);
        writeWith.WriteMemberInclusionStruct(withDebug);
        int withBits = (int)writeWith.Position;

        WriteContext writeWithout = new(buffer, 0);
        writeWithout.WriteMemberInclusionStruct(withoutDebug);
        int withoutBits = (int)writeWithout.Position;

        Assert.Equal(withBits, withoutBits);
    }
}
