using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Contracts.Ids;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalytics.Engine.Collector.Collectors.Contexts;

public sealed class CollectContext
{
   public required Compilation Compilation { get; set; }
   public required SourceText SourceText { get; set; }
   public required SemanticModel SemanticModel { get; set; }
   public required Document Document { get; set; }
   
   public required SyntaxTree SyntaxTree { get; set; }
   public required SyntaxNode SyntaxNode { get; set; }
   
   public required ProjectOptions Options { get; set; }
   public required CollectorStore Store { get; set; }
   
   public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
   public bool AddSubComponentsImmediately { get; set; } = true;
   
   public StringId ProjectId { get; set; } = StringId.Empty;
   public StringId FileId { get; set; } = StringId.Empty;

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