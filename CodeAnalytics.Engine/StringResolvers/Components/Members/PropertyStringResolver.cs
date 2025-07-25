using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class PropertyStringResolver : IStringResolver<PropertyComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref PropertyComponent input)
   {
      return ValueTask.CompletedTask;
   }
}