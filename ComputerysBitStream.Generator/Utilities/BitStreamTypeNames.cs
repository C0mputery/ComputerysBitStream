using System;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator;

internal static class BitStreamTypeNames {
    public static readonly string DefaultSettings = typeof(DefaultBitStreamSettingsAttribute).FullName!;
    public static readonly string Settings = typeof(BitStreamSettingsAttribute).FullName!;
    public static readonly string Serializer = typeof(BitStreamSerializerAttribute).FullName!;

    public static readonly string Primitive = typeof(BitStreamPrimitiveAttribute).FullName!;
    public static readonly string RestrictedPrimitiveMethod = typeof(BitStreamRestrictedPrimitiveMethodAttribute).FullName!;
    public static readonly string PrimitiveMethod = typeof(BitStreamPrimitiveMethodAttribute).FullName!;
    public static readonly string PrimitiveContext = typeof(BitStreamPrimitiveContextAttribute).FullName!;
    public static readonly string FixedSizePrimitive = typeof(BitStreamFixedSizePrimitiveAttribute).FullName!;
    public static readonly string QuantizedPrimitive = typeof(BitStreamQuantizedPrimitiveAttribute).FullName!;

    public static readonly string Struct = typeof(BitStreamStructAttribute).FullName!;
    public static readonly string StructInclude = typeof(BitStreamStructIncludeAttribute).FullName!;
    public static readonly string StructIgnore = typeof(BitStreamStructIgnoreAttribute).FullName!;
    public static readonly string ProxyStruct = typeof(BitStreamProxyStructAttribute).FullName!;
    public static readonly string StructMetadata = typeof(BitStreamStructMetadataAttribute).FullName!;
    public static readonly string StructQuantized = typeof(BitStreamStructQuantizedAttribute).FullName!;
    public static readonly string StructVariableLength = typeof(BitStreamStructVariableLengthAttribute).FullName!;
    public static readonly string StructCollectionMaxEntries = typeof(BitStreamStructCollectionMaxEntriesAttribute).FullName!;

    public static readonly string UInt32 = typeof(uint).FullName!;

    public static readonly string WriteContext = typeof(WriteContext).FullName!;
    public static readonly string ReadContext = typeof(ReadContext).FullName!;
    public static readonly string DefaultSettingsInterface = typeof(IDefaultSettings).FullName!;
    public static readonly string ReadOnlySpan = typeof(ReadOnlySpan<>).FullName!;
    public static readonly string Span = typeof(Span<>).FullName!;
}
