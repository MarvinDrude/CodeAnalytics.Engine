using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class MethodSymbolDescriptorSerializer : SpecDescriptorSerializer<MethodSymbolSpecDescriptor, MethodSymbolSpec>
{
   private readonly ISerializer<BTreeIndexDescriptor<int>> _parameterCountSerializer = SerializerRegistry.For<BTreeIndexDescriptor<int>>();
   
   public override void Write(ref ByteWriter writer, ref MethodSymbolSpecDescriptor value)
   {
      base.Write(ref writer, ref value);
      
      var indexes = value.Index ?? throw new InvalidOperationException();
      
      var parameterCount = indexes.ParameterCount;
      _parameterCountSerializer.Write(ref writer, ref parameterCount);
   }

   public override bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out MethodSymbolSpecDescriptor value)
   {
      value = null;
      
      if (!base.TryRead(ref reader, out value))
      {
         return false;
      }

      if (!_parameterCountSerializer.TryRead(ref reader, out var parameterCount))
      {
         return false;
      }
      
      value.Index = new MethodSymbolSpecDescriptor.Indexes()
      {
         ParameterCount = parameterCount
      };
      
      return true;
   }

   public override int CalculateByteLength(ref MethodSymbolSpecDescriptor value)
   {
      var indexes = value.Index ?? throw new InvalidOperationException();
      var parameterCount = indexes.ParameterCount;
      
      return base.CalculateByteLength(ref value)
             + _parameterCountSerializer.CalculateByteLength(ref parameterCount);
   }
   
   protected override MethodSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new MethodSymbolSpecDescriptor()
      {
         FileName = fileName,
         Index = null!
      };
   }
}