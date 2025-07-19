using CodeAnalytics.Engine.Ids;

namespace CodeAnalytics.Engine.Collector.Collectors.Options;

public class BaseOptions
{
   public required string ProjectPath { get; set; }
   public required string BasePath { get; set; }
   public required string OutputBasePath { get; set; }
   
   public int InitialCapacityPerComponentPool { get; set; } = 1_000;
   
   public required NodeIdStore NodeIdStore { get; set; }
   public required StringIdStore StringIdStore { get; set; }
   
   private string? _relativePath;
   public string RelativePath => _relativePath ??= Path.GetRelativePath(BasePath, ProjectPath);
}