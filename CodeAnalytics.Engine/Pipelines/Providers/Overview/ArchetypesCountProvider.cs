using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Analyze.Interfaces;
using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Pipelines.Common;
using CodeAnalytics.Engine.Pipelines.Steps.Common;
using CodeAnalytics.Engine.Pipelines.Steps.Overview;

namespace CodeAnalytics.Engine.Pipelines.Providers.Overview;

public sealed class ArchetypesCountProvider
{
   private readonly IAnalyzeStoreProvider _storeProvider;
   private readonly IPipelineCacheProvider _cacheProvider;
   
   public ArchetypesCountProvider(
      IAnalyzeStoreProvider storeProvider,
      IPipelineCacheProvider cacheProvider)
   {
      _storeProvider = storeProvider;
      _cacheProvider = cacheProvider;
   }
   
   public async ValueTask<ArchetypesCountResult> Run(PipelineParameters parameters)
   {
      var store = await _storeProvider.GetStore();
      return await AnalyzePipeline<AnalyzeStore, Dictionary<StringId, ArchetypeChunkViews>>
         .Create(new ArchetypesPerProjectStep(_cacheProvider, true))
         .AddStep(new ArchetypesCountStep(store, parameters, _cacheProvider, false))
         .Execute(store);
   }
}