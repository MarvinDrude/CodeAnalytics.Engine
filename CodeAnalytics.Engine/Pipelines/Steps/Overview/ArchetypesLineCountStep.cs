using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Merges.Members;
using CodeAnalytics.Engine.Merges.Types;
using CodeAnalytics.Engine.Pipelines.Common;

namespace CodeAnalytics.Engine.Pipelines.Steps.Overview;

public sealed class ArchetypesLineCountStep 
   : PipelineStepBase<Dictionary<StringId, ArchetypeChunkViews>, ArchetypesLineCountResult>
{
   private readonly AnalyzeStore _store;
   private readonly PipelineParameters _parameters;

   public ArchetypesLineCountStep(
      AnalyzeStore store, 
      PipelineParameters parameters,
      IPipelineCacheProvider cacheProvider, 
      bool useCache) 
      : base(null, cacheProvider, useCache)
   {
      _store = store;
      _parameters = parameters;
   }

   protected override ValueTask<ArchetypesLineCountResult> Process(
      Dictionary<StringId, ArchetypeChunkViews> input, CancellationToken ct = default)
   {
      ArchetypesLineCountResult result = new()
      {
         Global = new ArchetypesLineCountEntry(),
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
            perProject = result.PerProject[projectId] = new ArchetypesLineCountEntry();
         }
         
         RunArchetypes(projectId, views.Classes, perProject, (entry) => entry.Classes);
         RunArchetypes(projectId, views.Structs, perProject, (entry) => entry.Structs);
         RunArchetypes(projectId, views.Interfaces, perProject, (entry) => entry.Interfaces);
         RunArchetypes(projectId, views.Enums, perProject, (entry) => entry.Enums);
         RunArchetypes(projectId, views.EnumValues, perProject, (entry) => entry.EnumValues);
         
         RunArchetypes(projectId, views.Constructors, perProject, (entry) => entry.Constructors);
         RunArchetypes(projectId, views.Methods, perProject, (entry) => entry.Methods);
         RunArchetypes(projectId, views.Fields, perProject, (entry) => entry.Fields);
         RunArchetypes(projectId, views.Properties, perProject, (entry) => entry.Properties);
      }

      var store = _store.Inner.ComponentStore;

      var classes = store.GetOrCreatePool<ClassComponent, ClassMerger>();
      var structs = store.GetOrCreatePool<StructComponent, StructMerger>();
      var interfaces = store.GetOrCreatePool<InterfaceComponent, InterfaceMerger>();
      var enums = store.GetOrCreatePool<EnumComponent, EnumMerger>();
      var enumValues = store.GetOrCreatePool<EnumValueComponent, EnumValueMerger>();
      
      var constructors = store.GetOrCreatePool<ConstructorComponent, ConstructorMerger>();
      var methods = store.GetOrCreatePool<MethodComponent, MethodMerger>();
      var fields = store.GetOrCreatePool<FieldComponent, FieldMerger>();
      var properties = store.GetOrCreatePool<PropertyComponent, PropertyMerger>();
      
      foreach (var (nodeId, entry) in _store.Inner.LineCountStore.LineCountsPerNode)
      {
         var total = entry.GetTotal(_parameters.Projects);

         var target = nodeId switch
         {
            _ when classes.TryGetSlot(nodeId, out _) => global.Classes,
            _ when structs.TryGetSlot(nodeId, out _) => global.Structs,
            _ when interfaces.TryGetSlot(nodeId, out _) => global.Interfaces,
            _ when enums.TryGetSlot(nodeId, out _) => global.Enums,
            _ when enumValues.TryGetSlot(nodeId, out _) => global.EnumValues,
            
            _ when constructors.TryGetSlot(nodeId, out _) => global.Constructors,
            _ when methods.TryGetSlot(nodeId, out _) => global.Methods,
            _ when fields.TryGetSlot(nodeId, out _) => global.Fields,
            _ when properties.TryGetSlot(nodeId, out _) => global.Properties,
            _ => null
         };
         if (target is null) continue;

         target.CodeCount += total.CodeCount;
         target.LineCount += total.LineCount;
      }
      
      return new ValueTask<ArchetypesLineCountResult>(result);
   }

   private void RunArchetypes<TArchetype>(
      StringId projectId,
      ArchetypeChunkView<TArchetype> view,
      ArchetypesLineCountEntry perProject,
      Func<ArchetypesLineCountEntry, ArchetypesLineCounts> callback)
      where TArchetype : IArchetype, IEquatable<TArchetype>
   {
      foreach (ref var cls in view)
      {
         if (!_store.Inner.LineCountStore.LineCountsPerNode.TryGetValue(cls.NodeId, out var lineCount))
         {
            continue;
         }

         var stats = lineCount.StatsPerProject.GetValueOrDefault(projectId);
         var target = callback(perProject);
         
         target.CodeCount += stats?.CodeCount ?? 0;
         target.LineCount += stats?.LineCount ?? 0;
      }
   }
}

public sealed class ArchetypesLineCountResult
{
   public required Dictionary<StringId, ArchetypesLineCountEntry> PerProject { get; set; }
   public required ArchetypesLineCountEntry Global { get; set; }
}

public sealed class ArchetypesLineCountEntry
{
   public ArchetypesLineCounts Classes { get; set; } = new ();
   public ArchetypesLineCounts Structs { get; set; } = new ();
   public ArchetypesLineCounts Interfaces { get; set; } = new ();
   public ArchetypesLineCounts Enums { get; set; } = new ();
   public ArchetypesLineCounts EnumValues { get; set; } = new ();
   
   public ArchetypesLineCounts Constructors { get; set; } = new ();
   public ArchetypesLineCounts Properties { get; set; } = new ();
   public ArchetypesLineCounts Methods { get; set; } = new ();
   public ArchetypesLineCounts Fields { get; set; } = new ();
}

public sealed class ArchetypesLineCounts
{
   public int LineCount { get; set; }
   public int CodeCount { get; set; }
}