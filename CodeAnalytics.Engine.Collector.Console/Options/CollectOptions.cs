using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Ids;
using CodeAnalytics.Engine.Occurrences;

namespace CodeAnalytics.Engine.Collector.Console.Options;

public sealed class CollectOptions
{
   public required string Path { get; set; }
   public required string BasePath { get; set; }
   public required string OutputBasePath { get; set; }
   
   public int InitialCapacityPerComponentPool { get; set; } = 1_000;
   public int MaxDegreeOfParallelism { get; set; } = 1;
   public bool WriteSourceFiles { get; set; } = true;
   
   public static ProjectOptions CreateProjectOptions(
      CollectOptions options,
      IServiceProvider serviceProvider)
   {
      return new ProjectOptions()
      {
         NodeIdStore = new NodeIdStore("NodeIdStore"),
         StringIdStore = new StringIdStore("StringIdStore"),
         Occurrences = new OccurrenceRegistry(),
         ServiceProvider = serviceProvider,
         BasePath = options.BasePath,
         OutputBasePath = options.OutputBasePath,
         InitialCapacityPerComponentPool = options.InitialCapacityPerComponentPool,
         Path = options.Path,
         WriteSourceFiles = options.WriteSourceFiles,
      };
   }

   public static SolutionOptions CreateSolutionOptions(
      CollectOptions options,
      IServiceProvider serviceProvider)
   {
      return new SolutionOptions()
      {
         NodeIdStore = new NodeIdStore("NodeIdStore"),
         StringIdStore = new StringIdStore("StringIdStore"),
         Occurrences = new OccurrenceRegistry(),
         ServiceProvider = serviceProvider,
         BasePath = options.BasePath,
         OutputBasePath = options.OutputBasePath,
         InitialCapacityPerComponentPool = options.InitialCapacityPerComponentPool,
         MaxDegreeOfParallelism = options.MaxDegreeOfParallelism,
         Path = options.Path,
         WriteSourceFiles = options.WriteSourceFiles,
      };
   }
}