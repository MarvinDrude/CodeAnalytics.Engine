using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;

namespace CodeAnalytics.Engine.Utils;

public static class ColorUtils
{
   public static string GetBaseColorVariableName(IArchetype archetype)
   {
      return archetype switch
      {
         ClassArchetype => "var(--class-color",
         StructArchetype => "var(--struct-color",
         EnumArchetype => "var(--enum-color",
         InterfaceArchetype => "var(--interface-color",
         
         MethodArchetype => "var(--method-color",
         PropertyArchetype => "var(--property-color",
         FieldArchetype => "var(--field-color",
         ConstructorArchetype => "var(--constructor-color",
         _ => "",
      };
   }
}