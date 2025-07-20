
using CodeAnalytics.Engine.Collector.Collectors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

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

