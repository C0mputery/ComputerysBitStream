using System.Collections.Generic;
using System.Collections.Immutable;
using ComputerysBitStream.Generator.Emission;

namespace ComputerysBitStream.Generator.Emitters;

internal readonly ref partial struct StructPrimitiveSourceEmitter {
    private void EmitCollectionMethods() {
        ImmutableArray<ResolvedStructMember> members = _members;
        List<string> methods = [];
        for (int memberIndex = 0; memberIndex < members.Length; memberIndex++) {
            if (members[memberIndex].Collection is not ResolvedStructCollection collection) { continue; }

            methods.Add(EmitCollectionLengthReader(memberIndex, collection));
            ImmutableArray<int> ranks = collection.Source.Ranks;
            for (int level = 0; level < ranks.Length; level++) {
                CollectionLevelContext context = new(memberIndex, level, collection);
                methods.Add(EmitCollectionValidator(context));
                methods.Add(EmitCollectionWriter(context));
                methods.Add(EmitCollectionReader(context));
                methods.Add(EmitCollectionSizer(context));
            }
        }

        if (methods.Count == 0) { return; }
        _writer.WriteLine();
        _writer.WriteBlocks(methods);
    }

    private static string EmitCollectionLengthReader(int memberIndex, in ResolvedStructCollection collection) {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 private static bool TryReadCollection{{memberIndex}}Length(ref ReadContext context, int maxRead, out int length) {
                     if (maxRead < 0) {
                         length = 0;
                         return false;
                     }

                     long startPosition = context.Position;
                     if (!{{collection.IntExtensionClass}}.{{collection.IntTryReadMethod}}(ref context, out {{collection.IntTargetTypeEmitName}} encodedLength) || encodedLength > int.MaxValue) {
                         context.Position = startPosition;
                         length = 0;
                         return false;
                     }

                     length = (int)encodedLength;
                     if (length > maxRead) {
                         context.Position = startPosition;
                         length = 0;
                         return false;
                     }

                     return true;
                 }
                 """;
    }

    private static string EmitCollectionValidator(in CollectionLevelContext context) {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 private static void ValidateCollection{{context.MemberIndex}}Level{{context.Level}}({{context.ArrayType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(BuildCollectionValidatorBody(context), 1)}}
                 }
                 """;
    }

    private static string EmitCollectionWriter(in CollectionLevelContext context) {
        string body = context.Level == 0
            ? $$"""
                ValidateCollection{{context.MemberIndex}}Level0(value);
                {{BuildCollectionWriterBody(context)}}
                """
            : BuildCollectionWriterBody(context);

        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 private static void WriteCollection{{context.MemberIndex}}Level{{context.Level}}(ref WriteContext context, {{context.ArrayType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(body, 1)}}
                 }
                 """;
    }

    private static string EmitCollectionReader(in CollectionLevelContext context) {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 private static bool TryReadCollection{{context.MemberIndex}}Level{{context.Level}}(ref ReadContext context, out {{context.ArrayType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(BuildCollectionReaderBody(context), 1)}}
                 }
                 """;
    }

    private static string EmitCollectionSizer(in CollectionLevelContext context) {
        return $$"""
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 private static int GetCollection{{context.MemberIndex}}Level{{context.Level}}Size({{context.ArrayType}} value) {
                     {{SourceWriter.MaintainRelativeIndent(BuildCollectionSizerBody(context), 1)}}
                 }
                 """;
    }

    private static string BuildCollectionValidatorBody(in CollectionLevelContext context) {
        string nestedValidation = context.IsElementLevel
            ? string.Empty
            : $"foreach ({context.LevelElementType} item in value) {{ ValidateCollection{context.MemberIndex}Level{context.Level + 1}(item); }}";

        return $$"""
                 if (value is null) { return; }
                 {{BuildZeroLowerBoundGuard(context.Rank)}}
                 {{BuildDimensionLimitChecks(context)}}
                 {{nestedValidation}}
                 """;
    }

    private static string BuildCollectionWriterBody(in CollectionLevelContext context) {
        if (context.IsElementVector) {
            return $$"""
                     value ??= {{context.CreateEmptyArray()}};
                     {{BuildZeroLowerBoundGuard(context.Rank)}}
                     {{context.Collection.ElementWriteContextClass}}.{{context.Collection.ElementWriteWithMaxCountMethod}}(ref context, value, {{context.Limits[context.LimitOffset]}}{{context.Collection.ElementExtraArguments}});
                     """;
        }

        string writeBody = context.IsElementLevel
            ? BuildFlattenedElementWrite(context)
            : $"foreach ({context.LevelElementType} item in value) {{ WriteCollection{context.MemberIndex}Level{context.Level + 1}(ref context, item); }}";

        return $$"""
                 value ??= {{context.CreateEmptyArray()}};
                 {{BuildZeroLowerBoundGuard(context.Rank)}}
                 {{BuildDimensionLimitChecks(context)}}
                 {{BuildLengthPrefixWrites(context)}}
                 {{writeBody}}
                 """;
    }

    private static string BuildCollectionReaderBody(in CollectionLevelContext context) {
        if (context.IsElementVector) {
            return $"return {context.Collection.ElementReadContextClass}.{context.Collection.ElementTryReadMethod}(ref context, {context.Limits[context.LimitOffset]}{context.Collection.ElementExtraArguments}, out value);";
        }

        if (context.IsElementLevel) {
            return $$"""
                     {{BuildLengthReads(context)}}
                     {{BuildMultidimensionalElementRead(context)}}
                     """;
        }

        string readChild = $$"""
                             if (!TryReadCollection{{context.MemberIndex}}Level{{context.Level + 1}}(ref context, out {{context.LevelElementType}} item)) { return false; }
                             value[{{BuildIndexList(context.Rank)}}] = item;
                             """;

        return $$"""
                 {{BuildLengthReads(context)}}
                 value = {{context.CreateSizedArray("length")}};
                 {{BuildNestedLoops(context.Rank, readChild)}}
                 return true;
                 """;
    }

    private static string BuildCollectionSizerBody(in CollectionLevelContext context) {
        string prefixBits = BuildLengthPrefixSizeAccumulation(context);
        string accumulation = context.IsElementLevel
            ? BuildElementSizeAccumulation(context)
            : $"foreach ({context.LevelElementType} item in value) {{ bits += GetCollection{context.MemberIndex}Level{context.Level + 1}Size(item); }}";

        return $$"""
                 value ??= {{context.CreateEmptyArray()}};
                 {{BuildZeroLowerBoundGuard(context.Rank)}}
                 int bits = {{prefixBits}};
                 checked {
                     {{SourceWriter.MaintainRelativeIndent(accumulation, 1)}}
                 }
                 return bits;
                 """;
    }

    private static string BuildDimensionLimitChecks(in CollectionLevelContext context) {
        List<string> lines = [];
        for (int dimension = 0; dimension < context.Rank; dimension++) {
            lines.Add($"if (value.GetLength({dimension}) > {context.Limits[context.LimitOffset + dimension]}) {{ throw new ArgumentException(\"Array dimension {dimension} exceeds its configured maximum entry count.\", nameof(value)); }}");
        }
        return string.Join("\n", lines);
    }

    private static string BuildLengthPrefixWrites(in CollectionLevelContext context) {
        List<string> lines = [];
        for (int dimension = 0; dimension < context.Rank; dimension++) {
            lines.Add($"{context.Collection.IntExtensionClass}.{context.Collection.IntWriteMethod}(ref context, ({context.Collection.IntTargetTypeEmitName})value.GetLength({dimension}));");
        }
        return string.Join("\n", lines);
    }

    private static string BuildLengthPrefixSizeAccumulation(in CollectionLevelContext context) {
        if (context.IsElementVector) {
            return $"{context.Collection.IntExtensionClass}.{context.Collection.IntSizeMethod}(({context.Collection.IntTargetTypeEmitName})value.Length)";
        }

        List<string> parts = [];
        for (int dimension = 0; dimension < context.Rank; dimension++) {
            parts.Add($"{context.Collection.IntExtensionClass}.{context.Collection.IntSizeMethod}(({context.Collection.IntTargetTypeEmitName})value.GetLength({dimension}))");
        }
        return string.Join(" + ", parts);
    }

    private static string BuildFlattenedElementWrite(in CollectionLevelContext context) {
        string elementType = context.Collection.ElementTypeEmitName;
        return $$"""
                 {{elementType}}[] flattened = new {{elementType}}[value.Length];
                 int flattenedIndex = 0;
                 foreach ({{elementType}} item in value) { flattened[flattenedIndex++] = item; }
                 {{context.Collection.ElementWriteContextClass}}.{{context.Collection.ElementWriteWithoutLengthMethod}}(ref context, flattened{{context.Collection.ElementExtraArguments}});
                 """;
    }

    private static string BuildLengthReads(in CollectionLevelContext context) {
        List<string> lines = [];
        for (int dimension = 0; dimension < context.Rank; dimension++) {
            lines.Add($"if (!TryReadCollection{context.MemberIndex}Length(ref context, {context.Limits[context.LimitOffset + dimension]}, out int length{dimension})) {{ value = {context.CreateEmptyArray()}; return false; }}");
        }
        return string.Join("\n", lines);
    }

    private static string BuildMultidimensionalElementRead(in CollectionLevelContext context) {
        List<string> totalLengthLines = [];
        for (int dimension = 0; dimension < context.Rank; dimension++) {
            totalLengthLines.Add($"totalLength *= length{dimension};");
        }

        string elementType = context.Collection.ElementTypeEmitName;
        return $$"""
                 long totalLength = 1;
                 {{string.Join("\n", totalLengthLines)}}
                 if (totalLength > int.MaxValue || !{{context.Collection.ElementReadContextClass}}.{{context.Collection.ElementTryReadWithCountMethod}}(ref context, (int)totalLength{{context.Collection.ElementExtraArguments}}, out {{elementType}}[] flattened)) { value = {{context.CreateEmptyArray()}}; return false; }
                 value = {{context.CreateSizedArray("length")}};
                 int flattenedIndex = 0;
                 {{BuildNestedLoops(context.Rank, $"value[{BuildIndexList(context.Rank)}] = flattened[flattenedIndex++];")}}
                 return true;
                 """;
    }

    private static string BuildElementSizeAccumulation(in CollectionLevelContext context) {
        if (context.Collection.ElementFixedSize is int fixedSize) {
            return $"bits += value.Length * {fixedSize};";
        }

        if (context.Collection.ElementSizeExpression is string sizeExpression) {
            string itemSize = sizeExpression.Replace("{0}", "item");
            return $"foreach ({context.Collection.ElementTypeEmitName} item in value) {{ bits += {itemSize}; }}";
        }

        return string.Empty;
    }

    private static string BuildZeroLowerBoundGuard(int rank) {
        List<string> conditions = [];
        for (int dimension = 0; dimension < rank; dimension++) { conditions.Add($"value.GetLowerBound({dimension}) != 0"); }
        return $"if ({string.Join(" || ", conditions)}) {{ throw new ArgumentException(\"Only zero-based arrays are supported.\", nameof(value)); }}";
    }

    private static string BuildNestedLoops(int rank, string innerBody) => BuildNestedLoops(0, rank, innerBody);

    private static string BuildNestedLoops(int dimension, int rank, string innerBody) {
        if (dimension >= rank) { return innerBody; }

        return $$"""
                 for (int index{{dimension}} = 0; index{{dimension}} < length{{dimension}}; index{{dimension}}++) {
                     {{SourceWriter.MaintainRelativeIndent(BuildNestedLoops(dimension + 1, rank, innerBody), 1)}}
                 }
                 """;
    }

    private static string BuildIndexList(int rank) {
        List<string> indices = [];
        for (int dimension = 0; dimension < rank; dimension++) { indices.Add($"index{dimension}"); }
        return string.Join(", ", indices);
    }

    private readonly record struct CollectionLevelContext(int MemberIndex, int Level, ResolvedStructCollection Collection) {
        public ImmutableArray<int> Ranks => Collection.Source.Ranks;
        public ImmutableArray<int> Limits => Collection.Source.MaxEntries;
        public int Rank => Ranks[Level];
        public bool IsElementLevel => Level == Ranks.Length - 1;
        public bool IsElementVector => IsElementLevel && Rank == 1;

        public int LimitOffset {
            get {
                int offset = 0;
                for (int i = 0; i < Level; i++) { offset += Ranks[i]; }
                return offset;
            }
        }

        public string ArrayType {
            get {
                ImmutableArray<string> arrayTypes = Collection.Source.ArrayTypeEmitFormats;
                return arrayTypes[Level];
            }
        }

        public string LevelElementType {
            get {
                if (IsElementLevel) { return Collection.ElementTypeEmitName; }
                ImmutableArray<string> arrayTypes = Collection.Source.ArrayTypeEmitFormats;
                return arrayTypes[Level + 1];
            }
        }

        public string CreateEmptyArray() => FormatArrayCreateExpression(ArrayType, LevelElementType, Rank, null);
        public string CreateSizedArray(string lengthPrefix) => FormatArrayCreateExpression(ArrayType, LevelElementType, Rank, lengthPrefix);
    }

    private static string FormatArrayCreateExpression(string arrayType, string elementType, int rank, string? lengthPrefix) {
        List<string> lengths = [];
        for (int dimension = 0; dimension < rank; dimension++) {
            lengths.Add(lengthPrefix is null ? "0" : $"{lengthPrefix}{dimension}");
        }

        return $"({arrayType})Array.CreateInstance(typeof({elementType}), new int[] {{ {string.Join(", ", lengths)} }})";
    }
}
