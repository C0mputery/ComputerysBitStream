using System.Collections.Generic;
using ComputerysBitStream;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator;

internal record BitStreamTypeInfo(
    string ClassNamespace,
    string TargetTypeFullName,
    string TargetTypeName,
    int Size,
    RawRoleBindings RawMethods,
    SourceLocationInfo? Location,
    Helpers<DuplicateRawRoleInfo> DuplicateRoles,
    Helpers<NonPublicRawMethodInfo> NonPublicRawMethods
);

internal readonly record struct RawRoleBindings {
    public readonly string? WriteRawMethodName;
    public readonly string? WriteSpanRawMethodName;
    public readonly string? PeekRawMethodName;
    public readonly string? ReadRawMethodName;
    public readonly string? PeekArrayRawMethodName;
    public readonly string? ReadArrayRawMethodName;
    public readonly string? PeekSpanRawMethodName;
    public readonly string? ReadSpanRawMethodName;
    public RawRoleBindings(Dictionary<BitStreamRawRole, string> methodsByRole) {
        methodsByRole.TryGetValue(BitStreamRawRole.Write, out WriteRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.WriteSpan, out WriteSpanRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.Peek, out PeekRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.Read, out ReadRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.PeekArray, out PeekArrayRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.ReadArray, out ReadArrayRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.PeekSpan, out PeekSpanRawMethodName);
        methodsByRole.TryGetValue(BitStreamRawRole.ReadSpan, out ReadSpanRawMethodName);
    }
}

internal record struct DuplicateRawRoleInfo(string Role, string ClassName, string FirstMethod, string SecondMethod, SourceLocationInfo? Location);
internal record struct NonPublicRawMethodInfo(string Role, string ClassName, string MethodName, string Accessibility, SourceLocationInfo? Location);