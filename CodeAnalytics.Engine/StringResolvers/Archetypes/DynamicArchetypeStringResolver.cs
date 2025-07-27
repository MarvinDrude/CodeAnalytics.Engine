using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Archetypes.Members;
using CodeAnalytics.Engine.StringResolvers.Archetypes.Types;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes;

public sealed class DynamicArchetypeStringResolver : IStringResolver<List<IArchetype>>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, List<IArchetype> input)
   {
      foreach (var archetype in input)
      {
         switch (archetype)
         {
            case PropertyArchetype arch:
               await PropertyArchetypeStringResolver.Resolve(result, arch);
               break;
            case FieldArchetype arch:
               await FieldArchetypeStringResolver.Resolve(result, arch);
               break;
            case MethodArchetype arch:
               await MethodArchetypeStringResolver.Resolve(result, arch);
               break;
            case ConstructorArchetype arch:
               await ConstructorArchetypeStringResolver.Resolve(result, arch);
               break;
            
            case ClassArchetype arch:
               await ClassArchetypeStringResolver.Resolve(result, arch);
               break;
            case InterfaceArchetype arch:
               await InterfaceArchetypeStringResolver.Resolve(result, arch);
               break;
            case EnumArchetype arch:
               await EnumArchetypeStringResolver.Resolve(result, arch);
               break;
            case EnumValueArchetype arch:
               await EnumValueArchetypeStringResolver.Resolve(result, arch);
               break;
            case StructArchetype arch:
               await StructArchetypeStringResolver.Resolve(result, arch);
               break;
         }
      }
   }
}