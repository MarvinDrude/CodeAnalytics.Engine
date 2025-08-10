using System.ComponentModel.DataAnnotations;
using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Interfaces;
using CodeAnalytics.Engine.Storage.Models.Components.Types;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public class EnumValueComponent : IComponent, IMemberComponent
{
   public long Id { get; set; }

   [MaxLength(1000)]
   public required string Name { get; set; }
   
   public long Value { get; set; }
   public ulong UValue { get; set; }
   
   public bool IsULong { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
   
   public required long ContainingTypeId { get; set; }
   public required SymbolComponent ContainingType { get; set; }
}