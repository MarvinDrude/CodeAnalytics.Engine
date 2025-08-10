namespace CodeAnalytics.Engine.Storage.Enums.Components;

public enum ComponentKind : byte
{
   Unknown = 0,
   
   Constructor = 1,
   EnumValue = 2,
   Field = 3,
   Method = 4,
   Property = 5,
   
   Class = 6,
   Enum = 7,
   Interface = 8,
   Struct = 9,
}