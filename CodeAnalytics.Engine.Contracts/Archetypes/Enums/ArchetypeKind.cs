namespace CodeAnalytics.Engine.Contracts.Archetypes.Enums;

public enum ArchetypeKind : byte
{
   Class = 1,
   Interface = 2,
   Struct = 3,
   Enum = 4,
   
   Method = 5,
   Property = 6,
   Field = 7,
   Constructor = 8,
}