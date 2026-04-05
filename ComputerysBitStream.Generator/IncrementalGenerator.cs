using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ComputerysBitStream.Generator {
    [Generator]
    public class IncrementalGenerator : IIncrementalGenerator {
        private static readonly string ClassAttribute = typeof(BitStreamTypeAttribute).FullName!;

        public void Initialize(IncrementalGeneratorInitializationContext context) {
            IncrementalValuesProvider<BitStreamTypeInfo> pipeline = 
                context.SyntaxProvider.ForAttributeWithMetadataName(
                        fullyQualifiedMetadataName: ClassAttribute,
                        predicate: (node, _) => node is ClassDeclarationSyntax,
                        transform: Transform)
                .Where(info => info is not null)
                .Select((info, _) => info!);

            IncrementalValueProvider<ImmutableArray<BitStreamTypeInfo>> collected = pipeline.Collect();
            
            context.RegisterSourceOutput(collected, (sourceContext, handlers) => {
                Dictionary<string, BitStreamTypeInfo> handlersByTarget = new();
                foreach (BitStreamTypeInfo handler in handlers) {
                    foreach (DuplicateRawRoleInfo duplicate in handler.DuplicateRoles) {
                        Location? location = null;
                        if (duplicate.Location.HasValue) {
                            location = Location.Create(duplicate.Location.Value.FilePath, duplicate.Location.Value.TextSpan, duplicate.Location.Value.LineSpan);
                        }
                        sourceContext.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.DuplicateRawRoleRule, location, duplicate.Role, duplicate.ClassName, duplicate.FirstMethod, duplicate.SecondMethod));
                    }

                    if (handlersByTarget.ContainsKey(handler.TargetTypeFullName)) {
                        Location? location = null;
                        BitStreamLocation? bitStreamLocation = handler.Location;
                        if (bitStreamLocation.HasValue) {   
                            location = Location.Create(bitStreamLocation.Value.FilePath, bitStreamLocation.Value.TextSpan, bitStreamLocation.Value.LineSpan);
                        }
                        
                        Diagnostic diagnostic = Diagnostic.Create(DiagnosticDescriptors.DuplicateTypeRule, location, handler.TargetTypeFullName);
                        sourceContext.ReportDiagnostic(diagnostic);
                    } else {
                        handlersByTarget[handler.TargetTypeFullName] = handler;
                    }
                }
            });
            
            IncrementalValueProvider<BitStreamTypeInfo?> intHandlerProvider = pipeline
                .Where(info => info.TargetTypeFullName == SyntaxFacts.GetText(SyntaxKind.IntKeyword))
                .Collect()
                .Select((handlers, _) => handlers.Length > 0 ? (BitStreamTypeInfo?)handlers[0] : null);
            
            IncrementalValuesProvider<(BitStreamTypeInfo handler, BitStreamTypeInfo? intHandler)> combined = pipeline.Combine(intHandlerProvider);
            
            context.RegisterSourceOutput(combined, (sourceContext, pair) => {
                string source = BitStreamSourceEmitter.EmitSource(pair.handler, pair.intHandler);
                sourceContext.AddSource($"{pair.handler.TargetTypeName}ContextExtensions.g.cs", SourceText.From(source, Encoding.UTF8));
            });
        }

        private static BitStreamTypeInfo? Transform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
            if (context.TargetSymbol is not INamedTypeSymbol classSymbol) { return null; }

            AttributeData? classAttributeData = context.Attributes.FirstOrDefault(); 
            if (classAttributeData == null || classAttributeData.ConstructorArguments.Length == 0) { return null; }

            TypedConstant typeArgument = classAttributeData.ConstructorArguments[0];
            if (typeArgument.Value is not INamedTypeSymbol targetTypeSymbol) { return null; }
            
            TypedConstant sizeArgument = classAttributeData.ConstructorArguments[1];
            if (sizeArgument.Value is not int size) { return null; }
            
            List<IMethodSymbol> members = classSymbol.GetMembers().OfType<IMethodSymbol>().ToList();
            
            Dictionary<BitStreamRawRole, string> methodsByRole = new Dictionary<BitStreamRawRole, string>();
            ImmutableArray<DuplicateRawRoleInfo>.Builder duplicates = ImmutableArray.CreateBuilder<DuplicateRawRoleInfo>();
            foreach (IMethodSymbol? member in members) {
                AttributeData? attribute = member.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.Name == nameof(BitStreamRawAttribute));
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
                SpecialType.System_Byte    => "Byte",
                SpecialType.System_SByte   => "SByte",
                SpecialType.System_Int16   => "Short",
                SpecialType.System_UInt16  => "UShort",
                SpecialType.System_Int32   => "Int",
                SpecialType.System_UInt32  => "UInt",
                SpecialType.System_Int64   => "Long",
                SpecialType.System_UInt64  => "ULong",
                SpecialType.System_Single  => "Float",
                SpecialType.System_Double  => "Double",
                SpecialType.System_Decimal => "Decimal",
                SpecialType.System_String  => "String",
                SpecialType.System_Char    => "Char",
                
                // doupt these will ever get hit but trying to make this compleate
                SpecialType.System_DateTime => "DateTime",
                SpecialType.System_IntPtr  => "NInt",
                SpecialType.System_UIntPtr => "NUInt",
                SpecialType.System_Object  => "Object",
                SpecialType.System_Void    => "Void",
                
                _ => symbol.ToDisplayString(CSharpDefaultFormat)
            };
        }
        
        private static readonly SymbolDisplayFormat CSharpDefaultFormat = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
        );
    }
}
