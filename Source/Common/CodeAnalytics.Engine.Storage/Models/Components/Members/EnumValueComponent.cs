using System.ComponentModel.DataAnnotations;
using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Types;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public class EnumValueComponent
{
   public long Id { get; set; }

   [MaxLength(1000)]
   public required string Name { get; set; }
   
   public long Value { get; set; }
   public ulong UValue { get; set; }
   
   public bool IsULong { get; set; }
   
   public required long ParentEnumId { get; set; }
   public required EnumComponent ParentEnum { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
}