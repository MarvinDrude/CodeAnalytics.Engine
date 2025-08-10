using System.ComponentModel.DataAnnotations;
using CodeAnalytics.Engine.Storage.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Structure;

[Index(nameof(Id))]
public sealed class ProjectReference
{
   public long Id { get; set; }
   
   [MaxLength(2000)]
   public required string Name { get; set; }
   [MaxLength(5000)]
   public required string RelativePath { get; set; }

   public List<SymbolDeclaration> SymbolDeclarations { get; set; } = [];
   
   public long SolutionReferenceId { get; set; }
   public required SolutionReference SolutionReference { get; set; }
}