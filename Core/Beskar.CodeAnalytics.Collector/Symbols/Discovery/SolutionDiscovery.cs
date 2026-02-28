using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class SolutionDiscovery
{
   public static async Task<bool> Discover(
      DiscoveryBatch batch, CsSolutionHandle solutionHandle)
   {
      var uniquePath = Path.GetRelativePath(batch.Options.BasePath, solutionHandle.Solution.FilePath ?? "Unknown.slnx");
      var stringDef = batch.StringDefinitions.GetStringFileView(uniquePath);
      var id = batch.Identifiers.GenerateIdentifier(uniquePath, stringDef);
      
      var name = Path.GetFileName(uniquePath);
      var nameDef = batch.StringDefinitions.GetStringFileView(name);

      foreach (var project in solutionHandle.Solution.Projects)
      {
         var projectPath = Path.GetRelativePath(batch.Options.BasePath, project.FilePath ?? $"{Guid.NewGuid()}.csproj");
         var pPathDef = batch.StringDefinitions.GetStringFileView(projectPath);
         var projectId = batch.Identifiers.GenerateIdentifier(projectPath, pPathDef);
         
         batch.WriteDiscoveryEdge(id, projectId, SymbolEdgeType.SolutionProject);
      }
      
      var solution = new SolutionSpec()
      {
         Id = id,
         FilePath = stringDef,
         Name = nameDef,
         Projects = new StorageView<uint>(-1, -1)
      };

      await batch.SolutionWriter.Write(id, solution);
      return true;
   }
}