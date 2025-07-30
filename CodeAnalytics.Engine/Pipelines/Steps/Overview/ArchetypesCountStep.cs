using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Pipelines.Common;

namespace CodeAnalytics.Engine.Pipelines.Steps.Overview;

public sealed class ArchetypesCountStep 
   : PipelineStepBase<Dictionary<StringId, ArchetypeChunkViews>, ArchetypesCountResult>
{
   private readonly AnalyzeStore _store;
   private readonly PipelineParameters _parameters;
   
   public ArchetypesCountStep(
      AnalyzeStore store,
      PipelineParameters parameters,
      IPipelineCacheProvider cacheProvider, 
      bool useCache) 
      : base(null, cacheProvider, useCache)
   {
      _store = store;
      _parameters = parameters;
   }

   protected override ValueTask<ArchetypesCountResult> Process(
      Dictionary<StringId, ArchetypeChunkViews> input, CancellationToken ct = default)
   {
      ArchetypesCountResult result = new()
      {
         Global = new ArchetypesCountEntry(),
         PerProject = []
      };
      var global = result.Global;
      
      foreach (var (projectId, views) in input)
      {
         if (!_parameters.Projects.Contains(projectId))
         {
            continue;
         }
         
         if (!result.PerProject.TryGetValue(projectId, out var perProject))
         {
            perProject = result.PerProject[projectId] = new ArchetypesCountEntry();
         }

         perProject.ClassCount = views.Classes.Count;
         perProject.StructCount = views.Structs.Count;
         perProject.InterfaceCount = views.Interfaces.Count;
         perProject.EnumCount = views.Enums.Count;
         perProject.EnumValueCount = views.EnumValues.Count;
         
         perProject.ConstructorCount = views.Constructors.Count;
         perProject.MethodCount = views.Methods.Count;
         perProject.FieldCount = views.Fields.Count;
         perProject.PropertyCount = views.Properties.Count;
      }

      global.ClassCount = CountArchetypes(_store.ClassChunk);
      global.StructCount = CountArchetypes(_store.StructChunk);
      global.InterfaceCount = CountArchetypes(_store.InterfaceChunk);
      global.EnumCount = CountArchetypes(_store.EnumChunk);
      global.EnumValueCount = CountArchetypes(_store.EnumValueChunk);
      
      global.ConstructorCount = CountArchetypes(_store.ConstructorChunk);
      global.MethodCount = CountArchetypes(_store.MethodChunk);
      global.FieldCount = CountArchetypes(_store.FieldChunk);
      global.PropertyCount = CountArchetypes(_store.PropertyChunk);
      
      return new ValueTask<ArchetypesCountResult>(result);
   }

   private int CountArchetypes<TArchetype>(ArchetypeChunkBase<TArchetype> chunk)
      where TArchetype : IArchetype, IEquatable<TArchetype>
   {
      var result = 0;
      
      foreach (ref var archetype in chunk.Entries)
      {
         if (!archetype.SymbolComponent.Projects.ContainsAny(_parameters.Projects))
         {
            continue;
         }

         result++;
      }

      return result;
   } 
}

public sealed class ArchetypesCountResult
{
   public required Dictionary<StringId, ArchetypesCountEntry> PerProject { get; set; }
   public required ArchetypesCountEntry Global { get; set; }
}

public sealed class ArchetypesCountEntry
{
   public int ClassCount { get; set; }
   public int StructCount { get; set; }
   public int InterfaceCount { get; set; }
   public int EnumCount { get; set; }
   public int EnumValueCount { get; set; }
   
   public int ConstructorCount { get; set; }
   public int MethodCount { get; set; }
   public int FieldCount { get; set; }
   public int PropertyCount { get; set; }
}