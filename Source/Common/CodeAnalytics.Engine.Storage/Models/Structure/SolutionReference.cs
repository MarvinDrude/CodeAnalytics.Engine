using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Structure;

[Index(nameof(Id))]
public sealed class SolutionReference
{
   public long Id { get; set; }
   
   [MaxLength(2000)]
   public required string Name { get; set; }
   [MaxLength(5000)]
   public required string RelativePath { get; set; }
   
   public List<ProjectReference> ProjectReferences { get; set; } = [];
}