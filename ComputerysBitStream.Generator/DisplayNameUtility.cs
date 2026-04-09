using System.Text;
using Microsoft.CodeAnalysis;

namespace ComputerysBitStream.Generator;

internal static class DisplayNameUtility {
    private static readonly SymbolDisplayFormat CSharpDefaultFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes |
                              SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
    );

    public static string GetDisplayName(ITypeSymbol symbol) {
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
            SpecialType.System_Decimal => "Decimal",
            SpecialType.System_String => "String",
            SpecialType.System_Char => "Char",

            // doupt these will ever get hit but trying to make this compleate
            SpecialType.System_DateTime => "DateTime",
            SpecialType.System_IntPtr => "NInt",
            SpecialType.System_UIntPtr => "NUInt",
            SpecialType.System_Object => "Object",
            SpecialType.System_Void => "Void",

            _ => SanitizeTypeName(symbol.ToDisplayString(CSharpDefaultFormat))
        };
    }

    private static string SanitizeTypeName(string displayName) {
        if (string.IsNullOrWhiteSpace(displayName)) { return "Type"; }

        StringBuilder builder = new(displayName.Length);

        for (int i = 0; i < displayName.Length; i++) {
            char c = displayName[i];

            if (char.IsLetterOrDigit(c) || c == '_') {
                builder.Append(c);
                continue;
            }

            switch (c) {
                case '<':
                    AppendWord("Of", builder);
                    break;
                case ',':
                    AppendWord("And", builder);
                    break;
                case '[': {
                    int rank = 1;
                    int end = i + 1;

                    while (end < displayName.Length && displayName[end] != ']') {
                        if (displayName[end] == ',') { rank++; }

                        end++;
                    }

                    AppendWord(rank == 1 ? "Array" : $"{rank}DArray", builder);

                    if (end < displayName.Length) { i = end; }

                    break;
                }
                case '?':
                    AppendWord("Nullable", builder);
                    break;
            }
        }

        if (builder.Length == 0) { return "Type"; }

        if (char.IsDigit(builder[0])) { builder.Insert(0, 'T'); }

        return builder.ToString();
    }

    private static void AppendWord(string word, StringBuilder builder) {
        if (word.Length > 0) { builder.Append(word); }
    }
}