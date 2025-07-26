
namespace CodeAnalytics.Engine.Contracts.StringResolvers;

public interface IStringResolver<in TType>
{
   public static abstract ValueTask Resolve(Dictionary<int, string> result, TType input);
}