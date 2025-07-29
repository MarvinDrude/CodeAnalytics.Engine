namespace CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

public interface IAnalyzePipeline<in TInput, TOutput>
{
   public ValueTask<TOutput> Execute(TInput input, CancellationToken ct = default);
}