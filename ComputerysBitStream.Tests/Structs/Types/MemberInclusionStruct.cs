using ComputerysBitStream.Attributes;
using ComputerysBitStream.Primitives.FixedSize;

namespace ComputerysBitStream.Tests.Structs.Types;

[BitStreamStruct]
public partial struct MemberInclusionStruct {
    public int Health { get; set; }

    [BitStreamSerializer(typeof(PrimitiveFloatExtensions))]
    public float Speed { get; set; }

    [BitStreamStructIgnore] public int DebugOnly { get; set; }
    public int PublicField;
    [BitStreamStructInclude] public int IncludedField;
}
