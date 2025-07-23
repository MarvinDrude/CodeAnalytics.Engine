using System.Collections.Concurrent;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Merges.Common;

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

   public void Clean(MergableComponentPool<SymbolComponent, SymbolMerger> pool)
   {
      foreach (var key in _byNodes.Keys.ToList())
      {
         if (!pool.TryGetSlot(key, out _))
         {
            _byNodes.TryRemove(key, out _);
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