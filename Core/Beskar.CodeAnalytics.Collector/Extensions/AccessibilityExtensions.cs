using Beskar.CodeAnalytics.Storage.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class AccessibilityExtensions
{
   extension(Accessibility accessibility)
   {
      public AccessModifier ToStorage()
      {
         return accessibility switch
         {
            Accessibility.NotApplicable => AccessModifier.NotApplicable,
            Accessibility.Protected => AccessModifier.Protected,
            Accessibility.ProtectedAndInternal => AccessModifier.ProtectedAndInternal,
            Accessibility.ProtectedOrInternal => AccessModifier.ProtectedOrInternal,
            Accessibility.Public => AccessModifier.Public,
            Accessibility.Internal => AccessModifier.Internal,
            Accessibility.Private => AccessModifier.Private,
            _ => throw new InvalidOperationException()
         };
      }
   }
}