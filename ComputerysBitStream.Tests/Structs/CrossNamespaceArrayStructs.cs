namespace ComputerysBitStream.Tests.ArrayElements {
    [BitStreamStruct]
    public partial struct CrossNamespaceArrayElement {
        public int Value { get; set; }
    }
}

namespace ComputerysBitStream.Tests.Structs {
    [BitStreamSettings]
    [BitStreamSerializer(typeof(ArrayElements.CrossNamespaceArrayElement))]
    public interface ICrossNamespaceArraySettings : IDefaultSettings { }

    [BitStreamStruct(typeof(ICrossNamespaceArraySettings))]
    public partial struct CrossNamespaceArrayMemberStruct {
        [BitStreamStructCollectionMaxEntries(8)]
        public ArrayElements.CrossNamespaceArrayElement[] Values { get; set; }
    }
}
