using System.Collections.Concurrent;
using CodeAnalytics.Engine.Collectors.Caches;
using CodeAnalytics.Engine.Collectors.Extensions.Database;
using CodeAnalytics.Engine.Collectors.Options;
using CodeAnalytics.Engine.Storage.Common;
using CodeAnalytics.Engine.Storage.Models.Structure;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalytics.Engine.Collectors.Models.Contexts;

public sealed class CollectContext
{
   public required Compilation Compilation { get; set; }
   
   public required SourceText SourceText { get; set; }
   public required SemanticModel SemanticModel { get; set; }
   
   public required Document Document { get; set; }
   public required CancellationToken CancellationToken { get; set; }
   
   public required SyntaxTree SyntaxTree { get; set; }
   public required SyntaxNode SyntaxNode { get; set; }
   
   public required CollectorOptions Options { get; set; }
   public required DbMainContext DbContext { get; set; }
   
   public required DbSolution DbSolution { get; set; }
   public required DbProject DbProject { get; set; }
   public required DbFile DbFile { get; set; }
   
   public required SymbolIdCache SymbolIdCache { get; set; }
   
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
      
      return SyntaxNode switch
      {
         OrderingSyntax orderingSyntax => SemanticModel.GetSymbolInfo(orderingSyntax, ct).Symbol,
         SelectOrGroupClauseSyntax sogSyntax => SemanticModel.GetSymbolInfo(sogSyntax, ct).Symbol,
         ExpressionSyntax expressionSyntax => SemanticModel.GetSymbolInfo(expressionSyntax, ct).Symbol,
         ConstructorInitializerSyntax constructorSyntax => SemanticModel.GetSymbolInfo(constructorSyntax, ct).Symbol,
         PrimaryConstructorBaseTypeSyntax primarySyntax => SemanticModel.GetSymbolInfo(primarySyntax, ct).Symbol,
         AttributeSyntax attributeSyntax => SemanticModel.GetSymbolInfo(attributeSyntax, ct).Symbol,
         CrefSyntax crefSyntax => SemanticModel.GetSymbolInfo(crefSyntax, ct).Symbol,
         _ => null
      };
   }

   public ValueTask<DbSymbolId> GetDbSymbolId(string hashId)
   {
      return SymbolIdCache.TryGetId(hashId, out var dbSymbolId) 
         ? ValueTask.FromResult(dbSymbolId) 
         : AwaitDbSymbolId(hashId);
   }

   private async ValueTask<DbSymbolId> AwaitDbSymbolId(string hashId)
   {
      var symbolId = await DbContext.GetSymbolId(hashId);
      
      if (symbolId != DbSymbolId.Empty)
      {
         SymbolIdCache.Set(hashId, symbolId);
      }
      
      return symbolId;
   }
}