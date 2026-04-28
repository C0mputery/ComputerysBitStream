using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using ComputerysBitStream.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream;

internal static class StructWrapperSourceEmitter {
    private static readonly string GeneratedNamespace = nameof(ComputerysBitStream);

    internal static SourceText EmitSource(ParsedStructData structData, ParsedRawData? intHandler) {
        bool hasIntHandlerWrite = false;
        bool hasIntHandlerPeek = false;
        if (intHandler != null) {
            hasIntHandlerWrite = intHandler.Value.Methods.ContainsKey(BitStreamRawRole.Write);
            hasIntHandlerPeek = intHandler.Value.Methods.ContainsKey(BitStreamRawRole.Peek);
        }

        string accessibility = structData.Accessibility == Accessibility.Public ? "public" : "internal";
        string alias = structData.Alias;
        string typeFullyQualifiedName = structData.TypeFullyQualifiedName;
        bool isFixedSize = structData.IsFixedSize;
        int fixedSize = structData.FixedSize;
        ImmutableArray<ResolvedStructMember> members = structData.Members;

        using StringWriter stringWriter = new StringWriter();
        using IndentedTextWriter writer = new IndentedTextWriter(stringWriter, new string(' ', 4));

        writer.WriteLines($$"""
        using System;
        using System.Runtime.CompilerServices;

        namespace {{GeneratedNamespace}} {
        """);

        writer.Indent++;

        writer.WriteLine($"{accessibility} static class {alias}WriteContextExtensions {{");
        writer.Indent++;
        EmitWriteSingle(writer, alias, typeFullyQualifiedName, members, isFixedSize, fixedSize);
        if (hasIntHandlerWrite) {
            EmitWriteSpan(writer, alias, typeFullyQualifiedName, intHandler!.Value, isFixedSize, fixedSize);
        }
        EmitWriteSpanWithoutLength(writer, alias, typeFullyQualifiedName, isFixedSize, fixedSize);
        writer.Indent--;
        writer.WriteLine("}");

        writer.WriteLine($"{accessibility} static class {alias}ReadContextExtensions {{");
        writer.Indent++;
        EmitPeekSingle(writer, alias, typeFullyQualifiedName, members, isFixedSize, fixedSize);
        EmitReadSingle(writer, alias, typeFullyQualifiedName, members, isFixedSize, fixedSize);
        if (hasIntHandlerPeek) {
            EmitPeekArray(writer, alias, typeFullyQualifiedName, intHandler!.Value, isFixedSize, fixedSize);
            EmitReadArray(writer, alias, typeFullyQualifiedName, intHandler!.Value, isFixedSize, fixedSize);
            EmitPeekSpan(writer, alias, typeFullyQualifiedName, intHandler!.Value, isFixedSize, fixedSize);
            EmitReadSpan(writer, alias, typeFullyQualifiedName, intHandler!.Value, isFixedSize, fixedSize);
        }
        EmitPeekArrayWithoutLength(writer, alias, typeFullyQualifiedName, isFixedSize, fixedSize);
        EmitReadArrayWithoutLength(writer, alias, typeFullyQualifiedName, isFixedSize, fixedSize);
        EmitPeekSpanWithoutLength(writer, alias, typeFullyQualifiedName, isFixedSize, fixedSize);
        EmitReadSpanWithoutLength(writer, alias, typeFullyQualifiedName, isFixedSize, fixedSize);
        writer.Indent--;
        writer.WriteLine("}");

        writer.WriteLine($"{accessibility} static class {alias}SizeExtensions {{");
        writer.Indent++;
        EmitSizeMethod(writer, alias, typeFullyQualifiedName, members, isFixedSize, fixedSize);
        writer.Indent--;
        writer.WriteLine("}");

        writer.Indent--;
        writer.WriteLine("}");

        return SourceText.From(stringWriter.ToString(), Encoding.UTF8);
    }

    private static void EmitMethod(IndentedTextWriter writer, string docs, string signature, string body) {
        writer.WriteLines(docs);
        writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        writer.WriteLine(signature);
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLines(body);
        writer.Indent--;
        writer.WriteLine("}");
    }

    private readonly record struct DocParameter(string Name, string Description);

    private static string Doc(string summary, string? returns = null, string? remarks = null, params DocParameter[] parameters) {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine($"/// <summary>{summary}</summary>");
        foreach (DocParameter parameter in parameters) {
            stringBuilder.AppendLine($"/// <param name=\"{parameter.Name}\">{parameter.Description}</param>");
        }
        if (returns != null) stringBuilder.AppendLine($"/// <returns>{returns}</returns>");
        if (remarks != null) stringBuilder.AppendLine(remarks);
        return stringBuilder.ToString().TrimEnd();
    }

    // 1. int Get{Type}SizeInBits({Type} value)
    // 2. bool Is{Type}FixedSizeStruct({Type} value)
    private static void EmitSizeMethod(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ImmutableArray<ResolvedStructMember> members, bool isFixedSize, int fixedSize) {
        string body = isFixedSize
            ? $"return {fixedSize};"
            : "return " + string.Join(" + ", members.Select(member => $"value.{member.MemberName}.Get{member.Alias}SizeInBits()")) + ";";

        writer.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
        EmitMethod(writer,
            Doc(
                $"Gets the size in bits of a <see cref=\"{typeFullyQualifiedName}\"/> value.",
                returns: "The size in bits.",
                parameters: [new DocParameter("value", "The value to measure.")]
            ),
            $"public static int Get{alias}SizeInBits(this {typeFullyQualifiedName} value)",
            body
        );

        writer.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
        EmitMethod(writer,
            Doc(
                $"Gets a value indicating whether <see cref=\"{typeFullyQualifiedName}\"/> has a fixed size in bits.",
                returns: "<see langword=\"true\"/> if the type has a fixed size; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("value", "The value to check.")]
            ),
            $"public static bool Is{alias}FixedSizeStruct(this {typeFullyQualifiedName} value)",
            $"return {isFixedSize.ToString().ToLowerInvariant()};"
        );
    }

    // 1. void Write{Type}({Type} value)
    // 2. void Write({Type} value)
    private static void EmitWriteSingle(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ImmutableArray<ResolvedStructMember> members, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize ? $"context.ThrowIfNoSpace(\"{alias}\", {fixedSize});" : "";
        string memberWrites = string.Join("\n", members.Select(member => $"context.Write{member.Alias}(value.{member.MemberName});"));
        string body = string.IsNullOrEmpty(spaceCheck) ? memberWrites : $"{spaceCheck}\n{memberWrites}";

        EmitMethod(writer,
            Doc(
                $"Writes a <see cref=\"{typeFullyQualifiedName}\"/> value to the bit stream.",
                parameters: [new DocParameter("context", "The write context."), new DocParameter("value", "The value to write.")]
            ),
            $"public static void Write{alias}(this ref WriteContext context, {typeFullyQualifiedName} value)",
            body
        );

        EmitMethod(writer,
            Doc(
                $"Writes a <see cref=\"{typeFullyQualifiedName}\"/> value to the bit stream.",
                parameters: [new DocParameter("context", "The write context."), new DocParameter("value", "The value to write.")]
            ),
            $"public static void Write(this ref WriteContext context, {typeFullyQualifiedName} value)",
            $"context.Write{alias}(value);"
        );
    }

    // 1. void Write{Type}s(ReadOnlySpan<{Type}> values)
    // 2. void Write(ReadOnlySpan<{Type}> values)
    private static void EmitWriteSpan(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ParsedRawData intHandler, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = values.Length * {fixedSize} + {intHandler.Size};\ncontext.ThrowIfNoSpace(\"{alias} array\", bitsNeeded);"
            : $"int bitsNeeded = {intHandler.Size};\nforeach ({typeFullyQualifiedName} value in values) {{ bitsNeeded += value.Get{alias}SizeInBits(); }}\ncontext.ThrowIfNoSpace(\"{alias} array\", bitsNeeded);";

        EmitMethod(writer,
            Doc(
                $"Writes a length-prefixed span of <see cref=\"{typeFullyQualifiedName}\"/> values to the bit stream.",
                parameters: [new DocParameter("context", "The write context."), new DocParameter("values", "The values to write.")]
            ),
            $"public static void Write{alias}s(this ref WriteContext context, ReadOnlySpan<{typeFullyQualifiedName}> values)",
            $"{spaceCheck}\ncontext.{intHandler.Methods[BitStreamRawRole.Write].MethodName}(values.Length);\nforeach ({typeFullyQualifiedName} value in values) {{ context.Write{alias}(value); }}"
        );

        EmitMethod(writer,
            Doc(
                $"Writes a length-prefixed span of <see cref=\"{typeFullyQualifiedName}\"/> values to the bit stream.",
                parameters: [new DocParameter("context", "The write context."), new DocParameter("values", "The values to write.")]
            ),
            $"public static void Write(this ref WriteContext context, ReadOnlySpan<{typeFullyQualifiedName}> values)",
            $"context.Write{alias}s(values);"
        );
    }

    // 1. void Write{Type}sWithoutLength(ReadOnlySpan<{Type}> values)
    // 2. void WriteWithoutLength(ReadOnlySpan<{Type}> values)
    private static void EmitWriteSpanWithoutLength(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int totalSize = values.Length * {fixedSize};\ncontext.ThrowIfNoSpace(\"{alias} span\", totalSize);"
            : $"int totalSize = 0;\nforeach ({typeFullyQualifiedName} value in values) {{ totalSize += value.Get{alias}SizeInBits(); }}\ncontext.ThrowIfNoSpace(\"{alias} span\", totalSize);";

        EmitMethod(writer,
            Doc(
                $"Writes a span of <see cref=\"{typeFullyQualifiedName}\"/> values to the bit stream without a length prefix.",
                parameters: [new DocParameter("context", "The write context."), new DocParameter("values", "The values to write.")]
            ),
            $"public static void Write{alias}sWithoutLength(this ref WriteContext context, ReadOnlySpan<{typeFullyQualifiedName}> values)",
            $"{spaceCheck}\nforeach ({typeFullyQualifiedName} value in values) {{ context.Write{alias}(value); }}"
        );

        EmitMethod(writer,
            Doc(
                $"Writes a span of <see cref=\"{typeFullyQualifiedName}\"/> values to the bit stream without a length prefix.",
                parameters: [new DocParameter("context", "The write context."), new DocParameter("values", "The values to write.")]
            ),
            $"public static void WriteWithoutLength(this ref WriteContext context, ReadOnlySpan<{typeFullyQualifiedName}> values)",
            $"context.Write{alias}sWithoutLength(values);"
        );
    }

    // 1. {Type} Peek{Type}()
    // 2. void Peek(out {Type} value)
    // 3. bool TryPeek{Type}(out {Type} value)
    // 4. bool TryPeek(out {Type} value)
    private static void EmitPeekSingle(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ImmutableArray<ResolvedStructMember> members, bool isFixedSize, int fixedSize) {
            string memberReads = string.Join(",\n", members.Select(member => $"    {member.MemberName} = context.Read{member.Alias}()"));
        string readBody = $"int originalPosition = context.Position;\n{typeFullyQualifiedName} result = new {typeFullyQualifiedName} {{\n{memberReads}\n}};\ncontext.Position = originalPosition;\nreturn result;";
        string peekBody = isFixedSize
            ? $"if (context.IsInsufficientSpace({fixedSize})) {{ return default; }}\n{readBody}"
            : BuildVariableSizePeekBody(typeFullyQualifiedName, members);

        EmitMethod(writer,
            Doc(
                $"Peeks at a <see cref=\"{typeFullyQualifiedName}\"/> value at the current position without advancing the bit stream.",
                returns: "The value at the current position, or the default value if there is insufficient data.",
                parameters: [new DocParameter("context", "The read context.")]
            ),
            $"public static {typeFullyQualifiedName} Peek{alias}(this ref ReadContext context)",
            peekBody
        );

        EmitMethod(writer,
            Doc(
                $"Peeks at a <see cref=\"{typeFullyQualifiedName}\"/> value at the current position without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("value", "When this method returns, contains the value at the current position, or the default value if there is insufficient data.")]
            ),
            $"public static void Peek(this ref ReadContext context, out {typeFullyQualifiedName} value)",
            $"value = context.Peek{alias}();"
        );

        string tryPeekBody = isFixedSize
            ? $"if (context.IsInsufficientSpace({fixedSize})) {{ value = default; return false; }}\nvalue = context.Peek{alias}();\nreturn true;"
            : BuildVariableSizeTryPeekBody(typeFullyQualifiedName, members);

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a <see cref=\"{typeFullyQualifiedName}\"/> value at the current position without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the value could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("value", "When this method returns, contains the value at the current position if successful; otherwise, the default value.")]
            ),
            $"public static bool TryPeek{alias}(this ref ReadContext context, out {typeFullyQualifiedName} value)",
            tryPeekBody
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a <see cref=\"{typeFullyQualifiedName}\"/> value at the current position without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the value could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("value", "When this method returns, contains the value at the current position if successful; otherwise, the default value.")]
            ),
            $"public static bool TryPeek(this ref ReadContext context, out {typeFullyQualifiedName} value)",
            $"return context.TryPeek{alias}(out value);"
        );
    }

    private static string BuildVariableSizePeekBody(string typeFullyQualifiedName, ImmutableArray<ResolvedStructMember> members) {
        string tryReads = string.Join("\n", members.Select((member, index) => $"if (!context.TryRead{member.Alias}(out {member.TypeFullyQualifiedName} temporary{index})) {{ context.Position = originalPosition; return default; }}"));
        string construct = string.Join(",\n", members.Select((member, index) => $"    {member.MemberName} = temporary{index}"));
        return $"int originalPosition = context.Position;\n{tryReads}\n{typeFullyQualifiedName} result = new {typeFullyQualifiedName} {{\n{construct}\n}};\ncontext.Position = originalPosition;\nreturn result;";
    }

    private static string BuildVariableSizeTryPeekBody(string typeFullyQualifiedName, ImmutableArray<ResolvedStructMember> members) {
        string tryReads = string.Join("\n", members.Select((member, index) => $"if (!context.TryRead{member.Alias}(out {member.TypeFullyQualifiedName} temporary{index})) {{ context.Position = originalPosition; value = default; return false; }}"));
        string construct = string.Join(",\n", members.Select((member, index) => $"    {member.MemberName} = temporary{index}"));
        return $"int originalPosition = context.Position;\n{tryReads}\ncontext.Position = originalPosition;\nvalue = new {typeFullyQualifiedName} {{\n{construct}\n}};\nreturn true;";
    }

    // 1. {Type} Read{Type}()
    // 2. void Read(out {Type} value)
    // 3. bool TryRead{Type}(out {Type} value)
    // 4. bool TryRead(out {Type} value)
    private static void EmitReadSingle(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ImmutableArray<ResolvedStructMember> members, bool isFixedSize, int fixedSize) {
        string readBody;
        string tryReadBody;

        if (isFixedSize) {
        string memberReads = string.Join(",\n", members.Select(member => $"    {member.MemberName} = context.Read{member.Alias}()"));
            readBody = $"if (context.IsInsufficientSpace({fixedSize})) {{ return default; }}\nreturn new {typeFullyQualifiedName} {{\n{memberReads}\n}};";
            tryReadBody = $"if (context.IsInsufficientSpace({fixedSize})) {{ value = default; return false; }}\nvalue = context.Read{alias}();\nreturn true;";
        } else {
            string tryReads = string.Join("\n", members.Select((member, index) => $"if (!context.TryRead{member.Alias}(out {member.TypeFullyQualifiedName} temporary{index})) {{ context.Position = originalPosition; return default; }}"));
            string construct = string.Join(",\n", members.Select((member, index) => $"    {member.MemberName} = temporary{index}"));
            readBody = $"int originalPosition = context.Position;\n{tryReads}\nreturn new {typeFullyQualifiedName} {{\n{construct}\n}};";

            string tryTryReads = string.Join("\n", members.Select((member, index) => $"if (!context.TryRead{member.Alias}(out {member.TypeFullyQualifiedName} temporary{index})) {{ context.Position = originalPosition; value = default; return false; }}"));
            tryReadBody = $"int originalPosition = context.Position;\n{tryTryReads}\nvalue = new {typeFullyQualifiedName} {{\n{construct}\n}};\nreturn true;";
        }

        EmitMethod(writer,
            Doc(
                $"Reads a <see cref=\"{typeFullyQualifiedName}\"/> value from the current position and advances the bit stream.",
                returns: "The value at the current position, or the default value if there is insufficient data.",
                parameters: [new DocParameter("context", "The read context.")]
            ),
            $"public static {typeFullyQualifiedName} Read{alias}(this ref ReadContext context)",
            readBody
        );

        EmitMethod(writer,
            Doc(
                $"Reads a <see cref=\"{typeFullyQualifiedName}\"/> value from the current position and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("value", "When this method returns, contains the value at the current position, or the default value if there is insufficient data.")]
            ),
            $"public static void Read(this ref ReadContext context, out {typeFullyQualifiedName} value)",
            $"value = context.Read{alias}();"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a <see cref=\"{typeFullyQualifiedName}\"/> value from the current position and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the value could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("value", "When this method returns, contains the value at the current position if successful; otherwise, the default value.")]
            ),
            $"public static bool TryRead{alias}(this ref ReadContext context, out {typeFullyQualifiedName} value)",
            tryReadBody
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a <see cref=\"{typeFullyQualifiedName}\"/> value from the current position and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the value could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("value", "When this method returns, contains the value at the current position if successful; otherwise, the default value.")]
            ),
            $"public static bool TryRead(this ref ReadContext context, out {typeFullyQualifiedName} value)",
            $"return context.TryRead{alias}(out value);"
        );
    }

    // 1. {Type}[] Peek{Type}s()
    // 2. void Peek(out {Type}[] values)
    // 3. bool TryPeek{Type}s(out {Type}[] values)
    // 4. bool TryPeek(out {Type}[] values)
    private static void EmitPeekArray(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ParsedRawData intHandler, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoop(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Peeks at a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values at the current position without advancing the bit stream.",
                returns: "An array of values, or an empty array if there is insufficient data or the encoded length is invalid.",
                parameters: [new DocParameter("context", "The read context.")]
            ),
            $"public static {typeFullyQualifiedName}[] Peek{alias}s(this ref ReadContext context)",
            isFixedSize
                ? $"if (context.IsInsufficientSpace({intHandler.Size})) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (count < 0) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\n{spaceCheck}\nint originalPosition = context.Position;\ncontext.Position += {intHandler.Size};\n{typeFullyQualifiedName}[] values = new {typeFullyQualifiedName}[count];\nfor (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}\ncontext.Position = originalPosition;\nreturn values;"
                : $"if (!context.TryPeek{alias}s(out {typeFullyQualifiedName}[] values)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\nreturn values;"
        );

        EmitMethod(writer,
            Doc(
                $"Peeks at a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values at the current position without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("values", "When this method returns, contains the values at the current position, or an empty array if there is insufficient data or the encoded length is invalid.")]
            ),
            $"public static void Peek(this ref ReadContext context, out {typeFullyQualifiedName}[] values)",
            $"values = context.Peek{alias}s();"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values at the current position without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryPeek{alias}s(this ref ReadContext context, out {typeFullyQualifiedName}[] values)",
            $"if (context.IsInsufficientSpace({intHandler.Size})) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (count < 0) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\ncontext.Position += {intHandler.Size};\nvalues = new {typeFullyQualifiedName}[count];\n{tryLoop}\ncontext.Position = originalPosition;\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values at the current position without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryPeek(this ref ReadContext context, out {typeFullyQualifiedName}[] values)",
            $"return context.TryPeek{alias}s(out values);"
        );
    }

    // 1. {Type}[] Read{Type}s()
    // 2. void Read(out {Type}[] values)
    // 3. bool TryRead{Type}s(out {Type}[] values)
    // 4. bool TryRead(out {Type}[] values)
    private static void EmitReadArray(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ParsedRawData intHandler, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}" : "";
        string trySpaceCheck = isFixedSize ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}" : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoop(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Reads a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values from the current position and advances the bit stream.",
                returns: "An array of values, or an empty array if there is insufficient data or the encoded length is invalid.",
                parameters: [new DocParameter("context", "The read context.")]
            ),
            $"public static {typeFullyQualifiedName}[] Read{alias}s(this ref ReadContext context)",
            isFixedSize
                ? $"if (context.IsInsufficientSpace({intHandler.Size})) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (count < 0) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\n{spaceCheck}\ncontext.Position += {intHandler.Size};\n{typeFullyQualifiedName}[] values = new {typeFullyQualifiedName}[count];\nfor (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}\nreturn values;"
                : $"if (!context.TryRead{alias}s(out {typeFullyQualifiedName}[] values)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\nreturn values;"
        );

        EmitMethod(writer,
            Doc(
                $"Reads a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values from the current position and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("values", "When this method returns, contains the values at the current position, or an empty array if there is insufficient data or the encoded length is invalid.")]
            ),
            $"public static void Read(this ref ReadContext context, out {typeFullyQualifiedName}[] values)",
            $"values = context.Read{alias}s();"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values from the current position and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryRead{alias}s(this ref ReadContext context, out {typeFullyQualifiedName}[] values)",
            $"if (context.IsInsufficientSpace({intHandler.Size})) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (count < 0) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\ncontext.Position += {intHandler.Size};\nvalues = new {typeFullyQualifiedName}[count];\n{tryLoop}\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a length-prefixed array of <see cref=\"{typeFullyQualifiedName}\"/> values from the current position and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryRead(this ref ReadContext context, out {typeFullyQualifiedName}[] values)",
            $"return context.TryRead{alias}s(out values);"
        );
    }

    // 1. {Type}[] Peek{Type}s(int count)
    // 2. void Peek(int count, out {Type}[] values)
    // 3. bool TryPeek{Type}s(int count, out {Type}[] values)
    // 4. bool TryPeek(int count, out {Type}[] values)
    private static void EmitPeekArrayWithoutLength(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoop(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Peeks at an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length at the current position without advancing the bit stream.",
                returns: "An array of values, or an empty array if there is insufficient data or <paramref name=\"count\"/> is invalid.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek.")]
            ),
            $"public static {typeFullyQualifiedName}[] Peek{alias}s(this ref ReadContext context, int count)",
            isFixedSize
                ? $"if (count < 0) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\n{spaceCheck}\nint originalPosition = context.Position;\n{typeFullyQualifiedName}[] values = new {typeFullyQualifiedName}[count];\nfor (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}\ncontext.Position = originalPosition;\nreturn values;"
                : $"if (!context.TryPeek{alias}s(count, out {typeFullyQualifiedName}[] values)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\nreturn values;"
        );

        EmitMethod(writer,
            Doc(
                $"Peeks at an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length at the current position without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("values", "When this method returns, contains the values at the current position, or an empty array if there is insufficient data or <paramref name=\"count\"/> is invalid.")]
            ),
            $"public static void Peek(this ref ReadContext context, int count, out {typeFullyQualifiedName}[] values)",
            $"values = context.Peek{alias}s(count);"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length at the current position without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryPeek{alias}s(this ref ReadContext context, int count, out {typeFullyQualifiedName}[] values)",
            $"if (count < 0) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\nvalues = new {typeFullyQualifiedName}[count];\n{tryLoop}\ncontext.Position = originalPosition;\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length at the current position without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryPeek(this ref ReadContext context, int count, out {typeFullyQualifiedName}[] values)",
            $"return context.TryPeek{alias}s(count, out values);"
        );
    }

    // 1. {Type}[] Read{Type}s(int count)
    // 2. void Read(int count, out {Type}[] values)
    // 3. bool TryRead{Type}s(int count, out {Type}[] values)
    // 4. bool TryRead(int count, out {Type}[] values)
    private static void EmitReadArrayWithoutLength(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoop(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Reads an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length from the current position and advances the bit stream.",
                returns: "An array of values, or an empty array if there is insufficient data or <paramref name=\"count\"/> is invalid.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read.")]
            ),
            $"public static {typeFullyQualifiedName}[] Read{alias}s(this ref ReadContext context, int count)",
            isFixedSize
                ? $"if (count < 0) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\n{spaceCheck}\n{typeFullyQualifiedName}[] values = new {typeFullyQualifiedName}[count];\nfor (int i = 0; i < count; i++) {{ values[i] = context.Read{alias}(); }}\nreturn values;"
                : $"if (!context.TryRead{alias}s(count, out {typeFullyQualifiedName}[] values)) {{ return Array.Empty<{typeFullyQualifiedName}>(); }}\nreturn values;"
        );

        EmitMethod(writer,
            Doc(
                $"Reads an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length from the current position and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("values", "When this method returns, contains the values at the current position, or an empty array if there is insufficient data or <paramref name=\"count\"/> is invalid.")]
            ),
            $"public static void Read(this ref ReadContext context, int count, out {typeFullyQualifiedName}[] values)",
            $"values = context.Read{alias}s(count);"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length from the current position and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryRead{alias}s(this ref ReadContext context, int count, out {typeFullyQualifiedName}[] values)",
            $"if (count < 0) {{ values = Array.Empty<{typeFullyQualifiedName}>(); return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\nvalues = new {typeFullyQualifiedName}[count];\n{tryLoop}\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read an array of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length from the current position and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("values", "When this method returns, contains the values at the current position if successful; otherwise, an empty array.")]
            ),
            $"public static bool TryRead(this ref ReadContext context, int count, out {typeFullyQualifiedName}[] values)",
            $"return context.TryRead{alias}s(count, out values);"
        );
    }

    // 1. void Peek{Type}s(ref Span<{Type}> destination)
    // 2. void Peek(ref Span<{Type}> destination)
    // 3. bool TryPeek{Type}s(ref Span<{Type}> destination)
    // 4. bool TryPeek(ref Span<{Type}> destination)
    private static void EmitPeekSpan(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ParsedRawData intHandler, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return; }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoopSpan(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Peeks at a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Peek{alias}s(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            isFixedSize
                ? $"if (context.IsInsufficientSpace({intHandler.Size})) {{ return; }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (0 > count || count > destination.Length) {{ return; }}\n{spaceCheck}\nint originalPosition = context.Position;\ncontext.Position += {intHandler.Size};\nfor (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}\ncontext.Position = originalPosition;"
                : $"if (!context.TryPeek{alias}s(ref destination)) {{ return; }}"
        );

        EmitMethod(writer,
            Doc(
                $"Peeks at a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Peek(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            $"context.Peek{alias}s(ref destination);"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryPeek{alias}s(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            $"if (context.IsInsufficientSpace({intHandler.Size})) {{ return false; }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (0 > count || count > destination.Length) {{ return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\ncontext.Position += {intHandler.Size};\n{tryLoop}\ncontext.Position = originalPosition;\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryPeek(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            $"return context.TryPeek{alias}s(ref destination);"
        );
    }

    // 1. void Read{Type}s(ref Span<{Type}> destination)
    // 2. void Read(ref Span<{Type}> destination)
    // 3. bool TryRead{Type}s(ref Span<{Type}> destination)
    // 4. bool TryRead(ref Span<{Type}> destination)
    private static void EmitReadSpan(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, ParsedRawData intHandler, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return; }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize} + {intHandler.Size};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoopSpan(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Reads a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Read{alias}s(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            isFixedSize
                ? $"if (context.IsInsufficientSpace({intHandler.Size})) {{ return; }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (0 > count || count > destination.Length) {{ return; }}\n{spaceCheck}\ncontext.Position += {intHandler.Size};\nfor (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}"
                : $"if (!context.TryRead{alias}s(ref destination)) {{ return; }}"
        );

        EmitMethod(writer,
            Doc(
                $"Reads a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Read(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            $"context.Read{alias}s(ref destination);"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryRead{alias}s(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            $"if (context.IsInsufficientSpace({intHandler.Size})) {{ return false; }}\nint count = context.{intHandler.Methods[BitStreamRawRole.Peek].MethodName}();\nif (0 > count || count > destination.Length) {{ return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\ncontext.Position += {intHandler.Size};\n{tryLoop}\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a length-prefixed sequence of <see cref=\"{typeFullyQualifiedName}\"/> values into the specified destination span and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryRead(this ref ReadContext context, ref Span<{typeFullyQualifiedName}> destination)",
            $"return context.TryRead{alias}s(ref destination);"
        );
    }

    // 1. void Peek{Type}s(int count, ref Span<{Type}> destination)
    // 2. void Peek(int count, ref Span<{Type}> destination)
    // 3. bool TryPeek{Type}s(int count, ref Span<{Type}> destination)
    // 4. bool TryPeek(int count, ref Span<{Type}> destination)
    private static void EmitPeekSpanWithoutLength(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return; }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoopSpan(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Peeks at a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Peek{alias}s(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            isFixedSize
                ? $"if (0 > count || count > destination.Length) {{ return; }}\n{spaceCheck}\nint originalPosition = context.Position;\nfor (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}\ncontext.Position = originalPosition;"
                : $"if (!context.TryPeek{alias}s(count, ref destination)) {{ return; }}"
        );

        EmitMethod(writer,
            Doc(
                $"Peeks at a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span without advancing the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Peek(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            $"context.Peek{alias}s(count, ref destination);"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryPeek{alias}s(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            $"if (0 > count || count > destination.Length) {{ return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\n{tryLoop}\ncontext.Position = originalPosition;\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to peek at a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span without advancing the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to peek."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryPeek(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            $"return context.TryPeek{alias}s(count, ref destination);"
        );
    }

    // 1. void Read{Type}s(int count, ref Span<{Type}> destination)
    // 2. void Read(int count, ref Span<{Type}> destination)
    // 3. bool TryRead{Type}s(int count, ref Span<{Type}> destination)
    // 4. bool TryRead(int count, ref Span<{Type}> destination)
    private static void EmitReadSpanWithoutLength(IndentedTextWriter writer, string alias, string typeFullyQualifiedName, bool isFixedSize, int fixedSize) {
        string spaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return; }}"
            : "";

        string trySpaceCheck = isFixedSize
            ? $"int bitsNeeded = count * {fixedSize};\nif (context.IsInsufficientSpace(bitsNeeded)) {{ return false; }}"
            : "";
        string tryLoop = isFixedSize
            ? $"for (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}"
            : BuildVariableSizeTryLoopSpan(alias, typeFullyQualifiedName);

        EmitMethod(writer,
            Doc(
                $"Reads a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Read{alias}s(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            isFixedSize
                ? $"if (0 > count || count > destination.Length) {{ return; }}\n{spaceCheck}\nfor (int i = 0; i < count; i++) {{ destination[i] = context.Read{alias}(); }}"
                : $"if (!context.TryRead{alias}s(count, ref destination)) {{ return; }}"
        );

        EmitMethod(writer,
            Doc(
                $"Reads a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span and advances the bit stream.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static void Read(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            $"context.Read{alias}s(count, ref destination);"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryRead{alias}s(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            $"if (0 > count || count > destination.Length) {{ return false; }}\n{trySpaceCheck}\nint originalPosition = context.Position;\n{tryLoop}\nreturn true;"
        );

        EmitMethod(writer,
            Doc(
                $"Attempts to read a sequence of <see cref=\"{typeFullyQualifiedName}\"/> values of the specified length into the destination span and advance the bit stream.",
                returns: "<see langword=\"true\"/> if the values could be read; otherwise, <see langword=\"false\"/>.",
                parameters: [new DocParameter("context", "The read context."), new DocParameter("count", "The number of values to read."), new DocParameter("destination", "The span that receives the values.")]
            ),
            $"public static bool TryRead(this ref ReadContext context, int count, ref Span<{typeFullyQualifiedName}> destination)",
            $"return context.TryRead{alias}s(count, ref destination);"
        );
    }

    private static string BuildVariableSizeTryLoop(string alias, string typeFullyQualifiedName) {
        return $"for (int i = 0; i < count; i++) {{\n    if (!context.TryRead{alias}(out values[i])) {{\n        context.Position = originalPosition;\n        values = Array.Empty<{typeFullyQualifiedName}>();\n        return false;\n    }}\n}}";
    }

    private static string BuildVariableSizeTryLoopSpan(string alias, string typeFullyQualifiedName) {
        return $"for (int i = 0; i < count; i++) {{\n    if (!context.TryRead{alias}(out destination[i])) {{\n        context.Position = originalPosition;\n        return false;\n    }}\n}}";
    }
}
