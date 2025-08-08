namespace CodeAnalytics.Engine.Collector.Collectors.Options;

public class ProjectOptions : BaseOptions
{
   public bool IsProjectOnly { get; set; } = true;
   
   public static ProjectOptions Create(SolutionOptions options)
   {
      return new ProjectOptions()
      {
         NodeIdStore = options.NodeIdStore,
         StringIdStore =  options.StringIdStore,
         Path = options.Path,
         BasePath = options.BasePath,
         OutputBasePath = options.OutputBasePath,
         InitialCapacityPerComponentPool = options.InitialCapacityPerComponentPool,
         ServiceProvider = options.ServiceProvider,
         Occurrences = options.Occurrences,
         IsProjectOnly = false,
         WriteSourceFiles = options.WriteSourceFiles
      };
   }
}