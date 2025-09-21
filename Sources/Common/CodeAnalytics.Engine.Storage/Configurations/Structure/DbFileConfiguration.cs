using CodeAnalytics.Engine.Storage.Models.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Structure;

public sealed class DbFileConfiguration : IEntityTypeConfiguration<DbFile>
{
   public void Configure(EntityTypeBuilder<DbFile> builder)
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

      builder.HasMany(x => x.Projects)
         .WithMany(x => x.Files);
   }

   public static readonly ValueConverter<DbFileId, long> IdConverter = 
      new(id => id.Value, id => new DbFileId(id));
}
