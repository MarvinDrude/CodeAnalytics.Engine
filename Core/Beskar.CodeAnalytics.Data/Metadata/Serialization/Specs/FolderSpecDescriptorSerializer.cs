using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Specs;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public sealed class FolderSpecDescriptorSerializer : SpecDescriptorSerializer<FolderSpecDescriptor, FolderSpec>
{
   private readonly ISerializer<BTreeIndexDescriptor<uint>> _parentIndexSerializer = SerializerRegistry.For<BTreeIndexDescriptor<uint>>();
   
   public override void Write(ref ByteWriter writer, ref FolderSpecDescriptor value)
   {
      base.Write(ref writer, ref value);
      
      var indexes = value.Index;
      
      var parentId = indexes.ParentId;
      _parentIndexSerializer.Write(ref writer, ref parentId);
   }

   public override bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out FolderSpecDescriptor value)
   {
      value = null;
      
      if (!base.TryRead(ref reader, out value))
      {
         return false;
      }

      if (!_parentIndexSerializer.TryRead(ref reader, out var parentId))
      {
         return false;
      }
      
      value.Index = new FolderSpecDescriptor.Indexes()
      {
         ParentId = parentId
      };
      
      return true;
   }

   public override int CalculateByteLength(ref FolderSpecDescriptor value)
   {
      var indexes = value.Index ?? throw new InvalidOperationException();
      var parentId = indexes.ParentId;
      
      return base.CalculateByteLength(ref value)
         + _parentIndexSerializer.CalculateByteLength(ref parentId);
   }

   protected override FolderSpecDescriptor CreateDescriptor(string fileName)
   {
      return new FolderSpecDescriptor()
      {
         FileName = fileName,
         Index = null!
      };
   }
}