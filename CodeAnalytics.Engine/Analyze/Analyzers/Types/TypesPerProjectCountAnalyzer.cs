using CodeAnalytics.Engine.Analyze.Analyzers.Interfaces;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Analyze.Analyzers.Types;

public sealed class TypesPerProjectCountAnalyzer 
   : IAnalyzer<TypesPerProjectCountResult>
{
   public TypesPerProjectCountResult Analyze(AnalyzeStore store)
   {
      ref var interfaces = ref store.InterfaceChunk.Entries;
      ref var classes = ref store.ClassChunk.Entries;
      ref var enums = ref store.EnumChunk.Entries;
      ref var structs = ref store.StructChunk.Entries;

      var lineCountStore = store.Inner.LineCountStore;
      var result = new TypesPerProjectCountResult();

      Analyze(ref interfaces, lineCountStore, result, 
         static (perProject, total, stats) =>
         {
            perProject.Interfaces.Count++;
            perProject.Interfaces.LineCount += stats.LineCount;
            perProject.Interfaces.CodeCount += stats.CodeCount;
            
            total.Interfaces.Count++;
            total.Interfaces.LineCount += stats.LineCount;
            total.Interfaces.CodeCount += stats.CodeCount;
         });
      Analyze(ref classes, lineCountStore, result, 
         static (perProject, total, stats) =>
         {
            perProject.Classes.Count++;
            perProject.Classes.LineCount += stats.LineCount;
            perProject.Classes.CodeCount += stats.CodeCount;
            
            total.Classes.Count++;
            total.Classes.LineCount += stats.LineCount;
            total.Classes.CodeCount += stats.CodeCount;
         });
      Analyze(ref enums, lineCountStore, result, 
         static (perProject, total, stats) =>
         {
            perProject.Enums.Count++;
            perProject.Enums.LineCount += stats.LineCount;
            perProject.Enums.CodeCount += stats.CodeCount;
            
            total.Enums.Count++;
            total.Enums.LineCount += stats.LineCount;
            total.Enums.CodeCount += stats.CodeCount;
         });
      Analyze(ref structs, lineCountStore, result, 
         static (perProject, total, stats) =>
         {
            perProject.Structs.Count++;
            perProject.Structs.LineCount += stats.LineCount;
            perProject.Structs.CodeCount += stats.CodeCount;
            
            total.Structs.Count++;
            total.Structs.LineCount += stats.LineCount;
            total.Structs.CodeCount += stats.CodeCount;
         });
      
      return result;
   }

   private void Analyze<TArchetype>(
      ref PooledList<TArchetype> archetypes,
      LineCountStore lineCountStore,
      TypesPerProjectCountResult result,
      Action<TypesPerProjectCount, TypesPerProjectCount, LineCountStats> action)
      where TArchetype : IArchetype, IEquatable<TArchetype>
   {
      foreach (ref var archetype in archetypes)
      {
         if (!lineCountStore.LineCountsPerNode.TryGetValue(archetype.NodeId, out var lineCounts))
         {
            continue;
         }
         
         foreach (var (_, lineCount) in lineCounts.StatsPerFile)
         {
            if (!result.PerProject.TryGetValue(lineCount.ProjectId, out var target))
            {
               target = result.PerProject[lineCount.ProjectId] = new TypesPerProjectCount()
               {
                  Path = lineCount.ProjectId.ToString()
               };
            }

            target.Total.Count++;
            target.Total.LineCount += lineCount.LineCount;
            target.Total.CodeCount += lineCount.CodeCount;

            result.Total.Total.Count++;
            result.Total.Total.LineCount += lineCount.LineCount;
            result.Total.Total.CodeCount += lineCount.CodeCount;
            
            action.Invoke(target, result.Total, lineCount);
         }
      }
   }
}

public sealed class TypesPerProjectCountResult
{
   public TypesPerProjectCount Total { get; } = new()
   {
      Path = "Total"
   };

   public Dictionary<StringId, TypesPerProjectCount> PerProject { get; } = [];
}

public sealed class TypesPerProjectCount
{
   public required string Path { get; init; }
   public TypePerProjectCounts Total { get; } = new();
   
   public TypePerProjectCounts Classes { get; } = new();
   public TypePerProjectCounts Structs { get; } = new();
   public TypePerProjectCounts Interfaces { get; } = new();
   public TypePerProjectCounts Enums { get; } = new();
}

public sealed class TypePerProjectCounts
{
   public int Count { get; set; }
   public int LineCount { get; set; }
   public int CodeCount { get; set; }
}