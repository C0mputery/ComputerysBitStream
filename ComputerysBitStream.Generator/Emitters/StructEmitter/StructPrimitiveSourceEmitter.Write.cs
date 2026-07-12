using System.Collections.Generic;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Emission;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct StructPrimitiveSourceEmitter {
    private void EmitWriteMethods() {
        bool hasWrite = Has(BitStreamPrimitiveRole.Write);
        bool hasWriteSpan = Has(BitStreamPrimitiveRole.WriteSpan);
        if (!hasWrite && !hasWriteSpan) { return; }

        List<string> methods = [];
        if (hasWrite) { methods.Add(EmitWrite()); }
        if (hasWriteSpan) { methods.Add(EmitWriteSpan()); }
        _writer.WriteBlocks(methods);

        if (Has(BitStreamPrimitiveRole.Peek)) { _writer.WriteLine(); }
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
        System.Collections.Immutable.ImmutableArray<ResolvedStructMember> members = _members;
        for (int i = 0; i < members.Length; i++) {
            ResolvedStructMember member = members[i];
            lines.Add(member.Kind == ResolvedStructMemberKind.Collection
                ? $"WriteCollection{i}Level0(ref context, value.{member.MemberName});"
                : $"{member.WriteCall};");
        }
        return string.Join("\n", lines);
    }
}
