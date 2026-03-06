using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;

public sealed class DatabaseDescriptorSerializer : ISerializer<DatabaseDescriptor>
{
   private readonly ISerializer<StructureDescriptor> _structureSerializer = SerializerRegistry.For<StructureDescriptor>();
   
   public void Write(ref ByteWriter writer, ref DatabaseDescriptor value)
   {
      var structure = value.Structure;
      _structureSerializer.Write(ref writer, ref structure);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out DatabaseDescriptor value)
   {
      value = null;
      if (!_structureSerializer.TryRead(ref reader, out var structure))
      {
         return false;
      }

      value = new DatabaseDescriptor()
      {
         Structure = structure,
         BaseFolderPath = string.Empty
      };
      
      return true;
   }

   public int CalculateByteLength(ref DatabaseDescriptor value)
   {
      var structure = value.Structure;
      
      return _structureSerializer.CalculateByteLength(ref structure);
   }
}