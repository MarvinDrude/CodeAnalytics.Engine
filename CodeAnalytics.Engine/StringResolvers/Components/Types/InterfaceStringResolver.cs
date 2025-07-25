using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class InterfaceStringResolver : IStringResolver<InterfaceComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref InterfaceComponent input)
   {
      return ValueTask.CompletedTask;
   }
}