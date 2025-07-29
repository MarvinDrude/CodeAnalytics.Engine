using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Extensions.Symbols;

public static class MethodSymbolExtensions
{
   public static List<IMethodSymbol> GetImplementedInterfaceMembers(this IMethodSymbol impl)
   {
      return impl.ExplicitOrImplicitInterfaceImplementations()
         .OfType<IMethodSymbol>()
         .ToList();
   }
}