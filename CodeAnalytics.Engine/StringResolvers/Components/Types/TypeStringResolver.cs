using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Types;

public sealed class TypeStringResolver : IStringResolver<TypeComponent>
{
   public static ValueTask Resolve(Dictionary<int, string> result, TypeComponent input)
   {
      return ValueTask.CompletedTask;
   }
}