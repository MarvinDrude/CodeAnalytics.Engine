using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Extensions;

public static class AccessibilityExtensions
{
   extension(Accessibility access)
   {
      public AccessModifier ToAccessModifier()
      {
         return access switch
         {
            Accessibility.NotApplicable => AccessModifier.NotApplicable,
            Accessibility.Private => AccessModifier.Private,
            Accessibility.Internal => AccessModifier.Internal,
            Accessibility.Protected => AccessModifier.Protected,
            Accessibility.ProtectedAndInternal => AccessModifier.ProtectedInternal,
            Accessibility.ProtectedOrInternal => AccessModifier.ProtectedOrInternal,
            Accessibility.Public => AccessModifier.Public,
            _ => AccessModifier.NotApplicable,
         };
      }
   }
}