using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
   public required string DbConnectionString { get; set; }
   
   public bool WriteSourceFiles { get; set; } = true;
   
   public required IServiceProvider ServiceProvider { get; set; }

   [field: AllowNull, MaybeNull]
   public string RelativePath => field ??= System.IO.Path.GetRelativePath(BasePath, Path);
}