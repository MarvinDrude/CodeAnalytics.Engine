using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Storage.Contexts;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using ProjectReference = CodeAnalytics.Engine.Storage.Models.Structure.ProjectReference;

namespace CodeAnalytics.Engine.Collector.Collectors.Contexts;

public sealed class CollectContext
{
   public required Compilation Compilation { get; set; }
   
   public required SourceText SourceText { get; set; }
   public required SemanticModel SemanticModel { get; set; }
   
   public required Document Document { get; set; }
   public required CancellationToken CancellationToken { get; set; }
   
   public required SyntaxTree SyntaxTree { get; set; }
   public required SyntaxNode SyntaxNode { get; set; }

   public required ProjectOptions Options { get; set; }
   
   public required DbMainContext DbMainContext { get; set; }
   
   public required SolutionReference SolutionReference { get; set; }
   public required ProjectReference ProjectReference { get; set; }
   public required FileReference FileReference { get; set; }
   
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
   
   public string GetRelativePath(string absolutePath)
   {
      return Path.GetRelativePath(Options.BasePath, absolutePath);
   }
}