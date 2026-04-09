using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ComputerysBitStream.Generator;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream;

internal static class RawTypeWrapperSourceEmitter {
    private static readonly string GeneratedNamespace = nameof(ComputerysBitStream);

    internal static void WriteLines(this IndentedTextWriter writer, string text) {
        string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        foreach (string line in lines) {
            if (string.IsNullOrWhiteSpace(line)) { writer.WriteLineNoTabs(""); } 
            else { writer.WriteLine(line); }
        }
    }
    
    internal static SourceText EmitSource(ParsedRawData type, ParsedRawData? intHandler) {
        bool hasIntHandlerWrite = false;
        bool hasIntHandlerPeek = false;
        if (intHandler != null) {
            hasIntHandlerWrite = intHandler.Value.Methods.ContainsKey(BitStreamRawRole.Write);
            hasIntHandlerPeek = intHandler.Value.Methods.ContainsKey(BitStreamRawRole.Peek);
        }
        
        using StringWriter stringWriter = new StringWriter();
        using IndentedTextWriter writer = new IndentedTextWriter(stringWriter, new string(' ', 4));
        
        writer.WriteLines($$"""
        using System;
        using System.Runtime.CompilerServices;

        namespace {{GeneratedNamespace}} {
        """);
        
        writer.Indent++;
        
        Dictionary<BitStreamRawRole, RawMethodData> methods = type.Methods;

        bool hasWriteRawMethod = methods.ContainsKey(BitStreamRawRole.Write);
        bool hasWriteSpanRawMethod = methods.ContainsKey(BitStreamRawRole.WriteSpan);
        if (hasWriteRawMethod || hasWriteSpanRawMethod) {
            writer.WriteLine($"public static class {type.Alias}WriteContextExtensions {{");
            writer.Indent++;
            if (hasWriteRawMethod) { writer.WriteLines(WriteMethods(type)); }
            if (hasWriteSpanRawMethod) {
                if (hasIntHandlerWrite) { writer.WriteLines(SpanWriteMethods(type, intHandler!.Value)); }
                writer.WriteLines(SpanWriteWithoutLengthMethods(type));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }

        bool hasPeekRawMethod = methods.ContainsKey(BitStreamRawRole.Peek);
        bool hasReadRawMethod = methods.ContainsKey(BitStreamRawRole.Read);
        bool hasPeekArrayRawMethod = methods.ContainsKey(BitStreamRawRole.PeekArray);
        bool hasReadArrayRawMethod = methods.ContainsKey(BitStreamRawRole.ReadArray);
        bool hasPeekSpanRawMethod = methods.ContainsKey(BitStreamRawRole.PeekSpan);
        bool hasReadSpanRawMethod = methods.ContainsKey(BitStreamRawRole.ReadSpan);
        if (hasPeekRawMethod || hasReadRawMethod || hasPeekArrayRawMethod || hasReadArrayRawMethod || hasPeekSpanRawMethod || hasReadSpanRawMethod) {
            writer.WriteLine($"public static class {type.Alias}ReadContextExtensions {{");
            writer.Indent++;
            if (hasPeekRawMethod) { writer.WriteLines(PeekMethods(type)); }
            if (hasReadRawMethod) { writer.WriteLines(ReadMethods(type)); }
            if (hasPeekArrayRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(PeekArrayMethods(type, intHandler!.Value)); }
                writer.WriteLines(PeekArrayMethodsWithoutLengthMethods(type));
            }
            if (hasReadArrayRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(ReadArrayMethods(type, intHandler!.Value)); }
                writer.WriteLines(ReadArrayWithoutLengthMethods(type));
            }
            if (hasPeekSpanRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(PeekSpanMethods(type, intHandler!.Value)); }
                writer.WriteLines(PeekSpanMethodsWithoutLengthMethods(type));
            }
            if (hasReadSpanRawMethod) {
                if (hasIntHandlerPeek) { writer.WriteLines(ReadSpanMethods(type, intHandler!.Value)); }
                writer.WriteLines(ReadSpanMethodsWithoutLengthMethods(type));
            }
            writer.Indent--;
            writer.WriteLine("}");
        }
        
        writer.Indent--;
        
        writer.WriteLine("}");
        
        return SourceText.From(stringWriter.ToString(), Encoding.UTF8);
    }
    
    // 1. void Write{Type}({Type} value)
    // 2. void Write({Type} value)
    private static string WriteMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Writes a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.Alias}}(this ref WriteContext context, {{type.TargetTypeFullyQualifiedName}} value) {
            context.ThrowIfNoSpace("{{type.Alias}}", {{type.Size}});
            
            context.{{type.Methods[BitStreamRawRole.Write].MethodName}}(value);
        }

        /// <summary>
        /// Writes a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ref WriteContext context, {{type.TargetTypeFullyQualifiedName}} value) => context.Write{{type.Alias}}(value);
        """;
    }

    // 1. void Write{Type}s(ReadOnlySpan<{Type}> values)
    // 2. void Write(ReadOnlySpan<{Type}> values)
    private static string SpanWriteMethods(ParsedRawData type, ParsedRawData intHandler) {
        return $$"""
        /// <summary>
        /// Writes a length-prefixed span of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.Alias}}s(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullyQualifiedName}}> values) {
            int bitsNeeded = values.Length * {{type.Size}} + {{intHandler.Size}};
            context.ThrowIfNoSpace("{{type.Alias}} array", bitsNeeded);
            
            context.{{intHandler.Methods[BitStreamRawRole.Write].MethodName}}(values.Length);
            context.{{type.Methods[BitStreamRawRole.WriteSpan].MethodName}}(values);
        }

        /// <summary>
        /// Writes a length-prefixed span of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullyQualifiedName}}> values) => context.Write{{type.Alias}}s(values);
        """;
    }
    
    // 1. void Write{Type}sWithoutLength(ReadOnlySpan<{Type}> values)
    // 2. void WriteWithoutLength(ReadOnlySpan<{Type}> values)
    private static string SpanWriteWithoutLengthMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Writes a span of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values to the bit stream without a length prefix.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write{{type.Alias}}sWithoutLength(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullyQualifiedName}}> values) {
            int totalSize = values.Length * {{type.Size}};
            context.ThrowIfNoSpace("{{type.Alias}} span", totalSize);
            
            context.{{type.Methods[BitStreamRawRole.WriteSpan].MethodName}}(values);
        }

        /// <summary>
        /// Writes a span of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values to the bit stream.
        /// </summary>
        /// <param name="context">The write context.</param>
        /// <param name="values">The values to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteWithoutLength(this ref WriteContext context, ReadOnlySpan<{{type.TargetTypeFullyQualifiedName}}> values) => context.Write{{type.Alias}}sWithoutLength(values);
        """;
    }
    
    // 1. {Type} Peek{Type}()
    // 2. void Peek(out {Type} value)
    // 3. bool TryPeek{Type}(out {Type} value)
    // 4. bool TryPeek(out {Type} value)
    private static string PeekMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Peeks at a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>The value at the current position, or the default value if there is insufficient data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullyQualifiedName}} Peek{{type.Alias}}(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{type.Size}})) { return default; }
            
            return context.{{type.Methods[BitStreamRawRole.Peek].MethodName}}();
        }

        /// <summary>
        /// Peeks at a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position, or the default value if there is insufficient data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}} value) => value = context.Peek{{type.Alias}}();

        /// <summary>
        /// Attempts to peek at a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.Alias}}(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}} value) {
            if (context.IsInsufficientSpace({{type.Size}})) {
                value = default;
                return false;
            }
            
            value = context.{{type.Methods[BitStreamRawRole.Peek].MethodName}}();
            return true;
        }

        /// <summary>
        /// Attempts to peek at a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}} value) => context.TryPeek{{type.Alias}}(out value);
        """;
    }
    
    // 1. {Type} Read{Type}()
    // 2. void Read(out {Type} value)
    // 3. bool TryRead{Type}(out {Type} value)
    // 4. bool TryRead(out {Type} value)
    private static string ReadMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Reads a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>The value at the current position, or the default value if there is insufficient data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullyQualifiedName}} Read{{type.Alias}}(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{type.Size}})) { return default; }
            
            return context.{{type.Methods[BitStreamRawRole.Read].MethodName}}();
        }

        /// <summary>
        /// Reads a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position, or the default value if there is insufficient data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}} value) => value = context.Read{{type.Alias}}();

        /// <summary>
        /// Attempts to read a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.Alias}}(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}} value) {
            if (context.IsInsufficientSpace({{type.Size}})) {
                value = default;
                return false;
            }
            
            value = context.{{type.Methods[BitStreamRawRole.Read].MethodName}}();
            return true;
        }

        /// <summary>
        /// Attempts to read a <see cref="{{type.TargetTypeFullyQualifiedName}}"/> value from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="value">When this method returns, contains the value at the current position if successful; otherwise, the default value.</param>
        /// <returns><see langword="true"/> if the value could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}} value) => context.TryRead{{type.Alias}}(out value);
        """;
    }
    
    // 1. {Type}[] Peek{Type}s()
    // 2. void Peek(out {Type}[] values)
    // 3. bool TryPeek{Type}s(out {Type}[] values)
    // 4. bool TryPeek(out {Type}[] values)
    private static string PeekArrayMethods(ParsedRawData type, ParsedRawData intHandler) {
        return $$"""
        /// <summary>
        /// Peeks at a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or the encoded length is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullyQualifiedName}}[] Peek{{type.Alias}}s(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }
            
            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }
            
            context.Position += {{intHandler.Size}};
            {{type.TargetTypeFullyQualifiedName}}[] values = context.{{type.Methods[BitStreamRawRole.PeekArray].MethodName}}(count);
            context.Position -= {{intHandler.Size}};
            
            return values;
        }

        /// <summary>
        /// Peeks at a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or the encoded length is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}}[] values) => values = context.Peek{{type.Alias}}s();

        /// <summary>
        /// Attempts to peek at a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.Alias}}s(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}}[] values) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }
            
            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }
            
            context.Position += {{intHandler.Size}};
            values = context.{{type.Methods[BitStreamRawRole.PeekArray].MethodName}}(count);
            context.Position -= {{intHandler.Size}};
            
            return true;
        }

        /// <summary>
        /// Attempts to peek at a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}}[] values) => context.TryPeek{{type.Alias}}s(out values);
        """;
    }
    
    // 1. {Type}[] Peek{Type}s(int count)
    // 2. void Peek(int count, out {Type}[] values)
    // 3. bool TryPeek{Type}s(int count, out {Type}[] values)
    // 4. bool TryPeek(int count, out {Type}[] values)
    private static string PeekArrayMethodsWithoutLengthMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Peeks at an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullyQualifiedName}}[] Peek{{type.Alias}}s(this ref ReadContext context, int count) {
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }

            {{type.TargetTypeFullyQualifiedName}}[] values = context.{{type.Methods[BitStreamRawRole.PeekArray].MethodName}}(count);
            return values;
        }

        /// <summary>
        /// Peeks at an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, int count, out {{type.TargetTypeFullyQualifiedName}}[] values) => values = context.Peek{{type.Alias}}s(count);

        /// <summary>
        /// Attempts to peek at an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.Alias}}s(this ref ReadContext context, int count, out {{type.TargetTypeFullyQualifiedName}}[] values) {
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }

            values = context.{{type.Methods[BitStreamRawRole.PeekArray].MethodName}}(count);
            return true;
        }

        /// <summary>
        /// Attempts to peek at an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length at the current position without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, int count, out {{type.TargetTypeFullyQualifiedName}}[] values) => context.TryPeek{{type.Alias}}s(count, out values);
        """;
    }
    
    // 1. {Type}[] Read{Type}s()
    // 2. void Read(out {Type}[] values)
    // 3. bool TryRead{Type}s(out {Type}[] values)
    // 4. bool TryRead(out {Type}[] values)
    private static string ReadArrayMethods(ParsedRawData type, ParsedRawData intHandler) {
        return $$"""
        /// <summary>
        /// Reads a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or the encoded length is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullyQualifiedName}}[] Read{{type.Alias}}s(this ref ReadContext context) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }

            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }
            
            context.Position += {{intHandler.Size}};
            {{type.TargetTypeFullyQualifiedName}}[] values = context.{{type.Methods[BitStreamRawRole.ReadArray].MethodName}}(count);
            return values;
        }

        /// <summary>
        /// Reads a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or the encoded length is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}}[] values) => values = context.Read{{type.Alias}}s();

        /// <summary>
        /// Attempts to read a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.Alias}}s(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}}[] values) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }

            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }

            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }
            
            context.Position += {{intHandler.Size}};
            values = context.{{type.Methods[BitStreamRawRole.ReadArray].MethodName}}(count);
            return true;
        }

        /// <summary>
        /// Attempts to read a length-prefixed array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, out {{type.TargetTypeFullyQualifiedName}}[] values) => context.TryRead{{type.Alias}}s(out values);
        """;
    }
    
    // 1. {Type}[] Read{Type}s(int count)
    // 2. void Read(int count, out {Type}[] values)
    // 3. bool TryRead{Type}s(int count, out {Type}[] values)
    // 4. bool TryRead(int count, out {Type}[] values)
    private static string ReadArrayWithoutLengthMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Reads an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>An array of values, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type.TargetTypeFullyQualifiedName}}[] Read{{type.Alias}}s(this ref ReadContext context, int count) {
            if (count < 0) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return Array.Empty<{{type.TargetTypeFullyQualifiedName}}>(); }

            {{type.TargetTypeFullyQualifiedName}}[] values = context.{{type.Methods[BitStreamRawRole.ReadArray].MethodName}}(count);
            return values;
        }

        /// <summary>
        /// Reads an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length from the current position and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="values">When this method returns, contains the values at the current position, or an empty array if there is insufficient data or <paramref name="count"/> is invalid.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, int count, out {{type.TargetTypeFullyQualifiedName}}[] values) => values = context.Read{{type.Alias}}s(count);

        /// <summary>
        /// Attempts to read an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.Alias}}s(this ref ReadContext context, int count, out {{type.TargetTypeFullyQualifiedName}}[] values) {
            if (count < 0) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) {
                values = Array.Empty<{{type.TargetTypeFullyQualifiedName}}>();
                return false;
            }

            values = context.{{type.Methods[BitStreamRawRole.ReadArray].MethodName}}(count);
            return true;
        }

        /// <summary>
        /// Attempts to read an array of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length from the current position and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="values">When this method returns, contains the values at the current position if successful; otherwise, an empty array.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, int count, out {{type.TargetTypeFullyQualifiedName}}[] values) => context.TryRead{{type.Alias}}s(count, out values);
        """;
    }
    
    // 1. void Peek{Type}s(ref Span<{Type}> destination)
    // 2. void Peek(ref Span<{Type}> destination)
    // 3. bool TryPeek{Type}s(ref Span<{Type}> destination)
    // 4. bool TryPeek(ref Span<{Type}> destination)
    private static string PeekSpanMethods(ParsedRawData type, ParsedRawData intHandler) {
        return $$"""
        /// <summary>
        /// Peeks at a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek{{type.Alias}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return; }
            
            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (0 > count || count > destination.Length) { return; }
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.Methods[BitStreamRawRole.PeekSpan].MethodName}}(count, ref destination);
            context.Position -= {{intHandler.Size}};
        }

        /// <summary>
        /// Peeks at a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.Peek{{type.Alias}}s(ref destination);

        /// <summary>
        /// Attempts to peek at a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.Alias}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return false; }
            
            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.Methods[BitStreamRawRole.PeekSpan].MethodName}}(count, ref destination);
            context.Position -= {{intHandler.Size}};
            
            return true;
        }

        /// <summary>
        /// Attempts to peek at a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.TryPeek{{type.Alias}}s(ref destination);
        """;
    }
    
    // 1. void Peek{Type}s(int count, ref Span<{Type}> destination)
    // 2. void Peek(int count, Span<{Type}> destination)
    // 3. bool TryPeek{Type}s(int count, ref Span<{Type}> destination)
    // 4. bool TryPeek(int count, ref Span<{Type}> destination)
    private static string PeekSpanMethodsWithoutLengthMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Peeks at a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek{{type.Alias}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (0 > count || count > destination.Length) { return; }
            
            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.{{type.Methods[BitStreamRawRole.PeekSpan].MethodName}}(count, ref destination);
        }

        /// <summary>
        /// Peeks at a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Peek(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.Peek{{type.Alias}}s(count, ref destination);

        /// <summary>
        /// Attempts to peek at a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek{{type.Alias}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            context.{{type.Methods[BitStreamRawRole.PeekSpan].MethodName}}(count, ref destination);
            
            return true;
        }

        /// <summary>
        /// Attempts to peek at a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span without advancing the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to peek.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.TryPeek{{type.Alias}}s(count, ref destination);
        """;
    }
    // 1. void Read{Type}s(ref Span<{Type}> destination)
    // 2. void Read(ref Span<{Type}> destination)
    // 3. bool TryRead{Type}s(ref Span<{Type}> destination)
    // 4. bool TryRead(ref Span<{Type}> destination)
    private static string ReadSpanMethods(ParsedRawData type, ParsedRawData intHandler) {
        return $$"""
        /// <summary>
        /// Reads a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read{{type.Alias}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return; }
            
            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (0 > count || count > destination.Length) { return; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.Methods[BitStreamRawRole.ReadSpan].MethodName}}(count, ref destination);
        }

        /// <summary>
        /// Reads a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.Read{{type.Alias}}s(ref destination);

        /// <summary>
        /// Attempts to read a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.Alias}}s(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (context.IsInsufficientSpace({{intHandler.Size}})) { return false; }
            
            int count = context.{{intHandler.Methods[BitStreamRawRole.Peek].MethodName}}();
            if (0 > count || count > destination.Length) { return false; }
            
            int bitsNeeded = count * {{type.Size}} + {{intHandler.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }
            
            context.Position += {{intHandler.Size}};
            context.{{type.Methods[BitStreamRawRole.ReadSpan].MethodName}}(count, ref destination);
            return true;
        }

        /// <summary>
        /// Attempts to read a length-prefixed sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values into the specified destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.TryRead{{type.Alias}}s(ref destination);
        """;
    }
    
    // 1. void Read{Type}s(int count, ref Span<{Type}> destination)
    // 2. void Read(int count, ref Span<{Type}> destination)
    // 3. bool TryRead{Type}s(int count, ref Span<{Type}> destination)
    // 4. bool TryRead(int count, ref Span<{Type}> destination)
    private static string ReadSpanMethodsWithoutLengthMethods(ParsedRawData type) {
        return $$"""
        /// <summary>
        /// Reads a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read{{type.Alias}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (0 > count || count > destination.Length) { return; }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return; }

            context.{{type.Methods[BitStreamRawRole.ReadSpan].MethodName}}(count, ref destination);
        }

        /// <summary>
        /// Reads a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span and advances the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Read(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.Read{{type.Alias}}s(count, ref destination);

        /// <summary>
        /// Attempts to read a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead{{type.Alias}}s(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) {
            if (0 > count || count > destination.Length) { return false; }

            int bitsNeeded = count * {{type.Size}};
            if (context.IsInsufficientSpace(bitsNeeded)) { return false; }

            context.{{type.Methods[BitStreamRawRole.ReadSpan].MethodName}}(count, ref destination);
            return true;
        }

        /// <summary>
        /// Attempts to read a sequence of <see cref="{{type.TargetTypeFullyQualifiedName}}"/> values of the specified length into the destination span and advance the bit stream.
        /// </summary>
        /// <param name="context">The read context.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="destination">The span that receives the values.</param>
        /// <returns><see langword="true"/> if the values could be read; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead(this ref ReadContext context, int count, ref Span<{{type.TargetTypeFullyQualifiedName}}> destination) => context.TryRead{{type.Alias}}s(count, ref destination);
        """;
    }
}

