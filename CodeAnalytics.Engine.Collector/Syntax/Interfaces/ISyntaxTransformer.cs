using CodeAnalytics.Engine.Collector.Collectors.Contexts;

namespace CodeAnalytics.Engine.Collector.Syntax.Interfaces;

public interface ISyntaxTransformer
{
   public void Transform(CollectContext context);
}