using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Roslyn;

internal readonly record struct ParameterSpec(ITypeSymbol? Type, RefKind? RefKind);

internal static class MethodSignatureUtility {
    internal static ParameterSpec Ref(ITypeSymbol? type) => new(type, RefKind.Ref);

    internal static ParameterSpec Value(ITypeSymbol? type) => new(type, null);

    internal static ParameterSpec Out(ITypeSymbol? type) => new(type, RefKind.Out);

    internal static bool IsRefExtensionMethod(IMethodSymbol method) => method.Parameters.Length > 0 && method.IsExtensionMethod && method.Parameters[0].RefKind == RefKind.Ref;

    internal static bool MatchesSignature(IMethodSymbol method, ITypeSymbol? returnType, params ParameterSpec[] parameters) {
        if (!MatchesReturnType(method, returnType)) { return false; }
        if (method.Parameters.Length != parameters.Length) { return false; }

        for (int i = 0; i < parameters.Length; i++) {
            ParameterSpec parameter = parameters[i];
            if (!MatchesParameter(method, i, parameter.Type, parameter.RefKind)) { return false; }
        }

        return true;
    }

    private static bool MatchesReturnType(IMethodSymbol method, ITypeSymbol? expected) => expected != null && SymbolEqualityComparer.Default.Equals(method.ReturnType, expected);

    private static bool MatchesParameter(IMethodSymbol method, int index, ITypeSymbol? expectedType, RefKind? expectedRefKind = null) {
        if (expectedType == null || index >= method.Parameters.Length) { return false; }

        IParameterSymbol parameter = method.Parameters[index];
        if (!SymbolEqualityComparer.Default.Equals(parameter.Type, expectedType)) { return false; }

        return !expectedRefKind.HasValue || parameter.RefKind == expectedRefKind.Value;
    }
}
