using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct StructPrimitiveSourceEmitter {
    private void EmitReadMethods() {
        bool hasPeek = Has(BitStreamPrimitiveRole.Peek);
        bool hasRead = Has(BitStreamPrimitiveRole.Read);
        bool hasPeekArray = Has(BitStreamPrimitiveRole.PeekArray);
        bool hasReadArray = Has(BitStreamPrimitiveRole.ReadArray);
        bool hasPeekSpan = Has(BitStreamPrimitiveRole.PeekSpan);
        bool hasReadSpan = Has(BitStreamPrimitiveRole.ReadSpan);
        if (!hasPeek && !hasRead && !hasPeekArray && !hasReadArray && !hasPeekSpan && !hasReadSpan) { return; }

        List<string> methods = [];
        if (hasPeek) { methods.Add(EmitPeek()); }
        if (hasRead) { methods.Add(EmitRead()); }
        if (hasPeekArray) { methods.Add(EmitPeekArray()); }
        if (hasReadArray) { methods.Add(EmitReadArray()); }
        if (hasPeekSpan) { methods.Add(EmitPeekSpan()); }
        if (hasReadSpan) { methods.Add(EmitReadSpan()); }
        _writer.WriteBlocks(methods);
    }

    private string EmitPeek() {
        string body = IsFixedSize ? BuildFixedPeekBody() : BuildVariablePositionScopedBody(restorePosition: true);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}} {{Method(BitStreamPrimitiveRole.Peek)}}(this ref ReadContext context) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private string EmitRead() {
        string body = IsFixedSize ? BuildFixedReadBody() : BuildVariablePositionScopedBody(restorePosition: false);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}} {{Method(BitStreamPrimitiveRole.Read)}}(this ref ReadContext context) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private string EmitPeekArray() {
        string peekSpanMethod = Method(BitStreamPrimitiveRole.PeekSpan);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] {{Method(BitStreamPrimitiveRole.PeekArray)}}(this ref ReadContext context, int count) {
                     {{_targetType}}[] result = new {{_targetType}}[count];
                     {{peekSpanMethod}}(ref context, count, result);
                     return result;
                 }
                 """;
    }

    private string EmitReadArray() {
        string readSpanMethod = Method(BitStreamPrimitiveRole.ReadSpan);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] {{Method(BitStreamPrimitiveRole.ReadArray)}}(this ref ReadContext context, int count) {
                     {{_targetType}}[] result = new {{_targetType}}[count];
                     {{readSpanMethod}}(ref context, count, result);
                     return result;
                 }
                 """;
    }

    private string EmitPeekSpan() {
        string readSpanMethod = Method(BitStreamPrimitiveRole.ReadSpan);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void {{Method(BitStreamPrimitiveRole.PeekSpan)}}(this ref ReadContext context, int count, Span<{{_targetType}}> destination) {
                     long originalPosition = context.Position;
                     {{readSpanMethod}}(ref context, count, destination);
                     context.Position = originalPosition;
                 }
                 """;
    }

    private string EmitReadSpan() {
        string readMethod = Method(BitStreamPrimitiveRole.Read);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void {{Method(BitStreamPrimitiveRole.ReadSpan)}}(this ref ReadContext context, int count, Span<{{_targetType}}> destination) {
                     Span<{{_targetType}}> destinationSlice = destination.Slice(0, count);
                     for (int i = 0; i < count; i++) {
                         destinationSlice[i] = {{readMethod}}(ref context);
                     }
                 }
                 """;
    }

    private string BuildFixedPeekBody() {
        return $$"""
                 {{GeneratedSourceSyntax.EmitThrowInsufficientReadSpace(_alias, FixedSize.ToString())}}
                 long originalPosition = context.Position;
                 {{_targetType}} result = new {{_targetType}} {
                 {{BuildObjectInitializer(useTempVariables: false)}}
                 };
                 context.Position = originalPosition;
                 return result;
                 """;
    }

    private string BuildFixedReadBody() {
        return $$"""
                 {{GeneratedSourceSyntax.EmitThrowInsufficientReadSpace(_alias, FixedSize.ToString())}}
                 return new {{_targetType}} {
                 {{BuildObjectInitializer(useTempVariables: false)}}
                 };
                 """;
    }

    private string BuildVariablePositionScopedBody(bool restorePosition) {
        string failureStatement = $"context.Position = originalPosition; {GeneratedSourceSyntax.EmitThrowReadFailed(_alias)}";
        string tryReads = string.Join("\n", BuildMemberTryReadLines(failureStatement));
        string construct = BuildObjectInitializer(useTempVariables: true);

        if (restorePosition) {
            return $$"""
                     long originalPosition = context.Position;
                     {{tryReads}}
                     {{_targetType}} result = new {{_targetType}} {
                     {{construct}}
                     };
                     context.Position = originalPosition;
                     return result;
                     """;
        }

        return $$"""
                 long originalPosition = context.Position;
                 {{tryReads}}
                 return new {{_targetType}} {
                 {{construct}}
                 };
                 """;
    }

    private List<string> BuildMemberTryReadLines(string failureReturn) {
        List<string> lines = [];
        ImmutableArray<ResolvedStructMember> memberArray = _members;
        for (int i = 0; i < memberArray.Length; i++) {
            AddMemberTryReadLines(lines, memberArray[i], i, failureReturn);
        }
        return lines;
    }

    private static void AddMemberTryReadLines(List<string> lines, ResolvedStructMember member, int memberIndex, string failureReturn) {
        string tempName = $"temp{memberIndex}";
        string tempDecl = $"{GeneratedSourceSyntax.GetShortTypeName(member.TypeFullyQualifiedName)} {tempName}";

        switch (member.TryRead.Kind) {
            case MemberTryReadKind.TryReadOut:
                lines.Add($"if (!{member.TryRead.TryReadCall}(ref context, out {tempDecl})) {{ context.Position = originalPosition; {failureReturn} }}");
                break;
            case MemberTryReadKind.PreflightThenRead:
                lines.Add($"if (context.IsInsufficientSpace({member.TryRead.FixedBits})) {{ context.Position = originalPosition; {failureReturn} }}");
                lines.Add($"{tempDecl} = {member.ReadExpression};");
                break;
        }
    }

    private string BuildObjectInitializer(bool useTempVariables) {
        List<string> lines = [];
        ImmutableArray<ResolvedStructMember> memberArray = _members;
        for (int i = 0; i < memberArray.Length; i++) {
            ResolvedStructMember member = memberArray[i];
            string valueExpression = useTempVariables ? $"temp{i}" : member.ReadExpression;
            lines.Add($"    {member.MemberName} = {valueExpression}");
        }
        return string.Join(",\n", lines);
    }
}
