using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Types;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Types;

public sealed class EnumArchetypeStringResolver : IStringResolver<EnumArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, EnumArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await EnumStringResolver.Resolve(result, input.Enum);
   }
}