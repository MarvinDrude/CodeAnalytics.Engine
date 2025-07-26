using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class PropertyStringResolver : IStringResolver<PropertyComponent>
{
   public static ValueTask Resolve(Dictionary<int, string> result, PropertyComponent input)
   {
      return ValueTask.CompletedTask;
   }
}