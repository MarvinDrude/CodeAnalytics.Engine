using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Extensions.Symbols;

public static class SymbolExtensions
{
   extension<TSymbol>(TSymbol symbol)
      where TSymbol : ISymbol
   {
      public string? GenerateUniqueId()
      {
         if (!SymbolKey.CanCreate(symbol))
         {
            
         }
      }

      public bool IsBodyLevelSymbol()
      {
         return symbol switch
         {
            ILabelSymbol 
               or IRangeVariableSymbol
               or ILocalSymbol 
               or IMethodSymbol { MethodKind: MethodKind.LocalFunction } => true,
            _ => false
         };
      }
   }
}