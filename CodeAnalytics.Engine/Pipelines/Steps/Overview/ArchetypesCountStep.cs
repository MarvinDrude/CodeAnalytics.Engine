using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Pipelines.Common;

namespace CodeAnalytics.Engine.Pipelines.Steps.Overview;

public sealed class ArchetypesCountStep 
   : PipelineStepBase<Dictionary<StringId, ArchetypeChunkViews>, ArchetypesCountResult>
{
   private readonly AnalyzeStore _store;
   
   public ArchetypesCountStep(
      AnalyzeStore store, IPipelineCacheProvider cacheProvider, bool useCache) 
      : base(cacheProvider, useCache)
   {
      _store = store;
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

      global.ClassCount = _store.ClassChunk.Count;
      global.StructCount = _store.StructChunk.Count;
      global.InterfaceCount = _store.InterfaceChunk.Count;
      global.EnumCount = _store.EnumChunk.Count;
      global.EnumValueCount = _store.EnumValueChunk.Count;
      
      global.ConstructorCount = _store.ConstructorChunk.Count;
      global.MethodCount = _store.MethodChunk.Count;
      global.FieldCount = _store.FieldChunk.Count;
      global.PropertyCount = _store.PropertyChunk.Count;
      
      return new ValueTask<ArchetypesCountResult>(result);
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