using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAnalytics.Engine.Storage.Configurations.Symbols.Common;

public sealed class DbSymbolConfiguration : IEntityTypeConfiguration<DbSymbol>
{
   public void Configure(EntityTypeBuilder<DbSymbol> builder)
   {
      builder.HasKey(x => x.Id);
      
      builder.Property(x => x.Id)
         .HasConversion(IdConverter)
         .ValueGeneratedOnAdd();

      builder.HasIndex(x => x.UniqueId)
         .IsUnique();
      builder.Property(x => x.UniqueId)
         .HasMaxLength(3_000);

      builder.HasIndex(x => x.UniqueIdHash)
         .IsUnique();
      builder.Property(x => x.UniqueIdHash)
         .HasMaxLength(500);

      builder.Property(x => x.Name)
         .HasMaxLength(2_000);

      builder.Property(x => x.MetadataName)
         .HasMaxLength(2_000);
      
      builder.Property(x => x.FullPathName)
         .HasMaxLength(3_000);
      
      builder.Property(x => x.Language)
         .HasMaxLength(60);
      
      
   }

   public static readonly ValueConverter<DbSymbolId, long> IdConverter = 
      new(id => id.Value, id => new DbSymbolId(id));
}
