using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator.Roslyn;

internal readonly record struct SignatureValidation(bool IsValid, string? ExpectedSignature);

internal readonly record struct MethodSignatureRule(
    ITypeSymbol ReturnType,
    ParameterSpec[] Parameters,
    string ExpectedSignature,
    bool RequiresRefExtension = true
) {
    public MethodSignatureRule AppendParameters(ParameterSpec[] additionalParameters, string additionalSignatureSuffix) => new(
        ReturnType,
        [..Parameters, ..additionalParameters],
        ExpectedSignature.TrimEnd(')') + additionalSignatureSuffix,
        RequiresRefExtension
    );
}

internal static class MethodSignatureValidator {
    public static SignatureValidation Validate(IMethodSymbol method, MethodSignatureRule rule) {
        if (rule.RequiresRefExtension && !MethodSignatureUtility.IsRefExtensionMethod(method)) {
            return new SignatureValidation(IsValid: false, ExpectedSignature: rule.ExpectedSignature);
        }

        return new SignatureValidation(
            MethodSignatureUtility.MatchesSignature(method, rule.ReturnType, rule.Parameters),
            rule.ExpectedSignature
        );
    }
}
