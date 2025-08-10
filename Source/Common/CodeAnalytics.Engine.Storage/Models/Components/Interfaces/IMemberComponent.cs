using CodeAnalytics.Engine.Storage.Models.Components.Common;

namespace CodeAnalytics.Engine.Storage.Models.Components.Interfaces;

public interface IMemberComponent
{
   public long ContainingTypeId { get; set; }
   public SymbolComponent ContainingType { get; set; }
}