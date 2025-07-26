using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Types;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Types;

public sealed class InterfaceArchetypeStringResolver : IStringResolver<InterfaceArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, InterfaceArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await TypeStringResolver.Resolve(result, input.Type);
      await InterfaceStringResolver.Resolve(result, input.Interface);
   }
}