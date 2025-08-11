﻿
using CodeAnalytics.Engine.Collector.Collectors;
using CodeAnalytics.Engine.Collector.Console.Options;
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

var options = CollectOptions.CreateSolutionOptions(collectOptions, provider);
await using var collector = new SolutionCollector(options);

try
{
   await collector.Collect();
   ProgramLogger.LogFinishedCollecting(logger);
}
finally
{
   ProgramLogger.LogFinish(logger);
}

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