using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Structure;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(Name), IsUnique = false)]
[Index(nameof(RelativeFilePath), IsUnique = true)]
public sealed class DbProject
{
   public long Id { get; set; }
   
   [MaxLength(1000)]
   public required string Name { get; set; }
   [MaxLength(3000)]
   public required string RelativeFilePath { get; set; }
   
   [MaxLength(1000)]
   public required string AssemblyName { get; set; }

   [InverseProperty(nameof(DbSolution.Projects))] 
   public List<DbSolution> Solutions { get; set; } = [];

   [InverseProperty(nameof(DbFile.Projects))]
   public List<DbFile> Files { get; set; } = [];
   
   [InverseProperty(nameof(DbProjectReference.SourceProject))]
   public List<DbProjectReference> ProjectReferences { get; set; } = [];
}