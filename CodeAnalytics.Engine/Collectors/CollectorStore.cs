using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Ids;
using CodeAnalytics.Engine.Occurrences;
using CodeAnalytics.Engine.Serialization.Stores;

namespace CodeAnalytics.Engine.Collectors;

public sealed class CollectorStore : IDisposable
{
   public required NodeIdStore NodeIdStore { get; init; }
   public required StringIdStore StringIdStore { get; init; }
   
   public required OccurrenceRegistry Occurrences { get; init; }
   public required HashSet<StringId> Projects { get; init; }
   
   public required MergableComponentStore ComponentStore { get; init; }
   public required LineCountStore LineCountStore { get; init; }

   public void Merge(CollectorStore collectorStore)
   {
      ComponentStore.Merge(collectorStore.ComponentStore);
      LineCountStore.Merge(collectorStore.LineCountStore);

      foreach (var project in collectorStore.Projects)
      {
         Projects.Add(project);
      }
   }

   public void Dispose()
   {
      ComponentStore.Dispose();
   }

   public Memory<byte> ToMemory()
   {
      var writer = new ByteWriter(stackalloc byte[512], 2048);

      try
      {
         var self = this;
         CollectorStoreSerializer.Serialize(ref writer, ref self);
         
         return writer.WrittenSpan.ToArray();
      }
      finally
      {
         writer.Dispose();
      }
   }
   
   public static CollectorStore? FromMemory(Memory<byte> memory)
   {
      var reader = new ByteReader(memory.Span);

      return CollectorStoreSerializer.TryDeserialize(ref reader, out var store) 
         ? store : null;
   }
}