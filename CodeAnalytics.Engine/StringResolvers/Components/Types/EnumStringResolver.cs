using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class EnumStringResolver : IStringResolver<EnumComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref EnumComponent input)
   {
      return ValueTask.CompletedTask;
   }
}