using ComputerysBitStream.Primitives.VariableLength;
using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class MemberSerializerOverrideTests {
    [Fact]
    public void BitStreamSerializer_ShouldOverrideVariableLengthAttribute() {
        const int value = 5;

        ulong[] buffer = new ulong[TestConstants.BufferWordCount];
        WriteContext variableContext = new(buffer, 0);
        variableContext.WriteVariableLengthInt(value);
        int variableBits = (int)variableContext.Position;
        Assert.True(variableBits < 32);

        WriteContext structContext = new(buffer, 0);
        structContext.WriteMemberSerializerOverrideStruct(new MemberSerializerOverrideStruct {
            VariableLengthValue = value,
            FixedOverrideValue = value,
        });

        Assert.Equal(variableBits + 32, structContext.Position);
    }

    [Fact]
    public void ShouldRoundTrip() {
        MemberSerializerOverrideStruct original = new() { VariableLengthValue = 42, FixedOverrideValue = 12345 };

        RoundTripTestHarness<MemberSerializerOverrideStruct>.AssertSingleValueRoundTrip(
            0,
            original,
            static (ref WriteContext context, MemberSerializerOverrideStruct value) => context.WriteMemberSerializerOverrideStruct(value),
            static context => context.PeekMemberSerializerOverrideStruct(),
            static context => context.ReadMemberSerializerOverrideStruct(),
            AssertEqual
        );
        return;

        static void AssertEqual(MemberSerializerOverrideStruct expected, MemberSerializerOverrideStruct actual) {
            Assert.Equal(expected.VariableLengthValue, actual.VariableLengthValue);
            Assert.Equal(expected.FixedOverrideValue, actual.FixedOverrideValue);
        }
    }
}
