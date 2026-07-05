using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct StructPrimitiveSourceEmitter {
    private void EmitVariableLengthMethods() {
        if (IsFixedSize) { return; }

        _writer.WriteLine();
        List<string> methods = [];
        if (Has(BitStreamPrimitiveRole.TryRead)) { methods.Add(EmitTryRead()); }
        if (Has(BitStreamPrimitiveRole.Size)) { methods.Add(EmitSize()); }
        _writer.WriteBlocks(methods);
    }

    private string EmitTryRead() {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool {{Method(BitStreamPrimitiveRole.TryRead)}}(this ref ReadContext context, out {{_targetType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(BuildTryReadBody(), 1)}}
                 }
                 """;
    }

    private string EmitSize() {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static int {{Method(BitStreamPrimitiveRole.Size)}}({{_targetType}} value) {
                     return {{BuildSizeBody()}};
                 }
                 """;
    }

    private string BuildTryReadBody() {
        string tryReads = string.Join("\n", BuildMemberTryReadLines("value = default; return false;"));

        return $$"""
                 long originalPosition = context.Position;
                 {{tryReads}}
                 value = new {{_targetType}} {
                 {{BuildObjectInitializer(useTempVariables: true)}}
                 };
                 return true;
                 """;
    }

    private string BuildSizeBody() {
        ImmutableArray<ResolvedStructMember> memberArray = _members;
        if (memberArray.Length == 0) { return "0"; }

        List<string> parts = [];
        foreach (ResolvedStructMember member in memberArray) { parts.Add(member.SizeExpression); }
        return string.Join(" + ", parts);
    }
}
