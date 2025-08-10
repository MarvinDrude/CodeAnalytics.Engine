using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Common;

[Index(nameof(Id), IsUnique = true)]
public sealed class SymbolDeclaration
{
   public long Id { get; set; }
   
   public required int SpanIndex { get; set; }
   
   public required long FileReferenceId { get; set; }
   public required FileReference FileReference { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
}