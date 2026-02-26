using System.Collections.Generic;
using System.Collections.Immutable;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;


namespace ComputerysBitStream.Generator {
    [Generator]
    public class BitStreamGenerator : IIncrementalGenerator {
        private static readonly DiagnosticDescriptor DuplicateTypeRule = new DiagnosticDescriptor(
            id: "BS001",
            title: "Duplicate BitStreamTypeAttribute",
            messageFormat: "The type '{0}' is already handled by another BitStreamTypeAttribute",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DuplicateRawRoleRule = new DiagnosticDescriptor(
            id: "BS002",
            title: "Duplicate BitStreamRawAttribute role",
            messageFormat: "The role '{0}' is specified more than once in '{1}' (first: '{2}', again: '{3}')",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
        
        private static readonly string ClassAttribute = typeof(BitStreamTypeAttribute).FullName!;
        private static readonly string MemberAttribute = typeof(BitStreamRawAttribute).FullName!;

        public void Initialize(IncrementalGeneratorInitializationContext context) {
            IncrementalValuesProvider<BitStreamTypeInfo?> pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
                    fullyQualifiedMetadataName: ClassAttribute,
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: Transform)
                .Where(info => info is not null);

            IncrementalValueProvider<ImmutableArray<BitStreamTypeInfo>> collected = pipeline.Collect()!;
            context.RegisterSourceOutput(collected, RegisterSourceOutputAction);
        }

        private BitStreamTypeInfo? Transform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
            if (context.TargetSymbol is not INamedTypeSymbol classSymbol) { return null; }

            AttributeData? classAttributeData = context.Attributes.FirstOrDefault(); 
            if (classAttributeData == null || classAttributeData.ConstructorArguments.Length == 0) { return null; }

            TypedConstant typeArgument = classAttributeData.ConstructorArguments[0];
            if (typeArgument.Value is not INamedTypeSymbol targetTypeSymbol) { return null; }
            
            TypedConstant sizeArgument = classAttributeData.ConstructorArguments[1];
            if (sizeArgument.Value is not int size) { return null; }
            
            List<IMethodSymbol> members = classSymbol.GetMembers().OfType<IMethodSymbol>().ToList();
            
            Dictionary<BitStreamRawRole, string> methodsByRole = new Dictionary<BitStreamRawRole, string>();
            List<DuplicateRawRoleInfo> duplicates = [];
            foreach (IMethodSymbol? member in members) {
                AttributeData? attribute = member.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == MemberAttribute);
                if (attribute?.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int roleValue) {
                    BitStreamRawRole role = (BitStreamRawRole)roleValue;
                    if (methodsByRole.TryGetValue(role, out string? firstMethod)) {
                        duplicates.Add(new DuplicateRawRoleInfo(
                            Role: role.ToString(),
                            ClassName: classSymbol.Name,
                            FirstMethod: firstMethod,
                            SecondMethod: member.Name,
                            Location: attribute.ApplicationSyntaxReference?.GetSyntax(cancel).GetLocation()
                        ));
                    } else {
                        methodsByRole[role] = member.Name;
                    }
                }
            }
            
            return new BitStreamTypeInfo(
                ClassNamespace: classSymbol.ContainingNamespace.ToDisplayString(),
                ClassName: classSymbol.Name,
                TargetTypeFullName: targetTypeSymbol.ToDisplayString(),
                TargetTypeName: GetTargetTypeName(targetTypeSymbol),
                Size: size,
                WriteRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.Write, out string? writeRaw) ? writeRaw : null, 
                WriteSpanRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.WriteSpan, out string? writeSpanRaw) ? writeSpanRaw : null, 
                PeekRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.Peek, out string? peekRaw) ? peekRaw : null,
                ReadRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.Read, out string? readRaw) ? readRaw : null,
                PeekArrayRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.PeekArray, out string? peekArrayRaw) ? peekArrayRaw : null,
                ReadArrayRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.ReadArray, out string? readArrayRaw) ? readArrayRaw : null,
                PeekSpanRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.PeekSpan, out string? peekSpanRaw) ? peekSpanRaw : null,
                ReadSpanRawMethodName: methodsByRole.TryGetValue(BitStreamRawRole.ReadSpan, out string? readSpanRaw) ? readSpanRaw : null,
                Location: classAttributeData.ApplicationSyntaxReference?.GetSyntax(cancel).GetLocation(),
                DuplicateRoles: duplicates.ToImmutableArray()
                );
        }
        
        private static string GetTargetTypeName(ITypeSymbol symbol) {
            return symbol.SpecialType switch {
                SpecialType.System_Boolean => "Bool",
                SpecialType.System_Byte => "Byte",
                SpecialType.System_SByte => "SByte",
                SpecialType.System_Int16 => "Short",
                SpecialType.System_UInt16 => "UShort",
                SpecialType.System_Int32 => "Int",
                SpecialType.System_UInt32 => "UInt",
                SpecialType.System_Int64 => "Long",
                SpecialType.System_UInt64 => "ULong",
                SpecialType.System_Single => "Float",
                SpecialType.System_Double => "Double",
                SpecialType.System_String => "String",
                _ => symbol.Name
            };
        }

        private static void RegisterSourceOutputAction(SourceProductionContext context, ImmutableArray<BitStreamTypeInfo> handlers) {
            Dictionary<string, BitStreamTypeInfo> handlersByTarget = new();
            foreach (BitStreamTypeInfo handler in handlers) {
                foreach (DuplicateRawRoleInfo duplicate in handler.DuplicateRoles) {
                    Location? location = null;
                    if (duplicate.Location.HasValue) {
                        location = Location.Create(duplicate.Location.Value.FilePath, duplicate.Location.Value.TextSpan, duplicate.Location.Value.LineSpan);
                    }
                    context.ReportDiagnostic(Diagnostic.Create(DuplicateRawRoleRule, location, duplicate.Role, duplicate.ClassName, duplicate.FirstMethod, duplicate.SecondMethod));
                }

                if (handlersByTarget.ContainsKey(handler.TargetTypeFullName)) {
                    Location? location = null;
                    BitStreamLocation? bitStreamLocation = handler.Location;
                    if (bitStreamLocation.HasValue) {   
                        location = Location.Create(bitStreamLocation.Value.FilePath, bitStreamLocation.Value.TextSpan, bitStreamLocation.Value.LineSpan);
                    }
                    
                    Diagnostic diagnostic = Diagnostic.Create(DuplicateTypeRule, location, handler.TargetTypeFullName);
                    context.ReportDiagnostic(diagnostic);
                } else {
                    handlersByTarget[handler.TargetTypeFullName] = handler;
                }
            }
            GenerateSource(context, handlersByTarget);
        }

        private static void GenerateSource(SourceProductionContext context, Dictionary<string, BitStreamTypeInfo> handlersByTarget) {
            handlersByTarget.TryGetValue(SyntaxFacts.GetText(SyntaxKind.IntKeyword), out BitStreamTypeInfo? intHandler);
            
            foreach (BitStreamTypeInfo handler in handlersByTarget.Values) {
                string source = CreateExtensions(handler, intHandler);
                context.AddSource($"{handler.TargetTypeName}ContextExtensions.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static string CreateExtensions(BitStreamTypeInfo type, BitStreamTypeInfo? intHandler) {
            using StringWriter stringWriter = new StringWriter();
            using IndentedTextWriter writer = new IndentedTextWriter(stringWriter, "    ");

            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.CompilerServices;");
            if (type.ClassNamespace != "ComputerysBitStream") {
                writer.WriteLine($"using {type.ClassNamespace};");
            }

            if (intHandler != null && intHandler.ClassNamespace != type.ClassNamespace && intHandler.ClassNamespace != "ComputerysBitStream") {
                writer.WriteLine($"using {intHandler.ClassNamespace};");
            }

            writer.WriteLine();
            writer.WriteLine("namespace ComputerysBitStream {");
            writer.Indent++;

            // Write methods
            writer.WriteLine($"public static class {type.TargetTypeName}WriteContextExtensions {{");
            writer.Indent++;

            if (type.WriteRawMethodName != null) {
                // void Write{Type}({Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Write{type.TargetTypeName}(this ref WriteContext context, {type.TargetTypeFullName} value) {{");
                writer.Indent++;
                writer.WriteLine($"context.ThrowIfNoSpace(\"{type.TargetTypeName}\", {type.Size});");
                writer.WriteLine($"context.{type.WriteRawMethodName}(value);");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();

                // void Write({Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Write(this ref WriteContext context, {type.TargetTypeFullName} value) => context.Write{type.TargetTypeName}(value);");
                writer.WriteLine();
            }

            if (type.WriteSpanRawMethodName != null) {
                if (intHandler != null) {
                    // void Write{Type}s(ReadOnlySpan<{Type}> values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Write{type.TargetTypeName}s(this ref WriteContext context, ReadOnlySpan<{type.TargetTypeFullName}> values) {{");
                    writer.Indent++;
                    writer.WriteLine($"int bitsNeeded = values.Length * {type.Size} + {intHandler.Size};");
                    writer.WriteLine($"context.ThrowIfNoSpace(\"{type.TargetTypeName} array\", bitsNeeded);");
                    writer.WriteLine($"context.{intHandler.WriteRawMethodName}(values.Length);");
                    writer.WriteLine($"context.{type.WriteSpanRawMethodName}(values);");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();

                    // void Write(ReadOnlySpan<{Type}> values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Write(this ref WriteContext context, ReadOnlySpan<{type.TargetTypeFullName}> values) => context.Write{type.TargetTypeName}s(values);");
                    writer.WriteLine();
                }

                // void Write{Type}sWithoutLength(ReadOnlySpan<{Type}> values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Write{type.TargetTypeName}sWithoutLength(this ref WriteContext context, ReadOnlySpan<{type.TargetTypeFullName}> values) {{");
                writer.Indent++;
                writer.WriteLine($"int bitsNeeded = values.Length * {type.Size};");
                writer.WriteLine($"context.ThrowIfNoSpace(\"{type.TargetTypeName} array without length\", bitsNeeded);");
                writer.WriteLine($"context.{type.WriteSpanRawMethodName}(values);");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();

                // void WriteWithoutLength(ReadOnlySpan<{Type}> values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void WriteWithoutLength(this ref WriteContext context, ReadOnlySpan<{type.TargetTypeFullName}> values) => context.Write{type.TargetTypeName}sWithoutLength(values);");
                writer.WriteLine();
            }

            writer.Indent--;
            writer.WriteLine("}");

            // Read extensions
            writer.WriteLine();
            writer.WriteLine($"public static class {type.TargetTypeName}ReadContextExtensions {{");
            writer.Indent++;

            if (type.PeekRawMethodName != null) {
                // {Type} Peek{Type}()
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static {type.TargetTypeFullName} Peek{type.TargetTypeName}(this ref ReadContext context) {{");
                writer.Indent++;
                writer.WriteLine($"if (context.IsInsufficientSpace({type.Size})) {{ return default; }}");
                writer.WriteLine($"return context.{type.PeekRawMethodName}();");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // void Peek(out {Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Peek(this ref ReadContext context, out {type.TargetTypeFullName} value) => value = context.Peek{type.TargetTypeName}();");
                writer.WriteLine();
                
                // bool TryPeek{Type}(out {Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryPeek{type.TargetTypeName}(this ref ReadContext context, out {type.TargetTypeFullName} value) {{");
                writer.Indent++;
                writer.WriteLine($"if (context.IsInsufficientSpace({type.Size})) {{");
                writer.Indent++;
                writer.WriteLine("value = default;");
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($"value = context.{type.PeekRawMethodName}();");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // bool TryPeek(out {Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryPeek(this ref ReadContext context, out {type.TargetTypeFullName} value) => context.TryPeek{type.TargetTypeName}(out value);");
                writer.WriteLine();
            }

            if (type.ReadRawMethodName != null) {
                // {Type} Read{Type}()
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static {type.TargetTypeFullName} Read{type.TargetTypeName}(this ref ReadContext context) {{");
                writer.Indent++;
                writer.WriteLine($"if (context.IsInsufficientSpace({type.Size})) {{ return default; }}");
                writer.WriteLine($"return context.{type.ReadRawMethodName}();");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // void Read(out {Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Read(this ref ReadContext context, out {type.TargetTypeFullName} value) => value = context.Read{type.TargetTypeName}();");
                writer.WriteLine();
                
                // bool TryRead{Type}(out {Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryRead{type.TargetTypeName}(this ref ReadContext context, out {type.TargetTypeFullName} value) {{");
                writer.Indent++;
                writer.WriteLine($"if (context.IsInsufficientSpace({type.Size})) {{");
                writer.Indent++;
                writer.WriteLine("value = default;");
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($"value = context.{type.ReadRawMethodName}();");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // bool TryRead(out {Type} value)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryRead(this ref ReadContext context, out {type.TargetTypeFullName} value) => context.TryRead{type.TargetTypeName}(out value);");
                writer.WriteLine();
            }
            
            if (type.PeekArrayRawMethodName != null) {
                if (intHandler != null) {
                    // {Type}[] Peek{Type}s()
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static {type.TargetTypeFullName}[] Peek{type.TargetTypeName}s(this ref ReadContext context) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine($"if (count < 0) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}"); // TODO: add some configurable max count.
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"{type.TargetTypeFullName}[] values = context.{type.PeekArrayRawMethodName}(count);");
                    writer.WriteLine($"context.Position -= {intHandler.Size};");
                    writer.WriteLine("return values;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // void Peek(out {Type}[] values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Peek(this ref ReadContext context, out {type.TargetTypeFullName}[] values) => values = context.Peek{type.TargetTypeName}s();");
                    writer.WriteLine();

                    // bool TryPeek{Type}s(out {Type}[] values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryPeek{type.TargetTypeName}s(this ref ReadContext context, out {type.TargetTypeFullName}[] values) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{");
                    writer.Indent++;
                    writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine("if (count < 0) {"); // TODO: add some configurable max count.
                    writer.Indent++;
                    writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine("if (context.IsInsufficientSpace(bitsNeeded)) {");
                    writer.Indent++;
                    writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"values = context.{type.PeekArrayRawMethodName}(count);");
                    writer.WriteLine($"context.Position -= {intHandler.Size};");
                    writer.WriteLine("return true;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // bool TryPeek(out {Type}[] values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryPeek(this ref ReadContext context, out {type.TargetTypeFullName}[] values) => context.TryPeek{type.TargetTypeName}s(out values);");
                    writer.WriteLine();
                }

                // {Type}[] Peek{Type}s(int count)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static {type.TargetTypeFullName}[] Peek{type.TargetTypeName}s(this ref ReadContext context, int count) {{");
                writer.Indent++;
                writer.WriteLine($"if (count < 0) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}"); // TODO: add some configurable max count.
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}");
                writer.WriteLine($"{type.TargetTypeFullName}[] values = context.{type.PeekArrayRawMethodName}(count);");
                writer.WriteLine("return values;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // void Peek(int count, out {Type}[] values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Peek(this ref ReadContext context, int count, out {type.TargetTypeFullName}[] values) => values = context.Peek{type.TargetTypeName}s(count);");
                writer.WriteLine();
                
                // bool TryPeek{Type}s(int count, out {Type}[] values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryPeek{type.TargetTypeName}s(this ref ReadContext context, int count, out {type.TargetTypeFullName}[] values) {{");
                writer.Indent++;
                writer.WriteLine("if (count < 0) {"); // TODO: add some configurable max count.
                writer.Indent++;
                writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{");
                writer.Indent++;
                writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($"values = context.{type.PeekArrayRawMethodName}(count);");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // bool TryPeek(int count, out {Type}[] values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryPeek(this ref ReadContext context, int count, out {type.TargetTypeFullName}[] values) => context.TryPeek{type.TargetTypeName}s(count, out values);");
                writer.WriteLine();
            }

            if (type.ReadArrayRawMethodName != null) {
                if (intHandler != null) {
                    // {Type}[] Read{Type}s()
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static {type.TargetTypeFullName}[] Read{type.TargetTypeName}s(this ref ReadContext context) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine($"if (count < 0) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}"); // TODO: add some configurable max count.
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"{type.TargetTypeFullName}[] values = context.{type.ReadArrayRawMethodName}(count);");
                    writer.WriteLine("return values;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // void Read(out {Type}[] values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Read(this ref ReadContext context, out {type.TargetTypeFullName}[] values) => values = context.Read{type.TargetTypeName}s();");
                    writer.WriteLine();
                    
                    // bool TryRead{Type}s(out {Type}[] values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryRead{type.TargetTypeName}s(this ref ReadContext context, out {type.TargetTypeFullName}[] values) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{");
                    writer.Indent++;
                    writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine("if (count < 0) {"); // TODO: add some configurable max count.
                    writer.Indent++;
                    writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine("if (context.IsInsufficientSpace(bitsNeeded)) {");
                    writer.Indent++;
                    writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                    writer.WriteLine("return false;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"values = context.{type.ReadArrayRawMethodName}(count);");
                    writer.WriteLine("return true;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // bool TryRead(out {Type}[] values)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryRead(this ref ReadContext context, out {type.TargetTypeFullName}[] values) => context.TryRead{type.TargetTypeName}s(out values);");
                    writer.WriteLine();
                }

                // {Type}[] Read{Type}s(int count)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static {type.TargetTypeFullName}[] Read{type.TargetTypeName}s(this ref ReadContext context, int count) {{");
                writer.Indent++;
                writer.WriteLine($"if (count < 0) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}"); // TODO: add some configurable max count.
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return Array.Empty<{type.TargetTypeFullName}>(); }}");
                writer.WriteLine($"{type.TargetTypeFullName}[] values = context.{type.ReadArrayRawMethodName}(count);");
                writer.WriteLine("return values;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // void Read(int count, out {Type}[] values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Read(this ref ReadContext context, int count, out {type.TargetTypeFullName}[] values) => values = context.Read{type.TargetTypeName}s(count);");
                writer.WriteLine();
                
                // bool TryRead{Type}s(int count, out {Type}[] values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryRead{type.TargetTypeName}s(this ref ReadContext context, int count, out {type.TargetTypeFullName}[] values) {{");
                writer.Indent++;
                writer.WriteLine("if (count < 0) {"); // TODO: add some configurable max count.
                writer.Indent++;
                writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{");
                writer.Indent++;
                writer.WriteLine($"values = Array.Empty<{type.TargetTypeFullName}>();");
                writer.WriteLine("return false;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($"values = context.{type.ReadArrayRawMethodName}(count);");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // bool TryRead(int count, out {Type}[] values)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryRead(this ref ReadContext context, int count, out {type.TargetTypeFullName}[] values) => context.TryRead{type.TargetTypeName}s(count, out values);");
                writer.WriteLine();
            }

            if (type.PeekSpanRawMethodName != null) {
                if (intHandler != null) {
                    // void Peek{Type}s(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Peek{type.TargetTypeName}s(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{ return; }}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine("if (0 > count || count > destination.Length) { return; }");
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine("if (context.IsInsufficientSpace(bitsNeeded)) { return; }");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"context.{type.PeekSpanRawMethodName}(count, ref destination);");
                    writer.WriteLine($"context.Position -= {intHandler.Size};");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // void Peek(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Peek(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) => context.Peek{type.TargetTypeName}s(ref destination);");

                    // bool TryPeek{Type}s(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryPeek{type.TargetTypeName}s(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{ return false; }}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine("if (0 > count || count > destination.Length) { return false; }");
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine("if (context.IsInsufficientSpace(bitsNeeded)) { return false; }");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"context.{type.PeekSpanRawMethodName}(count, ref destination);");
                    writer.WriteLine($"context.Position -= {intHandler.Size};");
                    writer.WriteLine("return true;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // bool TryPeek(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryPeek(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) => context.TryPeek{type.TargetTypeName}s(ref destination);");
                }

                // void Peek{Type}s(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Peek{type.TargetTypeName}s(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) {{");
                writer.Indent++;
                writer.WriteLine("if (0 > count || count > destination.Length) { return; }");
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return; }}");
                writer.WriteLine($"context.{type.PeekSpanRawMethodName}(count, ref destination);");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // void Peek(int count, Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Peek(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) => context.Peek{type.TargetTypeName}s(count, ref destination);");
                writer.WriteLine();
                
                // bool TryPeek{Type}s(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryPeek{type.TargetTypeName}s(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) {{");
                writer.Indent++;
                writer.WriteLine("if (0 > count || count > destination.Length) { return false; }");
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return false; }}");
                writer.WriteLine($"context.{type.PeekSpanRawMethodName}(count, ref destination);");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // bool TryPeek(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryPeek(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) => context.TryPeek{type.TargetTypeName}s(count, ref destination);");
                writer.WriteLine();
            }
            
            if (type.ReadSpanRawMethodName != null) {
                if (intHandler != null) {
                    // void Read{Type}s(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Read{type.TargetTypeName}s(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{ return; }}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine("if (0 > count || count > destination.Length) { return; }");
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine("if (context.IsInsufficientSpace(bitsNeeded)) { return; }");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"context.{type.ReadSpanRawMethodName}(count, ref destination);");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // void Read(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static void Read(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) => context.Read{type.TargetTypeName}s(ref destination);");

                    // bool TryRead{Type}s(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryRead{type.TargetTypeName}s(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) {{");
                    writer.Indent++;
                    writer.WriteLine($"if (context.IsInsufficientSpace({intHandler.Size})) {{ return false; }}");
                    writer.WriteLine($"int count = context.{intHandler.PeekRawMethodName}();");
                    writer.WriteLine("if (0 > count || count > destination.Length) { return false; }");
                    writer.WriteLine($"int bitsNeeded = count * {type.Size} + {intHandler.Size};");
                    writer.WriteLine("if (context.IsInsufficientSpace(bitsNeeded)) { return false; }");
                    writer.WriteLine($"context.Position += {intHandler.Size};");
                    writer.WriteLine($"context.{type.ReadSpanRawMethodName}(count, ref destination);");
                    writer.WriteLine("return true;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    
                    // bool TryRead(ref Span<{Type}> destination)
                    writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                    writer.WriteLine($"public static bool TryRead(this ref ReadContext context, ref Span<{type.TargetTypeFullName}> destination) => context.TryRead{type.TargetTypeName}s(ref destination);");
                }

                // void Read{Type}s(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Read{type.TargetTypeName}s(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) {{");
                writer.Indent++;
                writer.WriteLine("if (0 > count || count > destination.Length) { return; }");
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return; }}");
                writer.WriteLine($"context.{type.ReadSpanRawMethodName}(count, ref destination);");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // void Read(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static void Read(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) => context.Read{type.TargetTypeName}s(count, ref destination);");
                writer.WriteLine();
                
                // bool TryRead{Type}s(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryRead{type.TargetTypeName}s(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) {{");
                writer.Indent++;
                writer.WriteLine("if (0 > count || count > destination.Length) { return false; }");
                writer.WriteLine($"int bitsNeeded = count * {type.Size};");
                writer.WriteLine($"if (context.IsInsufficientSpace(bitsNeeded)) {{ return false; }}");
                writer.WriteLine($"context.{type.ReadSpanRawMethodName}(count, ref destination);");
                writer.WriteLine("return true;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                
                // bool TryRead(int count, ref Span<{Type}> destination)
                writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine($"public static bool TryRead(this ref ReadContext context, int count, ref Span<{type.TargetTypeFullName}> destination) => context.TryRead{type.TargetTypeName}s(count, ref destination);");
                writer.WriteLine();
            }

            writer.Indent--;
            writer.WriteLine("}");

            writer.Indent--;
            writer.WriteLine("}");

            return stringWriter.ToString();
        }
    }
}
