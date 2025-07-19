using CodeAnalytics.Engine.Contracts.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Enums.Symbols;

public static class AccessModifierMapper
{
   public static AccessModifier Map(this Accessibility access)
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