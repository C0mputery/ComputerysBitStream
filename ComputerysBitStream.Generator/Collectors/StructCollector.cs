using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator.Collectors;

internal static class StructCollector {
    public static IncrementalValuesProvider<Collected<StructDefinition>> GetAllStructData(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<Collected<StructDefinition>> structData = GetStructData(context);
        IncrementalValuesProvider<Collected<StructDefinition>> proxyStructData = GetProxyStructData(context);

        IncrementalValueProvider<ImmutableArray<Collected<StructDefinition>>> collectedStructData = structData.Collect();
        IncrementalValueProvider<ImmutableArray<Collected<StructDefinition>>> collectedProxyStructData = proxyStructData.Collect();
        IncrementalValueProvider<(ImmutableArray<Collected<StructDefinition>> Left, ImmutableArray<Collected<StructDefinition>> Right)> combined = collectedStructData.Combine(collectedProxyStructData);
        return combined.SelectMany((pair, _) => pair.Left.AddRange(pair.Right));
    }

    public static IncrementalValuesProvider<Collected<StructDefinition>> GetStructData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: BitStreamTypeNames.Struct,
            predicate: (SyntaxNode node, CancellationToken _) => node is StructDeclarationSyntax || (node is RecordDeclarationSyntax record && record.IsKind(SyntaxKind.RecordStructDeclaration)),
            transform: StructAttributeDataTransform
        );
    }

    private static Collected<StructDefinition> StructAttributeDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        AttributeData attributeData = context.Attributes[0];
        TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax)context.TargetNode;
        INamedTypeSymbol structSymbol = (INamedTypeSymbol)context.TargetSymbol;
        Collected<StructDefinition> collected = CollectStandaloneStruct(attributeData, structSymbol, context.SemanticModel.Compilation);

        if (typeDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)) {
            return collected;
        }

        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        diagnostics.AddRange(collected.Diagnostics);
        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBePartial, typeDeclaration.Identifier.GetLocation(), structSymbol.GetFullyQualifiedName(), "BitStreamStruct"));
        return new Collected<StructDefinition>(collected.Value, diagnostics.ToImmutable());
    }

    public static Collected<StructDefinition> CollectStandaloneStruct(AttributeData attributeData, INamedTypeSymbol structSymbol, Compilation compilation) {
        Collected<StructDefinition> collected = CollectStructCore(attributeData, structSymbol, compilation);
        return AttachStructSettings(collected, attributeData, compilation);
    }

    public static Collected<StructDefinition> CollectIncludedStruct(AttributeData attributeData, INamedTypeSymbol structSymbol, Compilation compilation) {
        return CollectStructCore(attributeData, structSymbol, compilation);
    }

    private static Collected<StructDefinition> CollectStructCore(AttributeData attributeData, INamedTypeSymbol structSymbol, Compilation compilation) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        Location? attributeLocation = attributeData.GetLocation();

        if (structSymbol.DeclaredAccessibility != Accessibility.Public) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBePublic, attributeLocation, structSymbol.GetFullyQualifiedName(), "BitStreamStruct"));
        }

        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();
        string alias = arguments.TryGetValue("alias", out string? aliasValue) ? aliasValue : string.Empty;

        HashSet<string> excludedMembers = [];
        HashSet<string> includedMembers = [];
        foreach (ISymbol member in structSymbol.GetMembers()) {
            CollectMemberInclusionAttributes(member, excludedMembers, includedMembers, diagnostics);
        }

        ImmutableArray<StructMemberDefinition> members = CollectMembers(structSymbol, excludedMembers, includedMembers, diagnostics);
        string fullyQualifiedName = structSymbol.GetFullyQualifiedName();

        StructDefinition definition = new(
            TypeFullyQualifiedName: fullyQualifiedName,
            Alias: string.IsNullOrEmpty(alias) ? DisplayNameUtility.GetDisplayName(structSymbol) : alias,
            Namespace: structSymbol.GetFullyQualifiedNamespace(),
            Members: members,
            IsProxyClass: false,
            DeclarationTypeFullyQualifiedName: fullyQualifiedName,
            DeclarationTypeEmitName: structSymbol.GetEmitTypeName(),
            Settings: null,
            Location: attributeLocation
        );

        return new Collected<StructDefinition>(definition, diagnostics.ToImmutable());
    }

    public static IncrementalValuesProvider<Collected<StructDefinition>> GetProxyStructData(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: BitStreamTypeNames.ProxyStruct,
            predicate: (SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax,
            transform: ProxyStructAttributeDataTransform
        );
    }

    private static Collected<StructDefinition> ProxyStructAttributeDataTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        AttributeData attributeData = context.Attributes[0];
        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        INamedTypeSymbol proxyClassSymbol = (INamedTypeSymbol)context.TargetSymbol;
        Collected<StructDefinition> collected = CollectStandaloneProxyStruct(attributeData, proxyClassSymbol, context.SemanticModel.Compilation);

        if (classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)) {
            return collected;
        }

        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        diagnostics.AddRange(collected.Diagnostics);
        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBePartial, classDeclaration.Identifier.GetLocation(), proxyClassSymbol.GetFullyQualifiedName(), "BitStreamProxyStruct"));
        return new Collected<StructDefinition>(collected.Value, diagnostics.ToImmutable());
    }

    public static Collected<StructDefinition> CollectStandaloneProxyStruct(AttributeData attributeData, INamedTypeSymbol proxyClassSymbol, Compilation compilation) {
        Collected<StructDefinition> collected = CollectProxyStructCore(attributeData, proxyClassSymbol, compilation);
        return AttachStructSettings(collected, attributeData, compilation);
    }

    public static Collected<StructDefinition> CollectIncludedProxyStruct(AttributeData attributeData, INamedTypeSymbol proxyClassSymbol, Compilation compilation) {
        return CollectProxyStructCore(attributeData, proxyClassSymbol, compilation);
    }

    private static Collected<StructDefinition> CollectProxyStructCore(AttributeData attributeData, INamedTypeSymbol proxyClassSymbol, Compilation compilation) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        Location? attributeLocation = attributeData.GetLocation();
        string proxyClassFullyQualifiedName = proxyClassSymbol.GetFullyQualifiedName();

        if (!proxyClassSymbol.IsStatic) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBeStatic, attributeLocation, proxyClassFullyQualifiedName, "BitStreamProxyStruct"));
        }

        if (proxyClassSymbol.DeclaredAccessibility != Accessibility.Public) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.TypeMustBePublic, attributeLocation, proxyClassFullyQualifiedName, "BitStreamProxyStruct"));
        }

        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();

        if (!arguments.TryGetValue("targetStruct", out ITypeSymbol? targetType)) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.ProxyStructNotStruct, attributeLocation, "unknown"));
            return CreateProxyStructData(proxyClassFullyQualifiedName, proxyClassSymbol.GetEmitTypeName(), proxyClassSymbol.GetFullyQualifiedNamespace(), attributeLocation, ImmutableArray<StructMemberDefinition>.Empty, diagnostics);
        }

        if (targetType is not INamedTypeSymbol structSymbol || structSymbol.TypeKind != TypeKind.Struct) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.ProxyStructNotStruct, attributeLocation, targetType.GetFullyQualifiedName()));
            return CreateProxyStructData(proxyClassFullyQualifiedName, proxyClassSymbol.GetEmitTypeName(), proxyClassSymbol.GetFullyQualifiedNamespace(), attributeLocation, ImmutableArray<StructMemberDefinition>.Empty, diagnostics);
        }

        string alias = arguments.TryGetValue("alias", out string? aliasValue) ? aliasValue : string.Empty;
        alias = string.IsNullOrEmpty(alias) ? DisplayNameUtility.GetDisplayName(structSymbol) : alias;

        HashSet<string> excludedMembers = [];
        HashSet<string> includedMembers = [];
        HashSet<string> proxyDefinedMembers = GetProxyDefinedMemberNames(proxyClassSymbol);

        foreach (ISymbol member in proxyClassSymbol.GetMembers()) {
            CollectMemberInclusionAttributes(member, excludedMembers, includedMembers, diagnostics, reportInaccessibleInclude: false);
        }

        foreach (ISymbol member in structSymbol.GetMembers()) {
            if (proxyDefinedMembers.Contains(member.Name)) { continue; }
            CollectMemberInclusionAttributes(member, excludedMembers, includedMembers, diagnostics);
        }

        ValidateProxyMembers(proxyClassSymbol, structSymbol, diagnostics);
        ValidateProxyMemberAccessibility(proxyClassSymbol, diagnostics);

        ImmutableArray<StructMemberDefinition> members = CollectMembers(structSymbol, excludedMembers, includedMembers, diagnostics, proxyClassSymbol, proxyDefinedMembers);

        return CreateProxyStructData(
            proxyClassFullyQualifiedName,
            proxyClassSymbol.GetEmitTypeName(),
            proxyClassSymbol.GetFullyQualifiedNamespace(),
            attributeLocation,
            members,
            diagnostics,
            structSymbol.GetFullyQualifiedName(),
            alias,
            settings: null
        );
    }

    private static Collected<StructDefinition> AttachStructSettings(Collected<StructDefinition> collected, AttributeData attributeData, Compilation compilation) {
        ImmutableArray<DiagnosticValueType>.Builder diagnostics = ImmutableArray.CreateBuilder<DiagnosticValueType>();
        diagnostics.AddRange(collected.Diagnostics);

        Location? attributeLocation = attributeData.GetLocation();
        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();
        SettingsReference? settings = CollectSettingsReference(arguments, attributeLocation, compilation, diagnostics);

        StructDefinition definition = collected.Value with { Settings = settings };
        return new Collected<StructDefinition>(definition, diagnostics.ToImmutable());
    }

    private static Collected<StructDefinition> CreateProxyStructData(
        string declarationTypeFullyQualifiedName, string declarationTypeEmitName, string? declaringNamespace,
        Location? attributeLocation, ImmutableArray<StructMemberDefinition> members, ImmutableArray<DiagnosticValueType>.Builder diagnostics,
        string typeFullyQualifiedName = "", string alias = "", SettingsReference? settings = null
    ) {
        StructDefinition definition = new(
            TypeFullyQualifiedName: typeFullyQualifiedName,
            Alias: alias,
            Namespace: declaringNamespace,
            Members: members,
            IsProxyClass: true,
            DeclarationTypeFullyQualifiedName: declarationTypeFullyQualifiedName,
            DeclarationTypeEmitName: declarationTypeEmitName,
            Settings: settings,
            Location: attributeLocation
        );
        return new Collected<StructDefinition>(definition, diagnostics.ToImmutable());
    }

    private static void CollectMemberInclusionAttributes(
        ISymbol member, HashSet<string> excludedMembers, HashSet<string> includedMembers,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics, bool reportInaccessibleInclude = true
    ) {
        bool hasIgnore = member.HasAttribute(BitStreamTypeNames.StructIgnore);
        bool hasInclude = member.HasAttribute(BitStreamTypeNames.StructInclude);

        if (hasIgnore && hasInclude) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.ConflictingStructMemberAttributes, member.Locations.FirstOrDefault(), member.Name));
            excludedMembers.Add(member.Name);
            return;
        }

        switch (member) {
            case IPropertySymbol when hasIgnore:
            case IFieldSymbol when hasIgnore:
                excludedMembers.Add(member.Name);
                break;
            case IPropertySymbol when hasInclude:
            case IFieldSymbol when hasInclude:
                if (member.DeclaredAccessibility != Accessibility.Public) {
                    if (reportInaccessibleInclude) {
                        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InaccessibleStructMember, member.Locations.FirstOrDefault(), member.Name));
                    }

                    break;
                }

                includedMembers.Add(member.Name);
                break;
        }
    }

    private static bool ShouldIncludeMember(
        string memberName, bool isProperty, Accessibility declaredAccessibility, HashSet<string> excludedMembers,
        HashSet<string> includedMembers, bool useProxyInclusionRules, HashSet<string> proxyDefinedMembers
    ) {
        if (excludedMembers.Contains(memberName)) { return false; }
        if (includedMembers.Contains(memberName)) { return true; }

        if (useProxyInclusionRules) {
            return proxyDefinedMembers.Contains(memberName);
        }

        return isProperty && declaredAccessibility == Accessibility.Public;
    }

    private static SettingsReference? CollectSettingsReference(
        ImmutableDictionary<string, TypedConstant> arguments, Location? attributeLocation, Compilation compilation,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics
    ) {
        ImmutableArray<ITypeSymbol> settingsInterfaces = arguments.TryGetValue("settings", out TypedConstant settingsArgument) ? TypedConstantUtility.ExtractTypeSymbols(settingsArgument) : ImmutableArray<ITypeSymbol>.Empty;

        Collected<SettingsReference?> collected = SettingsCollectionSession.CollectSettingsReference(compilation, settingsInterfaces, attributeLocation);
        diagnostics.AddRange(collected.Diagnostics);
        return collected.Value;
    }

    private static ImmutableArray<StructMemberDefinition> CollectMembers(
        INamedTypeSymbol structSymbol, HashSet<string> excludedMembers, HashSet<string> includedMembers,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics, INamedTypeSymbol? proxyClassSymbol = null, HashSet<string>? proxyDefinedMembers = null
    ) {
        bool useProxyInclusionRules = proxyClassSymbol is not null;
        proxyDefinedMembers ??= proxyClassSymbol is not null ? GetProxyDefinedMemberNames(proxyClassSymbol) : [];
        ImmutableArray<StructMemberDefinition>.Builder members = ImmutableArray.CreateBuilder<StructMemberDefinition>();
        string structDisplayName = structSymbol.GetFullyQualifiedName();

        IEnumerable<ISymbol> orderedMembers = structSymbol.GetMembers().OrderBy(static member => member.DeclaringSyntaxReferences.FirstOrDefault()?.Span.Start ?? 0);

        foreach (ISymbol member in orderedMembers) {
            if (proxyClassSymbol is not null
                && TryGetProxyMember(proxyClassSymbol, member.Name, out ISymbol? proxyMember)
                && !ProxyMemberMatchesTarget(proxyMember, member)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.ProxyMemberTypeMismatch, proxyMember.Locations.FirstOrDefault(), member.Name, structDisplayName));
                continue;
            }

            switch (member) {
                case IPropertySymbol property:
                    TryAddProperty(property, excludedMembers, includedMembers, members, diagnostics, useProxyInclusionRules, proxyDefinedMembers, proxyClassSymbol);
                    break;
                case IFieldSymbol field:
                    TryAddField(field, excludedMembers, includedMembers, members, diagnostics, useProxyInclusionRules, proxyDefinedMembers, proxyClassSymbol);
                    break;
            }
        }

        return members.ToImmutable();
    }

    private static void TryAddProperty(
        IPropertySymbol property, HashSet<string> excludedMembers, HashSet<string> includedMembers,
        ImmutableArray<StructMemberDefinition>.Builder members, ImmutableArray<DiagnosticValueType>.Builder diagnostics,
        bool useProxyInclusionRules, HashSet<string> proxyDefinedMembers, INamedTypeSymbol? proxyClassSymbol = null
    ) {
        if (property.IsStatic || property.IsImplicitlyDeclared || property.IsIndexer) { return; }
        if (property.DeclaredAccessibility != Accessibility.Public) { return; }

        bool explicitlyIncluded = includedMembers.Contains(property.Name);
        if (!ShouldIncludeMember(property.Name, true, property.DeclaredAccessibility, excludedMembers, includedMembers, useProxyInclusionRules, proxyDefinedMembers)) { return; }

        if (property.GetMethod is null) { return; }

        bool hasWritableProxyMirror = proxyClassSymbol is not null && TryGetProxyMember(proxyClassSymbol, property.Name, out ISymbol? proxyMember) && IsWritableProxyMirror(proxyMember);

        if (property.GetMethod.DeclaredAccessibility != Accessibility.Public) {
            if (explicitlyIncluded) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MemberSkipped, property.Locations.FirstOrDefault(), property.Name, "getter is not public"));
            }

            return;
        }

        if (property.SetMethod is null && !hasWritableProxyMirror) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MemberSkipped, property.Locations.FirstOrDefault(), property.Name, "property is read-only"));
            return;
        }

        if (property.SetMethod is not null
            && property.SetMethod.DeclaredAccessibility != Accessibility.Public
            && !hasWritableProxyMirror) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MemberSkipped, property.Locations.FirstOrDefault(), property.Name, "setter is not public"));
            return;
        }

        ISymbol attributeSource = ResolveAttributeSource(property, proxyClassSymbol);
        bool isInitOnly = property.SetMethod?.IsInitOnly ?? false;
        if (hasWritableProxyMirror && attributeSource is IPropertySymbol { SetMethod: not null } proxyProperty) {
            isInitOnly = proxyProperty.SetMethod.IsInitOnly;
        }

        members.Add(CreateMemberData(property.Name, property.Type, true, isInitOnly, attributeSource, diagnostics));
    }

    private static void TryAddField(
        IFieldSymbol field,
        HashSet<string> excludedMembers,
        HashSet<string> includedMembers,
        ImmutableArray<StructMemberDefinition>.Builder members,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics,
        bool useProxyInclusionRules,
        HashSet<string> proxyDefinedMembers,
        INamedTypeSymbol? proxyClassSymbol = null
    ) {
        if (field.IsStatic || field.IsImplicitlyDeclared) { return; }
        if (field.DeclaredAccessibility != Accessibility.Public) { return; }

        if (!ShouldIncludeMember(field.Name, false, field.DeclaredAccessibility, excludedMembers, includedMembers, useProxyInclusionRules, proxyDefinedMembers)) {
            return;
        }

        if (field.IsReadOnly || field.IsConst) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MemberSkipped, field.Locations.FirstOrDefault(), field.Name, "field is read-only"));
            return;
        }

        if (field.RefKind != RefKind.None) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MemberSkipped, field.Locations.FirstOrDefault(), field.Name, "field is a ref field"));
            return;
        }

        ISymbol attributeSource = ResolveAttributeSource(field, proxyClassSymbol);
        members.Add(CreateMemberData(field.Name, field.Type, false, false, attributeSource, diagnostics));
    }

    private static ISymbol ResolveAttributeSource(ISymbol structMember, INamedTypeSymbol? proxyClassSymbol) {
        if (proxyClassSymbol is not null && TryGetProxyMember(proxyClassSymbol, structMember.Name, out ISymbol? proxyMember)) {
            return proxyMember;
        }

        return structMember;
    }

    private static bool TryGetProxyMember(INamedTypeSymbol proxyClassSymbol, string memberName, [NotNullWhen(true)] out ISymbol? proxyMember) {
        foreach (ISymbol member in proxyClassSymbol.GetMembers(memberName)) {
            if (member.DeclaredAccessibility != Accessibility.Public) { continue; }

            switch (member) {
                case IPropertySymbol { IsIndexer: false, IsStatic: true, IsImplicitlyDeclared: false }:
                case IFieldSymbol { IsStatic: true, IsImplicitlyDeclared: false }:
                    proxyMember = member;
                    return true;
            }
        }

        proxyMember = null;
        return false;
    }

    private static HashSet<string> GetProxyDefinedMemberNames(INamedTypeSymbol proxyClassSymbol) {
        HashSet<string> names = [];
        foreach (ISymbol member in proxyClassSymbol.GetMembers()) {
            if (member.DeclaredAccessibility != Accessibility.Public) { continue; }

            switch (member) {
                case IPropertySymbol { IsIndexer: false, IsStatic: true, IsImplicitlyDeclared: false } property:
                    names.Add(property.Name);
                    break;
                case IFieldSymbol { IsStatic: true, IsImplicitlyDeclared: false } field:
                    names.Add(field.Name);
                    break;
            }
        }

        return names;
    }

    private static void ValidateProxyMembers(INamedTypeSymbol proxyClassSymbol, INamedTypeSymbol structSymbol, ImmutableArray<DiagnosticValueType>.Builder diagnostics) {
        string structDisplayName = structSymbol.GetFullyQualifiedName();

        foreach (ISymbol proxyMember in proxyClassSymbol.GetMembers()) {
            switch (proxyMember) {
                case IPropertySymbol { IsIndexer: false, IsStatic: true, IsImplicitlyDeclared: false }:
                case IFieldSymbol { IsStatic: true, IsImplicitlyDeclared: false }:
                    break;
                default:
                    continue;
            }

            if (structSymbol.GetMembers(proxyMember.Name).FirstOrDefault(static member => member is IPropertySymbol or IFieldSymbol) is null) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.ProxyMemberNotOnTarget, proxyMember.Locations.FirstOrDefault(), proxyMember.Name, structDisplayName));
            }
        }
    }

    private static void ValidateProxyMemberAccessibility(INamedTypeSymbol proxyClassSymbol, ImmutableArray<DiagnosticValueType>.Builder diagnostics) {
        foreach (ISymbol proxyMember in proxyClassSymbol.GetMembers()) {
            switch (proxyMember) {
                case IPropertySymbol { IsIndexer: false, IsStatic: true, IsImplicitlyDeclared: false }:
                case IFieldSymbol { IsStatic: true, IsImplicitlyDeclared: false }:
                    break;
                default:
                    continue;
            }

            if (proxyMember.DeclaredAccessibility != Accessibility.Public) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InaccessibleStructMember, proxyMember.Locations.FirstOrDefault(), proxyMember.Name));
            }
        }
    }

    private static bool ProxyMemberMatchesTarget(ISymbol proxyMember, ISymbol targetMember) {
        ITypeSymbol? proxyType = GetMemberType(proxyMember);
        ITypeSymbol? targetType = GetMemberType(targetMember);
        return proxyType is not null && targetType is not null && SymbolEqualityComparer.Default.Equals(proxyType, targetType);
    }

    private static ITypeSymbol? GetMemberType(ISymbol member) {
        return member switch {
            IPropertySymbol property => property.Type,
            IFieldSymbol field => field.Type,
            _ => null,
        };
    }

    private static bool IsWritableProxyMirror(ISymbol proxyMember) {
        return proxyMember switch {
            IPropertySymbol { SetMethod: { DeclaredAccessibility: Accessibility.Public } } => true,
            IFieldSymbol { IsReadOnly: false, IsConst: false } => true,
            _ => false,
        };
    }

    private static StructMemberDefinition CreateMemberData(string memberName, ITypeSymbol memberType, bool isProperty, bool isInitOnly, ISymbol memberSymbol, ImmutableArray<DiagnosticValueType>.Builder diagnostics) {
        string? serializerExtensionClass = null;
        foreach (AttributeData attribute in memberSymbol.GetAttributes()) {
            if (!attribute.IsAttribute(BitStreamTypeNames.Serializer)) { continue; }

            if (attribute.TryGetConstructorArgumentByName("type", out TypedConstant typeArgument)) {
                if (typeArgument.TryGetValue(out INamedTypeSymbol? serializerType)) {
                    if (serializerExtensionClass is not null) {
                        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.DuplicateMemberSerializer, attribute.GetLocation(), memberName));
                        continue;
                    }

                    serializerExtensionClass = serializerType.GetFullyQualifiedName();
                }
                else {
                    diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.MissingAttributeArgument, attribute.GetLocation(), "type", "BitStreamSerializer"));
                }
            }
        }

        QuantizedDefinition? quantized = null;
        if (memberSymbol.TryGetAttribute(BitStreamTypeNames.StructQuantized, out AttributeData? quantizedAttribute)) {
            if (TryParseQuantized(quantizedAttribute, memberSymbol, diagnostics, out QuantizedDefinition parsedQuantized)) {
                quantized = parsedQuantized;
            }
        }

        bool isVariableLength = memberSymbol.TryGetAttribute(BitStreamTypeNames.StructVariableLength, out _);
        if (isVariableLength && quantized is not null) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.ConflictingStructMemberSerializationAttributes, memberSymbol.Locations.FirstOrDefault(), memberName));
            isVariableLength = false;
        }

        StructCollectionDefinition? collection = null;
        bool hasCollectionAttribute = memberSymbol.TryGetAttribute(BitStreamTypeNames.StructCollectionMaxEntries, out AttributeData? collectionAttribute);
        if (memberType is IArrayTypeSymbol arrayType) {
            if (!hasCollectionAttribute || collectionAttribute is null) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.CollectionMaxEntriesRequired, memberSymbol.Locations.FirstOrDefault(), memberName));
            }
            else if (TryParseCollection(arrayType, collectionAttribute, memberName, diagnostics, out StructCollectionDefinition parsedCollection)) {
                collection = parsedCollection;
            }
        }
        else if (hasCollectionAttribute) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.CollectionAttributeOnNonArray, collectionAttribute?.GetLocation() ?? memberSymbol.Locations.FirstOrDefault(), memberName));
        }

        return new StructMemberDefinition(
            MemberName: memberName,
            TypeFullyQualifiedFormat: memberType.GetFullyQualifiedName(),
            TypeEmitFormat: memberType.GetEmitTypeName(),
            IsProperty: isProperty,
            IsInitOnly: isInitOnly,
            SerializerExtensionClassFullyQualifiedName: serializerExtensionClass,
            IsVariableLength: isVariableLength,
            Quantized: quantized,
            Collection: collection,
            Location: memberSymbol.Locations.FirstOrDefault()
        );
    }

    private static bool TryParseCollection(
        IArrayTypeSymbol arrayType,
        AttributeData attribute,
        string memberName,
        ImmutableArray<DiagnosticValueType>.Builder diagnostics,
        out StructCollectionDefinition collection
    ) {
        collection = default;
        ImmutableArray<int>.Builder ranks = ImmutableArray.CreateBuilder<int>();
        ImmutableArray<string>.Builder arrayTypes = ImmutableArray.CreateBuilder<string>();
        ImmutableArray<string>.Builder arrayEmitTypes = ImmutableArray.CreateBuilder<string>();
        ITypeSymbol leafType = arrayType;
        while (leafType is IArrayTypeSymbol currentArray) {
            arrayTypes.Add(currentArray.GetFullyQualifiedName());
            arrayEmitTypes.Add(currentArray.GetEmitTypeName());
            ranks.Add(currentArray.Rank);
            leafType = currentArray.ElementType;
        }

        ImmutableDictionary<string, TypedConstant> arguments = attribute.GetConstructorArgumentsByName();
        ImmutableArray<int>.Builder limits = ImmutableArray.CreateBuilder<int>();
        if (arguments.TryGetValue("maxRead", out TypedConstant maxReadArgument) && maxReadArgument.TryGetValue(out int maxRead)) {
            limits.Add(maxRead);
        }

        if (arguments.TryGetValue("nestedMaxReads", out TypedConstant nestedArgument)) {
            foreach (TypedConstant value in nestedArgument.Values) {
                if (value.TryGetValue(out int nestedLimit)) { limits.Add(nestedLimit); }
            }
        }

        int expectedLimitCount = 0;
        foreach (int rank in ranks) { expectedLimitCount += rank; }

        if (limits.Count != expectedLimitCount) {
            diagnostics.Add(new DiagnosticValueType(
                DiagnosticDescriptors.InvalidCollectionMaxEntries,
                attribute.GetLocation(),
                memberName,
                expectedLimitCount,
                limits.Count
            ));
            return false;
        }

        long maximumLeafCount = 1;
        foreach (int limit in limits) {
            if (limit < 0) {
                diagnostics.Add(new DiagnosticValueType(
                    DiagnosticDescriptors.CollectionMaxEntriesNegative,
                    attribute.GetLocation(),
                    memberName
                ));
                return false;
            }

            if (limit != 0 && maximumLeafCount > int.MaxValue / limit) {
                diagnostics.Add(new DiagnosticValueType(
                    DiagnosticDescriptors.CollectionMaxEntriesProductOverflow,
                    attribute.GetLocation(),
                    memberName,
                    int.MaxValue
                ));
                return false;
            }

            maximumLeafCount *= limit;
        }

        collection = new StructCollectionDefinition(
            LeafTypeFullyQualifiedFormat: leafType.GetFullyQualifiedName(),
            LeafTypeEmitFormat: leafType.GetEmitTypeName(),
            ArrayTypeFullyQualifiedFormats: arrayTypes.ToImmutable(),
            ArrayTypeEmitFormats: arrayEmitTypes.ToImmutable(),
            Ranks: ranks.ToImmutable(),
            MaxEntries: limits.ToImmutable()
        );
        return true;
    }

    private static bool TryParseQuantized(AttributeData attributeData, ISymbol memberSymbol, ImmutableArray<DiagnosticValueType>.Builder diagnostics, out QuantizedDefinition quantizedDefinition) {
        quantizedDefinition = default;
        Location? location = attributeData.GetLocation();
        ImmutableDictionary<string, TypedConstant> arguments = attributeData.GetConstructorArgumentsByName();

        if (!arguments.TryGetValue("minMember", out string? minMemberName) || !arguments.TryGetValue("maxMember", out string? maxMemberName) || !arguments.TryGetValue("bitCount", out int bitCount)) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidQuantizedMember, location, "unknown", memberSymbol.Name));
            return false;
        }

        ITypeSymbol? minSource;
        if (arguments.TryGetValue("minSource", out TypedConstant minSourceArgument)) {
            if (!minSourceArgument.TryGetValue(out minSource)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidAttributeArgument, location, "minSource", "BitStreamStructQuantized", minSourceArgument.ToCSharpString()));
                return false;
            }
        }
        else if (arguments.TryGetValue("source", out TypedConstant sourceArgument)) {
            if (!sourceArgument.TryGetValue(out minSource)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidAttributeArgument, location, "source", "BitStreamStructQuantized", sourceArgument.ToCSharpString()));
                return false;
            }
        }
        else { minSource = memberSymbol.ContainingType; }

        ITypeSymbol? maxSource;
        if (arguments.TryGetValue("maxSource", out TypedConstant maxSourceArgument)) {
            if (!maxSourceArgument.TryGetValue(out maxSource)) {
                diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidAttributeArgument, location, "maxSource", "BitStreamStructQuantized", maxSourceArgument.ToCSharpString()));
                return false;
            }
        }
        else { maxSource = minSource; }

        if (bitCount <= 0) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidQuantizedBitCount, location, bitCount.ToString(), memberSymbol.Name));
            return false;
        }

        if (!TryResolveRangeExpression(minSource, minMemberName, memberSymbol.Name, location, diagnostics, out string minExpression)) { return false; }
        if (!TryResolveRangeExpression(maxSource, maxMemberName, memberSymbol.Name, location, diagnostics, out string maxExpression)) { return false; }

        quantizedDefinition = new QuantizedDefinition(minExpression, maxExpression, bitCount, location);
        return true;
    }

    private static bool TryResolveRangeExpression(ITypeSymbol? sourceType, string memberName, string annotatedMemberName, Location? location, ImmutableArray<DiagnosticValueType>.Builder diagnostics, out string expression) {
        expression = string.Empty;
        if (sourceType is not INamedTypeSymbol namedType) {
            diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidQuantizedMember, location, memberName, annotatedMemberName));
            return false;
        }

        ISymbol? member = namedType.GetMembers(memberName).FirstOrDefault();
        if (member is IFieldSymbol { IsConst: true } constField) {
            expression = constField.ConstantValue?.ToString() ?? $"{namedType.GetFullyQualifiedName()}.{memberName}";
            return true;
        }

        if (member is IFieldSymbol { IsStatic: true, IsReadOnly: true }) {
            expression = $"{namedType.GetFullyQualifiedName()}.{memberName}";
            return true;
        }

        if (member is IPropertySymbol { IsStatic: true, GetMethod: not null, SetMethod: null }) {
            expression = $"{namedType.GetFullyQualifiedName()}.{memberName}";
            return true;
        }

        diagnostics.Add(new DiagnosticValueType(DiagnosticDescriptors.InvalidQuantizedMember, location, memberName, annotatedMemberName));
        return false;
    }
}
