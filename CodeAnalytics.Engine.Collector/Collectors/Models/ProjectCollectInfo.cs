using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalytics.Engine.Collector.Collectors.Models;

public sealed class ProjectParseInfo
{
   public required Compilation? Compilation { get; set; }
   
   public required MSBuildWorkspace? Workspace { get; set; }
}