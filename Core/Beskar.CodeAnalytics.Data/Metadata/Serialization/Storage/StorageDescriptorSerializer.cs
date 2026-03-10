using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Storage;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Storage;

public sealed class StorageDescriptorSerializer : ISerializer<StorageDescriptor>
{
   private readonly ISerializer<List<StorageFileDescriptor>> _filesSerializer = SerializerRegistry.For<List<StorageFileDescriptor>>();
    
   public void Write(ref ByteWriter writer, ref StorageDescriptor value)
   {
      var files = value.Files;
      _filesSerializer.Write(ref writer, ref files);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out StorageDescriptor value)
   {
      if (!_filesSerializer.TryRead(ref reader, out var files))
      {
         value = null;
         return false;
      }

      value = new StorageDescriptor
      {
         Files = files
      };
      return true;
   }

   public int CalculateByteLength(ref StorageDescriptor value)
   {
      var files = value.Files;
      return _filesSerializer.CalculateByteLength(ref files);
   }
}