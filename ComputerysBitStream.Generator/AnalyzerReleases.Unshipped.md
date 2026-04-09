; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
CBS001 | BitStream | Error | Multiple methods with the same role
CBS002 | BitStream | Error | Multiple included raw types for the same target type
CBS003 | BitStream | Warning | Multiple global settings
CBS004 | BitStream | Error | Missing BitStreamSettings attribute
CBS005 | BitStream | Warning | Invalid setting type
CBS006 | BitStream | Warning | No raw methods on BitStreamRawType class
CBS007 | BitStream | Error | BitStreamRawMethod method is not public static
CBS008 | BitStream | Error | Duplicate alias across raw types
CBS009 | BitStream | Error | Invalid size in BitStreamRawType
CBS010 | BitStream | Error | BitStreamRawType class is not static
CBS011 | BitStream | Error | Invalid raw method signature

