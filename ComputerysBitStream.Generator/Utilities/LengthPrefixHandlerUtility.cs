using System;
using System.Collections.Generic;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Emitters;

namespace ComputerysBitStream.Generator;

internal static class LengthPrefixHandlerUtility {
    public static PrimitiveDefinition? Find(SettingsDefinition settings) {
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in settings.Primitives) {
            PrimitiveDefinition candidate = pair.Value;
            if (candidate.Mode != PrimitiveSerializationMode.VariableLength) { continue; }
            if (!string.Equals(candidate.TargetTypeFullyQualifiedName, BitStreamTypeNames.UInt32, StringComparison.Ordinal)) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.Write)) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.TryRead)) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.Size)) { continue; }

            return candidate;
        }

        return null;
    }
}
