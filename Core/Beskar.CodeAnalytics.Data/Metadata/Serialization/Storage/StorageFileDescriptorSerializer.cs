using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Storage;
using Me.Memory.Buffers;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Storage;

public sealed class StorageFileDescriptorSerializer : ISerializer<StorageFileDescriptor>
{
   public void Write(ref ByteWriter writer, ref StorageFileDescriptor value)
   {
      throw new NotImplementedException();
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out StorageFileDescriptor value)
   {
      throw new NotImplementedException();
   }

   public int CalculateByteLength(ref StorageFileDescriptor value)
   {
      throw new NotImplementedException();
   }
}