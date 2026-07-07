using System.Collections.Generic;
using ComputerysBitStream.Attributes;

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

        _writer.WriteLine($"public static class {_alias}ReadContextExtensions {{");
        _writer.Indent++;

        List<string> methods = [];
        if (hasPeek) {
            methods.Add(EmitTryPeekAlias());
            methods.Add(EmitPeekAlias());
        }
        if (hasRead) {
            methods.Add(EmitTryReadAlias());
            methods.Add(EmitReadAlias());
        }
        if (hasPeekArray) {
            if (_hasIntPeek) {
                methods.Add(EmitTryPeekAliassOut());
                methods.Add(EmitPeekAliassOut());
                methods.Add(EmitTryPeekAliassOutWithMaxCount());
                methods.Add(EmitPeekAliassOutWithMaxCount());
            }
            methods.Add(EmitTryPeekAliassWithCount());
            methods.Add(EmitPeekAliassWithCount());
        }
        if (hasReadArray) {
            if (_hasIntPeek) {
                methods.Add(EmitTryReadAliassOut());
                methods.Add(EmitReadAliassOut());
                methods.Add(EmitTryReadAliassOutWithMaxCount());
                methods.Add(EmitReadAliassOutWithMaxCount());
            }
            methods.Add(EmitTryReadAliassWithCount());
            methods.Add(EmitReadAliassWithCount());
        }
        if (hasPeekSpan) {
            if (_hasIntPeek) {
                methods.Add(EmitTryPeekAliassIntoSpan());
                methods.Add(EmitPeekAliassIntoSpan());
                methods.Add(EmitTryPeekAliassIntoSpanWithMaxCount());
                methods.Add(EmitPeekAliassIntoSpanWithMaxCount());
            }
            methods.Add(EmitTryPeekAliassIntoSpanWithCount());
            methods.Add(EmitPeekAliassIntoSpanWithCount());
        }
        if (hasReadSpan) {
            if (_hasIntPeek) {
                methods.Add(EmitTryReadAliassIntoSpan());
                methods.Add(EmitReadAliassIntoSpan());
                methods.Add(EmitTryReadAliassIntoSpanWithMaxCount());
                methods.Add(EmitReadAliassIntoSpanWithMaxCount());
            }
            methods.Add(EmitTryReadAliassIntoSpanWithCount());
            methods.Add(EmitReadAliassIntoSpanWithCount());
        }
        _writer.WriteBlocks(methods);

        _writer.Indent--;
        _writer.WriteLine("}");
        _writer.WriteLine();
    }

    private string EmitTryPeekAlias() {
        string body = TryPeekAliasBody();

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}(this ref ReadContext context{{_extraParams}}, out {{_targetType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private string EmitPeekAlias() {
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

    private string EmitTryReadAlias() {
        string body = TryReadAliasBody();

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}(this ref ReadContext context{{_extraParams}}, out {{_targetType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private string EmitReadAlias() {
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

    private string EmitTryPeekAliassOut() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}s(this ref ReadContext context{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) {
                         values = {{empty}};
                         return false;
                     }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(PeekAliassOutBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitPeekAliassOut() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Peek{{_alias}}s(this ref ReadContext context{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekArrayCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryReadAliassOut() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}s(this ref ReadContext context{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) {
                         values = {{empty}};
                         return false;
                     }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(ReadAliassOutBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitReadAliassOut() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Read{{_alias}}s(this ref ReadContext context{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadArrayCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekAliassWithCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}s(this ref ReadContext context, int count{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}count < 0) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(PeekAliassWithCountBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitPeekAliassWithCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Peek{{_alias}}s(this ref ReadContext context, int count{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekArrayWithCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryReadAliassWithCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}s(this ref ReadContext context, int count{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}count < 0) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(ReadAliassWithCountBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitReadAliassWithCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Read{{_alias}}s(this ref ReadContext context, int count{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadArrayWithCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekAliassIntoSpan() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}s(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) { return false; }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0 || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(PeekAliassIntoSpanBody(), 1)}}
                 }
                 """;
    }

    private string EmitPeekAliassIntoSpan() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Peek{{_alias}}s(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekSpanCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryReadAliassIntoSpan() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}s(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) { return false; }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0 || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(ReadAliassIntoSpanBody(), 1)}}
                 }
                 """;
    }

    private string EmitReadAliassIntoSpan() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Read{{_alias}}s(this ref ReadContext context, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadSpanCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekAliassIntoSpanWithCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}s(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}count < 0 || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(PeekAliassIntoSpanWithCountBody(), 1)}}
                 }
                 """;
    }

    private string EmitPeekAliassIntoSpanWithCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Peek{{_alias}}s(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekSpanWithCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryReadAliassIntoSpanWithCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}s(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}count < 0 || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(ReadAliassIntoSpanWithCountBody(), 1)}}
                 }
                 """;
    }

    private string EmitReadAliassIntoSpanWithCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesIntoSpanWithCount}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Read{{_alias}}s(this ref ReadContext context, int count, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadSpanWithCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekAliassOutWithMaxCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}s(this ref ReadContext context, uint maxCount{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) {
                         values = {{empty}};
                         return false;
                     }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0 || count > maxCount) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(PeekAliassOutBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitPeekAliassOutWithMaxCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Peek{{_alias}}s(this ref ReadContext context, uint maxCount{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekArrayWithMaxCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryReadAliassOutWithMaxCount() {
        string empty = $"Array.Empty<{_targetType}>()";

        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}s(this ref ReadContext context, uint maxCount{{_extraParams}}, out {{_targetType}}[] values) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) {
                         values = {{empty}};
                         return false;
                     }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0 || count > maxCount) {
                         values = {{empty}};
                         return false;
                     }

                     {{SourceWriter.MaintainRelativeIndent(ReadAliassOutBody(empty), 1)}}
                 }
                 """;
    }

    private string EmitReadAliassOutWithMaxCount() {
        string typeName = $"{_alias} array";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{_targetType}}[] Read{{_alias}}s(this ref ReadContext context, uint maxCount{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadArrayWithMaxCountCall(), "return values;"), 1)}}
                 }
                 """;
    }

    private string EmitTryPeekAliassIntoSpanWithMaxCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryPeekValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryPeek{{_alias}}s(this ref ReadContext context, uint maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) { return false; }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0 || count > maxCount || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(PeekAliassIntoSpanBody(), 1)}}
                 }
                 """;
    }

    private string EmitPeekAliassIntoSpanWithMaxCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.PeekValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Peek{{_alias}}s(this ref ReadContext context, uint maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryPeekSpanWithMaxCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string EmitTryReadAliassIntoSpanWithMaxCount() {
        return $$"""
                 {{GeneratedDocumentationSyntax.TryReadValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryRead{{_alias}}s(this ref ReadContext context, uint maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     if ({{QuantizedFailPrefix}}context.IsInsufficientSpace({{_intSize}})) { return false; }

                     int count = {{_intExtensionClass}}.{{_intPeekMethodName}}(ref context);
                     if (count < 0 || count > maxCount || count > destination.Length) { return false; }

                     {{SourceWriter.MaintainRelativeIndent(ReadAliassIntoSpanBody(), 1)}}
                 }
                 """;
    }

    private string EmitReadAliassIntoSpanWithMaxCount() {
        string typeName = $"{_alias} span";
        return $$"""
                 {{GeneratedDocumentationSyntax.ReadValuesIntoSpanWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Read{{_alias}}s(this ref ReadContext context, uint maxCount, Span<{{_targetType}}> destination{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(EmitThrowIfTryReadFailedBody(typeName, TryReadSpanWithMaxCountCall(), string.Empty), 1)}}
                 }
                 """;
    }

    private string TryPeekAliasBody() {
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

    private string TryReadAliasBody() {
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

    private string PeekAliassOutBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     context.Position += {{_intSize}};
                     {{TryReadArrayLoop(empty)}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: true, ArrayReadFailStatement(empty))}}
                 context.Position += {{_intSize}};
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekArray)}}(ref context, count{{_extraArgs}});
                 context.Position -= {{_intSize}};

                 return true;
                 """;
    }

    private string ReadAliassOutBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     context.Position += {{_intSize}};
                     {{TryReadArrayLoop(empty)}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: true, ArrayReadFailStatement(empty))}}
                 context.Position += {{_intSize}};
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadArray)}}(ref context, count{{_extraArgs}});
                 return true;
                 """;
    }

    private string PeekAliassWithCountBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadArrayLoop(empty)}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: false, ArrayReadFailStatement(empty))}}
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekArray)}}(ref context, count{{_extraArgs}});
                 return true;
                 """;
    }

    private string ReadAliassWithCountBody(string empty) {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadArrayLoop(empty)}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: false, ArrayReadFailStatement(empty))}}
                 values = {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadArray)}}(ref context, count{{_extraArgs}});
                 return true;
                 """;
    }

    private string PeekAliassIntoSpanBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     context.Position += {{_intSize}};
                     {{TryReadSpanLoop()}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: true, "return false;")}}
                 context.Position += {{_intSize}};
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekSpan)}}(ref context, count, destination{{_extraArgs}});
                 context.Position -= {{_intSize}};

                 return true;
                 """;
    }

    private string ReadAliassIntoSpanBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     context.Position += {{_intSize}};
                     {{TryReadSpanLoop()}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: true, "return false;")}}
                 context.Position += {{_intSize}};
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.ReadSpan)}}(ref context, count, destination{{_extraArgs}});
                 return true;
                 """;
    }

    private string PeekAliassIntoSpanWithCountBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadSpanLoop()}}
                     context.Position = startPosition;
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: false, "return false;")}}
                 {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.PeekSpan)}}(ref context, count, destination{{_extraArgs}});
                 return true;
                 """;
    }

    private string ReadAliassIntoSpanWithCountBody() {
        if (_mode == PrimitiveSerializationMode.VariableLength) {
            return $$"""
                     long startPosition = context.Position;
                     {{TryReadSpanLoop()}}
                     return true;
                     """;
        }

        return $$"""
                 {{ReadBitsNeededGuard("count", includeIntSize: false, "return false;")}}
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

    private string ReadBitsNeededGuard(string countExpression, bool includeIntSize, string failStatement) {
        if (_mode == PrimitiveSerializationMode.VariableLength) { return ""; }

        string bitsExpression = includeIntSize
            ? $"{PerElementBits(countExpression)} + {_intSize}"
            : PerElementBits(countExpression);

        return $$"""
                 long bitsNeeded = {{bitsExpression}};
                 if (context.GetRemainingCapacity() < bitsNeeded) {
                     {{SourceWriter.MaintainRelativeIndent(failStatement, 1)}}
                 }
                 """;
    }
}
