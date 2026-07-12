using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Roslyn;

internal readonly record struct SignatureValidation(bool IsValid, string? ExpectedSignature);

internal readonly record struct MethodSignatureRule(ITypeSymbol ReturnType, ParameterSpec[] Parameters, string ExpectedSignature, bool RequiresRefExtension = true) {
    public SignatureValidation Validate(IMethodSymbol method) {
        if (RequiresRefExtension && !MethodSignatureUtility.IsRefExtensionMethod(method)) {
            return new SignatureValidation(IsValid: false, ExpectedSignature: ExpectedSignature);
        }

        return new SignatureValidation(
            MethodSignatureUtility.MatchesSignature(method, ReturnType, Parameters),
            ExpectedSignature
        );
    }
}
