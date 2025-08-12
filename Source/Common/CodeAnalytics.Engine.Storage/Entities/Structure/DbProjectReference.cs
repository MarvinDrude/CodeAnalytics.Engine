using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Structure;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbProjectReference
{
   public long Id { get; set; }
   
   [ForeignKey(nameof(SourceProjectId))]
   public DbProject? SourceProject { get; set; }
   public required long SourceProjectId { get; set; }
   
   [ForeignKey(nameof(ReferencedProjectId))]
   public DbProject? ReferencedProject { get; set; }
   public long ReferencedProjectId { get; set; }
   
   
}