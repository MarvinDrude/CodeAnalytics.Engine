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
   public required DbSet<ParameterComponent> ParameterComponents { get; set; }
   
   public required DbSet<FieldComponent> FieldComponents { get; set; }
   public required DbSet<ConstructorComponent> ConstructorComponents { get; set; }
   public required DbSet<MethodComponent> MethodComponents { get; set; }
   public required DbSet<PropertyComponent> PropertyComponents { get; set; }
   public required DbSet<EnumValueComponent> EnumValueComponents { get; set; }
   
   public required DbSet<ClassComponent> ClassComponents { get; set; }
   public required DbSet<EnumComponent> EnumComponents { get; set; }
   public required DbSet<InterfaceComponent> InterfaceComponents { get; set; }
   public required DbSet<StructComponent> StructComponents { get; set; }
   
   protected override void OnModelCreating(ModelBuilder builder)
   {
      // builder.Entity<FieldComponent>()
      //    .HasOne(x => x.Type)
      //    .WithMany(x => x.FieldMemberTypes)
      //    .HasForeignKey(x => x.TypeId)
      //    .IsRequired(false);
      // builder.Entity<FieldComponent>()
      //    .HasOne(x => x.ContainingType)
      //    .WithMany(x => x.FieldContainingTypes)
      //    .HasForeignKey(x => x.ContainingTypeId)
      //    .IsRequired();
      
      
   }

   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
      builder.UseSqlite(@"Data Source=C:\Users\marvi\RiderProjects2\Source\identifier.sqlite");
   }
}