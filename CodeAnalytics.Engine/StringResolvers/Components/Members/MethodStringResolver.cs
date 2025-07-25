using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class MethodStringResolver : IStringResolver<MethodComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref MethodComponent input)
   {
      return ValueTask.CompletedTask;
   }
}