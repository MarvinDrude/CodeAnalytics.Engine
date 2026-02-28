namespace Beskar.CodeAnalytics.Data.Enums.Symbols;

public enum AccessModifier : byte
{
   NotApplicable = 0,
   Private = 1,
   ProtectedAndFriend = 2,
   ProtectedAndInternal = ProtectedAndFriend,
   Protected = 3,
   Friend = 4,
   Internal = Friend,
   ProtectedOrFriend = 5,
   ProtectedOrInternal = ProtectedOrFriend,
   Public = 6
}