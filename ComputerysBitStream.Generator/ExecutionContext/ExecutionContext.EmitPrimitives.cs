using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Emitters;

namespace ComputerysBitStream.Generator;

internal readonly ref partial struct ExecutionContext {
    private void EmitPrimitiveDefinitions(ImmutableArray<PrimitiveDefinition> primitives) {
        PrimitiveDefinition? globalIntHandler = FindLengthPrefixHandler(_globalSettings);

        HashSet<string> usedEmissionKeys = [];
        foreach (PrimitiveDefinition primitive in primitives) {
            string emissionKey = $"{primitive.Namespace}|{primitive.Alias}";
            if (!usedEmissionKeys.Add(emissionKey)) {
                _context.ReportDiagnostic(new DiagnosticValueType(Diagnostics.DuplicatePrimitiveDefinition, primitive.Location, primitive.TargetTypeFullyQualifiedName, primitive.Alias, primitive.Namespace).ToDiagnostic());
                continue;
            }

            PrimitiveDefinition? intHandler = ResolveLengthPrefixHandler(primitive, globalIntHandler);
            if (NeedsLengthPrefixHandlerDiagnostic(primitive, intHandler)) {
                _context.ReportDiagnostic(new DiagnosticValueType(Diagnostics.MissingLengthPrefixHandler, primitive.Location, primitive.Alias, primitive.TargetTypeFullyQualifiedName).ToDiagnostic());
            }

            _context.AddSource(GetPrimitiveHintName(primitive), PrimitiveWrapperSourceEmitter.EmitSource(primitive, intHandler));
        }
    }

    private static string GetPrimitiveHintName(in PrimitiveDefinition primitive) {
        return GeneratedSourceSyntax.GetSourceHintFileName(primitive.Namespace, $"{primitive.Alias}ContextExtensions");
    }

    private PrimitiveDefinition? ResolveLengthPrefixHandler(in PrimitiveDefinition primitive, PrimitiveDefinition? globalIntHandler) {
        if (primitive.Settings is not SettingsReference reference) { return globalIntHandler; }

        foreach (string interfaceName in reference.LocalSettingsInterfaceFullyQualifiedNames) {
            if (!_localSettingsByInterface.TryGetValue(interfaceName, out SettingsDefinition? localSettings)) { continue; }

            PrimitiveDefinition? handler = FindLengthPrefixHandler(localSettings);
            if (handler is not null) { return handler; }
        }

        if (reference.ExternalSettings is not null) {
            PrimitiveDefinition? handler = FindLengthPrefixHandler(reference.ExternalSettings);
            if (handler is not null) { return handler; }
        }

        return globalIntHandler;
    }

    private static PrimitiveDefinition? FindLengthPrefixHandler(SettingsDefinition settings) {
        PrimitiveDefinition? best = null;
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in settings.Primitives) {
            PrimitiveDefinition candidate = pair.Value;
            if (candidate.Mode != PrimitiveSerializationMode.FixedSize) { continue; } // TODO: make this support Variable length types as well.
            if (!string.Equals(candidate.TargetTypeFullyQualifiedName, BitStreamTypeNames.Int32, StringComparison.Ordinal)) { continue; }
            if (candidate.FixedSize is not int fixedSize || fixedSize <= 0) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.Write)) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.Peek)) { continue; }

            if (best is not PrimitiveDefinition current || IsPreferredLengthPrefixHandler(candidate, current)) { best = candidate; }
        }

        return best;
    }

    private static bool IsPreferredLengthPrefixHandler(in PrimitiveDefinition candidate, in PrimitiveDefinition current) {
        bool candidateIsDefaultAlias = string.Equals(candidate.Alias, DisplayNameUtility.DefaultInt32Alias, StringComparison.Ordinal);
        bool currentIsDefaultAlias = string.Equals(current.Alias, DisplayNameUtility.DefaultInt32Alias, StringComparison.Ordinal);
        if (candidateIsDefaultAlias != currentIsDefaultAlias) { return candidateIsDefaultAlias; }

        return string.CompareOrdinal(candidate.ExtensionClassFullyQualifiedName, current.ExtensionClassFullyQualifiedName) < 0;
    }

    private static bool NeedsLengthPrefixHandlerDiagnostic(in PrimitiveDefinition primitive, PrimitiveDefinition? intHandler) {
        if (NeedsLengthPrefixWriteHandler(primitive, intHandler)) { return true; }
        return NeedsLengthPrefixReadHandler(primitive, intHandler);
    }

    private static bool NeedsLengthPrefixWriteHandler(in PrimitiveDefinition primitive, PrimitiveDefinition? intHandler) {
        if (!PrimitiveWrapperSourceEmitter.HasValidMethod(primitive, BitStreamPrimitiveRole.WriteSpan)) { return false; }
        return intHandler is not PrimitiveDefinition handler || !PrimitiveWrapperSourceEmitter.HasValidMethod(handler, BitStreamPrimitiveRole.Write);
    }

    private static bool NeedsLengthPrefixReadHandler(in PrimitiveDefinition primitive, PrimitiveDefinition? intHandler) {
        bool needsPrefix = PrimitiveWrapperSourceEmitter.HasValidMethod(primitive, BitStreamPrimitiveRole.PeekArray)
                           || PrimitiveWrapperSourceEmitter.HasValidMethod(primitive, BitStreamPrimitiveRole.ReadArray)
                           || PrimitiveWrapperSourceEmitter.HasValidMethod(primitive, BitStreamPrimitiveRole.PeekSpan)
                           || PrimitiveWrapperSourceEmitter.HasValidMethod(primitive, BitStreamPrimitiveRole.ReadSpan);
        if (!needsPrefix) { return false; }
        return intHandler is not PrimitiveDefinition handler || !PrimitiveWrapperSourceEmitter.HasValidMethod(handler, BitStreamPrimitiveRole.Peek);
    }
}
