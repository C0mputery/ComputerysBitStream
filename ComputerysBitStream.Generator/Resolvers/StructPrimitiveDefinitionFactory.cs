using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.EquatableCollections;
using ComputerysBitStream.Generator.Roslyn;

namespace ComputerysBitStream.Generator;

internal static class StructPrimitiveDefinitionFactory {
    internal static PrimitiveDefinition Create(in ResolvedStructDefinition resolved) {
        StructDefinition source = resolved.Source;
        return new PrimitiveDefinition(
            ExtensionClassFullyQualifiedName: resolved.PrimitiveExtensionClassFqn,
            TargetTypeFullyQualifiedName: source.TypeFullyQualifiedName,
            TargetTypeNamespace: GetTypeNamespace(source.TypeFullyQualifiedName),
            TargetTypeEmitName: GetEmitTypeName(source.TypeFullyQualifiedName),
            Alias: source.Alias,
            Namespace: source.Namespace,
            Mode: resolved.Mode,
            FixedSize: resolved.FixedSize,
            MinBits: null,
            MaxBits: null,
            Methods: resolved.Methods,
            Settings: source.Settings,
            Location: source.Location
        );
    }

    internal static EquatableImmutableDictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> CreateMethodDefinitions(string alias, PrimitiveSerializationMode mode) {
        Dictionary<BitStreamPrimitiveRole, PrimitiveMethodDefinition> methods = new() {
            [BitStreamPrimitiveRole.Write] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.Write), true),
            [BitStreamPrimitiveRole.WriteSpan] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.WriteSpan), true),
            [BitStreamPrimitiveRole.Peek] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.Peek), true),
            [BitStreamPrimitiveRole.Read] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.Read), true),
            [BitStreamPrimitiveRole.PeekArray] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.PeekArray), true),
            [BitStreamPrimitiveRole.ReadArray] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.ReadArray), true),
            [BitStreamPrimitiveRole.PeekSpan] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.PeekSpan), true),
            [BitStreamPrimitiveRole.ReadSpan] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.ReadSpan), true),
        };

        if (mode == PrimitiveSerializationMode.VariableLength) {
            methods[BitStreamPrimitiveRole.TryRead] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.TryRead), true);
            methods[BitStreamPrimitiveRole.Size] = new PrimitiveMethodDefinition(GetMethodName(alias, BitStreamPrimitiveRole.Size), true);
        }

        return methods.ToImmutableDictionary();
    }

    internal static string GetMethodName(string alias, BitStreamPrimitiveRole role) {
        return role switch {
            BitStreamPrimitiveRole.Write => $"Write{alias}StructPrimitive",
            BitStreamPrimitiveRole.WriteSpan => $"Write{alias}sStructPrimitive",
            BitStreamPrimitiveRole.Peek => $"Peek{alias}StructPrimitive",
            BitStreamPrimitiveRole.Read => $"Read{alias}StructPrimitive",
            BitStreamPrimitiveRole.TryRead => $"TryRead{alias}StructPrimitive",
            BitStreamPrimitiveRole.PeekArray => $"Peek{alias}StructArrayPrimitive",
            BitStreamPrimitiveRole.ReadArray => $"Read{alias}StructArrayPrimitive",
            BitStreamPrimitiveRole.PeekSpan => $"Peek{alias}StructSpanPrimitive",
            BitStreamPrimitiveRole.ReadSpan => $"Read{alias}StructSpanPrimitive",
            BitStreamPrimitiveRole.Size => $"Get{alias}StructSize",
            _ => string.Empty,
        };
    }

    private static string? GetTypeNamespace(string typeFullyQualifiedName) {
        int lastDot = typeFullyQualifiedName.LastIndexOf('.');
        if (lastDot < 0) { return null; }
        return typeFullyQualifiedName.Substring(0, lastDot);
    }

    internal static string GetEmitTypeName(string typeFullyQualifiedName) {
        int lastDot = typeFullyQualifiedName.LastIndexOf('.');
        return lastDot < 0 ? typeFullyQualifiedName : typeFullyQualifiedName.Substring(lastDot + 1);
    }
}
