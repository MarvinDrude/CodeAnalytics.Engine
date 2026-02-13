using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed class DiscoverContext
{
   public required CsProjectHandle ProjectHandle { get; set; }
   public Compilation Compilation => ProjectHandle.Compilation;
   
   public required SourceText SourceText { get; set; }
   
   public required SemanticModel SemanticModel { get; set; }
   
   public Document? Document { get; set; }
   
   public required SyntaxTree SyntaxTree { get; set; }
   
   public required SyntaxNode SyntaxNode { get; set; }
   
   public CancellationToken CancellationToken { get; set; }
   
   public required DiscoveryBatch DiscoveryBatch { get; set; }

   public void ResetSymbol()
   {
      _fetched = false;
      _symbol = null;
   }
   
   public ISymbol? Symbol
   {
      get
      {
         if (_fetched) return _symbol;
         
         _symbol = GetSymbol();
         _fetched = true;
         
         return _symbol;
      }
   }

   private ISymbol? _symbol;
   private bool _fetched;

   public ISymbol? GetSymbol(CancellationToken ct = default)
   {
      if (SyntaxNode is ICompilationUnitSyntax)
      {
         return Compilation.Assembly;
      }

      if (SemanticModel.GetDeclaredSymbol(SyntaxNode, ct) is { } symbol)
      {
         return symbol;
      }
      
      return SemanticModel.GetSymbolInfo(SyntaxNode, ct).Symbol;
   }
}