using CodeAnalytics.Engine.Collector.Collectors.Options;

namespace CodeAnalytics.Engine.Collector.Console.Options;

public class CollectOptions
{
   public required string Path { get; set; }
   public required string BasePath { get; set; }
   public required string OutputBasePath { get; set; }
   public required string DbConnectionString { get; set; }
   
   public int MaxDegreeOfParallelism { get; set; } = 1;
   public bool WriteSourceFiles { get; set; } = true;
   
   public static SolutionOptions CreateSolutionOptions(
      CollectOptions options,
      IServiceProvider serviceProvider)
   {
      return new SolutionOptions()
      {
         ServiceProvider = serviceProvider,
         BasePath = options.BasePath,
         OutputBasePath = options.OutputBasePath,
         MaxDegreeOfParallelism = options.MaxDegreeOfParallelism,
         Path = options.Path,
         WriteSourceFiles = options.WriteSourceFiles,
         DbConnectionString = options.DbConnectionString,
      };
   }
}