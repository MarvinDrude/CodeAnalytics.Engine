namespace Beskar.CodeAnalytics.Data.Enums.Symbols;

public enum SymbolEdgeType : byte
{
   DirectInterface = 1,
   AllInterface = 2,
   Parameter = 3,
   TypeParameter = 4,
   InstanceConstructor = 5,
   StaticConstructor = 6,
   Method = 7,
   ConstraintType = 8,
   
   SolutionProject = 200,
   ProjectReference = 201,
   ProjectFile = 202,
   ProjectSolution = 203,
}