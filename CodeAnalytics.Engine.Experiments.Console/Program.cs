
using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Collector.Collectors;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Ids;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var serviceCollection = new ServiceCollection();
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

var collector = new ProjectCollector(new ProjectOptions()
{
   NodeIdStore = new NodeIdStore("NodeIds"),
   StringIdStore = new StringIdStore("StringIds"),
   OutputBasePath = @"C:\content",
   Path = @"C:\Users\marvi\source\repos\CodeAnalytics.Engine\CodeAnalytics.Engine.TestContainer.SimpleOne\CodeAnalytics.Engine.TestContainer.SimpleOne.csproj",
   BasePath = @"C:\Users\marvi\source\repos\",
   ServiceProvider = provider,
   InitialCapacityPerComponentPool = 500,
});

var res = await collector.Collect();
var analyze = new AnalyzeStore(res.Success!);

var bytes = res.Success!.ToMemory();
var res2 = CollectorStore.FromMemory(bytes);

_ = "";