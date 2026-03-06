using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;

public sealed class DatabaseDescriptorSerializer : ISerializer<DatabaseDescriptor>
{
   private readonly ISerializer<StructureDescriptor> _structureSerializer = SerializerRegistry.For<StructureDescriptor>();
   private readonly ISerializer<SymbolsDescriptor> _symbolsSerializer = SerializerRegistry.For<SymbolsDescriptor>();
   private readonly ISerializer<SymbolEdgeSpecDescriptor> _symbolSpecSerializer = SerializerRegistry.For<SymbolEdgeSpecDescriptor>();
   
   public void Write(ref ByteWriter writer, ref DatabaseDescriptor value)
   {
      var structure = value.Structure;
      _structureSerializer.Write(ref writer, ref structure);

      var symbols = value.Symbols;
      _symbolsSerializer.Write(ref writer, ref symbols);
      
      var edges = value.Edges;
      _symbolSpecSerializer.Write(ref writer, ref edges);
    }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out DatabaseDescriptor value)
   {
      value = null;
      if (!_structureSerializer.TryRead(ref reader, out var structure)
          || !_symbolsSerializer.TryRead(ref reader, out var symbols)
          || !_symbolSpecSerializer.TryRead(ref reader, out var symbolSpecs))
      {
         return false;
      }

      value = new DatabaseDescriptor()
      {
         Structure = structure,
         Symbols = symbols,
         Edges = symbolSpecs,
         BaseFolderPath = string.Empty
      };
      
      return true;
   }

   public int CalculateByteLength(ref DatabaseDescriptor value)
   {
      var structure = value.Structure;
      var edges = value.Edges;
      var symbols = value.Symbols;
      
      return _structureSerializer.CalculateByteLength(ref structure)
         + _symbolSpecSerializer.CalculateByteLength(ref edges)
         + _symbolsSerializer.CalculateByteLength(ref symbols);
   }
}