using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class FieldStringResolver : IStringResolver<FieldComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref FieldComponent input)
   {
      return ValueTask.CompletedTask;
   }
}