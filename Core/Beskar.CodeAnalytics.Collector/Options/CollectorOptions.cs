namespace Beskar.CodeAnalytics.Collector.Options;

/// <summary>
/// Every configuration available to collect
/// the initial analytics data needed from roslyn
/// </summary>
public sealed class CollectorOptions
{
   /// <summary>
   /// All paths of all solutions and/or projects you
   /// want to include .sln / .slnx / .csproj
   /// </summary>
   public required string[] TargetPaths { get; init; }
   
   /// <summary>
   /// The base path. All target paths must be inside of this path.
   /// The logic will use the relative paths to associate data
   /// </summary>
   public required string BasePath { get; init; }
   
   /// <summary>
   /// The output folder where all the database files
   /// are written to
   /// </summary>
   public required string OutputPath { get; init; }
   
   /// <summary>
   /// How many concurrent workers should be used
   /// for the collection of the analytics data
   /// </summary>
   public int MaxDegreeOfParallelism { get; init; }
   
   /// <summary>
   /// Whether to keep or delete intermediate files
   /// </summary>
   public bool DeleteIntermediateFiles { get; init; }
   
   /// <summary>
   /// Get any path as relative path to the
   /// global base path
   /// </summary>
   public string GetRelativePath(string path)
      => Path.GetRelativePath(BasePath, path);
}