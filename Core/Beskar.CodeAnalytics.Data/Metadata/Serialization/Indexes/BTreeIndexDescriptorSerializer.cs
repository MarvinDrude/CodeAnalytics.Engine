using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Indexes.Cache;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Indexes;

public sealed class BTreeIndexDescriptorSerializer<TKey> : ISerializer<BTreeIndexDescriptor<TKey>>
   where TKey : unmanaged, IComparable<TKey>
{
   private readonly ISerializer<TKey> _keySerializer = SerializerRegistry.For<TKey>();
   private readonly ISerializer<string> _stringSerializer = SerializerRegistry.For<string>();
   
   public void Write(ref ByteWriter writer, ref BTreeIndexDescriptor<TKey> value)
   {
      var name = value.Name;
      _stringSerializer.Write(ref writer, ref name);
      
      var fileName = value.FileName;
      _stringSerializer.Write(ref writer, ref fileName);

      var comparerId = IndexComparerRegistry<TKey>.GetId(value.Comparer) 
         ?? throw new InvalidOperationException();
      _stringSerializer.Write(ref writer, ref comparerId);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out BTreeIndexDescriptor<TKey> value)
   {
      value = null;
      
      if (!_stringSerializer.TryRead(ref reader, out var name)
          || !_stringSerializer.TryRead(ref reader, out var fileName)
          || !_stringSerializer.TryRead(ref reader, out var comparerId))
      {
         return false;
      }
      
      value = new BTreeIndexDescriptor<TKey>()
      {
         Comparer = IndexComparerRegistry<TKey>.GetComparer(comparerId) 
            ?? throw new InvalidOperationException(),
         Name = name,
         FileName = fileName
      };
      
      return true;
   }

   public int CalculateByteLength(ref BTreeIndexDescriptor<TKey> value)
   {
      var name = value.Name;
      var fileName = value.FileName;
      var comparerId = IndexComparerRegistry<TKey>.GetId(value.Comparer) 
         ?? throw new InvalidOperationException();
      
      return _stringSerializer.CalculateByteLength(ref name)
         + _stringSerializer.CalculateByteLength(ref fileName)
         + _stringSerializer.CalculateByteLength(ref comparerId);
   }
}