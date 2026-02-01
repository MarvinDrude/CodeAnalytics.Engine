namespace Beskar.CodeAnalytics.Storage.Entities.Edges;

public enum EdgeType : byte
{
   DirectInterface = 1,
   AllInterface = 2,
   Parameter = 3,
   TypeParameter = 4,
   InstanceConstructor = 5,
   StaticConstructor = 6,
   Method = 7,
   ConstraintType = 8
}