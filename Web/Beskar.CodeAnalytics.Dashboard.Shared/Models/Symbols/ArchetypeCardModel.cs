using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Symbols;

public sealed class ArchetypeCardModel<TArchetype>
   where TArchetype : unmanaged
{
   public required TArchetype Archetype { get; init; }
   
   public Dictionary<StringFileView, string> Strings { get; init; } = [];
}