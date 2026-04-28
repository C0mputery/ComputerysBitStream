using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using ComputerysBitStream.Generator;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream;

internal static class StructMetadataSourceEmitter {
    internal static SourceText EmitSource(ParsedStructData structData, bool isProxyClass, string declarationTypeFullyQualifiedName) {
        bool isFixedSize = structData.IsFixedSize;
        int fixedSize = structData.FixedSize;

        (string? namespaceName, string typeName) = ParseFullyQualifiedName(declarationTypeFullyQualifiedName);

        string typeKind = isProxyClass ? "static partial class" : "partial struct";
        string isFixedSizeLower = isFixedSize.ToString().ToLowerInvariant();

        string arguments = $"{isFixedSizeLower}, {fixedSize}";

        using StringWriter stringWriter = new StringWriter();
        using IndentedTextWriter writer = new IndentedTextWriter(stringWriter, new string(' ', 4));

        if (!string.IsNullOrEmpty(namespaceName)) {
            writer.WriteLines($$"""
            namespace {{namespaceName}} {
                [global::ComputerysBitStream.BitStreamStructMetadata({{arguments}})]
                {{typeKind}} {{typeName}} { }
            }
            """);
        } else {
            writer.WriteLines($$"""
            [global::ComputerysBitStream.BitStreamStructMetadata({{arguments}})]
            {{typeKind}} {{typeName}} { }
            """);
        }

        return SourceText.From(stringWriter.ToString(), Encoding.UTF8);
    }

    private static (string? Namespace, string TypeName) ParseFullyQualifiedName(string fullyQualifiedName) {
        string name = fullyQualifiedName.StartsWith("global::") ? fullyQualifiedName.Substring(8) : fullyQualifiedName;
        int lastDot = name.LastIndexOf('.');
        if (lastDot < 0) {
            return (null, name);
        }
        return (name.Substring(0, lastDot), name.Substring(lastDot + 1));
    }
}
