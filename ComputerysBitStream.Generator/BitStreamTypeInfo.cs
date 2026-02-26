using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


namespace ComputerysBitStream.Generator;

public record struct BitStreamLocation(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan) {
    public static implicit operator BitStreamLocation?(Location? location) {
        if (location == null) { return null; }
        FileLinePositionSpan lineSpan = location.GetLineSpan();
        return new BitStreamLocation(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }
}

public record BitStreamTypeInfo(
    string ClassNamespace,
    string ClassName,
    string TargetTypeFullName,
    string TargetTypeName,
    int Size,
    string? WriteRawMethodName,
    string? WriteSpanRawMethodName,
    string? PeekRawMethodName,
    string? ReadRawMethodName,
    string? PeekArrayRawMethodName,
    string? ReadArrayRawMethodName,
    string? PeekSpanRawMethodName,
    string? ReadSpanRawMethodName,
    BitStreamLocation? Location,
    ImmutableArray<DuplicateRawRoleInfo> DuplicateRoles
);

public record struct DuplicateRawRoleInfo(string Role, string ClassName, string FirstMethod, string SecondMethod, BitStreamLocation? Location);
