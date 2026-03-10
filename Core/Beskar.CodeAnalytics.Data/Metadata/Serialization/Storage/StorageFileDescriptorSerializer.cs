using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Enums.Storage;
using Beskar.CodeAnalytics.Data.Metadata.Storage;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Storage;

public sealed class StorageFileDescriptorSerializer : ISerializer<StorageFileDescriptor>
{
   private readonly ISerializer<string> _stringSerializer = SerializerRegistry.For<string>();
   private readonly ISerializer<DateTimeOffset> _dateTimeSerializer = SerializerRegistry.For<DateTimeOffset>();
   
   public void Write(ref ByteWriter writer, ref StorageFileDescriptor value)
   {
      var parentName = value.ParentName;
      _stringSerializer.Write(ref writer, ref parentName);
      
      var name = value.Name;
      _stringSerializer.Write(ref writer, ref name);
      
      var fileName = value.FileName;
      _stringSerializer.Write(ref writer, ref fileName);

      writer.WriteLittleEndian(value.Kind);
      
      var modified = value.LastModified;
      _dateTimeSerializer.Write(ref writer, ref modified);
      
      writer.WriteLittleEndian(value.ByteCount);
      writer.WriteLittleEndian(value.RowCount);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out StorageFileDescriptor value)
   {
      if (!_stringSerializer.TryRead(ref reader, out var parentName)
          || !_stringSerializer.TryRead(ref reader, out var name)
          || !_stringSerializer.TryRead(ref reader, out var fileName))
      {
         value = null;
         return false;
      }
      
      var kind = reader.ReadLittleEndian<StorageFileKind>();

      if (!_dateTimeSerializer.TryRead(ref reader, out var modified))
      {
         value = null;
         return false;
      }

      var byteCount = reader.ReadLittleEndian<ulong>();
      var rowCount = reader.ReadLittleEndian<ulong>();

      value = new StorageFileDescriptor()
      {
         FileName = fileName,
         ParentName = parentName,
         Name = name,
         
         Kind = kind,
         LastModified = modified,
         
         ByteCount = byteCount,
         RowCount = rowCount
      };
      
      return true;
   }

   public int CalculateByteLength(ref StorageFileDescriptor value)
   {
      var modified = value.LastModified;
      var parentName = value.ParentName;
      var name = value.Name;
      var fileName = value.FileName;
      
      return _stringSerializer.CalculateByteLength(ref parentName)
         + _stringSerializer.CalculateByteLength(ref name)
         + _stringSerializer.CalculateByteLength(ref fileName)
         + sizeof(StorageFileKind)
         + _dateTimeSerializer.CalculateByteLength(ref modified)
         + sizeof(long)
         + sizeof(long);
   }
}