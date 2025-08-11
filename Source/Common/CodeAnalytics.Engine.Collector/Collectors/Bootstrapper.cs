using CodeAnalytics.Engine.Collector.Collectors.Models;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalytics.Engine.Collector.Collectors;

public static class Bootstrapper
{
   public static void InitLocators()
   {
      MSBuildLocator.RegisterDefaults();
   }

   public static async Task<OpenSolutionResult> OpenSolution(
      string solutionPath, CancellationToken ct = default)
   {
      var workspace = MSBuildWorkspace.Create();
      var solution = await workspace.OpenSolutionAsync(solutionPath, cancellationToken: ct);
      
      return new OpenSolutionResult(workspace, solution);
   }
}