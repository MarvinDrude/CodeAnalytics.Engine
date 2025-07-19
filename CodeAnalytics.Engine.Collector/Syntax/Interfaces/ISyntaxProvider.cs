namespace CodeAnalytics.Engine.Collector.Syntax.Interfaces;

public interface ISyntaxProvider
{
   public ISyntaxPredicator Predicator { get; }
   
   public ISyntaxTransformer Transformer { get; }
}