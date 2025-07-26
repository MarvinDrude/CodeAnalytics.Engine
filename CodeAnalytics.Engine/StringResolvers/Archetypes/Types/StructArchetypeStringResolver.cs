using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Types;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Types;

public sealed class StructArchetypeStringResolver : IStringResolver<StructArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, StructArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await TypeStringResolver.Resolve(result, input.Type);
      await StructStringResolver.Resolve(result, input.Struct);
   }
}