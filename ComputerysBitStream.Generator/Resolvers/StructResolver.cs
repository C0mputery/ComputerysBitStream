using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator;

internal sealed class StructResolver {
    private readonly Action<DiagnosticValueType> _reportDiagnostic;
    private readonly SettingsDefinition _globalSettings;
    private readonly ImmutableDictionary<string, SettingsDefinition> _localSettingsByInterface;
    private readonly Dictionary<string, PrimitiveDefinition> _primitivesByExtensionClass;
    private readonly Dictionary<string, PrimitiveDefinition> _primitivesByTargetType;
    private readonly Dictionary<string, ResolvedStructDefinition?> _resolvedStructs = new();
    private readonly HashSet<string> _computingStructs = new();

    internal StructResolver(
        Action<DiagnosticValueType> reportDiagnostic,
        SettingsDefinition globalSettings,
        ImmutableDictionary<string, SettingsDefinition> localSettingsByInterface,
        IEnumerable<PrimitiveDefinition> additionalPrimitives
    ) {
        _reportDiagnostic = reportDiagnostic;
        _globalSettings = globalSettings;
        _localSettingsByInterface = localSettingsByInterface;
        _primitivesByExtensionClass = new Dictionary<string, PrimitiveDefinition>(StringComparer.Ordinal);
        _primitivesByTargetType = new Dictionary<string, PrimitiveDefinition>(StringComparer.Ordinal);

        IndexPrimitives(globalSettings.Primitives);
        foreach (SettingsDefinition localSettings in localSettingsByInterface.Values) { IndexPrimitives(localSettings.Primitives); }
        foreach (PrimitiveDefinition primitive in additionalPrimitives) { IndexPrimitive(primitive); }
    }

    internal ResolvedStructDefinition? Resolve(in StructDefinition structDefinition) {
        string typeFqn = structDefinition.TypeFullyQualifiedName;
        if (string.IsNullOrEmpty(typeFqn)) { return null; }

        if (_resolvedStructs.TryGetValue(typeFqn, out ResolvedStructDefinition? cached)) { return cached; }

        if (!_computingStructs.Add(typeFqn)) {
            _reportDiagnostic(new DiagnosticValueType(Diagnostics.CyclicStructReference, structDefinition.Location, typeFqn));
            _resolvedStructs[typeFqn] = null;
            return null;
        }

        SettingsDefinition effectiveSettings = GetEffectiveSettings(structDefinition.Settings);
        string settingsLabel = GetSettingsLabel(structDefinition.Settings);
        string generatedNamespace = GetGeneratedNamespace(structDefinition);
        List<string> requiredUsings = [];

        ImmutableArray<ResolvedStructMember>.Builder resolvedMembers = ImmutableArray.CreateBuilder<ResolvedStructMember>();
        bool isVariableLength = false;
        int aggregateFixedSize = 0;
        bool anyMemberSkipped = false;

        foreach (StructMemberDefinition member in structDefinition.Members) {
            if (!TryResolveMember(member, effectiveSettings, settingsLabel, generatedNamespace, requiredUsings, out ResolvedStructMember resolvedMember, out bool memberIsVariableLength, out int memberFixedBits)) {
                anyMemberSkipped = true;
                continue;
            }

            resolvedMembers.Add(resolvedMember);
            isVariableLength |= memberIsVariableLength;
            if (!memberIsVariableLength) { aggregateFixedSize += memberFixedBits; }
        }

        _computingStructs.Remove(typeFqn);

        if (anyMemberSkipped || resolvedMembers.Count == 0) {
            if (resolvedMembers.Count == 0) {
                _reportDiagnostic(new DiagnosticValueType(Diagnostics.StructNoSerializableMembers, structDefinition.Location, typeFqn));
            }

            _resolvedStructs[typeFqn] = null;
            return null;
        }

        string alias = structDefinition.Alias;
        string extensionClassFqn = GetStructPrimitiveExtensionClassFqn(alias, generatedNamespace);
        PrimitiveSerializationMode mode = isVariableLength ? PrimitiveSerializationMode.VariableLength : PrimitiveSerializationMode.FixedSize;
        EquatableImmutableDictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methods = StructPrimitiveDefinitionFactory.CreateMethodDefinitions(alias, mode);

        ResolvedStructDefinition resolved = new(
            Source: structDefinition,
            Mode: mode,
            FixedSize: isVariableLength ? null : aggregateFixedSize,
            Members: resolvedMembers.ToImmutable(),
            PrimitiveExtensionClassFqn: extensionClassFqn,
            Methods: methods,
            RequiredUsings: requiredUsings.ToImmutableArray()
        );

        _resolvedStructs[typeFqn] = resolved;
        return resolved;
    }

    private bool TryResolveMember(
        in StructMemberDefinition member,
        SettingsDefinition effectiveSettings,
        string settingsLabel,
        string generatedNamespace,
        List<string> requiredUsings,
        out ResolvedStructMember resolvedMember,
        out bool isVariableLength,
        out int fixedBits
    ) {
        resolvedMember = default;
        isVariableLength = false;
        fixedBits = 0;

        string memberType = member.TypeFullyQualifiedFormat;
        string memberAccess = $"value.{member.MemberName}";

        if (!string.IsNullOrEmpty(member.SerializerExtensionClassFullyQualifiedName)
            && effectiveSettings.Primitives.TryGetValue(member.SerializerExtensionClassFullyQualifiedName!, out PrimitiveDefinition serializerPrimitive)) {
            return TryCreatePrimitiveMember(member, serializerPrimitive, memberAccess, generatedNamespace, requiredUsings, out resolvedMember, out isVariableLength, out fixedBits);
        }

        if (member.QuantizedRange is QuantizedRangeDefinition quantizedRange) {
            if (!TryFindPrimitiveByTargetType(effectiveSettings, memberType, PrimitiveSerializationMode.Quantized, out PrimitiveDefinition quantizedPrimitive)) {
                _reportDiagnostic(new DiagnosticValueType(
                    Diagnostics.QuantizedPrimitiveNotInSettings,
                    member.Location ?? quantizedRange.Location,
                    member.MemberName,
                    memberType
                ));
                return false;
            }

            string min = quantizedRange.MinExpression;
            string max = quantizedRange.MaxExpression;
            int bitCount = quantizedRange.BitCount;
            string writeMethod = GetPrimitiveMethodName(quantizedPrimitive, BitStreamPrimitiveRole.Write);
            string readMethod = GetPrimitiveMethodName(quantizedPrimitive, BitStreamPrimitiveRole.Read);
            string extensionClass = QualifyPrimitiveExtension(quantizedPrimitive, generatedNamespace, requiredUsings);
            string readExpression = $"{extensionClass}.{readMethod}(ref context, {min}, {max}, {bitCount})";
            MemberTryReadSpec tryRead = CreateMemberTryRead(string.Empty, string.Empty, bitCount, isVariableLength: false);

            resolvedMember = new ResolvedStructMember(
                MemberName: member.MemberName,
                TypeFullyQualifiedName: memberType,
                IsInitOnly: member.IsInitOnly,
                Kind: ResolvedStructMemberKind.Quantized,
                WriteCall: $"{extensionClass}.{writeMethod}(ref context, {memberAccess}, {min}, {max}, {bitCount})",
                ReadExpression: readExpression,
                TryRead: tryRead,
                SizeExpression: bitCount.ToString(),
                QuantizedRange: quantizedRange
            );
            fixedBits = bitCount;
            return true;
        }

        if (effectiveSettings.Structs.TryGetValue(memberType, out StructDefinition nestedStruct)) {
            ResolvedStructDefinition? nestedResolved = Resolve(nestedStruct);
            if (nestedResolved is not ResolvedStructDefinition nested) {
                _reportDiagnostic(new DiagnosticValueType(Diagnostics.StructMemberNotSerializable, member.Location, memberType, settingsLabel));
                return false;
            }

            string nestedAlias = nestedStruct.Alias;
            string nestedExtensionClass = QualifyExtensionClass(generatedNamespace, nested.PrimitiveExtensionClassFqn, requiredUsings);
            string nestedWrite = StructPrimitiveDefinitionFactory.GetMethodName(nestedAlias, BitStreamPrimitiveRole.Write);
            string nestedRead = StructPrimitiveDefinitionFactory.GetMethodName(nestedAlias, BitStreamPrimitiveRole.Read);
            string nestedTryRead = StructPrimitiveDefinitionFactory.GetMethodName(nestedAlias, BitStreamPrimitiveRole.TryRead);
            string nestedSize = StructPrimitiveDefinitionFactory.GetMethodName(nestedAlias, BitStreamPrimitiveRole.Size);
            bool nestedIsVariableLength = nested.Mode == PrimitiveSerializationMode.VariableLength;
            string nestedReadExpression = $"{nestedExtensionClass}.{nestedRead}(ref context)";
            MemberTryReadSpec tryRead = CreateMemberTryRead(
                nestedExtensionClass,
                nestedTryRead,
                nestedIsVariableLength ? 0 : (nested.FixedSize ?? 0),
                nestedIsVariableLength
            );

            resolvedMember = new ResolvedStructMember(
                MemberName: member.MemberName,
                TypeFullyQualifiedName: memberType,
                IsInitOnly: member.IsInitOnly,
                Kind: ResolvedStructMemberKind.NestedStruct,
                WriteCall: $"{nestedExtensionClass}.{nestedWrite}(ref context, {memberAccess})",
                ReadExpression: nestedReadExpression,
                TryRead: tryRead,
                SizeExpression: nestedIsVariableLength
                    ? $"{nestedExtensionClass}.{nestedSize}({memberAccess})"
                    : (nested.FixedSize ?? 0).ToString(),
                QuantizedRange: null
            );
            isVariableLength = nestedIsVariableLength;
            fixedBits = isVariableLength ? 0 : (nested.FixedSize ?? 0);
            return true;
        }

        if (effectiveSettings.ExternalStructs.TryGetValue(memberType, out ExternalStructDefinition externalStruct)) {
            string externalExtensionClass = QualifyExtensionClass(
                generatedNamespace,
                GetStructPrimitiveExtensionClassFqn(externalStruct.Alias, externalStruct.ExtensionNamespace),
                requiredUsings
            );
            string externalWrite = StructPrimitiveDefinitionFactory.GetMethodName(externalStruct.Alias, BitStreamPrimitiveRole.Write);
            string externalRead = StructPrimitiveDefinitionFactory.GetMethodName(externalStruct.Alias, BitStreamPrimitiveRole.Read);
            string externalTryRead = StructPrimitiveDefinitionFactory.GetMethodName(externalStruct.Alias, BitStreamPrimitiveRole.TryRead);
            string externalSize = StructPrimitiveDefinitionFactory.GetMethodName(externalStruct.Alias, BitStreamPrimitiveRole.Size);
            bool isVariableLengthExternal = externalStruct.IsVariableLength;
            string externalReadExpression = $"{externalExtensionClass}.{externalRead}(ref context)";
            MemberTryReadSpec tryRead = CreateMemberTryRead(
                externalExtensionClass,
                externalTryRead,
                isVariableLengthExternal ? 0 : externalStruct.Size,
                isVariableLengthExternal
            );

            resolvedMember = new ResolvedStructMember(
                MemberName: member.MemberName,
                TypeFullyQualifiedName: memberType,
                IsInitOnly: member.IsInitOnly,
                Kind: ResolvedStructMemberKind.ExternalStruct,
                WriteCall: $"{externalExtensionClass}.{externalWrite}(ref context, {memberAccess})",
                ReadExpression: externalReadExpression,
                TryRead: tryRead,
                SizeExpression: isVariableLengthExternal
                    ? $"{externalExtensionClass}.{externalSize}({memberAccess})"
                    : externalStruct.Size.ToString(),
                QuantizedRange: null
            );
            isVariableLength = isVariableLengthExternal;
            fixedBits = isVariableLengthExternal ? 0 : externalStruct.Size;
            return true;
        }

        if (TryFindPrimitiveByTargetType(effectiveSettings, memberType, out PrimitiveDefinition primitive)) {
            return TryCreatePrimitiveMember(member, primitive, memberAccess, generatedNamespace, requiredUsings, out resolvedMember, out isVariableLength, out fixedBits);
        }

        _reportDiagnostic(new DiagnosticValueType(Diagnostics.StructMemberNotSerializable, member.Location, memberType, settingsLabel));
        return false;
    }

    private bool TryCreatePrimitiveMember(
        in StructMemberDefinition member,
        in PrimitiveDefinition primitive,
        string memberAccess,
        string generatedNamespace,
        List<string> requiredUsings,
        out ResolvedStructMember resolvedMember,
        out bool isVariableLength,
        out int fixedBits
    ) {
        string extensionClass = QualifyPrimitiveExtension(primitive, generatedNamespace, requiredUsings);
        string writeMethod = GetPrimitiveMethodName(primitive, BitStreamPrimitiveRole.Write);
        string readMethod = GetPrimitiveMethodName(primitive, BitStreamPrimitiveRole.Read);
        string tryReadMethod = GetPrimitiveMethodName(primitive, BitStreamPrimitiveRole.TryRead);
        string sizeMethod = GetPrimitiveMethodName(primitive, BitStreamPrimitiveRole.Size);

        isVariableLength = primitive.Mode == PrimitiveSerializationMode.VariableLength;
        fixedBits = isVariableLength ? 0 : (primitive.FixedSize ?? 0);
        string readExpression = $"{extensionClass}.{readMethod}(ref context)";
        MemberTryReadSpec tryRead = CreateMemberTryRead(
            extensionClass,
            tryReadMethod,
            fixedBits,
            isVariableLength
        );

        resolvedMember = new ResolvedStructMember(
            MemberName: member.MemberName,
            TypeFullyQualifiedName: member.TypeFullyQualifiedFormat,
            IsInitOnly: member.IsInitOnly,
            Kind: ResolvedStructMemberKind.Primitive,
            WriteCall: $"{extensionClass}.{writeMethod}(ref context, {memberAccess})",
            ReadExpression: readExpression,
            TryRead: tryRead,
            SizeExpression: isVariableLength
                ? $"{extensionClass}.{sizeMethod}({memberAccess})"
                : (primitive.FixedSize ?? 0).ToString(),
            QuantizedRange: null
        );
        return true;
    }

    private static MemberTryReadSpec CreateMemberTryRead(
        string extensionClass,
        string tryReadMethod,
        int fixedBits,
        bool isVariableLength
    ) {
        if (isVariableLength && !string.IsNullOrEmpty(tryReadMethod)) {
            return new MemberTryReadSpec(MemberTryReadKind.TryReadOut, $"{extensionClass}.{tryReadMethod}", 0);
        }

        return new MemberTryReadSpec(MemberTryReadKind.PreflightThenRead, null, fixedBits);
    }

    private static string GetPrimitiveMethodName(in PrimitiveDefinition primitive, BitStreamPrimitiveRole role) {
        if (primitive.Methods.TryGetValue(role, out PrimitiveMethodDefinition method) && method.IsValid) { return method.MethodName; }
        return string.Empty;
    }

    private static string QualifyPrimitiveExtension(in PrimitiveDefinition primitive, string generatedNamespace, List<string> requiredUsings) {
        return GeneratedSourceSyntax.QualifyTypeReference(generatedNamespace, primitive.ExtensionClassFullyQualifiedName, requiredUsings);
    }

    private static string QualifyExtensionClass(string generatedNamespace, string extensionClassFqn, List<string> requiredUsings) {
        GeneratedSourceSyntax.CollectAdditionalUsings(
            requiredUsings,
            GeneratedSourceSyntax.GetNamespaceFromFullyQualifiedName(extensionClassFqn),
            generatedNamespace
        );
        return GeneratedSourceSyntax.GetShortTypeName(extensionClassFqn);
    }

    private static string GetGeneratedNamespace(in StructDefinition structDefinition) {
        return structDefinition.Namespace ?? nameof(ComputerysBitStream);
    }

    internal static string GetStructPrimitiveExtensionClassFqn(string alias, string? namespaceName) {
        string className = $"{alias}StructPrimitiveExtensions";
        return namespaceName is null ? $"{nameof(ComputerysBitStream)}.{className}" : $"{namespaceName}.{className}";
    }

    private bool TryFindPrimitiveByTargetType(SettingsDefinition settings, string targetTypeFqn, out PrimitiveDefinition primitive) {
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in settings.Primitives) {
            if (string.Equals(pair.Value.TargetTypeFullyQualifiedName, targetTypeFqn, StringComparison.Ordinal)) {
                primitive = pair.Value;
                return true;
            }
        }

        if (_primitivesByTargetType.TryGetValue(targetTypeFqn, out primitive)) { return true; }
        primitive = default;
        return false;
    }

    private static bool TryFindPrimitiveByTargetType(SettingsDefinition settings, string targetTypeFqn, PrimitiveSerializationMode mode, out PrimitiveDefinition primitive) {
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in settings.Primitives) {
            PrimitiveDefinition candidate = pair.Value;
            if (candidate.Mode != mode) { continue; }
            if (string.Equals(candidate.TargetTypeFullyQualifiedName, targetTypeFqn, StringComparison.Ordinal)) {
                primitive = candidate;
                return true;
            }
        }

        primitive = default;
        return false;
    }

    private SettingsDefinition GetEffectiveSettings(SettingsReference? settingsReference) {
        Dictionary<string, PrimitiveDefinition> primitives = new();
        Dictionary<string, StructDefinition> structs = new();
        Dictionary<string, ExternalStructDefinition> externalStructs = new();

        MergeSettings(_globalSettings, primitives, structs, externalStructs);

        if (settingsReference?.ExternalSettings is SettingsDefinition externalSettings) {
            MergeSettings(externalSettings, primitives, structs, externalStructs);
        }

        if (settingsReference is SettingsReference reference) {
            foreach (string interfaceName in reference.LocalSettingsInterfaceFullyQualifiedNames) {
                if (!_localSettingsByInterface.TryGetValue(interfaceName, out SettingsDefinition? localSettings)) { continue; }
                MergeSettings(localSettings, primitives, structs, externalStructs);
            }
        }

        return new SettingsDefinition(
            InterfaceFullyQualifiedNames: [],
            Primitives: primitives.ToImmutableDictionary(),
            Structs: structs.ToImmutableDictionary(),
            ExternalStructs: externalStructs.ToImmutableDictionary(),
            Location: settingsReference?.Location
        );
    }

    private static void MergeSettings(
        SettingsDefinition source,
        Dictionary<string, PrimitiveDefinition> primitives,
        Dictionary<string, StructDefinition> structs,
        Dictionary<string, ExternalStructDefinition> externalStructs
    ) {
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in source.Primitives) { primitives[pair.Key] = pair.Value; }
        foreach (KeyValuePair<string, StructDefinition> pair in source.Structs) { structs[pair.Key] = pair.Value; }
        foreach (KeyValuePair<string, ExternalStructDefinition> pair in source.ExternalStructs) { externalStructs[pair.Key] = pair.Value; }
    }

    private static string GetSettingsLabel(SettingsReference? settingsReference) {
        if (settingsReference is not SettingsReference reference) { return "global"; }
        ImmutableArray<string> localInterfaces = reference.LocalSettingsInterfaceFullyQualifiedNames;
        if (localInterfaces.Length > 0) {
            return string.Join(", ", localInterfaces);
        }

        if (reference.ExternalSettings is SettingsDefinition externalSettings) {
            ImmutableArray<string> externalInterfaces = externalSettings.InterfaceFullyQualifiedNames;
            if (externalInterfaces.Length > 0) {
                return string.Join(", ", externalInterfaces);
            }
        }

        return "global";
    }

    private void IndexPrimitives(EquatableImmutableDictionary<string, PrimitiveDefinition> primitives) {
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in primitives) { IndexPrimitive(pair.Value); }
    }

    private void IndexPrimitive(in PrimitiveDefinition primitive) {
        if (!string.IsNullOrEmpty(primitive.ExtensionClassFullyQualifiedName)) {
            _primitivesByExtensionClass[primitive.ExtensionClassFullyQualifiedName] = primitive;
        }

        if (!string.IsNullOrEmpty(primitive.TargetTypeFullyQualifiedName)) {
            _primitivesByTargetType[primitive.TargetTypeFullyQualifiedName] = primitive;
        }
    }
}
