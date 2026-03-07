
using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class SymbolSpecDescriptorSerializer : SpecDescriptorSerializer<SymbolSpecDescriptor, SymbolSpec>
{
   private readonly ISerializer<NGramIndexDescriptor> _ngramSerializer = SerializerRegistry.For<NGramIndexDescriptor>();

   public override void Write(ref ByteWriter writer, ref SymbolSpecDescriptor value)
   {
      base.Write(ref writer, ref value);

      var indexes = value.Index;
      
      var fullPathName = indexes.FullPathName;
      _ngramSerializer.Write(ref writer, ref fullPathName);
   }

   public override bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out SymbolSpecDescriptor value)
   {
      value = null;
      
      if (!base.TryRead(ref reader, out value))
      {
         return false;
      }

      if (!_ngramSerializer.TryRead(ref reader, out var fullPathName))
      {
         return false;
      }

      value.Index = new SymbolSpecDescriptor.Indexes()
      {
         FullPathName = fullPathName
      };
      
      return true;
   }

   protected override SymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new SymbolSpecDescriptor()
      {
         FileName = fileName,
         Index = null!
      };
   }
}