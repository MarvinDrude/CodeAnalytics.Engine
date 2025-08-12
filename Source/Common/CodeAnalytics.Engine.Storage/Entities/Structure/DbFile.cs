using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Structure;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(Name), IsUnique = false)]
[Index(nameof(RelativeFilePath), IsUnique = true)]
public sealed class DbFile
{
   public long Id { get; set; }
   
   [MaxLength(1000)]
   public required string Name { get; set; }
   [MaxLength(3000)]
   public required string RelativeFilePath { get; set; }
   
   [ForeignKey(nameof(ProjectId))]
   public DbProject? Project { get; set; }
   public required long ProjectId { get; set; }
}