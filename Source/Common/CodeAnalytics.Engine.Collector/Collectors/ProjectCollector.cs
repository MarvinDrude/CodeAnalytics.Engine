using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collector.Collectors;

public sealed partial class ProjectCollector : IProjectCollector
{
   private readonly ILogger<ProjectCollector> _logger;
   private readonly ProjectOptions _options;

   private CollectContext? _context;

   public ProjectCollector(ProjectOptions options)
   {
      _options = options;
      _logger = options.ServiceProvider.GetRequiredService<ILogger<ProjectCollector>>();
   }

   public Task Collect(Project project, CancellationToken ct = default)
   {
      
   }
   
   public ValueTask DisposeAsync()
   {
      return ValueTask.CompletedTask;
   }
}