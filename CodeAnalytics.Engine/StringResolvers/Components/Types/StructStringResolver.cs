using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class StructStringResolver : IStringResolver<StructComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref StructComponent input)
   {
      return ValueTask.CompletedTask;
   }
}