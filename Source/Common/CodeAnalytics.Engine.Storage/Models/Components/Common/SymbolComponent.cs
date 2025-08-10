using System.ComponentModel.DataAnnotations;
using CodeAnalytics.Engine.Storage.Enums.Components;
using CodeAnalytics.Engine.Storage.Models.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Members;
using CodeAnalytics.Engine.Storage.Models.Components.Types;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Common;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(NodeHash), IsUnique = true)]
public sealed class SymbolComponent
{
   public required long Id { get; set; }
   public required ComponentKind Kind { get; set; }
   
   [MaxLength(500)]
   public required string NodeHash { get; set; }
   
   [MaxLength(1500)]
   public required string Name { get; set; }
   [MaxLength(1500)]
   public required string MetadataName { get; set; }
   [MaxLength(5000)]
   public required string FullPathName { get; set; }
   
   public List<SymbolDeclaration> SymbolDeclarations { get; set; } = [];
}