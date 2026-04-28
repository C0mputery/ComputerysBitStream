using System;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator;

internal static class StructCollector {
    public static readonly string StructAttribute = typeof(BitStreamStructAttribute).FullName!;
    private static readonly string IncludeAttribute = typeof(BitStreamStructIncludeAttribute).FullName!;
    private static readonly string ExcludeAttribute = typeof(BitStreamStructIgnoreAttribute).FullName!;
    public static readonly string ProxyStructAttribute = typeof(BitStreamProxyStructAttribute).FullName!;
    public static readonly string ExternalStructAttribute = typeof(BitStreamStructMetadataAttribute).FullName!;
    private static readonly string SettingsAttribute = typeof(BitStreamSettingsAttribute).FullName!;

    public static IncrementalValuesProvider<StructData> GetAllStructData(IncrementalGeneratorInitializationContext context) {
        IncrementalValuesProvider<StructData> structData = GetStructAttributes(context);
        IncrementalValuesProvider<StructData> externalStructData = GetProxyStructAttributes(context);
        
        // Cursed
        IncrementalValueProvider<ImmutableArray<StructData>> collectedStructData = structData.Collect();
        IncrementalValueProvider<ImmutableArray<StructData>> collectedProxyStructData = externalStructData.Collect();
        IncrementalValueProvider<(ImmutableArray<StructData> Left, ImmutableArray<StructData> Right)> combined = collectedStructData.Combine(collectedProxyStructData);
        return combined.SelectMany((pair, ct) => pair.Left.AddRange(pair.Right));
    }

    private static IncrementalValuesProvider<StructData> GetStructAttributes(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: StructAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is StructDeclarationSyntax,
            transform: GetStructAttributesTransform
        );
    }
    
    private static StructData GetStructAttributesTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        INamedTypeSymbol structSymbol = (INamedTypeSymbol)context.TargetSymbol;
        StructDeclarationSyntax structDeclaration = (StructDeclarationSyntax)context.TargetNode;
        Location structLocation = structDeclaration.Identifier.GetLocation();
        string structName = structSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (!structDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)) {
            return new StructData(
                TypeFullyQualifiedName: string.Empty,
                Alias: string.Empty,
                Members: ImmutableArray<StructMemberData>.Empty,
                Accessibility: Accessibility.NotApplicable,
                SettingsInterfaceFullyQualifiedName: null,
                IsProxyClass: false,
                DeclarationTypeFullyQualifiedName: string.Empty,
                Location: structLocation,
                Diagnostics: ImmutableArray.Create(DiagnosticData.Create(Diagnostics.StructNotPartial, structLocation, [structName]))
            );
        }

        return ParseStructData(context.Attributes[0], structSymbol);
    }
    
    public static StructData ParseStructData(AttributeData attributeData, INamedTypeSymbol structSymbol) {
        ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;

        string alias = DisplayNameUtility.GetDisplayName(structSymbol);
        string? settingsInterfaceFullyQualifiedName = null;

        object? firstArgument = null;
        object? secondArgument = null;
        if (constructorArguments.Length >= 1) { firstArgument = constructorArguments[0].Value; }
        if (constructorArguments.Length >= 2) { secondArgument = constructorArguments[1].Value; }

        Location? attributeLocation = attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation();
        List<DiagnosticData> diagnostics = [];

        switch (firstArgument) {
            case string aliasValue:
                alias = aliasValue;
                break;
            case ITypeSymbol settingsType:
                settingsInterfaceFullyQualifiedName = settingsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!settingsType.HasAttribute(SettingsAttribute)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidStructSettingsType, attributeLocation, [settingsInterfaceFullyQualifiedName]));
                }
                break;
        }

        switch (secondArgument) {
            case ITypeSymbol settingsType:
                settingsInterfaceFullyQualifiedName = settingsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!settingsType.HasAttribute(SettingsAttribute)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidStructSettingsType, attributeLocation, [settingsInterfaceFullyQualifiedName]));
                }
                break;
        }

        HashSet<string> excludedProperties = new HashSet<string>(StringComparer.Ordinal);
        HashSet<string> includedFields = new HashSet<string>(StringComparer.Ordinal);
        foreach (ISymbol member in structSymbol.GetMembers()) {
            switch (member) {
                case IPropertySymbol property when property.HasAttribute(ExcludeAttribute):
                    excludedProperties.Add(property.Name);
                    break;
                case IFieldSymbol field when field.HasAttribute(IncludeAttribute):
                    includedFields.Add(field.Name);
                    break;
            }
        }
        CollectMembersResult collectMembersResult = CollectMembers(structSymbol, excludedProperties, includedFields);
        diagnostics.AddRange(collectMembersResult.Diagnostics);

        string fullyQualifiedName = structSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        return new StructData(
            TypeFullyQualifiedName: fullyQualifiedName,
            Alias: alias,
            Members: collectMembersResult.Members.ToImmutableArray(),
            Accessibility: structSymbol.DeclaredAccessibility,
            SettingsInterfaceFullyQualifiedName: settingsInterfaceFullyQualifiedName,
            IsProxyClass: false,
            DeclarationTypeFullyQualifiedName: fullyQualifiedName,
            Location: attributeLocation,
            Diagnostics: diagnostics.ToImmutableArray()
        );
    }

    private static IncrementalValuesProvider<StructData> GetProxyStructAttributes(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: ProxyStructAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax,
            transform: GetProxyStructAttributesTransform
        );
    }

    private static StructData GetProxyStructAttributesTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        INamedTypeSymbol classSymbol = (INamedTypeSymbol)context.TargetSymbol;
        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
        Location classLocation = classDeclaration.Identifier.GetLocation();
        string className = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        string proxyClassFullyQualifiedName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (!classSymbol.IsStatic) {
            return new StructData(
                TypeFullyQualifiedName: string.Empty,
                Alias: string.Empty,
                Members: ImmutableArray<StructMemberData>.Empty,
                Accessibility: Accessibility.NotApplicable,
                SettingsInterfaceFullyQualifiedName: null,
                IsProxyClass: true,
                DeclarationTypeFullyQualifiedName: proxyClassFullyQualifiedName,
                Location: classLocation,
                Diagnostics: ImmutableArray.Create(DiagnosticData.Create(Diagnostics.ProxyStructClassNotStatic, classLocation, [className]))
            );
        }

        if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword)) {
            return new StructData(
                TypeFullyQualifiedName: string.Empty,
                Alias: string.Empty,
                Members: ImmutableArray<StructMemberData>.Empty,
                Accessibility: Accessibility.NotApplicable,
                SettingsInterfaceFullyQualifiedName: null,
                IsProxyClass: true,
                DeclarationTypeFullyQualifiedName: proxyClassFullyQualifiedName,
                Location: classLocation,
                Diagnostics: ImmutableArray.Create(DiagnosticData.Create(Diagnostics.ProxyStructClassNotPartial, classLocation, [className]))
            );
        }

        return ParseProxyStructData(context.Attributes[0], proxyClassFullyQualifiedName);
    }

    public static StructData ParseProxyStructData(AttributeData attributeData, string proxyClassFullyQualifiedName) {
        ImmutableArray<TypedConstant> constructorArguments = attributeData.ConstructorArguments;
        Location? attributeLocation = attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation();

        if (constructorArguments.Length == 0 || constructorArguments[0].Value is not ITypeSymbol targetType) {
            return new StructData(
                TypeFullyQualifiedName: string.Empty,
                Alias: string.Empty,
                Members: ImmutableArray<StructMemberData>.Empty,
                Accessibility: Accessibility.NotApplicable,
                SettingsInterfaceFullyQualifiedName: null,
                IsProxyClass: true,
                DeclarationTypeFullyQualifiedName: proxyClassFullyQualifiedName,
                Location: attributeLocation,
                Diagnostics: ImmutableArray.Create(DiagnosticData.Create(Diagnostics.ProxyStructNotStruct, attributeLocation, ["unknown"]))
            );
        }
        if (targetType is not INamedTypeSymbol structSymbol) {
            return new StructData(
                TypeFullyQualifiedName: string.Empty,
                Alias: string.Empty,
                Members: ImmutableArray<StructMemberData>.Empty,
                Accessibility: Accessibility.NotApplicable,
                SettingsInterfaceFullyQualifiedName: null,
                IsProxyClass: true,
                DeclarationTypeFullyQualifiedName: proxyClassFullyQualifiedName,
                Location: attributeLocation,
                Diagnostics: ImmutableArray.Create(DiagnosticData.Create(Diagnostics.ProxyStructNotStruct, attributeLocation, [targetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)]))
            );
        }
        if (structSymbol.TypeKind != TypeKind.Struct) {
            return new StructData(
                TypeFullyQualifiedName: string.Empty,
                Alias: string.Empty,
                Members: ImmutableArray<StructMemberData>.Empty,
                Accessibility: Accessibility.NotApplicable,
                SettingsInterfaceFullyQualifiedName: null,
                IsProxyClass: true,
                DeclarationTypeFullyQualifiedName: proxyClassFullyQualifiedName,
                Location: attributeLocation,
                Diagnostics: ImmutableArray.Create(DiagnosticData.Create(Diagnostics.ProxyStructNotStruct, attributeLocation, [structSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)]))
            );
        }

        string alias = DisplayNameUtility.GetDisplayName(structSymbol);
        string? settingsInterfaceFullyQualifiedName = null;
        string?[] includes = [];
        string?[] ignores = [];

        object? secondArgument = null;
        object? thirdArgument = null;
        object? fourthArgument = null;
        object? fifthArgument = null;
        if (constructorArguments.Length >= 2) { secondArgument = GetArgumentValue(constructorArguments[1]); }
        if (constructorArguments.Length >= 3) { thirdArgument = GetArgumentValue(constructorArguments[2]); }
        if (constructorArguments.Length >= 4) { fourthArgument = GetArgumentValue(constructorArguments[3]); }
        if (constructorArguments.Length >= 5) { fifthArgument = GetArgumentValue(constructorArguments[4]); }

        List<DiagnosticData> diagnostics = [];

        switch (secondArgument) {
            case string aliasValue:
                alias = aliasValue;
                break;
            case TypedConstant includesArray:
                includes = ExtractStringArray(includesArray);
                break;
            case ITypeSymbol settingsType:
                settingsInterfaceFullyQualifiedName = settingsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!settingsType.HasAttribute(SettingsAttribute)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidStructSettingsType, attributeLocation, [settingsInterfaceFullyQualifiedName]));
                }
                break;
        }
        
        switch (thirdArgument) {
            case TypedConstant ignoresArray:
                ignores = ExtractStringArray(ignoresArray);
                break;
            case ITypeSymbol settingsType:
                settingsInterfaceFullyQualifiedName = settingsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!settingsType.HasAttribute(SettingsAttribute)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidStructSettingsType, attributeLocation, [settingsInterfaceFullyQualifiedName]));
                }
                break;
        }
        
        switch (fourthArgument) {
            case ITypeSymbol settingsType:
                settingsInterfaceFullyQualifiedName = settingsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!settingsType.HasAttribute(SettingsAttribute)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidStructSettingsType, attributeLocation, [settingsInterfaceFullyQualifiedName]));
                }
                break;
            case string aliasValue:
                alias = aliasValue;
                break;
        }

        switch (fifthArgument) {
            case ITypeSymbol settingsType:
                settingsInterfaceFullyQualifiedName = settingsType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!settingsType.HasAttribute(SettingsAttribute)) {
                    diagnostics.Add(DiagnosticData.Create(Diagnostics.InvalidStructSettingsType, attributeLocation, [settingsInterfaceFullyQualifiedName]));
                }
                break;
        }

        HashSet<string> includesSet = new HashSet<string>(StringComparer.Ordinal);
        foreach (string? include in includes) {
            if (include != null) { includesSet.Add(include); }
        }
        
        HashSet<string> ignoresSet = new HashSet<string>(StringComparer.Ordinal);
        foreach (string? ignore in ignores) {
            if (ignore != null) { ignoresSet.Add(ignore); }
        }
        
        foreach (string includeName in includesSet) {
            if (!structSymbol.GetMembers(includeName).Any()) { diagnostics.Add(DiagnosticData.Create(Diagnostics.ProxyStructIncludeNotFound, attributeLocation, [includeName])); }
        }

        HashSet<string> excludedProperties = new HashSet<string>(ignoresSet, StringComparer.Ordinal);
        if (includesSet.Count > 0) {
            foreach (ISymbol member in structSymbol.GetMembers()) {
                if (member is IPropertySymbol property && !includesSet.Contains(property.Name)) {
                    excludedProperties.Add(property.Name);
                }
            }
        }
        
        HashSet<string> effectiveIncludes = new HashSet<string>(includesSet, StringComparer.Ordinal);
        effectiveIncludes.ExceptWith(ignoresSet);
        CollectMembersResult collected = CollectMembers(structSymbol, excludedProperties, effectiveIncludes);

        diagnostics.AddRange(collected.Diagnostics);

        return new StructData(
            TypeFullyQualifiedName: structSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Alias: alias,
            Members: collected.Members.ToImmutableArray(),
            Accessibility: structSymbol.DeclaredAccessibility,
            SettingsInterfaceFullyQualifiedName: settingsInterfaceFullyQualifiedName,
            IsProxyClass: true,
            DeclarationTypeFullyQualifiedName: proxyClassFullyQualifiedName,
            Location: attributeLocation,
            Diagnostics: diagnostics.ToImmutableArray()
        );
    }

    private readonly record struct CollectMembersResult(List<StructMemberData> Members, List<DiagnosticData> Diagnostics);

    private static CollectMembersResult CollectMembers(INamedTypeSymbol structSymbol, HashSet<string> excludedProperties, HashSet<string> includedFields) { 
        List<DiagnosticData> diagnostics = [];
        List<StructMemberData> members = [];

        IEnumerable<ISymbol> orderedMembers = structSymbol.GetMembers()
            .OrderBy(member => member.DeclaringSyntaxReferences.FirstOrDefault()?.Span.Start ?? 0);

        foreach (ISymbol member in orderedMembers) {
            switch (member) {
                case IPropertySymbol property: {
                    if (property.DeclaredAccessibility != Accessibility.Public) { continue; }
                    if (property.IsStatic) { continue; }
                    if (property.IsImplicitlyDeclared) { continue; }
                    if (property.IsIndexer) { continue; }
                    if (excludedProperties.Contains(property.Name)) { continue; }
                    if (property.GetMethod is null || property.GetMethod.DeclaredAccessibility != Accessibility.Public) { continue; }
                    if (property.SetMethod is null) {
                        diagnostics.Add(DiagnosticData.Create(Diagnostics.ReadOnlyPropertySkipped, property.Locations.FirstOrDefault(), [property.Name]));
                        continue;
                    }
                    if (property.SetMethod.DeclaredAccessibility != Accessibility.Public) {
                        diagnostics.Add(DiagnosticData.Create(Diagnostics.NonPublicSetterSkipped, property.Locations.FirstOrDefault(), [property.Name]));
                        continue;
                    }

                    string typeFullyQualifiedFormat = property.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    members.Add(new StructMemberData(
                        MemberName: property.Name,
                        TypeFullyQualifiedFormat: typeFullyQualifiedFormat,
                        IsProperty: true,
                        IsInitOnly: property.SetMethod.IsInitOnly,
                        Location: property.Locations.FirstOrDefault()
                    ));
                    break;
                }
                case IFieldSymbol field: {
                    if (field.DeclaredAccessibility != Accessibility.Public) { continue; }
                    if (field.IsStatic) { continue; }
                    if (field.IsImplicitlyDeclared) { continue; }
                    if (!includedFields.Contains(field.Name)) { continue; }
                    if (field.IsReadOnly || field.IsConst) {
                        diagnostics.Add(DiagnosticData.Create(Diagnostics.ReadOnlyFieldSkipped, field.Locations.FirstOrDefault(), [field.Name]));
                        continue;
                    }
                    if (field.RefKind != RefKind.None) {
                        diagnostics.Add(DiagnosticData.Create(Diagnostics.RefFieldSkipped, field.Locations.FirstOrDefault(), [field.Name]));
                        continue;
                    }

                    string typeFullyQualifiedFormat = field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    members.Add(new StructMemberData(
                        MemberName: field.Name,
                        TypeFullyQualifiedFormat: typeFullyQualifiedFormat,
                        IsProperty: false,
                        IsInitOnly: false,
                        Location: field.Locations.FirstOrDefault()
                    ));
                    break;
                }
            }
        }

        return new CollectMembersResult(members, diagnostics);
    }

    // stupid and ugly
    private static object? GetArgumentValue(TypedConstant constant) {
        return constant.Kind == TypedConstantKind.Array ? constant : constant.Value;
    }

    private static string?[] ExtractStringArray(TypedConstant constant) {
        ImmutableArray<TypedConstant> values = constant.Values;
        if (values.IsDefaultOrEmpty) { return []; }

        string?[] result = new string?[values.Length];
        for (int i = 0; i < values.Length; i++) {
            result[i] = values[i].Value as string;
        }

        return result;
    }
}