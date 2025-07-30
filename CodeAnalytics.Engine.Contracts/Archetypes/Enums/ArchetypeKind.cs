namespace CodeAnalytics.Engine.Contracts.Archetypes.Enums;

public enum ArchetypeKind : byte
{
   Unknown = 0,
   
   Class = 1,
   Interface = 2,
   Struct = 3,
   Enum = 4,
   EnumValue = 5,
   
   Method = 6,
   Property = 7,
   Field = 8,
   Constructor = 9,
}