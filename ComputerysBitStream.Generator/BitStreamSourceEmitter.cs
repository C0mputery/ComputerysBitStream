using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using ComputerysBitStream.Generator;

namespace ComputerysBitStream;

internal static class BitStreamSourceEmitter {
    private static void WriteLines(this IndentedTextWriter writer, string text) {
        string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        foreach (string line in lines) { writer.WriteLine(line); }
    }
    
    // TODO: don't hardcode the ComputerysBitStream namespace
    internal static string EmitSource(BitStreamTypeInfo type, BitStreamTypeInfo? intHandler) {
        bool hasIntHandler = intHandler != null;
        
        using StringWriter stringWriter = new StringWriter();
        using IndentedTextWriter writer = new IndentedTextWriter(stringWriter, new string(' ', 4));
        
        StringBuilder additionalUsings = new StringBuilder();
        if (type.ClassNamespace != "ComputerysBitStream") { additionalUsings.AppendLine($"using {type.ClassNamespace};"); }
        if (hasIntHandler && intHandler!.ClassNamespace != type.ClassNamespace && intHandler.ClassNamespace != "ComputerysBitStream") { additionalUsings.AppendLine($"using {intHandler.ClassNamespace};"); }
        
        writer.WriteLines($$"""
        using System;
        using System.Runtime.CompilerServices;
        {{additionalUsings.ToString().TrimEnd()}}
        
        namespace ComputerysBitStream {
        """);
        
        writer.Indent++;
        
        bool hasWriteRawMethod = type.WriteRawMethodName != null;
        bool hasWriteSpanRawMethod = type.WriteSpanRawMethodName != null;
        if (hasWriteRawMethod || hasWriteSpanRawMethod) {
            writer.WriteLine($"public static class {type.TargetTypeName}WriteContextExtensions {{");
            writer.Indent++;
            if (hasWriteRawMethod) { writer.WriteLines(WriteMethods(type)); }
            if (hasWriteSpanRawMethod) {
                if (hasIntHandler) { writer.WriteLines(SpanWriteMethods(type, intHandler!)); }
                writer.WriteLines(SpanWriteWithoutLengthMethods(type));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }

        bool hasPeekRawMethod = type.PeekRawMethodName != null;
        bool hasReadRawMethod = type.ReadRawMethodName != null;
        bool hasPeekArrayRawMethod = type.PeekArrayRawMethodName != null;
        bool hasReadArrayRawMethod = type.ReadArrayRawMethodName != null;
        bool hasPeekSpanRawMethod = type.PeekSpanRawMethodName != null;
        bool hasReadSpanRawMethod = type.ReadSpanRawMethodName != null;
        if (hasPeekRawMethod || hasReadRawMethod || hasPeekArrayRawMethod || hasReadArrayRawMethod || hasPeekSpanRawMethod || hasReadSpanRawMethod) {
            writer.WriteLine($"public static class {type.TargetTypeName}ReadContextExtensions {{");
            writer.Indent++;
            if (hasPeekRawMethod) { writer.WriteLines(PeekMethods(type)); }
            if (hasReadRawMethod) { writer.WriteLines(ReadMethods(type)); }
            if (hasPeekArrayRawMethod) {
                if (hasIntHandler) { writer.WriteLines(PeekArrayMethods(type, intHandler!)); }
                writer.WriteLines(PeekArrayMethodsWithoutLengthMethods(type));
            }
            if (hasReadArrayRawMethod) {
                if (hasIntHandler) { writer.WriteLines(ReadArrayMethods(type, intHandler!)); }
                writer.WriteLines(ReadArrayWithoutLengthMethods(type));
            }
            if (hasPeekSpanRawMethod) {
                if (hasIntHandler) { writer.WriteLines(PeekSpanMethods(type, intHandler!)); }
                writer.WriteLines(PeekSpanMethodsWithoutLengthMethods(type));
            }
            if (hasReadSpanRawMethod) {
                if (hasIntHandler) { writer.WriteLines(ReadSpanMethods(type, intHandler!)); }
                writer.WriteLines(ReadSpanMethodsWithoutLengthMethods(type));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }
        
        writer.Indent--;
        
        writer.WriteLine("}");
        
        return stringWriter.ToString();
    }
    
    // 1. void Write{Type}({Type} value)
    // 2. void Write({Type} value)
    private static string WriteMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.TargetTypeName}}(this ref WriteContext context, {{type.TargetTypeFullName}} value) {
            context.ThrowIfNoSpace("{{type.TargetTypeName}}", {{type.Size}});
            
            context.{{type.WriteRawMethodName}}(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ref WriteContext context, {{type.TargetTypeFullName}} value) => context.Write{{type.TargetTypeName}}(value);
        """;
    }

    // 1. void Write{Type}s(ReadOnlySpan<{Type}> values)
    // 2. void Write(ReadOnlySpan<{Type}> values)
    private static string SpanWriteMethods(BitStreamTypeInfo type, BitStreamTypeInfo intHandler) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.TargetTypeName}}s(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) {
            int bitsNeeded = values.Length * {{type.Size}} + {{intHandler.Size}};
            context.ThrowIfNoSpace("{{type.TargetTypeName}} array", bitsNeeded);
            
            context.{{intHandler.WriteRawMethodName}}(values.Length);
            context.{{type.WriteSpanRawMethodName}}(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) => context.Write{{type.TargetTypeName}}s(values);
        """;
    }
    
    // 1. void Write{Type}sWithoutLength(ReadOnlySpan<{Type}> values)
    // 2. void WriteWithoutLength(ReadOnlySpan<{Type}> values)
    private static string SpanWriteWithoutLengthMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.TargetTypeName}}sWithoutLength(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) {
            int totalSize = values.Length * {{type.Size}};
            context.ThrowIfNoSpace("{{type.TargetTypeName}} span", totalSize);
            
            context.{{type.WriteSpanRawMethodName}}(values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WithoutLength(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) => context.Write{{type.TargetTypeName}}s(values);
        """;
    }
    
    // 1. {Type} Peek{Type}()
    // 2. void Peek(out {Type} value)
    // 3. bool TryPeek{Type}(out {Type} value)
    // 4. bool TryPeek(out {Type} value)
    private static string PeekMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}} Peek{{type.TargetTypeName}}(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{type.Size}})) { return default; }
            
            return context.{{type.PeekRawMethodName}}();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, out {{type.TargetTypeFullName}} value) => value = context.Peek{{type.TargetTypeName}}();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}(this ref ReadContext context, out {{type.TargetTypeFullName}} value) {
            if (context.IsInsufficientSpace({{type.Size}})) {
                value = default;
                return false;
            }
            
            value = context.{{type.PeekRawMethodName}}();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, out {{type.TargetTypeFullName}} value) => context.TryPeek{{type.TargetTypeName}}(out value);
        """;
    }
    
    // 1. {Type} Read{Type}()
    // 2. void Read(out {Type} value)
    // 3. bool TryRead{Type}(out {Type} value)
    // 4. bool TryRead(out {Type} value)
    private static string ReadMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}} Read{{type.TargetTypeName}}(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{type.Size}})) { return default; }
            
            return context.{{type.ReadRawMethodName}}();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, out {{type.TargetTypeFullName}} value) => value = context.Read{{type.TargetTypeName}}();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}(this ref ReadContext context, out {{type.TargetTypeFullName}} value) {
            if (context.IsInsufficientSpace({{type.Size}})) {
                value = default;
                return false;
            }
            
            value = context.{{type.ReadRawMethodName}}();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, out {{type.TargetTypeFullName}} value) => context.TryRead{{type.TargetTypeName}}(out value);
        """;
    }
    
    // 1. {Type}[] Peek{Type}s()
    // 2. void Peek(out {Type}[] values)
    // 3. bool TryPeek{Type}s(out {Type}[] values)
    // 4. bool TryPeek(out {Type}[] values)
    private static string PeekArrayMethods(BitStreamTypeInfo type, BitStreamTypeInfo intHandler) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Peek{{type.TargetTypeName}}s(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            int count = context.{{intHandler.PeekRawMethodName}}();
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            context.Position += {{intHandler.Size}};
            {{type.TargetTypeFullName}}[] values = context.{{type.PeekArrayRawMethodName}}(count);
            context.Position -= {{intHandler.Size}};
            
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) => values = context.Peek{{type.TargetTypeName}}s();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }
            
            int count = context.{{intHandler.PeekRawMethodName}}();
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }
            
            context.Position += {{intHandler.Size}};
            values = context.{{type.PeekArrayRawMethodName}}(count);
            context.Position -= {{intHandler.Size}};
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) => context.TryPeek{{type.TargetTypeName}}s(out values);
        """;
    }
    
    // 1. {Type}[] Peek{Type}s(int count)
    // 2. void Peek(int count, out {Type}[] values)
    // 3. bool TryPeek{Type}s(int count, out {Type}[] values)
    // 4. bool TryPeek(int count, out {Type}[] values)
    private static string PeekArrayMethodsWithoutLengthMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Peek{{type.TargetTypeName}}s(this ref ReadContext context, int count) {
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            {{type.TargetTypeFullName}}[] values = context.{{type.PeekArrayRawMethodName}}(count);
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) => values = context.Peek{{type.TargetTypeName}}s(count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) {
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            values = context.{{type.PeekArrayRawMethodName}}(count);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) => context.TryPeek{{type.TargetTypeName}}s(count, out values);
        """;
    }
    
    // 1. {Type}[] Read{Type}s()
    // 2. void Read(out {Type}[] values)
    // 3. bool TryRead{Type}s(out {Type}[] values)
    // 4. bool TryRead(out {Type}[] values)
    private static string ReadArrayMethods(BitStreamTypeInfo type, BitStreamTypeInfo intHandler) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Read{{type.TargetTypeName}}s(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            int count = context.{{intHandler.PeekRawMethodName}}();
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            context.Position += {{intHandler.Size}};
            {{type.TargetTypeFullName}}[] values = context.{{type.ReadArrayRawMethodName}}(count);
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) => values = context.Read{{type.TargetTypeName}}s();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            int count = context.{{intHandler.PeekRawMethodName}}();
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }
            
            context.Position += {{intHandler.Size}};
            values = context.{{type.ReadArrayRawMethodName}}(count);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) => context.TryRead{{type.TargetTypeName}}s(out values);
        """;
    }
    
    // 1. {Type}[] Read{Type}s(int count)
    // 2. void Read(int count, out {Type}[] values)
    // 3. bool TryRead{Type}s(int count, out {Type}[] values)
    // 4. bool TryRead(int count, out {Type}[] values)
    private static string ReadArrayWithoutLengthMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Read{{type.TargetTypeName}}s(this ref ReadContext context, int count) {
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            {{type.TargetTypeFullName}}[] values = context.{{type.ReadArrayRawMethodName}}(count);
            return values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) => values = context.Read{{type.TargetTypeName}}s(count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) {
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            values = context.{{type.ReadArrayRawMethodName}}(count);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) => context.TryRead{{type.TargetTypeName}}s(count, out values);
        """;
    }
    
    // 1. void Peek{Type}s(ref Span<{Type}> destination)
    // 2. void Peek(ref Span<{Type}> destination)
    // 3. bool TryPeek{Type}s(ref Span<{Type}> destination)
    // 4. bool TryPeek(ref Span<{Type}> destination)
    private static string PeekSpanMethods(BitStreamTypeInfo type, BitStreamTypeInfo intHandler) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return; }
            
            int count = context.{{intHandler.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return; }
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.PeekSpanRawMethodName}}(count, ref destination);
            context.Position -= {{intHandler.Size}};
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) => context.Peek{{type.TargetTypeName}}s(ref destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return false; }
            
            int count = context.{{intHandler.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.PeekSpanRawMethodName}}(count, ref destination);
            context.Position -= {{intHandler.Size}};
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) => context.TryPeek{{type.TargetTypeName}}s(ref destination);
        """;
    }
    
    // 1. void Peek{Type}s(int count, ref Span<{Type}> destination)
    // 2. void Peek(int count, Span<{Type}> destination)
    // 3. bool TryPeek{Type}s(int count, ref Span<{Type}> destination)
    // 4. bool TryPeek(int count, ref Span<{Type}> destination)
    private static string PeekSpanMethodsWithoutLengthMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return; }
            
            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.{{type.PeekSpanRawMethodName}}(count, ref destination);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.Peek{{type.TargetTypeName}}s(count, ref destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            context.{{type.PeekSpanRawMethodName}}(count, ref destination);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.TryPeek{{type.TargetTypeName}}s(count, ref destination);
        """;
    }
    // 1. void Read{Type}s(ref Span<{Type}> destination)
    // 2. void Read(ref Span<{Type}> destination)
    // 3. bool TryRead{Type}s(ref Span<{Type}> destination)
    // 4. bool TryRead(ref Span<{Type}> destination)
    private static string ReadSpanMethods(BitStreamTypeInfo type, BitStreamTypeInfo intHandler) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return; }
            
            int count = context.{{intHandler.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.ReadSpanRawMethodName}}(count, ref destination);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) => context.Read{{type.TargetTypeName}}s(ref destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return false; }
            
            int count = context.{{intHandler.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.ReadSpanRawMethodName}}(count, ref destination);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) => context.TryRead{{type.TargetTypeName}}s(ref destination);
        """;
    }
    
    // 1. void Read{Type}s(int count, ref Span<{Type}> destination)
    // 2. void Read(int count, ref Span<{Type}> destination)
    // 3. bool TryRead{Type}s(int count, ref Span<{Type}> destination)
    // 4. bool TryRead(int count, ref Span<{Type}> destination)
    private static string ReadSpanMethodsWithoutLengthMethods(BitStreamTypeInfo type) {
        return $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return; }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }

            context.{{type.ReadSpanRawMethodName}}(count, ref destination);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.Read{{type.TargetTypeName}}s(count, ref destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return false; }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }

            context.{{type.ReadSpanRawMethodName}}(count, ref destination);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.TryRead{{type.TargetTypeName}}s(count, ref destination);
        """;
    }
}

