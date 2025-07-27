using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class EnumValueStringResolver : IStringResolver<EnumValueComponent>
{
   public static ValueTask Resolve(Dictionary<int, string> result, EnumValueComponent input)
   {
      input.Name.Resolve(result);
      return ValueTask.CompletedTask;
   }
}