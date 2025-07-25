using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class ClassStringResolver : IStringResolver<ClassComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref ClassComponent input)
   {
      return ValueTask.CompletedTask;
   }
}