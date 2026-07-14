using System.Collections.Generic;
using ComputerysBitStream.Attributes;
using ComputerysBitStream.Generator.Emission;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct PrimitiveWrapperSourceEmitter {
    private void EmitReadContextClass() {
        bool hasPeek = Has(BitStreamPrimitiveRole.Peek);
        bool hasRead = Has(BitStreamPrimitiveRole.Read);
        bool hasPeekArray = Has(BitStreamPrimitiveRole.PeekArray);
        bool hasReadArray = Has(BitStreamPrimitiveRole.ReadArray);
        bool hasPeekSpan = Has(BitStreamPrimitiveRole.PeekSpan);
        bool hasReadSpan = Has(BitStreamPrimitiveRole.ReadSpan);
        if (!hasPeek && !hasRead && !hasPeekArray && !hasReadArray && !hasPeekSpan && !hasReadSpan) { return; }

        if (_mode == PrimitiveSerializationMode.VariableLength && !Has(BitStreamPrimitiveRole.TryRead)) { return; }

        _writer.WriteLine(GeneratedDocumentationSyntax.ReadContextExtensionsClass);
        _writer.WriteLine($"public static class {_alias}ReadContextExtensions {{");
        _writer.Indent++;

        List<string> methods = [];
        if (hasPeek) {
            methods.Add(EmitTryPeekValue());
            methods.Add(EmitPeekValue());
        }
        if (hasRead) {
            methods.Add(EmitTryReadValue());
            methods.Add(EmitReadValue());
        }
        if (hasPeekArray) {
            if (_hasIntTryRead) {
                methods.Add(EmitTryPeekValuesWithLength());
                methods.Add(EmitPeekValuesWithLength());
                methods.Add(EmitTryPeekValuesWithMaxCount());
                methods.Add(EmitPeekValuesWithMaxCount());
            }
            methods.Add(EmitTryPeekValuesWithCount());
            methods.Add(EmitPeekValuesWithCount());
        }
        if (hasReadArray) {
            if (_hasIntTryRead) {
                methods.Add(EmitTryReadValuesWithLength());
                methods.Add(EmitReadValuesWithLength());
                methods.Add(EmitTryReadValuesWithMaxCount());
                methods.Add(EmitReadValuesWithMaxCount());
            }
            methods.Add(EmitTryReadValuesWithCount());
            methods.Add(EmitReadValuesWithCount());
        }
        if (hasPeekSpan) {
            if (_hasIntTryRead) {
                methods.Add(EmitTryPeekValuesIntoSpanWithLength());
                methods.Add(EmitPeekValuesIntoSpanWithLength());
                methods.Add(EmitTryPeekValuesIntoSpanWithMaxCount());
                methods.Add(EmitPeekValuesIntoSpanWithMaxCount());
            }
            methods.Add(EmitTryPeekValuesIntoSpanWithCount());
            methods.Add(EmitPeekValuesIntoSpanWithCount());
        }
        if (hasReadSpan) {
            if (_hasIntTryRead) {
                methods.Add(EmitTryReadValuesIntoSpanWithLength());
                methods.Add(EmitReadValuesIntoSpanWithLength());
                methods.Add(EmitTryReadValuesIntoSpanWithMaxCount());
                methods.Add(EmitReadValuesIntoSpanWithMaxCount());
            }
            methods.Add(EmitTryReadValuesIntoSpanWithCount());
            methods.Add(EmitReadValuesIntoSpanWithCount());
        }
        _writer.WriteBlocks(methods);

        _writer.Indent--;
        _writer.WriteLine("}");
        _writer.WriteLine();
    }

    private string EmitTryPeekValue() {
        string body = TryPeekValueBody();

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}(this ref ReadContext context{{_extraParams}}, out {{_targetType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private string EmitPeekValue() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     {{GeneratedDocumentationSyntax.PeekValue}}
                     [MethodImpl(MethodImplOptions.AggressiveInlining)]
                     public static {{_targetType}} Peek{{_alias}}(this ref ReadContext context{{_extraParams}}) {
                         {{SourceWriter.MaintainRelativeIndent(GeneratedSourceSyntax.EmitThrowIfTryReadFailed(_alias, TryPeekScalarCall(), "return value;"), 1)}}
                     }
                     """;
        }

        if (_mode == PrimitiveSerializationMode.Quantized) {
            return $$"""
                     {{GeneratedDocumentationSyntax.PeekValue}}
                     [MethodImpl(MethodImplOptions.AggressiveInlining)]
                     public static {{_targetType}} Peek{{_alias}}(this ref ReadContext context{{_extraParams}}) {
                         {{BitCountValidationThrow}}
                         {{GeneratedSourceSyntax.EmitThrowInsufficientReadSpace(_alias, "bitCount")}}
                         return {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Peek)}}(ref context{{_extraArgs}});
                     }
                     """;
        }

        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}} Peek{{_alias}}(this ref ReadContext context{{_extraParams}}) {
                     {{GeneratedSourceSyntax.EmitThrowInsufficientReadSpace(_alias, FixedSize.ToString())}}
                     return {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Peek)}}(ref context{{_extraArgs}});
                 }
                 """;
    }

    private string EmitTryReadValue() {
        string body = TryReadValueBody();

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}(this ref ReadContext context{{_extraParams}}, out {{_targetType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private string EmitReadValue() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     {{GeneratedDocumentationSyntax.ReadValue}}
                     [MethodImpl(MethodImplOptions.AggressiveInlining)]
                     public static {{_targetType}} Read{{_alias}}(this ref ReadContext context{{_extraParams}}) {
                         {{SourceWriter.MaintainRelativeIndent(GeneratedSourceSyntax.EmitThrowIfTryReadFailed(_alias, TryReadScalarCall(), "return value;"), 1)}}
                     }
                     """;
        }

        if (_mode == PrimitiveSerializationMode.Quantized) {
            return $$"""
                     {{GeneratedDocumentationSyntax.ReadValue}}
                     [MethodImpl(MethodImplOptions.AggressiveInlining)]
                     public static {{_targetType}} Read{{_alias}}(this ref ReadContext context{{_extraParams}}) {
                         {{BitCountValidationThrow}}
                         {{GeneratedSourceSyntax.EmitThrowInsufficientReadSpace(_alias, "bitCount")}}
                         return {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Read)}}(ref context{{_extraArgs}});
                     }
                     """;
        }

        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}} Read{{_alias}}(this ref ReadContext context{{_extraParams}}) {
                     {{GeneratedSourceSyntax.EmitThrowInsufficientReadSpace(_alias, FixedSize.ToString())}}
                     return {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Read)}}(ref context{{_extraArgs}});
                 }
                 """;
    }

    private string EmitTryPeekValuesWithLength() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_pluralAlias}}(this ref ReadContext context{{_extraParams}}, out {{_targetType}}[] values) {
                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead(ArrayReadFailStatement(empty)), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(PeekValuesWithLengthBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitPeekValuesWithLength() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Peek{{_pluralAlias}}(this ref ReadContext context{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekArrayCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryReadValuesWithLength() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_pluralAlias}}(this ref ReadContext context{{_extraParams}}, out {{_targetType}}[] values) {
                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead(ArrayReadFailStatement(empty)), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(ReadValuesWithLengthBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitReadValuesWithLength() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Read{{_pluralAlias}}(this ref ReadContext context{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadArrayCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekValuesWithCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_pluralAlias}}(this ref ReadContext context, int count{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}count < 0) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(PeekValuesWithCountBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitPeekValuesWithCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Peek{{_pluralAlias}}(this ref ReadContext context, int count{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekArrayWithCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryReadValuesWithCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_pluralAlias}}(this ref ReadContext context, int count{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}count < 0) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(ReadValuesWithCountBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitReadValuesWithCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Read{{_pluralAlias}}(this ref ReadContext context, int count{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadArrayWithCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekValuesIntoSpanWithLength() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_pluralAlias}}(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead("return false;", "count > destination.Length"), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(PeekValuesIntoSpanWithLengthBody(), 1)}}
                 }
                 """;
    }

    private string EmitPeekValuesIntoSpanWithLength() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Peek{{_pluralAlias}}(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekSpanCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryReadValuesIntoSpanWithLength() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_pluralAlias}}(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead("return false;", "count > destination.Length"), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(ReadValuesIntoSpanWithLengthBody(), 1)}}
                 }
                 """;
    }

    private string EmitReadValuesIntoSpanWithLength() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Read{{_pluralAlias}}(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadSpanCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekValuesIntoSpanWithCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_pluralAlias}}(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}count < 0 || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(PeekValuesIntoSpanWithCountBody(), 1)}}
                 }
                 """;
    }

    private string EmitPeekValuesIntoSpanWithCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Peek{{_pluralAlias}}(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekSpanWithCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryReadValuesIntoSpanWithCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_pluralAlias}}(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}count < 0 || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(ReadValuesIntoSpanWithCountBody(), 1)}}
                 }
                 """;
    }

    private string EmitReadValuesIntoSpanWithCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Read{{_pluralAlias}}(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadSpanWithCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekValuesWithMaxCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}maxCount < 0) {
                         values = {{empty}};
                         return false;
                     }

                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead(ArrayReadFailStatement(empty), "count > maxCount"), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(PeekValuesWithLengthBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitPeekValuesWithMaxCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Peek{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekArrayWithMaxCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryReadValuesWithMaxCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}maxCount < 0) {
                         values = {{empty}};
                         return false;
                     }

                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead(ArrayReadFailStatement(empty), "count > maxCount"), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(ReadValuesWithLengthBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitReadValuesWithMaxCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Read{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadArrayWithMaxCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekValuesIntoSpanWithMaxCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesIntoSpanWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}maxCount < 0) { return false; }

                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead("return false;", "count > maxCount || count > destination.Length"), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(PeekValuesIntoSpanWithLengthBody(), 1)}}
                 }
                 """;
    }

    private string EmitPeekValuesIntoSpanWithMaxCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesIntoSpanWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Peek{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekSpanWithMaxCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryReadValuesIntoSpanWithMaxCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesIntoSpanWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}maxCount < 0) { return false; }

                     long startPosition = context.Position;
                     {{SourceWriter.MaintainRelativeIndent(EmitLengthPrefixRead("return false;", "count > maxCount || count > destination.Length"), 1)}}
                     {{SourceWriter.MaintainRelativeIndent(ReadValuesIntoSpanWithLengthBody(), 1)}}
                 }
                 """;
    }

    private string EmitReadValuesIntoSpanWithMaxCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesIntoSpanWithMaxCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Read{{_pluralAlias}}WithMaxCount(this ref ReadContext context, int maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadSpanWithMaxCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string TryPeekValueBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     bool success = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.TryRead)}}(ref context, out value);
                     context.Position = startPosition;
                     return success;
                     """;
        }

        return $$"""
                 if ({{SingleReadFailCondition}}) {
                     value = default;
                     return false;
                 }

                 value = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Peek)}}(ref context{{_extraArgs}});
                 return true;
                 """;
    }

    private string TryReadValueBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     return {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.TryRead)}}(ref context, out value);
                     """;
        }

        return $$"""
                 if ({{SingleReadFailCondition}}) {
                     value = default;
                     return false;
                 }

                 value = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Read)}}(ref context{{_extraArgs}});
                 return true;
                 """;
    }

    private string EmitLengthPrefixRead(string failStatement, string? countConstraint = null) {
        string constraintBlock = string.IsNullOrEmpty(countConstraint)
            ? string.Empty
            : $$"""
                if ({{countConstraint}}) {
                    context.Position = startPosition;
                    {{SourceWriter.MaintainRelativeIndent(failStatement, 1)}}
                }
                """;

        return $$"""
                 if ({{QuantizedFailPrefix}}!{{_intExtensionClass}}.{{_intTryReadMethodName}}(ref context, out {{_intTargetType}} encodedCount) || encodedCount > int.MaxValue) {
                     context.Position = startPosition;
                     {{SourceWriter.MaintainRelativeIndent(failStatement, 1)}}
                 }

                 int count = (int)encodedCount;
                 {{constraintBlock}}
                 """;
    }

    private string PeekValuesWithLengthBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     {{TryReadArrayLoop(empty)}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", ArrayReadFailStatement(empty), restorePositionOnFail: true)}}
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekArray)}}(ref context, count{{_extraArgs}});
                 context.Position = startPosition;

                 return true;
                 """;
    }

    private string ReadValuesWithLengthBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     {{TryReadArrayLoop(empty)}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", ArrayReadFailStatement(empty), restorePositionOnFail: true)}}
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadArray)}}(ref context, count{{_extraArgs}});
                 return true;
                 """;
    }

    private string PeekValuesWithCountBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadArrayLoop(empty)}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", ArrayReadFailStatement(empty), restorePositionOnFail: false)}}
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekArray)}}(ref context, count{{_extraArgs}});
                 return true;
                 """;
    }

    private string ReadValuesWithCountBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadArrayLoop(empty)}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", ArrayReadFailStatement(empty), restorePositionOnFail: false)}}
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadArray)}}(ref context, count{{_extraArgs}});
                 return true;
                 """;
    }

    private string PeekValuesIntoSpanWithLengthBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     {{TryReadSpanLoop()}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", "return false;", restorePositionOnFail: true)}}
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekSpan)}}(ref context, count, destination{{_extraArgs}});
                 context.Position = startPosition;

                 return true;
                 """;
    }

    private string ReadValuesIntoSpanWithLengthBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     {{TryReadSpanLoop()}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", "return false;", restorePositionOnFail: true)}}
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadSpan)}}(ref context, count, destination{{_extraArgs}});
                 return true;
                 """;
    }

    private string PeekValuesIntoSpanWithCountBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadSpanLoop()}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", "return false;", restorePositionOnFail: false)}}
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekSpan)}}(ref context, count, destination{{_extraArgs}});
                 return true;
                 """;
    }

    private string ReadValuesIntoSpanWithCountBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadSpanLoop()}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", "return false;", restorePositionOnFail: false)}}
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadSpan)}}(ref context, count, destination{{_extraArgs}});
                 return true;
                 """;
    }

    private string TryReadArrayLoop(string empty) {
        return $$"""
                 values = count == 0 ? {{empty}} : new {{_targetType}}[count];
                 for (int i = 0; i < count; i++) {
                     if (!{{_extensionClass}}.{{Method(BitStreamPrimitiveRole.TryRead)}}(ref context, out {{_targetType}} item)) {
                         context.Position = startPosition;
                         values = {{empty}};
                         return false;
                     }

                     values[i] = item;
                 }
                 """;
    }

    private string TryReadSpanLoop() {
        return $$"""
                 Span<{{_targetType}}> destinationSlice = destination.Slice(0, count);
                 for (int i = 0; i < count; i++) {
                     if (!{{_extensionClass}}.{{Method(BitStreamPrimitiveRole.TryRead)}}(ref context, out {{_targetType}} item)) {
                         context.Position = startPosition;
                         destinationSlice.Clear();
                         return false;
                     }

                     destinationSlice[i] = item;
                 }
                 """;
    }

    private static string ArrayReadFailStatement(string emptyArrayExpression) {
        return $$"""
                 values = {{emptyArrayExpression}};
                 return false;
                 """;
    }

    private string ReadBitsNeededGuard(string countExpression, string failStatement, bool restorePositionOnFail) {
        if (_mode == PrimitiveSerializationMode.VariableLength) { return ""; }

        string restore = restorePositionOnFail ? "context.Position = startPosition;\n" : "";
        return $$"""
                 long bitsNeeded = {{PerElementBits(countExpression)}};
                 if (context.GetRemainingCapacity() < bitsNeeded) {
                     {{restore}}{{SourceWriter.MaintainRelativeIndent(failStatement, 1)}}
                 }
                 """;
    }
}
