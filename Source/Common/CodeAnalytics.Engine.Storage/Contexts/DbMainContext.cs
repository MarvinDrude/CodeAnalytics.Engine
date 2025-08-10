using CodeAnalytics.Engine.Storage.Models.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Members;
using CodeAnalytics.Engine.Storage.Models.Components.Types;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Contexts;

public sealed class DbMainContext : DbContext
{
   public required DbSet<SolutionReference> SolutionReferences { get; set; }
   public required DbSet<ProjectReference> ProjectReferences { get; set; }
   public required DbSet<FileReference> FileReferences { get; set; }
   
   public required DbSet<SymbolComponent> SymbolComponents { get; set; }
   public required DbSet<SymbolDeclaration> SymbolDeclarations { get; set; }
   
   public required DbSet<FieldComponent> FieldComponents { get; set; }
   
   protected override void OnModelCreating(ModelBuilder builder)
   {
      builder.Entity<FieldComponent>()
         .HasOne(x => x.Type)
         .WithMany(x => x.FieldMemberTypes)
         .HasForeignKey(x => x.TypeId)
         .IsRequired(false);
      builder.Entity<FieldComponent>()
         .HasOne(x => x.ContainingType)
         .WithMany(x => x.FieldContainingTypes)
         .HasForeignKey(x => x.ContainingTypeId)
         .IsRequired();
      
      
   }
}