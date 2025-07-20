using CodeAnalytics.Engine.Extensions.System;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Extensions.Symbols;

public static class ParameterSymbolExtensions
{
   public static string GenerateParameterId(this IParameterSymbol symbol, IMethodSymbol method)
   {
      return $"{method.GenerateSeedId()}::{symbol.GenerateSeedId()}".GenerateId();
   }
}