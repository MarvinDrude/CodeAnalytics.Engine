namespace Beskar.CodeAnalytics.Data.Enums.Symbols;

public enum RefType : byte
{
   None = 0,
   Ref = 1,
   Out = 2,
   In = 3,
   RefReadOnly = In,
   RefReadOnlyParameter = 4
}