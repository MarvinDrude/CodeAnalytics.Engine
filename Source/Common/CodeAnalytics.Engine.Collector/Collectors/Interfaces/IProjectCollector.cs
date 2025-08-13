using CodeAnalytics.Engine.Storage.Entities.Structure;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Collectors.Interfaces;

public interface IProjectCollector : IAsyncDisposable
{
   public Task<Result<int, Error<string>>> Collect(
      DbSolution dbSolution,
      Workspace workSpace, 
      Project project, 
      CancellationToken ct = default);
}