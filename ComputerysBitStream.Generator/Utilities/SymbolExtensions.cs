using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal static class SymbolExtensions {
    private static readonly SymbolDisplayFormat FullyQualifiedFormatWithoutUseSpecialTypesAndGlobal = new SymbolDisplayFormat(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions:
        SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
    );

    private static readonly SymbolDisplayFormat EmitTypeFormat = new(
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
    );

    public static string GetFullyQualifiedName(this ISymbol symbol) { return symbol.ToDisplayString(FullyQualifiedFormatWithoutUseSpecialTypesAndGlobal); }

    public static string GetEmitTypeName(this ITypeSymbol symbol) { return symbol.ToDisplayString(EmitTypeFormat); }

    public static string? GetFullyQualifiedNamespace(this ISymbol symbol) {
        INamespaceSymbol? namespaceSymbol = symbol.ContainingNamespace;
        if (namespaceSymbol is null || namespaceSymbol.IsGlobalNamespace) { return null; }

        return namespaceSymbol.ToDisplayString(FullyQualifiedFormatWithoutUseSpecialTypesAndGlobal);
    }

    public static bool IsDefinedIn(this ISymbol symbol, Compilation compilation) { return SymbolEqualityComparer.Default.Equals(symbol.ContainingAssembly, compilation.Assembly); }

    public static Location? GetLocation(this AttributeData attributeData) { return attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(); }

    public static ImmutableDictionary<string, TypedConstant> GetConstructorArgumentsByName(this AttributeData attributeData) {
        ImmutableDictionary<string, TypedConstant>.Builder arguments = ImmutableDictionary.CreateBuilder<string, TypedConstant>();

        IMethodSymbol? constructor = attributeData.AttributeConstructor;
        if (constructor is not null) {
            ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
            ImmutableArray<IParameterSymbol> parameters = constructor.Parameters;
            int count = Math.Min(constructorArguments.Length, parameters.Length);
            for (int i = 0; i < count; i++) { arguments[parameters[i].Name] = constructorArguments[i]; }
        }

        foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments) { arguments[namedArgument.Key] = namedArgument.Value; }

        return arguments.ToImmutable();
    }

    public static bool TryGetConstructorArgumentByName(this AttributeData attributeData, string name, out TypedConstant value) {
        foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments) {
            if (string.Equals(namedArgument.Key, name, StringComparison.Ordinal)) {
                value = namedArgument.Value;
                return true;
            }
        }

        IMethodSymbol? constructor = attributeData.AttributeConstructor;
        if (constructor is not null) {
            ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
            ImmutableArray<IParameterSymbol> parameters = constructor.Parameters;
            int count = Math.Min(constructorArguments.Length, parameters.Length);
            for (int i = 0; i < count; i++) {
                if (string.Equals(parameters[i].Name, name, StringComparison.Ordinal)) {
                    value = constructorArguments[i];
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    public static bool IsAttribute(this AttributeData attributeData, string fullyQualifiedAttributeName) {
        INamedTypeSymbol? attributeClass = attributeData.AttributeClass;
        return attributeClass is not null && string.Equals(attributeClass.ToDisplayString(FullyQualifiedFormatWithoutUseSpecialTypesAndGlobal), fullyQualifiedAttributeName, StringComparison.Ordinal);
    }

    public static bool HasAttribute(this ISymbol symbol, string fullyQualifiedAttributeName) {
        ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
        foreach (AttributeData attribute in attributes) {
            if (attribute.IsAttribute(fullyQualifiedAttributeName)) { return true; }
        }

        return false;
    }

    public static bool IsInTypeWithAttribute(this ISymbol? symbol, string fullyQualifiedAttributeName) {
        INamedTypeSymbol? type = symbol?.ContainingType;
        while (type is not null) {
            if (type.HasAttribute(fullyQualifiedAttributeName)) { return true; }
            type = type.ContainingType;
        }

        return false;
    }

    public static bool TryGetAttribute(this ISymbol symbol, string fullyQualifiedAttributeName, [NotNullWhen(true)] out AttributeData? attributeData) {
        ImmutableArray<AttributeData> attributes = symbol.GetAttributes();
        foreach (AttributeData attribute in attributes) {
            if (!attribute.IsAttribute(fullyQualifiedAttributeName)) { continue; }

            attributeData = attribute;
            return true;
        }

        attributeData = null;
        return false;
    }

    public static bool HasRestrictedPrimitiveMethodAttribute(this ISymbol symbol, INamedTypeSymbol restrictedAttribute) {
        foreach (AttributeData attribute in symbol.GetAttributes()) {
            if (IsOrDerivesFrom(attribute.AttributeClass, restrictedAttribute)) { return true; }
        }

        return false;
    }

    private static bool IsOrDerivesFrom(INamedTypeSymbol? symbol, INamedTypeSymbol expected) {
        while (symbol is not null) {
            if (SymbolEqualityComparer.Default.Equals(symbol, expected)) { return true; }
            symbol = symbol.BaseType;
        }

        return false;
    }
}
