using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class ScopeKindExtensions
{
   extension(ScopedKind kind)
   {
      public ScopeType ToStorage()
      {
         return kind switch
         {
            ScopedKind.None => ScopeType.None,
            ScopedKind.ScopedRef => ScopeType.ScopedRef,
            ScopedKind.ScopedValue => ScopeType.ScopedValue,
            _ => ScopeType.None
         };
      }
   }
}