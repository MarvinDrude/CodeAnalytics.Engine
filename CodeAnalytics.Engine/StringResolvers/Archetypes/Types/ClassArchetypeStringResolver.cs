using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Members;
using CodeAnalytics.Engine.StringResolvers.Components.Types;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Types;

public sealed class ClassArchetypeStringResolver : IStringResolver<ClassArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, ClassArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await TypeStringResolver.Resolve(result, input.Type);
      await ClassStringResolver.Resolve(result, input.Class);
   }
}