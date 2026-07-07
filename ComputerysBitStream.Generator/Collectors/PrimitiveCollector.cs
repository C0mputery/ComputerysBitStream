using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ComputerysBitStream.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator.Collectors;

internal static class PrimitiveCollector {
    public static IncrementalValuesProvider<Collected<PrimitiveDefinition>> GetPrimitiveData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: BitStreamTypeNames.Primitive,
            predicate: (SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax,
            transform: PrimitiveAttributeDataTransform
        );
    }

    private static Collected<PrimitiveDefinition> PrimitiveAttributeDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        AttributeData attributeData = context.Attributes[0];
        return CollectPrimitiveData(attributeData, (INamedTypeSymbol)context.TargetSymbol, context.SemanticModel.Compilation);
    }

    public static Collected<PrimitiveDefinition> CollectPrimitiveData(AttributeData attributeData, INamedTypeSymbol targetTypeSymbol, Compilation compilation, bool includeSettings = true) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        Location? attributeLocation = attributeData.GetLocation();

        if (!targetTypeSymbol.IsStatic) {
            diagnostics.Add(new DiagnosticValueType(Diagnostics.TypeMustBeStatic, attributeLocation, targetTypeSymbol.Name, "BitStreamPrimitive"));
        }

        if (targetTypeSymbol.DeclaredAccessibility != Accessibility.Public) {
            diagnostics.Add(new DiagnosticValueType(Diagnostics.TypeMustBePublic, attributeLocation, targetTypeSymbol.GetFullyQualifiedName(), "BitStreamPrimitive"));
        }

        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();

        if (!arguments.TryGetValue("target", out ITypeSymbol? targetType)) {
            diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingAttributeArgument, attributeLocation, "target", "BitStreamPrimitive"));
            return CreateInvalidPrimitiveDefinition(targetTypeSymbol, attributeLocation, diagnostics);
        }

        if (!arguments.TryGetValue("serializationMode", out PrimitiveSerializationMode mode)) {
            diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingAttributeArgument, attributeLocation, "serializationMode", "BitStreamPrimitive"));
            return CreateInvalidPrimitiveDefinition(targetTypeSymbol, targetType, attributeLocation, diagnostics);
        }

        string alias = arguments.TryGetValue("alias", out string? aliasValue) ? aliasValue : string.Empty;
        alias = string.IsNullOrEmpty(alias) ? DisplayNameUtility.GetDisplayName(targetType) : alias;
        int? fixedSize = null;
        int? minBits = null;
        int? maxBits = null;

        switch (mode) {
            case PrimitiveSerializationMode.FixedSize:
                if (targetTypeSymbol.TryGetAttribute(BitStreamTypeNames.FixedSizePrimitive, out AttributeData? fixedSizeAttribute)) {
                    if (!fixedSizeAttribute.TryGetValue("size", out int parsedFixedSize)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingAttributeArgument, attributeLocation, "size", "BitStreamFixedSizePrimitive"));
                    }
                    else {
                        fixedSize = parsedFixedSize;
                        if (fixedSize <= 0) { diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidFixedSize, attributeLocation, fixedSize.ToString())); }
                    }
                }
                else { diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingCompanionAttribute, attributeLocation, targetTypeSymbol.Name, "FixedSize", "BitStreamFixedSizePrimitive")); }
                break;
            case PrimitiveSerializationMode.Quantized:
                if (targetTypeSymbol.TryGetAttribute(BitStreamTypeNames.QuantizedPrimitive, out AttributeData? quantizedAttribute)) {
                    ImmutableDictionary<string, TypedConstant> quantizedArguments = quantizedAttribute.GetConstructorArgumentsByName();
                    if (!quantizedArguments.TryGetValue("minimumBits", out int parsedMinBits)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingAttributeArgument, attributeLocation, "minimumBits", "BitStreamQuantizedPrimitive"));
                    }
                    else { minBits = parsedMinBits; }

                    if (!quantizedArguments.TryGetValue("maximumBits", out int parsedMaxBits)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingAttributeArgument, attributeLocation, "maximumBits", "BitStreamQuantizedPrimitive"));
                    }
                    else { maxBits = parsedMaxBits; }

                    if (minBits is not null && maxBits is not null && (minBits <= 0 || maxBits < minBits)) {
                        diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidQuantizedBitRange, attributeLocation, minBits.ToString(), maxBits.ToString()));
                    }
                }
                else { diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingCompanionAttribute, attributeLocation, targetTypeSymbol.Name, "Quantized", "BitStreamQuantizedPrimitive")); }
                break;
            case PrimitiveSerializationMode.VariableLength:
            default:
                break;
        }

        PrimitiveSignatureContext signatureContext = PrimitiveSignatureContext.Create(compilation, targetType);
        Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methodsByRole = CollectPrimitiveMethods(targetTypeSymbol, mode, signatureContext, diagnostics);

        if (methodsByRole.Count == 0) { diagnostics.Add(new DiagnosticValueType(Diagnostics.NoPrimitiveMethods, attributeLocation, targetTypeSymbol.Name)); }

        if (mode == PrimitiveSerializationMode.VariableLength) {
            if (!HasValidMethod(methodsByRole, BitStreamPrimitiveRole.Size)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingSizeRole, attributeLocation, targetTypeSymbol.Name));
            }

            if (!HasValidMethod(methodsByRole, BitStreamPrimitiveRole.TryRead)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingTryReadRole, attributeLocation, targetTypeSymbol.Name));
            }
        }
        else {
            if (HasValidMethod(methodsByRole, BitStreamPrimitiveRole.Size)) {
                PrimitiveMethodDefinition sizeMethod = methodsByRole[BitStreamPrimitiveRole.Size];
                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidSizeRole, attributeLocation, sizeMethod.MethodName));
            }

            if (HasValidMethod(methodsByRole, BitStreamPrimitiveRole.TryRead)) {
                PrimitiveMethodDefinition tryReadMethod = methodsByRole[BitStreamPrimitiveRole.TryRead];
                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidTryReadRole, attributeLocation, tryReadMethod.MethodName));
            }
        }

        SettingsReference? settings = null;
        if (includeSettings) {
            ImmutableArray<ITypeSymbol> settingsInterfaces = arguments.TryGetValue("settings", out TypedConstant settingsArgument) ? TypedConstantUtility.ExtractTypeSymbols(settingsArgument) : ImmutableArray<ITypeSymbol>.Empty;

            Collected<SettingsReference?> collectedSettings = SettingsCollectionSession.CollectSettingsReference(compilation, settingsInterfaces, attributeLocation);
            diagnostics.AddRange(collectedSettings.Diagnostics);
            settings = collectedSettings.Value;
        }

        PrimitiveDefinition definition = new(
            ExtensionClassFullyQualifiedName: targetTypeSymbol.GetFullyQualifiedName(),
            TargetTypeFullyQualifiedName: targetType.GetFullyQualifiedName(),
            TargetTypeNamespace: targetType.GetFullyQualifiedNamespace(),
            TargetTypeEmitName: targetType.GetEmitTypeName(),
            Alias: alias,
            Namespace: targetTypeSymbol.GetFullyQualifiedNamespace(),
            Mode: mode,
            FixedSize: fixedSize,
            MinBits: minBits,
            MaxBits: maxBits,
            Methods: methodsByRole.ToImmutableDictionary(),
            Settings: settings,
            Location: attributeLocation
        );

        return new Collected<PrimitiveDefinition>(definition, diagnostics.ToImmutable());
    }

    private static Collected<PrimitiveDefinition> CreateInvalidPrimitiveDefinition(
        INamedTypeSymbol targetTypeSymbol,
        Location? attributeLocation,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics
    ) {
        return CreateInvalidPrimitiveDefinition(targetTypeSymbol, null, attributeLocation, diagnostics);
    }

    private static Collected<PrimitiveDefinition> CreateInvalidPrimitiveDefinition(
        INamedTypeSymbol targetTypeSymbol,
        ITypeSymbol? targetType,
        Location? attributeLocation,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics
    ) {
        string targetTypeName = targetType?.GetFullyQualifiedName() ?? targetTypeSymbol.GetFullyQualifiedName();
        ITypeSymbol targetTypeForEmit = targetType ?? targetTypeSymbol;
        PrimitiveDefinition definition = new(
            ExtensionClassFullyQualifiedName: targetTypeSymbol.GetFullyQualifiedName(),
            TargetTypeFullyQualifiedName: targetTypeName,
            TargetTypeNamespace: targetTypeForEmit.GetFullyQualifiedNamespace(),
            TargetTypeEmitName: targetTypeForEmit.GetEmitTypeName(),
            Alias: DisplayNameUtility.GetDisplayName(targetType ?? targetTypeSymbol),
            Namespace: targetTypeSymbol.GetFullyQualifiedNamespace(),
            Mode: default,
            FixedSize: null,
            MinBits: null,
            MaxBits: null,
            Methods: ImmutableDictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition>.Empty,
            Settings: null,
            Location: attributeLocation
        );

        return new Collected<PrimitiveDefinition>(definition, diagnostics.ToImmutable());
    }

    private static bool HasValidMethod(Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methodsByRole, BitStreamPrimitiveRole role) {
        return methodsByRole.TryGetValue(role, out PrimitiveMethodDefinition method) && method.IsValid;
    }

    private static Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> CollectPrimitiveMethods(
        INamedTypeSymbol targetTypeSymbol,
        PrimitiveSerializationMode mode,
        PrimitiveSignatureContext signatureContext,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics
    ) {
        Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methodsByRole = new();

        foreach (IMethodSymbol member in targetTypeSymbol.GetMembers().OfType<IMethodSymbol>()) {
            if (!member.TryGetAttribute(BitStreamTypeNames.PrimitiveMethod, out AttributeData? methodAttribute)) { continue; }

            if (!methodAttribute.TryGetValue("role", out BitStreamPrimitiveRole role)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.MissingAttributeArgument, methodAttribute.GetLocation(), "role", "BitStreamPrimitiveMethod"));
                continue;
            }

            if (methodsByRole.ContainsKey(role)) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.DuplicateRole, methodAttribute.GetLocation(), role));
                continue;
            }

            bool isPublicStatic = member.IsStatic && member.DeclaredAccessibility == Accessibility.Public;
            if (!isPublicStatic) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.MethodNotPublicStatic, methodAttribute.GetLocation(), member.Name));
            }

            SignatureValidation validation = ValidatePrimitiveMethodSignature(member, role, mode, signatureContext);
            if (validation.ExpectedSignature is not null && !validation.IsValid) {
                diagnostics.Add(new DiagnosticValueType(Diagnostics.InvalidPrimitiveMethodSignature, methodAttribute.GetLocation(), member.Name, role.ToString(), validation.ExpectedSignature));
            }

            bool isValid = isPublicStatic && validation.IsValid;
            methodsByRole[role] = new PrimitiveMethodDefinition(member.Name, isValid);
        }

        return methodsByRole;
    }

    private static SignatureValidation ValidatePrimitiveMethodSignature(IMethodSymbol method, BitStreamPrimitiveRole role, PrimitiveSerializationMode mode, PrimitiveSignatureContext context) {
        MethodSignatureRule rule = GetRoleSignatureRule(role, context);
        if (mode == PrimitiveSerializationMode.Quantized && role != BitStreamPrimitiveRole.Size && role != BitStreamPrimitiveRole.TryRead) { rule = WithQuantization(rule, context); }

        return MethodSignatureValidator.Validate(method, rule);
    }

    private static MethodSignatureRule GetRoleSignatureRule(BitStreamPrimitiveRole role, PrimitiveSignatureContext context) {
        string typeName = context.TypeName;

        return role switch {
            BitStreamPrimitiveRole.Size => new MethodSignatureRule(
                context.IntType, [MethodSignatureUtility.Value(context.TargetType)],
                $"public static int MethodName({typeName} value)", RequiresRefExtension: false
            ),
            BitStreamPrimitiveRole.Write => new MethodSignatureRule(
                context.VoidType, [MethodSignatureUtility.Ref(context.WriteContext), MethodSignatureUtility.Value(context.TargetType)],
                $"public static void MethodName(this ref WriteContext context, {typeName} value)"
            ),
            BitStreamPrimitiveRole.WriteSpan => new MethodSignatureRule(
                context.VoidType, [MethodSignatureUtility.Ref(context.WriteContext), MethodSignatureUtility.Value(context.ReadOnlySpanOfTarget)],
                $"public static void MethodName(this ref WriteContext context, ReadOnlySpan<{typeName}> values)"
            ),
            BitStreamPrimitiveRole.TryRead => new MethodSignatureRule(
                context.BoolType, [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Out(context.TargetType)],
                $"public static bool MethodName(this ref ReadContext context, out {typeName} value)"
            ),
            BitStreamPrimitiveRole.Peek or BitStreamPrimitiveRole.Read => new MethodSignatureRule(
                context.TargetType, [MethodSignatureUtility.Ref(context.ReadContext)],
                $"public static {typeName} MethodName(this ref ReadContext context)"
            ),
            BitStreamPrimitiveRole.PeekArray or BitStreamPrimitiveRole.ReadArray => new MethodSignatureRule(
                context.ArrayOfTarget, [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.IntType)],
                $"public static {typeName}[] MethodName(this ref ReadContext context, int count)"
            ),
            BitStreamPrimitiveRole.PeekSpan or BitStreamPrimitiveRole.ReadSpan => new MethodSignatureRule(
                context.VoidType,
                [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.IntType), MethodSignatureUtility.Value(context.SpanOfTarget)],
                $"public static void MethodName(this ref ReadContext context, int count, Span<{typeName}> destination)"
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null),
        };
    }

    private static MethodSignatureRule WithQuantization(MethodSignatureRule rule, PrimitiveSignatureContext context) {
        string typeName = context.TypeName;
        return rule.AppendParameters(
            [MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.IntType)],
            $", {typeName} min, {typeName} max, int bitCount)"
        );
    }

    private readonly record struct PrimitiveSignatureContext(
        ITypeSymbol TargetType,
        INamedTypeSymbol? WriteContext,
        INamedTypeSymbol? ReadContext,
        ITypeSymbol? ReadOnlySpanOfTarget,
        ITypeSymbol? SpanOfTarget,
        ITypeSymbol ArrayOfTarget,
        ITypeSymbol IntType,
        ITypeSymbol VoidType,
        ITypeSymbol BoolType
    ) {
        public static PrimitiveSignatureContext Create(Compilation compilation, ITypeSymbol targetType) {
            INamedTypeSymbol? readOnlySpanType = compilation.GetTypeByMetadataName(BitStreamTypeNames.ReadOnlySpan);
            INamedTypeSymbol? spanType = compilation.GetTypeByMetadataName(BitStreamTypeNames.Span);

            return new PrimitiveSignatureContext(
                TargetType: targetType,
                WriteContext: compilation.GetTypeByMetadataName(BitStreamTypeNames.WriteContext),
                ReadContext: compilation.GetTypeByMetadataName(BitStreamTypeNames.ReadContext),
                ReadOnlySpanOfTarget: readOnlySpanType?.Construct(targetType),
                SpanOfTarget: spanType?.Construct(targetType),
                ArrayOfTarget: compilation.CreateArrayTypeSymbol(targetType),
                IntType: compilation.GetSpecialType(SpecialType.System_Int32),
                VoidType: compilation.GetSpecialType(SpecialType.System_Void),
                BoolType: compilation.GetSpecialType(SpecialType.System_Boolean)
            );
        }

        public string TypeName => TargetType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
    }
}
