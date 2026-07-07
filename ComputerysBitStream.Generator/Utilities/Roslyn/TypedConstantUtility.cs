using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Roslyn;

internal static class TypedConstantUtility {
    public static ImmutableArray<ITypeSymbol> ExtractTypeSymbols(TypedConstant constant) {
        if (constant.Kind == TypedConstantKind.Array) {
            ImmutableArray<ITypeSymbol>.Builder builder = ImmutableArray.CreateBuilder<ITypeSymbol>();
            foreach (TypedConstant value in constant.Values) {
                if (value.Value is ITypeSymbol typeSymbol) { builder.Add(typeSymbol); }
            }

            return builder.ToImmutable();
        }

        if (constant.Value is ITypeSymbol single) { return ImmutableArray.Create(single); }

        return ImmutableArray<ITypeSymbol>.Empty;
    }
}
