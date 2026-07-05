using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ComputerysBitStream.Generator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PrimitiveMethodUsageAnalyzer : DiagnosticAnalyzer {
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Diagnostics.PrimitiveMethodCalledOutsidePrimitive);

    public override void Initialize(AnalysisContext context) {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(startContext => {
            INamedTypeSymbol? restrictedAttribute = startContext.Compilation.GetTypeByMetadataName(BitStreamMetadataNames.RestrictedPrimitiveMethod);
            if (restrictedAttribute is null) { return; }

            startContext.RegisterSyntaxNodeAction(analysisContext => AnalyzeInvocation(analysisContext, restrictedAttribute), SyntaxKind.InvocationExpression);
        });
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context, INamedTypeSymbol restrictedAttribute) {
        if (IsAllowedCaller(context.ContainingSymbol)) { return; }

        InvocationExpressionSyntax invocation = (InvocationExpressionSyntax)context.Node;
        IMethodSymbol? targetMethod = GetInvokedMethod(context.SemanticModel, invocation, context.CancellationToken);
        if (targetMethod is null || !targetMethod.HasRestrictedPrimitiveMethodAttribute(restrictedAttribute)) { return; }

        context.ReportDiagnostic(Diagnostic.Create(Diagnostics.PrimitiveMethodCalledOutsidePrimitive, invocation.GetLocation(), targetMethod.Name));
    }

    private static IMethodSymbol? GetInvokedMethod(SemanticModel semanticModel, InvocationExpressionSyntax invocation, System.Threading.CancellationToken cancellationToken) {
        SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(invocation, cancellationToken);
        if (symbolInfo.Symbol is not IMethodSymbol method) { return null; }

        return method.ReducedFrom ?? method;
    }

    private static bool IsAllowedCaller(ISymbol? containingSymbol) {
        return containingSymbol.IsInTypeWithAttribute(BitStreamMetadataNames.Primitive) || containingSymbol.IsInTypeWithAttribute(BitStreamMetadataNames.PrimitiveContext);
    }
}
