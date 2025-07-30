using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

namespace CodeAnalytics.Engine.Pipelines.Common;

public sealed class AnalyzePipeline<TInput, TOutput> : IAnalyzePipeline<TInput, TOutput>
{
   private readonly Func<TInput, CancellationToken, ValueTask<TOutput>> _executer;

   internal AnalyzePipeline(Func<TInput, CancellationToken, ValueTask<TOutput>> executer)
   {
      _executer = executer;
   }
   
   public ValueTask<TOutput> Execute(TInput input, CancellationToken ct = default)
   {
      return _executer.Invoke(input, ct);
   }

   public AnalyzePipeline<TInput, TNext> AddStep<TNext>(IPipelineStep<TOutput, TNext> step)
   {
      return new AnalyzePipeline<TInput, TNext>(async (input, ct) =>
      {
         var result = await _executer.Invoke(input, ct);
         return await step.Execute(result, ct);
      });
   }

   public static AnalyzePipeline<TInput, TOutput> Create(IPipelineStep<TInput, TOutput> firstStep)
   {
      return new AnalyzePipeline<TInput, TOutput>(firstStep.Execute);
   }
}