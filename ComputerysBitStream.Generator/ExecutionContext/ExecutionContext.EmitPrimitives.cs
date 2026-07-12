using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Diagnostics;
using ComputerysBitStream.Generator.Emitters;
using ComputerysBitStream.Generator.Emission;

namespace ComputerysBitStream.Generator;

internal readonly ref partial struct ExecutionContext {
    private void EmitPrimitiveDefinitions(ImmutableArray<PrimitiveDefinition> primitives) {
        PrimitiveDefinition? globalIntHandler = LengthPrefixHandlerUtility.Find(_globalSettings);

        HashSet<string> usedEmissionKeys = [];
        foreach (PrimitiveDefinition primitive in primitives) {
            string emissionKey = $"{primitive.Namespace}|{primitive.Alias}";
            if (!usedEmissionKeys.Add(emissionKey)) {
                _context.ReportDiagnostic(new DiagnosticValueType(DiagnosticDescriptors.DuplicatePrimitiveDefinition, primitive.Location, primitive.TargetTypeFullyQualifiedName, primitive.Alias, primitive.Namespace).ToDiagnostic());
                continue;
            }

            PrimitiveDefinition? intHandler = ResolveLengthPrefixHandler(primitive, globalIntHandler);
            if (NeedsLengthPrefixHandlerDiagnostic(primitive, intHandler)) {
                _context.ReportDiagnostic(new DiagnosticValueType(DiagnosticDescriptors.MissingLengthPrefixHandler, primitive.Location, primitive.Alias, primitive.TargetTypeFullyQualifiedName).ToDiagnostic());
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

            PrimitiveDefinition? handler = LengthPrefixHandlerUtility.Find(localSettings);
            if (handler is not null) { return handler; }
        }

        if (reference.ExternalSettings is not null) {
            PrimitiveDefinition? handler = LengthPrefixHandlerUtility.Find(reference.ExternalSettings);
            if (handler is not null) { return handler; }
        }

        return globalIntHandler;
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
