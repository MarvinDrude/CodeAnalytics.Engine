using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class ConstructorStringResolver : IStringResolver<ConstructorComponent>
{
   public static ValueTask Resolve(Dictionary<int, string> result, ConstructorComponent input)
   {
      return ValueTask.CompletedTask;
   }
}