using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Descriptors;
using Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;
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
      
      // Spec descriptors
      SerializerRegistry.Register(new FolderSpecDescriptorSerializer());
      
      // Descriptors
      SerializerRegistry.Register(new StructureDescriptorSerializer());
      SerializerRegistry.Register(new DatabaseDescriptorSerializer());
   }
}