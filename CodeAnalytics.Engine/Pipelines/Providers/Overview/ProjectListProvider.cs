using CodeAnalytics.Engine.Analyze.Interfaces;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Pipelines.Common;

namespace CodeAnalytics.Engine.Pipelines.Providers.Overview;

public sealed class ProjectListProvider : PipelineProviderBase, IPipelineProvider
{
   private readonly IAnalyzeStoreProvider _storeProvider;
   private readonly IPipelineCacheProvider _cacheProvider;
   
   public ProjectListProvider(
      IAnalyzeStoreProvider storeProvider,
      IPipelineCacheProvider cacheProvider)
   {
      _storeProvider = storeProvider;
      _cacheProvider = cacheProvider;
   }
   
   public async ValueTask<ProjectListResult> Run(PipelineParameters parameters)
   {
      var store = await _storeProvider.GetStore();
      return new ProjectListResult()
      {
         Projects = store.Inner.Projects.Select(x => new ProjectListEntry()
         {
            Id = x,
            ProjectName = x.ToString()
         }).ToHashSet()
      };
   }
   
   public async ValueTask<string> RunRawString(PipelineParameters parameters)
   {
      var result = await Run(parameters);
      return Serialize(result);
   }
   
   public static string Identifier => nameof(ProjectListProvider);
}

public sealed class ProjectListResult
{
   public HashSet<ProjectListEntry> Projects { get; set; } = [];
}

public sealed record ProjectListEntry
{
   public required string ProjectName { get; set; }
   public required StringId Id { get; set; }
}