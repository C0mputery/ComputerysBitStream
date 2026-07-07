using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Roslyn;

internal static class AttributeArgumentUtility {
    public static bool TryGetValue<T>(this TypedConstant constant, [NotNullWhen(true)] out T value) {
        if (constant.Value is T directValue) {
            value = directValue;
            return true;
        }

        Type targetType = typeof(T);
        if (targetType.IsEnum && constant.Value is int intValue && Enum.IsDefined(targetType, intValue)) {
            value = (T)Enum.ToObject(targetType, intValue);
            return true;
        }

        value = default!;
        return false;
    }

    public static bool TryGetValue<T>(this AttributeData attributeData, string name, [NotNullWhen(true)] out T value) {
        if (attributeData.TryGetConstructorArgumentByName(name, out TypedConstant argument)) { return argument.TryGetValue(out value); }

        value = default!;
        return false;
    }

    public static bool TryGetValue<T>(this ImmutableDictionary<string, TypedConstant> arguments, string name, [NotNullWhen(true)] out T value) {
        if (arguments.TryGetValue(name, out TypedConstant argument)) { return argument.TryGetValue(out value); }

        value = default!;
        return false;
    }
}
