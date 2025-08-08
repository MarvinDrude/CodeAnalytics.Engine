using CodeAnalytics.Engine.Collector.Collectors.Models;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeAnalytics.Engine.Collector.Collectors;

public static class Bootstrapper
{
   public static void InitLocators()
   {
      MSBuildLocator.RegisterDefaults();
   }
   
   public static async Task<string[]> GetProjectPathsBySolution(
      string solutionPath, CancellationToken ct = default)
   {
      using var workspace = MSBuildWorkspace.Create();
      
      var solution = await workspace.OpenSolutionAsync(solutionPath, cancellationToken: ct);
      var allProjects = solution.Projects.Select(x => x.FilePath);
      
      return [.. 
         allProjects
            .Where(x => x is not null)
            .Select(x => x!)
      ];
   }

   public static async Task<(MSBuildWorkspace WorkSpace, Solution Solution)> OpenSolution(
      string solutionPath, CancellationToken ct = default)
   {
      var workspace = MSBuildWorkspace.Create();
      var solution = await workspace.OpenSolutionAsync(solutionPath, cancellationToken: ct);

      return (workspace, solution);
   }
   
   public static async Task<ProjectParseInfo?> GetProjectByPath(
      string projectPath, CancellationToken ct = default)
   {
      var workspace = MSBuildWorkspace.Create();

      try
      {
         var raw = await workspace.OpenProjectAsync(projectPath, cancellationToken: ct);
         var compilation = await raw.GetCompilationAsync(ct);

         if (compilation is null || raw.FilePath is null)
         {
            return null;
         }

         return new ProjectParseInfo()
         {
            Compilation = compilation,
            Workspace = workspace,
         };
      }
      catch (Exception)
      {
         workspace?.Dispose();
         throw;
      }
   }
}