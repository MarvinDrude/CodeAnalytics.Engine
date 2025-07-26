using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class StructStringResolver : IStringResolver<StructComponent>
{
   public static ValueTask Resolve(Dictionary<int, string> result, StructComponent input)
   {
      return ValueTask.CompletedTask;
   }
}