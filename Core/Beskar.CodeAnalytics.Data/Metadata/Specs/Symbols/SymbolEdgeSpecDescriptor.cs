using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Readers;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class SymbolEdgeSpecDescriptor
   : SpecDescriptor<SymbolEdgeSpec>
{
   private static readonly int _structSize = Unsafe.SizeOf<SymbolEdgeSpec>();
   
   public MmfHandle.SpanView<SymbolEdgeSpec> LeaseTargetIds<T>(uint sourceId, StorageView<T> view)
      where T : unmanaged
   {
      var reader = GetReader();
      return reader.Lease(view.Offset * _structSize, view.Count);
   }

   public ArrayBuilderResult<uint> GetTargetIds(uint sourceId, SymbolEdgeType type)
   {
      var reader = GetReader();
      
      var firstIndex = FindFirstIndex(reader, 0, sourceId, type);
      if (firstIndex == -1) return null;
      
      ArrayBuilder<uint> result = new (12);

      try
      {
         using var lease = reader.Lease(firstIndex, reader.ItemCount - firstIndex);
         var span = lease.Span;
         var offset = 0;

         while (offset < span.Length
                && span[offset].SourceSymbolId == sourceId)
         {
            var current = span[offset++];
            if (current.Type != type) break;

            result.Add(current.TargetSymbolId);
         }
      }
      catch (Exception)
      {
         result.Dispose();
         throw;
      }
      
      return result;
   }

   public ArrayBuilderResult<uint> GetTargetIds(uint sourceId, params ReadOnlySpan<SymbolEdgeType> types)
   {
      if (types.Length == 0) return null;
      var reader = GetReader();
      
      var firstIndex = FindFirstIndex(reader, 0, sourceId, types[0]);
      if (firstIndex == -1) return null;
      
      ArrayBuilder<uint> result = new (12);
      try
      {
         using var lease = reader.Lease(firstIndex, reader.ItemCount - firstIndex);
         var span = lease.Span;

         var offset = 0;
         var typeIndex = 0;

         while (offset < span.Length && typeIndex < types.Length)
         {
            ref readonly var entry = ref span[offset];
            if (entry.SourceSymbolId != sourceId) break;

            var currentTargetType = types[typeIndex];
            if (entry.Type == currentTargetType)
            {
               result.Add(entry.TargetSymbolId);
               offset++;
            }
            else if (entry.Type < currentTargetType)
            {
               offset++;
            }
            else
            {
               typeIndex++;
            }
         }
      }
      catch (Exception)
      {
         result.Dispose();
         throw;
      }

      return result;
   }

   public ArrayBuilderResult<uint> GetTargetIds(IEnumerable<uint> sourceIds, SymbolEdgeType type)
   {
      var reader = GetReader();
      var sortedSources = sourceIds.Distinct().OrderBy(id => id).ToArray();
      
      ArrayBuilder<uint> result = new (12);

      try
      {
         using var lease = reader.LeaseAll();

         var span = lease.Span;
         var currentIdx = 0;

         foreach (var sourceId in sortedSources)
         {
            var firstIndex = FindFirstIndex(reader, currentIdx, sourceId, type);
            if (firstIndex == -1 || firstIndex >= span.Length) continue;

            currentIdx = firstIndex;
            while (currentIdx < span.Length)
            {
               ref readonly var entry = ref span[currentIdx];

               if (entry.SourceSymbolId != sourceId) break;
               if (entry.Type != type) break;

               result.Add(entry.TargetSymbolId);
               currentIdx++;
            }
         }
      }
      catch (Exception)
      {
         result.Dispose();
         throw;
      }
      
      return result;
   }

   private int FindFirstIndex(SpecFileReader<SymbolEdgeSpec> reader, int startIndex, uint sourceId, SymbolEdgeType type)
   {
      var low = startIndex;
      var high = reader.ItemCount - 1;
      var result = -1;
      
      using var lease = reader.LeaseAll();
      var span = lease.Span;

      while (low <= high)
      {
         var mid = low + ((high - low) / 2);
         ref readonly var midEntry = ref span[mid];

         var cmp = midEntry.SourceSymbolId.CompareTo(sourceId);
         if (cmp == 0)
         {
            cmp = midEntry.Type.CompareTo(type);
         }

         if (cmp == 0)
         {
            result = mid;
            high = mid - 1;
         }
         else if (cmp < 0)
         {
            low = mid + 1;
         }
         else
         {
            high = mid - 1;
         }
      }

      return result;
   }
   
   public override IComparer<SymbolEdgeSpec> Comparer => field ??= Comparer<SymbolEdgeSpec>.Create(
      static (x, y) =>
      {
         var compareSource = x.SourceSymbolId.CompareTo(y.SourceSymbolId);
         if (compareSource != 0) return compareSource;
         
         var compareType = x.Type.CompareTo(y.Type);
         if (compareType != 0) return compareType;
         
         return x.TargetSymbolId.CompareTo(y.TargetSymbolId);
      });
}