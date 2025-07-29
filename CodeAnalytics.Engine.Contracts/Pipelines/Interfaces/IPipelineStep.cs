namespace CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

public interface IPipelineStep<in TInput, TOutput>
{
   public string Name { get; }

   public ValueTask<TOutput> Execute(TInput input, CancellationToken ct = default);
}