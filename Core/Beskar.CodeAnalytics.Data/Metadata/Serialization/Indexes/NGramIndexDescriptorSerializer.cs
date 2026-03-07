using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Indexes;

public sealed class NGramIndexDescriptorSerializer : ISerializer<NGramIndexDescriptor>
{
   private readonly ISerializer<string> _stringSerializer = SerializerRegistry.For<string>();
   
   public void Write(ref ByteWriter writer, ref NGramIndexDescriptor value)
   {
      var name = value.Name;
      _stringSerializer.Write(ref writer, ref name);
      
      var fileName = value.FileName;
      _stringSerializer.Write(ref writer, ref fileName);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out NGramIndexDescriptor value)
   {
      value = null;
      
      if (!_stringSerializer.TryRead(ref reader, out var name)
          || !_stringSerializer.TryRead(ref reader, out var fileName))
      {
         return false;
      }
      
      value = new NGramIndexDescriptor()
      {
         Name = name,
         FileName = fileName
      };
      
      return true;
   }

   public int CalculateByteLength(ref NGramIndexDescriptor value)
   {
      var name = value.Name;
      var fileName = value.FileName;
      
      return _stringSerializer.CalculateByteLength(ref name)
             + _stringSerializer.CalculateByteLength(ref fileName);
   }
}