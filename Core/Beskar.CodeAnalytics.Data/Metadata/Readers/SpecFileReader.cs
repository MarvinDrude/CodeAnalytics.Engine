using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Metadata.Readers;

public sealed class SpecFileReader<TSpec> : ISpecFileReader
   where TSpec : unmanaged, ISpec
{
   public int ItemCount => _itemCount;
   
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

   public ArrayBuilderResult<TSpec> GetSpecsBySortedIds(scoped in ReadOnlySpan<uint> sortedIds)
   {
      var results = new ArrayBuilder<TSpec>(sortedIds.Length);
      
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
            results.Add(span[sourceIdx]);
         }
         else
         {
            throw new KeyNotFoundException($"ID {targetId} not found in spec file.");
         }
      }

      return results;
   }

   public MmfHandle.SpanView<TSpec> LeaseById(uint id)
   {
      var buffer = _handle.GetBuffer();
      var span = buffer.GetSpan<TSpec>(0, _itemCount);
      
      var (start, length) = FindRange(span, id);

      return length == 0 
         ? new MmfHandle.SpanView<TSpec>(buffer, 0, 0) 
         : new MmfHandle.SpanView<TSpec>(buffer, start * _structSize, length);
   }

   public MmfHandle.SpanView<TSpec> Lease(int index, int count)
   {
      return _handle.LeaseSpanView<TSpec>(index * _structSize, count);
   }

   public MmfHandle.SpanView<TSpec> LeaseAll()
   {
      return _handle.LeaseSpanView<TSpec>(0, _itemCount);
   }

   public int FindFirstIndex(uint id)
   {
      using var buffer = _handle.GetBuffer();
      var span = buffer.GetSpan<TSpec>(0, _itemCount);
      
      return span.BinaryFindIndex(id);
   }
   
   private (int start, int length) FindRange(
      scoped in ReadOnlySpan<TSpec> span, uint id)
   {
      int first = -1;
      int last = -1;

      // Find the first occurrence
      int low = 0;
      int high = span.Length - 1;
      while (low <= high)
      {
         int mid = low + (high - low) / 2;
         if (span[mid].Identifier >= id)
         {
            if (span[mid].Identifier == id) first = mid;
            high = mid - 1;
         }
         else
         {
            low = mid + 1;
         }
      }

      if (first == -1) return (0, 0);

      // Find the last occurrence
      low = first;
      high = span.Length - 1;
      while (low <= high)
      {
         int mid = low + (high - low) / 2;
         if (span[mid].Identifier <= id)
         {
            if (span[mid].Identifier == id) last = mid;
            low = mid + 1;
         }
         else
         {
            high = mid - 1;
         }
      }

      return (first, last - first + 1);
   }
   
   public void Dispose()
   {
      _handle.Dispose();
   }
}

public interface ISpecFileReader : IDisposable
{
   
}