using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
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
            INamedTypeSymbol? restrictedAttribute = startContext.Compilation.GetTypeByMetadataName(BitStreamTypeNames.RestrictedPrimitiveMethod);
            if (restrictedAttribute is null) { return; }
            startContext.RegisterSyntaxNodeAction(analysisContext => AnalyzeInvocation(analysisContext, restrictedAttribute), SyntaxKind.InvocationExpression);
        });
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context, INamedTypeSymbol restrictedAttribute) {
        if (IsAllowedCaller(context.ContainingSymbol)) { return; }

        InvocationExpressionSyntax invocation = (InvocationExpressionSyntax)context.Node;
        if (!TryGetInvokedMethod(context.SemanticModel, invocation, context.CancellationToken, out IMethodSymbol? targetMethod)) { return; }
        if (!targetMethod.HasRestrictedPrimitiveMethodAttribute(restrictedAttribute)) { return; }

        context.ReportDiagnostic(Diagnostic.Create(Diagnostics.PrimitiveMethodCalledOutsidePrimitive, invocation.GetLocation(), targetMethod.Name));
    }

    private static bool TryGetInvokedMethod(SemanticModel semanticModel, InvocationExpressionSyntax invocation, System.Threading.CancellationToken cancellationToken, [NotNullWhen(true)] out IMethodSymbol? targetMethod) {
        SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(invocation, cancellationToken);
        if (symbolInfo.Symbol is not IMethodSymbol method) {
            targetMethod = null;
            return false;
        }

        targetMethod = method.ReducedFrom ?? method;
        return true;
    }

    private static bool IsAllowedCaller(ISymbol? containingSymbol) {
        return containingSymbol.IsInTypeWithAttribute(BitStreamTypeNames.Primitive) || containingSymbol.IsInTypeWithAttribute(BitStreamTypeNames.PrimitiveContext);
    }
}
