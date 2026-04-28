using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal static class SymbolExtensions {
    private static readonly SymbolDisplayFormat FullyQualifiedWithoutGlobalFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

    public static string GetFullyQualifiedWithoutGlobalFormat(this ISymbol symbol) { return symbol.ToDisplayString(FullyQualifiedWithoutGlobalFormat); }
    
    public static bool IsAttribute(this AttributeData attributeData, string fullyQualifiedAttributeName) {
        return attributeData.AttributeClass?.ToDisplayString(FullyQualifiedWithoutGlobalFormat) == fullyQualifiedAttributeName;
    }

    public static bool HasAttribute(this ISymbol symbol, string fullyQualifiedAttributeName) {
        ImmutableArray<AttributeData> attributeDataArray = symbol.GetAttributes();
        foreach (AttributeData attribute in attributeDataArray) {
            if (attribute.IsAttribute(fullyQualifiedAttributeName)) { return true; }
        }

        return false;
    }
    
    public static bool TryGetAttribute(this ISymbol symbol, string fullyQualifiedAttributeName, [NotNullWhen(true)] out AttributeData? attributeData) {
        ImmutableArray<AttributeData> attributeDataArray = symbol.GetAttributes();
        foreach (AttributeData attribute in attributeDataArray) {
            if (!attribute.IsAttribute(fullyQualifiedAttributeName)) { continue; }
            attributeData = attribute;
            return true;
        }

        attributeData = null;
        return false;
    }
}