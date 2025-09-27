using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalytics.Engine.Collectors.Models.Common;

public sealed record OpenSolutionResult(
   MSBuildWorkspace WorkSpace,
   Solution Solution) : IDisposable
{
   public void Dispose()
   {
      WorkSpace.Dispose();
   }
}