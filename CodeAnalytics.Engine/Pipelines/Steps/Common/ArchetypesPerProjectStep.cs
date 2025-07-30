using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Pipelines.Common;

namespace CodeAnalytics.Engine.Pipelines.Steps.Common;

public sealed class ArchetypesPerProjectStep 
   : PipelineStepBase<AnalyzeStore, Dictionary<StringId, ArchetypeChunkViews>>
{
   public ArchetypesPerProjectStep(IPipelineCacheProvider cacheProvider, bool useCache) 
      : base(cacheProvider, useCache)
   {
   }

   protected override ValueTask<Dictionary<StringId, ArchetypeChunkViews>> Process(
      AnalyzeStore input, CancellationToken ct = default)
   {
      Dictionary<StringId, ArchetypeChunkViews> result = [];
      
      IterateArchetypes(result, ref input.ClassChunk.Entries, input, static (views, index) => views.Classes.Add(index));
      IterateArchetypes(result, ref input.EnumChunk.Entries, input, static (views, index) => views.Enums.Add(index));
      IterateArchetypes(result, ref input.EnumValueChunk.Entries, input, static (views, index) => views.EnumValues.Add(index));
      IterateArchetypes(result, ref input.StructChunk.Entries, input, static (views, index) => views.Structs.Add(index));
      IterateArchetypes(result, ref input.InterfaceChunk.Entries, input, static (views, index) => views.Interfaces.Add(index));
      
      IterateArchetypes(result, ref input.ConstructorChunk.Entries, input, static (views, index) => views.Constructors.Add(index));
      IterateArchetypes(result, ref input.MethodChunk.Entries, input, static (views, index) => views.Methods.Add(index));
      IterateArchetypes(result, ref input.FieldChunk.Entries, input, static (views, index) => views.Fields.Add(index));
      IterateArchetypes(result, ref input.PropertyChunk.Entries, input, static (views, index) => views.Properties.Add(index));
      
      return new ValueTask<Dictionary<StringId, ArchetypeChunkViews>>(result);
   }

   private void IterateArchetypes<TArchetype>(
      Dictionary<StringId, ArchetypeChunkViews> result,
      ref PooledList<TArchetype> archetypes, 
      AnalyzeStore input,
      Action<ArchetypeChunkViews, int> callback)
      where TArchetype : IArchetype, IEquatable<TArchetype>
   {
      for (var index = 0; index < archetypes.Count; index++)
      {
         ref var archetype = ref archetypes.GetByReference(index);
         
         foreach (var projectId in archetype.SymbolComponent.Projects)
         {
            var views = GetOrCreate(result, projectId, input);
            callback(views, index);
         }
      }
   }

   private ArchetypeChunkViews GetOrCreate(
      Dictionary<StringId, ArchetypeChunkViews> result, 
      StringId projectId,
      AnalyzeStore store)
   {
      if (!result.TryGetValue(projectId, out var chunk))
      {
         chunk = result[projectId] = new ArchetypeChunkViews(store);
      }

      return chunk;
   }
}