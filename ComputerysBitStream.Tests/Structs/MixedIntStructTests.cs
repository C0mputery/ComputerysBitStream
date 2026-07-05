namespace ComputerysBitStream.Tests.Structs;

public class MixedIntStructTests {
    [Fact]
    public void MixedFixedAndVariableLengthMembers_ShouldRoundTrip() {
        MixedIntStruct original = new() { FixedValue = 12345, VariableValue = 42 };

        RoundTripTestHarness<MixedIntStruct>.AssertSingleValueRoundTrip(
            0,
            original,
            static (ref WriteContext context, MixedIntStruct value) => context.WriteMixedIntStruct(value),
            static context => context.PeekMixedIntStruct(),
            static context => context.ReadMixedIntStruct(),
            AssertEqual
        );
        return;

        static void AssertEqual(MixedIntStruct expected, MixedIntStruct actual) {
            Assert.Equal(expected.FixedValue, actual.FixedValue);
            Assert.Equal(expected.VariableValue, actual.VariableValue);
        }
    }

    [Fact]
    public void ShouldBeVariableLength_WhenAnyMemberUsesVariableLengthEncoding() {
        Assert.True(StructMetadataAssertions.GetMetadataSize(typeof(MixedIntStruct)) < 0);
    }
}
