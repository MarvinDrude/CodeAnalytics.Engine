using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public abstract class SpecDescriptorSerializer<TSpecDescriptor, TSpec> : ISerializer<TSpecDescriptor>
   where TSpecDescriptor : SpecDescriptor<TSpec>
   where TSpec : unmanaged, ISpec
{
   private readonly ISerializer<string> _stringSerializer = SerializerRegistry.For<string>();
   
   protected abstract TSpecDescriptor CreateDescriptor(string fileName);
   
   public virtual void Write(ref ByteWriter writer, ref TSpecDescriptor value)
   {
      var fileName = value.FileName;
      _stringSerializer.Write(ref writer, ref fileName);
   }

   public virtual bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out TSpecDescriptor value)
   {
      value = null;
      if (!_stringSerializer.TryRead(ref reader, out var fileName))
      {
         return false;
      }

      value = CreateDescriptor(fileName);
      return true;
   }

   public virtual int CalculateByteLength(ref TSpecDescriptor value)
   {
      var fileName = value.FileName;
      return _stringSerializer.CalculateByteLength(ref fileName);
   }
}