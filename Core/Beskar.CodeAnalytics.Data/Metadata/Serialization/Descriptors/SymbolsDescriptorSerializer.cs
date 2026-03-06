using System.Diagnostics.CodeAnalysis;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Me.Memory.Buffers;
using Me.Memory.Serialization;
using Me.Memory.Serialization.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;

public sealed class SymbolsDescriptorSerializer : ISerializer<SymbolsDescriptor>
{
   public void Write(ref ByteWriter writer, ref SymbolsDescriptor value)
   {
      var symbols = value.Symbols;
      _symbol.Write(ref writer, ref symbols);
      
      var fields = value.Fields;
      _field.Write(ref writer, ref fields);
      
      var properties = value.Properties;
      _property.Write(ref writer, ref properties);
      
      var parameters = value.Parameters;
      _parameter.Write(ref writer, ref parameters);
      
      var typeParameters = value.TypeParameters;
      _typeParameter.Write(ref writer, ref typeParameters);
      
      var namedTypes = value.NamedTypes;
      _namedType.Write(ref writer, ref namedTypes);
      
      var methods = value.Methods;
      _method.Write(ref writer, ref methods);
      
      var types = value.Types;
      _type.Write(ref writer, ref types);
   }

   public bool TryRead(ref ByteReader reader, [MaybeNullWhen(false)] out SymbolsDescriptor value)
   {
      value = null;

      if (!_symbol.TryRead(ref reader, out var symbols)
          || !_field.TryRead(ref reader, out var fields)
          || !_property.TryRead(ref reader, out var properties)
          || !_parameter.TryRead(ref reader, out var parameters)
          || !_typeParameter.TryRead(ref reader, out var typeParameters)
          || !_namedType.TryRead(ref reader, out var namedTypes)
          || !_method.TryRead(ref reader, out var methods)
          || !_type.TryRead(ref reader, out var types))
      {
         return false;
      }

      value = new SymbolsDescriptor()
      {
         Symbols = symbols,
         Fields = fields,
         Properties = properties,
         Parameters = parameters,
         TypeParameters = typeParameters,
         NamedTypes = namedTypes,
         Methods = methods,
         Types = types
      };
      
      return true;
   }

   public int CalculateByteLength(ref SymbolsDescriptor value)
   {
      var symbols = value.Symbols;
      var fields = value.Fields;
      var properties = value.Properties;
      var parameters = value.Parameters;
      var typeParameters = value.TypeParameters;
      var namedTypes = value.NamedTypes;
      var methods = value.Methods;
      var types = value.Types;
      
      return _symbol.CalculateByteLength(ref symbols) 
             + _field.CalculateByteLength(ref fields)
             + _property.CalculateByteLength(ref properties)
             + _parameter.CalculateByteLength(ref parameters)
             + _typeParameter.CalculateByteLength(ref typeParameters)
             + _namedType.CalculateByteLength(ref namedTypes)
             + _method.CalculateByteLength(ref methods)
             + _type.CalculateByteLength(ref types);
   }
   
   private readonly ISerializer<SymbolSpecDescriptor> _symbol = SerializerRegistry.For<SymbolSpecDescriptor>();
   private readonly ISerializer<FieldSymbolSpecDescriptor> _field = SerializerRegistry.For<FieldSymbolSpecDescriptor>();
   private readonly ISerializer<PropertySymbolSpecDescriptor> _property = SerializerRegistry.For<PropertySymbolSpecDescriptor>();
   private readonly ISerializer<ParameterSymbolSpecDescriptor> _parameter = SerializerRegistry.For<ParameterSymbolSpecDescriptor>();
   private readonly ISerializer<TypeParameterSymbolSpecDescriptor> _typeParameter = SerializerRegistry.For<TypeParameterSymbolSpecDescriptor>();
   private readonly ISerializer<NamedTypeSymbolSpecDescriptor> _namedType = SerializerRegistry.For<NamedTypeSymbolSpecDescriptor>();
   private readonly ISerializer<MethodSymbolSpecDescriptor> _method = SerializerRegistry.For<MethodSymbolSpecDescriptor>();
   private readonly ISerializer<TypeSymbolSpecDescriptor> _type = SerializerRegistry.For<TypeSymbolSpecDescriptor>();
}