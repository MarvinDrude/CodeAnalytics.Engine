using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;

namespace CodeAnalytics.Engine.Archetypes.Common;

public sealed class ArchetypeChunkView<TArchetype>
   where TArchetype : IArchetype, IEquatable<TArchetype>
{
   public int Count => _indexes.Count;
   
   private readonly ArchetypeChunkBase<TArchetype> _chunk;
   private readonly List<int> _indexes = [];
   
   public ArchetypeChunkView(ArchetypeChunkBase<TArchetype> archetypes)
   {
      _chunk = archetypes;
   }

   public void Add(int index)
   {
      _indexes.Add(index);
   }
   
   public delegate void ArchetypeDelegate<TArch>(ref TArch archetype) 
      where TArch : IArchetype, IEquatable<TArch>;

   public Enumerator GetEnumerator() => new (_chunk, CollectionsMarshal.AsSpan(_indexes));
   
   public ref struct Enumerator
   {
      private readonly ArchetypeChunkBase<TArchetype> _chunk;
      private readonly ReadOnlySpan<int> _indexes;
      private int _index;

      public Enumerator(ArchetypeChunkBase<TArchetype> chunk, ReadOnlySpan<int> indexes)
      {
         _chunk = chunk;
         _indexes = indexes;

         _index = -1;
      }

      public ref TArchetype Current
      {
         [MethodImpl(MethodImplOptions.AggressiveInlining)]
         get => ref _chunk.Entries.GetByReference(_indexes[_index]);
      }
      
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext() => ++_index < _indexes.Length;
   }
}