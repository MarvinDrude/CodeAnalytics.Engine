using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Members;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Members;

public sealed class MethodArchetypeStringResolver : IStringResolver<MethodArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, MethodArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await MemberStringResolver.Resolve(result, input.Member);
      await MethodStringResolver.Resolve(result, input.Method);
   }
}