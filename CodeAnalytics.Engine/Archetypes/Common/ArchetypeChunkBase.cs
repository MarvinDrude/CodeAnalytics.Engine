using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;

namespace CodeAnalytics.Engine.Archetypes.Common;

public abstract class ArchetypeChunkBase<TArchetype> : IDisposable
   where TArchetype : IArchetype, IEquatable<TArchetype>
{
   public int Count => _entries.Count;
   protected PooledList<TArchetype> _entries;
   
   public ref PooledList<TArchetype> Entries => ref _entries;

   public void Dispose()
   {
      if (_entries.IsDisposed)
      {
         return;
      }
      
      GC.SuppressFinalize(this);

      foreach (ref var entry in _entries)
      {
         entry.Dispose();
      }
      
      _entries.Dispose();
   }
}