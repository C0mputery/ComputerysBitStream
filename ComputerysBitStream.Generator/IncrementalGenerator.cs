using System.Collections.Generic;
using System.Collections.Immutable;
using System.CodeDom.Compiler;
using System.IO;
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
        private static readonly DiagnosticDescriptor DuplicateTypeRule = new DiagnosticDescriptor(
            id: "BS001",
            title: "Duplicate BitStreamTypeAttribute",
            messageFormat: "The type '{0}' is already handled by another BitStreamTypeAttribute",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DuplicateRawRoleRule = new DiagnosticDescriptor(
            id: "BS002",
            title: "Duplicate BitStreamRawAttribute role",
            messageFormat: "The role '{0}' is specified more than once in '{1}' (first: '{2}', again: '{3}')",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
        
        private static readonly string ClassAttribute = typeof(BitStreamTypeAttribute).FullName!;
        private static readonly string MemberAttribute = typeof(BitStreamRawAttribute).FullName!;

        public void Initialize(IncrementalGeneratorInitializationContext context) {
            IncrementalValuesProvider<BitStreamTypeInfo?> pipeline = 
                context.SyntaxProvider.ForAttributeWithMetadataName(
                        fullyQualifiedMetadataName: ClassAttribute,
                        predicate: (node, _) => node is ClassDeclarationSyntax,
                        transform: Transform)
                .Where(info => info is not null);

            IncrementalValueProvider<ImmutableArray<BitStreamTypeInfo>> collected = pipeline.Collect()!;
            context.RegisterSourceOutput(collected, RegisterSourceOutputAction);
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
            List<DuplicateRawRoleInfo> duplicates = [];
            foreach (IMethodSymbol? member in members) {
                AttributeData? attribute = member.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == MemberAttribute);
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
                SpecialType.System_String => "String",
                _ => symbol.Name
            };
        }

        private static void RegisterSourceOutputAction(SourceProductionContext context, ImmutableArray<BitStreamTypeInfo> handlers) {
            Dictionary<string, BitStreamTypeInfo> handlersByTarget = new();
            foreach (BitStreamTypeInfo handler in handlers) {
                foreach (DuplicateRawRoleInfo duplicate in handler.DuplicateRoles) {
                    Location? location = null;
                    if (duplicate.Location.HasValue) {
                        location = Location.Create(duplicate.Location.Value.FilePath, duplicate.Location.Value.TextSpan, duplicate.Location.Value.LineSpan);
                    }
                    context.ReportDiagnostic(Diagnostic.Create(DuplicateRawRoleRule, location, duplicate.Role, duplicate.ClassName, duplicate.FirstMethod, duplicate.SecondMethod));
                }

                if (handlersByTarget.ContainsKey(handler.TargetTypeFullName)) {
                    Location? location = null;
                    BitStreamLocation? bitStreamLocation = handler.Location;
                    if (bitStreamLocation.HasValue) {   
                        location = Location.Create(bitStreamLocation.Value.FilePath, bitStreamLocation.Value.TextSpan, bitStreamLocation.Value.LineSpan);
                    }
                    
                    Diagnostic diagnostic = Diagnostic.Create(DuplicateTypeRule, location, handler.TargetTypeFullName);
                    context.ReportDiagnostic(diagnostic);
                } else {
                    handlersByTarget[handler.TargetTypeFullName] = handler;
                }
            }
            
            handlersByTarget.TryGetValue(SyntaxFacts.GetText(SyntaxKind.IntKeyword), out BitStreamTypeInfo? intHandler);
            foreach (BitStreamTypeInfo handler in handlersByTarget.Values) {
                string source = BitStreamSourceEmitter.EmitSource(handler, intHandler);
                context.AddSource($"{handler.TargetTypeName}ContextExtensions.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
    }
}
