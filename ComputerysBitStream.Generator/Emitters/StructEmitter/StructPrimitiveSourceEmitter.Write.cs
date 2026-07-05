using System.Collections.Generic;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct StructPrimitiveSourceEmitter {
    private void EmitWriteMethods(SourceWriter writer) {
        bool hasWrite = Has(BitStreamPrimitiveRole.Write);
        bool hasWriteSpan = Has(BitStreamPrimitiveRole.WriteSpan);
        if (!hasWrite && !hasWriteSpan) { return; }

        List<string> methods = [];
        if (hasWrite) { methods.Add(EmitWrite()); }
        if (hasWriteSpan) { methods.Add(EmitWriteSpan()); }
        writer.WriteBlocks(methods);

        if (Has(BitStreamPrimitiveRole.Peek)) { writer.WriteLine(); }
    }

    private string EmitWrite() {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void {{Method(BitStreamPrimitiveRole.Write)}}(this ref WriteContext context, {{_targetType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(BuildWriteBody(), 1)}}
                 }
                 """;
    }

    private string EmitWriteSpan() {
        string writeMethod = Method(BitStreamPrimitiveRole.Write);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void {{Method(BitStreamPrimitiveRole.WriteSpan)}}(this ref WriteContext context, ReadOnlySpan<{{_targetType}}> values) {
                     for (int i = 0; i < values.Length; i++) {
                         {{writeMethod}}(ref context, values[i]);
                     }
                 }
                 """;
    }

    private string BuildWriteBody() {
        List<string> lines = [];
        foreach (ResolvedStructMember member in _members) {
            lines.Add($"{member.WriteCall};");
        }
        return string.Join("\n", lines);
    }
}
