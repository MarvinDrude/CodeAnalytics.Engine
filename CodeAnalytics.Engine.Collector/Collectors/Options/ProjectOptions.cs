namespace CodeAnalytics.Engine.Collector.Collectors.Options;

public class ProjectOptions : BaseOptions
{
   public static ProjectOptions Create(SolutionOptions options)
   {
      return new ProjectOptions()
      {
         NodeIdStore = options.NodeIdStore,
         StringIdStore =  options.StringIdStore,
         ProjectPath = options.ProjectPath,
         BasePath = options.BasePath,
         OutputBasePath = options.OutputBasePath,
         InitialCapacityPerComponentPool = options.InitialCapacityPerComponentPool,
      };
   }
}