using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.StringResolvers.Components.Common;
using CodeAnalytics.Engine.StringResolvers.Components.Members;

namespace CodeAnalytics.Engine.StringResolvers.Archetypes.Members;

public sealed class ConstructorArchetypeStringResolver : IStringResolver<ConstructorArchetype>
{
   public static async ValueTask Resolve(Dictionary<int, string> result, ConstructorArchetype input)
   {
      await SymbolStringResolver.Resolve(result, input.Symbol);
      await MemberStringResolver.Resolve(result, input.Member);
      await ConstructorStringResolver.Resolve(result, input.Constructor);
   }
}