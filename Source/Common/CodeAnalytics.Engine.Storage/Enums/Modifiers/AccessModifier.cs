namespace CodeAnalytics.Engine.Storage.Enums.Modifiers;

public enum AccessModifier : byte
{
   NotApplicable = 0,
   Private = 1,
   ProtectedInternal = 2,
   Protected = 3,
   Internal = 4,
   ProtectedOrInternal = 5,
   Public = 6
}