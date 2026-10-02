using ComputerysBitStream.Tests.Structs.Types;
using ComputerysBitStream.Tests.Utilities;

namespace ComputerysBitStream.Tests.Structs;

public class MixedIntStructTests {
    [Fact]
    public void ShouldBeVariableLength_WhenAnyMemberUsesVariableLengthEncoding() {
        Assert.True(StructMetadataAssertions.GetMetadataSize(typeof(MixedIntStruct)) < 0);
    }
}
