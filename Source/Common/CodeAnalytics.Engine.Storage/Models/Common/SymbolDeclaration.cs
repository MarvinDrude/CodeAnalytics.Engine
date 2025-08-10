using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Common;

[Index(nameof(Id))]
public sealed class SymbolDeclaration
{
   public long Id { get; set; }
   
   public required int SpanIndex { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
   
   public required long ProjectReferenceId { get; set; }
   public required ProjectReference ProjectReference { get; set; }
}