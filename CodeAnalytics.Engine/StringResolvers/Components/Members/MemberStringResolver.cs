using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.StringResolvers;

namespace CodeAnalytics.Engine.StringResolvers.Components.Members;

public sealed class MemberStringResolver : IStringResolver<MemberComponent>
{
   public static ValueTask Resolve(Dictionary<int, string> result, MemberComponent input)
   {
      return ValueTask.CompletedTask;
   }
}