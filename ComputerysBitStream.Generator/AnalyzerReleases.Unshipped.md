; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
CBS001 | BitStream | Error | Multiple methods with the same role
CBS002 | BitStream | Error | Multiple included raw types for the same target type
CBS003 | BitStream | Error | Multiple global settings
CBS004 | BitStream | Error | Missing BitStreamSettings attribute
CBS005 | BitStream | Warning | Invalid setting type
CBS006 | BitStream | Warning | No raw methods on BitStreamRawType class
CBS007 | BitStream | Error | BitStreamRawMethod method is not public static
CBS008 | BitStream | Error | Duplicate alias across raw types
CBS009 | BitStream | Error | Invalid size in BitStreamRawType
CBS010 | BitStream | Error | BitStreamRawType class is not static
CBS011 | BitStream | Error | Invalid raw method signature
CBS012 | BitStream | Warning | Struct member type not serializable
CBS013 | BitStream | Error | Struct has no serializable members
CBS014 | BitStream | Error | Duplicate alias across raw types or structs
CBS015 | BitStream | Warning | Read-only property skipped
CBS016 | BitStream | Warning | Read-only field skipped despite inclusion attribute
CBS017 | BitStream | Error | BitStreamProxyStructAttribute target is not a struct
CBS018 | BitStream | Warning | Included member not found on external struct
CBS019 | BitStream | Warning | Property skipped because its setter is not public
CBS020 | BitStream | Error | BitStreamProxyStruct class is not static
CBS021 | BitStream | Error | BitStreamProxyStruct class is not partial
CBS022 | BitStream | Error | BitStreamStruct struct is not partial
CBS023 | BitStream | Error | Cyclic struct reference detected
CBS024 | BitStream | Warning | Ref field skipped because ref fields cannot be serialized
CBS025 | BitStream | Error | Invalid settings type in BitStreamStruct

