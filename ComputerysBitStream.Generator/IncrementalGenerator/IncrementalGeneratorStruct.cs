/*using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComputerysBitStream.Generator;

public partial class IncrementalGenerator : IIncrementalGenerator {
    private static readonly string StructAttribute = typeof(BitStreamStructAttribute).FullName!;
    private static readonly string IncludeAttribute = typeof(BitStreamStructIncludeAttribute).FullName!;
    private static readonly string ExcludeAttribute = typeof(BitStreamStructIgnoreAttribute).FullName!;

    private static IncrementalValuesProvider<StructAttributeData> GetStructAttributes(IncrementalGeneratorInitializationContext context) {
        return context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: StructAttribute,
            predicate: (SyntaxNode node, CancellationToken _) => node is StructDeclarationSyntax,
            transform: GetStructAttributesTransform);
    }

    private static StructAttributeData GetStructAttributesTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancel) {
        
    }
}*/