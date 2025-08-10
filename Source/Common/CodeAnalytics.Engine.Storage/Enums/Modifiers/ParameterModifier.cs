namespace CodeAnalytics.Engine.Storage.Enums.Modifiers;

[Flags]
public enum ParameterModifier : int
{
   None = 0,
   This = 1 << 0,
   Ref = 1 << 1,
   Out = 1 << 2,
   In = 1 << 3,
   Scoped =  1 << 4,
   Params =  1 << 5,
   ReadOnly =  1 << 6,
}