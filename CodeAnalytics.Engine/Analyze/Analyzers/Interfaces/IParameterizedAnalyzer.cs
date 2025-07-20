namespace CodeAnalytics.Engine.Analyze.Analyzers.Interfaces;

public interface IParameterizedAnalyzer<in TParameters, out TResult>
{
   public TResult Analyze(AnalyzeStore store, TParameters parameters);
}