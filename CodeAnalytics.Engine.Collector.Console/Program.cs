
using CodeAnalytics.Engine.Collector.Collectors;
using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Console.Options;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Compression;
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
Bootstrapper.InitLocators();

var logger = provider.GetRequiredService<ILogger<Program>>();
var collectOptions = configuration.GetSection("Collect").Get<CollectOptions>() 
   ?? throw new Exception("Invalid configuration");

Result<CollectorStore, Error<string>> result;

if (collectOptions.Path.ToLower().EndsWith(".csproj"))
{
   var options = CollectOptions.CreateProjectOptions(collectOptions, provider);
   var collector = new ProjectCollector(options);

   result = await collector.Collect();
}
else
{
   var options = CollectOptions.CreateSolutionOptions(collectOptions, provider);
   await using ISolutionCollector collector = collectOptions.MaxDegreeOfParallelism is 1 
      ? new SequentialSolutionCollector(options)
      : new SolutionCollector(options);

   result = await collector.Collect();
}

if (result is { IsSuccess: true, Success: { } store })
{
   ProgramLogger.LogFinishedCollecting(logger);
   
   await File.WriteAllBytesAsync(
      Path.Combine(collectOptions.OutputBasePath, "data.caec"), 
      new DeflateCompressor().Compress(store.ToMemory()));
}
else
{
   ProgramLogger.LogError(logger, result.Error.Detail);
}

ProgramLogger.LogFinish(logger);

internal static partial class ProgramLogger
{
   [LoggerMessage(
      EventId = 0,
      Level = LogLevel.Information,
      Message = "Finished collecting. Writing to file..."
   )]
   public static partial void LogFinishedCollecting(ILogger logger);
   
   [LoggerMessage(
      EventId = 1,
      Level = LogLevel.Information,
      Message = "Finished program."
   )]
   public static partial void LogFinish(ILogger logger);

   [LoggerMessage(
      EventId = 2,
      Level = LogLevel.Error,
      Message = "Fatal error: {Message}."
   )]
   public static partial void LogError(ILogger logger, string message);
}