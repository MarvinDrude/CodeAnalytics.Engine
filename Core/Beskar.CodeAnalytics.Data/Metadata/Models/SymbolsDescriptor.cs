using Beskar.CodeAnalytics.Data.Entities.Archetypes;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public sealed class SymbolsDescriptor
{
   public required FieldSymbolSpecDescriptor Fields { get; set; }
   
   public required MethodSymbolSpecDescriptor Methods { get; set; }
   
   public required PropertySymbolSpecDescriptor Properties { get; set; }
   
   public required SymbolSpecDescriptor Symbols { get; set; }
   
   public required TypeSymbolSpecDescriptor Types { get; set; }
   
   public required TypeParameterSymbolSpecDescriptor TypeParameters { get; set; }
   
   public required ParameterSymbolSpecDescriptor Parameters { get; set; }
   
   public required NamedTypeSymbolSpecDescriptor NamedTypes { get; set; }

   public FieldArchetype GetFieldArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (symbol.Type is not SymbolType.Field) 
         throw new InvalidOperationException();
      
      var field = Fields.GetReader().GetSpecById(symbolId);
      return new FieldArchetype
      {
         Symbol = symbol,
         Field = field
      };
   }

   public MethodArchetype GetMethodArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (symbol.Type is not SymbolType.Method) 
         throw new InvalidOperationException();
      
      var method = Methods.GetReader().GetSpecById(symbolId);
      return new MethodArchetype
      {
         Symbol = symbol,
         Method = method
      };
   }

   public PropertyArchetype GetPropertyArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (symbol.Type is not SymbolType.Property) 
         throw new InvalidOperationException();
      
      var property = Properties.GetReader().GetSpecById(symbolId);
      return new PropertyArchetype
      {
         Symbol = symbol,
         Property = property
      };
   }

   public TypeArchetype GetTypeArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (!symbol.Type.IsType) 
         throw new InvalidOperationException();
      
      var type = Types.GetReader().GetSpecById(symbolId);
      return new TypeArchetype
      {
         Symbol = symbol,
         Type = type
      };
   }

   public TypeParameterArchetype GetTypeParameterArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (symbol.Type is not SymbolType.TypeParameter) 
         throw new InvalidOperationException();
      
      var typeParameter = TypeParameters.GetReader().GetSpecById(symbolId);
      return new TypeParameterArchetype
      {
         Symbol = symbol,
         TypeParameter = typeParameter
      };
   }

   public ParameterArchetype GetParameterArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (symbol.Type is not SymbolType.Parameter) 
         throw new InvalidOperationException();
      
      var parameter = Parameters.GetReader().GetSpecById(symbolId);
      return new ParameterArchetype
      {
         Symbol = symbol,
         Parameter = parameter
      };
   }

   public NamedTypeArchetype GetNamedTypeArchetype(uint symbolId)
   {
      var symbol = Symbols.GetReader().GetSpecById(symbolId);
      if (symbol.Type is not SymbolType.NamedType) 
         throw new InvalidOperationException();
      
      var namedType = NamedTypes.GetReader().GetSpecById(symbolId);
      var type = Types.GetReader().GetSpecById(symbolId);
      
      return new NamedTypeArchetype
      {
         Symbol = symbol,
         Type = type,
         NamedType = namedType
      };
   }
   
   public async Task Initialize(DatabaseDescriptor database)
   {
      await Fields.Initialize(database);
      await Methods.Initialize(database);
      await Properties.Initialize(database);
      await Symbols.Initialize(database);
      await Types.Initialize(database);
      await TypeParameters.Initialize(database);
      await Parameters.Initialize(database);
      await NamedTypes.Initialize(database);
   }
}