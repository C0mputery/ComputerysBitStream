using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ComputerysBitStream.Generator;

namespace ComputerysBitStream;

internal static class BitStreamSourceEmitter {
    private static readonly string GeneratedNamespace = nameof(ComputerysBitStream);
    
    private static void WriteLines(this IndentedTextWriter writer, string text) {
        string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        foreach (string line in lines) {
            if (string.IsNullOrWhiteSpace(line)) { writer.WriteLineNoTabs(""); } 
            else { writer.WriteLine(line); }
        }
    }
    
    internal static string EmitSource(BitStreamTypeInfo type, BitStreamTypeInfo? intHandler) {
        bool hasIntHandlerWrite = false;
        bool hasIntHandlerPeek = false;
        if (intHandler != null) { 
            hasIntHandlerWrite = intHandler!.RawMethods.WriteRawMethodName != null;
            hasIntHandlerPeek = intHandler!.RawMethods.PeekRawMethodName != null;
        }
        
        using StringWriter stringWriter = new StringWriter();
        using IndentedTextWriter writer = new IndentedTextWriter(stringWriter, new string(' ', 4));
        
        writer.WriteLines($$"""
        using System;
        using System.Runtime.CompilerServices;
        {{BuildAdditionalUsings(type, intHandler)}}
        
        namespace {{GeneratedNamespace}} {
        """);
        
        writer.Indent++;
        
        RawRoleBindings rawMethods  = type.RawMethods;

        bool hasWriteRawMethod = rawMethods.WriteRawMethodName != null;
        bool hasWriteSpanRawMethod = rawMethods.WriteSpanRawMethodName != null;
        if (hasWriteRawMethod || hasWriteSpanRawMethod) {
            writer.WriteLine($"public static class {type.TargetTypeName}WriteContextExtensions {{");
            writer.Indent++;
            if (hasWriteRawMethod) { writer.WriteLines(WriteMethods(type)); }
            if (hasWriteSpanRawMethod) {
                if (hasIntHandlerWrite) { writer.WriteLines(SpanWriteMethods(type, intHandler!)); }
                writer.WriteLines(SpanWriteWithoutLengthMethods(type));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }

        bool hasPeekRawMethod = rawMethods.PeekRawMethodName != null;
        bool hasReadRawMethod = rawMethods.ReadRawMethodName != null;
        bool hasPeekArrayRawMethod = rawMethods.PeekArrayRawMethodName != null;
        bool hasReadArrayRawMethod = rawMethods.ReadArrayRawMethodName != null;
        bool hasPeekSpanRawMethod = rawMethods.PeekSpanRawMethodName != null;
        bool hasReadSpanRawMethod = rawMethods.ReadSpanRawMethodName != null;
        if (hasPeekRawMethod || hasReadRawMethod || hasPeekArrayRawMethod || hasReadArrayRawMethod || hasPeekSpanRawMethod || hasReadSpanRawMethod) {
            writer.WriteLine($"public static class {type.TargetTypeName}ReadContextExtensions {{");
            writer.Indent++;
            if (hasPeekRawMethod) { writer.WriteLines(PeekMethods(type)); }
            if (hasReadRawMethod) { writer.WriteLines(ReadMethods(type)); }
            if (hasPeekArrayRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(PeekArrayMethods(type, intHandler!)); }
                writer.WriteLines(PeekArrayMethodsWithoutLengthMethods(type));
            }
            if (hasReadArrayRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(ReadArrayMethods(type, intHandler!)); }
                writer.WriteLines(ReadArrayWithoutLengthMethods(type));
            }
            if (hasPeekSpanRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(PeekSpanMethods(type, intHandler!)); }
                writer.WriteLines(PeekSpanMethodsWithoutLengthMethods(type));
            }
            if (hasReadSpanRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(ReadSpanMethods(type, intHandler!)); }
                writer.WriteLines(ReadSpanMethodsWithoutLengthMethods(type));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }
        
        writer.Indent--;
        
        writer.WriteLine("}");
        
        return stringWriter.ToString();
    }
    
    private static string BuildAdditionalUsings(BitStreamTypeInfo type, BitStreamTypeInfo? intHandler) { 
        HashSet<string> namespaces = new HashSet<string>(StringComparer.Ordinal);
        if (type.ClassNamespace != GeneratedNamespace) { namespaces.Add(type.ClassNamespace); }
        if (intHandler is not null && intHandler.ClassNamespace != GeneratedNamespace) { namespaces.Add(intHandler.ClassNamespace); }
        
        if (namespaces.Count == 0) { return string.Empty; }
        
        StringBuilder builder = new StringBuilder();
        foreach (string ns in namespaces) { builder.Append("using ").Append(ns).AppendLine(";"); }
        return builder.ToString().TrimEnd();
    }
    
    
    // 1. void Write{Type}({Type} value)
    // 2. void Write({Type} value)
    private static string WriteMethods(BitStreamTypeInfo type) {
        return $$"""
        /// <summary>
        /// Writes a <see cref="{{type.TargetTypeFullName}}"/> value to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.TargetTypeName}}(this ref WriteContext context, {{type.TargetTypeFullName}} value) {
            context.ThrowIfNoSpace("{{type.TargetTypeName}}", {{type.Size}});
            
            context.{{type.RawMethods.WriteRawMethodName}}(value);
        }

        /// <summary>
        /// Writes a <see cref="{{type.TargetTypeFullName}}"/> value to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ref WriteContext context, {{type.TargetTypeFullName}} value) => context.Write{{type.TargetTypeName}}(value);
        """;
    }

    // 1. void Write{Type}s(ReadOnlySpan<{Type}> values)
    // 2. void Write(ReadOnlySpan<{Type}> values)
    private static string SpanWriteMethods(BitStreamTypeInfo type, BitStreamTypeInfo intHandler) {
        return $$"""
        /// <summary>
        /// Writes a length-prefixed span of <see cref="{{type.TargetTypeFullName}}"/> values to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.TargetTypeName}}s(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) {
            int bitsNeeded = values.Length * {{type.Size}} + {{intHandler.Size}};
            context.ThrowIfNoSpace("{{type.TargetTypeName}} array", bitsNeeded);
            
            context.{{intHandler.RawMethods.WriteRawMethodName}}(values.Length);
            context.{{type.RawMethods.WriteSpanRawMethodName}}(values);
        }

        /// <summary>
        /// Writes a length-prefixed span of <see cref="{{type.TargetTypeFullName}}"/> values to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) => context.Write{{type.TargetTypeName}}s(values);
        """;
    }
    
    // 1. void Write{Type}sWithoutLength(ReadOnlySpan<{Type}> values)
    // 2. void WriteWithoutLength(ReadOnlySpan<{Type}> values)
    private static string SpanWriteWithoutLengthMethods(BitStreamTypeInfo type) {
        return $$"""
        /// <summary>
        /// Writes a span of <see cref="{{type.TargetTypeFullName}}"/> values to the bit stream without a length prefix.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.TargetTypeName}}sWithoutLength(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) {
            int totalSize = values.Length * {{type.Size}};
            context.ThrowIfNoSpace("{{type.TargetTypeName}} span", totalSize);
            
            context.{{type.RawMethods.WriteSpanRawMethodName}}(values);
        }

        /// <summary>
        /// Writes a span of <see cref="{{type.TargetTypeFullName}}"/> values to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WithoutLength(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullName}}> values) => context.Write{{type.TargetTypeName}}sWithoutLength(values);
        """;
    }
    
    // 1. {Type} Peek{Type}()
    // 2. void Peek(out {Type} value)
    // 3. bool TryPeek{Type}(out {Type} value)
    // 4. bool TryPeek(out {Type} value)
    private static string PeekMethods(BitStreamTypeInfo type) {
        return $$"""
        /// <summary>
        /// Peeks at a <see cref="{{type.TargetTypeFullName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>The value at the current position, or the default value if there is insufficient data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}} Peek{{type.TargetTypeName}}(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{type.Size}})) { return default; }
            
            return context.{{type.RawMethods.PeekRawMethodName}}();
        }

        /// <summary>
        /// Peeks at a <see cref="{{type.TargetTypeFullName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position, or the default value if there is insufficient data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, out {{type.TargetTypeFullName}} value) => value = context.Peek{{type.TargetTypeName}}();

        /// <summary>
        /// Attempts to peek at a <see cref="{{type.TargetTypeFullName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}(this ref ReadContext context, out {{type.TargetTypeFullName}} value) {
            if (context.IsInsufficientSpace({{type.Size}})) {
                value = default;
                return false;
            }
            
            value = context.{{type.RawMethods.PeekRawMethodName}}();
            return true;
        }

        /// <summary>
        /// Attempts to peek at a <see cref="{{type.TargetTypeFullName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Reads a <see cref="{{type.TargetTypeFullName}}"/> value from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>The value at the current position, or the default value if there is insufficient data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}} Read{{type.TargetTypeName}}(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{type.Size}})) { return default; }
            
            return context.{{type.RawMethods.ReadRawMethodName}}();
        }

        /// <summary>
        /// Reads a <see cref="{{type.TargetTypeFullName}}"/> value from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position, or the default value if there is insufficient data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, out {{type.TargetTypeFullName}} value) => value = context.Read{{type.TargetTypeName}}();

        /// <summary>
        /// Attempts to read a <see cref="{{type.TargetTypeFullName}}"/> value from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}(this ref ReadContext context, out {{type.TargetTypeFullName}} value) {
            if (context.IsInsufficientSpace({{type.Size}})) {
                value = default;
                return false;
            }
            
            value = context.{{type.RawMethods.ReadRawMethodName}}();
            return true;
        }

        /// <summary>
        /// Attempts to read a <see cref="{{type.TargetTypeFullName}}"/> value from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Peeks at a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or the encoded length is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Peek{{type.TargetTypeName}}s(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            context.Position += {{intHandler.Size}};
            {{type.TargetTypeFullName}}[] values = context.{{type.RawMethods.PeekArrayRawMethodName}}(count);
            context.Position -= {{intHandler.Size}};
            
            return values;
        }

        /// <summary>
        /// Peeks at a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or the encoded length is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) => values = context.Peek{{type.TargetTypeName}}s();

        /// <summary>
        /// Attempts to peek at a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }
            
            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
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
            values = context.{{type.RawMethods.PeekArrayRawMethodName}}(count);
            context.Position -= {{intHandler.Size}};
            
            return true;
        }

        /// <summary>
        /// Attempts to peek at a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Peeks at an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Peek{{type.TargetTypeName}}s(this ref ReadContext context, int count) {
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            {{type.TargetTypeFullName}}[] values = context.{{type.RawMethods.PeekArrayRawMethodName}}(count);
            return values;
        }

        /// <summary>
        /// Peeks at an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) => values = context.Peek{{type.TargetTypeName}}s(count);

        /// <summary>
        /// Attempts to peek at an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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

            values = context.{{type.RawMethods.PeekArrayRawMethodName}}(count);
            return true;
        }

        /// <summary>
        /// Attempts to peek at an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Reads a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or the encoded length is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Read{{type.TargetTypeName}}s(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }
            
            context.Position += {{intHandler.Size}};
            {{type.TargetTypeFullName}}[] values = context.{{type.RawMethods.ReadArrayRawMethodName}}(count);
            return values;
        }

        /// <summary>
        /// Reads a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or the encoded length is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) => values = context.Read{{type.TargetTypeName}}s();

        /// <summary>
        /// Attempts to read a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, out {{type.TargetTypeFullName}}[] values) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) {
                values = Array.Empty<{{type.TargetTypeFullName}}>();
                return false;
            }

            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
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
            values = context.{{type.RawMethods.ReadArrayRawMethodName}}(count);
            return true;
        }

        /// <summary>
        /// Attempts to read a length-prefixed array of <see cref="{{type.TargetTypeFullName}}"/> values from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Reads an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullName}}[] Read{{type.TargetTypeName}}s(this ref ReadContext context, int count) {
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullName}}>(); }

            {{type.TargetTypeFullName}}[] values = context.{{type.RawMethods.ReadArrayRawMethodName}}(count);
            return values;
        }

        /// <summary>
        /// Reads an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, int count, out {{type.TargetTypeFullName}}[] values) => values = context.Read{{type.TargetTypeName}}s(count);

        /// <summary>
        /// Attempts to read an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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

            values = context.{{type.RawMethods.ReadArrayRawMethodName}}(count);
            return true;
        }

        /// <summary>
        /// Attempts to read an array of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Peeks at a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return; }
            
            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return; }
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.RawMethods.PeekSpanRawMethodName}}(count, ref destination);
            context.Position -= {{intHandler.Size}};
        }

        /// <summary>
        /// Peeks at a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) => context.Peek{{type.TargetTypeName}}s(ref destination);

        /// <summary>
        /// Attempts to peek at a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return false; }
            
            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.RawMethods.PeekSpanRawMethodName}}(count, ref destination);
            context.Position -= {{intHandler.Size}};
            
            return true;
        }

        /// <summary>
        /// Attempts to peek at a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Peeks at a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return; }
            
            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.{{type.RawMethods.PeekSpanRawMethodName}}(count, ref destination);
        }

        /// <summary>
        /// Peeks at a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.Peek{{type.TargetTypeName}}s(count, ref destination);

        /// <summary>
        /// Attempts to peek at a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            context.{{type.RawMethods.PeekSpanRawMethodName}}(count, ref destination);
            
            return true;
        }

        /// <summary>
        /// Attempts to peek at a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Reads a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return; }
            
            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.RawMethods.ReadSpanRawMethodName}}(count, ref destination);
        }

        /// <summary>
        /// Reads a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) => context.Read{{type.TargetTypeName}}s(ref destination);

        /// <summary>
        /// Attempts to read a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return false; }
            
            int count = context.{{intHandler.RawMethods.PeekRawMethodName}}();
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.RawMethods.ReadSpanRawMethodName}}(count, ref destination);
            return true;
        }

        /// <summary>
        /// Attempts to read a length-prefixed sequence of <see cref="{{type.TargetTypeFullName}}"/> values into the specified destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Reads a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return; }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }

            context.{{type.RawMethods.ReadSpanRawMethodName}}(count, ref destination);
        }

        /// <summary>
        /// Reads a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.Read{{type.TargetTypeName}}s(count, ref destination);

        /// <summary>
        /// Attempts to read a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.TargetTypeName}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) {
            if (0 > count || count > destination.Length) { return false; }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }

            context.{{type.RawMethods.ReadSpanRawMethodName}}(count, ref destination);
            return true;
        }

        /// <summary>
        /// Attempts to read a sequence of <see cref="{{type.TargetTypeFullName}}"/> values of the specified length into the destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullName}}> destination) => context.TryRead{{type.TargetTypeName}}s(count, ref destination);
        """;
    }
}

