﻿namespace CodeAnalytics.Engine.Collector.Collectors.Options;

public class ProjectOptions : BaseOptions
{
   public bool IsProjectOnly { get; set; } = true;
   
   public static ProjectOptions Create(SolutionOptions options)
   {
      return new ProjectOptions()
      {
         Path = options.Path,
         BasePath = options.BasePath,
         OutputBasePath = options.OutputBasePath,
         ServiceProvider = options.ServiceProvider,
         IsProjectOnly = false,
         WriteSourceFiles = options.WriteSourceFiles
      };
   }
}