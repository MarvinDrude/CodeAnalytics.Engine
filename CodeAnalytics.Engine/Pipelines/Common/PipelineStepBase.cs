using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

namespace CodeAnalytics.Engine.Pipelines.Common;

public abstract class PipelineStepBase<TInput, TOutput> : IPipelineStep<TInput, TOutput>
{
   public string Name { get; }

   private readonly IPipelineCacheProvider _cacheProvider;
   private readonly bool _useCache;

   protected PipelineStepBase(
      string? name,
      IPipelineCacheProvider cacheProvider, 
      bool useCache)
   {
      Name = name ?? Guid.CreateVersion7().ToString();
      
      _cacheProvider = cacheProvider;
      _useCache = useCache;
   }
   
   public async ValueTask<TOutput> Execute(TInput input, CancellationToken ct = default)
   {
      if (_useCache && _cacheProvider.TryGet<TOutput>(Name, out var cached))
      {
         return cached;
      }
      
      var result = await Process(input, ct);

      if (_useCache)
      {
         _cacheProvider.Set(Name, result);
      }

      return result;
   }
   
   protected abstract ValueTask<TOutput> Process(TInput input, CancellationToken ct = default);
}