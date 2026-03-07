using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;

namespace Beskar.CodeAnalytics.Data.Metadata.Readers;

public sealed class SpecFileReader<TSpec> : ISpecFileReader
   where TSpec : unmanaged, ISpec
{
   private readonly MmfHandle _handle;
   private readonly int _structSize = Unsafe.SizeOf<TSpec>();
   private readonly int _itemCount;
   
   private readonly IComparer<TSpec> _comparer;
   
   public SpecFileReader(string filePath, IComparer<TSpec> comparer)
   {
      _handle = new MmfHandle(filePath, writable: false);
      _itemCount = (int)(_handle.Length / _structSize);
      
      _comparer = comparer;
   }

   public TSpec GetSpecByIndex(int index)
   {
      if (index < 0 || index >= _itemCount)
         throw new ArgumentOutOfRangeException(nameof(index));
      
      using var buffer = _handle.GetBuffer();
      // copies the spec so closing buffer is safe
      return buffer.GetRef<TSpec>(index * _structSize);
   }

   public TSpec GetSpecById(uint id)
   {
      using var buffer = _handle.GetBuffer();
      var span = buffer.GetSpan<TSpec>(0, _itemCount);
      var index = span.BinaryFindIndex(id);
      
      // copies the spec so closing buffer is safe
      return index == -1 
         ? throw new KeyNotFoundException() 
         : span[index];
   }

   public TSpec[] GetSpecsBySortedIds(scoped in ReadOnlySpan<uint> sortedIds)
   {
      var results = new TSpec[sortedIds.Length];
      var foundCount = 0;
      
      using var buffer = _handle.GetBuffer();
      var span = buffer.GetSpan<TSpec>(0, _itemCount);

      var sourceIdx = 0;
      foreach (var targetId in sortedIds)
      {
         while (sourceIdx < span.Length && span[sourceIdx].Identifier < targetId)
         {
            sourceIdx++;
         }

         if (sourceIdx < span.Length && span[sourceIdx].Identifier == targetId)
         {
            results[foundCount++] = span[sourceIdx];
         }
         else
         {
            throw new KeyNotFoundException($"ID {targetId} not found in spec file.");
         }
      }

      return results;
   }

   public MmfHandle.SpanView<TSpec> Lease(int index, int count)
   {
      return _handle.LeaseSpanView<TSpec>(index * _structSize, count);
   }

   public MmfHandle.SpanView<TSpec> LeaseAll()
   {
      return _handle.LeaseSpanView<TSpec>(0, _itemCount);
   }
   
   public void Dispose()
   {
      _handle.Dispose();
   }
}

public interface ISpecFileReader : IDisposable
{
   
}