using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator;

internal static class RawTypeCollector {
    public static readonly string RawTypeAttribute = typeof(BitStreamRawTypeAttribute).FullName!;
    private static readonly string RawMethodAttribute = typeof(BitStreamRawMethodAttribute).FullName!;

    public static IncrementalValuesProvider<RawData> GetBitStreamRawAttributeData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: RawTypeAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax,
            transform: RawTypeAttributeDataTransform
        );
    }

    private static RawData RawTypeAttributeDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        AttributeData attributeData = context.Attributes[0];
        return RawTypeAttributeData(attributeData, (INamedTypeSymbol)context.TargetSymbol, context.SemanticModel.Compilation);
    }

    public static RawData RawTypeAttributeData(AttributeData attributeData, INamedTypeSymbol targetTypeSymbol, Compilation compilation) {
        ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;

        ITypeSymbol type = (ITypeSymbol)constructorArguments[0].Value!;
        int size = (int)constructorArguments[1].Value!;

        string alias = constructorArguments.Length switch {
            2 => DisplayNameUtility.GetDisplayName(type),
            3 => (string?)constructorArguments[2].Value ?? DisplayNameUtility.GetDisplayName(type),
            _ => throw new ArgumentOutOfRangeException()
        };

        List<DiagnosticData> diagnostics = [];
        Location? attributeLocation = attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation();

        if (!targetTypeSymbol.IsStatic) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.RawTypeClassNotStatic, attributeLocation, [targetTypeSymbol.Name]));
        }

        if (size <= 0) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidSize, attributeLocation, [size.ToString()]));
        }

        INamedTypeSymbol? writeContextType = compilation.GetTypeByMetadataName(typeof(WriteContext).FullName!);
        INamedTypeSymbol? readContextType = compilation.GetTypeByMetadataName(typeof(ReadContext).FullName!);
        INamedTypeSymbol? readOnlySpanType = compilation.GetTypeByMetadataName(typeof(ReadOnlySpan<>).FullName!);
        INamedTypeSymbol? spanType = compilation.GetTypeByMetadataName(typeof(Span<>).FullName!);
        ITypeSymbol? readOnlySpanOfTarget = readOnlySpanType?.Construct(type);
        ITypeSymbol? spanOfTarget = spanType?.Construct(type);
        ITypeSymbol? arrayOfTarget = compilation.CreateArrayTypeSymbol(type);
        ITypeSymbol intType = compilation.GetSpecialType(SpecialType.System_Int32);

        IEnumerable<IMethodSymbol> methods = targetTypeSymbol.GetMembers().OfType<IMethodSymbol>();
        List<RawMethodData> methodAttributes = [];
        foreach (IMethodSymbol member in methods) {
            ImmutableArray<AttributeData> memberAttributes = member.GetAttributes();
            foreach (AttributeData methodAttribute in memberAttributes) {
                if (!methodAttribute.IsAttribute(RawMethodAttribute)) { continue; }

                if (!member.IsStatic || member.DeclaredAccessibility != Accessibility.Public) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.MethodNotPublicStatic, methodAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [member.Name]));
                }

                BitStreamRawRole role = (BitStreamRawRole)methodAttribute.ConstructorArguments[0].Value!;
                bool isValid = TryValidateRawMethodSignature(member, role, type, writeContextType, readContextType, readOnlySpanOfTarget, spanOfTarget, arrayOfTarget, intType, compilation, out string? expectedSignature);

                if (expectedSignature != null && !isValid) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidRawMethodSignature, methodAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [member.Name, role.ToString(), expectedSignature]));
                }

                methodAttributes.Add(
                    new RawMethodData(
                        Role: role,
                        MethodName: member.Name,
                        Location: methodAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                    )
                );
            }
        }

        if (methodAttributes.Count == 0) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.NoRawMethods, attributeLocation, [targetTypeSymbol.Name]));
        }

        return new RawData(
            TargetTypeFullyQualifiedName: type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Alias: alias,
            Size: size,
            Methods: methodAttributes.ToImmutableArray(),
            Location: attributeLocation,
            Diagnostics: diagnostics.ToImmutableArray()
        );
    }

    private static bool TryValidateRawMethodSignature(
        IMethodSymbol method, BitStreamRawRole role, ITypeSymbol targetType,
        INamedTypeSymbol? writeContextType, INamedTypeSymbol? readContextType, ITypeSymbol? readOnlySpanOfTarget,
        ITypeSymbol? spanOfTarget, ITypeSymbol? arrayOfTarget, ITypeSymbol intType,
        Compilation compilation, out string? expectedSignature
    ) {
        if (method.Parameters.Length == 0 || !method.IsExtensionMethod || method.Parameters[0].RefKind != RefKind.Ref) {
            expectedSignature = null;
            return false;
        }

        string typeName = targetType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        ITypeSymbol voidType = compilation.GetSpecialType(SpecialType.System_Void);

        switch (role) {
            case BitStreamRawRole.Write:
                expectedSignature = $"public static void MethodName(this ref WriteContext context, {typeName} value)";
                return MatchesSignature(method, voidType, new ParameterSpec(writeContextType, RefKind.Ref), new ParameterSpec(targetType, null));
            case BitStreamRawRole.WriteSpan:
                expectedSignature = $"public static void MethodName(this ref WriteContext context, ReadOnlySpan<{typeName}> values)";
                return MatchesSignature(method, voidType, new ParameterSpec(writeContextType, RefKind.Ref), new ParameterSpec(readOnlySpanOfTarget, null));
            case BitStreamRawRole.Peek:
            case BitStreamRawRole.Read:
                expectedSignature = $"public static {typeName} MethodName(this ref ReadContext context)";
                return MatchesSignature(method, targetType, new ParameterSpec(readContextType, RefKind.Ref));
            case BitStreamRawRole.PeekArray:
            case BitStreamRawRole.ReadArray:
                expectedSignature = $"public static {typeName}[] MethodName(this ref ReadContext context, int count)";
                return MatchesSignature(method, arrayOfTarget, new ParameterSpec(readContextType, RefKind.Ref), new ParameterSpec(intType, null));
            case BitStreamRawRole.PeekSpan:
            case BitStreamRawRole.ReadSpan:
                expectedSignature = $"public static void MethodName(this ref ReadContext context, int count, Span<{typeName}> destination)";
                return MatchesSignature(method, voidType, new ParameterSpec(readContextType, RefKind.Ref), new ParameterSpec(intType, null), new ParameterSpec(spanOfTarget, null));
            default:
                throw new ArgumentOutOfRangeException(nameof(role), role, null);
        }
    }

    private readonly record struct ParameterSpec(ITypeSymbol? Type, RefKind? RefKind);

    private static bool MatchesSignature(IMethodSymbol method, ITypeSymbol? returnType, params ParameterSpec[] parameters) {
        if (!MatchesReturnType(method, returnType)) return false;
        if (method.Parameters.Length != parameters.Length) return false;

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
