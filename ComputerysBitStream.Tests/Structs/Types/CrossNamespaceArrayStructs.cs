using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Tests.Structs.Types;

[BitStreamStruct]
public partial struct CrossNamespaceArrayElement {
    public int Value { get; set; }
}

[BitStreamSettings]
[BitStreamSerializer(typeof(CrossNamespaceArrayElement))]
public interface ICrossNamespaceArraySettings : IDefaultSettings { }

[BitStreamStruct(typeof(ICrossNamespaceArraySettings))]
public partial struct CrossNamespaceArrayMemberStruct {
    [BitStreamStructCollectionMaxEntries(8)]
    public CrossNamespaceArrayElement[] Values { get; set; }
}
