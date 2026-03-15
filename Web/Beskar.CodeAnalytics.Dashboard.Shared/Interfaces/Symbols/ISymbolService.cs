using Beskar.CodeAnalytics.Dashboard.Shared.Models.Symbols;
using Beskar.CodeAnalytics.Data.Entities.Archetypes;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Symbols;

public interface ISymbolService
{
   public SymbolType GetSymbolType(uint symbolId);

   public ArchetypeCardModel<FieldArchetype> GetFieldCard(uint symbolId);
   public ArchetypeCardModel<MethodArchetype> GetMethodCard(uint symbolId);
   public ArchetypeCardModel<PropertyArchetype> GetPropertyCard(uint symbolId);
   public ArchetypeCardModel<TypeArchetype> GetTypeCard(uint symbolId);
   public ArchetypeCardModel<TypeParameterArchetype> GetTypeParameterCard(uint symbolId);
   public ArchetypeCardModel<NamedTypeArchetype> GetNamedTypeCard(uint symbolId);
}