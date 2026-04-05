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
            IncrementalValuesProvider<BitStreamTypeInfo> handlers = 
                context.SyntaxProvider.ForAttributeWithMetadataName(
                        fullyQualifiedMetadataName: ClassAttribute,
                        predicate: (node, _) => node is ClassDeclarationSyntax,
                        transform: Transform)
                .Where(info => info is not null)
                .Select((info, _) => info!);

            IncrementalValueProvider<ImmutableArray<BitStreamTypeInfo>> collectedHandlers = handlers.Collect();
            IncrementalValueProvider<ValidationResult> validation = collectedHandlers.Select((allHandlers, _) => ValidateHandlers(allHandlers));

            IncrementalValuesProvider<Diagnostic> diagnostics = validation.SelectMany((result, _) => result.Diagnostics);
            context.RegisterSourceOutput(diagnostics, (sourceContext, diagnostic) => sourceContext.ReportDiagnostic(diagnostic));

            IncrementalValuesProvider<BitStreamTypeInfo> uniqueHandlers = validation.SelectMany((result, _) => result.UniqueHandlers);

            IncrementalValueProvider<BitStreamTypeInfo?> intHandlerProvider = uniqueHandlers
                .Where(info => info.TargetTypeFullName == SyntaxFacts.GetText(SyntaxKind.IntKeyword))
                .Collect()
                .Select((bitStreamTypeInfos, _) => bitStreamTypeInfos.Length > 0 ? (BitStreamTypeInfo?)bitStreamTypeInfos[0] : null);
            
            IncrementalValuesProvider<(BitStreamTypeInfo handler, BitStreamTypeInfo? intHandler)> combined = uniqueHandlers.Combine(intHandlerProvider);
            
            context.RegisterSourceOutput(combined, (sourceContext, pair) => {
                string source = BitStreamSourceEmitter.EmitSource(pair.handler, pair.intHandler);
                sourceContext.AddSource($"{pair.handler.TargetTypeName}ContextExtensions.g.cs", SourceText.From(source, Encoding.UTF8));
            });
        }

        private static ValidationResult ValidateHandlers(ImmutableArray<BitStreamTypeInfo> handlers) {
            Dictionary<string, BitStreamTypeInfo> firstByTarget = new Dictionary<string, BitStreamTypeInfo>();
            ImmutableArray<Diagnostic>.Builder diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            ImmutableArray<BitStreamTypeInfo>.Builder uniqueHandlers = ImmutableArray.CreateBuilder<BitStreamTypeInfo>();

            foreach (BitStreamTypeInfo handler in handlers) {
                foreach (DuplicateRawRoleInfo duplicate in handler.DuplicateRoles) {
                    diagnostics.Add(DiagnosticDescriptors.CreateDuplicateRawRole(duplicate));
                }
                foreach (NonPublicRawMethodInfo nonPublicRawMethod in handler.NonPublicRawMethods) {
                    diagnostics.Add(DiagnosticDescriptors.CreateNonPublicRawMethod(nonPublicRawMethod));
                }

                if (firstByTarget.ContainsKey(handler.TargetTypeFullName)) {
                    diagnostics.Add(DiagnosticDescriptors.CreateDuplicateType(handler));
                    continue;
                }

                firstByTarget[handler.TargetTypeFullName] = handler;
                uniqueHandlers.Add(handler);
            }

            return new ValidationResult(uniqueHandlers.ToImmutable(), diagnostics.ToImmutable());
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
            ImmutableArray<NonPublicRawMethodInfo>.Builder nonPublicRawMethods = ImmutableArray.CreateBuilder<NonPublicRawMethodInfo>();
            foreach (IMethodSymbol? member in members) {
                AttributeData? attribute = member.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.Name == nameof(BitStreamRawAttribute));
                if (attribute?.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int roleValue) {
                    BitStreamRawRole role = (BitStreamRawRole)roleValue;

                    if (member.DeclaredAccessibility != Accessibility.Public) {
                        nonPublicRawMethods.Add(new NonPublicRawMethodInfo(
                            Role: role.ToString(),
                            ClassName: classSymbol.Name,
                            MethodName: member.Name,
                            Accessibility: member.DeclaredAccessibility.ToString(),
                            Location: attribute.ApplicationSyntaxReference?.GetSyntax(cancel).GetLocation()
                        ));
                    }

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
                RawMethods: new RawRoleBindings(methodsByRole),
                Location: classAttributeData.ApplicationSyntaxReference?.GetSyntax(cancel).GetLocation(),
                DuplicateRoles: duplicates.ToImmutableArray(),
                NonPublicRawMethods: nonPublicRawMethods.ToImmutableArray()
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

        private readonly record struct ValidationResult(
            ImmutableArray<BitStreamTypeInfo> UniqueHandlers,
            ImmutableArray<Diagnostic> Diagnostics
        );
    }
}
