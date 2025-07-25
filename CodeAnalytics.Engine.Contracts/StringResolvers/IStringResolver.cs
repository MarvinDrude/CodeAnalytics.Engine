
namespace CodeAnalytics.Engine.Contracts.StringResolvers;

public interface IStringResolver<TType>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref TType input);
}