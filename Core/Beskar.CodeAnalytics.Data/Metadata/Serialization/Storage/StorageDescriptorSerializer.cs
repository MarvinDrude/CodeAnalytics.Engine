using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Storage;
using Me.Memory.Buffers;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Storage;

public sealed class StorageDescriptorSerializer : ISerializer<StorageDescriptor>
{
   public void Write(ref ByteWriter writer, ref StorageDescriptor value)
   {
      throw new NotImplementedException();
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out StorageDescriptor value)
   {
      throw new NotImplementedException();
   }

   public int CalculateByteLength(ref StorageDescriptor value)
   {
      throw new NotImplementedException();
   }
}