using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Structure;

public sealed class DbSolutionConfiguration : IEntityTypeConfiguration<DbSolution>
{
   public void Configure(EntityTypeBuilder<DbSolution> builder)
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
   }

   public static readonly ValueConverter<DbSolutionId, long> IdConverter = 
      new(id => id.Value, id => new DbSolutionId(id));
}