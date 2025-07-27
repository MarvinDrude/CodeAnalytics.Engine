using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;

namespace CodeAnalytics.Engine.Extensions.Internal;

public static class ArchetypeExtensions
{
   public static string GetDisplayName(this IArchetype archetype)
   {
      return archetype switch
      {
         ClassArchetype => "Class",
         StructArchetype => "Struct",
         EnumArchetype => "Enum",
         EnumValueArchetype => "Enum Value",
         InterfaceArchetype => "Interface",
         
         MethodArchetype => "Method",
         PropertyArchetype => "Property",
         FieldArchetype => "Field",
         ConstructorArchetype => "Constructor",
         _ => "",
      };
   }
}