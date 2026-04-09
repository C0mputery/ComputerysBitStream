using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator;

// Yeah so this is just not a thing built into c# and a bunch of ppl better at C# than me do this soooooooooooo
// https://github.com/dotnet/runtime/issues/125409 mircoslop enginer
internal readonly struct EquatableImmutableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableImmutableArray<T>>, IEnumerable<T> where T : IEquatable<T> {
    private readonly ImmutableArray<T> _array = array;

    public bool Equals(EquatableImmutableArray<T> other) { return _array.SequenceEqual(other._array); }
    public override bool Equals(object? obj) { return obj is EquatableImmutableArray<T> other && Equals(other); }

    public override int GetHashCode() {
        int hash = 17;
        foreach (T? element in _array) { hash = hash * 31 + (element?.GetHashCode() ?? 0); }
        return hash;
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator EquatableImmutableArray<T>(ImmutableArray<T> array) => new(array);
    public static implicit operator ImmutableArray<T>(EquatableImmutableArray<T> array) => array._array;

    public bool IsDefaultOrEmpty => _array.IsDefaultOrEmpty;
}

internal readonly record struct ValueTypeLocation(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan) {
    public static implicit operator ValueTypeLocation?(Location? location) {
        if (location == null) { return null; }
        FileLinePositionSpan lineSpan = location.GetLineSpan();
        return new ValueTypeLocation(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }
    
    public static implicit operator ValueTypeLocation(Location location) {
        FileLinePositionSpan lineSpan = location.GetLineSpan();
        return new ValueTypeLocation(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }
    
    public readonly Location ToLocation() { return Location.Create(FilePath, TextSpan, LineSpan); }
}

internal static class SymbolExtensions {
    private static readonly SymbolDisplayFormat FullyQualifiedWithoutGlobalFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

    public static bool IsAttribute(this AttributeData attributeData, string fullyQualifiedAttributeName) {
        return attributeData.AttributeClass?.ToDisplayString(FullyQualifiedWithoutGlobalFormat) == fullyQualifiedAttributeName;
    }

    public static bool HasAttribute(this ISymbol symbol, string fullyQualifiedAttributeName) {
        ImmutableArray<AttributeData> attributeDatas = symbol.GetAttributes();
        foreach (AttributeData attribute in attributeDatas) {
            if (attribute.IsAttribute(fullyQualifiedAttributeName)) { return true; }
        }

        return false;
    }
    
    public static bool TryGetAttribute(this ISymbol symbol, string fullyQualifiedAttributeName, [NotNullWhen(true)] out AttributeData? attributeData) {
        ImmutableArray<AttributeData> attributeDatas = symbol.GetAttributes();
        foreach (AttributeData attribute in attributeDatas) {
            if (!attribute.IsAttribute(fullyQualifiedAttributeName)) { continue; }
            attributeData = attribute;
            return true;
        }

        attributeData = null;
        return false;
    }
}

