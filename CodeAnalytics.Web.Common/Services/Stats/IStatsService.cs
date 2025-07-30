using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;

namespace CodeAnalytics.Web.Common.Services.Stats;

public interface IStatsService
{
   public Task<TResult> GetStats<TResult, TProvider>(PipelineParameters parameters)
      where TProvider : IPipelineProvider;
}