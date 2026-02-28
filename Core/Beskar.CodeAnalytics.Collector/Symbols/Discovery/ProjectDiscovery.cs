using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class ProjectDiscovery
{
   public static async Task<uint> Discover(
      DiscoveryBatch batch, CsProjectHandle handle)
   {
      var project = handle.Project;
      
      var projectPath = Path.GetRelativePath(batch.Options.BasePath, project.FilePath ?? $"{Guid.NewGuid()}.csproj");
      var pPathDef = batch.StringDefinitions.GetStringFileView(projectPath);
      var projectId = batch.Identifiers.GenerateIdentifier(projectPath, pPathDef);
      
      var name = Path.GetFileName(projectPath);
      var nameDef = batch.StringDefinitions.GetStringFileView(name);

      var assemblyName = handle.Project.AssemblyName;
      var assemblyNameDef = batch.StringDefinitions.GetStringFileView(assemblyName);
      
      var rootNamespace = project.DefaultNamespace ?? project.AssemblyName;
      var rootNamespaceDef = batch.StringDefinitions.GetStringFileView(rootNamespace);

      foreach (var reference in project.ProjectReferences)
      {
         if (handle.Project.Solution.GetProject(reference.ProjectId) is not { } refProject)
         {
            continue;
         }
         
         var referencePath = Path.GetRelativePath(batch.Options.BasePath, refProject.FilePath ?? $"{Guid.NewGuid()}.csproj");
         var referencePathDef = batch.StringDefinitions.GetStringFileView(referencePath);
         var refProjectId = batch.Identifiers.GenerateIdentifier(referencePath, referencePathDef);
         
         batch.WriteDiscoveryEdge(projectId, refProjectId, SymbolEdgeType.ProjectReference);
      }
      
      var uniquePath = Path.GetRelativePath(batch.Options.BasePath, project.Solution.FilePath ?? "Unknown.slnx");
      var stringDef = batch.StringDefinitions.GetStringFileView(uniquePath);
      var id = batch.Identifiers.GenerateIdentifier(uniquePath, stringDef);
      
      batch.WriteDiscoveryEdge(projectId, id, SymbolEdgeType.ProjectSolution);
      
      var spec = new ProjectSpec()
      {
         Id = projectId,
         ProjectFilePath = pPathDef,
         Name = nameDef,
         AssemblyName = assemblyNameDef,
         RootNamespace = rootNamespaceDef,
         
         Files = new StorageView<FileSpec>(-1, -1),
         ProjectReferences = new StorageView<ProjectSpec>(-1, -1),
         Solutions = new StorageView<SolutionSpec>(-1, -1),
      };

      await batch.ProjectWriter.Write(projectId, spec);
      return projectId;
   }
}