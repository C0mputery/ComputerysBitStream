using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator;

internal record struct RawData(
    string TargetTypeFullyQualifiedName,
    string Alias,
    int Size,
    EquatableImmutableArray<RawMethodData> Methods,
    ValueTypeLocation? Location,
    EquatableImmutableArray<DiagnosticData> Diagnostics = default
);

internal record struct RawMethodData(
    BitStreamRawRole Role,
    string MethodName,
    ValueTypeLocation? Location
);

public partial class IncrementalGenerator : IIncrementalGenerator {
    private static readonly string RawTypeAttribute = typeof(BitStreamRawTypeAttribute).FullName!;
    private static readonly string RawMethodAttribute = typeof(BitStreamRawMethodAttribute).FullName!;
    private static IncrementalValuesProvider<RawData> GetBitStreamRawAttributeData(IncrementalGeneratorInitializationContext context) {
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
    
    private static RawData RawTypeAttributeData(AttributeData attributeData, INamedTypeSymbol targetTypeSymbol, Compilation compilation) {
        ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;

        ITypeSymbol type = (ITypeSymbol)constructorArguments[0].Value!;
        int size = (int)constructorArguments[1].Value!;
        string alias;
        if (constructorArguments.Length > 2) { alias = (string)constructorArguments[2].Value!; }
        else { alias = DisplayNameUtility.GetDisplayName(type); }

        List<DiagnosticData> diagnostics = [];
        Location? attributeLocation = attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation();

        if (!targetTypeSymbol.IsStatic) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.RawTypeClassNotStatic, attributeLocation, [targetTypeSymbol.Name]));
        }

        if (size <= 0) {
            diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidSize, attributeLocation, [size.ToString()]));
        }

        INamedTypeSymbol? writeContextType = compilation.GetTypeByMetadataName("ComputerysBitStream.WriteContext");
        INamedTypeSymbol? readContextType = compilation.GetTypeByMetadataName("ComputerysBitStream.ReadContext");
        INamedTypeSymbol? readOnlySpanType = compilation.GetTypeByMetadataName("System.ReadOnlySpan`1");
        INamedTypeSymbol? spanType = compilation.GetTypeByMetadataName("System.Span`1");
        ITypeSymbol? readOnlySpanOfTarget = readOnlySpanType?.Construct(type);
        ITypeSymbol? spanOfTarget = spanType?.Construct(type);
        ITypeSymbol? arrayOfTarget = compilation.CreateArrayTypeSymbol(type);
        ITypeSymbol intType = compilation.GetSpecialType(SpecialType.System_Int32);

        IEnumerable<IMethodSymbol> methods = targetTypeSymbol.GetMembers().OfType<IMethodSymbol>();
        List<RawMethodData> methodAttributes = [];
        foreach (IMethodSymbol member in methods) {
            ImmutableArray<AttributeData> memberAttributes = member.GetAttributes();
            foreach (AttributeData methodAttribute in memberAttributes) {
                if (methodAttribute.IsAttribute(RawMethodAttribute)) {
                    if (!member.IsStatic || member.DeclaredAccessibility != Accessibility.Public) {
                        diagnostics.Add(DiagnosticData.Create(Diagnostics.MethodNotPublicStatic, methodAttribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(), [member.Name]));
                    }

                    BitStreamRawRole role = (BitStreamRawRole)methodAttribute.ConstructorArguments[0].Value!;
                    string? expectedSignature = GetExpectedSignature(role, type, alias, readOnlySpanOfTarget, spanOfTarget);
                    if (expectedSignature != null && !IsSignatureValid(member, role, type, writeContextType, readContextType, readOnlySpanOfTarget, spanOfTarget, arrayOfTarget, intType, compilation)) {
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

    private static string? GetExpectedSignature(BitStreamRawRole role, ITypeSymbol targetType, string alias, ITypeSymbol? readOnlySpanOfTarget, ITypeSymbol? spanOfTarget) {
        string typeName = targetType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        return role switch {
            BitStreamRawRole.Write => $"public static void MethodName(this ref WriteContext context, {typeName} value)",
            BitStreamRawRole.WriteSpan => $"public static void MethodName(this ref WriteContext context, ReadOnlySpan<{typeName}> values)",
            BitStreamRawRole.Peek => $"public static {typeName} MethodName(this ref ReadContext context)",
            BitStreamRawRole.Read => $"public static {typeName} MethodName(this ref ReadContext context)",
            BitStreamRawRole.PeekArray => $"public static {typeName}[] MethodName(this ref ReadContext context, int count)",
            BitStreamRawRole.ReadArray => $"public static {typeName}[] MethodName(this ref ReadContext context, int count)",
            BitStreamRawRole.PeekSpan => $"public static void MethodName(this ref ReadContext context, int count, ref Span<{typeName}> destination)",
            BitStreamRawRole.ReadSpan => $"public static void MethodName(this ref ReadContext context, int count, ref Span<{typeName}> destination)",
            BitStreamRawRole.Debug => null,
            _ => null,
        };
    }

    private static bool IsSignatureValid(
        IMethodSymbol method,
        BitStreamRawRole role,
        ITypeSymbol targetType,
        INamedTypeSymbol? writeContextType,
        INamedTypeSymbol? readContextType,
        ITypeSymbol? readOnlySpanOfTarget,
        ITypeSymbol? spanOfTarget,
        ITypeSymbol? arrayOfTarget,
        ITypeSymbol intType,
        Compilation compilation) {
        if (method.Parameters.Length == 0) { return false; }
        if (!method.IsExtensionMethod) { return false; }
        IParameterSymbol firstParam = method.Parameters[0];
        if (firstParam.RefKind != RefKind.Ref) { return false; }

        ITypeSymbol voidType = compilation.GetSpecialType(SpecialType.System_Void);

        bool FirstParamIs(ITypeSymbol? expected) => expected != null && SymbolEqualityComparer.Default.Equals(firstParam.Type, expected);
        bool ReturnIs(ITypeSymbol? expected) => expected != null && SymbolEqualityComparer.Default.Equals(method.ReturnType, expected);
        bool ParamIs(int index, ITypeSymbol? expected) => expected != null && index < method.Parameters.Length && SymbolEqualityComparer.Default.Equals(method.Parameters[index].Type, expected);
        bool ParamIsRef(int index, ITypeSymbol? expected) => expected != null && index < method.Parameters.Length && method.Parameters[index].RefKind == RefKind.Ref && SymbolEqualityComparer.Default.Equals(method.Parameters[index].Type, expected);

        return role switch {
            BitStreamRawRole.Write => FirstParamIs(writeContextType) && ReturnIs(voidType) && method.Parameters.Length == 2 && ParamIs(1, targetType),
            BitStreamRawRole.WriteSpan => FirstParamIs(writeContextType) && ReturnIs(voidType) && method.Parameters.Length == 2 && ParamIs(1, readOnlySpanOfTarget),
            BitStreamRawRole.Peek => FirstParamIs(readContextType) && ReturnIs(targetType) && method.Parameters.Length == 1,
            BitStreamRawRole.Read => FirstParamIs(readContextType) && ReturnIs(targetType) && method.Parameters.Length == 1,
            BitStreamRawRole.PeekArray => FirstParamIs(readContextType) && ReturnIs(arrayOfTarget) && method.Parameters.Length == 2 && ParamIs(1, intType),
            BitStreamRawRole.ReadArray => FirstParamIs(readContextType) && ReturnIs(arrayOfTarget) && method.Parameters.Length == 2 && ParamIs(1, intType),
            BitStreamRawRole.PeekSpan => FirstParamIs(readContextType) && ReturnIs(voidType) && method.Parameters.Length == 3 && ParamIs(1, intType) && ParamIsRef(2, spanOfTarget),
            BitStreamRawRole.ReadSpan => FirstParamIs(readContextType) && ReturnIs(voidType) && method.Parameters.Length == 3 && ParamIs(1, intType) && ParamIsRef(2, spanOfTarget),
            BitStreamRawRole.Debug => true,
            _ => false,
        };
    }
}
