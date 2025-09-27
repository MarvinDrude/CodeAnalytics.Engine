using CodeAnalytics.Engine.Collectors.Options;
using CodeAnalytics.Engine.Storage.Common;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Me.Memory.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Collectors.Common;

public sealed partial class SolutionCollector : IAsyncDisposable
{
   private readonly ILogger<SolutionCollector> _logger;
   private readonly IDbContextFactory<DbMainContext> _dbContextFactory;

   private readonly IOptionsMonitor<CollectorOptions> _optionsMonitor;
   private CollectorOptions CollectorOptions => _optionsMonitor.CurrentValue;
   
   private readonly WorkPool _workPool;
   
   private int _currentProjectCount;
   private int _maxProjectCount;

   public SolutionCollector(
      ILogger<SolutionCollector> logger,
      IOptionsMonitor<CollectorOptions> optionsMonitor,
      IDbContextFactory<DbMainContext> dbContextFactory)
   {
      _logger = logger;
      _optionsMonitor = optionsMonitor;
      _dbContextFactory = dbContextFactory;

      _workPool = new WorkPool(new WorkPoolOptions()
      {
         MaxDegreeOfParallelism = CollectorOptions.MaxDegreeOfParallelism
      });
      _currentProjectCount = 0;
   }

   public async Task Collect(CancellationToken ct = default)
   {
      using var pack = await WorkspaceBootstrapper.OpenSolution(CollectorOptions.Path, ct);
      var projects = pack.Solution.Projects.ToList();

      await using var dbContext = await _dbContextFactory.CreateDbContextAsync(ct);
      LogUpdateProjectCount(_currentProjectCount, projects.Count);

      var dbSolution = await GetOrCreateDbSolution(dbContext, pack.Solution, ct);
      
      
   }

   
   
   private async Task<DbSolution> GetOrCreateDbSolution(
      DbMainContext dbContext,
      Solution solution,
      CancellationToken ct = default)
   {
      var relativePath = Path.GetRelativePath(
         CollectorOptions.BasePath, solution.FilePath ?? CollectorOptions.Path);
      var created = new DbSolution()
      {
         Name = Path.GetFileName(relativePath),
         RelativeFilePath = relativePath,
         Projects = []
      };

      return await dbContext.UpdateOrCreate(dbContext.Solutions)
         .Match(x => x.RelativeFilePath == relativePath)
         .OnCreate(() => created)
         .Execute(ct);
   }
   
   public async ValueTask DisposeAsync()
   {
      await _workPool.DisposeAsync();
   }
}