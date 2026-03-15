using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Symbols;
using Beskar.CodeAnalytics.Dashboard.Shared.Models.Symbols;
using Beskar.CodeAnalytics.Data.Entities.Archetypes;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Dashboard.Services.Symbols;

public sealed class SymbolService(IDatabaseProvider provider) : ISymbolService
{
   private readonly IDatabaseProvider _provider = provider;
   private DatabaseDescriptor Descriptor => _provider.GetDescriptor();

   public SymbolType GetSymbolType(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      return db.Symbols.GetReader().GetSpecById(symbolId).Type;
   }
   
   public ArchetypeCardModel<FieldArchetype> GetFieldCard(uint symbolId)
   {
      var db = Descriptor.Symbols;

      return new ArchetypeCardModel<FieldArchetype>()
      {
         Archetype = db.GetFieldArchetype(symbolId),
      };
   }

   public ArchetypeCardModel<MethodArchetype> GetMethodCard(uint symbolId)
   {
      var db = Descriptor.Symbols;

      return new ArchetypeCardModel<MethodArchetype>()
      {
         Archetype = db.GetMethodArchetype(symbolId),
      };
   }

   public ArchetypeCardModel<NamedTypeArchetype> GetNamedTypeCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      return new ArchetypeCardModel<NamedTypeArchetype>()
      {
         Archetype = db.GetNamedTypeArchetype(symbolId),
      };
   }
   
   public ArchetypeCardModel<PropertyArchetype> GetPropertyCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      return new ArchetypeCardModel<PropertyArchetype>()
      {
         Archetype = db.GetPropertyArchetype(symbolId),
      };
   }

   public ArchetypeCardModel<TypeArchetype> GetTypeCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      return new ArchetypeCardModel<TypeArchetype>()
      {
         Archetype = db.GetTypeArchetype(symbolId),
      };
   }
   
   public ArchetypeCardModel<TypeParameterArchetype> GetTypeParameterCard(uint symbolId)
   {
      var db = Descriptor.Symbols;
      
      return new ArchetypeCardModel<TypeParameterArchetype>()
      {
         Archetype = db.GetTypeParameterArchetype(symbolId),
      };
   }
}