using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Models.Syntax;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;

public sealed class LinePreviewsDescriptorSerializer : ISerializer<LinePreviewsDescriptor>
{
   private readonly ISerializer<string> _stringSerializer = SerializerRegistry.For<string>();
   
   public void Write(ref ByteWriter writer, ref LinePreviewsDescriptor value)
   {
      var fileName = value.FileName;
      _stringSerializer.Write(ref writer, ref fileName);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out LinePreviewsDescriptor value)
   {
      value = null;
      if (!_stringSerializer.TryRead(ref reader, out var fileName))
      {
         return false;
      }

      value = new LinePreviewsDescriptor()
      {
         FileName = fileName
      };
      
      return true;
   }

   public int CalculateByteLength(ref LinePreviewsDescriptor value)
   {
      var fileName = value.FileName;
      
      return _stringSerializer.CalculateByteLength(ref fileName);
   }
}