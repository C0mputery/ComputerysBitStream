using System;
using System.Collections.Generic;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Emitters;
using ComputerysBitStream.Generator.Roslyn;

namespace ComputerysBitStream.Generator;

internal static class LengthPrefixHandlerUtility {
    public static PrimitiveDefinition? Find(SettingsDefinition settings) {
        PrimitiveDefinition? best = null;
        foreach (KeyValuePair<string, PrimitiveDefinition> pair in settings.Primitives) {
            PrimitiveDefinition candidate = pair.Value;
            if (candidate.Mode != PrimitiveSerializationMode.FixedSize) { continue; } // TODO: make this support Variable length types as well.
            if (!string.Equals(candidate.TargetTypeFullyQualifiedName, BitStreamTypeNames.Int32, StringComparison.Ordinal)) { continue; }
            if (candidate.FixedSize is not int fixedSize || fixedSize <= 0) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.Write)) { continue; }
            if (!PrimitiveWrapperSourceEmitter.HasValidMethod(candidate, BitStreamPrimitiveRole.Peek)) { continue; }

            if (best is not PrimitiveDefinition current || IsPreferred(candidate, current)) { best = candidate; }
        }

        return best;
    }

    private static bool IsPreferred(in PrimitiveDefinition candidate, in PrimitiveDefinition current) {
        bool candidateIsDefaultAlias = string.Equals(candidate.Alias, DisplayNameUtility.DefaultInt32Alias, StringComparison.Ordinal);
        bool currentIsDefaultAlias = string.Equals(current.Alias, DisplayNameUtility.DefaultInt32Alias, StringComparison.Ordinal);
        if (candidateIsDefaultAlias != currentIsDefaultAlias) { return candidateIsDefaultAlias; }

        return string.CompareOrdinal(candidate.ExtensionClassFullyQualifiedName, current.ExtensionClassFullyQualifiedName) < 0;
    }
}
