using System.Text.Json.Serialization;
using CodeAnalytics.Engine.Ids;

namespace CodeAnalytics.Engine.Collector.Collectors.Options;

[JsonPolymorphic]
[JsonDerivedType(typeof(ProjectOptions), "project")]
[JsonDerivedType(typeof(SolutionOptions), "solution")]
public class BaseOptions
{
   /// <summary>
   /// Either a project or solution file path
   /// </summary>
   public required string Path { get; set; }
   public required string BasePath { get; set; }
   public required string OutputBasePath { get; set; }
   
   public int InitialCapacityPerComponentPool { get; set; } = 1_000;
   
   public required NodeIdStore NodeIdStore { get; set; }
   public required StringIdStore StringIdStore { get; set; }
   
   public required IServiceProvider ServiceProvider { get; set; }
   
   private string? _relativePath;
   public string RelativePath => _relativePath ??= System.IO.Path.GetRelativePath(BasePath, Path);
}