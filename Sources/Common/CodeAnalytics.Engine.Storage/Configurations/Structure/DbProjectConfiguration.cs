using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Structure;

public sealed class DbProjectConfiguration : IEntityTypeConfiguration<DbProject>
{
   public void Configure(EntityTypeBuilder<DbProject> builder)
   {
      builder.HasKey(x => x.Id);
      
      builder.Property(x => x.Id)
         .HasConversion(IdConverter)
         .ValueGeneratedOnAdd();

      builder.HasIndex(x => x.Name)
         .IsUnique(false);
      builder.Property(x => x.Name)
         .HasMaxLength(2_000);

      builder.HasIndex(x => x.RelativeFilePath)
         .IsUnique();
      builder.Property(x => x.RelativeFilePath)
         .HasMaxLength(2_000);

      builder.HasIndex(x => x.AssemblyName)
         .IsUnique();
      builder.Property(x => x.AssemblyName)
         .HasMaxLength(2_000);

      builder.HasMany(x => x.Solutions)
         .WithMany(x => x.Projects);

      builder.HasMany(x => x.ReferencedProjects)
         .WithMany(x => x.ReferencedByProjects);
   }

   public static readonly ValueConverter<DbProjectId, long> IdConverter = 
      new(id => id.Value, id => new DbProjectId(id));
}
