using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.Roslyn;
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
        return CollectStandalonePrimitive(attributeData, (INamedTypeSymbol)context.TargetSymbol, context.SemanticModel.Compilation);
    }

    public static Collected<PrimitiveDefinition> CollectStandalonePrimitive(AttributeData attributeData, INamedTypeSymbol targetTypeSymbol, Compilation compilation) {
        Collected<PrimitiveDefinition> collected = CollectPrimitiveCore(attributeData, targetTypeSymbol, compilation);
        return AttachPrimitiveSettings(collected, attributeData, compilation);
    }

    public static Collected<PrimitiveDefinition> CollectPrimitiveCore(AttributeData attributeData, INamedTypeSymbol targetTypeSymbol, Compilation compilation) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        Location? attributeLocation = attributeData.GetLocation();

        if (!targetTypeSymbol.IsStatic) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBeStatic, attributeLocation, targetTypeSymbol.Name, "BitStreamPrimitive"));
        }

        if (targetTypeSymbol.DeclaredAccessibility != Accessibility.Public) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBePublic, attributeLocation, targetTypeSymbol.GetFullyQualifiedName(), "BitStreamPrimitive"));
        }

        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();

        if (!arguments.TryGetValue("target", out ITypeSymbol? targetType)) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attributeLocation, "target", "BitStreamPrimitive"));
            return new Collected<PrimitiveDefinition>(default, diagnostics.ToImmutable());
        }

        if (!arguments.TryGetValue("serializationMode", out PrimitiveSerializationMode mode)) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attributeLocation, "serializationMode", "BitStreamPrimitive"));
            return new Collected<PrimitiveDefinition>(default, diagnostics.ToImmutable());
        }

        string alias = arguments.TryGetValue("alias", out string? aliasValue) ? aliasValue : string.Empty;
        if (string.IsNullOrEmpty(alias)) { alias = DisplayNameUtility.GetDisplayName(targetType); }

        int? fixedSize = null;
        int? minBits = null;
        int? maxBits = null;

        switch (mode) {
            case PrimitiveSerializationMode.FixedSize:
                if (!targetTypeSymbol.TryGetAttribute(BitStreamTypeNames.FixedSizePrimitive, out AttributeData? fixedSizeAttribute)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingCompanionAttribute, attributeLocation, targetTypeSymbol.Name, "FixedSize", "BitStreamFixedSizePrimitive"));
                    break;
                }

                if (!fixedSizeAttribute.TryGetValue("size", out int parsedFixedSize)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attributeLocation, "size", "BitStreamFixedSizePrimitive"));
                    break;
                }

                fixedSize = parsedFixedSize;
                if (fixedSize <= 0) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidFixedSize, attributeLocation, fixedSize.ToString()));
                }

                break;
            case PrimitiveSerializationMode.Quantized:
                if (!targetTypeSymbol.TryGetAttribute(BitStreamTypeNames.QuantizedPrimitive, out AttributeData? quantizedAttribute)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingCompanionAttribute, attributeLocation, targetTypeSymbol.Name, "Quantized", "BitStreamQuantizedPrimitive"));
                    break;
                }

                ImmutableDictionary<string, TypedConstant> quantizedArguments = quantizedAttribute.GetConstructorArgumentsByName();
                if (!quantizedArguments.TryGetValue("minimumBits", out int parsedMinBits)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attributeLocation, "minimumBits", "BitStreamQuantizedPrimitive"));
                }
                else { minBits = parsedMinBits; }

                if (!quantizedArguments.TryGetValue("maximumBits", out int parsedMaxBits)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attributeLocation, "maximumBits", "BitStreamQuantizedPrimitive"));
                }
                else { maxBits = parsedMaxBits; }

                if (minBits is not null && maxBits is not null && (minBits <= 0 || maxBits < minBits)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidQuantizedBitRange, attributeLocation, minBits.ToString(), maxBits.ToString()));
                }
                break;
            case PrimitiveSerializationMode.VariableLength:
            default:
                break;
        }

        PrimitiveSignatureContext signatureContext = PrimitiveSignatureContext.Create(compilation, targetType);
        Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methodsByRole = CollectPrimitiveMethods(targetTypeSymbol, mode, signatureContext, diagnostics);

        if (methodsByRole.Count == 0) { diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.NoPrimitiveMethods, attributeLocation, targetTypeSymbol.Name)); }

        if (mode == PrimitiveSerializationMode.VariableLength) {
            if (!HasValidMethod(methodsByRole, BitStreamPrimitiveRole.Size)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingSizeRole, attributeLocation, targetTypeSymbol.Name));
            }

            if (!HasValidMethod(methodsByRole, BitStreamPrimitiveRole.TryRead)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingTryReadRole, attributeLocation, targetTypeSymbol.Name));
            }
        }
        else {
            if (HasValidMethod(methodsByRole, BitStreamPrimitiveRole.Size)) {
                PrimitiveMethodDefinition sizeMethod = methodsByRole[BitStreamPrimitiveRole.Size];
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidSizeRole, attributeLocation, sizeMethod.MethodName));
            }

            if (HasValidMethod(methodsByRole, BitStreamPrimitiveRole.TryRead)) {
                PrimitiveMethodDefinition tryReadMethod = methodsByRole[BitStreamPrimitiveRole.TryRead];
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidTryReadRole, attributeLocation, tryReadMethod.MethodName));
            }
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
            Settings: null,
            Location: attributeLocation
        );

        return new Collected<PrimitiveDefinition>(definition, diagnostics.ToImmutable());
    }

    private static Collected<PrimitiveDefinition> AttachPrimitiveSettings(Collected<PrimitiveDefinition> collected, AttributeData attributeData, Compilation compilation) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        diagnostics.AddRange(collected.Diagnostics);

        Location? attributeLocation = attributeData.GetLocation();
        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();
        ImmutableArray<ITypeSymbol> settingsInterfaces = arguments.TryGetValue("settings", out TypedConstant settingsArgument) ? TypedConstantUtility.ExtractTypeSymbols(settingsArgument) : ImmutableArray<ITypeSymbol>.Empty;

        Collected<SettingsReference?> collectedSettings = SettingsCollectionSession.CollectSettingsReference(compilation, settingsInterfaces, attributeLocation);
        diagnostics.AddRange(collectedSettings.Diagnostics);

        PrimitiveDefinition definition = collected.Value with { Settings = collectedSettings.Value };
        return new Collected<PrimitiveDefinition>(definition, diagnostics.ToImmutable());
    }

    private static bool HasValidMethod(Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methodsByRole, BitStreamPrimitiveRole role) {
        return methodsByRole.TryGetValue(role, out PrimitiveMethodDefinition method) && method.IsValid;
    }

    private static Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> CollectPrimitiveMethods(
        INamedTypeSymbol targetTypeSymbol, PrimitiveSerializationMode mode, PrimitiveSignatureContext signatureContext,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics
    ) {
        Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methodsByRole = new();
        Dictionary<BitStreamPrimitiveRole, Location?> roleLocations = new();

        foreach (IMethodSymbol member in targetTypeSymbol.GetMembers().OfType<IMethodSymbol>()) {
            if (!member.TryGetAttribute(BitStreamTypeNames.PrimitiveMethod, out AttributeData? methodAttribute)) { continue; }

            Location? methodLocation = methodAttribute.GetLocation();

            if (!methodAttribute.TryGetValue("role", out BitStreamPrimitiveRole role)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, methodLocation, "role", "BitStreamPrimitiveMethod"));
                continue;
            }

            if (methodsByRole.ContainsKey(role)) {
                if (roleLocations.TryGetValue(role, out Location? firstLocation)) {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateRole, firstLocation, role));
                    roleLocations.Remove(role);
                }

                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateRole, methodLocation, role));
                continue;
            }

            bool isPublicStatic = member.IsStatic && member.DeclaredAccessibility == Accessibility.Public;
            if (!isPublicStatic) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MethodNotPublicStatic, methodAttribute.GetLocation(), member.Name));
            }

            SignatureValidation validation = GetSignatureRule(role, mode, signatureContext).Validate(member);
            if (validation.ExpectedSignature is not null && !validation.IsValid) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidPrimitiveMethodSignature, methodAttribute.GetLocation(), member.Name, role.ToString(), validation.ExpectedSignature));
            }

            bool isValid = isPublicStatic && validation.IsValid;
            methodsByRole[role] = new PrimitiveMethodDefinition(member.Name, isValid);
            roleLocations[role] = methodLocation;
        }

        return methodsByRole;
    }

    private static MethodSignatureRule GetSignatureRule(BitStreamPrimitiveRole role, PrimitiveSerializationMode mode, PrimitiveSignatureContext context) {
        string typeName = context.TypeName;

        return (role, mode) switch {
            (BitStreamPrimitiveRole.Size, _) => new MethodSignatureRule(
                context.IntType, [MethodSignatureUtility.Value(context.TargetType)],
                $"public static int MethodName({typeName} value)", RequiresRefExtension: false
            ),
            (BitStreamPrimitiveRole.TryRead, _) => new MethodSignatureRule(
                context.BoolType, [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Out(context.TargetType)],
                $"public static bool MethodName(this ref ReadContext context, out {typeName} value)"
            ),
            (BitStreamPrimitiveRole.Write, PrimitiveSerializationMode.Quantized) => new MethodSignatureRule(
                context.VoidType,
                [MethodSignatureUtility.Ref(context.WriteContext), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.IntType)],
                $"public static void MethodName(this ref WriteContext context, {typeName} value, {typeName} min, {typeName} max, int bitCount)"
            ),
            (BitStreamPrimitiveRole.Write, _) => new MethodSignatureRule(
                context.VoidType, [MethodSignatureUtility.Ref(context.WriteContext), MethodSignatureUtility.Value(context.TargetType)],
                $"public static void MethodName(this ref WriteContext context, {typeName} value)"
            ),
            (BitStreamPrimitiveRole.WriteSpan, PrimitiveSerializationMode.Quantized) => new MethodSignatureRule(
                context.VoidType,
                [MethodSignatureUtility.Ref(context.WriteContext), MethodSignatureUtility.Value(context.ReadOnlySpanOfTarget), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.IntType)],
                $"public static void MethodName(this ref WriteContext context, ReadOnlySpan<{typeName}> values, {typeName} min, {typeName} max, int bitCount)"
            ),
            (BitStreamPrimitiveRole.WriteSpan, _) => new MethodSignatureRule(
                context.VoidType, [MethodSignatureUtility.Ref(context.WriteContext), MethodSignatureUtility.Value(context.ReadOnlySpanOfTarget)],
                $"public static void MethodName(this ref WriteContext context, ReadOnlySpan<{typeName}> values)"
            ),
            (BitStreamPrimitiveRole.Peek or BitStreamPrimitiveRole.Read, PrimitiveSerializationMode.Quantized) => new MethodSignatureRule(
                context.TargetType,
                [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.IntType)],
                $"public static {typeName} MethodName(this ref ReadContext context, {typeName} min, {typeName} max, int bitCount)"
            ),
            (BitStreamPrimitiveRole.Peek or BitStreamPrimitiveRole.Read, _) => new MethodSignatureRule(
                context.TargetType, [MethodSignatureUtility.Ref(context.ReadContext)],
                $"public static {typeName} MethodName(this ref ReadContext context)"
            ),
            (BitStreamPrimitiveRole.PeekArray or BitStreamPrimitiveRole.ReadArray, PrimitiveSerializationMode.Quantized) => new MethodSignatureRule(
                context.ArrayOfTarget,
                [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.IntType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.IntType)],
                $"public static {typeName}[] MethodName(this ref ReadContext context, int count, {typeName} min, {typeName} max, int bitCount)"
            ),
            (BitStreamPrimitiveRole.PeekArray or BitStreamPrimitiveRole.ReadArray, _) => new MethodSignatureRule(
                context.ArrayOfTarget, [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.IntType)],
                $"public static {typeName}[] MethodName(this ref ReadContext context, int count)"
            ),
            (BitStreamPrimitiveRole.PeekSpan or BitStreamPrimitiveRole.ReadSpan, PrimitiveSerializationMode.Quantized) => new MethodSignatureRule(
                context.VoidType,
                [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.IntType), MethodSignatureUtility.Value(context.SpanOfTarget), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.TargetType), MethodSignatureUtility.Value(context.IntType)],
                $"public static void MethodName(this ref ReadContext context, int count, Span<{typeName}> destination, {typeName} min, {typeName} max, int bitCount)"
            ),
            (BitStreamPrimitiveRole.PeekSpan or BitStreamPrimitiveRole.ReadSpan, _) => new MethodSignatureRule(
                context.VoidType,
                [MethodSignatureUtility.Ref(context.ReadContext), MethodSignatureUtility.Value(context.IntType), MethodSignatureUtility.Value(context.SpanOfTarget)],
                $"public static void MethodName(this ref ReadContext context, int count, Span<{typeName}> destination)"
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null),
        };
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
