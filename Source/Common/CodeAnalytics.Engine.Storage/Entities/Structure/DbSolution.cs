using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Structure;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(Name), IsUnique = false)]
[Index(nameof(RelativeFilePath), IsUnique = true)]
public sealed class DbSolution
{
   public long Id { get; set; }
   
   [MaxLength(1000)]
   public required string Name { get; set; }
   [MaxLength(3000)]
   public required string RelativeFilePath { get; set; }
   
   [InverseProperty(nameof(DbProject.Solution))]
   public List<DbProject> Projects { get; set; } = [];
}