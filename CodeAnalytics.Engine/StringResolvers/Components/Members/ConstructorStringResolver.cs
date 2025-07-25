using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class ConstructorStringResolver : IStringResolver<ConstructorComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref ConstructorComponent input)
   {
      return ValueTask.CompletedTask;
   }
}