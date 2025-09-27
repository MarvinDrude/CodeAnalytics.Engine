
using CodeAnalytics.Engine.Collectors.Common;
using CodeAnalytics.Engine.Collectors.Extensions;
using CodeAnalytics.Engine.Storage.Extensions;
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

serviceCollection.AddOptions();
serviceCollection.AddAndGetCollectorOptions(configuration);
serviceCollection.AddAndGetDatabaseOptions(configuration);

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
WorkspaceBootstrapper.InitLocators();

var logger = provider.GetRequiredService<ILogger<Program>>();

try
{
   
   ProgramLogger.LogFinishedCollecting(logger);
}
catch (Exception error)
{
   ProgramLogger.LogError(logger, error);
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
      Message = "Finished collecting. Cleaning up..."
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
      Message = "Fatal error."
   )]
   public static partial void LogError(ILogger logger, Exception error);
}