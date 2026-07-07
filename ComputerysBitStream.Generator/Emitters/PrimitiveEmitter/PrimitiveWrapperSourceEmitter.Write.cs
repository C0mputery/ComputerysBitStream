using System.Collections.Generic;
using ComputerysBitStream.Attributes;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct PrimitiveWrapperSourceEmitter {
    private void EmitWriteContextClass() {
        bool hasWrite = Has(BitStreamPrimitiveRole.Write);
        bool hasWriteSpan = Has(BitStreamPrimitiveRole.WriteSpan);
        if (!hasWrite && !hasWriteSpan) { return; }

        _writer.WriteLine($"public static class {_alias}WriteContextExtensions {{");
        _writer.Indent++;

        List<string> methods = [];
        if (hasWrite) { methods.Add(EmitWriteValue()); }
        if (hasWriteSpan) {
            if (_hasIntWrite) { methods.Add(EmitWriteValuesWithLength()); }
            methods.Add(EmitWriteValuesWithoutLength());
        }
        _writer.WriteBlocks(methods);

        _writer.Indent--;
        _writer.WriteLine("}");
        _writer.WriteLine();
    }

    private string EmitWriteValue() {
        string guard = _mode switch {
            PrimitiveSerializationMode.Quantized => $"""
                                                     {BitCountValidationThrow}
                                                     context.ThrowIfInsufficientSpace("{_alias}", bitCount);
                                                     """,
            PrimitiveSerializationMode.VariableLength => $"""context.ThrowIfInsufficientSpace("{_alias}", {_extensionClass}.{Method(BitStreamPrimitiveRole.Size)}(value));""",
            _ => $"""context.ThrowIfInsufficientSpace("{_alias}", {FixedSize});"""
        };

        return $$"""
                 {{GeneratedDocumentationSyntax.WriteValue}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Write{{_alias}}(this ref WriteContext context, {{_targetType}} value{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(guard, 1)}}

                     {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Write)}}(ref context, value{{_extraArgs}});
                 }
                 """;
    }

    private string EmitWriteValuesWithLength() {
        string guard = SpanWriteGuard(includeLengthPrefix: true, operation: $"{_alias} array");

        return $$"""
                 {{GeneratedDocumentationSyntax.WriteValuesWithLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Write{{_pluralAlias}}(this ref WriteContext context, ReadOnlySpan<{{_targetType}}> values{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(guard, 1)}}

                     {{_intExtensionClass}}.{{_intWriteMethodName}}(ref context, values.Length);
                     {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.WriteSpan)}}(ref context, values{{_extraArgs}});
                 }
                 """;
    }

    private string EmitWriteValuesWithoutLength() {
        string guard = SpanWriteGuard(includeLengthPrefix: false, operation: $"{_alias} span");

        return $$"""
                 {{GeneratedDocumentationSyntax.WriteValuesWithoutLength}}
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static void Write{{_pluralAlias}}WithoutLength(this ref WriteContext context, ReadOnlySpan<{{_targetType}}> values{{_extraParams}}) {
                     {{SourceWriter.MaintainRelativeIndent(guard, 1)}}

                     {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.WriteSpan)}}(ref context, values{{_extraArgs}});
                 }
                 """;
    }

    private string SpanWriteGuard(bool includeLengthPrefix, string operation) {
        string prefixBits = includeLengthPrefix ? _intSize.ToString() : "0";
        string bitsNeededDeclaration = _mode switch {
            PrimitiveSerializationMode.VariableLength => $$"""
                                                           long bitsNeeded = {{prefixBits}};
                                                           for (int i = 0; i < values.Length; i++) { bitsNeeded += {{_extensionClass}}.{{Method(BitStreamPrimitiveRole.Size)}}(values[i]); }
                                                           """,
            _ => includeLengthPrefix
                ? $"long bitsNeeded = {PerElementBits("values.Length")} + {_intSize};"
                : $"long bitsNeeded = {PerElementBits("values.Length")};"
        };

        return $$"""
                 {{QuantizedBitCountValidationPrefix()}}{{bitsNeededDeclaration}}
                 long availableBits = context.GetRemainingCapacity();
                 if (availableBits < bitsNeeded) { throw new InsufficientWriteCapacityException("{{operation}}", bitsNeeded > int.MaxValue ? int.MaxValue : (int)bitsNeeded, availableBits, context.Position); }
                 """;
    }
}
