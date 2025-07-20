namespace CodeAnalytics.Engine.Analyze.Analyzers.Interfaces;

public interface IAnalyzer<out TResult>
{
   public TResult Analyze(AnalyzeStore store);
}