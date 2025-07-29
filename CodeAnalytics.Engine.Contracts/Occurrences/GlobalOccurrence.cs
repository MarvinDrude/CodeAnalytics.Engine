using System.Collections.Concurrent;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Occurrences;

public sealed class GlobalOccurrence
{
   public required NodeId NodeId { get; set; }
   
   public ConcurrentDictionary<StringId, ProjectOccurrence> ProjectOccurrences { get; init; } = [];
   
   public ConcurrentDictionary<DeclarationOccurrence, bool> DeclarationMap { get; init; } = [];
   public List<DeclarationOccurrence> Declarations => DeclarationMap.Keys.ToList();
   
   public void AddDeclaration(DeclarationOccurrence occurrence)
   {
      DeclarationMap[occurrence] = true;
   }

   public void MergeDeclarations(GlobalOccurrence occurrence)
   {
      foreach (var declaration in occurrence.Declarations)
      {
         DeclarationMap[declaration] = true;
      }

      foreach (var (_, project) in occurrence.ProjectOccurrences)
      {
         foreach (var (_, file) in project.FileOccurrences)
         {
            foreach (var line in file.LineOccurrences)
            {
               if (!line.IsDeclaration)
               {
                  continue;
               }
               
               var projectTarget = GetOrCreateByProject(project.PathId);
               var fileTarget = projectTarget.GetOrCreate(file.PathId);

               fileTarget.LineOccurrences.Add(line);
            }
         }
      }
   }
   
   public ProjectOccurrence GetOrCreateByProject(StringId pathId)
   {
      return ProjectOccurrences.GetOrAdd(pathId, (_) => new ProjectOccurrence()
      {
         PathId = pathId
      });
   }

   public Dictionary<StringId, ProjectOccurrence> ToDictionary()
   {
      return ProjectOccurrences
         .ToDictionary();
   }
}