
/*
 * Main program to collect the necessary data
 * for the analytics to work
 */

using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Collector.Projects;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Bake.Common;
using Beskar.CodeAnalytics.Data.Bake.Steps;
using Me.Memory.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var configBuilder = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .AddEnvironmentVariables(prefix: "CA_")
   .AddCommandLine(args);
var configuration = configBuilder.Build();

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<IConfiguration>(configuration);
serviceCollection.AddLogging(lb =>
{
   Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Debug()
      .WriteTo.Console(theme: AnsiConsoleTheme.Code)
      .CreateLogger();
   
   lb.ClearProviders();
   lb.AddSerilog(Log.Logger);
});

var provider = serviceCollection.BuildServiceProvider();

var logger = provider.GetRequiredService<ILogger<Program>>();
var collectOptions = configuration.GetSection("Collect").Get<CollectorOptions>() 
   ?? throw new Exception("Invalid configuration");

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
var projectLogger = provider.GetRequiredService<ILogger<ProjectCollector>>();
var solutionLogger = provider.GetRequiredService<ILogger<SolutionCollector>>();

Console.CancelKeyPress += (_, args) =>
{
   args.Cancel = true;
   cts.Cancel();
};

try
{
   var firstEncounter = true;
   var batch = DiscoveryBatch.CreateEmpty(collectOptions);
   var workPool = new WorkPool(new WorkPoolOptions()
   {
      MaxDegreeOfParallelism = collectOptions.MaxDegreeOfParallelism
   });
   
   foreach (var filePath in collectOptions.TargetPaths)
   {
      if (filePath.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
      {
         var projectHandle =
            await WorkSpaceLoader.InitializeFromProject(filePath, firstEncounter, cancellationToken);
         if (projectHandle.Success is not { } successProject)
         {
            throw new InvalidOperationException(projectHandle.Error);
         }

         var project = new ProjectCollector(successProject, projectLogger);
         await project.Discover(batch, cancellationToken);

         firstEncounter = false;
         continue;
      }

      var solutionHandle = await WorkSpaceLoader.InitializeFromSolution(
         filePath, firstEncounter, cancellationToken);
      if (solutionHandle.Success is not { } success)
      {
         throw new InvalidOperationException(solutionHandle.Error);
      }

      await using var solution = new SolutionCollector(
         success, collectOptions, solutionLogger, loggerFactory);
      await solution.Discover(batch, cancellationToken);

      firstEncounter = false;
   }

   var fileNames = batch.GetFileNames();
   await batch.DisposeAsync();

   var engine = new BakeEngine(collectOptions.OutputPath, loggerFactory)
      .AddStep(new SymbolSortBakeStep(loggerFactory))
      .AddStep(new EdgeConnectionBakeStep(loggerFactory))
      .AddStep(new IndexBakeStep(loggerFactory));

   await engine.Execute(batch.StringDefinitions, workPool, 
      fileNames, collectOptions.DeleteIntermediateFiles, 
      cancellationToken);
}
catch (Exception ex)
{
   ProgramLogger.LogError(logger, ex);
}
finally
{
   ProgramLogger.LogFinish(logger);
}

internal static partial class ProgramLogger
{
   [LoggerMessage(Level = LogLevel.Information, Message = "Finished program.")]
   public static partial void LogFinish(ILogger logger);

   [LoggerMessage(Level = LogLevel.Error, Message = "Fatal error.")]
   public static partial void LogError(ILogger logger, Exception err);
}