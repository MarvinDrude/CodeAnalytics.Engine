using System.Collections.Concurrent;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Merges.Common;
using CodeAnalytics.Engine.Merges.Members;

namespace CodeAnalytics.Engine.Occurrences;

public sealed class OccurrenceRegistry
{
   private readonly ConcurrentDictionary<NodeId, Lazy<GlobalOccurrence>> _byNodes = [];

   public OccurrenceRegistry()
   {
      
   }

   public OccurrenceRegistry(Dictionary<NodeId, GlobalOccurrence> dict)
   {
      foreach (var (key, value) in dict)
      {
         CreateKeyValue(key, value);
      }
   }

   public void AddOccurrence(
      ref SyntaxSpan span, List<SyntaxSpan> lineSpans, 
      StringId projectId, StringId fileId, int index,
      int lineNumber)
   {
      var all = GetOrCreate(span.Reference);
      var project = all.GetOrCreateByProject(projectId);
      var file = project.GetOrCreate(fileId);
      
      file.LineOccurrences.Add(new NodeOccurrence()
      {
         LineSpans = lineSpans,
         IsDeclaration = span.IsDeclaration,
         SpanIndex = index,
         LineNumber = lineNumber,
      });

      if (span.IsDeclaration)
      {
         all.AddDeclaration(new DeclarationOccurrence()
         {
            NodeId = span.Reference,
            FileId = fileId,
            LineNumber = lineNumber,
            SpanIndex = index
         });
      }
   }

   public void Clean(MergableComponentPool<SymbolComponent, SymbolMerger> pool, CollectorStore store)
   {
      foreach (var key in _byNodes.Keys.ToList())
      {
         if (!pool.TryGetSlot(key, out var slot))
         {
            _byNodes.TryRemove(key, out _);
            continue;
         }

         ref var component = ref pool.Entries.GetByReference(slot);
         var occurrence = Get(key) ?? throw new NullReferenceException();

         foreach (var declr in occurrence.Declarations)
         {
            component.Declarations.Add(new FileLocationId(declr.FileId, declr.SpanIndex));
         }
      }
      
      ConnectMethods(store);
      ConnectProperties(store);
   }

   private void ConnectMethods(CollectorStore store)
   {
      var methods = store.ComponentStore.GetOrCreatePool<MethodComponent, MethodMerger>();

      foreach (ref var method in methods.Entries)
      {
         if (store.Occurrences.Get(method.Id) is not { } concrete)
         {
            continue;
         }
         
         if (!method.OverrideId.IsEmpty 
             && store.Occurrences.Get(method.OverrideId) is { } baseMethod)
         {
            baseMethod.MergeDeclarations(concrete);
         }

         foreach (ref var id in method.InterfaceImplementations)
         {
            if (store.Occurrences.Get(id) is { } interFace)
            {
               interFace.MergeDeclarations(concrete);
            }
         }
      }
   }

   private void ConnectProperties(CollectorStore store)
   {
      var properties = store.ComponentStore.GetOrCreatePool<PropertyComponent, PropertyMerger>();

      foreach (ref var property in properties.Entries)
      {
         if (store.Occurrences.Get(property.Id) is not { } concrete)
         {
            continue;
         }
         
         if (!property.OverrideId.IsEmpty 
             && store.Occurrences.Get(property.OverrideId) is { } baseProperty)
         {
            baseProperty.MergeDeclarations(concrete);
         }

         foreach (ref var id in property.InterfaceImplementations)
         {
            if (store.Occurrences.Get(id) is { } interFace)
            {
               interFace.MergeDeclarations(concrete);
            }
         }
      }
   }

   public GlobalOccurrence? Get(NodeId nodeId)
   {
      return _byNodes.GetValueOrDefault(nodeId)?.Value ?? null;
   }

   public GlobalOccurrence GetOrCreate(NodeId nodeId)
   {
      var lazy = _byNodes.GetOrAdd(nodeId, _ => new Lazy<GlobalOccurrence>(
         () => new GlobalOccurrence()
         {
            NodeId = nodeId
         },
         LazyThreadSafetyMode.ExecutionAndPublication));
      
      return lazy.Value;
   }

   public Dictionary<NodeId, GlobalOccurrence> ToDictionary()
   {
      return _byNodes.ToDictionary(x => x.Key, x => x.Value.Value);
   }

   private void CreateKeyValue(NodeId nodeId, GlobalOccurrence occurrence)
   {
      _byNodes[nodeId] = new Lazy<GlobalOccurrence>(occurrence);
   }
}