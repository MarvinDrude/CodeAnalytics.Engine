using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;
using Beskar.CodeAnalytics.Data.Metadata.Specs;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;

public sealed class StructureDescriptorSerializer : ISerializer<StructureDescriptor>
{
   private readonly ISerializer<FolderSpecDescriptor> _folderSpecSerializer = SerializerRegistry.For<FolderSpecDescriptor>();
   
   public void Write(ref ByteWriter writer, ref StructureDescriptor value)
   {
      var folders = value.Folders;
      _folderSpecSerializer.Write(ref writer, ref folders);
      
      writer.WriteLittleEndian(value.RootFolderId);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out StructureDescriptor value)
   {
      value = null;

      if (!_folderSpecSerializer.TryRead(ref reader, out var folders))
      {
         return false;
      }

      var rootFolderId = reader.ReadLittleEndian<uint>();

      value = new StructureDescriptor()
      {
         Folders = folders,
         RootFolderId = rootFolderId
      };
      
      return true;
   }

   public int CalculateByteLength(ref StructureDescriptor value)
   {
      var folders = value.Folders;
      
      return _folderSpecSerializer.CalculateByteLength(ref folders) 
             + sizeof(uint);
   }
}