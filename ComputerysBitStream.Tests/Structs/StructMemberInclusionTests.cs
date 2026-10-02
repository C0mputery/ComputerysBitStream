using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

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
            AssertEqual
        );
        return;

        static void AssertEqual(MemberInclusionStruct expected, MemberInclusionStruct actual) {
            Assert.Equal(expected.Health, actual.Health);
            Assert.Equal(expected.Speed, actual.Speed);
            Assert.Equal(0, actual.DebugOnly);
            Assert.Equal(0, actual.PublicField);
            Assert.Equal(expected.IncludedField, actual.IncludedField);
        }
    }
}
