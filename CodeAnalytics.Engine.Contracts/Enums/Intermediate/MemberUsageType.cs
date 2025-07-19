namespace CodeAnalytics.Engine.Contracts.Enums.Intermediate;

public enum MemberUsageType : byte
{
   MethodInvocation = 1,
   PropertyGet = 2,
   PropertySet = 3,
   FieldRead = 4,
   FieldWrite = 5,
   Constructor = 6
}