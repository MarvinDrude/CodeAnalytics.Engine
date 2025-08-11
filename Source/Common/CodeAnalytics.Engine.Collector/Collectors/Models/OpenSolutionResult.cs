using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalytics.Engine.Collector.Collectors.Models;

public record OpenSolutionResult(
   MSBuildWorkspace WorkSpace,
   Solution Solution) : IDisposable
{
   public void Dispose()
   {
      GC.SuppressFinalize(this);
      WorkSpace.Dispose();
   }   
}