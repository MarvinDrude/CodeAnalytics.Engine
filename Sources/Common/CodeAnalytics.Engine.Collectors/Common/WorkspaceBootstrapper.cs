using CodeAnalytics.Engine.Collectors.Models.Common;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalytics.Engine.Collectors.Common;

public static class WorkspaceBootstrapper
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