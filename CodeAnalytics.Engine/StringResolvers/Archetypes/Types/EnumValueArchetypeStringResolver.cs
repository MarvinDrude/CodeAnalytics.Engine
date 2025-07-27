using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Types;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Types;

public sealed class EnumValueArchetypeStringResolver : IStringResolver<EnumValueArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, EnumValueArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await EnumValueStringResolver.Resolve(result, input.EnumValue);
   }
}