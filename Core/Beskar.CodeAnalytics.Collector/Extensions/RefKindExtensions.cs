using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class RefKindExtensions
{
   extension(RefKind refKind)
   {
      public RefType ToStorage()
      {
         return refKind switch
         {
            RefKind.None => RefType.None,
            RefKind.Ref => RefType.Ref,
            RefKind.Out => RefType.Out,
            RefKind.RefReadOnly => RefType.RefReadOnly,
            RefKind.RefReadOnlyParameter => RefType.RefReadOnlyParameter,
            _ => throw new ArgumentOutOfRangeException(nameof(refKind), refKind, null)
         };
      }
   }
}