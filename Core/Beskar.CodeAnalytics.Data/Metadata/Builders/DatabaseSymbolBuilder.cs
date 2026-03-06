using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Builders;

public sealed class DatabaseSymbolBuilder
{
   private string? _fileNameSymbolSpec;
   private string? _fileNameFieldSymbolSpec;
   private string? _fileNameMethodSymbolSpec;
   private string? _fileNamePropertySymbolSpec;
   private string? _fileNameTypeSymbolSpec;
   private string? _fileNameTypeParameterSymbolSpec;
   private string? _fileNameNamedTypeSymbolSpec;
   private string? _fileNameParameterSymbolSpec;
   
   public DatabaseSymbolBuilder WithSymbolSpec(string fileName)
   {
      _fileNameSymbolSpec = fileName;
      return this;
   }
   
   public DatabaseSymbolBuilder WithFieldSymbolSpec(string fileName)
   {
      _fileNameFieldSymbolSpec = fileName;
      return this;
   }

   public DatabaseSymbolBuilder WithMethodSymbolSpec(string fileName)
   {
      _fileNameMethodSymbolSpec = fileName;
      return this;
   }

   public DatabaseSymbolBuilder WithPropertySpec(string fileName)
   {
      _fileNamePropertySymbolSpec = fileName;
      return this;
   }

   public DatabaseSymbolBuilder WithTypeSymbolSpec(string fileName)
   {
      _fileNameTypeSymbolSpec = fileName;
      return this;
   }

   public DatabaseSymbolBuilder WithTypeParameterSymbolSpec(string fileName)
   {
      _fileNameTypeParameterSymbolSpec = fileName;
      return this;
   }

   public DatabaseSymbolBuilder WithNamedTypeSymbolSpec(string fileName)
   {
      _fileNameNamedTypeSymbolSpec = fileName;
      return this;
   }
   
   public DatabaseSymbolBuilder WithParameterSymbolSpec(string fileName)
   {
      _fileNameParameterSymbolSpec = fileName;
      return this;
   }
   
   public SymbolsDescriptor Build()
   {
      return new SymbolsDescriptor()
      {
         Symbols = new SymbolSpecDescriptor()
         {
            FileName = _fileNameSymbolSpec 
               ?? throw new InvalidOperationException("Symbol spec is not set")
         },
         Fields = new FieldSymbolSpecDescriptor()
         {
            FileName = _fileNameFieldSymbolSpec 
               ?? throw new InvalidOperationException("Field symbol spec is not set")
         },
         Methods = new MethodSymbolSpecDescriptor()
         {
            FileName = _fileNameMethodSymbolSpec 
               ?? throw new InvalidOperationException("Method symbol spec is not set")
         },
         NamedTypes = new NamedTypeSymbolSpecDescriptor()
         {
            FileName = _fileNameNamedTypeSymbolSpec 
               ?? throw new InvalidOperationException("Named type symbol spec is not set")
         },
         Parameters = new ParameterSymbolSpecDescriptor()
         {
            FileName = _fileNameParameterSymbolSpec 
               ?? throw new InvalidOperationException("Parameter symbol spec is not set")
         },
         Properties = new PropertySymbolSpecDescriptor()
         {
            FileName = _fileNamePropertySymbolSpec 
               ?? throw new InvalidOperationException("Property symbol spec is not set")
         },
         TypeParameters = new TypeParameterSymbolSpecDescriptor()
         {
            FileName = _fileNameTypeParameterSymbolSpec 
               ?? throw new InvalidOperationException("Type parameter symbol spec is not set")
         },
         Types = new TypeSymbolSpecDescriptor()
         {
            FileName = _fileNameTypeSymbolSpec 
               ?? throw new InvalidOperationException("Type symbol spec is not set")
         }
      };
   }
}