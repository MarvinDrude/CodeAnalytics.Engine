using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Extensions.Symbols;

public static class NamedTypeSymbolExtensions
{
   public static ImmutableArray<IFieldSymbol> GetFields(this INamedTypeSymbol symbol,
      Func<IFieldSymbol, bool> predicate)
   {
      return [
         .. symbol.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(predicate)
      ];
   }

   public static ImmutableArray<IMethodSymbol> GetMethods(this INamedTypeSymbol symbol,
      Func<IMethodSymbol, bool> predicate)
   {
      return [
         .. symbol.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(predicate)
      ];
   }

   public static ImmutableArray<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol,
      Func<IPropertySymbol, bool> predicate)
   {
      return [
         .. symbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(predicate)
      ];
   }
}