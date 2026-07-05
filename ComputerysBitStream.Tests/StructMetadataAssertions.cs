using System.Reflection;

namespace ComputerysBitStream.Tests;

public static class StructMetadataAssertions {
    public static int GetMetadataSize(Type type) {
        CustomAttributeData? data = type.GetCustomAttributesData()
            .FirstOrDefault(attribute => attribute.AttributeType == typeof(BitStreamStructMetadataAttribute));

        Assert.NotNull(data);
        return (int)data.ConstructorArguments[0].Value!;
    }

    public static bool IsFixedSize(Type type) => GetMetadataSize(type) > 0;
}
