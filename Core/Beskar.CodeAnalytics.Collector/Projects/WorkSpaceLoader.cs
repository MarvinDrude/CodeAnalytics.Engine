using Beskar.CodeAnalytics.Collector.Projects.Models;
using Me.Memory.Results;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace Beskar.CodeAnalytics.Collector.Projects;

public static class WorkSpaceLoader
{
   public static async Task<Result<CsProjectHandle, string>> InitializeFromProject(
      string projectPath, bool registerLocator, CancellationToken cancellationToken)
   {
      if (registerLocator)
      {
         MSBuildLocator.RegisterDefaults();
      }

      var workSpace = MSBuildWorkspace.Create();
      var project = await workSpace.OpenProjectAsync(projectPath, cancellationToken: cancellationToken);

      if (project is not { FilePath.Length: > 0 })
      {
         return "Project could not be loaded. (is null or empty file path)";
      }

      return new CsProjectHandle(new CsSolutionHandle(workSpace, project.Solution), project);
   }

   public static async Task<Result<CsSolutionHandle, string>> InitializeFromSolution(
      string solutionPath, bool registerLocator, CancellationToken cancellationToken)
   {
      if (registerLocator)
      {
         MSBuildLocator.RegisterDefaults();
      }
      
      var workSpace = MSBuildWorkspace.Create();
      var solution = await workSpace.OpenSolutionAsync(solutionPath, cancellationToken: cancellationToken);

      if (solution is not { FilePath.Length: > 0 })
      {
         return "Solution could not be loaded. (is null or empty file path)";
      }
      
      return new CsSolutionHandle(workSpace, solution);
   }
}