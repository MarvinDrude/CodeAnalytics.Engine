using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Metadata.Readers;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Storage;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.System;
using Beskar.CodeAnalytics.Data.Metadata.Specs;
using Me.Memory.Serialization;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization;

[SuppressMessage("Usage", "CA2255:The \'ModuleInitializer\' attribute should not be used in libraries")]
public static class Serialization
{
   [ModuleInitializer]
   public static void Initialize()
   {
      // Defaults
      SerializerRegistry.InitializeDefaults();
      
      // System
      SerializerRegistry.Register(new DateTimeOffsetSerializer());
      
      // Indexes
      SerializerRegistry.Register(new NGramIndexDescriptorSerializer());
      SerializerRegistry.Register(new BTreeIndexDescriptorSerializer<uint>());
      SerializerRegistry.Register(new BTreeIndexDescriptorSerializer<int>());
      
      // Storage
      SerializerRegistry.Register(new StorageDescriptorSerializer());
      SerializerRegistry.Register(new StorageFileDescriptorSerializer());
      
      // Spec descriptors
      SerializerRegistry.Register(new FolderSpecDescriptorSerializer());
      SerializerRegistry.Register(new ProjectSpecDescriptorSerializer());
      SerializerRegistry.Register(new SolutionSpecDescriptorSerializer());
      SerializerRegistry.Register(new FileSpecDescriptorSerializer());
      SerializerRegistry.Register(new SymbolLocationSpecDescriptorSerializer());
      
      // Symbols descriptors
      SerializerRegistry.Register(new FieldSymbolSpecDescriptorSerializer());
      SerializerRegistry.Register(new PropertySymbolSpecDescriptorSerializer());
      SerializerRegistry.Register(new MethodSymbolDescriptorSerializer());
      SerializerRegistry.Register(new ParameterSymbolSpecDescriptorSerializer());
      SerializerRegistry.Register(new TypeParameterSymbolSpecDescriptorSerializer());
      SerializerRegistry.Register(new TypeSymbolSpecDescriptorSerializer());
      SerializerRegistry.Register(new NamedTypeSymbolSpecDescriptorSerializer());
      SerializerRegistry.Register(new SymbolEdgeSpecDescriptorSerializer());
      SerializerRegistry.Register(new SymbolSpecDescriptorSerializer());
      
      // Descriptors
      SerializerRegistry.Register(new StringPoolDescriptorSerializer());
      SerializerRegistry.Register(new SyntaxFileDescriptorSerializer());
      SerializerRegistry.Register(new SymbolsDescriptorSerializer());
      SerializerRegistry.Register(new StructureDescriptorSerializer());
      SerializerRegistry.Register(new DatabaseDescriptorSerializer());
   }
}